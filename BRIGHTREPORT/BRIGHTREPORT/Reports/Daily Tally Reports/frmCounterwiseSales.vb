Imports System.Data.OleDb

Public Class frmCounterwiseSales
    Dim dt As DataTable
    Dim cmd As OleDbCommand
    Dim StrSql As String
    Dim StrFilter As String
    Dim StrGrsNet As String

    Dim dtCostCentre As New DataTable
    Dim dtCashCounter As New DataTable
    Dim dtItemCounter As New DataTable
    Dim SelectedCompany As String
    Dim Hide_RepairDetails As Boolean = IIf(GetAdmindbSoftValue("RPT_COUNTERWISESALES_HIDE_REPDET", "N") = "Y", True, False)

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub

    Private Sub frmCounterwiseSales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{tab}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabmain.SelectedTab.Name = tabView.Name Then
            If tabmain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub frmCounterwiseSales_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        grbInput.Location = New Point((ScreenWid - grbInput.Width) / 2, ((ScreenHit - 128) - grbInput.Height) / 2)
        LoadCompany(chkLstCompany)
        ProcAddCostCentre()
        ProcAddCashCounter()
        ProcAddNodeId()
        ProcAddItemCounter()
        tabmain.ItemSize = New System.Drawing.Size(1, 1)
        tabmain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        tabmain.SelectedTab = tabGen
        ''Hallmark
        cmbHallmarkFilter.Items.Clear()
        cmbHallmarkFilter.Items.Add("BOTH")
        cmbHallmarkFilter.Items.Add("WITH HALLMARK")
        cmbHallmarkFilter.Items.Add("WITHOUT HALLMARK")
        cmbHallmarkFilter.Text = "BOTH"
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ProcAddCostCentre()
        StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        Dim STRCC As String = UCase(objGPack.GetSqlValue(StrSql, , "N"))
        If STRCC = "Y" Then
            StrSql = "SELECT COSTID, COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTID "
            da = New OleDbDataAdapter(StrSql, cn)
            dtCostCentre = New DataTable
            da.Fill(dtCostCentre)
            If dtCostCentre.Rows.Count > 0 Then
                chkLstCostCentre.Items.Add("ALL", IIf(cnDefaultCostId, True, False))
                'For cnt As Integer = 0 To dtCostCentre.Rows.Count - 1
                '    chkLstCostCentre.Items.Add(dtCostCentre.Rows(cnt).Item(1).ToString)
                'Next
                For i As Integer = 0 To dtCostCentre.Rows.Count - 1
                    If cnCostName = dtCostCentre.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                        chkLstCostCentre.Items.Add(dtCostCentre.Rows(i).Item("COSTNAME").ToString, True)
                    Else
                        chkLstCostCentre.Items.Add(dtCostCentre.Rows(i).Item("COSTNAME").ToString, False)
                    End If
                Next
                If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
            Else
                chkLstCostCentre.Items.Clear()
                chkLstCostCentre.Enabled = False
            End If
        Else
            chkLstCostCentre.Items.Clear()
            chkLstCostCentre.Enabled = False
        End If
    End Sub

    Private Sub ProcAddCashCounter()
        StrSql = "SELECT CASHID, CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER ORDER BY CASHID"
        da = New OleDbDataAdapter(StrSql, cn)
        dtCashCounter = New DataTable
        da.Fill(dtCashCounter)
        If dtCashCounter.Rows.Count > 0 Then
            chkLstCashCounter.Items.Add("ALL", True)
            For cnt As Integer = 0 To dtCashCounter.Rows.Count - 1
                chkLstCashCounter.Items.Add(dtCashCounter.Rows(cnt).Item(1).ToString)
            Next
        Else
            chkLstCashCounter.Items.Clear()
            chkLstCashCounter.Enabled = False
        End If
    End Sub

    Private Sub ProcAddNodeId()
        StrSql = "SELECT DISTINCT SYSTEMID FROM ( "
        StrSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ISSUE UNION ALL "
        StrSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..RECEIPT  UNION ALL "
        StrSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN )X "
        StrSql += " ORDER BY SYSTEMID "
        da = New OleDbDataAdapter(StrSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            chkLstNodeId.Items.Add("ALL", True)
            For cnt As Integer = 0 To dt.Rows.Count - 1
                chkLstNodeId.Items.Add(dt.Rows(cnt).Item(0).ToString)
            Next
        Else
            chkLstNodeId.Items.Clear()
            chkLstNodeId.Enabled = False
        End If
    End Sub

    Private Sub ProcAddItemCounter()
        StrSql = "SELECT ITEMCTRID, ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRID"
        da = New OleDbDataAdapter(StrSql, cn)
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

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, e)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        'rbtGrswt.Checked = True
        'chkWithApp.Checked = False
        lbltitle.Text = ""
        gridView.DataSource = Nothing
        Prop_Gets()
        dtpFrom.Select()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        If tabmain.SelectedTab.Name = tabGen.Name Then
            btnExit_Click(Me, e)
        End If
    End Sub

    Private Sub BtnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        btnView_Search.Enabled = False
        ''gridView.DataSource = Nothing
        SelectedCompany = GetSelectedCompanyId(chkLstCompany, True)

        StrGrsNet = IIf(rbtGrswt.Checked = True, "GRSWT", "NETWT")

        If chkLstCostCentre.Enabled = True Then
            If Not chkLstCostCentre.CheckedItems.Count > 0 Then chkLstCostCentre.SetItemChecked(0, True)
        End If
        If chkLstCashCounter.Items.Count > 0 Then
            If Not chkLstCashCounter.CheckedItems.Count > 0 Then chkLstCashCounter.SetItemChecked(0, True)
        End If
        If chkLstNodeId.Items.Count > 0 Then
            If Not chkLstNodeId.CheckedItems.Count > 0 Then chkLstNodeId.SetItemChecked(0, True)
        End If
        If chkLstItemCounter.Items.Count > 0 Then
            If Not chkLstItemCounter.CheckedItems.Count > 0 Then chkLstItemCounter.SetItemChecked(0, True)
        End If

        Filteration()
        If rbtSummary.Checked Then
            ReportSummary()
        ElseIf rbtdetailed.Checked Then
            ReportDetailed()
        End If
        btnView_Search.Enabled = True
        If gridView.Rows.Count > 0 Then Title()
        If gridView.Rows.Count > 0 Then tabmain.SelectedTab = tabView
        Prop_Sets()
    End Sub

    Private Sub Filteration()
        Dim tempchkitem As String = Nothing
        StrFilter = ""
        tempchkitem = ""

        ''COSTCENTRE
        If chkLstCostCentre.Enabled = True Then
            If chkLstCostCentre.Items.Count > 1 And chkLstCostCentre.CheckedItems.Count > 0 And chkLstCostCentre.GetItemChecked(0) <> True Then
                For CNT As Integer = 1 To chkLstCostCentre.Items.Count - 1
                    If chkLstCostCentre.GetItemChecked(CNT) = True Then
                        tempchkitem += " '" + dtCostCentre.Rows(CNT - 1).Item(0) + "',"
                    End If
                Next
                If Len(tempchkitem) > 1 Then tempchkitem = Mid(Trim(tempchkitem), 1, Len(Trim(tempchkitem)) - 1)
            Else
                tempchkitem = ""
            End If

            If tempchkitem <> "" Then
                StrFilter = " AND COSTID IN (" & tempchkitem & ")"
            Else
                StrFilter = ""
            End If
        End If
        ''CASH COUNTER
        tempchkitem = ""
        If chkLstCashCounter.Items.Count > 0 Then
            If chkLstCashCounter.Items.Count > 0 And chkLstCashCounter.CheckedItems.Count > 0 And chkLstCashCounter.GetItemChecked(0) <> True Then
                For CNT As Integer = 1 To chkLstCashCounter.Items.Count - 1
                    If chkLstCashCounter.GetItemChecked(CNT) = True Then
                        tempchkitem += " '" & dtCashCounter.Rows(CNT - 1).Item(0) + "',"
                    End If
                Next
                If Len(tempchkitem) > 1 Then tempchkitem = Mid(Trim(tempchkitem), 1, Len(Trim(tempchkitem)) - 1)
            Else
                tempchkitem = ""
            End If
        End If
        If tempchkitem <> "" Then StrFilter += " AND CASHID IN (" & tempchkitem & ") "



        ''NODE ID
        tempchkitem = ""
        If chkLstNodeId.Items.Count > 0 Then
            If chkLstNodeId.Items.Count > 0 And chkLstNodeId.GetItemChecked(0) <> True Then
                For CNT As Integer = 1 To chkLstNodeId.Items.Count - 1
                    If chkLstNodeId.GetItemChecked(CNT) = True Then
                        tempchkitem = tempchkitem & " '" & chkLstNodeId.Items.Item(CNT) & "'" & ", "
                    End If
                Next
                If Len(tempchkitem) > 1 Then tempchkitem = Mid(Trim(tempchkitem), 1, Len(Trim(tempchkitem)) - 1)
            Else
                tempchkitem = ""
            End If
        End If
        If tempchkitem <> "" Then
            StrFilter += " AND SYSTEMID IN (" & tempchkitem & ")"
        End If

        'ITEM COUNTER
        tempchkitem = ""
        If chkLstItemCounter.Items.Count > 0 Then
            If chkLstItemCounter.Items.Count > 0 And chkLstCashCounter.CheckedItems.Count > 0 And chkLstItemCounter.GetItemChecked(0) <> True Then
                For CNT As Integer = 1 To chkLstItemCounter.Items.Count - 1
                    If chkLstItemCounter.GetItemChecked(CNT) = True Then
                        tempchkitem += " '" + dtItemCounter.Rows(CNT - 1).Item(0).ToString + "',"
                    End If
                Next
                If Len(tempchkitem) > 1 Then tempchkitem = Mid(Trim(tempchkitem), 1, Len(Trim(tempchkitem)) - 1)
            Else
                tempchkitem = ""
            End If
        End If
        If tempchkitem <> "" Then StrFilter += " AND ITEMCTRID IN (" & tempchkitem & ") "

        If Not chkWithApp.Checked Then StrFilter += " AND I.TRANTYPE NOT IN ('AI') "

    End Sub

    Private Sub ReportSummary()
        Try
            'Me.Cursor = Cursors.WaitCursor
            StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' "
            StrSql += " AND NAME = 'TEMP" & systemId & "COUNTERWISESALES')"
            StrSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "COUNTERWISESALES"
            StrSql += " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "COUNTERWISESALES("
            StrSql += " COUNTERNAME VARCHAR(50),"
            StrSql += " SALPCS INT,"
            StrSql += " TAGWT NUMERIC(12,3),"
            StrSql += " SALWT NUMERIC(12,3),"
            StrSql += " PARTLYDIFF NUMERIC(12,3),"
            StrSql += " MISCWT NUMERIC(12,3),"
            StrSql += " HOMESALES NUMERIC(12,3),"
            StrSql += " DIAWT NUMERIC(12,3),"
            StrSql += " TOTSALES NUMERIC(12,3),"
            StrSql += " AMOUNT NUMERIC(15,2),"
            StrSql += " COLHEAD VARCHAR(1),"
            StrSql += " SNO INT IDENTITY(1,1))"
            cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            'Me.Cursor = Cursors.WaitCursor
            StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' "
            StrSql += " AND NAME = 'TEMP" & systemId & "CSALES')"
            StrSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "CSALES"
            StrSql += " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "CSALES("
            StrSql += " COUNTERNAME VARCHAR(30),"
            StrSql += " PARTICULAR VARCHAR(50),"
            StrSql += " METALNAME VARCHAR(20),"
            StrSql += " SALPCS INT,"
            StrSql += " TAGWT NUMERIC(12,3),"
            StrSql += " SALWT NUMERIC(12,3),"
            StrSql += " PARTLYDIFF NUMERIC(12,3),"
            StrSql += " MISCWT NUMERIC(12,3),"
            StrSql += " HOMESALES NUMERIC(12,3),"
            StrSql += " DIAWT NUMERIC(12,3),"
            StrSql += " TOTSALES NUMERIC(12,3),"
            StrSql += " AMOUNT NUMERIC(15,2),"
            StrSql += " RESULT INT,"
            StrSql += " COLHEAD VARCHAR(1),"
            StrSql += " SNO INT IDENTITY(1,1))"
            cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            ProcSummaryQry()

            StrSql = "SELECT COUNTERNAME, TAGWT, SALPCS, SALWT, PARTLYDIFF, MISCWT, HOMESALES, "
            StrSql += " DIAWT, TOTSALES, AMOUNT, COLHEAD "
            StrSql += " FROM TEMPTABLEDB..TEMP" & systemId & "COUNTERWISESALES ORDER BY SNO "
            dt = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dt)
            If dt.Rows.Count < 1 Then
                MsgBox("NO RECORDS FOUND", MsgBoxStyle.Information)
                btnNew_Click(Me, New EventArgs)
                Exit Sub
            End If
            With gridView
                ''.DataSource = Nothing
                .DataSource = dt
                tabView.Show()
                GridViewFormat()
                .Columns("COLHEAD").Visible = False
                GridCellAlignment()
                GridColwidthFixing()
                .Focus()
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
            MsgBox(ex.StackTrace, MsgBoxStyle.Information)
        Finally
            Me.Cursor = Cursors.Arrow
        End Try
    End Sub
    Private Sub ProcSummaryQry()
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += " NAME = 'TEMP" & systemId & "SUMMQRY')"
        StrSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "SUMMQRY"
        StrSql += " SELECT FLAG,TRANTYPE,"
        StrSql += " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST   "
        StrSql += " WHERE METALID = I.METALID) METALNAME,  "
        StrSql += " ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER   "
        StrSql += " WHERE ITEMCTRID = I.ITEMCTRID),'') AS COUNTERNAME,  "
        StrSql += " ITEMID, "
        StrSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST  "
        StrSql += " WHERE ITEMID = I.ITEMID) ITEMNAME, "
        StrSql += " SUM(TAGPCS) TAGPCS , SUM(TAGGRSWT) TAGWT,"
        StrSql += " SUM(PCS) AS SALPCS,  SUM(" & StrGrsNet & ") AS SALWT, "
        StrSql += " (SELECT SUM(STNWT) FROM  " & cnStockDb & "..ISSSTONE S  "
        StrSql += " WHERE   ISSSNO IN (SELECT SNO FROM   " & cnStockDb & "..ISSUE    "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'   "
        StrSql += " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND ISNULL(CANCEL,'') = ''   "
        StrSql += " AND COMPANYID IN (" & SelectedCompany & ")"
        StrSql += " AND ITEMID = I.ITEMID AND I.SNO = S.ISSSNO)  "
        StrSql += " AND COMPANYID IN (" & SelectedCompany & ")"
        StrSql += " AND CATCODE =  (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY   "
        StrSql += " WHERE METALID = 'D' AND S.CATCODE = CATCODE) ) AS DIAWT,  "
        StrSql += " (CASE WHEN SUM(TAGPCS) >0 THEN (SUM(TAGPCS)-SUM(PCS)) "
        StrSql += " ELSE 0 END)PARTLYPCS, "
        StrSql += " (CASE WHEN SUM(TAGGRSWT)>0 THEN (SUM(TAGGRSWT)-SUM(" & StrGrsNet & "))  "
        StrSql += " ELSE 0 END)PARTLYDIFF, "
        StrSql += " SUM(AMOUNT) AMOUNT, "
        StrSql += " 1 RESULT,CONVERT(VARCHAR(50),NULL) PARTICULAR, ' ' COLHEAD "
        StrSql += " INTO TEMPTABLEDB..TEMP" & systemId & "SUMMQRY "
        StrSql += " FROM " & cnStockDb & "..ISSUE I   "
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'  "
        StrSql += " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''   "
        StrSql += " AND COMPANYID IN (" & SelectedCompany & ")"
        StrSql += StrFilter
        If Hide_RepairDetails Then
            StrSql += " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ODBATCHNO = I.BATCHNO and ORTYPE ='R')"
        End If
        StrSql += " GROUP BY I.ITEMCTRID,I.ITEMID,METALID,I.SNO,FLAG,TRANTYPE"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "SUMMQRY)>0 "
        StrSql += " BEGIN"
        StrSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "CSALES(METALNAME,COUNTERNAME,"
        StrSql += " TAGWT, SALPCS, SALWT, PARTLYDIFF, MISCWT, HOMESALES, "
        StrSql += " DIAWT, AMOUNT, RESULT )"
        StrSql += " SELECT  METALNAME,COUNTERNAME,SUM(TAGWT) TAGWT,"
        StrSql += " (SELECT SUM(SALPCS) FROM TEMPTABLEDB..TEMP" & systemId & "SUMMQRY WHERE TRANTYPE = 'SA' "
        StrSql += " AND METALNAME = T.METALNAME AND COUNTERNAME = T.COUNTERNAME "
        StrSql += " /*AND FLAG NOT IN ('C','B')*/) SALPCS, "
        StrSql += " (SELECT SUM(SALWT) FROM TEMPTABLEDB..TEMP" & systemId & "SUMMQRY WHERE TRANTYPE = 'SA' "
        StrSql += " AND METALNAME = T.METALNAME AND COUNTERNAME = T.COUNTERNAME "
        StrSql += " AND FLAG NOT IN ('C','B')) SALWT, "
        StrSql += " SUM(PARTLYDIFF) PARTLYDIFF, "
        StrSql += " (SELECT SUM(SALWT) FROM TEMPTABLEDB..TEMP" & systemId & "SUMMQRY WHERE TRANTYPE = 'MI' "
        StrSql += " AND METALNAME = T.METALNAME AND COUNTERNAME = T.COUNTERNAME) MISCWT, "
        StrSql += " (SELECT SUM(SALWT) FROM TEMPTABLEDB..TEMP" & systemId & "SUMMQRY WHERE TRANTYPE = 'SA' "
        StrSql += " AND FLAG IN ('C','B') AND METALNAME = T.METALNAME "
        StrSql += " AND COUNTERNAME = T.COUNTERNAME) HOMESALES, "
        StrSql += " (SELECT SUM(DIAWT) FROM TEMPTABLEDB..TEMP" & systemId & "SUMMQRY WHERE TRANTYPE = 'SA' "
        StrSql += " AND METALNAME = T.METALNAME AND COUNTERNAME = T.COUNTERNAME "
        StrSql += " AND FLAG NOT IN ('C','B')) DIAWT, "
        StrSql += " SUM(AMOUNT) AMOUNT, 1 "
        StrSql += " FROM TEMPTABLEDB..TEMP" & systemId & "SUMMQRY T "
        StrSql += " WHERE RESULT = 1 "
        StrSql += " GROUP BY METALNAME,COUNTERNAME"
        StrSql += " END"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "CSALES)>0 "
        StrSql += " BEGIN"
        StrSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "CSALES SET TOTSALES = ISNULL(SALWT,0)+ISNULL(HOMESALES,0) "
        StrSql += " END"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'TITLE
        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "CSALES)>0 "
        StrSql += " BEGIN"
        StrSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "CSALES(METALNAME, PARTICULAR, RESULT, COLHEAD) "
        StrSql += " SELECT DISTINCT METALNAME, METALNAME,  0, 'T' FROM "
        StrSql += " TEMPTABLEDB..TEMP" & systemId & "CSALES WHERE RESULT = 1 "
        StrSql += " END"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'GRANDTOTAL
        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "CSALES)>0 "
        StrSql += " BEGIN"
        StrSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "CSALES(METALNAME,COUNTERNAME,PARTICULAR, "
        StrSql += " TAGWT, SALPCS, SALWT, PARTLYDIFF, MISCWT, HOMESALES, DIAWT, TOTSALES, "
        StrSql += " AMOUNT, RESULT, COLHEAD) "
        StrSql += " SELECT 'Z', 'Z', 'GRANDTOTAL', "
        StrSql += " SUM(TAGWT) TAGWT, SUM(SALPCS) SALPCS, SUM(SALWT) SALWT, "
        StrSql += " SUM(PARTLYDIFF) PARTLYDIFF, SUM(MISCWT) MISCWT, SUM(HOMESALES) HOMESALES, "
        StrSql += " SUM(DIAWT) DIAWT, SUM(TOTSALES) TOTSALES, SUM(AMOUNT) AMOUNT, "
        StrSql += "  3, 'G' FROM "
        StrSql += " TEMPTABLEDB..TEMP" & systemId & "CSALES WHERE RESULT = 1 "
        StrSql += " END"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "CSALES)>0 "
        StrSql += " BEGIN"
        StrSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "CSALES SET PARTICULAR = ' "
        StrSql += " ' + COUNTERNAME WHERE RESULT = 1"
        StrSql += " END"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "CSALES)>0 "
        StrSql += " BEGIN"
        StrSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "COUNTERWISESALES(COUNTERNAME, "
        StrSql += " TAGWT, SALPCS, SALWT, PARTLYDIFF, MISCWT, HOMESALES, DIAWT, TOTSALES, "
        StrSql += " AMOUNT, COLHEAD) "
        StrSql += " SELECT (CASE WHEN RESULT = 0 THEN METALNAME ELSE PARTICULAR END) PARTICULAR, "
        StrSql += " CASE WHEN TAGWT=0 THEN NULL ELSE TAGWT END TAGWT, "
        StrSql += " CASE WHEN SALPCS=0 THEN NULL ELSE SALPCS END SALPCS, "
        StrSql += " CASE WHEN SALWT=0 THEN NULL ELSE SALWT END SALWT, "
        StrSql += " CASE WHEN PARTLYDIFF=0 THEN NULL ELSE PARTLYDIFF END PARTLYDIFF, "
        StrSql += " CASE WHEN MISCWT=0 THEN NULL ELSE MISCWT END MISCWT, "
        StrSql += " CASE WHEN HOMESALES=0 THEN NULL ELSE HOMESALES END HOMESALES, "
        StrSql += " CASE WHEN DIAWT=0 THEN NULL ELSE DIAWT END DIAWT, "
        StrSql += " CASE WHEN TOTSALES=0 THEN NULL ELSE TOTSALES END TOTSALES, "
        StrSql += " CASE WHEN AMOUNT=0 THEN NULL ELSE AMOUNT END AMOUNT, "
        StrSql += " COLHEAD "
        StrSql += " FROM TEMPTABLEDB..TEMP" & systemId & "CSALES ORDER BY METALNAME,COUNTERNAME,RESULT "
        StrSql += " END"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

    End Sub

    Private Sub GridCellAlignment()
        With gridView
            .Columns("COUNTERNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("TAGWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PARTLYDIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MISCWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("HOMESALES").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOTSALES").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub

    Private Sub GridColwidthFixing()
        With gridView
            If .Rows.Count > 0 Then
                .Columns("COUNTERNAME").Width = 200
                .Columns("SALPCS").Width = 60
                .Columns("TAGWT").Width = 100
                .Columns("SALWT").Width = 100
                .Columns("PARTLYDIFF").Width = 85
                .Columns("MISCWT").Width = 85
                .Columns("HOMESALES").Width = 85
                .Columns("DIAWT").Width = 70
                .Columns("TOTSALES").Width = 100
                .Columns("AMOUNT").Width = 120
            End If
        End With
    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    If Not gridView Is Nothing Then
    ''        If rbtSummary.Checked Then
    ''            Select Case gridView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''                Case "T"
    ''                    ''gridView.Rows(e.RowIndex).Cells("COUNTERNAME").Style.BackColor = reportHeadStyle.BackColor
    ''                    ''gridView.Rows(e.RowIndex).Cells("COUNTERNAME").Style.Font = reportHeadStyle.Font
    ''                    gridView.Rows(e.RowIndex).Cells(0).Style.BackColor = reportHeadStyle.BackColor
    ''                    gridView.Rows(e.RowIndex).Cells(0).Style.Font = reportHeadStyle.Font
    ''                Case "S"
    ''                    gridView.Rows(e.RowIndex).DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
    ''                    gridView.Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''                Case "G"
    ''                    gridView.Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''                    gridView.Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''            End Select
    ''        ElseIf rbtdetailed.Checked Then
    ''            Select Case gridView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''                Case "T"
    ''                    ''gridView.Rows(e.RowIndex).Cells("BILLNO").Style.BackColor = reportHeadStyle.BackColor
    ''                    ''gridView.Rows(e.RowIndex).Cells("BILLNO").Style.Font = reportHeadStyle.Font
    ''                    gridView.Rows(e.RowIndex).Cells(0).Style.BackColor = reportHeadStyle.BackColor
    ''                    gridView.Rows(e.RowIndex).Cells(0).Style.Font = reportHeadStyle.Font

    ''                Case "S"
    ''                    gridView.Rows(e.RowIndex).DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
    ''                    gridView.Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''                Case "G"
    ''                    gridView.Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''                    gridView.Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''            End Select
    ''        End If
    ''    End If
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                If Not gridView Is Nothing Then
                    If rbtSummary.Checked Then
                        Select Case .Cells("COLHEAD").Value.ToString
                            Case "T"
                                ''.Cells("COUNTERNAME").Style.BackColor = reportHeadStyle.BackColor
                                ''.Cells("COUNTERNAME").Style.Font = reportHeadStyle.Font
                                .Cells(0).Style.BackColor = reportHeadStyle.BackColor
                                .Cells(0).Style.Font = reportHeadStyle.Font
                            Case "S"
                                .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                                .DefaultCellStyle.Font = reportSubTotalStyle.Font
                            Case "G"
                                .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                                .DefaultCellStyle.Font = reportTotalStyle.Font
                        End Select
                    ElseIf rbtdetailed.Checked Then
                        Select Case .Cells("COLHEAD").Value.ToString
                            Case "T"
                                ''.Cells("BILLNO").Style.BackColor = reportHeadStyle.BackColor
                                ''.Cells("BILLNO").Style.Font = reportHeadStyle.Font
                                .Cells(0).Style.BackColor = reportHeadStyle.BackColor
                                .Cells(0).Style.Font = reportHeadStyle.Font

                            Case "S"
                                .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                                .DefaultCellStyle.Font = reportSubTotalStyle.Font
                            Case "G"
                                .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                                .DefaultCellStyle.Font = reportTotalStyle.Font
                        End Select
                    End If
                End If
            End With
        Next
    End Function
    Private Sub ReportDetailed()
        Try
            'Me.Cursor = Cursors.WaitCursor
            StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' "
            StrSql += " AND NAME = 'TEMP" & systemId & "COUNTERWISESALES')"
            StrSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "COUNTERWISESALES"
            StrSql += " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "COUNTERWISESALES("
            StrSql += " BILLNO VARCHAR(50),"
            StrSql += " BILLDATE SMALLDATETIME,"
            StrSql += " ITEMID INT,"
            StrSql += " ITEMNAME VARCHAR(50),"
            StrSql += " SUBITEMNAME VARCHAR(50),"
            StrSql += " TAGNO VARCHAR(20),"
            StrSql += " HM_BILLNO VARCHAR(100),"
            StrSql += " TAGPCS INT,"
            StrSql += " TAGWT NUMERIC(12,3),"
            StrSql += " SALPCS INT,"
            StrSql += " SALGRSWT NUMERIC(12,3),"
            StrSql += " SALNETWT NUMERIC(12,3),"
            StrSql += " DIAWT NUMERIC(12,3),"
            StrSql += " PARTLYPCS INT,"
            StrSql += " PARTLYWT NUMERIC(12,3),"
            StrSql += " MC NUMERIC(12,2),"
            StrSql += " DISCOUNT NUMERIC(12,2),"
            StrSql += " DISCAUTPERSON VARCHAR(30),"
            StrSql += " SALESMAN VARCHAR(30),"
            StrSql += " USERNAME VARCHAR(30),"
            StrSql += " SYSTEMID VARCHAR(3),"
            StrSql += " REMARKS VARCHAR(50),"
            StrSql += " COLHEAD VARCHAR(1),"
            StrSql += " SNO INT IDENTITY(1,1))"
            cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            ProcDetailQry()

            StrSql = "SELECT BILLNO, BILLDATE, ITEMNAME,SUBITEMNAME, TAGNO,HM_BILLNO, TAGPCS, TAGWT, SALPCS, SALGRSWT, "
            StrSql += " SALNETWT, DIAWT, PARTLYPCS, PARTLYWT,MC,DISCOUNT,DISCAUTPERSON,9 SALESMAN, USERNAME, SYSTEMID, REMARKS, COLHEAD"
            StrSql += " FROM TEMPTABLEDB..TEMP" & systemId & "COUNTERWISESALES ORDER BY SNO "
            dt = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dt)
            If dt.Rows.Count < 1 Then
                MsgBox("NO RECORDS FOUND", MsgBoxStyle.Information)
                btnNew_Click(Me, New EventArgs)
                Exit Sub
            End If
            With gridView
                ''.DataSource = Nothing
                .DataSource = dt
                tabView.Show()
                GridViewFormat()
                .Columns("COLHEAD").Visible = False
                DetailedGridCellAlignment()
                DetailedGridColwidthFixing()
                DetailedGridColHeading()
                .Focus()
                If ChkSubItem.Checked = True Then
                    .Columns("SUBITEMNAME").Visible = True
                Else
                    .Columns("SUBITEMNAME").Visible = False
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
            MsgBox(ex.StackTrace, MsgBoxStyle.Information)
        Finally
            Me.Cursor = Cursors.Arrow
        End Try
    End Sub

    Private Sub ProcDetailQry()
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += " NAME = 'TEMP" & systemId & "DETQRY')"
        StrSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DETQRY"
        StrSql += " SELECT (SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST "
        StrSql += " WHERE ITEMID = I.ITEMID) STOCKTYPE, "
        StrSql += " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST   "
        StrSql += " WHERE METALID = I.METALID) METALNAME,  "
        StrSql += " ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER   "
        StrSql += " WHERE ITEMCTRID = I.ITEMCTRID),'') AS COUNTERNAME,  "
        StrSql += " TRANNO BILLNO, TRANDATE BILLDATE ,ITEMID, "
        StrSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST  "
        StrSql += " WHERE ITEMID = I.ITEMID) ITEMNAME, "
        StrSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST  "
        StrSql += " WHERE SUBITEMID = I.SUBITEMID) SUBITEMNAME, "
        StrSql += " TAGNO,ISNULL((STUFF((SELECT CAST(',' + H.[HM_BILLNO] AS VARCHAR(MAX)) FROM " & cnStockDb & "..ISSHALLMARK H WHERE (H.ISSSNO=I.SNO) FOR XML PATH ('')), 1, 1, '')),'') HM_BILLNO ,SUM(TAGPCS) TAGPCS , SUM(TAGGRSWT) TAGWT,"
        StrSql += " SUM(PCS) AS SALPCS,  SUM(GRSWT) AS SALGRSWT, SUM(NETWT) AS SALNETWT,  "
        StrSql += " (SELECT SUM(STNWT) FROM  " & cnStockDb & "..ISSSTONE S  "
        StrSql += " WHERE   ISSSNO IN (SELECT SNO FROM   " & cnStockDb & "..ISSUE    "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'   "
        StrSql += " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND ISNULL(CANCEL,'') = ''   "
        StrSql += " AND COMPANYID IN (" & SelectedCompany & ")"
        StrSql += " AND ITEMID = I.ITEMID AND I.SNO = S.ISSSNO)  "
        StrSql += " AND COMPANYID IN (" & SelectedCompany & ")"
        StrSql += " AND CATCODE =  (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY   "
        StrSql += " WHERE METALID = 'D' AND S.CATCODE = CATCODE) ) AS DIAWT,  "
        StrSql += " (CASE WHEN SUM(TAGPCS) >0 THEN (SUM(TAGPCS)-SUM(PCS)) "
        StrSql += " ELSE 0 END)PARTLYPCS, "
        StrSql += " (CASE WHEN SUM(TAGGRSWT)>0 THEN (SUM(TAGGRSWT)-SUM(GRSWT))  "
        StrSql += " ELSE 0 END)PARTLYWT, "
        StrSql += " SUM(I.MCHARGE)MC,CASE WHEN  (SUM(I.DISCOUNT)+SUM(I.FIN_DISCOUNT))>0 THEN (SUM(I.DISCOUNT)+SUM(I.FIN_DISCOUNT)) ELSE 0 END AS DISCOUNT,"
        StrSql += " (SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID=I.DISC_EMPID) AS DISCAUTPERSON,"
        StrSql += " (SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER "
        StrSql += " WHERE EMPID = I.EMPID) SALESMAN, "
        StrSql += " (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER "
        StrSql += " WHERE USERID = I.USERID) USERNAME, "
        StrSql += " I.SYSTEMID,1 RESULT,CONVERT(VARCHAR(50),NULL) PARTICULAR, ' ' COLHEAD "
        StrSql += " INTO TEMPTABLEDB..TEMP" & systemId & "DETQRY "
        StrSql += " FROM " & cnStockDb & "..ISSUE I   "
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'  "
        StrSql += " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''   "
        StrSql += " AND COMPANYID IN (" & SelectedCompany & ")"
        StrSql += StrFilter
        If Hide_RepairDetails Then
            StrSql += " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ODBATCHNO = I.BATCHNO and ORTYPE ='R')"
        End If
        If cmbHallmarkFilter.Text <> "BOTH" Then
            If cmbHallmarkFilter.Text = "WITH HALLMARK" Then
                StrSql += " AND  EXISTS (SELECT TOP 1 1 FROM " & cnStockDb & "..ISSHALLMARK H WHERE H.ISSSNO=I.SNO)  "
            ElseIf cmbHallmarkFilter.Text = "WITHOUT HALLMARK" Then
                StrSql += " AND  NOT EXISTS (SELECT TOP 1 1 FROM " & cnStockDb & "..ISSHALLMARK H WHERE H.ISSSNO=I.SNO)  "
            End If
        End If
        StrSql += ""
        StrSql += " GROUP BY I.ITEMCTRID,I.TAGNO,TRANNO,TRANDATE,I.ITEMID,METALID"
        StrSql += " ,I.SUBITEMID"
        StrSql += " ,EMPID,SYSTEMID,I.SNO,USERID ,I.DISC_EMPID"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        'If cmbHallmarkFilter.Text <> "BOTH" Then
        '    If cmbHallmarkFilter.Text = "WITH HALLMARK" Then
        '        StrSql = " DELETE TEMPTABLEDB..TEMP" & systemId & "DETQRY WHERE ISNULL(HM_BILLNO,'') = ''  "
        '    ElseIf cmbHallmarkFilter.Text = "WITHOUT HALLMARK" Then
        '        StrSql = " DELETE TEMPTABLEDB..TEMP" & systemId & "DETQRY WHERE ISNULL(HM_BILLNO,'') <> ''  "
        '    End If
        '    cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.ExecuteNonQuery()
        'End If

        'TITLE
        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETQRY)>0 "
        StrSql += " BEGIN"
        StrSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETQRY(METALNAME, COUNTERNAME, PARTICULAR,HM_BILLNO, "
        StrSql += " RESULT, COLHEAD) "
        StrSql += " SELECT DISTINCT METALNAME, COUNTERNAME, COUNTERNAME,'', 0, 'T' FROM "
        StrSql += " TEMPTABLEDB..TEMP" & systemId & "DETQRY WHERE RESULT = 1 "
        StrSql += " END"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'SUBTOTAL
        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETQRY)>0 "
        StrSql += " BEGIN"
        StrSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETQRY(METALNAME,COUNTERNAME,PARTICULAR,HM_BILLNO, "
        StrSql += " TAGPCS, TAGWT, SALPCS, SALGRSWT, SALNETWT, DIAWT, PARTLYPCS, PARTLYWT,MC,DISCOUNT, RESULT, COLHEAD)"
        StrSql += " SELECT METALNAME, COUNTERNAME, COUNTERNAME+' TOTAL','', SUM(TAGPCS) TAGPCS, "
        StrSql += " SUM(TAGWT) TAGWT, SUM(SALPCS) SALPCS, "
        StrSql += " SUM(SALGRSWT) SALGRSWT, SUM(SALNETWT) SALNETWT, SUM(DIAWT) DIAWT, "
        StrSql += " SUM(PARTLYPCS) PARTLYPCS, SUM(PARTLYWT) PARTLYWT,SUM(MC)MC,SUM(DISCOUNT)DISCOUNT, 2, 'S' FROM "
        StrSql += " TEMPTABLEDB..TEMP" & systemId & "DETQRY WHERE RESULT = 1 "
        StrSql += " GROUP BY METALNAME,COUNTERNAME "
        StrSql += " END"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'GRANDTOTAL
        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETQRY)>0 "
        StrSql += " BEGIN"
        StrSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETQRY(METALNAME,COUNTERNAME,PARTICULAR,HM_BILLNO, "
        StrSql += " TAGPCS, TAGWT, SALPCS, SALGRSWT, SALNETWT, DIAWT, PARTLYPCS, PARTLYWT,MC,DISCOUNT, RESULT, COLHEAD)"
        StrSql += " SELECT 'Z','Z','GRANDTOTAL','', SUM(TAGPCS) TAGPCS, "
        StrSql += " SUM(TAGWT) TAGWT, SUM(SALPCS) SALPCS, "
        StrSql += " SUM(SALGRSWT) SALGRSWT, SUM(SALNETWT) SALNETWT, SUM(DIAWT) DIAWT, "
        StrSql += " SUM(PARTLYPCS) PARTLYPCS, SUM(PARTLYWT) PARTLYWT ,SUM(MC)MC,SUM(DISCOUNT)DISCOUNT, 3, 'G' FROM "
        StrSql += " TEMPTABLEDB..TEMP" & systemId & "DETQRY WHERE RESULT = 1 "
        StrSql += " END"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETQRY)>0 "
        StrSql += " BEGIN"
        StrSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "DETQRY SET PARTICULAR = ' "
        StrSql += " ' + CONVERT(VARCHAR(10),BILLNO) WHERE RESULT = 1"
        StrSql += " END"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETQRY)>0 "
        StrSql += " BEGIN"
        StrSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "COUNTERWISESALES(BILLNO , "
        StrSql += " BILLDATE, ITEMID,ITEMNAME,SUBITEMNAME, TAGNO,HM_BILLNO, TAGPCS, TAGWT, SALPCS, SALGRSWT, SALNETWT, "
        StrSql += " DIAWT, PARTLYPCS, PARTLYWT,MC,DISCOUNT,DISCAUTPERSON, SALESMAN, USERNAME, SYSTEMID, COLHEAD) "
        StrSql += " SELECT (CASE WHEN RESULT = 0 THEN PARTICULAR+'-'+METALNAME ELSE PARTICULAR END) BILLNO, "
        StrSql += " BILLDATE, ITEMID, LEFT(ITEMNAME,25)+'('+CONVERT(VARCHAR(5),ITEMID)+')',SUBITEMNAME, TAGNO,HM_BILLNO, "
        StrSql += " CASE WHEN TAGPCS=0 THEN NULL ELSE TAGPCS END TAGPCS, "
        StrSql += " CASE WHEN TAGWT=0 THEN NULL ELSE TAGWT END TAGWT, "
        StrSql += " CASE WHEN SALPCS=0 THEN NULL ELSE SALPCS END SALPCS, "
        StrSql += " CASE WHEN SALGRSWT=0 THEN NULL ELSE SALGRSWT END SALGRSWT, "
        StrSql += " CASE WHEN SALNETWT=0 THEN NULL ELSE SALNETWT END SALNETWT, "
        StrSql += " CASE WHEN DIAWT=0 THEN NULL ELSE DIAWT END DIAWT, "
        StrSql += " CASE WHEN PARTLYPCS=0 THEN NULL ELSE PARTLYPCS END PARTLYPCS, "
        StrSql += " CASE WHEN PARTLYWT=0 THEN NULL ELSE PARTLYWT END PARTLYWT, "
        StrSql += " CASE WHEN MC=0 THEN NULL ELSE MC END MC, "
        StrSql += " CASE WHEN DISCOUNT=0 THEN NULL ELSE DISCOUNT END DISCOUNT, "
        StrSql += " DISCAUTPERSON,SALESMAN, USERNAME, SYSTEMID, COLHEAD"
        StrSql += " FROM TEMPTABLEDB..TEMP" & systemId & "DETQRY "
        StrSql += " ORDER BY METALNAME, COUNTERNAME, RESULT "
        StrSql += " END"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub DetailedGridCellAlignment()
        With gridView
            .Columns("BILLNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("BILLDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("ITEMNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("TAGNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAGPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAGWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PARTLYPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PARTLYWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DISCOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DISCAUTPERSON").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("SALESMAN").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("USERNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("SYSTEMID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Sub

    Private Sub DetailedGridColwidthFixing()
        With gridView
            If .Rows.Count > 0 Then
                .Columns("BILLNO").Width = 200
                .Columns("BILLDATE").Width = 80
                .Columns("ITEMNAME").Width = 110
                .Columns("TAGNO").Width = 70
                .Columns("TAGPCS").Width = 50
                .Columns("TAGWT").Width = 80
                .Columns("SALPCS").Width = 50
                .Columns("SALGRSWT").Width = 80
                .Columns("SALNETWT").Width = 80
                .Columns("DIAWT").Width = 70
                .Columns("PARTLYPCS").Width = 50
                .Columns("PARTLYWT").Width = 70
                .Columns("MC").Width = 100
                .Columns("DISCOUNT").Width = 100
                .Columns("SALESMAN").Width = 120
                .Columns("DISCAUTPERSON").Width = 120
                .Columns("SALESMAN").Width = 120
                .Columns("USERNAME").Width = 120
                .Columns("SYSTEMID").Width = 40
                .Columns("BILLDATE").DefaultCellStyle.Format = "yyyy-MM-dd"
            End If
        End With
    End Sub

    Private Sub DetailedGridColHeading()
        With gridView
            .Columns("BILLNO").HeaderText = "BILLNO"
            .Columns("BILLDATE").HeaderText = "BILLDATE"
            .Columns("ITEMNAME").HeaderText = "ITEMNAME"
            .Columns("TAGNO").HeaderText = "TAGNO"
            .Columns("TAGPCS").HeaderText = "TAG PCS"
            .Columns("TAGWT").HeaderText = "TAGWT"
            .Columns("SALPCS").HeaderText = "SAL PCS"
            .Columns("SALGRSWT").HeaderText = "SAL GRSWT"
            .Columns("SALNETWT").HeaderText = "SAL NETWT"
            .Columns("DIAWT").HeaderText = "DIAWT"
            .Columns("PARTLYPCS").HeaderText = "PARTLY PCS"
            .Columns("PARTLYWT").HeaderText = "PARTLY WT"
            .Columns("MC").HeaderText = "MCCHARGE"
            .Columns("DISCOUNT").HeaderText = "DISCOUNT"
            .Columns("DISCAUTPERSON").HeaderText = "DISCAUTPERSON"
            .Columns("SALESMAN").HeaderText = "SALESMAN"
            .Columns("USERNAME").HeaderText = "USERNAME"
            .Columns("SYSTEMID").HeaderText = "SYS ID"
        End With
    End Sub

    Private Sub rbtdetailed_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtdetailed.CheckedChanged
        If rbtdetailed.Checked Then
            rbtGrswt.Checked = True
            grbwt.Visible = False
            ChkSubItem.Enabled = True
        Else
            grbwt.Visible = True
            rbtNetwt.Enabled = True
            ChkSubItem.Checked = False
            ChkSubItem.Enabled = False
        End If
    End Sub

    Private Sub Title()
        Dim TITLE As String = Nothing
        TITLE = " COUNTERWISE SALES"
        If rbtSummary.Checked = True Then
            TITLE += " SUMMARY"
        Else
            TITLE += " DETAILED"
        End If
        TITLE += " REPORT FROM " & dtpFrom.Value.ToString("dd-MM-yyyy") & " TO "
        TITLE += dtpTo.Value.ToString("dd-MM-yyyy")
        Dim Cname As String = ""
        If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                If chkLstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chkLstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        End If
        lbltitle.Text = TITLE + Cname
        lbltitle.Font = New Font("VERDENA", 9, FontStyle.Bold)
        lbltitle.ForeColor = Color.Blue
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabmain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmCounterwiseSales_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        GetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId)
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        GetChecked_CheckedList(chkLstCashCounter, obj.p_chkLstCashCounter)
        GetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter)
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_rbtdetailed = rbtdetailed.Checked
        obj.p_rbtGrswt = rbtGrswt.Checked
        obj.p_rbtNetwt = rbtNetwt.Checked
        obj.p_chkWithApp = chkWithApp.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmCounterwiseSales_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmCounterwiseSales_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmCounterwiseSales_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        SetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId, Nothing)
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, Nothing)
        SetChecked_CheckedList(chkLstCashCounter, obj.p_chkLstCashCounter, Nothing)
        SetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter, Nothing)
        rbtSummary.Checked = obj.p_rbtSummary
        rbtdetailed.Checked = obj.p_rbtdetailed
        rbtGrswt.Checked = obj.p_rbtGrswt
        rbtNetwt.Checked = obj.p_rbtNetwt
        chkWithApp.Checked = obj.p_chkWithApp
    End Sub
End Class

Public Class frmCounterwiseSales_Properties
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
    Private chkLstNodeId As New List(Of String)
    Public Property p_chkLstNodeId() As List(Of String)
        Get
            Return chkLstNodeId
        End Get
        Set(ByVal value As List(Of String))
            chkLstNodeId = value
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
    Private chkLstCashCounter As New List(Of String)
    Public Property p_chkLstCashCounter() As List(Of String)
        Get
            Return chkLstCashCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstCashCounter = value
        End Set
    End Property
    Private chkLstItemCounter As New List(Of String)
    Public Property p_chkLstItemCounter() As List(Of String)
        Get
            Return chkLstItemCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstItemCounter = value
        End Set
    End Property
    Private rbtSummary As Boolean = False
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private rbtdetailed As Boolean = False
    Public Property p_rbtdetailed() As Boolean
        Get
            Return rbtdetailed
        End Get
        Set(ByVal value As Boolean)
            rbtdetailed = value
        End Set
    End Property
    Private rbtGrswt As Boolean = False
    Public Property p_rbtGrswt() As Boolean
        Get
            Return rbtGrswt
        End Get
        Set(ByVal value As Boolean)
            rbtGrswt = value
        End Set
    End Property
    Private rbtNetwt As Boolean = False
    Public Property p_rbtNetwt() As Boolean
        Get
            Return rbtNetwt
        End Get
        Set(ByVal value As Boolean)
            rbtNetwt = value
        End Set
    End Property
    Private chkWithApp As Boolean = False
    Public Property p_chkWithApp() As Boolean
        Get
            Return chkWithApp
        End Get
        Set(ByVal value As Boolean)
            chkWithApp = value
        End Set
    End Property
End Class