Imports System.Data.OleDb

Public Class frmDailyAbstract_old
    Dim Cmd As New OleDbCommand
    Dim DT As New DataTable
    Dim StrSql As String
    Dim StrFilter As String
    Dim StrCostFiltration As String
    Dim StrCashCounterFtr As String
    Dim dsReportCol As New DataSet
    Dim SelectedCompanyId As String
    Dim hasChit As Boolean = False
    Dim RPT_SEPVAT_DABS As Boolean = IIf(GetAdmindbSoftValue("RPT_SEPVAT_DABS", "Y") = "Y", True, False)
    Dim RPT_ME_SUM_DABS As Boolean = IIf(GetAdmindbSoftValue("RPT_ME_SUM_DABS", "N") = "Y", True, False)
    Dim ViewMode As Int16 = Val(GetAdmindbSoftValue("RPT_DABS", "2"))
    Dim dtMergeHeader As DataTable


    Private Sub frmSalesAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            BtnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridView.MultiSelect = True
        ProcAddCostCentre()
        ProcAddCashCounter()
        ProcAddNodeId()
        GridViewHead.DataSource = Nothing
        'Me.WindowState = FormWindowState.Maximized
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        grpFiltration.Location = New Point((ScreenWid - grpFiltration.Width) / 2, ((ScreenHit - 128) - grpFiltration.Height) / 2)
        LoadCompany(chkLstCompany)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ProcAddCostCentre()
        StrSql = "SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' AND CTLTEXT = 'Y'"
        da = New OleDbDataAdapter(StrSql, cn)
        DT = New DataTable
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            StrSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            DT = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(DT)
            If DT.Rows.Count > 0 Then
                chklstCostCentre.Items.Add("ALL", True)
                For cnt As Integer = 0 To DT.Rows.Count - 1
                    chklstCostCentre.Items.Add(DT.Rows(cnt).Item(0).ToString)
                Next
            Else
                chklstCostCentre.Items.Clear()
                chklstCostCentre.Enabled = False
            End If
        Else
            chklstCostCentre.Items.Clear()
            chklstCostCentre.Enabled = False
        End If
    End Sub

    Private Sub ProcAddCashCounter()
        StrSql = "SELECT CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER ORDER BY CASHID"
        DT = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            chkLstCashCounter.Items.Add("ALL", True)
            For CNT As Integer = 0 To DT.Rows.Count - 1
                chkLstCashCounter.Items.Add(DT.Rows(CNT).Item(0).ToString)
            Next
        Else
            chkLstCashCounter.Items.Clear()
            chkLstCashCounter.Enabled = False
        End If
    End Sub

    Private Sub ProcAddNodeId()
        StrSql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN ORDER BY SYSTEMID"
        DT = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            chkLstNodeId.Items.Add("ALL", True)
            For CNT As Integer = 0 To DT.Rows.Count - 1
                chkLstNodeId.Items.Add(DT.Rows(CNT).Item(0).ToString)
            Next
        Else
            chkLstNodeId.Items.Clear()
            chkLstNodeId.Enabled = False
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            Dim title As String
            title = "DAILY ABSTRACT REPORT"
            title += vbCrLf + Trim(lbltitle.Text)
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle.Text, gridView, BrightPosting.GExport.GExportType.Print, GridViewHead)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub BtnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle.Text, gridView, BrightPosting.GExport.GExportType.Export, GridViewHead)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        hasChit = IIf(GetAdmindbSoftValue("CHITDB", "N") = "Y", True, False) And chkChitInfo.Checked
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        dtMergeHeader = New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("COL1", GetType(String))
            .Columns.Add("COL2~COL3~COL4", GetType(String))
            .Columns.Add("COL5~COL6~COL7", GetType(String))
            .Columns.Add("COL8~COL9", GetType(String))
            .Columns.Add("COL10", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("COL1").Caption = ""
            .Columns("COL2~COL3~COL4").Caption = "ISSUE"
            .Columns("COL5~COL6~COL7").Caption = "RECEIPT"
            .Columns("COL8~COL9").Caption = "AMOUNT"
            .Columns("COL10").Caption = "AVERAGE"
            .Columns("SCROLL").Caption = ""
        End With
        SelectedCompanyId = GetSelectedCompanyId(chkLstCompany, True)
        GridViewHead.DataSource = dtMergeHeader
        Filteration()
        CostIdFiltration()
        If ViewMode = 2 Then
            Report(False)
            btnDotMatrix.Visible = False
        Else
            Report1(False)
            btnDotMatrix.Visible = True
        End If
        Prop_Sets()
    End Sub

    Private Sub Title()
        Dim TITLE As String = Nothing
        If rbtCategoryWise.Checked = True Then
            TITLE = "CATEGORYWISE "
        Else
            TITLE = "METALWISE "
        End If
        TITLE += " DAILY ABSTRACT REPORT FROM " & dtpFrom.Value.ToString("dd-MM-yyyy") & " TO "
        TITLE += dtpTo.Value.ToString("dd-MM-yyyy")
        lbltitle.Text = TITLE
    End Sub

    Private Sub ColwidthFixing()
        If gridView.Rows.Count > 0 Then
            gridView.Columns("COL1").Width = 247
            gridView.Columns("COL2").Width = 50
            gridView.Columns("COL3").Width = 100
            gridView.Columns("COL4").Width = 100
            gridView.Columns("COL5").Width = 50
            gridView.Columns("COL6").Width = 100
            gridView.Columns("COL7").Width = 100
            gridView.Columns("COL8").Width = 120
            gridView.Columns("COL9").Width = 120
        End If
    End Sub

    Private Sub ColwidthFixing1()
        If gridView.Rows.Count > 0 Then
            gridView.Columns("DESC").Width = 247
            gridView.Columns("IPCS").Width = 50
            gridView.Columns("IGWT").Width = 100
            gridView.Columns("INWT").Width = 100
            gridView.Columns("RPCS").Width = 50
            gridView.Columns("RGWT").Width = 100
            gridView.Columns("RNWT").Width = 100
            gridView.Columns("REC").Width = 120
            gridView.Columns("PAY").Width = 120
            gridView.Columns("AVERAGE").Width = 120
        End If
    End Sub

    Private Sub CounterFiltration()
        Dim tempchkitem As String = Nothing
        ''CASH COUNTER
        If chkLstCashCounter.Items.Count > 0 And chkLstCashCounter.GetItemChecked(0) <> True Then
            For CNT As Integer = 1 To chkLstCashCounter.Items.Count - 1
                If chkLstCashCounter.GetItemChecked(CNT) = True Then
                    tempchkitem = tempchkitem & " '" & chkLstCashCounter.Items.Item(CNT) & "'" & ", "
                End If
            Next
            If Len(tempchkitem) > 1 Then tempchkitem = Mid(Trim(tempchkitem), 1, Len(Trim(tempchkitem)) - 1)
        Else
            tempchkitem = ""
        End If

        If tempchkitem <> "" Then
            StrCashCounterFtr = " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN"
            StrCashCounterFtr += " (" & tempchkitem & "))"
        End If

        tempchkitem = ""
    End Sub


    Private Sub CostIdFiltration()
        Dim tempchkitem As String = Nothing
        StrCostFiltration = ""
        ''COSTCENTRE
        If chklstCostCentre.Enabled = True Then
            If chklstCostCentre.Items.Count > 0 And chklstCostCentre.GetItemChecked(0) <> True Then
                For CNT As Integer = 1 To chklstCostCentre.Items.Count - 1
                    If chklstCostCentre.GetItemChecked(CNT) = True Then
                        tempchkitem = tempchkitem & " '" & chklstCostCentre.Items.Item(CNT) & "'" & ", "
                    End If
                Next
                If Len(tempchkitem) > 1 Then tempchkitem = Mid(Trim(tempchkitem), 1, Len(Trim(tempchkitem)) - 1)
            Else
                tempchkitem = ""
            End If

            If tempchkitem <> "" Then
                StrCostFiltration = " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN"
                StrCostFiltration += " (" & tempchkitem & "))"
            Else
                StrCostFiltration = ""
            End If
        End If
        tempchkitem = ""
    End Sub

    Private Sub Filteration()
        Dim tempchkitem As String = Nothing
        StrFilter = ""
        ''COSTCENTRE
        If chklstCostCentre.Enabled = True Then
            If chklstCostCentre.Items.Count > 0 And chklstCostCentre.GetItemChecked(0) <> True Then
                For CNT As Integer = 1 To chklstCostCentre.Items.Count - 1
                    If chklstCostCentre.GetItemChecked(CNT) = True Then
                        tempchkitem = tempchkitem & " '" & chklstCostCentre.Items.Item(CNT) & "'" & ", "
                    End If
                Next
                If Len(tempchkitem) > 1 Then tempchkitem = Mid(Trim(tempchkitem), 1, Len(Trim(tempchkitem)) - 1)
            Else
                tempchkitem = ""
            End If

            If tempchkitem <> "" Then
                StrFilter = " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN"
                StrFilter += " (" & tempchkitem & "))"
            Else
                StrFilter = ""
            End If
        End If
        tempchkitem = ""

        ''CASH COUNTER
        If chkLstCashCounter.Items.Count > 0 And chkLstCashCounter.GetItemChecked(0) <> True Then
            For CNT As Integer = 1 To chkLstCashCounter.Items.Count - 1
                If chkLstCashCounter.GetItemChecked(CNT) = True Then
                    tempchkitem = tempchkitem & " '" & chkLstCashCounter.Items.Item(CNT) & "'" & ", "
                End If
            Next
            If Len(tempchkitem) > 1 Then tempchkitem = Mid(Trim(tempchkitem), 1, Len(Trim(tempchkitem)) - 1)
        Else
            tempchkitem = ""
        End If

        If tempchkitem <> "" Then
            StrFilter += " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN"
            StrFilter += " (" & tempchkitem & "))"
        End If

        tempchkitem = ""

        ''NODE ID

        If chkLstNodeId.Items.Count > 0 Then
            If chkLstNodeId.GetItemChecked(0) <> True Then
                For CNT As Integer = 1 To chkLstNodeId.Items.Count - 1
                    If chkLstNodeId.GetItemChecked(CNT) = True Then
                        tempchkitem = tempchkitem & " '" & chkLstNodeId.Items.Item(CNT) & "'" & ", "
                    End If
                Next
                If Len(tempchkitem) > 1 Then tempchkitem = Mid(Trim(tempchkitem), 1, Len(Trim(tempchkitem)) - 1)
            End If
        Else
            tempchkitem = ""
        End If

        If tempchkitem <> "" Then
            StrFilter += " AND SYSTEMID IN (" & tempchkitem & ")"
        End If
    End Sub

    Private Sub Report1(ByVal DotMatrix As Boolean)
        Dim clsDailyAbs As New clsDailyAbstract
        clsDailyAbs.RateAvg = chkAverage.Checked
        clsDailyAbs.CategoryShortName = chkCatShortname.Checked
        clsDailyAbs.HomeSales = chkHomeSale.Checked
        clsDailyAbs.MiscRecPaySum = chkMiscRecPaySummary.Checked
        clsDailyAbs.CancelBillDetail = chkCancelBills.Checked
        clsDailyAbs.ChitInfo = chkChitInfo.Checked
        clsDailyAbs.SeperateBeeds = chkSeperateBeeds.Checked
        clsDailyAbs.WithVat = chkWithVat.Checked
        clsDailyAbs.WithCashOpening = chkCashOpening.Checked

        clsDailyAbs.CategoryWise = rbtCategoryWise.Checked
        clsDailyAbs.dtpFrom = dtpFrom.Value.Date
        clsDailyAbs.dtpTo = dtpTo.Value.Date
        clsDailyAbs.RPT_SEPVAT_DABS = RPT_SEPVAT_DABS
        clsDailyAbs.SelectedCompanyId = SelectedCompanyId
        clsDailyAbs.strFilter = StrFilter
        clsDailyAbs.CalcRedcRate = Convert.ToDouble(IIf(txtSiPurRate_Amt.Text = "", "0", txtSiPurRate_Amt.Text))
        clsDailyAbs.DotMatrix = DotMatrix
        clsDailyAbs.funcCalc()
        If DotMatrix = True Then
            Exit Sub
        End If
        lbltitle.Text = clsDailyAbs.strTitle

        gridView.DataSource = clsDailyAbs.dtTableAdd
        GridViewHead.DataSource = dtMergeHeader

        GridCellAlignment1()
        gridView.Columns("COLHEAD").Visible = False
        gridView.Columns("RESULT1").Visible = False
        gridView.Columns("KEYNO").Visible = False
        gridView.Columns("SCROLL").Visible = False

        For cnt As Integer = 0 To gridView.ColumnCount - 1
            gridView.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            gridView.Columns(cnt).Resizable = DataGridViewTriState.True
        Next
        With gridView
            .Columns("DESC").HeaderText = "DESCRIPTION"
            .Columns("IPCS").HeaderText = "PCS"
            .Columns("IGWT").HeaderText = "GRSWT"
            .Columns("INWT").HeaderText = "NETWT"
            .Columns("RPCS").HeaderText = "PCS"
            .Columns("RGWT").HeaderText = "GRSWT"
            .Columns("RNWT").HeaderText = "NETWT"
            .Columns("REC").HeaderText = "RECEIPT"
            .Columns("PAY").HeaderText = "PAYMENT"
            .Columns("AVERAGE").HeaderText = "AVERAGE"

            .Columns("DESC").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("IPCS").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("IGWT").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("INWT").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("RPCS").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("RGWT").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("RNWT").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("REC").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("PAY").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("AVERAGE").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
        gridView.ColumnHeadersDefaultCellStyle = reportColumnHeadStyle
        GridHead1()
        ColwidthFixing1()
        tabMain.SelectedTab = tabView
        tabView.Show()
        FillGridGroupStyle_KeyNoWise(gridView, "DESC")
        gridView.Focus()
    End Sub


    Private Sub Report(ByVal DotMatrix As Boolean)
        Try
            'Me.Cursor = Cursors.WaitCursor
            StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = "
            StrSql += " 'TEMP" & systemId & "SALEABSTRACT') "
            StrSql += " DROP TABLE TEMP" & systemId & "SALEABSTRACT "
            StrSql += " CREATE TABLE TEMP" & systemId & "SALEABSTRACT( "
            StrSql += " COL1 VARCHAR(100),"
            StrSql += " COL2 VARCHAR(100),"
            StrSql += " COL3 VARCHAR(100),"
            StrSql += " COL4 VARCHAR(100),"
            StrSql += " COL5 VARCHAR(100),"
            StrSql += " COL6 VARCHAR(100),"
            StrSql += " COL7 VARCHAR(100),"
            StrSql += " COL8 VARCHAR(100),"
            StrSql += " COL9 VARCHAR(100),"
            StrSql += " COL10 VARCHAR(100),"
            StrSql += " COLHEAD VARCHAR(1),"
            StrSql += " SNO INT IDENTITY(1,1))"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = "
            StrSql += " 'TEMP" & systemId & "SASRPU') "
            StrSql += " DROP TABLE TEMP" & systemId & "SASRPU "
            StrSql += " CREATE TABLE TEMP" & systemId & "SASRPU( "
            StrSql += " DESCRIPTION VARCHAR(100),"
            StrSql += " ISSPCS INT,"
            StrSql += " ISSGRSWT NUMERIC(15,3),"
            StrSql += " ISSNETWT NUMERIC(15,3),"
            StrSql += " RECPCS INT,"
            StrSql += " RECGRSWT NUMERIC(15,3),"
            StrSql += " RECNETWT NUMERIC(15,3),"
            StrSql += " RECEIPT NUMERIC(20,2),"
            StrSql += " PAYMENT NUMERIC(20,2),"
            StrSql += " AVERAGE VARCHAR(50),"
            StrSql += " COLHEAD VARCHAR(1),"
            StrSql += " RESULT1 INT,"
            StrSql += " SNO INT IDENTITY(1,1))"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                .Columns.Add("DESCRIPTION", GetType(String))
                .Columns.Add("ISSPCS~ISSGRSWT~ISSNETWT", GetType(String))
                .Columns.Add("RECPCS~RECGRSWT~RECNETWT", GetType(String))
                .Columns.Add("RECEIPT~PAYMENT", GetType(String))
                .Columns("DESCRIPTION").Caption = "DESCRIPTION"
                .Columns("ISSPCS~ISSGRSWT~ISSNETWT").Caption = "ISSUE"
                .Columns("RECPCS~RECGRSWT~RECNETWT").Caption = "RECEIPT"
                .Columns("RECEIPT~PAYMENT").Caption = "AMOUNT"
            End With
            dsReportCol = New DataSet
            dsReportCol.Tables.Add(dtMergeHeader)
            If hasChit Then
                StrSql = " SELECT name cnt FROM MASTER.DBO.SYSDATABASES WHERE NAME = '" & cnChitTrandb & "'"
                If Not objGPack.DupCheck(StrSql) Then MsgBox("SCHEME Database Not Found", MsgBoxStyle.Information) : Exit Sub
            End If
            ProcSASRPU(DotMatrix)
            StrSql = "SELECT COL1,COL2,COL3,COL4,COL5, "
            StrSql += " COL6,COL7,COL8,COL9,COL10, "
            StrSql += " COLHEAD FROM TEMP" & systemId & "SALEABSTRACT ORDER BY SNO "
            DT = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(DT)
            If Not DT.Rows.Count > 0 Then
                gridView.DataSource = Nothing
                GridViewHead.DataSource = Nothing
                GridViewHead.Rows.Clear()
                GridViewHead.Columns.Clear()
                btnView_Search.Enabled = True
                lbltitle.Text = ""
                MsgBox("NO RECORDS TO VIEW")
                dtpFrom.Focus()
                Exit Sub
            End If
            gridView.DataSource = Nothing
            gridView.DataSource = DT
            tabView.Show()
            GridViewFormat()
            With gridView
                .Columns("COL1").HeaderText = "DESCRIPTION"
                .Columns("COL2").HeaderText = "PCS"
                .Columns("COL3").HeaderText = "GRSWT"
                .Columns("COL4").HeaderText = "NETWT"
                .Columns("COL5").HeaderText = "PCS"
                .Columns("COL6").HeaderText = "GRSWT"
                .Columns("COL7").HeaderText = "NETWT"
                .Columns("COL8").HeaderText = "RECEIPT"
                .Columns("COL9").HeaderText = "PAYMENT"
                .Columns("COL10").HeaderText = "AVERAGE"
                .Columns("COLHEAD").HeaderText = "HCOL"
            End With
            GridCellAlignment()
            gridView.Columns("COLHEAD").Visible = False
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                gridView.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                gridView.Columns(cnt).Resizable = DataGridViewTriState.True
            Next
            gridView.ColumnHeadersDefaultCellStyle = reportColumnHeadStyle
            GridHead()
            ColwidthFixing()
            Title()

            If chkAverage.Checked = False Then
                gridView.Columns("COL10").Visible = False
                GridViewHead.Columns("COL10").Visible = False
            Else
                GridViewHead.Columns("COL10").Visible = True
            End If
            If chkPcs.Checked = False Then
                gridView.Columns("COL2").Visible = False
                gridView.Columns("COL5").Visible = False
            Else
                gridView.Columns("COL2").Visible = True
                gridView.Columns("COL5").Visible = True
            End If
            If chkGrswt.Checked = False Then
                gridView.Columns("COL3").Visible = False
                gridView.Columns("COL6").Visible = False
            Else
                gridView.Columns("COL3").Visible = True
                gridView.Columns("COL6").Visible = True
            End If
            If chkNetwt.Checked = False Then
                gridView.Columns("COL4").Visible = False
                gridView.Columns("COL7").Visible = False
            Else
                gridView.Columns("COL4").Visible = True
                gridView.Columns("COL7").Visible = True
            End If
            tabMain.SelectedTab = tabView
            gridView.Focus()
        Catch e As Exception
            MsgBox(e.Message, MsgBoxStyle.Information)
            MsgBox(e.Message + vbCrLf + e.StackTrace)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ProcRepairDelivery()
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSREPAIR') DROP TABLE TEMP" & systemId & "ABSREPAIR"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT CATCODE, "
        Else
            StrSql += vbCrLf + " SELECT METALID AS CATCODE, "
        End If
        StrSql += vbCrLf + " CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + " PAYMENT, AVERAGE, RATE, COLHEAD, RESULT, RESULT1 INTO TEMP" & systemId & "ABSREPAIR FROM ("
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT I.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            End If
        Else
            StrSql += vbCrLf + " SELECT I.METALID, "
            StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS CATNAME, "
        End If
        StrSql += vbCrLf + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += vbCrLf + " SUM(AMOUNT) "
        StrSql += vbCrLf + " AS RECEIPT, "
        StrSql += vbCrLf + " 0 AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        StrSql += vbCrLf + " ELSE "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        StrSql += vbCrLf + " I.TRANTYPE IN('RD') AND ISNULL(I.CANCEL,'') = '' "
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY I.CATCODE,C.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY I.METALID "
        End If

        ' ''TAX
        'StrSql += vbCrLf + " UNION ALL "
        'If rbtCategoryWise.Checked = True Then
        '    StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        '    StrSql += vbCrLf + " WHERE ACCODE = C.STAXID) AS CATNAME, "
        'Else
        '    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS CATNAME, "
        'End If
        'StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
        'StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        'StrSql += vbCrLf + " SUM(TAX) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        'StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
        'StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        'StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        'StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        'StrSql += vbCrLf + " I.TRANTYPE IN ('RD') AND ISNULL(I.CANCEL,'') = '' "
        'StrSql += StrFilter
        'If rbtCategoryWise.Checked = True Then
        '    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.STAXID HAVING SUM(TAX) <> 0"
        'Else
        '    StrSql += vbCrLf + " GROUP BY I.METALID "
        'End If

        ' '' SURCHARGE 
        'StrSql += vbCrLf + " UNION ALL "
        'If rbtCategoryWise.Checked = True Then
        '    StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        '    StrSql += vbCrLf + " WHERE ACCODE = C.SSCID) AS CATNAME, "
        'Else
        '    StrSql += vbCrLf + " SELECT I.METALID, 'SURCHARGE' AS CATNAME, "
        'End If
        'StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
        'StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        'StrSql += vbCrLf + " SUM(SC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        'StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
        'StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        'StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        'StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        'StrSql += vbCrLf + " I.TRANTYPE IN ('RD') AND ISNULL(I.CANCEL,'') = '' "
        'StrSql += StrFilter
        'If rbtCategoryWise.Checked = True Then
        '    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SSCID HAVING SUM(SC) <> 0"
        'Else
        '    StrSql += vbCrLf + " GROUP BY I.METALID "
        'End If

        ' '' ADDTIONAL SURCHARGE 
        'StrSql += vbCrLf + " UNION ALL "
        'If rbtCategoryWise.Checked = True Then
        '    StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        '    StrSql += vbCrLf + " WHERE ACCODE = C.SASCID) AS CATNAME, "
        'Else
        '    StrSql += vbCrLf + " SELECT I.METALID, 'ADDTIONALSURCHARGE' AS CATNAME, "
        'End If
        'StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
        'StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        'StrSql += vbCrLf + " SUM(ADSC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        'StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
        'StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        'StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        'StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        'StrSql += vbCrLf + " I.TRANTYPE IN ('RD') AND ISNULL(I.CANCEL,'') = '' "
        'StrSql += StrFilter
        'If rbtCategoryWise.Checked = True Then
        '    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SASCID HAVING SUM(ADSC) <> 0"
        'Else
        '    StrSql += vbCrLf + " GROUP BY I.METALID "
        'End If

        'StrSql += vbCrLf + " UNION ALL "
        'StrSql += vbCrLf + " SELECT CATCODE,CATNAME, SUM(ISSPCS) AS ISSPCS, "
        'StrSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
        'StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
        'StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE, COLHEAD FROM ("
        'If rbtCategoryWise.Checked = True Then
        '    StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..ISSUE "
        '    StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
        '    If chkCatShortname.Checked = True Then
        '        StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
        '    Else
        '        StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
        '    End If
        '    StrSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATNAME,  "
        'Else
        '    StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..ISSUE "
        '    ''strsql += vbcrlf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE)AS CATCODE, 'DIAMOND' AS CATNAME,  "
        '    StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS CATNAME,  "
        'End If
        'StrSql += vbCrLf + " SUM(S.STNPCS) ISSPCS,  "
        'StrSql += vbCrLf + " SUM(S.STNWT) AS ISSGRSWT,  "
        'StrSql += vbCrLf + " SUM(S.STNWT) AS ISSNETWT,  "
        'StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.STNAMT) AS RECEIPT,  "
        'StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
        'StrSql += vbCrLf + " 'D' COLHEAD  FROM " & cnStockDb & "..ISSSTONE S "
        'StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
        'StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        'StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        'StrSql += vbCrLf + " " & StrFilter & " AND ISNULL(TRANTYPE,'') = 'SA' AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
        'StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
        ''StrSql += StrFilter
        'If rbtCategoryWise.Checked = True Then
        '    StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
        'Else
        '    StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO  "
        'End If
        'StrSql += vbCrLf + " ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        StrSql += vbCrLf + " ) X "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSREPAIR)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSREPAIR(CATNAME, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += vbCrLf + " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, 0 RESULT, 0 RESULT1, 'T' COLHEAD "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSREPAIR"
        StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
        StrSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSREPAIR)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSREPAIR(CATCODE,CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'Z' AS CATCODE, 'SALES TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + " SUM(PAYMENT),4 RESULT,2 RESULT1, 'S' COLHEAD "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSREPAIR WHERE RESULT = 1 "
        StrSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSREPAIR)>0"
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'strsql += vbcrlf + " (DESCRIPTION, COLHEAD) VALUES('SALES','T') "
        StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'REPAIR DEL ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSREPAIR WHERE RESULT1 = 1 AND COLHEAD <> 'D'"
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,RESULT1) "
        StrSql += vbCrLf + " SELECT '  ' + CATNAME, "
        StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, COLHEAD, RESULT1 "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSREPAIR ORDER BY CATCODE,RESULT"
        StrSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcSales()
        If (RPT_ME_SUM_DABS = True And rbtMetalwise.Checked = True) Then Exit Sub
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSSALES') DROP TABLE TEMP" & systemId & "ABSSALES"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT CATCODE, "
        Else
            StrSql += vbCrLf + " SELECT METALID AS CATCODE, "
        End If
        StrSql += vbCrLf + " CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + " PAYMENT, AVERAGE, RATE, COLHEAD, RESULT, RESULT1 INTO TEMP" & systemId & "ABSSALES FROM ("
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT I.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            End If
        Else
            StrSql += vbCrLf + " SELECT I.METALID, "
            StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS CATNAME, "
        End If
        StrSql += vbCrLf + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += vbCrLf + " SUM(AMOUNT) "
        StrSql += vbCrLf + " AS RECEIPT, "
        StrSql += vbCrLf + " 0 AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        StrSql += vbCrLf + " ELSE "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        StrSql += vbCrLf + " FROM "
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " SELECT PCS"
        If chkSeperateBeeds.Checked = True Then
            StrSql += vbCrLf + " ,CONVERT(NUMERIC(14,3),GRSWT - "
            StrSql += vbCrLf + " ISNULL((SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE AS ISS WHERE ISSSNO  = I.SNO "
            StrSql += vbCrLf + "            AND "
            StrSql += vbCrLf + "            (EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = ISS.STNITEMID AND ISNULL(BEEDS,'') = 'Y' AND ISS.STNSUBITEMID = 0)"
            StrSql += vbCrLf + "            OR EXISTS (SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = ISS.STNITEMID AND SUBITEMID = ISS.STNSUBITEMID AND ISNULL(BEEDS,'') = 'Y' AND ISS.STNSUBITEMID <> 0)"
            StrSql += vbCrLf + "            )"
            StrSql += vbCrLf + "),0))AS GRSWT "
        Else
            StrSql += vbCrLf + " ,GRSWT"
        End If

        StrSql += vbCrLf + " ,NETWT"
        If RPT_SEPVAT_DABS = False Then
            StrSql += vbCrLf + " ,AMOUNT+ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0)"
        Else
            StrSql += vbCrLf + " ,AMOUNT"
        End If
        StrSql += vbCrLf + " -ISNULL("
        StrSql += vbCrLf + "(SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
        StrSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')"
        StrSql += vbCrLf + "),0)"
        StrSql += vbCrLf + " AS AMOUNT,RATE,CATCODE,METALID"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        StrSql += vbCrLf + " I.TRANTYPE IN('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += vbCrLf + " )AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY I.CATCODE,C.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY I.METALID "
        End If

        'If RPT_SEPVAT_DABS = False Then
        'Else
        '    StrSql += vbCrLf + " SUM(AMOUNT) "
        '    If rbtCategoryWise.Checked Then
        '        StrSql += vbCrLf + "  - ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE "
        '        StrSql += vbCrLf + "  WHERE ISSSNO IN "
        '        StrSql += vbCrLf + "  ("
        '        StrSql += vbCrLf + " SELECT SNO FROM " & cnStockDb & "..ISSUE I "
        '        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        '        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        '        StrSql += vbCrLf + " I.TRANTYPE IN('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
        '        StrSql += StrFilter
        '        StrSql += vbCrLf + "  )"
        '        StrSql += vbCrLf + "  AND CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = 'D' AND CATCODE = I.CATCODE)),0)"
        '    Else
        '        StrSql += vbCrLf + "  - ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE "
        '        StrSql += vbCrLf + "  ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE "
        '        StrSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'  AND  "
        '        StrSql += vbCrLf + "  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' " & StrFilter & "   AND COMPANYID IN (" & SelectedCompanyId & ")AND "
        '        StrSql += vbCrLf + "  CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
        '        StrSql += vbCrLf + "  WHERE METALID = I.METALID) AND ISNULL(CANCEL,'') = '')"
        '        StrSql += vbCrLf + "  AND TRANTYPE = 'SA'  AND COMPANYID IN (" & SelectedCompanyId & ") ),0)"
        '        ''strsql += vbcrlf + "  AND CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = 'D' AND CATCODE = R.CATCODE)),0)"
        '    End If
        '    StrSql += vbCrLf + " AS RECEIPT, "
        '    StrSql += vbCrLf + " 0 AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        '    StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        '    StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        '    StrSql += vbCrLf + " ELSE "
        '    StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        '    StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        '    StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        '    StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        '    StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        '    StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        '    StrSql += vbCrLf + " I.TRANTYPE IN('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
        '    StrSql += StrFilter
        '    If rbtCategoryWise.Checked = True Then
        '        StrSql += vbCrLf + " GROUP BY I.CATCODE,C.METALID "
        '    Else
        '        StrSql += vbCrLf + " GROUP BY I.METALID "
        '    End If
        'End If

        If RPT_SEPVAT_DABS Then
            ''TAX
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.STAXID) AS CATNAME, "
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS CATNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(TAX+ISNULL(TT.TAXAMT,0)) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
            StrSql += vbCrLf + " ON TT.SNO=I.SNO and TT.BATCHNO=I.BATCHNO "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.STAXID HAVING SUM(TAX) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID "
            End If

            '' SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.SSCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'SURCHARGE' AS CATNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(SC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SSCID HAVING SUM(SC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID  HAVING SUM(SC) <> 0"
            End If

            '' ADDTIONAL SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.SASCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'ADDTIONALSURCHARGE' AS CATNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(ADSC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SASCID HAVING SUM(ADSC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID HAVING SUM(ADSC) <> 0 "
            End If
        End If
        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT CATCODE,CATNAME, SUM(ISSPCS) AS ISSPCS, "
        StrSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
        StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE, COLHEAD FROM ("
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..ISSUE "
            StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
            Else
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
            End If
            StrSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATNAME,  "
        Else
            StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..ISSUE "
            ''strsql += vbcrlf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE)AS CATCODE, 'DIAMOND' AS CATNAME,  "
            StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS CATNAME,  "
        End If
        StrSql += vbCrLf + " SUM(S.STNPCS) ISSPCS,  "
        StrSql += vbCrLf + " SUM(S.STNWT) AS ISSGRSWT,  "
        StrSql += vbCrLf + " SUM(S.STNWT) AS ISSNETWT,  "
        StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.STNAMT) AS RECEIPT,  "
        StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
        StrSql += vbCrLf + " 'D' COLHEAD  FROM " & cnStockDb & "..ISSSTONE S "
        StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SA','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
        'StrSql += StrFilter
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
        Else
            StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO  "
        End If
        StrSql += vbCrLf + " ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        StrSql += vbCrLf + " ) X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATNAME, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += vbCrLf + " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, 0 RESULT, 0 RESULT1, 'T' COLHEAD "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES"
        'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATCODE,CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'Z' AS CATCODE, 'SALES TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + " SUM(PAYMENT),4 RESULT,2 RESULT1, 'S' COLHEAD "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES WHERE RESULT = 1 "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0"
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'strsql += vbcrlf + " (DESCRIPTION, COLHEAD) VALUES('SALES','T') "
        StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
        StrSql += " WHERE RESULT1 = 1"
        'StrSql += " AND COLHEAD <> 'D'"
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,RESULT1) "
        StrSql += vbCrLf + " SELECT '  ' + CATNAME, "
        StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, COLHEAD, RESULT1 "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
        StrSql += vbCrLf + " WHERE RESULT1 = 1 "
        StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub PROCSAPSMI()
        If Not (RPT_ME_SUM_DABS = True And rbtMetalwise.Checked = True) Then Exit Sub
        Dim Dtmetal As New DataTable
        StrSql = "SELECT METALID,METALNAME FROM " + cnAdminDb + "..METALMAST "
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(Dtmetal)
        If Not Dtmetal.Rows.Count > 0 Then Exit Sub
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = "
        StrSql += " 'TEMP" & systemId & "SASRPUTEMP') "
        StrSql += " DROP TABLE TEMP" & systemId & "SASRPUTEMP "
        StrSql += " CREATE TABLE TEMP" & systemId & "SASRPUTEMP( "
        StrSql += " DESCRIPTION VARCHAR(100),"
        StrSql += " ISSPCS INT,"
        StrSql += " ISSGRSWT NUMERIC(15,3),"
        StrSql += " ISSNETWT NUMERIC(15,3),"
        StrSql += " RECPCS INT,"
        StrSql += " RECGRSWT NUMERIC(15,3),"
        StrSql += " RECNETWT NUMERIC(15,3),"
        StrSql += " RECEIPT NUMERIC(20,2),"
        StrSql += " PAYMENT NUMERIC(20,2),"
        StrSql += " AVERAGE VARCHAR(50),"
        StrSql += " COLHEAD VARCHAR(1),"
        StrSql += " RESULT1 INT,"
        StrSql += " SNO INT IDENTITY(1,1),"
        StrSql += " METALID VARCHAR(2))"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        For cnt As Integer = 0 To Dtmetal.Rows.Count - 1
            ProcSalesMETAL(Dtmetal.Rows(cnt).Item(0).ToString)
            ProcPartlySalesMETAL(Dtmetal.Rows(cnt).Item(0).ToString)
            ProcMiscIssueMETAL(Dtmetal.Rows(cnt).Item(0).ToString)
        Next
        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SASRPUTEMP)>0"
        StrSql += " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(ISNULL(RECEIPT,0)))+']' ,'T' "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPUTEMP "
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        For cnt As Integer = 0 To Dtmetal.Rows.Count - 1
            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SASRPUTEMP WHERE METALID like '%" + Dtmetal.Rows(cnt).Item(0).ToString + "')>0"
            StrSql += " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) "
            StrSql += vbCrLf + " SELECT '" + Dtmetal.Rows(cnt).Item(1).ToString + "' ,'T' "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT"
            StrSql += vbCrLf + " ,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,RESULT1)"
            StrSql += vbCrLf + " select DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS"
            StrSql += vbCrLf + " ,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,RESULT1"
            StrSql += vbCrLf + " from TEMP" & systemId & "SASRPUTEMP WHERE METALID='" + Dtmetal.Rows(cnt).Item(0).ToString + "' ORDER BY METALID,SNO "
            If Dtmetal.Rows(cnt).Item(0).ToString.ToUpper() = "D" Then
                StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
                StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT"
                StrSql += vbCrLf + " ,RECNETWT,COLHEAD,RESULT1)"
                StrSql += vbCrLf + " select DESCRIPTION,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT),SUM(RECPCS)"
                StrSql += vbCrLf + " ,SUM(RECGRSWT),SUM(RECNETWT),COLHEAD,RESULT1"
                StrSql += vbCrLf + " from TEMP" & systemId & "SASRPUTEMP WHERE METALID='SD' GROUP BY DESCRIPTION,COLHEAD,RESULT1"
            End If
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION,ISSPCS, ISSGRSWT, ISSNETWT, "
            StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT1, COLHEAD) "
            StrSql += vbCrLf + " SELECT '" + Dtmetal.Rows(cnt).Item(1).ToString + " TOT',SUM(isnull(ISSPCS,0)),SUM(isnull(ISSGRSWT,0)),SUM(isnull(ISSNETWT,0)), "
            StrSql += vbCrLf + " SUM(isnull(RECPCS,0)),SUM(isnull(RECGRSWT,0)),SUM(isnull(RECNETWT,0)), SUM(isnull(RECEIPT,0)), "
            StrSql += vbCrLf + " SUM(isnull(PAYMENT,0)),2 RESULT1, 'S' COLHEAD "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPUTEMP WHERE METALID like '%" + Dtmetal.Rows(cnt).Item(0).ToString + "'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
        Next

        StrSql = vbCrLf + " UPDATE  TEMP" & systemId & "SASRPU SET ISSPCS=NULL where ISSPCS=0  "
        StrSql += vbCrLf + " UPDATE  TEMP" & systemId & "SASRPU SET ISSGRSWT=NULL where ISSGRSWT=0  "
        StrSql += vbCrLf + " UPDATE  TEMP" & systemId & "SASRPU SET ISSNETWT=NULL where ISSNETWT=0  "
        StrSql += vbCrLf + " UPDATE  TEMP" & systemId & "SASRPU SET RECPCS=NULL where RECPCS=0  "
        StrSql += vbCrLf + " UPDATE  TEMP" & systemId & "SASRPU SET RECGRSWT=NULL where RECGRSWT=0  "
        StrSql += vbCrLf + " UPDATE  TEMP" & systemId & "SASRPU SET RECNETWT=NULL where RECNETWT=0  "
        StrSql += vbCrLf + " UPDATE  TEMP" & systemId & "SASRPU SET RECEIPT=NULL where RECEIPT=0  "
        StrSql += vbCrLf + " UPDATE  TEMP" & systemId & "SASRPU SET PAYMENT=NULL where PAYMENT=0  "
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()

    End Sub

    Private Sub ProcMiscIssueMETAL(ByVal METALID As String)
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSMISCISSUE') DROP TABLE TEMP" & systemId & "ABSMISCISSUE"

        StrSql += " SELECT METALID, "
        StrSql += " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS CATNAME, "

        StrSql += " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += " 0 AS RECEIPT, 0 AS PAYMENT, 1 AS RESULT1, ' ' COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSMISCISSUE FROM " & cnStockDb & "..ISSUE I "
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " TRANTYPE = 'MI' AND ISNULL(I.CANCEL,'') = ''  AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND I.METALID='" + METALID + "' "
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")

        StrSql += " GROUP BY METALID "

        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSMISCISSUE)>0"
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPUTEMP"
        StrSql += " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,COLHEAD,RESULT1,METALID) "
        StrSql += " SELECT 'MISC ISSUE ' + CATNAME CATNAME, "
        StrSql += " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        StrSql += " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        StrSql += " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        StrSql += " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        StrSql += " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        StrSql += " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += " COLHEAD, RESULT1,'" + METALID + "'"
        StrSql += " FROM TEMP" & systemId & "ABSMISCISSUE "
        StrSql += " ORDER BY CATNAME,RESULT1"
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

    End Sub

    Private Sub ProcPartlySalesMETAL(ByVal METALID As String)
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPARTLYSALE') DROP TABLE TEMP" & systemId & "ABSPARTLYSALE "
        StrSql += " SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
        StrSql += " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSPARTLYSALE FROM ("
        If chkCatShortname.Checked = True Then
            StrSql += " SELECT (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
        Else
            StrSql += " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        End If
        StrSql += " WHERE CATCODE = I.CATCODE)AS CATNAME   "
        StrSql += " ,I.PCS AS ISSPCS, I.GRSWT AS ISSGRSWT, I.NETWT AS ISSNETWT "
        StrSql += " ,(SELECT ISNULL(SUM(STNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNPCS "
        StrSql += " ,(SELECT ISNULL(SUM(TAGSTNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNPCS "
        StrSql += " ,(SELECT ISNULL(SUM(STNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO )STNWT "
        StrSql += " ,(SELECT ISNULL(SUM(TAGSTNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNWT "
        StrSql += " ,TAGPCS,TAGGRSWT,TAGNETWT FROM " & cnStockDb & "..ISSUE I "
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
        StrSql += " AND I.TAGNO <> ''"
        StrSql += " AND ISNULL(I.CANCEL,'') = ''   "
        StrSql += " AND I.METALID='" + METALID + "' "
        StrSql += " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += " ) X "
        StrSql += " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
        StrSql += " ) GROUP BY CATNAME"
        StrSql += " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPARTLYSALE)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPUTEMP"
        StrSql += " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD,METALID) "
        StrSql += " SELECT 'PARTLY SALES ' + CATNAME CATNAME, "
        StrSql += " CASE WHEN ISSPCS    <> 0 THEN ISSPCS    ELSE NULL END ISSPCS, "
        StrSql += " CASE WHEN ISSGRSWT  <> 0 THEN ISSGRSWT  ELSE NULL END ISSGRSWT, "
        StrSql += " CASE WHEN ISSNETWT  <> 0 THEN ISSNETWT  ELSE NULL END ISSNETWT, "
        StrSql += " 1 RESULT1, COLHEAD,'" + METALID + "'"
        StrSql += " FROM TEMP" & systemId & "ABSPARTLYSALE ORDER BY RESULT1 "
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


    End Sub

    Private Sub ProcSalesMETAL(ByVal METALID As String)
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSSALES') DROP TABLE TEMP" & systemId & "ABSSALES"
        StrSql += vbCrLf + " SELECT METALID AS CATCODE, "
        StrSql += vbCrLf + " CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + " PAYMENT, AVERAGE, RATE, COLHEAD, RESULT, RESULT1 INTO TEMP" & systemId & "ABSSALES FROM ("
        StrSql += vbCrLf + " SELECT I.METALID, "
        StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS CATNAME, "
        StrSql += vbCrLf + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += vbCrLf + " SUM(AMOUNT) "
        StrSql += vbCrLf + " AS RECEIPT, "
        StrSql += vbCrLf + " 0 AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        StrSql += vbCrLf + " ELSE "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        StrSql += vbCrLf + " FROM "
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " SELECT PCS"
        If chkSeperateBeeds.Checked = True Then
            StrSql += vbCrLf + " ,CONVERT(NUMERIC(14,3),GRSWT - "
            StrSql += vbCrLf + " ISNULL((SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE AS ISS WHERE ISSSNO  = I.SNO "
            StrSql += vbCrLf + "            AND "
            StrSql += vbCrLf + "            (EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = ISS.STNITEMID AND ISNULL(BEEDS,'') = 'Y' AND ISS.STNSUBITEMID = 0)"
            StrSql += vbCrLf + "            OR EXISTS (SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = ISS.STNITEMID AND SUBITEMID = ISS.STNSUBITEMID AND ISNULL(BEEDS,'') = 'Y' AND ISS.STNSUBITEMID <> 0)"
            StrSql += vbCrLf + "            )"
            StrSql += vbCrLf + "),0))AS GRSWT "
        Else
            StrSql += vbCrLf + " ,GRSWT"
        End If

        StrSql += vbCrLf + " ,NETWT"
        If RPT_SEPVAT_DABS = False Then
            StrSql += vbCrLf + " ,AMOUNT+ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0)"
        Else
            StrSql += vbCrLf + " ,AMOUNT"
        End If
        StrSql += vbCrLf + " -ISNULL("
        StrSql += vbCrLf + "(SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
        StrSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')"
        StrSql += vbCrLf + "),0)"
        StrSql += vbCrLf + " AS AMOUNT,RATE,CATCODE,METALID"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        StrSql += vbCrLf + " I.TRANTYPE IN('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
        StrSql += vbCrLf + " AND I.METALID='" + METALID + "' "
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += vbCrLf + " )AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        StrSql += vbCrLf + " GROUP BY I.METALID "




        If RPT_SEPVAT_DABS Then
            ''TAX
            StrSql += vbCrLf + " UNION ALL "

            StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS CATNAME, "

            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(TAX+ISNULL(TT.TAXAMT,0)) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
            StrSql += vbCrLf + " ON TT.SNO=I.SNO and TT.BATCHNO=I.BATCHNO "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += vbCrLf + " AND I.METALID='" + METALID + "' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")

            StrSql += vbCrLf + " GROUP BY I.METALID "


            '' SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "

            StrSql += vbCrLf + " SELECT I.METALID, 'SURCHARGE' AS CATNAME, "

            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(SC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += vbCrLf + " AND I.METALID='" + METALID + "' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SSCID HAVING SUM(SC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID  HAVING SUM(SC) <> 0"
            End If

            '' ADDTIONAL SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "

            StrSql += vbCrLf + " SELECT I.METALID, 'ADDTIONALSURCHARGE' AS CATNAME, "

            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(ADSC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += vbCrLf + " AND I.METALID='" + METALID + "' "
            StrSql += StrFilter

            StrSql += vbCrLf + " GROUP BY I.METALID HAVING SUM(ADSC) <> 0 "

        End If
        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT CATCODE,CATNAME, SUM(ISSPCS) AS ISSPCS, "
        StrSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
        StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE, COLHEAD FROM ("
        StrSql += vbCrLf + " SELECT 'SD' AS CATCODE, 'STUD. DIAMOND' AS CATNAME,  "
        StrSql += vbCrLf + " SUM(S.STNPCS) ISSPCS,  "
        StrSql += vbCrLf + " SUM(S.STNWT) AS ISSGRSWT,  "
        StrSql += vbCrLf + " SUM(S.STNWT) AS ISSNETWT,  "
        StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.STNAMT) AS RECEIPT,  "
        StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
        StrSql += vbCrLf + " 'D' COLHEAD  FROM " & cnStockDb & "..ISSSTONE S "
        StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND I.METALID='" + METALID + "' "
        StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SA','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND S.ISSSNO IN ( SELECT SNO FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND I.METALID='" + METALID + "' "
        StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SA','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
        'StrSql += StrFilter

        StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO  "

        StrSql += vbCrLf + " ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        StrSql += vbCrLf + " ) X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATNAME, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += vbCrLf + " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, 0 RESULT, 0 RESULT1, 'T' COLHEAD "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES"
        'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATCODE,CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'Z' AS CATCODE, 'SALES TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + " SUM(PAYMENT),4 RESULT,2 RESULT1, 'S' COLHEAD "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES WHERE RESULT = 1 "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0"
        StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPUTEMP"
        ''strsql += vbcrlf + " (DESCRIPTION, COLHEAD) VALUES('SALES','T') "
        'StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) "
        'StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
        'StrSql += " WHERE RESULT1 = 1"
        'StrSql += " AND COLHEAD <> 'D'"
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPUTEMP"
        StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,RESULT1,METALID) "
        StrSql += vbCrLf + " SELECT 'SALES ' + CATNAME, "
        StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, COLHEAD, RESULT1,CATCODE "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
        StrSql += vbCrLf + " WHERE RESULT1 = 1 "
        StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcSalesReturn()
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSSALESRETURN') DROP TABLE TEMP" & systemId & "ABSSALESRETURN"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + "  SELECT CATCODE, "
        Else
            strsql += vbcrlf + "  SELECT METALID AS CATCODE, "
        End If
        strsql += vbcrlf + "  CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, "
        strsql += vbcrlf + "  RECGRSWT, RECNETWT, RECEIPT, "
        strsql += vbcrlf + "  PAYMENT, AVERAGE, RATE, COLHEAD, RESULT,  RESULT1  INTO TEMP" & systemId & "ABSSALESRETURN FROM ("
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + "  SELECT R.CATCODE, "
            If chkCatShortname.Checked = True Then
                strsql += vbcrlf + "  (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            Else
                strsql += vbcrlf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            End If
        Else
            StrSql += vbCrLf + "  SELECT R.METALID, "
            strsql += vbcrlf + "  (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = R.METALID)AS CATNAME, "
        End If
        strsql += vbcrlf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
        StrSql += vbCrLf + "  SUM(PCS) AS RECPCS, SUM(GRSWT) AS RECGRSWT, SUM(NETWT) AS RECNETWT, "
        StrSql += vbCrLf + " 0 AS RECEIPT, "
        StrSql += vbCrLf + " SUM(AMOUNT) AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        StrSql += vbCrLf + " ELSE "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        StrSql += vbCrLf + " FROM "
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " SELECT PCS"
        If chkSeperateBeeds.Checked = True Then
            StrSql += vbCrLf + " ,CONVERT(NUMERIC(14,3),GRSWT - "
            StrSql += vbCrLf + " ISNULL((SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE AS ISS WHERE ISSSNO  = R.SNO "
            StrSql += vbCrLf + "            AND "
            StrSql += vbCrLf + "            (EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = ISS.STNITEMID AND ISNULL(BEEDS,'') = 'Y' AND ISS.STNSUBITEMID = 0)"
            StrSql += vbCrLf + "            OR EXISTS (SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = ISS.STNITEMID AND SUBITEMID = ISS.STNSUBITEMID AND ISNULL(BEEDS,'') = 'Y' AND ISS.STNSUBITEMID <> 0)"
            StrSql += vbCrLf + "            )"
            StrSql += vbCrLf + "),0))AS GRSWT "
        Else
            StrSql += vbCrLf + " ,GRSWT"
        End If
        StrSql += vbCrLf + " ,NETWT"
        If RPT_SEPVAT_DABS = False Then
            StrSql += vbCrLf + " ,AMOUNT+ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0)"
        Else
            StrSql += vbCrLf + " ,AMOUNT"
        End If
        StrSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
        StrSql += vbCrLf + " AS AMOUNT,RATE,CATCODE,METALID"
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
        StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + "  TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
        StrSql += vbCrLf + " )AS R"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY R.CATCODE,C.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY R.METALID "
        End If

        'If RPT_SEPVAT_DABS = False Then
        '    StrSql += vbCrLf + "  0 AS RECEIPT, SUM(AMOUNT+ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0)) "
        '    StrSql += vbCrLf + "  AS PAYMENT, 1 AS RESULT, 1 AS RESULT1, "
        '    StrSql += vbCrLf + "  CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        '    StrSql += vbCrLf + "  (SUM(AMOUNT+ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0))/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        '    StrSql += vbCrLf + "  ELSE "
        '    StrSql += vbCrLf + "  (SUM(AMOUNT+ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0))/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        '    StrSql += vbCrLf + "  CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        'Else
        '    StrSql += vbCrLf + "  0 AS RECEIPT, SUM(AMOUNT) "
        '    'If rbtCategoryWise.Checked Then
        '    '    strsql += vbcrlf + "  - ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE "
        '    '    strsql += vbcrlf + "  ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT "
        '    '    strsql += vbcrlf + "  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'  AND  "
        '    '    strsql += vbcrlf + "  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND "
        '    '    strsql += vbcrlf + "  CATCODE = R.CATCODE AND ISNULL(CANCEL,'') = '' AND COMPANYID IN (" & SelectedCompanyId & "))"
        '    '    strsql += vbcrlf + "  " & StrFilter & " AND TRANTYPE = 'SR'  AND COMPANYID IN (" & SelectedCompanyId & ")"
        '    '    strsql += vbcrlf + "  AND CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = 'D' AND CATCODE = R.CATCODE)),0)"
        '    'Else
        '    '    strsql += vbcrlf + "  - ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE "
        '    '    strsql += vbcrlf + "  ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT "
        '    '    strsql += vbcrlf + "  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'  AND  "
        '    '    strsql += vbcrlf + "  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' " & StrFilter & "   AND COMPANYID IN (" & SelectedCompanyId & ")AND "
        '    '    strsql += vbcrlf + "  CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
        '    '    strsql += vbcrlf + "  WHERE METALID = R.METALID) AND ISNULL(CANCEL,'') = '')"
        '    '    strsql += vbcrlf + "  AND TRANTYPE = 'SR'  AND COMPANYID IN (" & SelectedCompanyId & ") ),0)"
        '    '    ''strsql += vbcrlf + "  AND CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = 'D' AND CATCODE = R.CATCODE)),0)"
        '    'End If
        '    StrSql += vbCrLf + "  AS PAYMENT, 1 AS RESULT, 1 AS RESULT1, "
        '    StrSql += vbCrLf + "  CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        '    StrSql += vbCrLf + "  (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        '    StrSql += vbCrLf + "  ELSE "
        '    StrSql += vbCrLf + "  (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        '    StrSql += vbCrLf + "  CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        'End If


        'strsql += vbcrlf + "  FROM " & cnStockDb & "..RECEIPT R"
        'strsql += vbcrlf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        'strsql += vbcrlf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        'strsql += vbcrlf + "  TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
        'StrSql += StrFilter
        'If rbtCategoryWise.Checked = True Then
        '    StrSql += vbCrLf + "  GROUP BY CATCODE,CASHID "
        'Else
        '    StrSql += vbCrLf + "  GROUP BY METALID,CASHID "
        'End If
        If RPT_SEPVAT_DABS Then
            StrSql += vbCrLf + "  UNION ALL "
            ''TAX
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.STAXID) AS CATNAME, "
            Else
                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNTAX' AS CATNAME,"
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(TAX+ISNULL(TT.TAXAMT,0)) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
            StrSql += vbCrLf + " ON TT.SNO=R.SNO and TT.BATCHNO=R.BATCHNO "
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE,R.CASHID,C.STAXID HAVING SUM(TAX) <> 0"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID,R.CASHID "
            End If
            StrSql += vbCrLf + "  UNION ALL "
            ''SURCHARGE
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.SSCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNSURCHARGE' AS CATNAME,"
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(SC) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE,R.CASHID,C.SSCID HAVING SUM(SC) <> 0"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID,R.CASHID HAVING SUM(SC) <> 0 "
            End If
            StrSql += vbCrLf + "  UNION ALL "

            ''ADDTIONAL SURCHARGE
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.SASCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNSURCHARGE' AS CATNAME,"
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(ADSC) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE,R.CASHID, C.SASCID HAVING SUM(ADSC) <> 0"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID,R.CASHID HAVING SUM(ADSC) <> 0 "
            End If

            StrSql += vbCrLf + "  UNION ALL "
            StrSql += vbCrLf + "  SELECT CATCODE,CATNAME, SUM(ISSPCS) AS ISSPCS, "
            StrSql += vbCrLf + "  SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += vbCrLf + "  SUM(RECPCS) AS RECPCS, SUM(RECGRSWT) AS RECGRSWT, SUM(RECNETWT) AS RECNETWT, "
            StrSql += vbCrLf + "  SUM(RECEIPT) RECEIPT, SUM(PAYMENT) PAYMENT, "
            StrSql += vbCrLf + "  RESULT, 1 AS RESULT1, AVERAGE, RATE, COLHEAD FROM ("
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..RECEIPT "
                StrSql += vbCrLf + "  WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
                If chkCatShortname.Checked = True Then
                    StrSql += vbCrLf + "  (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
                Else
                    StrSql += vbCrLf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
                End If
                StrSql += vbCrLf + "  WHERE CATCODE = S.CATCODE) CATNAME,  "
            Else
                StrSql += vbCrLf + "  SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..RECEIPT "
                StrSql += vbCrLf + "  WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & "))AS CATCODE, 'DIAMOND' AS CATNAME,  "
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS,  0 AS ISSGRSWT, 0 AS ISSNETWT,  "
            StrSql += vbCrLf + "  SUM(S.STNPCS) AS RECPCS,  SUM(S.STNWT)  AS RECGRSWT,  SUM(S.STNWT)  AS RECNETWT,  "
            StrSql += vbCrLf + "  0 AS RECEIPT,  SUM(S.STNAMT) AS PAYMENT,  "
            StrSql += vbCrLf + "  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            StrSql += vbCrLf + "  'D' COLHEAD  FROM " & cnStockDb & "..RECEIPTSTONE S "
            StrSql += vbCrLf + "  WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(CANCEL,'') = ''  AND TRANTYPE = 'SR' AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + "  AND  (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            StrSql += vbCrLf + "  AND COMPANYID IN (" & SelectedCompanyId & ")"
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY S.CATCODE,S.BATCHNO  "
            Else
                StrSql += vbCrLf + "  GROUP BY S.BATCHNO  "
            End If
            StrSql += vbCrLf + "  ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        End If
        StrSql += vbCrLf + "  ) X "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        strsql += vbcrlf + "  BEGIN "
        strsql += vbcrlf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN(CATNAME, "
        strsql += vbcrlf + "  ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        strsql += vbcrlf + "  RESULT, RESULT1, COLHEAD) "
        strsql += vbcrlf + "  SELECT 'SALES RETURN'CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        strsql += vbcrlf + "  0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT,  0 RESULT, 0 RESULT1, 'T' COLHEAD "
        strsql += vbcrlf + "  END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        strsql += vbcrlf + "  BEGIN "
        strsql += vbcrlf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        strsql += vbcrlf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
        strsql += vbcrlf + "  SELECT 'Z' AS CATCODE, 'SALES RETURN TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        strsql += vbcrlf + "  SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        strsql += vbcrlf + "  SUM(PAYMENT), 4 RESULT, 2 RESULT1,'S' COLHEAD "
        strsql += vbcrlf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE RESULT = 1 "
        strsql += vbcrlf + "  END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        strsql += vbcrlf + "  BEGIN "
        strsql += vbcrlf + " INSERT INTO TEMP" & systemId & "SASRPU"
        ''strsql += vbcrlf + "  (DESCRIPTION, COLHEAD) VALUES('SALES RETURN','T') "
        strsql += vbcrlf + "  (DESCRIPTION, COLHEAD) "
        strsql += vbcrlf + "  SELECT 'SALES RETURN ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
        strsql += vbcrlf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1"
        strsql += vbcrlf + "  INSERT INTO TEMP" & systemId & "SASRPU"
        strsql += vbcrlf + "  (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,RESULT1) "
        strsql += vbcrlf + "  SELECT '  ' + CATNAME, "
        strsql += vbcrlf + "  CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        strsql += vbcrlf + "  CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        strsql += vbcrlf + "  CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        strsql += vbcrlf + "  CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        strsql += vbcrlf + "  CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        strsql += vbcrlf + "  CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        strsql += vbcrlf + "  CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        strsql += vbcrlf + "  CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        strsql += vbcrlf + "  CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, COLHEAD, RESULT1  "
        StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1 ORDER BY RESULT1, CATCODE, RESULT"
        StrSql += vbCrLf + "  END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcPurchase()
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPURCHASE') DROP TABLE TEMP" & systemId & "ABSPURCHASE"
        If rbtCategoryWise.Checked = True Then
            StrSql += " SELECT CATCODE, "
        Else
            StrSql += " SELECT METALID AS CATCODE, "
        End If
        StrSql += " CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, "
        StrSql += " RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += " PAYMENT, AVERAGE, RATE, COLHEAD, RESULT,  RESULT1  INTO TEMP" & systemId & "ABSPURCHASE FROM ("
        If rbtCategoryWise.Checked = True Then
            StrSql += " SELECT R.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            Else
                StrSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            End If
        Else
            StrSql += " SELECT R.METALID, "
            StrSql += " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = R.METALID)AS CATNAME, "
        End If
        StrSql += " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
        StrSql += " SUM(PCS) AS RECPCS, SUM(GRSWT) AS RECGRSWT, SUM(NETWT) AS RECNETWT, "
        StrSql += vbCrLf + " 0 AS RECEIPT, "
        StrSql += vbCrLf + " SUM(AMOUNT) AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        StrSql += vbCrLf + " ELSE "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        StrSql += vbCrLf + " FROM "
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " SELECT PCS,GRSWT,NETWT"
        If RPT_SEPVAT_DABS = False Then
            StrSql += vbCrLf + " ,AMOUNT+ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0)"
        Else
            StrSql += vbCrLf + " ,AMOUNT"
        End If
        StrSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
        StrSql += vbCrLf + " AS AMOUNT,RATE,CATCODE,METALID"
        StrSql += " FROM " & cnStockDb & "..RECEIPT R"
        StrSql += " WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " TRANTYPE = 'PU' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
        StrSql += vbCrLf + " )AS R"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY R.CATCODE,C.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY R.METALID "
        End If
        'If RPT_SEPVAT_DABS = False Then
        '    StrSql += " 0 AS RECEIPT, SUM(AMOUNT-ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0)) "
        '    StrSql += " AS PAYMENT, 1 AS RESULT, 1 AS RESULT1, "
        '    StrSql += " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        '    StrSql += " (SUM(AMOUNT-ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0))/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        '    StrSql += " ELSE "
        '    StrSql += " (SUM(AMOUNT-ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0))/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        '    StrSql += " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        'Else
        '    StrSql += " 0 AS RECEIPT, SUM(AMOUNT) "
        '    'If rbtCategoryWise.Checked Then
        '    '    StrSql += " - ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE "
        '    '    StrSql += " ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT "
        '    '    StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'  AND  "
        '    '    StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND "
        '    '    StrSql += " CATCODE = R.CATCODE AND ISNULL(CANCEL,'') = '' AND COMPANYID IN (" & SelectedCompanyId & "))"
        '    '    StrSql += " " & StrFilter & " AND TRANTYPE = 'PU'  AND COMPANYID IN (" & SelectedCompanyId & ")"
        '    '    StrSql += " AND CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = 'D' AND CATCODE = R.CATCODE)),0)"
        '    'Else
        '    '    StrSql += " - ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE "
        '    '    StrSql += " ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT "
        '    '    StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'  AND  "
        '    '    StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        '    '    StrSql += " " & StrFilter & " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
        '    '    StrSql += " WHERE METALID = R.METALID) AND ISNULL(CANCEL,'') = '')"
        '    '    StrSql += " AND TRANTYPE = 'PU'  AND COMPANYID IN (" & SelectedCompanyId & ") ),0)"
        '    '    ''StrSql += " AND CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = 'D' AND CATCODE = R.CATCODE)),0)"
        '    'End If
        '    StrSql += " AS PAYMENT, 1 AS RESULT, 1 AS RESULT1, "
        '    StrSql += " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        '    StrSql += " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        '    StrSql += " ELSE "
        '    StrSql += " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        '    StrSql += " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        'End If

        'StrSql += " FROM " & cnStockDb & "..RECEIPT R"
        'StrSql += " WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        'StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        'StrSql += " TRANTYPE = 'PU' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
        'StrSql += StrFilter
        'If rbtCategoryWise.Checked = True Then
        '    StrSql += " GROUP BY CATCODE,CASHID "
        'Else
        '    StrSql += " GROUP BY METALID,CASHID "
        'End If
        If RPT_SEPVAT_DABS Then
            StrSql += " UNION ALL"
            If rbtCategoryWise.Checked = True Then
                StrSql += " SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += " WHERE ACCODE = C.PTAXID) AS CATNAME, "
            Else
                StrSql += " SELECT R.METALID, 'PURCHASETAX' AS CATNAME, "
            End If
            StrSql += " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += " SUM(TAX) AS RECEIPT,0 PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += " FROM " & cnStockDb & "..RECEIPT R"
            StrSql += " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += " WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += " R.TRANTYPE = 'PU' AND ISNULL(R.CANCEL,'') = ''  AND R.COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            If rbtCategoryWise.Checked = True Then
                StrSql += " GROUP BY R.CATCODE, C.PTAXID "
            Else
                StrSql += " GROUP BY R.METALID "
            End If
            StrSql += " HAVING SUM(TAX) > 0"
            StrSql += " UNION ALL"

            StrSql += " SELECT CATCODE,CATNAME, SUM(ISSPCS) AS ISSPCS, "
            StrSql += " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += " SUM(RECPCS) AS RECPCS, SUM(RECGRSWT) AS RECGRSWT, SUM(RECNETWT) AS RECNETWT, "
            StrSql += " SUM(RECEIPT) RECEIPT, SUM(PAYMENT) PAYMENT, "
            StrSql += " RESULT, 1 AS RESULT1, AVERAGE, RATE, COLHEAD FROM ("
            If rbtCategoryWise.Checked = True Then
                StrSql += " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..RECEIPT "
                StrSql += " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
                If chkCatShortname.Checked = True Then
                    StrSql += " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
                Else
                    StrSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
                End If
                StrSql += " WHERE CATCODE = S.CATCODE) CATNAME,  "
            Else
                StrSql += " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..RECEIPT "
                StrSql += " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & "))AS CATCODE, 'DIAMOND' AS CATNAME,  "
            End If
            StrSql += " 0 AS ISSPCS,  0 AS ISSGRSWT, 0 AS ISSNETWT,  "
            StrSql += " SUM(S.STNPCS) AS RECPCS,  SUM(S.STNWT)  AS RECGRSWT,  SUM(S.STNWT)  AS RECNETWT,  "
            StrSql += " 0 AS RECEIPT,  SUM(S.STNAMT) AS PAYMENT,  "
            StrSql += " 2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            StrSql += " 'D' COLHEAD  FROM " & cnStockDb & "..RECEIPTSTONE S "
            StrSql += " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += " " & Replace(StrFilter, "SYSTEMID", "S.SYSTEMID") & " AND ISNULL(CANCEL,'') = ''  AND TRANTYPE = 'PU' AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += " AND  (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
            If rbtCategoryWise.Checked = True Then
                StrSql += " GROUP BY S.CATCODE,S.BATCHNO  "
            Else
                StrSql += " GROUP BY S.BATCHNO  "
            End If
            StrSql += " ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        End If

        StrSql += " ) X "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPURCHASE)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "ABSPURCHASE(CATNAME, "
        StrSql += " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += " RESULT, RESULT1, COLHEAD) "
        StrSql += " SELECT 'PURCHASE'CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, 0 RESULT, 0 AS RESULT1, 'T' COLHEAD "
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPURCHASE)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "ABSPURCHASE(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
        StrSql += " SELECT 'Z' AS CATCODE, 'PURCHASE TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += " SUM(PAYMENT),4 RESULT, 2 AS RESULT1, 'S' COLHEAD "
        StrSql += " FROM TEMP" & systemId & "ABSPURCHASE WHERE RESULT = 1 "
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPURCHASE)>0"
        StrSql += " BEGIN "
        StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
        ''StrSql += " (DESCRIPTION, COLHEAD) VALUES('PURCHASE','T') "
        StrSql += " (DESCRIPTION, COLHEAD) "
        StrSql += " SELECT 'PURCHASE ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
        StrSql += " FROM TEMP" & systemId & "ABSPURCHASE WHERE RESULT1 = 1"
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,RESULT1) "
        StrSql += " SELECT '  ' + CATNAME, "
        StrSql += " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        StrSql += " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        StrSql += " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        StrSql += " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        StrSql += " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        StrSql += " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, COLHEAD, RESULT1  "
        StrSql += " FROM TEMP" & systemId & "ABSPURCHASE WHERE RESULT = 1 ORDER BY CATCODE, RESULT"
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcMiscIssue()
        If (RPT_ME_SUM_DABS = True And rbtMetalwise.Checked = True) Then Exit Sub
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSMISCISSUE') DROP TABLE TEMP" & systemId & "ABSMISCISSUE"
        If rbtCategoryWise.Checked = True Then
            If chkCatShortname.Checked = True Then
                StrSql += " SELECT CATCODE, "
                StrSql += " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            Else
                StrSql += " SELECT CATCODE, "
                StrSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            End If
        Else
            StrSql += " SELECT METALID, "
            StrSql += " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS CATNAME, "
        End If
        StrSql += " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += " 0 AS RECEIPT, 0 AS PAYMENT, 1 AS RESULT1, ' ' COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSMISCISSUE FROM " & cnStockDb & "..ISSUE I "
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " TRANTYPE = 'MI' AND ISNULL(I.CANCEL,'') = ''  AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        If rbtCategoryWise.Checked = True Then
            StrSql += " GROUP BY CATCODE "
        Else
            StrSql += " GROUP BY METALID "
        End If
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSMISCISSUE)>0"
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION, COLHEAD) VALUES('MISC ISSUE','T') "
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,COLHEAD,RESULT1) "
        StrSql += " SELECT '  ' + CATNAME CATNAME, "
        StrSql += " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        StrSql += " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        StrSql += " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        StrSql += " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        StrSql += " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        StrSql += " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += " COLHEAD, RESULT1 "
        StrSql += " FROM TEMP" & systemId & "ABSMISCISSUE "
        If rbtCategoryWise.Checked = True Then
            StrSql += " ORDER BY CATCODE,RESULT1"
        Else
            StrSql += " ORDER BY CATNAME,RESULT1"
        End If
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

    End Sub

    Private Sub ProcAdvDueSummary()
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVDUE')"
        StrSql += vbCrLf + " DROP TABLE TEMP" & systemId & "ADVDUE"
        StrSql += vbCrLf + " SELECT 'OPE' SEP"
        StrSql += vbCrLf + " ,CASE WHEN SUBSTRING(RUNNO,6,1) IN ('O','R') THEN 'O' ELSE TRANTYPE END TRANTYPE"
        'StrSql += vbCrLf + " ,CASE WHEN TRANTYPE IN ('D','J') THEN CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END ELSE CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END END AS AMOUNT"
        StrSql += vbCrLf + " ,CASE WHEN TRANTYPE IN ('D','J') THEN CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END ELSE CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END END AS AMOUNT"
        StrSql += " ,BATCHNO"
        StrSql += vbCrLf + " INTO TEMP" & systemId & "ADVDUE"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' AND ISNULL(COMPANYID,'') IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not chkMiscRecPaySummary.Checked Then StrSql += " AND TRANTYPE <> 'T'"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT 'REC' SEP,CASE WHEN SUBSTRING(RUNNO,6,1) IN ('O','R') THEN 'O' ELSE TRANTYPE END TRANTYPE,CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END AS AMOUNT,BATCHNO"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND RECPAY = 'R' AND ISNULL(CANCEL,'') = '' AND ISNULL(COMPANYID,'') IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not chkMiscRecPaySummary.Checked Then StrSql += " AND TRANTYPE <> 'T'"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT 'PAY' SEP,CASE WHEN SUBSTRING(RUNNO,6,1) IN ('O','R') THEN 'O' ELSE TRANTYPE END TRANTYPE,CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END AS AMOUNT,BATCHNO"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND RECPAY = 'P' AND ISNULL(CANCEL,'') = '' AND ISNULL(COMPANYID,'') IN (" & SelectedCompanyId & ")"
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += StrFilter
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " UPDATE TEMP" & systemId & "ADVDUE SET TRANTYPE = 'J'"
        StrSql += " FROM TEMP" & systemId & "ADVDUE AS O INNER JOIN " & cnAdminDb & "..ITEMDETAIL I ON I.BATCHNO = O.BATCHNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVDUESUMMARY')"
        StrSql += vbCrLf + " DROP TABLE TEMP" & systemId & "ADVDUESUMMARY"
        StrSql += vbCrLf + " SELECT "
        StrSql += vbCrLf + " CASE TRANTYPE WHEN 'O' THEN 'ORDER ADVANCE' WHEN 'D' THEN 'DUE' WHEN 'T' THEN 'OTHERS' WHEN 'J' THEN 'TOBE' ELSE 'ADVANCE' END AS TRANTYPE"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN AMOUNT ELSE 0 END) AS OPENING"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN AMOUNT ELSE 0 END)AS RECEIPT"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'PAY' THEN AMOUNT ELSE 0 END)AS PAYMENT"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'PAY' THEN -1*AMOUNT ELSE AMOUNT END)AS CLOSING"
        StrSql += vbCrLf + " INTO TEMP" & systemId & "ADVDUESUMMARY"
        StrSql += vbCrLf + " FROM"
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " SELECT SEP,TRANTYPE,AMOUNT FROM TEMP" & systemId & "ADVDUE WHERE TRANTYPE NOT IN ('D','J')"
        StrSql += vbCrLf + " )X"
        StrSql += vbCrLf + " GROUP BY TRANTYPE"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT "
        StrSql += vbCrLf + " CASE TRANTYPE WHEN 'O' THEN 'ORDER ADVANCE' WHEN 'D' THEN 'DUE' WHEN 'T' THEN 'OTHERS' WHEN 'J' THEN 'TOBE' ELSE 'ADVANCE' END AS TRANTYPE"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN AMOUNT ELSE 0 END) AS OPENING"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN AMOUNT ELSE 0 END)AS RECEIPT"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'PAY' THEN AMOUNT ELSE 0 END)AS PAYMENT"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN -1*AMOUNT ELSE AMOUNT END)AS CLOSING"
        StrSql += vbCrLf + " FROM"
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " SELECT SEP,TRANTYPE,AMOUNT FROM TEMP" & systemId & "ADVDUE WHERE TRANTYPE IN ('D','J')"
        StrSql += vbCrLf + " )X"
        StrSql += vbCrLf + " GROUP BY TRANTYPE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcSASRPU(ByVal DotMatrix As Boolean)

        PROCSAPSMI()
        ProcSales()
        ProcRepairDelivery()
        ProcSalesReturn()
        ProcPurchase()
        ProcOrderAdvanceAmountToWeight()
        ProcMiscIssue()

        If chkDetaledRecPay.Checked = False Then
            ''EMPTY LINE
            StrSql = " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
        ProcCreditSales()
        ProcCreditPurchase()
        ProcCreditAdjustment()
        ProcCreditPurchasePayment()
        ProcAdvanceReceived()
        ProcAdvanceAdjustment()
        ProcOrderAdvance()
        ProcOrderAdvanceAdjusted()
        ProcRepairAdvance()
        ProcRepairAdvanceAdjusted()
        ProcChitPayment()
        ProcMiscReceipt()
        ProcMiscPayment()
        ''EMPTY LINE
        StrSql = " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        ProcBeeds()
        ProcDiscount()
        ProcHandling()
        ProcroundOff()
        ProcApprovalIssue()
        ProcApprovalReceipt()
        ProcHomeSales()
        ProcPartlySales()
        ProcChitCollection()
        ProcWtSubtot()
        ProcCollection()
        ProcAmtSubtot()
        ProcAdvDueSummary()
        If chkCancelBills.Checked Then ProcCancelBills()

        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SASRPU)>0"
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
        StrSql += " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COLHEAD) "
        StrSql += " SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD "
        StrSql += " FROM TEMP" & systemId & "SASRPU ORDER BY SNO" 'WHERE ISNULL(RESULT1,1) NOT IN (0,2) ORDER BY SNO"
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ADVDUESUMMARY)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
        StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1,COLHEAD) VALUES('ADV-DUE SUMMARY','T')"
        StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
        StrSql += " (COL1,COL6,COL7,COL8,COL9,COLHEAD) "
        StrSql += " SELECT ' ','OPENING','RECEIPT','PAYMENT','CLOSING','S'"
        StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
        StrSql += " (COL1,COL6,COL7,COL8,COL9) "
        StrSql += vbCrLf + " SELECT ' ' + TRANTYPE, "
        StrSql += vbCrLf + " CASE WHEN OPENING   <> 0 THEN OPENING   ELSE NULL END OPENING, "
        StrSql += vbCrLf + " CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " CASE WHEN CLOSING   <> 0 THEN CLOSING  ELSE NULL END CLOSING "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ADVDUESUMMARY ORDER BY TRANTYPE"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        If chkCancelBills.Checked Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSCANCELBILL )>0 "
            StrSql += " BEGIN "
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1,COLHEAD) VALUES('CANCEL BILLS','T')"
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
            StrSql += " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9) "
            StrSql += " SELECT '  ' + TRANS, "
            StrSql += " CASE WHEN IPCS   <> 0 THEN IPCS   ELSE NULL END IPCS, "
            StrSql += " CASE WHEN IGRSWT <> 0 THEN IGRSWT ELSE NULL END IGRSWT, "
            StrSql += " CASE WHEN INETWT <> 0 THEN INETWT ELSE NULL END INETWT, "
            StrSql += " CASE WHEN RPCS   <> 0 THEN RPCS   ELSE NULL END RPCS, "
            StrSql += " CASE WHEN RGRSWT <> 0 THEN RGRSWT ELSE NULL END RGRSWT, "
            StrSql += " CASE WHEN RNETWT <> 0 THEN RNETWT ELSE NULL END RNETWT, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT FROM "
            StrSql += " TEMP" & systemId & "ABSCANCELBILL ORDER BY TRANTYPE, TRANNO"
            StrSql += " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        Dim dt As New DataTable("SASRPU")
        With dt
            .Columns.Add("DESCRIPTION", GetType(String))
            .Columns.Add("ISSPCS", GetType(Integer))
            .Columns.Add("ISSGRSWT", GetType(Decimal))
            .Columns.Add("ISSNETWT", GetType(Decimal))
            .Columns.Add("RECPCS", GetType(Integer))
            .Columns.Add("RECGRSWT", GetType(Decimal))
            .Columns.Add("RECNETWT", GetType(Decimal))
            .Columns.Add("RECEIPT", GetType(Decimal))
            .Columns.Add("PAYMENT", GetType(Decimal))
            .Columns.Add("AVERAGE", GetType(String))
            .Columns.Add("COLHEAD", GetType(String))

            .Columns("ISSPCS").Caption = "PCS"
            .Columns("ISSGRSWT").Caption = "GRSWT"
            .Columns("ISSNETWT").Caption = "NETWT"
            .Columns("RECPCS").Caption = "PCS"
            .Columns("RECGRSWT").Caption = "GRSWT"
            .Columns("RECNETWT").Caption = "NETWT"
            .Columns("RECEIPT").DataType = GetType(Decimal)
            .Columns("PAYMENT").DataType = GetType(Decimal)
            .Columns("AVERAGE").Caption = "AVERAGE"
            'StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SASRPU)>0 "
            'StrSql += " BEGIN "
            '.Columns("AVERAGE").MaxLength = objGPack.GetSqlValue(StrSql & "SELECT MAX(LEN(ISNULL(AVERAGE,0))) AS AA FROM TEMP" & systemId & "SASRPU END", , "-1")
            '.Columns("COLHEAD").MaxLength = objGPack.GetSqlValue(StrSql & "SELECT MAX(LEN(ISNULL(COLHEAD,0))) AS AA FROM TEMP" & systemId & "SASRPU END", , "-1")
        End With

        StrSql = "SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT, "
        StrSql += " AVERAGE, COLHEAD AS COLHEAD FROM TEMP" & systemId & "SASRPU ORDER BY SNO, RESULT1 "
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dt)
        'dt.Columns.Remove("AVERAGE")
        dt.AcceptChanges()
        If dt.Rows.Count > 0 Then dsReportCol.Tables.Add(dt)
    End Sub

    Private Sub ProcCreditSales()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDSAL') DROP TABLE TEMP" & systemId & "ABSCREDSAL "
        strsql += vbcrlf + " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += vbCrLf + " ISNULL((select top 1 '(JND)' from " & cnAdminDb & "..ITEMDETAIL where batchno=o.batchno),'')+"
        StrSql += vbCrLf + " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += vbCrLf + " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END) AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMP" & systemId & "ABSCREDSAL FROM " & cnAdminDb & "..OUTSTANDING O"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(O.CANCEL,'') = ''  "
        StrSql += vbCrLf + " AND O.COMPANYID IN (" & SelectedCompanyId & ") AND O.RECPAY = 'P' AND O.PAYMODE = 'DU'"
        StrSql += StrFilter
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSCREDSAL)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += vbCrLf + " SELECT 'CREDIT BILL(DUE) ['+CONVERT(VARCHAR,ISNULL(SUM(ISNULL(PAYMENT,0)-ISNULL(RECEIPT,0)),'')) +']'"
            StrSql += vbCrLf + " ,'T',NULL"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSCREDSAL"
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSCREDSAL"
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += vbCrLf + " SELECT 'CREDIT BILL(DUE)'"
            StrSql += vbCrLf + " ,'T',SUM(PAYMENT)-SUM(RECEIPT)"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSCREDSAL"
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcCreditPurchase()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDPUR') DROP TABLE TEMP" & systemId & "ABSCREDPUR"
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSCREDPUR FROM " & cnAdminDb & "..OUTSTANDING O"
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''  "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND O.RECPAY = 'R' AND PAYMODE = 'DU'"
        StrSql += StrFilter
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSCREDPUR)>0 "
        StrSql += " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) "
            StrSql += " SELECT 'CREDIT PURCHASE(DUE) [' + CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),'')) + ']','T'"
            StrSql += " FROM TEMP" & systemId & "ABSCREDPUR"
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  ' + CATNAME CATNAME, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += " 1 RESULT1, COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "ABSCREDPUR"
            If rbtAmount.Checked = True Then
                StrSql += " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,COLHEAD,RECEIPT) "
            StrSql += " SELECT 'CREDIT PURCHASE(DUE)','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += " FROM TEMP" & systemId & "ABSCREDPUR"
        End If
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcCreditAdjustment()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDADJ') DROP TABLE TEMP" & systemId & "ABSCREDADJ "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO  WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += vbCrLf + " ISNULL((select top 1 '(JD)' from " & cnAdminDb & "..ITEMDETAIL where runno=o.runno),'')+"
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSCREDADJ FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += "  AND O.RECPAY = 'R' AND COMPANYID IN (" & SelectedCompanyId & ") AND PAYMODE = 'DR'"
        StrSql += StrFilter
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSCREDADJ)>0 "
        StrSql += " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) "
            StrSql += " SELECT 'CREDIT ADJUSTMENT [' + CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),''))+']','T'"
            StrSql += " FROM TEMP" & systemId & "ABSCREDADJ "
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  ' + CATNAME CATNAME, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += " 1 RESULT1, COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "ABSCREDADJ "
            If rbtAmount.Checked = True Then
                StrSql += " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += " SELECT 'CREDIT ADJUSTMENT','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += " FROM TEMP" & systemId & "ABSCREDADJ "
        End If
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcCreditPurchasePayment()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDPURPAY') DROP TABLE TEMP" & systemId & "ABSCREDPURPAY "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO  WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN AMOUNT ELSE 0 END) AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSCREDPURPAY FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += "  AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += "  AND PAYMODE = 'DP' AND O.RECPAY = 'P'"
        StrSql += StrFilter
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSCREDPURPAY)>0 "
        StrSql += " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) "
            StrSql += " SELECT 'PURCHASE/SALESRETURN PAYMENT['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
            StrSql += " FROM TEMP" & systemId & "ABSCREDPURPAY "
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  ' + CATNAME CATNAME, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += " 1 RESULT1, COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "ABSCREDPURPAY "
            If rbtAmount.Checked = True Then
                StrSql += " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += " SELECT 'PURCHASE/SALESRETURN PAYMENT','T',SUM(PAYMENT)-SUM(RECEIPT)"
            StrSql += " FROM TEMP" & systemId & "ABSCREDPURPAY "
        End If
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcAdvanceReceived()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVREC') DROP TABLE TEMP" & systemId & "ADVREC "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ADVREC FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE = 'AR' AND O.RECPAY = 'R' "
        StrSql += " AND O.TRANTYPE = 'A'"
        StrSql += " AND NOT (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ADVREC)>0 "
        StrSql += " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) "
            StrSql += " SELECT 'ADVANCE RECEIVED ['+CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),''))+']','T'"
            StrSql += " FROM TEMP" & systemId & "ADVREC "
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  ' + CATNAME CATNAME, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += " 1 RESULT1, COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "ADVREC "
            If rbtAmount.Checked = True Then
                StrSql += " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += " SELECT 'ADVANCE RECEIVED','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += " FROM TEMP" & systemId & "ADVREC "
        End If
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub


    Private Sub ProcAdvanceAdjustment()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVADJ') DROP TABLE TEMP" & systemId & "ADVADJ "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END) AS PAYMENT, "
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ADVADJ FROM " & cnStockDb & ".. OUTSTANDING O"
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND PAYMODE IN ('AA','AP') AND O.RECPAY = 'P' "
        StrSql += " AND O.TRANTYPE = 'A'"
        StrSql += " AND NOT (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        'StrSql += " AND O.RUNNO LIKE 'A%'"
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ADVADJ)>0 "
        StrSql += " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) "
            StrSql += " SELECT 'ADVANCE ADJUSTED ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
            StrSql += " FROM TEMP" & systemId & "ADVADJ "

            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  ' + CATNAME CATNAME, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += " 1 RESULT1, COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "ADVADJ "
            If rbtAmount.Checked = True Then
                StrSql += " ORDER BY RESULT1,PAYMENT "
            Else
                StrSql += " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += " SELECT 'ADVANCE ADJUSTED','T',SUM(PAYMENT)-SUM(RECEIPT)"
            StrSql += " FROM TEMP" & systemId & "ADVADJ "
        End If
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcRepairAdvance()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "REPAIRADV') DROP TABLE TEMP" & systemId & "REPAIRADV "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE 0 END)  AS RECEIPT, 0 AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "REPAIRADV FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('OR','AR') AND O.RECPAY = 'R' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += " AND SUBSTRING(O.RUNNO,6,1) = 'R'"
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "REPAIRADV)>0 "
        StrSql += " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) "
            StrSql += " SELECT 'REPAIR ADVANCE RECEIVED ['+CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),''))+']','T'"
            StrSql += " FROM TEMP" & systemId & "REPAIRADV "
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  ' + CATNAME CATNAME, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += " 1 RESULT1, COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "REPAIRADV "
            If rbtAmount.Checked = True Then
                StrSql += " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += " SELECT 'REPAIR ADVANCE RECEIVED','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += " FROM TEMP" & systemId & "REPAIRADV "
        End If
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcRepairAdvanceAdjusted()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "REPAIRADVADJ') DROP TABLE TEMP" & systemId & "REPAIRADVADJ "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END)  AS PAYMENT, 0 AS RECEIPT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "REPAIRADVADJ FROM " & cnAdminDb & "..OUTSTANDING AS O "

        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('AA','AP') AND O.RECPAY = 'P' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += " AND SUBSTRING(O.RUNNO,6,1) = 'R'"
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "REPAIRADVADJ)>0 "
        StrSql += " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) "
            StrSql += " SELECT 'REPAIR ADVANCE ADJUSTED ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
            StrSql += " FROM TEMP" & systemId & "REPAIRADVADJ "
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  ' + CATNAME CATNAME, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += " 1 RESULT1, COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "REPAIRADVADJ "
            If rbtAmount.Checked = True Then
                StrSql += " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += " SELECT 'REPAIR ADVANCE ADJUSTED','T',SUM(PAYMENT)-SUM(RECEIPT)"
            StrSql += " FROM TEMP" & systemId & "REPAIRADVADJ "
        End If
        StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION, COLHEAD) "
        StrSql += " SELECT 'REPAIR ADVANCE ADJUSTED ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
        StrSql += " FROM TEMP" & systemId & "REPAIRADVADJ "
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += " SELECT '  ' + CATNAME CATNAME, "
        StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += " 1 RESULT1, COLHEAD  "
        StrSql += " FROM TEMP" & systemId & "REPAIRADVADJ "
        If rbtAmount.Checked = True Then
            StrSql += " ORDER BY RESULT1,RECEIPT "
        Else
            StrSql += " ORDER BY RESULT1,CATNAME "
        End If
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcOrderAdvance()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ORDERADV') DROP TABLE TEMP" & systemId & "ORDERADV "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE 0 END)  AS RECEIPT, 0 AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ORDERADV FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('OR','AR') AND O.RECPAY = 'R' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += " AND SUBSTRING(O.RUNNO,6,1) <> 'R'"
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ORDERADV)>0 "
        StrSql += " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) "
            StrSql += " SELECT 'ORDER ADVANCE RECEIVED ['+CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),''))+']','T'"
            StrSql += " FROM TEMP" & systemId & "ORDERADV "
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  ' + CATNAME CATNAME, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += " 1 RESULT1, COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "ORDERADV "
            If rbtAmount.Checked = True Then
                StrSql += " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += " SELECT 'ORDER ADVANCE RECEIVED','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += " FROM TEMP" & systemId & "ORDERADV "
        End If
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcOrderAdvanceAdjusted()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ORDERADVADJ') DROP TABLE TEMP" & systemId & "ORDERADVADJ "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END)  AS PAYMENT, 0 AS RECEIPT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ORDERADVADJ FROM " & cnAdminDb & "..OUTSTANDING AS O "

        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('AA','AP') AND O.RECPAY = 'P' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += " AND SUBSTRING(O.RUNNO,6,1) <> 'R'"
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ORDERADVADJ)>0 "
        StrSql += " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) "
            StrSql += " SELECT 'ORDER ADVANCE ADJUSTED ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
            StrSql += " FROM TEMP" & systemId & "ORDERADVADJ "
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  ' + CATNAME CATNAME, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += " 1 RESULT1, COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "ORDERADVADJ "
            If rbtAmount.Checked = True Then
                StrSql += " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += " SELECT 'ORDER ADVANCE ADJUSTED','T',SUM(PAYMENT)-SUM(RECEIPT)"
            StrSql += " FROM TEMP" & systemId & "ORDERADVADJ "
        End If
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcMiscReceipt()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "MISCRECEIPTS') DROP TABLE TEMP" & systemId & "MISCRECEIPTS "
        StrSql += vbCrLf + " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += " CASE WHEN ISNULL(O.REMARK1,'') <> '' THEN '('+O.REMARK1+')' ELSE '' END AS CATNAME,"
        'strsql += vbcrlf + " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += vbCrLf + " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT, "
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMP" & systemId & "MISCRECEIPTS FROM " & cnAdminDb & "..OUTSTANDING O"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND PAYMODE IN ('MR') AND O.RECPAY = 'R'"
        StrSql += StrFilter
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        StrSql += vbCrLf + " ,O.REMARK1"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If chkCashOpening.Checked Then
            StrSql = " IF (sELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCASHOPEN')>0 DROP TABLE TEMPCASHOPEN"
            StrSql += vbCrLf + " DECLARE @CASHID VARCHAR(7)"
            StrSql += vbCrLf + " SELECT @CASHID = CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'"
            StrSql += vbCrLf + " SELECT SUM(ISNULL(AMOUNT,0)) AS AMOUNT"
            StrSql += vbCrLf + " INTO TEMPCASHOPEN"
            StrSql += vbCrLf + " FROM"
            StrSql += vbCrLf + "     ("
            StrSql += vbCrLf + "     SELECT DEBIT-CREDIT AS AMOUNT FROM " & cnStockDb & "..OPENTRAILBALANCE"
            StrSql += vbCrLf + "     WHERE ACCODE = @CASHID"
            StrSql += vbCrLf + "     AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += StrCostFiltration
            StrSql += vbCrLf + "     UNION ALL"
            StrSql += vbCrLf + "     SELECT CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT"
            StrSql += vbCrLf + "     FROM " & cnStockDb & "..ACCTRAN A"
            StrSql += vbCrLf + "     WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + "     AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += vbCrLf + "     AND ISNULL(A.CANCEL,'') = ''"
            StrSql += StrFilter 'StrCostFiltration
            StrSql += vbCrLf + "     AND PAYMODE IN ('CA')"
            StrSql += vbCrLf + "     AND ACCODE = @CASHID"
            StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
            If Not hasChit Then StrSql += " AND FROMFLAG <> 'C'"
            StrSql += vbCrLf + "     )X"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            'StrSql = " IF (SELECT COUNT(*) FROM TEMPCASHOPEN)>0"
            'strsql += vbcrlf + " BEGIN"
            'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            'StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            'StrSql += vbCrLf + " SELECT 'CASH OPENING..' AS CATNAME, "
            'StrSql += vbCrLf + " CASE WHEN AMOUNT  > 0 THEN AMOUNT  ELSE NULL END RECEIPT, "
            'StrSql += vbCrLf + " CASE WHEN AMOUNT  < 0 THEN AMOUNT  ELSE NULL END PAYMENT, "
            'StrSql += vbCrLf + " 1 RESULT1, 'T'  "
            'StrSql += vbCrLf + " FROM TEMPCASHOPEN"
            'strsql += vbcrlf + " END"
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'Cmd.ExecuteNonQuery()
        End If


        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "MISCRECEIPTS)>0 "
        If chkCashOpening.Checked Then
            StrSql += vbCrLf + " OR (SELECT COUNT(*) FROM TEMPCASHOPEN)>0"
        End If
        StrSql += vbCrLf + " BEGIN "

        If chkCashOpening.Checked Then
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT 'CASH OPENING..'CATNAME, "
            StrSql += vbCrLf + " CASE WHEN AMOUNT  > 0 THEN AMOUNT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN AMOUNT  < 0 THEN AMOUNT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1,'T' COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPCASHOPEN"
        End If

        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) "
            StrSql += vbCrLf + " SELECT 'MISC RECEIPTS ['+ISNULL(CONVERT(VARCHAR,SUM(RECEIPT-PAYMENT)),'')+']','T'"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "MISCRECEIPTS"

            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " FROM TEMP" & systemId & "MISCRECEIPTS ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " FROM TEMP" & systemId & "MISCRECEIPTS ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += vbCrLf + " SELECT 'MISC RECEIPTS','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "MISCRECEIPTS"
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcChitPayment()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITPAYMENT') DROP TABLE TEMP" & systemId & "CHITPAYMENT "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += " '(GRP:' + CHQCARDREF + ' REGNO: ' + CHQCARDNO + ')' AS CATNAME,  "
        StrSql += " 0 AS RECEIPT, SUM(CASE WHEN TRANMODE = 'D' THEN O.AMOUNT ELSE -1*AMOUNT END) AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "CHITPAYMENT FROM " & cnStockDb & "..ACCTRAN O"
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('HP','HG','HZ','HB','HD')"
        StrSql += StrFilter
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.CHQCARDREF,O.CHQCARDNO"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "CHITPAYMENT)>0 "
        StrSql += " BEGIN "
        StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION, COLHEAD) "
        StrSql += " SELECT 'SCHEME PAYMENTS ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
        StrSql += " FROM TEMP" & systemId & "CHITPAYMENT"
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += " SELECT '  ' + CATNAME CATNAME, "
        StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += " 1 RESULT1, COLHEAD  "
        If rbtAmount.Checked = True Then
            StrSql += " FROM TEMP" & systemId & "CHITPAYMENT ORDER BY RESULT1,PAYMENT "
        Else
            StrSql += " FROM TEMP" & systemId & "CHITPAYMENT ORDER BY RESULT1,CATNAME "
        End If
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcMiscPayment()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "MISCPAYMENT') DROP TABLE TEMP" & systemId & "MISCPAYMENT "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += " CASE WHEN ISNULL(O.REMARK1,'') <> '' THEN '('+O.REMARK1+')' ELSE '' END AS CATNAME,"
        'StrSql += " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END) AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "MISCPAYMENT FROM " & cnAdminDb & "..OUTSTANDING O"
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('MP') AND O.RECPAY = 'P'"
        StrSql += StrFilter
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        StrSql += " ,O.REMARK1"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "MISCPAYMENT)>0 "
        StrSql += " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) "
            StrSql += " SELECT 'MISC PAYMENTS ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
            StrSql += " FROM TEMP" & systemId & "MISCPAYMENT"
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  ' + CATNAME CATNAME, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += " 1 RESULT1, COLHEAD  "
            If rbtAmount.Checked = True Then
                StrSql += " FROM TEMP" & systemId & "MISCPAYMENT ORDER BY RESULT1,PAYMENT "
            Else
                StrSql += " FROM TEMP" & systemId & "MISCPAYMENT ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += " SELECT 'MISC PAYMENTS','T',SUM(PAYMENT)-SUM(RECEIPT)"
            StrSql += " FROM TEMP" & systemId & "MISCPAYMENT"
        End If
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcBeeds()
        If chkSeperateBeeds.Checked = False Then Exit Sub
        StrSql = " SELECT "
        StrSql += " SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) AS GRSWT "
        StrSql += " FROM " & cnStockDb & "..ISSSTONE AS ISS WHERE "
        StrSql += vbCrLf + " ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE AS I  "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        StrSql += vbCrLf + " I.TRANTYPE IN('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
        StrSql += StrFilter
        StrSql += vbCrLf + "          )AND "
        StrSql += vbCrLf + "          (EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = ISS.STNITEMID AND ISNULL(BEEDS,'') = 'Y' AND ISS.STNSUBITEMID = 0)"
        StrSql += vbCrLf + "          OR EXISTS (SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = ISS.STNITEMID AND SUBITEMID = ISS.STNSUBITEMID AND ISNULL(BEEDS,'') = 'Y' AND ISS.STNSUBITEMID <> 0)"
        StrSql += vbCrLf + "          )"
        Dim sBeeds As Decimal = Val(objGPack.GetSqlValue(StrSql))

        StrSql = " SELECT "
        StrSql += " SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) AS GRSWT "
        StrSql += " FROM " & cnStockDb & "..RECEIPTSTONE AS ISS WHERE "
        StrSql += vbCrLf + "  ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT R"
        StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + "  TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += vbCrLf + "          )AND "
        StrSql += vbCrLf + "          (EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = ISS.STNITEMID AND ISNULL(BEEDS,'') = 'Y' AND ISS.STNSUBITEMID = 0)"
        StrSql += vbCrLf + "          OR EXISTS (SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = ISS.STNITEMID AND SUBITEMID = ISS.STNSUBITEMID AND ISNULL(BEEDS,'') = 'Y' AND ISS.STNSUBITEMID <> 0)"
        StrSql += vbCrLf + "          )"
        Dim rBeeds As Decimal = Val(objGPack.GetSqlValue(StrSql))

        If sBeeds <> 0 Or rBeeds <> 0 Then
            StrSql = " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,ISSGRSWT,RECGRSWT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  BEEDS' CATNAME,  "
            StrSql += " " & sBeeds & "," & rBeeds & ""
            StrSql += " ,1 RESULT1,'T'COLHEAD  "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub ProcDiscount()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "DISCOUNT') DROP TABLE TEMP" & systemId & "DISCOUNT "
        StrSql += " SELECT 'DISCOUNT' CATNAME,  "
        StrSql += " SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END) AS AMT, "
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "DISCOUNT FROM " & cnStockDb & "..ACCTRAN AS A  "
        StrSql += " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
        StrSql += " AND PAYMODE = 'DI'   AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END),0) <> 0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "DISCOUNT)>0 "
        StrSql += " BEGIN "
        'StrSql += " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += " SELECT '  ' + CATNAME CATNAME,  "
        StrSql += " CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT,  "
        StrSql += " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT, "
        StrSql += " 1 RESULT1, COLHEAD  "
        StrSql += " FROM TEMP" & systemId & "DISCOUNT ORDER BY CATNAME  "
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcHandling()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += " NAME = 'TEMP" & systemId & "HANDLING') DROP TABLE TEMP" & systemId & "HANDLING "
        StrSql += " SELECT 'HANDLING CHARGES' CATNAME,  "
        StrSql += " SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END) AS AMT, "
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "HANDLING FROM " & cnStockDb & "..ACCTRAN AS A  "
        StrSql += " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
        StrSql += " AND PAYMODE = 'HC'   AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END),0) <> 0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "HANDLING)>0 "
        StrSql += " BEGIN "
        'StrSql += " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += " SELECT '  ' + CATNAME CATNAME,  "
        StrSql += " CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT,  "
        StrSql += " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT, "
        StrSql += " 1 RESULT1, COLHEAD  "
        StrSql += " FROM TEMP" & systemId & "HANDLING ORDER BY CATNAME  "
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcroundOff()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ROUNDOFF') DROP TABLE TEMP" & systemId & "ROUNDOFF "
        StrSql += " SELECT 'ROUNDOFF' CATNAME, "
        StrSql += " SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END) AS AMT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ROUNDOFF FROM " & cnStockDb & "..ACCTRAN AS A "
        StrSql += " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
        StrSql += " AND PAYMODE = 'RO'  AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END),0) <> 0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ROUNDOFF)>0 "
        StrSql += " BEGIN "
        'StrSql += " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += " SELECT '  ' + CATNAME CATNAME, "
        StrSql += " CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT, "
        StrSql += " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT, "
        StrSql += " 1 RESULT1, COLHEAD  "
        StrSql += " FROM TEMP" & systemId & "ROUNDOFF ORDER BY CATNAME "
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub


    Private Sub ProcWtSubtot()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "WTSTOT') DROP TABLE TEMP" & systemId & "WTSTOT "
        StrSql += " SELECT * INTO TEMP" & systemId & "WTSTOT  FROM ("
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABSSALES WHERE ISNULL(RESULT1,1) NOT IN (0,2) " 'AND ISNULL(COLHEAD,'') <> 'D'
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABSREPAIR WHERE ISNULL(RESULT1,1) NOT IN (0,2) AND ISNULL(COLHEAD,'') <> 'D'"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABSSALESRETURN WHERE ISNULL(RESULT1,1) NOT IN (0,2) " 'AND ISNULL(COLHEAD,'') <> 'D'"
        StrSql += " UNION ALL"
        StrSql += " SELECT 0 RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABS_ORDAMTWT"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABSPURCHASE WHERE ISNULL(RESULT1,1) NOT IN (0,2)  " 'AND ISNULL(COLHEAD,'') <> 'D'"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ADVADJ"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ADVREC"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABSCREDSAL"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABSCREDPUR"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABSCREDADJ"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABSCREDPURPAY"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "CHITPAYMENT"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "MISCRECEIPTS"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "MISCPAYMENT"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ORDERADV"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ORDERADVADJ"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "REPAIRADV"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "REPAIRADVADJ"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "CHITCOLLECTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT, "
        StrSql += " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT  FROM TEMP" & systemId & "DISCOUNT"
        StrSql += " UNION ALL"
        StrSql += " SELECT CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT, "
        StrSql += " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT  FROM TEMP" & systemId & "HANDLING"
        StrSql += " UNION ALL"
        StrSql += " SELECT CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT, "
        StrSql += " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT  FROM TEMP" & systemId & "ROUNDOFF"
        If chkCashOpening.Checked Then
            StrSql += " UNION ALL"
            StrSql += " SELECT CASE WHEN AMOUNT > 0 THEN AMOUNT ELSE 0 END RECEIPT"
            StrSql += " ,CASE WHEN AMOUNT < 0 THEN AMOUNT ELSE 0 END PAYMENT FROM TEMPCASHOPEN"
        End If
        StrSql += " )X "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "WTSTOT)>0 "
        StrSql += " BEGIN "
        StrSql += "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "WTSTOTAL') DROP TABLE TEMP" & systemId & "WTSTOTAL "
        StrSql += " SELECT SUM(RECEIPT) AS RECEIPT, SUM(PAYMENT) AS PAYMENT INTO TEMP" & systemId
        StrSql += "WTSTOTAL FROM TEMP" & systemId & "WTSTOT  END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        DT = New DataTable
        StrSql = " SELECT * FROM TEMP" & systemId & "WTSTOT"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(DT)
        If DT.Rows.Count <= 0 Then Exit Sub

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "WTSTOTAL)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,COLHEAD) "
        StrSql += " SELECT 'TOTAL', RECEIPT, PAYMENT,'S' FROM TEMP" & systemId & "WTSTOTAL"
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "WTSTOTAL)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,COLHEAD) "
        StrSql += " SELECT 'GRAND TOTAL', "
        StrSql += " ISNULL((CASE WHEN X>0 THEN X END),NULL) AS RECEIPT,"
        StrSql += " ISNULL((CASE WHEN X<0 THEN X END),NULL) AS PAYMENT, 'G'"
        StrSql += " FROM ("
        StrSql += " SELECT RECEIPT-PAYMENT  AS X FROM TEMP" & systemId & "WTSTOTAL) Y"
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcChitCollection()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITCOLLECTION') DROP TABLE TEMP" & systemId & "CHITCOLLECTION "
        StrSql += VBCRLF + " SELECT CONVERT(NUMERIC(15,2),0)RECEIPT,CONVERT(NUMERIC(15,2),0)PAYMENT"
        StrSql += VBCRLF + " INTO TEMP" & systemId & "CHITCOLLECTION"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If hasChit Then
            StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CHITDBPREFIX'"
            If (Trim(objGPack.GetSqlValue(StrSql, , ""))) <> "" Then
                StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITCOLLECTION') DROP TABLE TEMP" & systemId & "CHITCOLLECTION "
                StrSql += vbCrLf + " SELECT  CASE "
                StrSql += vbCrLf + " WHEN MODEPAY = 'C' THEN 'SCHEME CASH' "
                StrSql += vbCrLf + " WHEN MODEPAY IN('Q','D') THEN 'CHEQUE' "
                StrSql += vbCrLf + " WHEN MODEPAY = 'R' THEN 'CREDITCARD'"
                StrSql += vbCrLf + " WHEN MODEPAY = 'E' THEN 'ETRANSFER'"
                StrSql += vbCrLf + " WHEN MODEPAY = 'O' THEN 'OTHERS'"
                StrSql += vbCrLf + " END CATNAME,"
                StrSql += vbCrLf + " SUM(AMOUNT) AS RECEIPT, 0 AS PAYMENT "
                StrSql += vbCrLf + " INTO TEMP" & systemId & "CHITCOLLECTION FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                StrSql += StrCostFiltration
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))AND T.GROUPCODE = SC.GROUPCODE AND T.REGNO = SC.REGNO)"
                StrSql += vbCrLf + " GROUP BY MODEPAY"
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT CATNAME,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT  FROM"
                StrSql += vbCrLf + " (SELECT  'CC COMMISION' CATNAME,"
                StrSql += vbCrLf + " (SELECT SUM(AMOUNT) FROM " & cnChitCompanyid & "SAVINGS..RECPAY WHERE T.ENTREFNO = EntRefNo) AS RECEIPT, 0 AS PAYMENT "
                StrSql += vbCrLf + " FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND MODEPAY='R' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                StrSql += StrCostFiltration
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))AND T.GROUPCODE = SC.GROUPCODE AND T.REGNO = SC.REGNO))X"
                StrSql += vbCrLf + " group by X.CATNAME "

                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()

                StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "CHITCOLLECTION)>0 "
                StrSql += vbCrLf + " BEGIN "
                StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
                StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION,COLHEAD) "
                'StrSql += VBCRLF + " VALUES('SCHEME COLLECTION ','T') "
                StrSql += vbCrLf + " SELECT 'SCHEME COLLECTION [' +  CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),SUM(AMOUNT))) + ']' AS PARTICULAR,'T' FROM " & cnChitTrandb & "..SCHEMETRAN "
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                StrSql += StrCostFiltration
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"

                StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) "
                StrSql += vbCrLf + " SELECT PARTICULAR FROM "
                StrSql += vbCrLf + " ("
                StrSql += vbCrLf + " SELECT 'NEW : Rs '+ CASE WHEN SUM(AMOUNT) > 0 THEN CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),SUM(AMOUNT))) + ' ('+CONVERT(VARCHAR,COUNT(*)) + ' NOs)' ELSE '' END AS PARTICULAR FROM " & cnChitTrandb & "..SCHEMETRAN "
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                StrSql += StrCostFiltration
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"
                StrSql += vbCrLf + " AND INSTALLMENT = 1"
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT 'OTHER : Rs '+ CASE WHEN SUM(AMOUNT) > 0 THEN CONVERT(VARCHAR,ISNULL(CONVERT(NUMERIC(15,2),SUM(AMOUNT)),'')) + ' ('+CONVERT(VARCHAR,ISNULL(COUNT(*),'')) + ' NOs)' ELSE '' END AS PARTICULAR FROM " & cnChitTrandb & "..SCHEMETRAN"
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                StrSql += StrCostFiltration
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"
                StrSql += vbCrLf + " AND INSTALLMENT <> 1"
                StrSql += vbCrLf + " )C"

                StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
                StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME,  "
                StrSql += vbCrLf + " CASE WHEN RECEIPT<> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
                StrSql += vbCrLf + " CASE WHEN PAYMENT<> 0 THEN PAYMENT  ELSE NULL END PAYMENT "
                StrSql += vbCrLf + " FROM TEMP" & systemId & "CHITCOLLECTION ORDER BY CATNAME "
                StrSql += vbCrLf + " END"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()

            End If
        End If
    End Sub

    Private Sub ProcOrderAdvanceAmountToWeight()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABS_ORDAMTWT') DROP TABLE TEMP" & systemId & "ABS_ORDAMTWT "
        StrSql += " SELECT TRANNO,'REF NO : (' + ISNULL((SELECT TOP 1 SUBSTRING(ORNO,6,20) FROM " & cnAdminDb & "..ORMAST WHERE BATCHNO = I.BATCHNO),'') + ') ' + CONVERT(VARCHAR,GRSWT) + '@' + CONVERT(VARCHAR,RATE) AS CATNAME"
        StrSql += " ,PCS AS RECPCS, GRSWT AS RECGRSWT, NETWT AS RECNETWT,AMOUNT PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABS_ORDAMTWT FROM " & cnStockDb & "..RECEIPT I"
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " AND I.TRANTYPE = 'AD' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABS_ORDAMTWT)>0 "
        StrSql += " BEGIN "
        StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION, COLHEAD) VALUES('ORDER ADV REC AMT TO WT','T') "
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,RECPCS,RECGRSWT,RECNETWT,RECEIPT,RESULT1,COLHEAD) "
        StrSql += " SELECT '  ' + CATNAME CATNAME, "
        StrSql += " CASE WHEN RECPCS    <> 0 THEN RECPCS    ELSE NULL END RECPCS, "
        StrSql += " CASE WHEN RECGRSWT  <> 0 THEN RECGRSWT  ELSE NULL END RECGRSWT, "
        StrSql += " CASE WHEN RECNETWT  <> 0 THEN RECNETWT  ELSE NULL END RECNETWT, "
        StrSql += " PAYMENT,"
        StrSql += " 1 RESULT1, COLHEAD  "
        StrSql += " FROM TEMP" & systemId & "ABS_ORDAMTWT ORDER BY RESULT1,TRANNO "
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


    End Sub

    Private Sub ProcApprovalIssue()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSAPPROVALISS') DROP TABLE TEMP" & systemId & "ABSAPPROVALISS "
        StrSql += " SELECT TRANNO, (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        StrSql += " WHERE ITEMID = I.ITEMID)+CASE WHEN ISNULL(TAGNO,'') <> '' THEN ' - TAGNO('+TAGNO+')' ELSE '' END AS CATNAME, "
        StrSql += " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSAPPROVALISS FROM " & cnStockDb & "..ISSUE I"
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " AND I.TRANTYPE = 'AI' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += " GROUP BY ITEMID,TAGNO,TRANNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        If chkDetaledApproval.Checked = True Then
            StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSAPPROVALISS)>0 "
            StrSql += " BEGIN "
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) VALUES('APPROVAL ISSUE','T') "
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  ' + CATNAME CATNAME, "
            StrSql += " CASE WHEN ISSPCS    <> 0 THEN ISSPCS    ELSE NULL END ISSPCS, "
            StrSql += " CASE WHEN ISSGRSWT  <> 0 THEN ISSGRSWT  ELSE NULL END ISSGRSWT, "
            StrSql += " CASE WHEN ISSNETWT  <> 0 THEN ISSNETWT  ELSE NULL END ISSNETWT, "
            StrSql += " 1 RESULT1, COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "ABSAPPROVALISS ORDER BY RESULT1,TRANNO "
            StrSql += " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSAPPROVALISS)>0 "
            StrSql += " BEGIN "
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD) "
            StrSql += " SELECT 'APPROVAL ISSUE', "
            StrSql += " CASE WHEN SUM(ISSPCS)    <> 0 THEN SUM(ISSPCS)    ELSE NULL END ISSPCS, "
            StrSql += " CASE WHEN sum(ISSGRSWT)  <> 0 THEN sum(ISSGRSWT)  ELSE NULL END ISSGRSWT, "
            StrSql += " CASE WHEN sum(ISSNETWT)  <> 0 THEN sum(ISSNETWT)  ELSE NULL END ISSNETWT, "
            StrSql += " 1 RESULT1, 'T' COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "ABSAPPROVALISS"
            StrSql += " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub ProcApprovalReceipt()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSAPPROVALREC') DROP TABLE TEMP" & systemId & "ABSAPPROVALREC "
        StrSql += " SELECT TRANNO, (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        StrSql += " WHERE ITEMID = I.ITEMID)+CASE WHEN ISNULL(TAGNO,'') <> '' THEN ' - TAGNO('+TAGNO+')' ELSE '' END + CASE WHEN ISNULL(RUNNO,'') <> '' THEN ' [RUNNO : ' +  SUBSTRING(RUNNO,6,20) + ']' ELSE '' END AS CATNAME, "
        StrSql += " SUM(PCS) AS RECPCS, SUM(GRSWT) AS RECGRSWT, SUM(NETWT) AS RECNETWT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSAPPROVALREC FROM " & cnStockDb & "..RECEIPT I"
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " AND I.TRANTYPE = 'AR' AND ISNULL(CANCEL,'') = ''"
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += " GROUP BY ITEMID,TAGNO,TRANNO,RUNNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        If chkDetaledApproval.Checked = True Then
            StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSAPPROVALREC)>0 "
            StrSql += " BEGIN "
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) VALUES('APPROVAL RECEIPT','T') "
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RECPCS,RECGRSWT,RECNETWT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  ' + CATNAME CATNAME, "
            StrSql += " CASE WHEN RECPCS    <> 0 THEN RECPCS    ELSE NULL END RECPCS, "
            StrSql += " CASE WHEN RECGRSWT  <> 0 THEN RECGRSWT  ELSE NULL END RECGRSWT, "
            StrSql += " CASE WHEN RECNETWT  <> 0 THEN RECNETWT  ELSE NULL END RECNETWT, "
            StrSql += " 1 RESULT1, COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "ABSAPPROVALREC ORDER BY RESULT1,TRANNO "
            StrSql += " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSAPPROVALREC)>0 "
            StrSql += " BEGIN "
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RECPCS,RECGRSWT,RECNETWT,RESULT1,COLHEAD) "
            StrSql += " SELECT 'APPROVAL RECEIPT', "
            StrSql += " CASE WHEN SUM(RECPCS)    <> 0 THEN SUM(RECPCS)    ELSE NULL END RECPCS, "
            StrSql += " CASE WHEN SUM(RECGRSWT)  <> 0 THEN SUM(RECGRSWT)  ELSE NULL END RECGRSWT, "
            StrSql += " CASE WHEN SUM(RECNETWT)  <> 0 THEN SUM(RECNETWT)  ELSE NULL END RECNETWT, "
            StrSql += " 1 RESULT1, 'T' COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "ABSAPPROVALREC "
            StrSql += " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub ProcHomeSales()
        If Not chkHomeSale.Checked Then Exit Sub
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSHOMESALE') DROP TABLE TEMP" & systemId & "ABSHOMESALE "
        ''StrSql += " SELECT ' BILLNO-'+CONVERT(VARCHAR(5),TRANNO) "
        ''StrSql += " AS CATNAME, "
        ''StrSql += " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT,  "
        ''StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        ''StrSql += " INTO TEMP" & systemId & "ABSHOMESALE FROM " & cnStockDb & "..ISSUE I  "
        ''StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'  AND ' "
        ''StrSql += dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        ''StrSql += " AND  TRANTYPE = 'SA' AND ISNULL(I.CANCEL,'') = ''  "
        ''StrSql += " AND ISNULL(FLAG,'') IN ('C', 'B')"
        ''StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        ''StrSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(STOCKTYPE,'') = 'T')"
        ''StrSql += " GROUP BY BATCHNO,TRANNO,TRANDATE  "
        StrSql += " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)CATNAME/**/"
        StrSql += " ,SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT,/**/"
        StrSql += " 1 AS RESULT1, CONVERT(VARCHAR,'') AS COLHEAD/**/"
        StrSql += " INTO TEMP" & systemId & "ABSHOMESALE FROM " & cnStockDb & "..ISSUE I/**/"
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'/**/"
        StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND  TRANTYPE = 'SA'/**/"
        StrSql += " AND ISNULL(I.CANCEL,'') = ''   AND ISNULL(FLAG,'') IN ('C', 'B') AND COMPANYID IN (" & SelectedCompanyId & ")/**/"
        StrSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(STOCKTYPE,'') = 'T')/**/"
        StrSql += " AND ISNULL(TAGNO,'') = ''/**/ " + StrFilter
        StrSql += " GROUP BY CATCODE/**/"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSHOMESALE)>0 "
        StrSql += " BEGIN "
        StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION, COLHEAD) VALUES('HOME SALES','T') "
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD) "
        StrSql += " SELECT '  ' + CATNAME CATNAME, "
        StrSql += " CASE WHEN ISSPCS    <> 0 THEN ISSPCS    ELSE NULL END ISSPCS, "
        StrSql += " CASE WHEN ISSGRSWT  <> 0 THEN ISSGRSWT  ELSE NULL END ISSGRSWT, "
        StrSql += " CASE WHEN ISSNETWT  <> 0 THEN ISSNETWT  ELSE NULL END ISSNETWT, "
        StrSql += " 1 RESULT1, COLHEAD  "
        StrSql += " FROM TEMP" & systemId & "ABSHOMESALE ORDER BY RESULT1"
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcPartlySales()
        If (RPT_ME_SUM_DABS = True And rbtMetalwise.Checked = True) Then Exit Sub
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPARTLYSALE') DROP TABLE TEMP" & systemId & "ABSPARTLYSALE "
        ''StrSql += " SELECT CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD,STNPCS,STNWT"
        ''StrSql += " INTO TEMP" & systemId & "ABSPARTLYSALE FROM ("
        ''If chkCatShortname.Checked = True Then
        ''    StrSql += " SELECT (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
        ''Else
        ''    StrSql += " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        ''End If
        ''StrSql += " WHERE CATCODE = I.CATCODE)AS CATNAME  "
        ''StrSql += " ,SUM(I.PCS) AS ISSPCS, SUM(I.GRSWT) AS ISSGRSWT"
        ''StrSql += " ,SUM(I.NETWT) AS ISSNETWT,   1 AS RESULT1, ' ' AS COLHEAD  "
        ''StrSql += " ,(SELECT ISNULL(SUM(STNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNPCS"
        ''StrSql += " ,(SELECT ISNULL(SUM(STNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNWT"
        ''StrSql += " ,(SELECT ISNULL(SUM(TAGSTNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNPCS"
        ''StrSql += " ,(SELECT ISNULL(SUM(TAGSTNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNWT"
        ''StrSql += " ,SUM(TAGPCS) TAGPCS, SUM(TAGGRSWT) TAGGRSWT,SUM(TAGNETWT) TAGNETWT"
        ''StrSql += " FROM " & cnStockDb & "..ISSUE I"
        ''StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        ''StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        ''StrSql += " AND  I.TRANTYPE = 'SA' "
        ''StrSql += " AND ISNULL(I.CANCEL,'') = ''   AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        ''StrSql += " AND ISNULL(TAGNO,'') <> ''"
        ''StrSql += " GROUP BY CATCODE,TRANNO,SNO"
        StrSql += " SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
        StrSql += " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSPARTLYSALE FROM ("
        If chkCatShortname.Checked = True Then
            StrSql += " SELECT (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
        Else
            StrSql += " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        End If
        StrSql += " WHERE CATCODE = I.CATCODE)AS CATNAME   "
        StrSql += " ,I.PCS AS ISSPCS, I.GRSWT AS ISSGRSWT, I.NETWT AS ISSNETWT "
        StrSql += " ,(SELECT ISNULL(SUM(STNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNPCS "
        StrSql += " ,(SELECT ISNULL(SUM(TAGSTNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNPCS "
        StrSql += " ,(SELECT ISNULL(SUM(STNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO )STNWT "
        StrSql += " ,(SELECT ISNULL(SUM(TAGSTNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNWT "
        StrSql += " ,TAGPCS,TAGGRSWT,TAGNETWT FROM " & cnStockDb & "..ISSUE I "
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
        StrSql += " AND I.TAGNO <> ''"
        StrSql += " AND ISNULL(I.CANCEL,'') = ''   "
        StrSql += " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += StrFilter
        StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
        StrSql += " ) X "
        StrSql += " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
        StrSql += " ) GROUP BY CATNAME"
        StrSql += " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        'StrSql += " WHERE CATCODE = I.CATCODE)AS CATNAME, "
        'StrSql += " SUM(I.PCS) AS ISSPCS, SUM(I.GRSWT) AS ISSGRSWT, SUM(I.NETWT) AS ISSNETWT,  "
        'StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        'StrSql += " INTO TEMP" & systemId & "ABSPARTLYSALE FROM "
        'StrSql += cnStockDb & "..ISSUE I, " & cnAdminDb & "..ITEMTAG T "
        'StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        'StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        'StrSql += " AND  I.TRANTYPE = 'SA' AND ISNULL(I.CANCEL,'') = ''  "
        'StrSql += " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        'StrSql += " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO AND"
        'StrSql += " ((T.GRSWT-I.GRSWT)<> 0 OR (T.NETWT-I.NETWT)<>0)"
        'StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        'StrSql += StrFilter
        'StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
        'StrSql += " GROUP BY CATCODE"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPARTLYSALE)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION, COLHEAD) VALUES('PARTLY SALES','T') "
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD) "
        StrSql += " SELECT '  ' + CATNAME CATNAME, "
        StrSql += " CASE WHEN ISSPCS    <> 0 THEN ISSPCS    ELSE NULL END ISSPCS, "
        StrSql += " CASE WHEN ISSGRSWT  <> 0 THEN ISSGRSWT  ELSE NULL END ISSGRSWT, "
        StrSql += " CASE WHEN ISSNETWT  <> 0 THEN ISSNETWT  ELSE NULL END ISSNETWT, "
        StrSql += " 1 RESULT1, COLHEAD  "
        StrSql += " FROM TEMP" & systemId & "ABSPARTLYSALE ORDER BY RESULT1 "
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


    End Sub

    Private Sub ProcCollection()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "COLDET') DROP TABLE TEMP" & systemId & "COLDET "
        StrSql += " DECLARE @CASHID VARCHAR(7)"
        StrSql += " SELECT @CASHID = CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'"
        StrSql += vbCrLf + " SELECT CONVERT(VARCHAR(50),(CASE "
        StrSql += vbCrLf + " WHEN PAYMODE='CA' THEN 'CASH' "
        strsql += vbcrlf + " WHEN PAYMODE = 'CC' THEN 'CREDIT CARD' "
        strsql += vbcrlf + " WHEN PAYMODE = 'CH' THEN 'CHEQUE' "
        StrSql += vbCrLf + " WHEN PAYMODE = 'SS' THEN 'SCHEME' "
        StrSql += vbCrLf + " WHEN PAYMODE = 'GV' THEN 'GIFT VOUCHER' "
        strsql += vbcrlf + " WHEN PAYMODE = 'ET' THEN 'ETRANSFER' "
        strsql += vbcrlf + " WHEN PAYMODE = 'OT' THEN 'OTHERS' "
        StrSql += vbCrLf + " END)) AS CATNAME,"
        strsql += vbcrlf + " (CASE WHEN SUM(AMOUNT)> 0 THEN  SUM(AMOUNT) ELSE 0 END) AS PAYMENT, "
        strsql += vbcrlf + " (CASE WHEN SUM(AMOUNT)< 0 THEN  ABS(SUM(AMOUNT)) ELSE 0 END) AS RECEIPT "
        strsql += vbcrlf + " INTO TEMP" & systemId & "COLDET FROM ("
        strsql += vbcrlf + " SELECT PAYMODE,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END)) AS AMOUNT"
        strsql += vbcrlf + " FROM " & cnStockDb & "..ACCTRAN A"
        strsql += vbcrlf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strsql += vbcrlf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not hasChit Then StrSql += " AND FROMFLAG <> 'C'"
        'StrSql += "  AND GRSWT = 0"
        StrSql += StrFilter
        StrSql += vbCrLf + " AND PAYMODE IN ('CC','CH','GV')"
        strsql += vbcrlf + " GROUP BY TRANMODE,PAYMODE"
        strsql += vbcrlf + " UNION ALL "
        strsql += vbcrlf + " SELECT 'SS' PAYMODE,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END)) AS AMOUNT"
        strsql += vbcrlf + " FROM " & cnStockDb & "..ACCTRAN A"
        strsql += vbcrlf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strsql += vbcrlf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        strsql += vbcrlf + " AND ISNULL(A.CANCEL,'') = ''"
        StrSql += StrFilter
        StrSql += vbCrLf + " AND PAYMODE IN ('SS','CB','CZ','CG','CD')"
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not hasChit Then StrSql += " AND FROMFLAG <> 'C'"
        StrSql += vbCrLf + " GROUP BY TRANMODE"

        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT 'CA' PAYMODE,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END)) AS AMOUNT"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
        StrSql += StrFilter
        StrSql += vbCrLf + " AND PAYMODE IN ('CA') AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE = '1')"
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not hasChit Then StrSql += " AND FROMFLAG <> 'C'"
        If chkCashOpening.Checked Then
            StrSql += vbCrLf + " AND ACCODE = @CASHID"
        End If
        StrSql += vbCrLf + " GROUP BY TRANMODE"
        If chkCashOpening.Checked Then
            StrSql += " UNION ALL"
            StrSql += " SELECT 'CA' PAYMODE,-1*SUM(AMOUNT) FROM TEMPCASHOPEN"
        End If
        ' ''Correction by Safi on 02-02-2010
        ' ''Begin
        'If hasChit Then
        '    StrSql += vbCrLf + " UNION ALL"
        '    StrSql += vbCrLf + " SELECT  CASE "
        '    StrSql += vbCrLf + " WHEN MODEPAY = 'C' THEN 'CA' "
        '    StrSql += vbCrLf + " WHEN MODEPAY IN('Q','D') THEN 'CH' "
        '    StrSql += vbCrLf + " WHEN MODEPAY = 'R' THEN 'CC'"
        '    StrSql += vbCrLf + " WHEN MODEPAY = 'E' THEN 'ET'"
        '    StrSql += vbCrLf + " WHEN MODEPAY = 'O' THEN 'OT'"
        '    StrSql += vbCrLf + " END PAYMODE,"
        '    StrSql += vbCrLf + " -1*SUM(AMOUNT) AS AMOUNT"
        '    StrSql += vbCrLf + " FROM " & cnChitTrandb & "..SCHEMECOLLECT "
        '    StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
        '    StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        '    StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
        '    StrSql += StrCostFiltration
        '    StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
        '    StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"
        '    StrSql += vbCrLf + " GROUP BY MODEPAY"
        '    ''End
        'End If
        strsql += vbcrlf + " )X "
        strsql += vbcrlf + " GROUP BY PAYMODE "
        strsql += vbcrlf + " HAVING(SUM(AMOUNT) <> 0)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "COLDET)>0 "
        strsql += vbcrlf + " BEGIN "
        strsql += vbcrlf + " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
        strsql += vbcrlf + " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
        strsql += vbcrlf + " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION,COLHEAD) "
        strsql += vbcrlf + " VALUES('COLLECTION DETAILS','T') "
        strsql += vbcrlf + " INSERT INTO TEMP" & systemId & "SASRPU"
        strsql += vbcrlf + " (DESCRIPTION,RECEIPT,PAYMENT) "
        strsql += vbcrlf + " SELECT '  ' + CATNAME CATNAME, "
        strsql += vbcrlf + " CASE WHEN RECEIPT<> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
        strsql += vbcrlf + " CASE WHEN PAYMENT<> 0 THEN PAYMENT  ELSE NULL END PAYMENT "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "COLDET ORDER BY CATNAME "
        StrSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcAmtSubtot()
        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "COLDET)>0 "
        StrSql += " BEGIN "
        StrSql += " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCOLDETTOT') DROP TABLE TEMP" & systemId & "ABSCOLDETTOT "
        StrSql += " SELECT   SUM(RECEIPT) AS RECEIPT, SUM(PAYMENT) AS PAYMENT INTO TEMP" & systemId
        StrSql += "ABSCOLDETTOT FROM TEMP" & systemId & "COLDET END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        DT = New DataTable
        StrSql = " SELECT * FROM TEMP" & systemId & "COLDET"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(DT)
        If DT.Rows.Count <= 0 Then Exit Sub

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSCOLDETTOT)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,COLHEAD) "
        StrSql += " SELECT  'TOTAL', "
        StrSql += " CASE WHEN RECEIPT<> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
        StrSql += " CASE WHEN PAYMENT<> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += " 'S' FROM TEMP" & systemId & "ABSCOLDETTOT "
        StrSql += " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSCOLDETTOT)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,COLHEAD) "
        StrSql += " SELECT 'GRAND TOTAL', ISNULL((CASE WHEN X>0 THEN X END),NULL) AS RECEIPT,"
        StrSql += " ISNULL((CASE WHEN X<0 THEN X END),NULL) AS PAYMENT, 'G'"
        StrSql += " FROM ("
        StrSql += " SELECT RECEIPT-PAYMENT  AS X FROM TEMP" & systemId & "ABSCOLDETTOT) Y"
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcCancelBills()
        StrSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCANCELBILL') DROP TABLE TEMP" & systemId & "ABSCANCELBILL "
        StrSql += " SELECT  TRANNO, TRANTYPE, (CASE "
        StrSql += " WHEN TRANTYPE = 'SA' THEN 'SAL BILL-'  "
        StrSql += " WHEN TRANTYPE = 'PU' THEN 'PUR BILL-'  "
        StrSql += " WHEN TRANTYPE = 'SR' THEN 'SAL RET BILL-'  "
        StrSql += " WHEN TRANTYPE = 'AD' THEN 'ORD ADV BILL-'  "
        StrSql += " WHEN TRANTYPE = 'OD' THEN 'ORD DEL BILL-'  "
        StrSql += " WHEN TRANTYPE = 'RD' THEN 'REP DEL BILL-'  "
        StrSql += " WHEN TRANTYPE = 'AI' THEN 'APP ISS BILL-'  END ) "
        'StrSql += " + CONVERT(VARCHAR(5),TRANNO) + ' - ' + CONVERT(VARCHAR(11),TRANDATE,103) AS TRANS, "
        StrSql += " + CONVERT(VARCHAR(5),TRANNO) AS TRANS, "
        StrSql += " IPCS, IGRSWT, INETWT, RPCS, RGRSWT, RNETWT, RECEIPT, PAYMENT"
        StrSql += " INTO TEMP" & systemId & "ABSCANCELBILL FROM (  "
        StrSql += " SELECT TRANNO, TRANDATE, TRANTYPE, SUM(PCS) IPCS, SUM(GRSWT) IGRSWT,  "
        StrSql += " SUM(NETWT) INETWT, 0 RPCS,  0 RGRSWT, 0 RNETWT, SUM(AMOUNT) RECEIPT, 0 PAYMENT  "
        StrSql += " FROM " & cnStockDb & "..ISSUE  "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND ISNULL(CANCEL,'') = 'Y'  AND COMPANYID IN (" & SelectedCompanyId & ") "
        StrSql += StrFilter
        StrSql += " GROUP BY TRANNO, TRANDATE, TRANTYPE"
        StrSql += " UNION ALL"
        StrSql += " SELECT TRANNO, TRANDATE, TRANTYPE, 0 IPCS, 0 IGRSWT, 0 INETWT,  "
        StrSql += " SUM(PCS) RPCS, SUM(GRSWT) RGRSWT, SUM(NETWT) RNETWT, "
        StrSql += " 0 RECEIPT, SUM(AMOUNT) PAYMENT FROM " & cnStockDb & "..RECEIPT  "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND ISNULL(CANCEL,'') = 'Y'   AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += " GROUP BY TRANNO, TRANDATE, TRANTYPE"
        StrSql += " ) X "
        StrSql += " WHERE TRANTYPE IN ('SA','SR','PU','AD','OD','RD','AI')"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    Select Case gridView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''        Case "T"
    ''            gridView.Rows(e.RowIndex).Cells("COL1").Style.BackColor = Color.LightBlue
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
    ''        Case "G"
    ''            gridView.Rows(e.RowIndex).Cells("COL8").Style.BackColor = Color.Red
    ''            gridView.Rows(e.RowIndex).Cells("COL9").Style.BackColor = Color.Red
    ''            gridView.Rows(e.RowIndex).Cells("COL8").Style.ForeColor = Color.White
    ''            gridView.Rows(e.RowIndex).Cells("COL9").Style.ForeColor = Color.White
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
    ''        Case "S"
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
    ''        Case "D"
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.ForeColor = Color.Blue
    ''    End Select
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("COL1").Style.BackColor = Color.LightBlue
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "G"
                        .Cells("COL8").Style.BackColor = Color.Red
                        .Cells("COL9").Style.BackColor = Color.Red
                        .Cells("COL8").Style.ForeColor = Color.White
                        .Cells("COL9").Style.ForeColor = Color.White
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "S"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "D"
                        .DefaultCellStyle.ForeColor = Color.Blue
                End Select
            End With
        Next
    End Function
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If gridView.Rows.Count > 0 Then
            If ViewMode = 2 Then
                GridHead()
            Else
                GridHead1()
            End If
        End If
    End Sub

    Private Sub GridHead1()
        With gridView
            GridViewHead.ColumnHeadersDefaultCellStyle = gridView.ColumnHeadersDefaultCellStyle
            GridViewHead.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            gridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            GridViewHead.Columns("COL1").Width = .Columns("DESC").Width
            GridViewHead.Columns("COL1").HeaderText = ""
            Dim TEMPCOLWIDTH As Integer = 0
            If chkPcs.Checked = True Then TEMPCOLWIDTH += .Columns("IPCS").Width
            If chkGrswt.Checked = True Then TEMPCOLWIDTH += .Columns("IGWT").Width
            If chkNetwt.Checked = True Then TEMPCOLWIDTH += .Columns("INWT").Width
            GridViewHead.Columns("COL2~COL3~COL4").Width = TEMPCOLWIDTH
            GridViewHead.Columns("COL2~COL3~COL4").HeaderText = "ISSUE"
            TEMPCOLWIDTH = 0
            If chkPcs.Checked = True Then TEMPCOLWIDTH += .Columns("RPCS").Width
            If chkGrswt.Checked = True Then TEMPCOLWIDTH += .Columns("RGWT").Width
            If chkNetwt.Checked = True Then TEMPCOLWIDTH += .Columns("RNWT").Width
            GridViewHead.Columns("COL5~COL6~COL7").Width = TEMPCOLWIDTH
            GridViewHead.Columns("COL5~COL6~COL7").HeaderText = "RECEIPT"
            GridViewHead.Columns("COL8~COL9").Width = .Columns("REC").Width + .Columns("PAY").Width
            GridViewHead.Columns("COL8~COL9").HeaderText = "AMOUNT"
            GridViewHead.Columns("COL10").Width = .Columns("AVERAGE").Width
            GridViewHead.Columns("COL10").HeaderText = "AVERAGE"
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            If colWid >= gridView.Width Then
                GridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                GridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                GridViewHead.Columns("SCROLL").HeaderText = ""
            Else
                GridViewHead.Columns("SCROLL").Visible = False
            End If
            'GridViewHead.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            'GridViewHead.Height = GridViewHead.ColumnHeadersHeight
        End With
    End Sub

    Private Sub GridHead()
        With gridView
            GridViewHead.ColumnHeadersDefaultCellStyle = gridView.ColumnHeadersDefaultCellStyle
            GridViewHead.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            gridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            GridViewHead.Columns("COL1").Width = .Columns("COL1").Width
            GridViewHead.Columns("COL1").HeaderText = ""
            Dim TEMPCOLWIDTH As Integer = 0
            If chkPcs.Checked = True Then TEMPCOLWIDTH += .Columns("COL2").Width
            If chkGrswt.Checked = True Then TEMPCOLWIDTH += .Columns("COL3").Width
            If chkNetwt.Checked = True Then TEMPCOLWIDTH += .Columns("COL4").Width
            GridViewHead.Columns("COL2~COL3~COL4").Width = TEMPCOLWIDTH
            GridViewHead.Columns("COL2~COL3~COL4").HeaderText = "ISSUE"
            TEMPCOLWIDTH = 0
            If chkPcs.Checked = True Then TEMPCOLWIDTH += .Columns("COL5").Width
            If chkGrswt.Checked = True Then TEMPCOLWIDTH += .Columns("COL6").Width
            If chkNetwt.Checked = True Then TEMPCOLWIDTH += .Columns("COL7").Width
            GridViewHead.Columns("COL5~COL6~COL7").Width = TEMPCOLWIDTH
            GridViewHead.Columns("COL5~COL6~COL7").HeaderText = "RECEIPT"
            GridViewHead.Columns("COL8~COL9").Width = .Columns("COL8").Width + .Columns("COL9").Width
            GridViewHead.Columns("COL8~COL9").HeaderText = "AMOUNT"
            GridViewHead.Columns("COL10").Width = .Columns("COL10").Width
            GridViewHead.Columns("COL10").HeaderText = "AVERAGE"
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            If colWid >= gridView.Width Then
                GridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                GridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                GridViewHead.Columns("SCROLL").HeaderText = ""
            Else
                GridViewHead.Columns("SCROLL").Visible = False
            End If
            'GridViewHead.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            'GridViewHead.Height = GridViewHead.ColumnHeadersHeight
        End With
    End Sub

    Private Sub GridCellAlignment()
        With gridView
            .Columns("COL1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("COL2").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL3").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL4").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL5").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL6").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL7").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL8").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL9").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub

    Private Sub GridCellAlignment1()
        With gridView
            .Columns("DESC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("IPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IGWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("INWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RGWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("REC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PAY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AVERAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                GridViewHead.HorizontalScrollingOffset = e.NewValue
                GridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                GridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub ExtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtToolStripMenuItem.Click
        btnExit_Click(Me, e)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Function funcNew() As Integer
        tran = Nothing
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        chkCompanySelectAll.Checked = False
        rbtCategoryWise.Checked = True
        chkAverage.Checked = False
        chkPcs.Checked = True
        chkNetwt.Checked = True
        chkGrswt.Checked = True
        chkCatShortname.Checked = False
        rbtAmount.Checked = True
        gridView.DataSource = Nothing
        chkHomeSale.Checked = False
        chkMiscRecPaySummary.Checked = False
        chkCancelBills.Checked = True

        If ViewMode <> 2 Then
            txtSiPurRate_Amt.Enabled = True
            chkDetaledRecPay.Enabled = False
            chkAverage.Checked = True
            chkPcs.Enabled = False
            chkGrswt.Enabled = False
            chkNetwt.Enabled = False
            chkAverage.Enabled = False
        Else
            txtSiPurRate_Amt.Enabled = False
            chkDetaledRecPay.Enabled = True
            chkPcs.Enabled = True
            chkGrswt.Enabled = True
            chkNetwt.Enabled = True
            chkAverage.Enabled = True
        End If
        Prop_Gets()
        chkChitInfo.Checked = IIf(GetAdmindbSoftValue("CHITDB", "N") = "Y", True, False)
        chkCashOpening.Checked = _CashOpening
        dtpFrom.Select()
    End Function

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, e)
    End Sub

    Private Sub chkGrswt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGrswt.CheckedChanged
        If chkGrswt.Checked = False Then
            chkNetwt.Checked = True
        End If
    End Sub

    Private Sub chkNetwt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNetwt.CheckedChanged
        If chkNetwt.Checked = False Then
            chkGrswt.Checked = True
        End If
    End Sub


    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
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


    Private Sub Prop_Gets()
        Dim obj As New frmDailyAbstract_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmDailyAbstract_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        SetChecked_CheckedList(chklstCostCentre, obj.p_chklstCostCentre, "ALL")
        SetChecked_CheckedList(chkLstCashCounter, obj.p_chkLstCashCounter, "ALL")
        SetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId, "ALL")
        rbtCategoryWise.Checked = obj.p_rbtCategoryWise
        rbtMetalwise.Checked = obj.p_rbtMetalwise
        rbtPartyName.Checked = obj.p_rbtPartyName
        rbtAmount.Checked = obj.p_rbtAmount
        chkPcs.Checked = obj.p_chkPcs
        chkCatShortname.Checked = obj.p_chkCatShortname
        chkChitInfo.Checked = obj.p_chkChitInfo
        chkGrswt.Checked = obj.p_chkGrswt
        chkHomeSale.Checked = obj.p_chkHomeSale
        chkNetwt.Checked = obj.p_chkNetwt
        chkMiscRecPaySummary.Checked = obj.p_chkMiscRecPaySummary
        chkAverage.Checked = obj.p_chkAverage
        chkCancelBills.Checked = obj.p_chkCancelBills
        chkSeperateBeeds.Checked = obj.p_chkSeperateBeeds
        chkCashOpening.Checked = obj.p_chkCashOpening
        chkDetaledRecPay.Checked = obj.p_chkDetaledRecPay
        txtSiPurRate_Amt.Text = obj.p_txtSiPurRate_Amt
        chkDetaledApproval.Checked = obj.p_chkDetailedApproval
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmDailyAbstract_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        GetChecked_CheckedList(chklstCostCentre, obj.p_chklstCostCentre)
        GetChecked_CheckedList(chkLstCashCounter, obj.p_chkLstCashCounter)
        GetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId)
        obj.p_rbtCategoryWise = rbtCategoryWise.Checked
        obj.p_rbtMetalwise = rbtMetalwise.Checked
        obj.p_rbtPartyName = rbtPartyName.Checked
        obj.p_rbtAmount = rbtAmount.Checked
        obj.p_chkPcs = chkPcs.Checked
        obj.p_chkCatShortname = chkCatShortname.Checked
        obj.p_chkChitInfo = chkChitInfo.Checked
        obj.p_chkGrswt = chkGrswt.Checked
        obj.p_chkHomeSale = chkHomeSale.Checked
        obj.p_chkNetwt = chkNetwt.Checked
        obj.p_chkMiscRecPaySummary = chkMiscRecPaySummary.Checked
        obj.p_chkAverage = chkAverage.Checked
        obj.p_chkCancelBills = chkCancelBills.Checked
        obj.p_chkSeperateBeeds = chkSeperateBeeds.Checked
        obj.p_chkCashOpening = chkCashOpening.Checked
        obj.p_chkDetaledRecPay = chkDetaledRecPay.Checked
        obj.p_txtSiPurRate_Amt = txtSiPurRate_Amt.Text
        obj.p_chkDetailedApproval = chkDetaledApproval.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmDailyAbstract_Properties))
    End Sub

    Private Sub btnDotMatrix_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDotMatrix.Click
        hasChit = IIf(GetAdmindbSoftValue("CHITDB", "N") = "Y", True, False) And chkChitInfo.Checked
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("COL1", GetType(String))
            .Columns.Add("COL2~COL3~COL4", GetType(String))
            .Columns.Add("COL5~COL6~COL7", GetType(String))
            .Columns.Add("COL8~COL9", GetType(String))
            .Columns.Add("COL10", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("COL1").Caption = ""
            .Columns("COL2~COL3~COL4").Caption = "ISSUE"
            .Columns("COL5~COL6~COL7").Caption = "RECEIPT"
            .Columns("COL8~COL9").Caption = "AMOUNT"
            .Columns("COL10").Caption = "AVERAGE"
            .Columns("SCROLL").Caption = ""
        End With
        SelectedCompanyId = GetSelectedCompanyId(chkLstCompany, True)
        GridViewHead.DataSource = dtMergeHeader
        Filteration()
        CostIdFiltration()
        Report1(True)
    End Sub
End Class

Public Class frmDailyAbstractOld_Properties

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
    Private chklstCostCentre As New List(Of String)
    Public Property p_chklstCostCentre() As List(Of String)
        Get
            Return chklstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chklstCostCentre = value
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
    Private chkLstNodeId As New List(Of String)
    Public Property p_chkLstNodeId() As List(Of String)
        Get
            Return chkLstNodeId
        End Get
        Set(ByVal value As List(Of String))
            chkLstNodeId = value
        End Set
    End Property
    Private rbtCategoryWise As Boolean = True
    Public Property p_rbtCategoryWise() As Boolean
        Get
            Return rbtCategoryWise
        End Get
        Set(ByVal value As Boolean)
            rbtCategoryWise = value
        End Set
    End Property
    Private rbtMetalwise As Boolean = False
    Public Property p_rbtMetalwise() As Boolean
        Get
            Return rbtMetalwise
        End Get
        Set(ByVal value As Boolean)
            rbtMetalwise = value
        End Set
    End Property
    Private rbtPartyName As Boolean = False
    Public Property p_rbtPartyName() As Boolean
        Get
            Return rbtPartyName
        End Get
        Set(ByVal value As Boolean)
            rbtPartyName = value
        End Set
    End Property
    Private rbtAmount As Boolean = False
    Public Property p_rbtAmount() As Boolean
        Get
            Return rbtAmount
        End Get
        Set(ByVal value As Boolean)
            rbtAmount = value
        End Set
    End Property
    Private chkPcs As Boolean = True

    Public Property p_chkPcs() As Boolean
        Get
            Return chkPcs
        End Get
        Set(ByVal value As Boolean)
            chkPcs = value
        End Set
    End Property
    Private chkCatShortname As Boolean = False
    Public Property p_chkCatShortname() As Boolean
        Get
            Return chkCompanySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCompanySelectAll = value
        End Set
    End Property
    Private chkChitInfo As Boolean = True
    Public Property p_chkChitInfo() As Boolean
        Get
            Return chkChitInfo
        End Get
        Set(ByVal value As Boolean)
            chkChitInfo = value
        End Set
    End Property
    Private chkGrswt As Boolean = True
    Public Property p_chkGrswt() As Boolean
        Get
            Return chkGrswt
        End Get
        Set(ByVal value As Boolean)
            chkGrswt = value
        End Set
    End Property
    Private chkHomeSale As Boolean = False
    Public Property p_chkHomeSale() As Boolean
        Get
            Return chkHomeSale
        End Get
        Set(ByVal value As Boolean)
            chkHomeSale = value
        End Set
    End Property

    Private chkNetwt As Boolean = True
    Public Property p_chkNetwt() As Boolean
        Get
            Return chkNetwt
        End Get
        Set(ByVal value As Boolean)
            chkNetwt = value
        End Set
    End Property
    Private chkMiscRecPaySummary As Boolean = False
    Public Property p_chkMiscRecPaySummary() As Boolean
        Get
            Return chkMiscRecPaySummary
        End Get
        Set(ByVal value As Boolean)
            chkMiscRecPaySummary = value
        End Set
    End Property
    Private chkAverage As Boolean = False
    Public Property p_chkAverage() As Boolean
        Get
            Return chkAverage
        End Get
        Set(ByVal value As Boolean)
            chkAverage = value
        End Set
    End Property
    Private chkCancelBills As Boolean = True
    Public Property p_chkCancelBills() As Boolean
        Get
            Return chkCancelBills
        End Get
        Set(ByVal value As Boolean)
            chkCancelBills = value
        End Set
    End Property
    Private chkSeperateBeeds As Boolean = False
    Public Property p_chkSeperateBeeds() As Boolean
        Get
            Return chkSeperateBeeds
        End Get
        Set(ByVal value As Boolean)
            chkSeperateBeeds = value
        End Set
    End Property
    Private chkCashOpening As Boolean = False
    Public Property p_chkCashOpening() As Boolean
        Get
            Return chkCashOpening
        End Get
        Set(ByVal value As Boolean)
            chkCashOpening = value
        End Set
    End Property
    Private chkDetaledRecPay As Boolean = False
    Public Property p_chkDetaledRecPay() As Boolean
        Get
            Return chkDetaledRecPay
        End Get
        Set(ByVal value As Boolean)
            chkDetaledRecPay = value
        End Set
    End Property
    Private txtSiPurRate_Amt As String
    Public Property p_txtSiPurRate_Amt() As String
        Get
            Return txtSiPurRate_Amt
        End Get
        Set(ByVal value As String)
            txtSiPurRate_Amt = value
        End Set
    End Property
    Private chkDetailedApproval As Boolean = False
    Public Property p_chkDetailedApproval() As Boolean
        Get
            Return chkDetailedApproval
        End Get
        Set(ByVal value As Boolean)
            chkDetailedApproval = value
        End Set
    End Property
End Class

