Imports System.Data.OleDb
'CAL ID-602: CLIENT NAME- PRINCT: CORRECTION- CREDIT CARD COMMISSION SHOULD BE COME WITH COLLECTION : ALTER BY SATHYA
Public Class frmDailyAbstractTT
    Dim Cmd As New OleDbCommand
    Dim DT As New DataTable
    Dim dtAverage As New DataTable
    Dim dtPurityRate As New DataTable

    Dim dtDiscount As New DataTable
    Dim dtSnoFrom As New DataTable

    Dim dtSnoTo As New DataTable
    Dim StrSql As String
    Dim StrFilter As String
    Dim StrCostFiltration As String
    Dim StrCashCounterFtr As String
    Dim StrUseridFtr As String
    Dim dsReportCol As New DataSet
    Dim SelectedCompanyId As String
    Dim hasChit As Boolean = False
    Dim RPT_SEPVAT_DABS As Boolean = IIf(GetAdmindbSoftValue("RPT_SEPVAT_DABS", "Y") = "Y", True, False)
    Dim RPT_ME_SUM_DABS As Boolean = IIf(GetAdmindbSoftValue("RPT_ME_SUM_DABS", "N") = "Y", True, False)
    Dim RPT_MIS_ISS_DABS As Boolean = IIf(GetAdmindbSoftValue("RPT_MIS_ISS_DABS", "Y") = "Y", True, False)
    Dim RPT_CNLINCL_DABS As Boolean = IIf(GetAdmindbSoftValue("RPT_CNLINCL_DABS", "N") = "Y", True, False)
    Dim RPT_SELFCTR_DABS As Boolean = IIf(GetAdmindbSoftValue("RPT_SELFCTR_DABS", "N") = "Y", True, False)
    Dim RPT_CASHNODE_DABS As Boolean = IIf(GetAdmindbSoftValue("RPT_CASHNODE_DABS", "N") = "Y", True, False)
    Dim ViewMode As Int16 = Val(GetAdmindbSoftValue("RPT_DABS", "2"))
    Dim RPT_CHKDIS_ROLEEDIT As Boolean = IIf(GetAdmindbSoftValue("RPT_CHKDIS_ROLEEDIT", "N") = "Y", True, False)
    Dim RPT_SEPORD_DABS As Boolean = IIf(GetAdmindbSoftValue("RPT_SEPORD_DABS", "Y") = "Y", True, False)
    Dim RPT_SEPSTUD_DABS As Boolean = IIf(GetAdmindbSoftValue("RPT_SEPSTUD_DABS", "Y") = "Y", True, False)
    Dim RPT_HIDE_ADVSUMMARY As Boolean = IIf(GetAdmindbSoftValue("RPT_HIDE_ADVSUMMARY", "Y") = "Y", True, False)
    Dim RPT_HIDE_GIFTOTHERS As Boolean = IIf(GetAdmindbSoftValue("RPT_HIDE_GIFTOTHERS", "Y") = "Y", True, False)
    Dim RPT_HIDE_LOTDETAILS As Boolean = IIf(GetAdmindbSoftValue("RPT_HIDE_LOTDETAILS", "Y") = "Y", True, False)
    Dim dtMergeHeader As DataTable
    Dim Authorize As Boolean = False
    Dim SCashCounterName As String
    Dim AvgRate As Double
    Dim PurityRate As Double
    Dim DISCOUNT As Double
    Dim NETWT As Double
    Dim RECEIPT As Double



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
        'If RPT_SELFCTR_DABS = True Then
        '    pnlUser.Visible = True
        '    ProcAddUser()
        '    Panel1.Location = New Point(15, 360)
        '    btnView_Search.Location = New Point(15, 530)
        '    btnNew.Location = New Point(123, 530)
        '    btnExit.Location = New Point(229, 530)
        'Else
        '    pnlUser.Visible = False
        '    Panel1.Location = New Point(15, 333)
        '    btnView_Search.Location = New Point(15, 504)
        '    btnNew.Location = New Point(123, 504)
        '    btnExit.Location = New Point(229, 504)
        'End If
        pnlUser.Visible = False
        Panel1.Location = New Point(15, 333)
        btnView_Search.Location = New Point(15, 530)
        btnNew.Location = New Point(123, 530)
        btnExit.Location = New Point(229, 530)
        chkAdvdueSummary.Enabled = False
        Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
        If Authorize = True Then chkAdvdueSummary.Enabled = True
        btnNew_Click(Me, New EventArgs)
        rbtCategoryWise.Checked = True
    End Sub

    Private Sub ProcAddUser()
        StrSql = "SELECT USERNAME,USERID FROM " & cnAdminDb & "..USERMASTER ORDER BY USERNAME"
        DT = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            cmbUser.Enabled = True
            cmbUser.Items.Add("ALL")
            For CNT As Integer = 0 To DT.Rows.Count - 1
                cmbUser.Items.Add(DT.Rows(CNT).Item(0).ToString)
                If RPT_SELFCTR_DABS = True And userId <> 999 And userId = DT.Rows(CNT).Item(1).ToString Then
                    cmbUser.Text = DT.Rows(CNT).Item(0).ToString
                    cmbUser.Enabled = False
                End If
            Next
        Else
            cmbUser.Items.Clear()
            cmbUser.Enabled = False
        End If

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
                chklstCostCentre.Items.Add("ALL", IIf(cnDefaultCostId, True, False))
                For cnt As Integer = 0 To DT.Rows.Count - 1
                    If cnCostName = DT.Rows(cnt).Item(0).ToString And cnDefaultCostId = False Then
                        chklstCostCentre.Items.Add(DT.Rows(cnt).Item(0).ToString, True)
                    Else
                        chklstCostCentre.Items.Add(DT.Rows(cnt).Item(0).ToString, False)
                    End If
                Next
                If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chklstCostCentre.Enabled = False
            Else
                chklstCostCentre.Items.Clear()
                chklstCostCentre.Enabled = False
            End If
        Else
            chklstCostCentre.Items.Clear()
            chklstCostCentre.Enabled = False
        End If
    End Sub




    'Private Sub ProcAddCashCounter()

    '    StrSql = "SELECT CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER WHERE ISNULL(ACTIVE,'Y') <> 'N' ORDER BY CASHID"
    '    DT = New DataTable
    '    da = New OleDbDataAdapter(StrSql, cn)
    '    da.Fill(DT)
    '    If DT.Rows.Count > 0 Then
    '        chkLstCashCounter.Items.Add("ALL")
    '        For CNT As Integer = 0 To DT.Rows.Count - 1
    '            chkLstCashCounter.Items.Add(DT.Rows(CNT).Item(0).ToString)
    '            chkLstCashCounter.Enabled = True
    '        Next
    '    Else
    '        chkLstCashCounter.Items.Clear()
    '        chkLstCashCounter.Enabled = False
    '    End If
    'End Sub

    Private Sub ProcAddCashCounter()

        StrSql = " SELECT CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER where "
        StrSql += " CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..USERCASH WHERE USERID='" & userId & "' )"
        Dim dtta As New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtta)

        StrSql = "SELECT CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER  ORDER BY CASHID"
        DT = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            chkLstCashCounter.Items.Add("ALL", IIf(RPT_SELFCTR_DABS And userId <> 999, False, True))
            For CNT As Integer = 0 To DT.Rows.Count - 1
                If RPT_SELFCTR_DABS = True And userId <> 999 And dtta.Rows.Count > 0 Then
                    For k As Integer = 0 To dtta.Rows.Count - 1
                        If DT.Rows(CNT).Item(0).ToString = dtta.Rows(k).Item(0).ToString Then
                            chkLstCashCounter.Items.Add(DT.Rows(CNT).Item(0).ToString, True)
                        Else
                            chkLstCashCounter.Items.Add(DT.Rows(CNT).Item(0).ToString, False)
                        End If
                    Next
                    chkLstCashCounter.Enabled = False
                Else
                    chkLstCashCounter.Enabled = True
                    chkLstCashCounter.Items.Add(DT.Rows(CNT).Item(0).ToString)
                End If
            Next
        Else
            chkLstCashCounter.Items.Clear()
            chkLstCashCounter.Enabled = False
        End If
    End Sub


    Private Sub ProcAddNodeId()
        Try
            StrSql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN ORDER BY SYSTEMID"
            DT = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(DT)
            If DT.Rows.Count > 0 Then
                chkLstNodeId.Items.Add("ALL")
                For CNT As Integer = 0 To DT.Rows.Count - 1
                    chkLstNodeId.Enabled = True
                    chkLstNodeId.Items.Add(DT.Rows(CNT).Item(0).ToString)
                Next
            Else
                chkLstNodeId.Items.Clear()
                chkLstNodeId.Enabled = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    'Private Sub ProcAddNodeId()
    '    Try
    '        StrSql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN ORDER BY SYSTEMID"
    '        DT = New DataTable
    '        da = New OleDbDataAdapter(StrSql, cn)
    '        da.Fill(DT)
    '        If DT.Rows.Count > 0 Then
    '            chkLstNodeId.Items.Add("ALL", IIf(RPT_SELFCTR_DABS And userId <> 999, False, True))
    '            For CNT As Integer = 0 To DT.Rows.Count - 1
    '                If RPT_SELFCTR_DABS = True And userId <> 999 Then
    '                    If DT.Rows(CNT).Item(0).ToString = systemId Then
    '                        chkLstNodeId.Items.Add(DT.Rows(CNT).Item(0).ToString, True)
    '                    Else
    '                        chkLstNodeId.Items.Add(DT.Rows(CNT).Item(0).ToString, False)
    '                    End If
    '                    chkLstNodeId.Enabled = False
    '                Else
    '                    chkLstNodeId.Enabled = True
    '                    chkLstNodeId.Items.Add(DT.Rows(CNT).Item(0).ToString)
    '                End If
    '                'If chkLstNodeId.Text = systemId Then chkLstNodeId.Add(chkLstNodeId.CheckedItems.Item(CNT).ToString)
    '            Next
    '        Else
    '            chkLstNodeId.Items.Clear()
    '            chkLstNodeId.Enabled = False
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
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
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle.Text, gridView, BrightPosting.GExport.GExportType.Export, GridViewHead)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        hasChit = IIf(GetAdmindbSoftValue("CHITDB", "N") = "Y", True, False) And chkChitInfo.Checked

        'If rbtmetalwisegrp.Checked = True Then
        '    Dim dtCheck As New DataTable
        '    'StrSql = "USE TEMPTABLEDB"
        '    'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        '    'Cmd.ExecuteNonQuery()
        '    StrSql = "SELECT * FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMPSALDIFF'"
        '    da = New OleDbDataAdapter(StrSql, cn)
        '    dtCheck.Clear()
        '    da.Fill(dtCheck)
        '    If dtCheck.Rows.Count > 0 Then

        '    Else
        '        MsgBox("PLEASE OPEN THE SALES WITH DIFFERENCE REPORT")
        '        Exit Sub
        '    End If

        '    'StrSql = "USE MASTER"
        '    'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        '    'Cmd.ExecuteNonQuery()
        'End If

        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If RPT_CNLINCL_DABS Then chkCancelBills.Checked = True
        dtMergeHeader = New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("COL1", GetType(String))
            .Columns.Add("COL2~COL3~COL4", GetType(String))
            .Columns.Add("COL5~COL6~COL7", GetType(String))
            .Columns.Add("COL8~COL9", GetType(String))
            .Columns.Add("COL10", GetType(String))
            .Columns.Add("COL11", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("COL1").Caption = ""
            .Columns("COL2~COL3~COL4").Caption = "ISSUE"
            .Columns("COL5~COL6~COL7").Caption = "RECEIPT"
            .Columns("COL8~COL9").Caption = "AMOUNT"
            .Columns("COL10").Caption = "AVERAGE"
            .Columns("COL11").Caption = "DISCPER"
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
    End Sub

    Private Sub Title()
        Dim TITLE As String = Nothing
        Dim Flag As Boolean = False
        If rbtCategoryWise.Checked = True Then
            TITLE = "CATEGORYWISE "
        Else
            TITLE = "METALWISE "
        End If
        TITLE += " DAILY ABSTRACT REPORT FROM " & dtpFrom.Value.ToString("dd-MM-yyyy") & " TO "
        TITLE += dtpTo.Value.ToString("dd-MM-yyyy")
        Dim Cname As String = ""
        If chklstCostCentre.Items.Count > 0 And chklstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chklstCostCentre.Items.Count - 1
                If chklstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chklstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1) : Flag = True
        End If
        TITLE = TITLE + IIf(Cname <> "", " FOR COSTCENTRE" & Cname, Cname)
        If RPT_SELFCTR_DABS = True And userId <> 999 And chkLstCashCounter.Enabled = False Then
            Dim Csahname As String = ""
            If chkLstCashCounter.Items.Count > 0 And chkLstCashCounter.CheckedItems.Count > 0 Then
                For CNT As Integer = 0 To chkLstCashCounter.Items.Count - 1
                    If chkLstCashCounter.GetItemChecked(CNT) = True Then
                        Csahname += "" & chkLstCashCounter.Items(CNT) + ","
                    End If
                Next
                If Csahname <> "" Then Csahname = " :" + Mid(Csahname, 1, Len(Csahname) - 1) : Flag = True
            End If
            If Flag Then
                TITLE = TITLE + vbCrLf + IIf(Csahname <> "", " ,CASHCOUNTER" & Csahname, Csahname)
            Else
                TITLE = TITLE + vbCrLf + IIf(Csahname <> "", " FOR CASHCOUNTER" & Csahname, Csahname)
            End If
        End If
        If RPT_CASHNODE_DABS = True Then
            Dim Csahname As String = ""
            If chkLstCashCounter.Items.Count > 0 And chkLstCashCounter.CheckedItems.Count > 0 Then
                For CNT As Integer = 0 To chkLstCashCounter.Items.Count - 1
                    If chkLstCashCounter.GetItemChecked(CNT) = True Then
                        Csahname += "" & chkLstCashCounter.Items(CNT) + ","
                    End If
                Next
                If Csahname <> "" Then Csahname = " :" + Mid(Csahname, 1, Len(Csahname) - 1) : Flag = True
            End If
            If Flag Then
                TITLE = TITLE + vbCrLf + IIf(Csahname <> "", " ,CASHCOUNTER" & Csahname, Csahname)
            Else
                TITLE = TITLE + vbCrLf + IIf(Csahname <> "", " FOR CASHCOUNTER" & Csahname, Csahname)
            End If
            Dim Nodename As String = ""
            If chkLstNodeId.Items.Count > 0 And chkLstNodeId.CheckedItems.Count > 0 Then
                For CNT As Integer = 0 To chkLstNodeId.Items.Count - 1
                    If chkLstNodeId.GetItemChecked(CNT) = True Then
                        Nodename += "" & chkLstNodeId.Items(CNT) + ","
                    End If
                Next
                If Nodename <> "" Then Nodename = " :" + Mid(Nodename, 1, Len(Nodename) - 1)
            End If
            If Flag Then
                TITLE = TITLE + vbCrLf + IIf(Nodename <> "", " ,SYSTEMID" & Nodename, Nodename)
            Else
                TITLE = TITLE + vbCrLf + IIf(Nodename <> "", " FOR SYSTEMID" & Nodename, Nodename)
            End If
        End If
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
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
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
        'If chklstCostCentre.Enabled = True Then
        If chklstCostCentre.Items.Count > 0 Then
            If chklstCostCentre.GetItemChecked(0) <> True Then
                For CNT As Integer = 1 To chklstCostCentre.Items.Count - 1
                    If chklstCostCentre.GetItemChecked(CNT) = True Then
                        tempchkitem = tempchkitem & " '" & chklstCostCentre.Items.Item(CNT) & "'" & ", "
                    End If
                Next
                If Len(tempchkitem) > 1 Then tempchkitem = Mid(Trim(tempchkitem), 1, Len(Trim(tempchkitem)) - 1)
            End If
        Else
            tempchkitem = ""
        End If

        If tempchkitem <> "" Then
            StrFilter = " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN"
            StrFilter += " (" & tempchkitem & "))"
        Else
            StrFilter = ""
        End If
        'End If
        tempchkitem = ""
        SCashCounterName = ""
        ''CASH COUNTER
        If chkLstCashCounter.Items.Count > 0 Then
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
        End If
        If tempchkitem <> "" Then
            StrFilter += " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN"
            StrFilter += " (" & tempchkitem & "))"
            If chkLstCashCounter.Enabled = False Then SCashCounterName = tempchkitem
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
        ''Userid
        If cmbUser.Text <> "ALL" And cmbUser.Text <> "" And cmbUser.Visible = True Then
            Dim USERID As String = GetSqlValue(cn, "SELECT USERID FROM " & cnAdminDb & "..USERMASTER I WHERE USERNAME ='" & cmbUser.Text & "'")
            StrUseridFtr = " AND USERID IN ('" & USERID & "')"
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
            StrSql += " COL11 VARCHAR(100),"
            StrSql += " COLHEAD VARCHAR(3),"
            StrSql += " CATCODE VARCHAR(5),"
            StrSql += " CATEGORYCODE VARCHAR(30),"
            StrSql += " ORDERNO INT,"
            StrSql += " COLHEAD1 VARCHAR(5),"
            StrSql += " AVGRATE DECIMAL(15,2),"
            If rbtmetalwisegrp.Checked = True Then
                StrSql += " RESULT2 INT,"
                StrSql += " COLHEAD2 VARCHAR(5),"
                StrSql += " TYPE VARCHAR(30),"
            End If
            StrSql += " SNO INT IDENTITY(1,1))"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
            StrSql += " AVERAGE VARCHAR(100),"
            StrSql += " DISCPER NUMERIC(20,2),"
            StrSql += " COLHEAD VARCHAR(3),"
            StrSql += " RESULT1 NUMERIC(15,2),"
            StrSql += " CATCODE VARCHAR(5),"
            StrSql += " CATEGORYCODE VARCHAR(30),"
            StrSql += " ORDERNO INT,"
            StrSql += " COLHEAD1 VARCHAR(5),"
            StrSql += " AVGRATE DECIMAL(15,2),"
            If rbtmetalwisegrp.Checked = True Then
                StrSql += " RESULT2 INT,"
                StrSql += " COLHEAD2 VARCHAR(5),"
                StrSql += " TYPE VARCHAR(30),"
            End If
            StrSql += " SNO INT IDENTITY(1,1))"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
                StrSql = " SELECT NAME CNT FROM MASTER.DBO.SYSDATABASES WHERE NAME = '" & cnChitTrandb & "'"
                If Not objGPack.DupCheck(StrSql) Then MsgBox("SCHEME Database Not Found", MsgBoxStyle.Information) : Exit Sub
            End If
            ProcSASRPU(DotMatrix)
            If rbtmetalwisegrp.Checked = True Then
                StrSql = " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL2=NULL WHERE COL2='0'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL3=NULL WHERE COL3='0.000'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL4=NULL WHERE COL4='0.000'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL5=NULL WHERE COL5='0'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL6=NULL WHERE COL6='0.000'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL7=NULL WHERE COL7='0.000'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL8=NULL WHERE COL8='0.00'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL9=NULL WHERE COL9='0.00'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL10=NULL WHERE COL10='0'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL11=NULL WHERE COL11='0'"
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()
            End If

            If rbtmetalwisegrp.Checked Then

                StrSql = " DELETE FROM TEMP" & systemId & "SALEABSTRACT"
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()

                StrSql = " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
                StrSql += vbCrLf + " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COLHEAD, ORDERNO, CATEGORYCODE, RESULT2, COLHEAD2, AVGRATE) "
                StrSql += vbCrLf + " SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,DISCPER,COLHEAD,8 , CATEGORYCODE, RESULT2, COLHEAD2, AVGRATE "
                StrSql += " FROM TEMP" & systemId & "SASRPU  ORDER BY SNO "
                'CATCODE, RESULT2, COLHEAD2" 'WHERE ISNULL(RESULT1,1) NOT IN (0,2) ORDER BY SNO"
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()

                'StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT  SET CATCODE = 'ZZZ' WHERE ORDERNO = 8 AND ISNULL(RESULT2,'') = ''  "
                'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                'Cmd.ExecuteNonQuery()

                StrSql = " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL2=NULL WHERE COL2='0'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL3=NULL WHERE COL3='0.000'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL4=NULL WHERE COL4='0.000'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL5=NULL WHERE COL5='0'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL6=NULL WHERE COL6='0.000'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL7=NULL WHERE COL7='0.000'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL8=NULL WHERE COL8='0.00'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL9=NULL WHERE COL9='0.00'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL10=NULL WHERE COL10='0'"
                StrSql += " UPDATE TEMP" & systemId & "SALEABSTRACT SET COL11=NULL WHERE COL11='0'"
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()

                StrSql = "SELECT COL1,COL2,COL3,COL4,COL5, "
                StrSql += " COL6,COL7,COL8,COL9,COL10,COL11, "
                StrSql += " COLHEAD,COLHEAD1, AVGRATE FROM TEMP" & systemId & "SALEABSTRACT ORDER BY SNO "
            Else
                StrSql = "SELECT COL1,COL2,COL3,COL4,COL5, "
                StrSql += " COL6,COL7,COL8,COL9,COL10,COL11, "
                StrSql += " COLHEAD,COLHEAD1 FROM TEMP" & systemId & "SALEABSTRACT ORDER BY SNO "
            End If
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
                .Columns("COL11").HeaderText = "DISC%"
                .Columns("COLHEAD").HeaderText = "HCOL"
                .Columns("COLHEAD1").HeaderText = "HCOL1"
            End With
            GridCellAlignment()
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("COLHEAD1").Visible = False
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
            If ChkMetalDisc.Checked = False Then
                gridView.Columns("COL11").Visible = False
                GridViewHead.Columns("COL11").Visible = False
            Else
                GridViewHead.Columns("COL11").Visible = True
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
        StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If rbtmetalwisegrp.Checked = False Then
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
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSREPAIR)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSREPAIR(CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
            StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
            StrSql += vbCrLf + " SELECT 'SALES' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT,SUM(ISNULL(PAYMENT,0))PAYMENT, 1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSREPAIR GROUP BY CATCODE"
            'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            'Cmd.ExecuteNonQuery()
        End If


        If rbtmetalwisegrp.Checked = False Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSREPAIR)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSREPAIR(CATCODE,CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
            StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
            StrSql += vbCrLf + " SELECT 'Z' AS CATCODE, 'SALES TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
            StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
            StrSql += vbCrLf + " SUM(PAYMENT),4 RESULT,2 RESULT1, 'S' COLHEAD "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSREPAIR WHERE RESULT = 1 "
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        If rbtmetalwisegrp.Checked = False Then
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
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSREPAIR)>0"
            StrSql += vbCrLf + " BEGIN "
            'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            ''strsql += vbcrlf + " (DESCRIPTION, COLHEAD) VALUES('SALES','T') "
            'StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) "
            'StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
            'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
            'StrSql += " WHERE RESULT1 = 1"
            'StrSql += " AND COLHEAD <> 'D'"
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,CATCODE,RESULT1) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, COLHEAD,CATCODE, RESULT1 "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSREPAIR"
            StrSql += vbCrLf + " WHERE RESULT1 = 1 AND COLHEAD='L'"
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub ProcSales_MetalGroup()
        If (RPT_ME_SUM_DABS = True And rbtMetalwise.Checked = True) Then Exit Sub
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSSALES') DROP TABLE TEMP" & systemId & "ABSSALES"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT CATCODE "
        Else
            StrSql += vbCrLf + " SELECT METALID AS CATCODE "
        End If
        If rbtmetalwisegrp.Checked = True Then
            StrSql += vbCrLf + " , CATEGORYCODE   "
        End If
        StrSql += vbCrLf + " ,CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + " PAYMENT, AVERAGE, RATE,"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " DISCPER,"
        StrSql += vbCrLf + "  COLHEAD, 1 RESULT, RESULT1 "
        If rbtmetalwisegrp.Checked = True Then
            StrSql += vbCrLf + "  ,1 RESULT2, 'A' COLHEAD2, '' TYPE"
        End If
        StrSql += vbCrLf + "  INTO TEMP" & systemId & "ABSSALES FROM ("
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT I.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            End If
        Else
            StrSql += vbCrLf + " SELECT I.METALID, "
            StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + " I.CATCODE AS CATEGORYCODE,"
            ElseIf rbtMetalwise.Checked = True Then
                StrSql += vbCrLf + " I.CATCODE AS CATCODE,"
            End If
        End If
        StrSql += vbCrLf + " CATNAME, SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += vbCrLf + " SUM(AMOUNT) AS RECEIPT, 0 AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        StrSql += vbCrLf + " ELSE "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/ (CASE WHEN COUNT(RATE) = 0 THEN COUNT(RATE) END) ))) AS RATE"
        'If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),(SUM(DISC)/ CASE WHEN SUM(NETWT)>0 THEN SUM(NETWT) WHEN SUM(PCS)>0 THEN SUM(PCS) ELSE 1 END )) AS DISCPER"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),(SUM(DISC)/((CASE WHEN SUM(AMOUNT)=0 THEN 1 ELSE SUM(AMOUNT) END)/100))) AS DISCPER"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(3),'') COLHEAD "
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
            StrSql += vbCrLf + " ,(SELECT GRSWT FROM " & cnStockDb & "..ISSUE WHERE SNO=I.SNO "
            StrSql += vbCrLf + " AND SNO NOT IN(SELECT ISSSNO FROM " & cnStockDb & "..ISSMETAL"
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") ))GRSWT "
        End If

        StrSql += vbCrLf + " ,(SELECT NETWT FROM " & cnStockDb & "..ISSUE WHERE SNO=I.SNO "
        StrSql += vbCrLf + " AND SNO NOT IN(SELECT ISSSNO FROM " & cnStockDb & "..ISSMETAL"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") ))NETWT "
        If RPT_SEPVAT_DABS = False Then
            StrSql += vbCrLf + " ,AMOUNT+ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0)"
        Else
            StrSql += vbCrLf + " ,AMOUNT"
        End If

        If RPT_SEPSTUD_DABS Then
            StrSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
            StrSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
            'Else
            '    StrSql += vbCrLf + " +ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
            '    StrSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
        End If

        StrSql += vbCrLf + " AS AMOUNT,RATE"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,(ISNULL(FIN_DISCOUNT,0)+ISNULL(DISCOUNT,0)) AS DISC "
        StrSql += vbCrLf + " ,CATCODE,METALID"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        StrSql += vbCrLf + " I.TRANTYPE IN('SA') AND ISNULL(I.CANCEL,'') = '' "
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")

        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT (SELECT SUM(PCS) FROM " & cnStockDb & "..ISSUE  WHERE SNO=I.ISSSNO)PCS"
        StrSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
        StrSql += vbCrLf + " ,SUM(GRSWT)NETWT"
        StrSql += vbCrLf + " ,NULL AS AMOUNT,RATE"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,0 AS DISC "
        StrSql += vbCrLf + " ,CATCODE,METALID"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSMETAL I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") "
        StrSql += vbCrLf + " AND ISSSNO  IN(SELECT SNO FROM " & cnStockDb & "..ISSUE "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND TRANTYPE IN('SA') AND ISNULL(CANCEL,'') = '' ) "
        StrSql += vbCrLf + " GROUP BY CATCODE,ISSSNO ,SNO,AMOUNT,RATE,METALID "

        StrSql += vbCrLf + " )AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY I.CATCODE,C.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME , I.CATCODE "
        End If

        If RPT_SEPVAT_DABS Then
            ''TAX
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.STAXID) AS CATNAME, "
                ' ElseIf rbtmetalwisegrp.Checked = True Then
                '    StrSql += vbCrLf + " SELECT I.METALID, 'VAT' AS METALNAME, CATNAME +'  [''VAT'']', "
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME, I.CATCODE, CATNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(I.TAX+ISNULL(TT.TAXAMT,0)-ISNULL(ISS.TAX,0)) AS RECEIPT, 0 AS PAYMENT, 1 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
            StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'VAT') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
            StrSql += vbCrLf + " ON TT.SNO=I.SNO and TT.BATCHNO=I.BATCHNO "
            StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAX) TAX ,ISSSNO,STNITEMID  from " & cnStockDb & "..ISSSTONE GROUP BY ISSSNO,STNITEMID ) ISS "
            StrSql += vbCrLf + " ON ISS.ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.STAXID HAVING SUM(I.TAX) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, I.CATCODE"
            End If

            '' SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.SSCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'SURCHARGE' AS METALNAME,CATNAME, I.CATCODE,"
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(SC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
            StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SSCID HAVING SUM(SC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, I.CATCODE  HAVING SUM(SC) <> 0"
            End If

            '' ADDTIONAL SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.SASCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'ADDTIONALSURCHARGE' AS METALNAME,CATNAME, I.CATCODE, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(ADSC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
            StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SASCID HAVING SUM(ADSC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, I.CATCODE HAVING SUM(ADSC) <> 0 "
            End If
        End If
        If RPT_SEPSTUD_DABS Then
            StrSql += vbCrLf + " UNION ALL "
            StrSql += vbCrLf + " SELECT CATCODE"
            If rbtCategoryWise.Checked = False Then
                StrSql += vbCrLf + ",METALNAME"
            End If
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + ", CATEGORYCODE"
            End If
            If rbtMetalwise.Checked = True Then
                StrSql += vbCrLf + " ,'' CATEGORYCODE"
            End If
            StrSql += vbCrLf + ", CATNAME"
            StrSql += vbCrLf + ", SUM(ISSPCS) AS ISSPCS, "
            StrSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
            StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " , COLHEAD FROM ("
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
                'If rbtmetalwisegrp.Checked = True Then
                '    StrSql += vbCrLf + " (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY  "
                '    StrSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATEGORYCODE  "
                'End If
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..ISSUE "
                ''strsql += vbcrlf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE)AS CATCODE, 'DIAMOND' AS CATNAME,  "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS METALNAME,  "
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=S.CATCODE)CATNAME, "
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + " (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=S.CATCODE)CATEGORYCODE, "
                End If

            End If
            StrSql += vbCrLf + " SUM(S.STNPCS) ISSPCS,SUM(S.STNWT) AS ISSGRSWT,SUM(S.STNWT) AS ISSNETWT,  "
            StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.STNAMT) AS RECEIPT,  "
            StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE,"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
            StrSql += vbCrLf + " 'C' COLHEAD  FROM " & cnStockDb & "..ISSSTONE S "
            StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SA') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            'StrSql += StrFilter
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
            Else
                StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE "
            End If
            StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
            If rbtCategoryWise.Checked = False Then
                StrSql += vbCrLf + " ,METALNAME"
            End If
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + " ,CATEGORYCODE "
            End If
            StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        End If
        If RPT_SEPVAT_DABS Then
            'TAX
            StrSql += vbCrLf + " UNION ALL "

            StrSql += vbCrLf + " SELECT '' METALID"

            If rbtCategoryWise.Checked = False Then
                StrSql += vbCrLf + ",METALNAME "
            Else
                StrSql += vbCrLf + ",  CATCODE"
            End If

            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + " ,CATEGORYCODE"
            End If
            'If rbtMetalwise.Checked = True Then
            '    StrSql += vbCrLf + " ,'' CATEGORYCODE"
            'End If
            StrSql += vbCrLf + " , CATNAME,SUM(ISSPCS) AS ISSPCS,SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
            StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            StrSql += vbCrLf + " ,COLHEAD FROM ("
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..ISSUE "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
                'If chkCatShortname.Checked = True Then
                '    StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE = S.CATCODE)  "
                'Else
                '    StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE = S.CATCODE)  "
                'End If
                StrSql += vbCrLf + "(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.STAXID) AS CATNAME,  "
            Else
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..ISSUE "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS METALNAME,  "
                StrSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.STAXID) AS CATNAME, "
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + " (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = C.CATCODE) AS CATEGORYCODE, "
                End If

            End If
            StrSql += vbCrLf + " 0 ISSPCS,0 AS ISSGRSWT,0 AS ISSNETWT,  "
            StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.TAX) AS RECEIPT,  "
            StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
            StrSql += vbCrLf + " 'VAT' COLHEAD  FROM " & cnStockDb & "..ISSSTONE S "
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = S.CATCODE"
            StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SA') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            'StrSql += StrFilter
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO,C.STAXID "
            Else
                StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE,C.STAXID, C.CATCODE    "
            End If
            StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
            If rbtCategoryWise.Checked = False Then
                StrSql += vbCrLf + " ,METALNAME"
            End If
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER"
            If rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " , CATEGORYCODE "

            StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        End If
        StrSql += vbCrLf + " ) X "
        'StrSql += vbCrLf + " WHERE CATNAME NOT LIKE('%(RT)%') AND CATNAME NOT LIKE '%VAT%' "
        'StrSql += vbCrLf + " AND  CATNAME NOT LIKE('%( WT)%') OR CATNAME NOT LIKE '%(WT)%' "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        '''''''''''' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''------------
        If rbtmetalwisegrp.Checked = False Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " DISCPER, "
            StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD) "
            StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
            StrSql += vbCrLf + " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, "
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " 0 DISCPER, "
            StrSql += vbCrLf + " 0 RESULT, 0 RESULT1, 'T' COLHEAD "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES"
            'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            'StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            'StrSql += vbCrLf + " BEGIN "
            'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATNAME, "
            'StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
            'StrSql += vbCrLf + " SELECT  CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            'StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
            'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES  GROUP BY CATCODE,CATNAME"
            ''StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            'StrSql += vbCrLf + " END "
            'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            'Cmd.ExecuteNonQuery()


            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATEGORYCODE, CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
            StrSql += vbCrLf + " SELECT CATEGORYCODE, CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
            StrSql += vbCrLf + " ,1 AS RESULT2, 'A' AS COLHEAD2, '' TYPE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES WHERE COLHEAD <> 'VAT'  GROUP BY CATCODE,CATNAME, CATEGORYCODE,RESULT"
            'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            'StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            'StrSql += vbCrLf + " BEGIN "
            'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATEGORYCODE, CATNAME, "
            'StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
            'StrSql += vbCrLf + " SELECT CATEGORYCODE, CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            'StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER, RESULT,1 RESULT1, 'R' COLHEAD,CATCODE "
            'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES_RATE WHERE COLHEAD <> 'VAT'  GROUP BY CATCODE,CATNAME, CATEGORYCODE,RESULT"
            ''StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            'StrSql += vbCrLf + " END "
            'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            'Cmd.ExecuteNonQuery()

            'StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            'StrSql += vbCrLf + " BEGIN "
            'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATNAME, "
            'StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "


            'StrSql += vbCrLf + " SELECT  CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            'StrSql += vbCrLf + " RESULT, RESULT1, 'L' COLHEAD,CATCODE"
            'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES  WHERE  CATCODE = 'G' AND COLHEAD <>'L'"
            'StrSql += vbCrLf + " END "
            'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            'Cmd.ExecuteNonQuery()

        End If

        If rbtmetalwisegrp.Checked = False Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATCODE,CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
            StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " , RESULT, RESULT1, COLHEAD) "
            StrSql += vbCrLf + " SELECT 'Z' AS CATCODE, 'SALES TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
            StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), SUM(PAYMENT)"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " ,0 DISCPER"
            StrSql += vbCrLf + " ,4 RESULT,2 RESULT1, 'S' COLHEAD "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES WHERE RESULT = 1 "
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        If rbtmetalwisegrp.Checked = False Then
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
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " ,COLHEAD,RESULT1) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END,"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " CASE WHEN DISCPER <> 0 THEN DISCPER ELSE NULL END DISCPER,"
            StrSql += vbCrLf + " COLHEAD, RESULT1 "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
            StrSql += vbCrLf + " WHERE RESULT1 = 1 "
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

        Else

            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,CATCODE,RESULT1,ORDERNO,COLHEAD1, RESULT2, COLHEAD2, TYPE) "
            StrSql += vbCrLf + " SELECT 'SALES' ,'T',CATCODE,1 RESULT1,0,'C', 1 RESULT2, '' COLHEAD2, '' TYPE "
            'StrSql += vbCrLf + " SELECT '  SALES' ,'L',CATCODE, RESULT1,1,'C' "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
            StrSql += "  GROUP BY CATCODE"
            'StrSql += "  ORDER BY COLHEAD "

            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (CATEGORYCODE, DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,DISCPER,COLHEAD,CATCODE,RESULT1,ORDERNO "
            StrSql += vbCrLf + " , RESULT2, COLHEAD2,  TYPE)"
            StrSql += vbCrLf + " SELECT CATEGORYCODE, '' + CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END,"
            StrSql += vbCrLf + " CASE WHEN DISCPER <> 0 THEN DISCPER ELSE NULL END DISCPER,"
            StrSql += vbCrLf + " COLHEAD,CATCODE, RESULT,1 "
            StrSql += vbCrLf + " ,1 AS RESULT2, 'A' AS COLHEAD2, '' TYPE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
            StrSql += vbCrLf + " WHERE COLHEAD IN ('L')"
            StrSql += vbCrLf + " AND CATNAME NOT LIKE('%(RT)%')  "
            StrSql += vbCrLf + " AND CATNAME NOT LIKE('%(WT)%') AND CATNAME NOT LIKE '%( WT)%'"
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            'StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            'StrSql += vbCrLf + "  BEGIN "
            'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALES(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
            'StrSql += vbCrLf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD, RESULT2, COLHEAD2, TYPE) "
            'StrSql += vbCrLf + "  SELECT CATCODE, 'SALES TOT' CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
            'StrSql += vbCrLf + "  SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
            'StrSql += vbCrLf + "  SUM(PAYMENT), 4 RESULT, 2 RESULT1,'Z' COLHEAD, 1 RESULT2,'Z' COLHEAD2, '' TYPE "
            'StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE RESULT = 1 "
            'StrSql += vbCrLf + "  END "
            'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            'Cmd.ExecuteNonQuery()

            '''ONLY RT CATNAME
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
            StrSql += vbCrLf + " SELECT CATEGORYCODE, CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,1 RESULT1, 'L' COLHEAD,CATCODE "
            StrSql += vbCrLf + " ,1 AS RESULT2, 'B' AS COLHEAD2, 'R' TYPE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES"
            'StrSql += vbCrLf + " WHERE  CATNAME LIKE('%(RT)%') AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
            StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND  CATNAME LIKE('%(RT)%') "
            StrSql += vbCrLf + " GROUP BY CATCODE,CATNAME, CATEGORYCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            '''RATE TOTAL
            StrSql = " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
            StrSql += vbCrLf + " SELECT '' CATEGORYCODE,'RATE TOTAL' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,1 RESULT1, 'R' COLHEAD,CATCODE "
            StrSql += vbCrLf + " ,1 AS RESULT2, 'B' AS COLHEAD2, 'RT' TYPE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES"
            'StrSql += vbCrLf + " WHERE  CATNAME LIKE('%(RT)%') AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
            StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND CATNAME LIKE('%(RT)%')  "
            StrSql += vbCrLf + " GROUP BY CATCODE"

            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            '''ONLY WT CATNAME
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
            StrSql += vbCrLf + " SELECT CATEGORYCODE, CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,1 RESULT1, 'L' COLHEAD,CATCODE "
            StrSql += vbCrLf + " ,1 AS RESULT2, 'C' AS COLHEAD2, 'W' TYPE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES"
            'StrSql += vbCrLf + " WHERE CATNAME LIKE('%(WT)%') OR CATNAME LIKE '% ( WT)%' AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
            StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND CATNAME LIKE('%( WT)%') OR CATNAME LIKE '% (WT)%' "
            StrSql += vbCrLf + " GROUP BY CATCODE,CATNAME, CATEGORYCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            ''WEIGHT TOTAL
            StrSql = " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
            StrSql += vbCrLf + " SELECT '' CATEGORYCODE, 'WEIGHT TOTAL' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,1 RESULT1, 'W' COLHEAD,CATCODE "
            StrSql += vbCrLf + " ,1 AS RESULT2, 'C' AS COLHEAD2, 'WT' TYPE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES"
            'StrSql += vbCrLf + " WHERE CATNAME LIKE('%(WT)%') OR CATNAME LIKE '% ( WT)%' AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
            StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND  CATNAME LIKE('%( WT)%') OR CATNAME LIKE '% (WT)%' "
            StrSql += vbCrLf + " GROUP BY CATCODE"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            ''''TOTAL 
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + "  BEGIN "
            StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "SASRPU(CATCODE, DESCRIPTION,ISSPCS, ISSGRSWT, ISSNETWT, "
            StrSql += vbCrLf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,  RESULT1, COLHEAD, RESULT2, COLHEAD2, TYPE) "

            StrSql += vbCrLf + "  SELECT CATCODE, 'TOTAL'DESCRIPTION,ISNULL(SUM(ISSPCS),0), ISNULL(SUM(ISSGRSWT),0),ISNULL(SUM(ISSNETWT),0), "
            StrSql += vbCrLf + "  ISNULL(SUM(RECPCS),0),ISNULL(SUM(RECGRSWT),0),ISNULL(SUM(RECNETWT),0), ISNULL(SUM(RECEIPT),0), "
            StrSql += vbCrLf + "  ISNULL(SUM(PAYMENT),0), 2 RESULT1,'Z' COLHEAD, 1 RESULT2, 'Z' COLHEAD2, '' TYPE "
            StrSql += vbCrLf + "  FROM TEMP" & systemId & "SASRPU "
            StrSql += vbCrLf + "  WHERE COLHEAD = 'L' AND RESULT2 = 1 AND DESCRIPTION NOT LIKE '%( WT)%'  AND DESCRIPTION NOT LIKE '%(RT)%' "
            StrSql += vbCrLf + "  AND DESCRIPTION NOT LIKE '% TOTAL%' AND ISNULL(COLHEAD2,'') <> '' AND COLHEAD2 NOT IN ('Z')"
            StrSql += vbCrLf + "  GROUP BY CATCODE"
            StrSql += vbCrLf + "  END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

        End If

    End Sub

    Private Sub ProcRepairGoldSales_MetalGroup()
        If (RPT_ME_SUM_DABS = True And rbtMetalwise.Checked = True) Then Exit Sub
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSSALESRD') DROP TABLE TEMP" & systemId & "ABSSALESRD"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT CATCODE "
        Else
            StrSql += vbCrLf + " SELECT METALID AS CATCODE "
        End If
        If rbtmetalwisegrp.Checked = True Then
            StrSql += vbCrLf + " , CATEGORYCODE   "
        End If
        StrSql += vbCrLf + " ,CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + " PAYMENT, AVERAGE, RATE,"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " DISCPER,"
        StrSql += vbCrLf + "  COLHEAD, 1 RESULT, RESULT1 "
        If rbtmetalwisegrp.Checked = True Then
            StrSql += vbCrLf + "  ,4 RESULT2, 'A' COLHEAD2, '' TYPE"
        End If
        StrSql += vbCrLf + "  INTO TEMP" & systemId & "ABSSALESRD FROM ("
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT I.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            End If
        Else
            StrSql += vbCrLf + " SELECT I.METALID, "
            StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + " I.CATCODE AS CATEGORYCODE,"
            ElseIf rbtMetalwise.Checked = True Then
                StrSql += vbCrLf + " I.CATCODE AS CATCODE,"
            End If
        End If
        StrSql += vbCrLf + " CATNAME, SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += vbCrLf + " SUM(AMOUNT) AS RECEIPT, 0 AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        StrSql += vbCrLf + " ELSE "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/ (CASE WHEN COUNT(RATE) = 0 THEN COUNT(RATE) END) ))) AS RATE"
        'If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),(SUM(DISC)/ CASE WHEN SUM(NETWT)>0 THEN SUM(NETWT) WHEN SUM(PCS)>0 THEN SUM(PCS) ELSE 1 END )) AS DISCPER"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),(SUM(DISC)/((CASE WHEN SUM(AMOUNT)=0 THEN 1 ELSE SUM(AMOUNT) END)/100))) AS DISCPER"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(3),'') COLHEAD "
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
            StrSql += vbCrLf + " ,(SELECT GRSWT FROM " & cnStockDb & "..ISSUE WHERE SNO=I.SNO "
            StrSql += vbCrLf + " AND SNO NOT IN(SELECT ISSSNO FROM " & cnStockDb & "..ISSMETAL"
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") ))GRSWT "
        End If

        StrSql += vbCrLf + " ,(SELECT NETWT FROM " & cnStockDb & "..ISSUE WHERE SNO=I.SNO "
        StrSql += vbCrLf + " AND SNO NOT IN(SELECT ISSSNO FROM " & cnStockDb & "..ISSMETAL"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") ))NETWT "
        If RPT_SEPVAT_DABS = False Then
            StrSql += vbCrLf + " ,AMOUNT+ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0)"
        Else
            StrSql += vbCrLf + " ,AMOUNT"
        End If

        If RPT_SEPSTUD_DABS Then
            StrSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
            StrSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
            'Else
            '    StrSql += vbCrLf + " +ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
            '    StrSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
        End If

        StrSql += vbCrLf + " AS AMOUNT,RATE"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,(ISNULL(FIN_DISCOUNT,0)+ISNULL(DISCOUNT,0)) AS DISC "
        StrSql += vbCrLf + " ,CATCODE,METALID"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        StrSql += vbCrLf + " I.TRANTYPE IN('RD','OD') AND ISNULL(I.CANCEL,'') = '' "
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")

        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT (SELECT SUM(PCS) FROM " & cnStockDb & "..ISSUE  WHERE SNO=I.ISSSNO)PCS"
        StrSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
        StrSql += vbCrLf + " ,SUM(GRSWT)NETWT"
        StrSql += vbCrLf + " ,NULL AS AMOUNT,RATE"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,0 AS DISC "
        StrSql += vbCrLf + " ,CATCODE,METALID"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSMETAL I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") "
        StrSql += vbCrLf + " AND ISSSNO  IN(SELECT SNO FROM " & cnStockDb & "..ISSUE "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND TRANTYPE IN('RD','OD') AND ISNULL(CANCEL,'') = '' ) "
        StrSql += vbCrLf + " GROUP BY CATCODE,ISSSNO ,SNO,AMOUNT,RATE,METALID "

        StrSql += vbCrLf + " )AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY I.CATCODE,C.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME , I.CATCODE "
        End If

        If RPT_SEPVAT_DABS Then
            ''TAX
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.STAXID) AS CATNAME, "
                ' ElseIf rbtmetalwisegrp.Checked = True Then
                '    StrSql += vbCrLf + " SELECT I.METALID, 'VAT' AS METALNAME, CATNAME +'  [''VAT'']', "
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME, I.CATCODE, CATNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(I.TAX+ISNULL(TT.TAXAMT,0)-ISNULL(ISS.TAX,0)) AS RECEIPT, 0 AS PAYMENT, 1 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
            StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'VAT') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
            StrSql += vbCrLf + " ON TT.SNO=I.SNO and TT.BATCHNO=I.BATCHNO "
            StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAX) TAX ,ISSSNO,STNITEMID  from " & cnStockDb & "..ISSSTONE GROUP BY ISSSNO,STNITEMID ) ISS "
            StrSql += vbCrLf + " ON ISS.ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('RD','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.STAXID HAVING SUM(I.TAX) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, I.CATCODE"
            End If

            '' SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.SSCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'SURCHARGE' AS METALNAME,CATNAME, I.CATCODE,"
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(SC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
            StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('RD','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SSCID HAVING SUM(SC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, I.CATCODE  HAVING SUM(SC) <> 0"
            End If

            '' ADDTIONAL SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.SASCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'ADDTIONALSURCHARGE' AS METALNAME,CATNAME, I.CATCODE, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(ADSC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
            StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('RD','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SASCID HAVING SUM(ADSC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, I.CATCODE HAVING SUM(ADSC) <> 0 "
            End If
        End If
        If RPT_SEPSTUD_DABS Then
            StrSql += vbCrLf + " UNION ALL "
            StrSql += vbCrLf + " SELECT CATCODE"
            If rbtCategoryWise.Checked = False Then
                StrSql += vbCrLf + ",METALNAME"
            End If
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + ", CATEGORYCODE"
            End If
            If rbtMetalwise.Checked = True Then
                StrSql += vbCrLf + " ,'' CATEGORYCODE"
            End If
            StrSql += vbCrLf + ", CATNAME"
            StrSql += vbCrLf + ", SUM(ISSPCS) AS ISSPCS, "
            StrSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
            StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " , COLHEAD FROM ("
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
                'If rbtmetalwisegrp.Checked = True Then
                '    StrSql += vbCrLf + " (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY  "
                '    StrSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATEGORYCODE  "
                'End If
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..ISSUE "
                ''strsql += vbcrlf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE)AS CATCODE, 'DIAMOND' AS CATNAME,  "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS METALNAME,  "
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=S.CATCODE)CATNAME, "
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + " (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=S.CATCODE)CATEGORYCODE, "
                End If

            End If
            StrSql += vbCrLf + " SUM(S.STNPCS) ISSPCS,SUM(S.STNWT) AS ISSGRSWT,SUM(S.STNWT) AS ISSNETWT,  "
            StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.STNAMT) AS RECEIPT,  "
            StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE,"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
            StrSql += vbCrLf + " 'C' COLHEAD  FROM " & cnStockDb & "..ISSSTONE S "
            StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('RD','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            'StrSql += StrFilter
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
            Else
                StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE "
            End If
            StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
            If rbtCategoryWise.Checked = False Then
                StrSql += vbCrLf + " ,METALNAME"
            End If
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + " ,CATEGORYCODE "
            End If
            StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        End If
        If RPT_SEPVAT_DABS Then
            'TAX
            StrSql += vbCrLf + " UNION ALL "

            StrSql += vbCrLf + " SELECT '' METALID"

            If rbtCategoryWise.Checked = False Then
                StrSql += vbCrLf + ",METALNAME "
            Else
                StrSql += vbCrLf + ",  CATCODE"
            End If

            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + " ,CATEGORYCODE"
            End If
            'If rbtMetalwise.Checked = True Then
            '    StrSql += vbCrLf + " ,'' CATEGORYCODE"
            'End If
            StrSql += vbCrLf + " , CATNAME,SUM(ISSPCS) AS ISSPCS,SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
            StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            StrSql += vbCrLf + " ,COLHEAD FROM ("
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..ISSUE "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
                'If chkCatShortname.Checked = True Then
                '    StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE = S.CATCODE)  "
                'Else
                '    StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE = S.CATCODE)  "
                'End If
                StrSql += vbCrLf + "(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.STAXID) AS CATNAME,  "
            Else
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..ISSUE "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS METALNAME,  "
                StrSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.STAXID) AS CATNAME, "
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + " (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = C.CATCODE) AS CATEGORYCODE, "
                End If

            End If
            StrSql += vbCrLf + " 0 ISSPCS,0 AS ISSGRSWT,0 AS ISSNETWT,  "
            StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.TAX) AS RECEIPT,  "
            StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
            StrSql += vbCrLf + " 'VAT' COLHEAD  FROM " & cnStockDb & "..ISSSTONE S "
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = S.CATCODE"
            StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('RD','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            'StrSql += StrFilter
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO,C.STAXID "
            Else
                StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE,C.STAXID, C.CATCODE    "
            End If
            StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
            If rbtCategoryWise.Checked = False Then
                StrSql += vbCrLf + " ,METALNAME"
            End If
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER"
            If rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " , CATEGORYCODE "

            StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        End If
        StrSql += vbCrLf + " ) X "
        'StrSql += vbCrLf + " WHERE CATNAME NOT LIKE('%(RT)%') AND CATNAME NOT LIKE '%VAT%' "
        'StrSql += vbCrLf + " AND  CATNAME NOT LIKE('%( WT)%') OR CATNAME NOT LIKE '%(WT)%' "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        '''''''''''' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''------------
        If rbtmetalwisegrp.Checked = True Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRD)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALESRD(CATEGORYCODE, CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
            StrSql += vbCrLf + " SELECT CATEGORYCODE, CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
            StrSql += vbCrLf + " ,4 AS RESULT2, 'A' AS COLHEAD2, '' TYPE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRD WHERE COLHEAD <> 'VAT'  GROUP BY CATCODE,CATNAME, CATEGORYCODE,RESULT"
            'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        

        If rbtmetalwisegrp.Checked = True Then

            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRD)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,CATCODE,RESULT1,ORDERNO,COLHEAD1, RESULT2, COLHEAD2, TYPE) "
            StrSql += vbCrLf + " SELECT 'ORDER/REPAIR' ,'L','ZZ'CATCODE,1 RESULT1,0,'D', 4 RESULT2, '' COLHEAD2, '' TYPE "
            'StrSql += vbCrLf + " SELECT '  SALES' ,'L',CATCODE, RESULT1,1,'C' "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRD "
            StrSql += "  GROUP BY CATCODE"
            'StrSql += "  ORDER BY COLHEAD "

            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (CATEGORYCODE, DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,DISCPER,COLHEAD,CATCODE,RESULT1,ORDERNO "
            StrSql += vbCrLf + " , RESULT2, COLHEAD2,  TYPE)"
            StrSql += vbCrLf + " SELECT CATEGORYCODE, '' + CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END,"
            StrSql += vbCrLf + " CASE WHEN DISCPER <> 0 THEN DISCPER ELSE NULL END DISCPER,"
            StrSql += vbCrLf + " COLHEAD, 'ZZ' CATCODE, RESULT,1 "
            StrSql += vbCrLf + " ,4 AS RESULT2, 'A' AS COLHEAD2, '' TYPE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRD "
            StrSql += vbCrLf + " WHERE COLHEAD IN ('L')"
            StrSql += vbCrLf + " AND CATNAME NOT LIKE('%(RT)%')  "
            StrSql += vbCrLf + " AND CATNAME NOT LIKE('%(WT)%') AND CATNAME NOT LIKE '%( WT)%'"
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            '''ONLY RT CATNAME
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRD)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
            StrSql += vbCrLf + " SELECT CATEGORYCODE, CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,1 RESULT1, 'L' COLHEAD,CATCODE "
            StrSql += vbCrLf + " ,4 AS RESULT2, 'B' AS COLHEAD2, 'R' TYPE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRD"
            'StrSql += vbCrLf + " WHERE  CATNAME LIKE('%(RT)%') AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
            StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND  CATNAME LIKE('%(RT)%') "
            StrSql += vbCrLf + " GROUP BY CATCODE,CATNAME, CATEGORYCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            '''RATE TOTAL
            StrSql = " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
            StrSql += vbCrLf + " SELECT '' CATEGORYCODE,'RATE TOTAL' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,1 RESULT1, 'R' COLHEAD,CATCODE "
            StrSql += vbCrLf + " ,4 AS RESULT2, 'B' AS COLHEAD2, 'RT' TYPE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRD"
            'StrSql += vbCrLf + " WHERE  CATNAME LIKE('%(RT)%') AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
            StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND CATNAME LIKE('%(RT)%')  "
            StrSql += vbCrLf + " GROUP BY CATCODE"

            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            '''ONLY WT CATNAME
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRD)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
            StrSql += vbCrLf + " SELECT CATEGORYCODE, CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,1 RESULT1, 'L' COLHEAD,CATCODE "
            StrSql += vbCrLf + " ,4 AS RESULT2, 'C' AS COLHEAD2, 'W' TYPE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRD"
            'StrSql += vbCrLf + " WHERE CATNAME LIKE('%(WT)%') OR CATNAME LIKE '% ( WT)%' AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
            StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND CATNAME LIKE('%( WT)%') OR CATNAME LIKE '% (WT)%' "
            StrSql += vbCrLf + " GROUP BY CATCODE,CATNAME, CATEGORYCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            ''WEIGHT TOTAL
            StrSql = " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
            StrSql += vbCrLf + " SELECT '' CATEGORYCODE, ' WEIGHT TOTAL' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,1 RESULT1, 'W' COLHEAD,CATCODE "
            StrSql += vbCrLf + " ,4 AS RESULT2, 'C' AS COLHEAD2, 'WT' TYPE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRD"
            'StrSql += vbCrLf + " WHERE CATNAME LIKE('%(WT)%') OR CATNAME LIKE '% ( WT)%' AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
            StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND  CATNAME LIKE('%( WT)%') OR CATNAME LIKE '% (WT)%' "
            StrSql += vbCrLf + " GROUP BY CATCODE"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            ''''TOTAL 
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRD)>0 "
            StrSql += vbCrLf + "  BEGIN "
            StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "SASRPU(CATCODE, DESCRIPTION,ISSPCS, ISSGRSWT, ISSNETWT, "
            StrSql += vbCrLf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,  RESULT1, COLHEAD, RESULT2, COLHEAD2, TYPE) "

            StrSql += vbCrLf + "  SELECT CATCODE, 'METAL TOTAL'DESCRIPTION,ISNULL(SUM(ISSPCS),0), ISNULL(SUM(ISSGRSWT),0),ISNULL(SUM(ISSNETWT),0), "
            StrSql += vbCrLf + "  ISNULL(SUM(RECPCS),0),ISNULL(SUM(RECGRSWT),0),ISNULL(SUM(RECNETWT),0), ISNULL(SUM(RECEIPT),0), "
            StrSql += vbCrLf + "  ISNULL(SUM(PAYMENT),0), 2 RESULT1,'Z' COLHEAD, 4 RESULT2, 'Z' COLHEAD2, '' TYPE "
            StrSql += vbCrLf + "  FROM TEMP" & systemId & "SASRPU "
            StrSql += vbCrLf + "  WHERE COLHEAD = 'L' AND RESULT2 = 4 AND DESCRIPTION NOT LIKE '%( WT)%'  AND DESCRIPTION NOT LIKE '%(RT)%' "
            StrSql += vbCrLf + "  AND DESCRIPTION NOT LIKE '% TOTAL%' AND ISNULL(COLHEAD2,'') <> '' AND COLHEAD2 NOT IN ('Z')"
            StrSql += vbCrLf + "  GROUP BY CATCODE"
            StrSql += vbCrLf + "  END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        '''SALES
        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0"
        StrSql += vbCrLf + " BEGIN "

        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        'StrSql += vbCrLf + " RESULT1, COLHEAD, CATCODE, CATEGORYCODE) "

        'StrSql += vbCrLf + " SELECT 'VAT' DESCRIPTION, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,0 AVERAGE, 0 AS RESULT1, 'T' COLHEAD ,'' CATCODE , ''"

        'StrSql += vbCrLf + " UNION ALL"

        'StrSql += vbCrLf + " SELECT 'SALES' DESCRIPTION, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,0 AVERAGE, 0 AS RESULT1, 'A' COLHEAD ,'' CATCODE , ''"
        'StrSql += vbCrLf + " UNION ALL"

        'StrSql += vbCrLf + " SELECT CATNAME as DESCRIPTION, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,0 AVERAGE, 0 AS RESULT1, 'T' COLHEAD , CATCODE,'' CATEGORYCODE "
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES WHERE COLHEAD IN('A')"
        'StrSql += vbCrLf + " UNION ALL"

        'StrSql += vbCrLf + " SELECT  CATNAME AS DESCRIPTION, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, '0'AVERAGE,"
        'StrSql += vbCrLf + " 1 RESULT1, 'V' COLHEAD,CATCODE , ''"
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES  WHERE  COLHEAD = 'VAT'"

        'StrSql += vbCrLf + " UNION ALL"

        StrSql += vbCrLf + " SELECT  'VAT' DESCRIPTION, ISNULL(SUM(ISSPCS),0), ISNULL(SUM(ISSGRSWT),0), ISNULL(SUM(ISSNETWT),0),"
        StrSql += vbCrLf + " ISNULL(SUM(RECPCS),0), ISNULL(SUM(RECGRSWT),0), ISNULL(SUM(RECNETWT),0), ISNULL(SUM(RECEIPT),0) SALESVAT, "
        StrSql += vbCrLf + " ISNULL(SUM(PAYMENT),0), 0 AVERAGE,2 RESULT1, 'M' COLHEAD, 'T' CATCODE , ''"
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES WHERE  COLHEAD = 'VAT' OR CATNAME LIKE '% VAT%'"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES WHERE  COLHEAD = 'VAT' "
        StrSql += vbCrLf + " GROUP BY RESULT1"
        StrSql += vbCrLf + " ORDER BY CATCODE, RESULT1 "
        StrSql += " END "
        Dim dtSalesVat As New DataTable
        Dim SalesVat As Decimal
        da = New OleDbDataAdapter(StrSql, cn)
        dtSalesVat.Clear()
        da.Fill(dtSalesVat)
        dtSalesVat.AcceptChanges()
        If dtSalesVat.Rows.Count > 0 Then
            SalesVat = Convert.ToDecimal(dtSalesVat.Rows(0).Item("SALESVAT").ToString)
        End If


        '''SALES RETURN
        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        'StrSql += vbCrLf + " RESULT1, COLHEAD, CATCODE, CATEGORYCODE) "
        'StrSql += vbCrLf + " SELECT 'SALES RETURN' DESCRIPTION, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,0 AVERAGE, 0 AS RESULT1, 'A' COLHEAD ,'' CATCODE ,''"
        'StrSql += vbCrLf + " UNION ALL"

        'StrSql += vbCrLf + " SELECT CATNAME as DESCRIPTION, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,0 AVERAGE, 0 AS RESULT1, 'T' COLHEAD , CATCODE ,''"
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN WHERE COLHEAD IN('A')"
        'StrSql += vbCrLf + " UNION ALL"

        'StrSql += vbCrLf + " SELECT  CATNAME AS DESCRIPTION, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, '0'AVERAGE,"
        'StrSql += vbCrLf + " 1 RESULT1, 'V' COLHEAD,CATCODE,'' CATEGORYCODE FROM TEMP" & systemId & "ABSSALESRETURN  WHERE  COLHEAD = 'VAT'"
        'StrSql += vbCrLf + " UNION ALL"

        StrSql += vbCrLf + " SELECT  'VAT' DESCRIPTION, ISNULL(SUM(ISSPCS),0), ISNULL(SUM(ISSGRSWT),0), ISNULL(SUM(ISSNETWT),0),"
        StrSql += vbCrLf + " ISNULL(SUM(RECPCS),0), ISNULL(SUM(RECGRSWT),0), ISNULL(SUM(RECNETWT),0), ISNULL(SUM(RECEIPT),0), "
        StrSql += vbCrLf + " ISNULL(SUM(PAYMENT),0)SALESRETVAT, 0 AVERAGE,2 RESULT1, 'M' COLHEAD, 'T' CATCODE ,''"
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN  WHERE  COLHEAD = 'VAT' OR CATNAME LIKE '% VAT%'"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN  WHERE  COLHEAD = 'VAT' "
        StrSql += vbCrLf + " GROUP BY RESULT1"
        StrSql += vbCrLf + " ORDER BY CATCODE, RESULT1 "
        StrSql += " END "

        Dim dtSalesReturnVat As New DataTable
        Dim SalesRetVat As Decimal
        da = New OleDbDataAdapter(StrSql, cn)
        dtSalesReturnVat.Clear()
        da.Fill(dtSalesReturnVat)
        dtSalesReturnVat.AcceptChanges()
        If dtSalesReturnVat.Rows.Count > 0 Then
            SalesRetVat = Convert.ToDecimal(dtSalesReturnVat.Rows(0).Item("SALESRETVAT").ToString)
        End If

        SalesVat = SalesVat - SalesRetVat

        StrSql = " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        StrSql += vbCrLf + " RESULT1, COLHEAD, CATCODE, CATEGORYCODE, RESULT2, COLHEAD2, TYPE) "

        If SalesVat >= 0 Then
            StrSql += " SELECT 'VAT   [SALES - SALES RETURN]' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
            StrSql += " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, '" & SalesVat & "' RECEIPT, 0 PAYMENT, '0' AVERAGE,  "
            StrSql += " 1 RESULT1, 'V' COLHEAD, 'ZZZ' CATCODE,'' CATEGORYCODE, 5, '', 'Z'"
        ElseIf SalesRetVat > SalesVat Then
            StrSql += " SELECT 'VAT   [SALES - SALES RETURN]' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
            StrSql += " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, '" & -1 * SalesVat & "' RECEIPT, 0 PAYMENT, '0' AVERAGE,  "
            StrSql += " 1 RESULT1, 'V' COLHEAD, 'ZZZ' CATCODE,'' CATEGORYCODE, 5, '', 'Z'"
        End If
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        'StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "SASRPUNEW') DROP TABLE TEMP" & systemId & "SASRPUNEW"
        'StrSql += "  SELECT * INTO TEMP" & systemId & "SASRPUNEW FROM TEMP" & systemId & "SASRPU"
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

        'StrSql = " DELETE FROM TEMP" & systemId & "SASRPU"
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

        StrSql = " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        StrSql += vbCrLf + " CATCODE, COLHEAD, RESULT2, COLHEAD2, TYPE) "

        StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = CATCODE)AS DESCRIPTION,"
        StrSql += vbCrLf + " 0 ISSPCS,0 ISSGRSWT,0 ISSNETWT,0 RECPCS,0 RECGRSWT,0 RECNETWT,0 RECEIPT,0 PAYMENT,'0' AVERAGE, CATCODE,'T' COLHEAD, 0 RESULT2, '' COLHEAD2, ''"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPU WHERE isnull(CATCODE,'')<>'' AND ISNULL(CATCODE,'') NOT IN ('Z')  GROUP BY CATCODE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()



        If rbtmetalwisegrp.Checked = True Then
            StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = "
            StrSql += " 'TEMP" & systemId & "SASRPUNEW') "
            StrSql += " DROP TABLE TEMP" & systemId & "SASRPUNEW "
            StrSql += vbCrLf + " SELECT * INTO TEMP" & systemId & "SASRPUNEW"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPU "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
            StrSql += " AVERAGE VARCHAR(100),"
            StrSql += " DISCPER NUMERIC(20,2),"
            StrSql += " COLHEAD VARCHAR(3),"
            StrSql += " RESULT1 NUMERIC(15,2),"
            StrSql += " CATCODE VARCHAR(5),"
            StrSql += " CATEGORYCODE VARCHAR(30),"
            StrSql += " ORDERNO INT,"
            StrSql += " COLHEAD1 VARCHAR(5),"
            StrSql += " AVGRATE DECIMAL(15,2),"
            If rbtmetalwisegrp.Checked = True Then
                StrSql += " RESULT2 INT,"
                StrSql += " COLHEAD2 VARCHAR(5),"
                StrSql += " TYPE VARCHAR(30),"
            End If
            StrSql += " SNO INT IDENTITY(1,1))"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()


            StrSql = " INSERT INTO TEMP" & systemId & "SASRPU ("
            StrSql += " DESCRIPTION, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, AVERAGE"
            StrSql += " ,DISCPER, COLHEAD, RESULT1, CATCODE, CATEGORYCODE, ORDERNO, COLHEAD1, RESULT2, COLHEAD2, TYPE, AVGRATE )"
            StrSql += " SELECT "
            StrSql += " DESCRIPTION, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, AVERAGE"
            StrSql += " ,DISCPER, COLHEAD, RESULT1, CATCODE, CATEGORYCODE, ORDERNO, COLHEAD1, RESULT2, COLHEAD2, TYPE, AVGRATE "
            StrSql += " FROM TEMP" & systemId & "SASRPUNEW ORDER  BY CATCODE, RESULT2, COLHEAD2"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

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
        StrSql += vbCrLf + " PAYMENT, AVERAGE, RATE,"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " DISCPER,"
        StrSql += vbCrLf + "  COLHEAD, RESULT, RESULT1 INTO TEMP" & systemId & "ABSSALES FROM ("
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT I.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            End If
        Else
            StrSql += vbCrLf + " SELECT I.METALID, "
            StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
            StrSql += vbCrLf + " CATNAME,"
        End If
        StrSql += vbCrLf + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += vbCrLf + " SUM(AMOUNT) AS RECEIPT, 0 AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        StrSql += vbCrLf + " ELSE "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE"
        'If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),(SUM(DISC)/ CASE WHEN SUM(NETWT)>0 THEN SUM(NETWT) WHEN SUM(PCS)>0 THEN SUM(PCS) ELSE 1 END )) AS DISCPER"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),(SUM(DISC)/(SUM(NULLIF(AMOUNT,0))/100))) AS DISCPER"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(3),'') COLHEAD "
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
            StrSql += vbCrLf + " ,(SELECT GRSWT FROM " & cnStockDb & "..ISSUE WHERE SNO=I.SNO "
            StrSql += vbCrLf + " AND SNO NOT IN(SELECT ISSSNO FROM " & cnStockDb & "..ISSMETAL"
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") ))GRSWT "
        End If
        StrSql += vbCrLf + " ,(SELECT NETWT FROM " & cnStockDb & "..ISSUE WHERE SNO=I.SNO "
        StrSql += vbCrLf + " AND SNO NOT IN(SELECT ISSSNO FROM " & cnStockDb & "..ISSMETAL"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") ))NETWT "
        If RPT_SEPVAT_DABS = False Then
            StrSql += vbCrLf + " ,AMOUNT+ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0)"
        Else
            StrSql += vbCrLf + " ,AMOUNT"
        End If
        If RPT_SEPSTUD_DABS Then
            StrSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
            StrSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
            'Else
            '    StrSql += vbCrLf + " +ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
            '    StrSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
        End If

        StrSql += vbCrLf + " AS AMOUNT,RATE"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,(ISNULL(FIN_DISCOUNT,0)+ISNULL(DISCOUNT,0)) AS DISC "
        StrSql += vbCrLf + " ,CATCODE,METALID"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        StrSql += vbCrLf + " I.TRANTYPE IN('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")

        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT (SELECT SUM(PCS) FROM " & cnStockDb & "..ISSUE  WHERE SNO=I.ISSSNO)PCS"
        StrSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
        StrSql += vbCrLf + " ,SUM(GRSWT)NETWT"
        StrSql += vbCrLf + " ,NULL AS AMOUNT,RATE"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,0 AS DISC "
        StrSql += vbCrLf + " ,CATCODE,METALID"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSMETAL I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") "
        StrSql += vbCrLf + " AND ISSSNO  IN(SELECT SNO FROM " & cnStockDb & "..ISSUE "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND TRANTYPE IN('SA','OD') AND ISNULL(CANCEL,'') = '' ) "
        StrSql += vbCrLf + " GROUP BY CATCODE,ISSSNO ,SNO,AMOUNT,RATE,METALID "

        StrSql += vbCrLf + " )AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY I.CATCODE,C.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME "
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
                StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME,CATNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(I.TAX+ISNULL(TT.TAXAMT,0)-ISNULL(ISS.TAX,0)) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
            StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
            StrSql += vbCrLf + " ON TT.SNO=I.SNO and TT.BATCHNO=I.BATCHNO "
            StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAX) TAX ,ISSSNO,STNITEMID  from " & cnStockDb & "..ISSSTONE GROUP BY ISSSNO,STNITEMID ) ISS "
            StrSql += vbCrLf + " ON ISS.ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.STAXID HAVING SUM(I.TAX) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME"
            End If

            '' SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.SSCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'SURCHARGE' AS METALNAME,CATNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(SC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
            StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SSCID HAVING SUM(SC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME  HAVING SUM(SC) <> 0"
            End If

            '' ADDTIONAL SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.SASCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'ADDTIONALSURCHARGE' AS METALNAME,CATNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(ADSC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
            StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SASCID HAVING SUM(ADSC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME HAVING SUM(ADSC) <> 0 "
            End If
        End If
        If RPT_SEPSTUD_DABS Then
            StrSql += vbCrLf + " UNION ALL "
            StrSql += vbCrLf + " SELECT CATCODE"
            If rbtCategoryWise.Checked = False Then
                StrSql += vbCrLf + ",METALNAME"
            End If
            StrSql += vbCrLf + ", CATNAME"
            StrSql += vbCrLf + ", SUM(ISSPCS) AS ISSPCS, "
            StrSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
            StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " , COLHEAD FROM ("
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
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS METALNAME,  "
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=S.CATCODE)CATNAME, "
            End If
            StrSql += vbCrLf + " SUM(S.STNPCS) ISSPCS,SUM(S.STNWT) AS ISSGRSWT,SUM(S.STNWT) AS ISSNETWT,  "
            StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.STNAMT) AS RECEIPT,  "
            StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE,"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
            StrSql += vbCrLf + " 'D' COLHEAD  FROM " & cnStockDb & "..ISSSTONE S "
            StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SA','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            'StrSql += StrFilter
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
            Else
                StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE "
            End If
            StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
            If rbtCategoryWise.Checked = False Then
                StrSql += vbCrLf + " ,METALNAME"
            End If
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        End If
        If RPT_SEPVAT_DABS Then
            'TAX
            StrSql += vbCrLf + " UNION ALL "
            StrSql += vbCrLf + " SELECT CATCODE"
            If rbtCategoryWise.Checked = False Then
                StrSql += vbCrLf + ",METALNAME"
            End If
            StrSql += vbCrLf + ", CATNAME,SUM(ISSPCS) AS ISSPCS,SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
            StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            StrSql += vbCrLf + " ,COLHEAD FROM ("
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..ISSUE "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
                'If chkCatShortname.Checked = True Then
                '    StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE = S.CATCODE)  "
                'Else
                '    StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE = S.CATCODE)  "
                'End If
                StrSql += vbCrLf + "(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.STAXID) AS CATNAME,  "
            Else
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..ISSUE "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS METALNAME,  "
                StrSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.STAXID) AS CATNAME, "
            End If
            StrSql += vbCrLf + " 0 ISSPCS,0 AS ISSGRSWT,0 AS ISSNETWT,  "
            StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.TAX) AS RECEIPT,  "
            StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
            StrSql += vbCrLf + " 'D' COLHEAD  FROM " & cnStockDb & "..ISSSTONE S "
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = S.CATCODE"
            StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SA','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            'StrSql += StrFilter
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO,C.STAXID  "
            Else
                StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE,C.STAXID  "
            End If
            StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
            If rbtCategoryWise.Checked = False Then
                StrSql += vbCrLf + " ,METALNAME"
            End If
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        End If
        StrSql += vbCrLf + " ) X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If rbtmetalwisegrp.Checked = False Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " DISCPER, "
            StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD) "
            StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
            StrSql += vbCrLf + " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, "
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " 0 DISCPER, "
            StrSql += vbCrLf + " 0 RESULT, 0 RESULT1, 'T' COLHEAD "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES"
            'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
            StrSql += vbCrLf + " SELECT  CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES GROUP BY CATCODE,CATNAME"
            'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        If rbtmetalwisegrp.Checked = False Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATCODE,CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
            StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " , RESULT, RESULT1, COLHEAD) "
            StrSql += vbCrLf + " SELECT 'Z' AS CATCODE, 'SALES TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
            StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), SUM(PAYMENT)"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " ,0 DISCPER"
            StrSql += vbCrLf + " ,4 RESULT,2 RESULT1, 'S' COLHEAD "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES WHERE RESULT = 1 "
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        If rbtmetalwisegrp.Checked = False Then
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
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " ,COLHEAD,RESULT1) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END,"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " CASE WHEN DISCPER <> 0 THEN DISCPER ELSE NULL END DISCPER,"
            StrSql += vbCrLf + " COLHEAD, RESULT1 "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
            StrSql += vbCrLf + " WHERE RESULT1 = 1 "
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else

            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,CATCODE,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT '  SALES' ,'L',CATCODE, RESULT1,0,'C' "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
            'StrSql += " WHERE RESULT1 = 1"
            StrSql += "  ORDER BY COLHEAD "

            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,DISCPER,COLHEAD,CATCODE,RESULT1,ORDERNO) "
            StrSql += vbCrLf + " SELECT '' + CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END,"
            StrSql += vbCrLf + " CASE WHEN DISCPER <> 0 THEN DISCPER ELSE NULL END DISCPER,"
            StrSql += vbCrLf + " COLHEAD,CATCODE, RESULT1,1 "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
            StrSql += vbCrLf + " WHERE RESULT1 = 1 AND COLHEAD='L'"
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,CATCODE,'L' COLHEAD,1,2,'M' COLHEAD1 "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPU GROUP BY CATCODE,COLHEAD,RESULT1"
            StrSql += vbCrLf + " ORDER BY RESULT1"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

    End Sub

    Private Sub ProcSalesAndOrder()
        If (RPT_ME_SUM_DABS = True And rbtMetalwise.Checked = True) Then Exit Sub
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSSALES') DROP TABLE TEMP" & systemId & "ABSSALES"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT CATCODE, "
        Else
            StrSql += vbCrLf + " SELECT METALID AS CATCODE, "
        End If
        StrSql += vbCrLf + " CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + " PAYMENT, AVERAGE, RATE,"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " DISCPER,"
        StrSql += vbCrLf + "  STYPE,COLHEAD, RESULT, RESULT1 INTO TEMP" & systemId & "ABSSALES FROM ("
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT I.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            End If
        Else
            StrSql += vbCrLf + " SELECT I.METALID, "
            StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
            StrSql += vbCrLf + " CATNAME,"
        End If
        StrSql += vbCrLf + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += vbCrLf + " SUM(AMOUNT) AS RECEIPT, 0 AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        StrSql += vbCrLf + " ELSE "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE"
        'If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),(SUM(DISC)/ CASE WHEN SUM(NETWT)>0 THEN SUM(NETWT) WHEN SUM(PCS)>0 THEN SUM(PCS) ELSE 1 END )) AS DISCPER"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),(SUM(DISC)/(SUM(AMOUNT)/100))) AS DISCPER"
        StrSql += vbCrLf + " ,STYPE,CONVERT(VARCHAR(3),'') COLHEAD "
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
        StrSql += vbCrLf + " AS AMOUNT,RATE"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,(ISNULL(FIN_DISCOUNT,0)+ISNULL(DISCOUNT,0)) AS DISC "
        StrSql += vbCrLf + " ,CATCODE,METALID,ISNULL(O.STYPE ,'S') STYPE"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " LEFT JOIN (SELECT 'O' AS STYPE,ODSNO,ODBATCHNO FROM " & cnAdminDb & "..ORMAST GROUP BY ODSNO,ODBATCHNO) O ON O.ODSNO=I.SNO and O.ODBATCHNO=I.BATCHNO "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        StrSql += vbCrLf + " I.TRANTYPE IN('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
        StrSql += vbCrLf + " )AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY I.CATCODE,C.METALID,I.STYPE "
        Else
            StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME,I.STYPE "
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
                StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME,CATNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(I.TAX+ISNULL(TT.TAXAMT,0)-ISNULL(ISS.TAX,0)) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
            StrSql += vbCrLf + " ,ISNULL(O.STYPE,'S')STYPE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
            StrSql += vbCrLf + " ON TT.SNO=I.SNO and TT.BATCHNO=I.BATCHNO "
            StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAX) TAX ,ISSSNO,STNITEMID  from " & cnStockDb & "..ISSSTONE GROUP BY ISSSNO,STNITEMID ) ISS "
            StrSql += vbCrLf + " ON ISS.ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')"
            StrSql += vbCrLf + " LEFT JOIN (SELECT 'O' AS STYPE,ODSNO,ODBATCHNO FROM " & cnAdminDb & "..ORMAST GROUP BY ODSNO,ODBATCHNO) O ON O.ODSNO=I.SNO and O.ODBATCHNO=I.BATCHNO "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.STAXID,O.STYPE HAVING SUM(I.TAX) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME,O.STYPE"
            End If

            '' SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.SSCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'SURCHARGE' AS METALNAME,CATNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(SC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
            StrSql += vbCrLf + " ,ISNULL(O.STYPE,'S')STYPE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " LEFT JOIN (SELECT 'O' AS STYPE,ODSNO,ODBATCHNO FROM " & cnAdminDb & "..ORMAST GROUP BY ODSNO,ODBATCHNO) O ON O.ODSNO=I.SNO and O.ODBATCHNO=I.BATCHNO "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE,C.SSCID,O.STYPE HAVING SUM(SC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME,O.STYPE  HAVING SUM(SC) <> 0"
            End If

            '' ADDTIONAL SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.SASCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'ADDTIONALSURCHARGE' AS METALNAME,CATNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(ADSC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
            StrSql += vbCrLf + " ,ISNULL(O.STYPE,'S')STYPE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " LEFT JOIN (SELECT 'O' AS STYPE,ODSNO,ODBATCHNO FROM " & cnAdminDb & "..ORMAST GROUP BY ODSNO,ODBATCHNO) O ON O.ODSNO=I.SNO and O.ODBATCHNO=I.BATCHNO "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SASCID,O.STYPE HAVING SUM(ADSC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME,O.STYPE HAVING SUM(ADSC) <> 0 "
            End If
        End If
        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT CATCODE"
        If rbtCategoryWise.Checked = False Then
            StrSql += vbCrLf + ",METALNAME"
        End If
        StrSql += vbCrLf + ", CATNAME"
        StrSql += vbCrLf + ", SUM(ISSPCS) AS ISSPCS, "
        StrSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
        StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER"
        StrSql += vbCrLf + " ,STYPE, COLHEAD FROM ("
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
            StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS METALNAME,  "
            StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=S.CATCODE)CATNAME, "
        End If
        StrSql += vbCrLf + " SUM(S.STNPCS) ISSPCS,SUM(S.STNWT) AS ISSGRSWT,SUM(S.STNWT) AS ISSNETWT,  "
        StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.STNAMT) AS RECEIPT,  "
        StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE,"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
        StrSql += vbCrLf + " ISNULL(O.STYPE,'S')STYPE,'D' COLHEAD  FROM " & cnStockDb & "..ISSSTONE S "
        StrSql += vbCrLf + " LEFT JOIN (SELECT 'O' AS STYPE,ODSNO,ODBATCHNO FROM " & cnAdminDb & "..ORMAST GROUP BY ODSNO,ODBATCHNO) O ON O.ODSNO=S.ISSSNO and O.ODBATCHNO=S.BATCHNO "
        StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SA','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
        'StrSql += StrFilter
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO,O.STYPE  "
        Else
            StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE,O.STYPE "
        End If
        StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
        If rbtCategoryWise.Checked = False Then
            StrSql += vbCrLf + " ,METALNAME"
        End If
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
        StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT,STYPE"
        If RPT_SEPVAT_DABS Then
            StrSql += vbCrLf + " UNION ALL "
            StrSql += vbCrLf + " SELECT CATCODE"
            If rbtCategoryWise.Checked = False Then
                StrSql += vbCrLf + ",METALNAME"
            End If
            StrSql += vbCrLf + ", CATNAME,SUM(ISSPCS) AS ISSPCS,SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
            StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            StrSql += vbCrLf + " ,STYPE,COLHEAD FROM ("
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..ISSUE "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
                'If chkCatShortname.Checked = True Then
                '    StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE = S.CATCODE)  "
                'Else
                '    StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE = S.CATCODE)  "
                'End If
                StrSql += vbCrLf + "(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.STAXID) AS CATNAME,  "
            Else
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..ISSUE "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS METALNAME,  "
                StrSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.STAXID) AS CATNAME, "
            End If
            StrSql += vbCrLf + " 0 ISSPCS,0 AS ISSGRSWT,0 AS ISSNETWT,  "
            StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.TAX) AS RECEIPT,  "
            StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
            StrSql += vbCrLf + " ISNULL(O.STYPE,'S')STYPE,'D' COLHEAD  FROM " & cnStockDb & "..ISSSTONE S "
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = S.CATCODE"
            StrSql += vbCrLf + " LEFT JOIN (SELECT 'O' AS STYPE,ODSNO,ODBATCHNO FROM " & cnAdminDb & "..ORMAST GROUP BY ODSNO,ODBATCHNO) O ON O.ODSNO=S.ISSSNO and O.ODBATCHNO=S.BATCHNO "
            StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SA','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            'StrSql += StrFilter
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO,C.STAXID,O.STYPE  "
            Else
                StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE,C.STAXID,O.STYPE  "
            End If
            StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
            If rbtCategoryWise.Checked = False Then
                StrSql += vbCrLf + " ,METALNAME"
            End If
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT,STYPE"
        End If
        StrSql += vbCrLf + " ) X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If rbtmetalwisegrp.Checked = False Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " DISCPER, "
            StrSql += vbCrLf + " RESULT, RESULT1,STYPE, COLHEAD) "
            StrSql += vbCrLf + " SELECT CASE WHEN ISNULL(STYPE,'') ='O' THEN 'ORDER ' ELSE 'STOCK ' END +'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
            StrSql += vbCrLf + " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, "
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " 0 DISCPER, "
            StrSql += vbCrLf + " 0 RESULT, 0 RESULT1,STYPE, 'T' COLHEAD "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES GROUP BY STYPE"
            'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            StrSql += vbCrLf + " RESULT, RESULT1,STYPE, COLHEAD,CATCODE) "
            StrSql += vbCrLf + " SELECT  CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,1 RESULT,1 RESULT1,STYPE, 'L' COLHEAD,CATCODE "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES GROUP BY CATCODE,CATNAME,STYPE"
            'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        If rbtmetalwisegrp.Checked = False Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATCODE,CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
            StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " , RESULT, RESULT1,STYPE, COLHEAD) "
            StrSql += vbCrLf + " SELECT 'Z' AS CATCODE, CASE WHEN ISNULL(STYPE,'') ='O' THEN 'ORDER ' ELSE 'DIRECT ' END +'SALES TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
            StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), SUM(PAYMENT)"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " ,0 DISCPER"
            StrSql += vbCrLf + " ,4 RESULT,2 RESULT1,STYPE, 'S' COLHEAD "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES WHERE RESULT = 1  GROUP BY STYPE"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        If rbtmetalwisegrp.Checked = False Then
            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            'strsql += vbcrlf + " (DESCRIPTION, COLHEAD) VALUES('SALES','T') "
            StrSql += vbCrLf + " (DESCRIPTION,COLHEAD) "
            StrSql += vbCrLf + " SELECT CASE WHEN ISNULL(STYPE,'') ='O' THEN 'ORDER ' ELSE 'STOCK ' END +'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES  "
            StrSql += " WHERE RESULT1 = 1 AND ISNULL(STYPE,'') <>'O' GROUP BY STYPE"
            'StrSql += " AND COLHEAD <> 'D'"
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " ,COLHEAD,RESULT1) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END,"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " CASE WHEN DISCPER <> 0 THEN DISCPER ELSE NULL END DISCPER,"
            StrSql += vbCrLf + " COLHEAD, RESULT1 "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
            StrSql += vbCrLf + " WHERE RESULT1 = 1 AND ISNULL(STYPE,'')<>'O'"
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"

            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            'strsql += vbcrlf + " (DESCRIPTION, COLHEAD) VALUES('SALES','T') "
            StrSql += vbCrLf + " (DESCRIPTION,COLHEAD) "
            StrSql += vbCrLf + " SELECT CASE WHEN ISNULL(STYPE,'') ='O' THEN 'ORDER ' ELSE 'STOCK ' END +'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES  "
            StrSql += " WHERE RESULT1 = 1 AND ISNULL(STYPE,'') ='O' GROUP BY STYPE"
            'StrSql += " AND COLHEAD <> 'D'"
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " ,COLHEAD,RESULT1) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END,"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " CASE WHEN DISCPER <> 0 THEN DISCPER ELSE NULL END DISCPER,"
            StrSql += vbCrLf + " COLHEAD, RESULT1 "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
            StrSql += vbCrLf + " WHERE RESULT1 = 1 AND ISNULL(STYPE,'') ='O'"
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else

            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,CATCODE,RESULT1,ORDERNO,COLHEAD1, RESULT2, COLHEAD2, TYPE) "
            StrSql += vbCrLf + " SELECT '  SALES' ,'L',CATCODE, RESULT1,0,'C', 1 RESULT2, '' COLHEAD2, '' TYPE  "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES"
            'StrSql += " WHERE RESULT1 = 1"
            StrSql += "  ORDER BY COLHEAD "

            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,DISCPER,COLHEAD,CATCODE,RESULT1,ORDERNO) "
            StrSql += vbCrLf + " SELECT '' + CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END,"
            StrSql += vbCrLf + " CASE WHEN DISCPER <> 0 THEN DISCPER ELSE NULL END DISCPER,"
            StrSql += vbCrLf + " COLHEAD,CATCODE, RESULT1,1 "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
            StrSql += vbCrLf + " WHERE RESULT1 = 1 AND COLHEAD='L'  "
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"

            'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            'StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,CATCODE,RESULT1,ORDERNO,COLHEAD1) "
            'StrSql += vbCrLf + " SELECT CASE WHEN ISNULL(STYPE,'') ='O' THEN 'ORDER ' ELSE 'DIRECT ' END +'  SALES' ,'L',CATCODE, RESULT1,0,'C' "
            'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES WHERE ISNULL(STYPE,'') ='O'"
            ''StrSql += " WHERE RESULT1 = 1"
            'StrSql += "  ORDER BY COLHEAD "
            'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            'StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,DISCPER,COLHEAD,CATCODE,RESULT1,ORDERNO) "
            'StrSql += vbCrLf + " SELECT '' + CATNAME, "
            'StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            'StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            'StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            'StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            'StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            'StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            'StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            'StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            'StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END,"
            'StrSql += vbCrLf + " CASE WHEN DISCPER <> 0 THEN DISCPER ELSE NULL END DISCPER,"
            'StrSql += vbCrLf + " COLHEAD,CATCODE, RESULT1,1 "
            'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
            'StrSql += vbCrLf + " WHERE RESULT1 = 1 AND COLHEAD='L' AND  ISNULL(STYPE,'') ='O' "
            'StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,CATCODE,'L' COLHEAD,1,2,'M' COLHEAD1 "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPU GROUP BY CATCODE,COLHEAD,RESULT1"
            StrSql += vbCrLf + " ORDER BY RESULT1"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            
        End If

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
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
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
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
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
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
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
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
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
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
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
            StrSql += StrUseridFtr

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
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND S.ISSSNO IN ( SELECT SNO FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND I.METALID='" + METALID + "' "
        StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SA','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
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

    Private Sub ProcSalesReturn123()
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSSALESRETURN') DROP TABLE TEMP" & systemId & "ABSSALESRETURN"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + "  SELECT CATCODE, "
        Else

            StrSql += vbCrLf + "  SELECT METALID AS CATCODE,"
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + " CATEGORYCODE,"
            End If
        End If
        StrSql += vbCrLf + "  CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, "
        StrSql += vbCrLf + "  RECGRSWT, RECNETWT, RECEIPT, "
        '''MODIFIED BY M
        'StrSql += vbCrLf + "  PAYMENT, AVERAGE, RATE, 'L' COLHEAD, RESULT, 4 RESULT1  INTO TEMP" & systemId & "ABSSALESRETURN FROM ("
        StrSql += vbCrLf + "  PAYMENT, AVERAGE, RATE,  COLHEAD, RESULT, 4 RESULT1  "
        If rbtmetalwisegrp.Checked = True Then
            StrSql += vbCrLf + "  ,2 RESULT2, 'D' COLHEAD2, '' TYPE"
        End If
        StrSql += vbCrLf + "  INTO TEMP" & systemId & "ABSSALESRETURN FROM ("

        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + "  SELECT R.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + "  (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            End If
        ElseIf rbtmetalwisegrp.Checked Then
            StrSql += vbCrLf + "  SELECT R.METALID, "
            StrSql += vbCrLf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            StrSql += vbCrLf + "  R.CATCODE AS CATEGORYCODE, "
        Else
            StrSql += vbCrLf + "  SELECT R.METALID, "
            StrSql += vbCrLf + "  (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = R.METALID)AS CATNAME, "
        End If
        StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
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
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
        StrSql += vbCrLf + " )AS R"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY R.CATCODE,C.METALID "
        ElseIf rbtmetalwisegrp.Checked Then
            StrSql += vbCrLf + " GROUP BY R.CATCODE,R.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY R.METALID "
        End If

        If RPT_SEPVAT_DABS Then
            StrSql += vbCrLf + "  UNION ALL "
            ''TAX
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.STAXID) AS CATNAME, "
            Else
                StrSql += vbCrLf + "  SELECT R.METALID, 'SALES RETURN TAX' AS CATNAME, "
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + "  R.CATCODE AS CATEGORYCODE,"
                End If
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(TAX+ISNULL(TT.TAXAMT,0)) AS PAYMENT, 1 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'VAT') COLHEAD "
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
            StrSql += vbCrLf + " ON TT.SNO=R.SNO and TT.BATCHNO=R.BATCHNO "
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE,R.CASHID,C.STAXID HAVING SUM(TAX) <> 0"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID,R.CASHID, R.CATCODE "
            End If
            StrSql += vbCrLf + "  UNION ALL "
            ''SURCHARGE
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.SSCID) AS CATNAME, "
            Else

                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNSURCHARGE' AS CATNAME, "
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + "  R.CATCODE AS CATEGORYCODE,"
                End If
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
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE,R.CASHID,C.SSCID HAVING SUM(SC) <> 0"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID,R.CASHID, R.CATCODE HAVING SUM(SC) <> 0 "
            End If
            StrSql += vbCrLf + "  UNION ALL "

            ''ADDTIONAL SURCHARGE
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.SASCID) AS CATNAME, "
            Else

                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNSURCHARGE' AS CATNAME, "
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + "  R.CATCODE AS CATEGORYCODE,"
                End If
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(ADSC) AS PAYMENT, 1 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE,R.CASHID, C.SASCID HAVING SUM(ADSC) <> 0"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID,R.CASHID, R.CATCODE HAVING SUM(ADSC) <> 0 "
            End If

            StrSql += vbCrLf + "  UNION ALL "


            StrSql += vbCrLf + "  SELECT  CATCODE,CATNAME "
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + " ,CATEGORYCODE"
            End If
            StrSql += vbCrLf + "  , SUM(ISSPCS) AS ISSPCS, SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
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
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + " '' CATEGORYCODE,"
                End If
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS,  0 AS ISSGRSWT, 0 AS ISSNETWT,  "
            StrSql += vbCrLf + "  SUM(S.STNPCS) AS RECPCS,  SUM(S.STNWT)  AS RECGRSWT,  SUM(S.STNWT)  AS RECNETWT,  "
            StrSql += vbCrLf + "  0 AS RECEIPT,  SUM(S.STNAMT) AS PAYMENT,  "
            StrSql += vbCrLf + "  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            StrSql += vbCrLf + "  'D' COLHEAD  FROM " & cnStockDb & "..RECEIPTSTONE S "
            StrSql += vbCrLf + "  WHERE S.TRANTYPE='SR' AND S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + "  " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(CANCEL,'') = ''  AND TRANTYPE = 'SR' AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + "  AND  (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            StrSql += vbCrLf + "  AND COMPANYID IN (" & SelectedCompanyId & ")"
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY S.CATCODE,S.BATCHNO  "
            Else
                StrSql += vbCrLf + "  GROUP BY S.BATCHNO, S.CATCODE  "
            End If
            StrSql += vbCrLf + "  ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + "  ,CATEGORYCODE"
            End If
        End If
        StrSql += vbCrLf + "  ) X "
        'StrSql += vbCrLf + " WHERE CATNAME NOT LIKE('%(RT)%') AND CATNAME NOT LIKE '%VAT%' "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



        'StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALESRETURN(CATEGORYCODE, CATNAME, "
        'StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
        'StrSql += vbCrLf + " SELECT CATEGORYCODE, CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        'StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
        'StrSql += vbCrLf + " ,2 AS RESULT2, 'D' AS COLHEAD2, '' TYPE"
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN WHERE COLHEAD <> 'VAT'  GROUP BY CATCODE,CATNAME"
        'If rbtmetalwisegrp.Checked = True Then
        '    StrSql += vbCrLf + "  ,CATEGORYCODE, RESULT"
        'End If
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()


        'If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "

        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN(CATNAME, "
        'StrSql += vbCrLf + "  ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        'StrSql += vbCrLf + "  RESULT, RESULT1, COLHEAD, RESULT2, COLHEAD2, TYPE) "

        'StrSql += vbCrLf + "  SELECT 'SALES RETURN'CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        'StrSql += vbCrLf + "  0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT,  0 RESULT, 3 RESULT1, 'T' COLHEAD, 1 RESULT2, '' COLHEAD2, 'S' TYPE  "
        'StrSql += vbCrLf + "  END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

        '' If rbtmetalwisegrp.Checked = False Then
        'StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        'StrSql += vbCrLf + "  BEGIN "
        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        'StrSql += vbCrLf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD, RESULT2, COLHEAD2, TYPE) "

        'StrSql += vbCrLf + "  SELECT 'Z' AS CATCODE, 'SALES RETURN TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        'StrSql += vbCrLf + "  SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        'StrSql += vbCrLf + "  SUM(PAYMENT), 4 RESULT, 2 RESULT1,'S' COLHEAD, 3 RESULT2, 'S' COLHEAD2, 'T' TYPE "
        'StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE RESULT = 1 "
        'StrSql += vbCrLf + "  END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        '' End If



        'Else
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALESRETURN(CATNAME, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2, TYPE ) "

        StrSql += vbCrLf + " SELECT 'SALES RETURN' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT,SUM(ISNULL(PAYMENT,0))PAYMENT, 1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE, 1 RESULT2, 'T' COLHEAD2, 'B' TYPE "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN WHERE ISNULL(CATCODE,'') <> '' AND CATCODE NOT IN ('Z') GROUP BY CATCODE"
        'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        'End If

        ' If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD, RESULT2, COLHEAD2, TYPE) "

        StrSql += vbCrLf + "  SELECT 'Z' AS CATCODE, 'SALES RETURN TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += vbCrLf + "  SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + "  SUM(PAYMENT), 4 RESULT, 2 RESULT1,'S' COLHEAD, 3 AS RESULT2, 'S' AS COLHEAD2, 'T' AS TYPE "
        StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE RESULT = 1 "
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        ' End If

        'If rbtmetalwisegrp.Checked = False Then
        'StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        'StrSql += vbCrLf + "  BEGIN "


        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += vbCrLf + "  (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE, CATCODE, COLHEAD,RESULT1, ORDERNO "
        'If rbtmetalwisegrp.Checked = True Then
        '    StrSql += vbCrLf + "  ,CATEGORYCODE"
        'End If
        'StrSql += vbCrLf + " ) SELECT '  ' + CATNAME, "
        'StrSql += vbCrLf + "  CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        'StrSql += vbCrLf + "  CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        'StrSql += vbCrLf + "  CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        'StrSql += vbCrLf + "  CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        'StrSql += vbCrLf + "  CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        'StrSql += vbCrLf + "  CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        'StrSql += vbCrLf + "  CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        'StrSql += vbCrLf + "  CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        'StrSql += vbCrLf + "  CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, CATCODE, COLHEAD, RESULT1, 4 ORDERNO  "
        'If rbtmetalwisegrp.Checked = True Then
        '    StrSql += vbCrLf + "  ,CATEGORYCODE"
        'End If
        'StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1 AND COLHEAD = 'L' ORDER BY RESULT1, CATCODE, RESULT"
        'StrSql += vbCrLf + "  END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()


        'StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        'StrSql += vbCrLf + "  BEGIN "
        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION, "
        'StrSql += vbCrLf + "  ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        'StrSql += vbCrLf + "  ORDERNO, RESULT1, COLHEAD1, COLHEAD, CATCODE) "
        'StrSql += vbCrLf + "  SELECT 'SALES RETURN' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        'StrSql += vbCrLf + "  0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT,  3 ORDERNO, 3 RESULT1, 'C' COLHEAD1, 'L' COLHEAD, CATCODE "
        'StrSql += vbCrLf + "  FROM TEMP" & systemId & "SASRPU  WHERE ISNULL(CATCODE,'') <> ''  GROUP BY CATCODE "
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (CATEGORYCODE, DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,CATCODE,RESULT1,ORDERNO "
        StrSql += vbCrLf + " , RESULT2, COLHEAD2,  TYPE)"
        StrSql += vbCrLf + " SELECT CATEGORYCODE, '' + CATNAME, "
        StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END,"
        'StrSql += vbCrLf + " CASE WHEN DISCPER <> 0 THEN DISCPER ELSE NULL END DISCPER,"
        StrSql += vbCrLf + " COLHEAD,CATCODE, RESULT,1 "
        StrSql += vbCrLf + " ,2 AS RESULT2, 'D' AS COLHEAD2, '' TYPE"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN "
        StrSql += vbCrLf + " WHERE COLHEAD IN ('L')"
        StrSql += vbCrLf + " AND CATNAME NOT LIKE('%(RT)%') AND CATNAME NOT LIKE '%VAT%' "
        StrSql += vbCrLf + " AND CATNAME NOT LIKE('%(WT)%') AND CATNAME NOT LIKE '%( WT)%'"
        StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        '''ONLY RT CATNAME

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
        StrSql += vbCrLf + " SELECT CATEGORYCODE, CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,1 RESULT1, 'L' COLHEAD,CATCODE "
        StrSql += vbCrLf + " ,2 AS RESULT2, 'R' AS COLHEAD2, 'R' TYPE"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN"
        StrSql += vbCrLf + " WHERE  CATNAME LIKE('%(RT)%') AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
        StrSql += vbCrLf + " GROUP BY CATCODE,CATNAME, CATEGORYCODE,RESULT"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        '''ONLY WT CATNAME
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
        StrSql += vbCrLf + " SELECT CATEGORYCODE, CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,1 RESULT1, 'L' COLHEAD,CATCODE "
        StrSql += vbCrLf + " ,2 AS RESULT2, 'W' AS COLHEAD2, 'W' TYPE"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN"
        StrSql += vbCrLf + " WHERE CATNAME LIKE('%(WT)%') OR CATNAME LIKE '% ( WT)%' AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
        StrSql += vbCrLf + " GROUP BY CATCODE,CATNAME, CATEGORYCODE,RESULT"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()



        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1, RESULT2, COLHEAD2, TYPE) "
        StrSql += vbCrLf + " SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
        StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
        StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,CATCODE,'X' COLHEAD,5,2,'M' COLHEAD1, 4 AS RESULT2, 'Z' AS COLHEAD2, 'Z' TYPE "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPU WHERE ISNULL(CATCODE,'') <> '' GROUP BY CATCODE"
        'StrSql += vbCrLf + " ORDER BY RESULT1"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        'StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1) "
        'StrSql += vbCrLf + " SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
        'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
        'StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,CATCODE,'R' COLHEAD,5,2,'M' COLHEAD1 "
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPU WHERE ISNULL(CATCODE,'') <> '' AND DESCRIPTION LIKE '%RT%'  GROUP BY CATCODE"
        ''StrSql += vbCrLf + " ORDER BY RESULT1"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

        'StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1) "
        'StrSql += vbCrLf + " SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
        'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
        'StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,CATCODE,'R' COLHEAD,5,2,'M' COLHEAD1 "
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPU WHERE ISNULL(CATCODE,'') <> '' AND DESCRIPTION LIKE '%WT%' GROUP BY CATCODE"
        ''StrSql += vbCrLf + " ORDER BY RESULT1"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

        'Else
        'StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0"
        'StrSql += vbCrLf + " BEGIN "
        ''StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        ' ''strsql += vbcrlf + " (DESCRIPTION, COLHEAD) VALUES('SALES','T') "
        ''StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) "
        ''StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
        ''StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
        ''StrSql += " WHERE RESULT1 = 1"
        ''StrSql += " AND COLHEAD <> 'D'"
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,CATCODE,RESULT1,ORDERNO) "
        'StrSql += vbCrLf + " SELECT '  ' + CATNAME, "
        'StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        'StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        'StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        'StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        'StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        'StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        'StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        'StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        'StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, COLHEAD,CATCODE, RESULT1,5 "
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN"
        'StrSql += vbCrLf + " WHERE RESULT1 = 1 AND COLHEAD='L'"
        'StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        'End If

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
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
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
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
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
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
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
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
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
            StrSql += vbCrLf + "  WHERE S.TRANTYPE='SR' AND S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
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

        'If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN(CATNAME, "
        StrSql += vbCrLf + "  ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + "  SELECT 'SALES RETURN'CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += vbCrLf + "  0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT,  0 RESULT, 0 RESULT1, 'T' COLHEAD "
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        'Else
        'StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALESRETURN(CATNAME, "
        'StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
        'StrSql += vbCrLf + " SELECT 'SALES RETURN' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        'StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT,SUM(ISNULL(PAYMENT,0))PAYMENT, 1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN GROUP BY CATCODE"
        ''StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        'End If

        ' If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + "  SELECT 'Z' AS CATCODE, 'SALES RETURN TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += vbCrLf + "  SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + "  SUM(PAYMENT), 4 RESULT, 2 RESULT1,'S' COLHEAD "
        StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE RESULT = 1 "
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        ' End If

        'If rbtmetalwisegrp.Checked = False Then
        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        ''strsql += vbcrlf + "  (DESCRIPTION, COLHEAD) VALUES('SALES RETURN','T') "
        StrSql += vbCrLf + "  (DESCRIPTION, COLHEAD) "
        StrSql += vbCrLf + "  SELECT 'SALES RETURN ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
        StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1"
        StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + "  (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,RESULT1) "
        StrSql += vbCrLf + "  SELECT '  ' + CATNAME, "
        StrSql += vbCrLf + "  CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        StrSql += vbCrLf + "  CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        StrSql += vbCrLf + "  CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        StrSql += vbCrLf + "  CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        StrSql += vbCrLf + "  CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        StrSql += vbCrLf + "  CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        StrSql += vbCrLf + "  CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + "  CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + "  CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, COLHEAD, RESULT1  "
        StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1 ORDER BY RESULT1, CATCODE, RESULT"
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        'Else
        'StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0"
        'StrSql += vbCrLf + " BEGIN "
        ''StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        ' ''strsql += vbcrlf + " (DESCRIPTION, COLHEAD) VALUES('SALES','T') "
        ''StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) "
        ''StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
        ''StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
        ''StrSql += " WHERE RESULT1 = 1"
        ''StrSql += " AND COLHEAD <> 'D'"
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,CATCODE,RESULT1,ORDERNO) "
        'StrSql += vbCrLf + " SELECT '  ' + CATNAME, "
        'StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        'StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        'StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        'StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        'StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        'StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        'StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        'StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        'StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, COLHEAD,CATCODE, RESULT1,5 "
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN"
        'StrSql += vbCrLf + " WHERE RESULT1 = 1 AND COLHEAD='L'"
        'StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        'End If

    End Sub

    Private Sub ProcSalesReturn_MetalGroup()
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSSALESRETURN') DROP TABLE TEMP" & systemId & "ABSSALESRETURN"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + "  SELECT CATCODE, "
        Else

            StrSql += vbCrLf + "  SELECT METALID AS CATCODE,"
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + " CATEGORYCODE,"
            End If
        End If
        StrSql += vbCrLf + "  CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, "
        StrSql += vbCrLf + "  RECGRSWT, RECNETWT, RECEIPT, "
        '''MODIFIED BY M
        'StrSql += vbCrLf + "  PAYMENT, AVERAGE, RATE, 'L' COLHEAD, RESULT, 4 RESULT1  INTO TEMP" & systemId & "ABSSALESRETURN FROM ("
        StrSql += vbCrLf + "  PAYMENT, AVERAGE, RATE,  COLHEAD, RESULT, 4 RESULT1  "

        If rbtmetalwisegrp.Checked = True Then
            StrSql += vbCrLf + "  ,2 RESULT2, 'A' COLHEAD2, '' TYPE"
        End If

        StrSql += vbCrLf + "  INTO TEMP" & systemId & "ABSSALESRETURN FROM ("

        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + "  SELECT R.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + "  (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            End If
        ElseIf rbtmetalwisegrp.Checked Then
            StrSql += vbCrLf + "  SELECT R.METALID, "
            StrSql += vbCrLf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            StrSql += vbCrLf + "  R.CATCODE AS CATEGORYCODE, "
        Else
            StrSql += vbCrLf + "  SELECT R.METALID, "
            StrSql += vbCrLf + "  (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = R.METALID)AS CATNAME, "
        End If
        StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
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
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
        StrSql += vbCrLf + " )AS R"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY R.CATCODE,C.METALID "
        ElseIf rbtmetalwisegrp.Checked Then
            StrSql += vbCrLf + " GROUP BY R.CATCODE,R.METALID "
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
                StrSql += vbCrLf + "  SELECT R.METALID, 'SALES RETURN TAX' AS CATNAME, "
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + "  R.CATCODE AS CATEGORYCODE,"
                End If
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(TAX+ISNULL(TT.TAXAMT,0)) AS PAYMENT, 1 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'VAT') COLHEAD "
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
            StrSql += vbCrLf + " ON TT.SNO=R.SNO and TT.BATCHNO=R.BATCHNO "
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE,R.CASHID,C.STAXID HAVING SUM(TAX) <> 0"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID,R.CASHID, R.CATCODE "
            End If
            StrSql += vbCrLf + "  UNION ALL "
            ''SURCHARGE
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.SSCID) AS CATNAME, "
            Else

                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNSURCHARGE' AS CATNAME, "
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + "  R.CATCODE AS CATEGORYCODE,"
                End If
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
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE,R.CASHID,C.SSCID HAVING SUM(SC) <> 0"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID,R.CASHID, R.CATCODE HAVING SUM(SC) <> 0 "
            End If
            StrSql += vbCrLf + "  UNION ALL "

            ''ADDTIONAL SURCHARGE
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.SASCID) AS CATNAME, "
            Else

                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNSURCHARGE' AS CATNAME, "
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + "  R.CATCODE AS CATEGORYCODE,"
                End If
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(ADSC) AS PAYMENT, 1 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE,R.CASHID, C.SASCID HAVING SUM(ADSC) <> 0"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID,R.CASHID, R.CATCODE HAVING SUM(ADSC) <> 0 "
            End If

            StrSql += vbCrLf + "  UNION ALL "


            StrSql += vbCrLf + "  SELECT  CATCODE,CATNAME "
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + "  ,CATEGORYCODE"
            End If
            StrSql += vbCrLf + "  , SUM(ISSPCS) AS ISSPCS, SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
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
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + " '' CATEGORYCODE,"
                End If
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS,  0 AS ISSGRSWT, 0 AS ISSNETWT,  "
            StrSql += vbCrLf + "  SUM(S.STNPCS) AS RECPCS,  SUM(S.STNWT)  AS RECGRSWT,  SUM(S.STNWT)  AS RECNETWT,  "
            StrSql += vbCrLf + "  0 AS RECEIPT,  SUM(S.STNAMT) AS PAYMENT,  "
            StrSql += vbCrLf + "  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            StrSql += vbCrLf + "  'D' COLHEAD  FROM " & cnStockDb & "..RECEIPTSTONE S "
            StrSql += vbCrLf + "  WHERE S.TRANTYPE='SR' AND S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + "  " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(CANCEL,'') = ''  AND TRANTYPE = 'SR' AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + "  AND  (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            StrSql += vbCrLf + "  AND COMPANYID IN (" & SelectedCompanyId & ")"
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY S.CATCODE,S.BATCHNO  "
            Else
                StrSql += vbCrLf + "  GROUP BY S.BATCHNO, S.CATCODE  "
            End If
            StrSql += vbCrLf + "  ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + "  ,CATEGORYCODE"
            End If
        End If
        StrSql += vbCrLf + "  ) X "
        'StrSql += vbCrLf + " WHERE CATNAME NOT LIKE('%(RT)%') AND CATNAME NOT LIKE '%VAT%' "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN( "
        If rbtmetalwisegrp.Checked = True Then
            StrSql += vbCrLf + "  CATEGORYCODE, RESULT2, COLHEAD2, TYPE,"
        End If
        StrSql += vbCrLf + "  CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT, RESULT1, COLHEAD, CATCODE) "

        StrSql += vbCrLf + "  SELECT "
        If rbtmetalwisegrp.Checked = True Then
            StrSql += vbCrLf + "  CATEGORYCODE, 2 RESULT2, 'A' COLHEAD2, '' TYPE, "
        End If
        StrSql += vbCrLf + "  CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "

        StrSql += vbCrLf + "  SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        StrSql += vbCrLf + "  SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "



        StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE COLHEAD <> 'VAT'  GROUP BY CATCODE,CATNAME"
        If rbtmetalwisegrp.Checked = True Then
            StrSql += vbCrLf + "  ,CATEGORYCODE ,RESULT2, COLHEAD2, TYPE"
        End If
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        'If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN(CATNAME, CATCODE, "
        StrSql += vbCrLf + "  ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT, RESULT1, COLHEAD, RESULT2, COLHEAD2, TYPE) "

        StrSql += vbCrLf + "  SELECT 'SALES RETURN'CATNAME, CATCODE, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += vbCrLf + "  0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT,  0 RESULT, 3 RESULT1, 'L' COLHEAD , 2 RESULT2, '' COLHEAD2, '' TYPE"
        StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN"
        StrSql += vbCrLf + "  GROUP BY CATCODE"
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()





        'Else
        'StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALESRETURN(CATNAME, "
        'StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
        'StrSql += vbCrLf + " SELECT 'SALES RETURN' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        'StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT,SUM(ISNULL(PAYMENT,0))PAYMENT, 1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN GROUP BY CATCODE"
        ''StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        'End If

        '''SALES RETURN TOTAL OLD
        '' If rbtmetalwisegrp.Checked = False Then
        'StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        'StrSql += vbCrLf + "  BEGIN "
        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        'StrSql += vbCrLf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD, RESULT2, COLHEAD2, TYPE) "
        'StrSql += vbCrLf + "  SELECT 'Z' AS CATCODE, 'SALES RETURN TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        'StrSql += vbCrLf + "  SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        'StrSql += vbCrLf + "  SUM(PAYMENT), 4 RESULT, 2 RESULT1,'Z' COLHEAD, 2 RESULT2,'Z' COLHEAD2, '' TYPE "
        'StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE RESULT = 1 "
        'StrSql += vbCrLf + "  END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()



        '''''''''''''''''''''''''''''''


        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()




        ' End If

        ' '' ''If rbtmetalwisegrp.Checked = False Then
        '' ''StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        '' ''StrSql += vbCrLf + "  BEGIN "
        ' '' ''StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        ''''''''''strsql += vbcrlf + "  (DESCRIPTION, COLHEAD) VALUES('SALES RETURN','T') "
        ' '' ''StrSql += vbCrLf + "  (DESCRIPTION, COLHEAD) "
        ' '' ''StrSql += vbCrLf + "  SELECT 'SALES RETURN ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
        ' '' ''StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1"

        '' ''StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "SASRPU"
        '' ''StrSql += vbCrLf + "  (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE, CATCODE, COLHEAD,RESULT1, ORDERNO "
        '' ''If rbtmetalwisegrp.Checked = True Then
        '' ''    StrSql += vbCrLf + "  ,CATEGORYCODE"
        '' ''End If
        '' ''StrSql += vbCrLf + " ) SELECT '  ' + CATNAME, "
        '' ''StrSql += vbCrLf + "  CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        '' ''StrSql += vbCrLf + "  CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        '' ''StrSql += vbCrLf + "  CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        '' ''StrSql += vbCrLf + "  CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        '' ''StrSql += vbCrLf + "  CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        '' ''StrSql += vbCrLf + "  CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        '' ''StrSql += vbCrLf + "  CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        '' ''StrSql += vbCrLf + "  CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        '' ''StrSql += vbCrLf + "  CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, CATCODE, COLHEAD, RESULT1, 4 ORDERNO  "
        '' ''If rbtmetalwisegrp.Checked = True Then
        '' ''    StrSql += vbCrLf + "  ,CATEGORYCODE"
        '' ''End If
        '' ''StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1 AND COLHEAD = 'L' ORDER BY RESULT1, CATCODE, RESULT"
        '' ''StrSql += vbCrLf + "  END "
        '' ''Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        '' ''Cmd.ExecuteNonQuery()



        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        StrSql += vbCrLf + " BEGIN "


        ''''''''''SALES HEADING
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,CATCODE,RESULT1,ORDERNO,COLHEAD1, RESULT2, COLHEAD2, TYPE) "
        'StrSql += vbCrLf + " SELECT '  SALES' ,'L',CATCODE, RESULT1,0,'C', 1 RESULT2, '' COLHEAD2, '' TYPE "
        ''StrSql += vbCrLf + " SELECT '  SALES' ,'L',CATCODE, RESULT1,1,'C' "
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES "
        'StrSql += "  GROUP BY CATCODE, RESULT1, COLHEAD"
        'StrSql += "  ORDER BY COLHEAD "


        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (CATEGORYCODE, DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,CATCODE,RESULT1,ORDERNO "
        StrSql += vbCrLf + " , RESULT2, COLHEAD2,  TYPE)"
        StrSql += vbCrLf + " SELECT CATEGORYCODE, '' + CATNAME, "
        StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END,"

        StrSql += vbCrLf + " COLHEAD,CATCODE, RESULT,1 "
        StrSql += vbCrLf + " ,2 AS RESULT2, 'A' AS COLHEAD2, '' TYPE"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN "
        StrSql += vbCrLf + " WHERE COLHEAD IN ('L')"
        StrSql += vbCrLf + " AND CATNAME NOT LIKE('%(RT)%') "
        StrSql += vbCrLf + " AND CATNAME NOT LIKE('%(WT)%') AND CATNAME NOT LIKE '%( WT)%'"
        StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        '''ONLY RT CATNAME
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
        StrSql += vbCrLf + " SELECT CATEGORYCODE, CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,1 RESULT1, 'L' COLHEAD,CATCODE "
        StrSql += vbCrLf + " ,2 AS RESULT2, 'B' AS COLHEAD2, 'R' TYPE"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN"
        'StrSql += vbCrLf + " WHERE  CATNAME LIKE('%(RT)%') AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
        StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND  CATNAME LIKE('%(RT)%') "
        StrSql += vbCrLf + " GROUP BY CATCODE,CATNAME, CATEGORYCODE,RESULT"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()



        '''RT TOTAL
        StrSql = vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + " RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
        StrSql += vbCrLf + " SELECT '' CATEGORYCODE, '  RATE TOTAL' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,1 RESULT1, 'L' COLHEAD,CATCODE "
        StrSql += vbCrLf + " ,2 AS RESULT2, 'B' AS COLHEAD2, 'R' TYPE"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN"
        StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND CATNAME LIKE('%(RT)%')  "
        StrSql += vbCrLf + " GROUP BY CATCODE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()



        '''ONLY WT CATNAME
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
        StrSql += vbCrLf + " SELECT CATEGORYCODE, CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,1 RESULT1, 'L' COLHEAD,CATCODE "
        StrSql += vbCrLf + " ,2 AS RESULT2, 'C' AS COLHEAD2, 'W' TYPE"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN"
        'StrSql += vbCrLf + " WHERE CATNAME LIKE('%(WT)%') OR CATNAME LIKE '% ( WT)%' AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
        StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND CATNAME LIKE('%( WT)%')  "
        StrSql += vbCrLf + " GROUP BY CATCODE,CATNAME, CATEGORYCODE,RESULT"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        '''WT TOTAL
        StrSql = " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
        StrSql += vbCrLf + "  ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
        StrSql += vbCrLf + " SELECT '' CATEGORYCODE, 'WEIGHT TOTAL' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,1 RESULT1, 'L' COLHEAD,CATCODE "
        StrSql += vbCrLf + " ,2 AS RESULT2, 'C' AS COLHEAD2, 'W' TYPE"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN"
        'StrSql += vbCrLf + " WHERE CATNAME LIKE('%(WT)%') OR CATNAME LIKE '% ( WT)%' AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
        StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND CATNAME LIKE('%( WT)%')  "
        StrSql += vbCrLf + " GROUP BY CATCODE"

        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        '''''SALES RETURN HEADING
        ''StrSql = "  INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION, "
        ''StrSql += vbCrLf + "  ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        ''StrSql += vbCrLf + "  ORDERNO, RESULT1, COLHEAD1, COLHEAD, CATCODE "
        ''If rbtmetalwisegrp.Checked = True Then
        ''    StrSql += vbCrLf + "  , RESULT2, COLHEAD2, TYPE"
        ''End If
        ''StrSql += vbCrLf + "  )SELECT 'SALES RETURN' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        ''StrSql += vbCrLf + "  0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT,  3 ORDERNO, 3 RESULT1, 'C' COLHEAD1, 'L' COLHEAD, CATCODE  "
        ''If rbtmetalwisegrp.Checked = True Then
        ''    StrSql += vbCrLf + "  ,1 RESULT2, 'T' COLHEAD2, '' TYPE"
        ''End If
        ''StrSql += vbCrLf + "  FROM TEMP" & systemId & "SASRPU  WHERE ISNULL(CATCODE,'') <> ''  GROUP BY CATCODE "
        ''Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        ''Cmd.ExecuteNonQuery()

        'StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1 "
        'If rbtmetalwisegrp.Checked = True Then
        '    StrSql += vbCrLf + "  , RESULT2, COLHEAD2, TYPE"
        'End If
        'StrSql += vbCrLf + " )SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
        'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
        'StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,CATCODE,'X' COLHEAD,5,2,'M' COLHEAD1 "
        'If rbtmetalwisegrp.Checked = True Then
        '    StrSql += vbCrLf + "  ,2 RESULT2, 'Z' COLHEAD2, 'M' TYPE"
        'End If
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPU WHERE ISNULL(CATCODE,'') <> '' GROUP BY CATCODE"
        ''StrSql += vbCrLf + " ORDER BY RESULT1"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

        '''TOTAL 
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "SASRPU(CATCODE, DESCRIPTION,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT1, COLHEAD, RESULT2, COLHEAD2, TYPE) "

        StrSql += vbCrLf + "  SELECT CATCODE, 'TOTAL'DESCRIPTION,ISNULL(SUM(ISSPCS),0), ISNULL(SUM(ISSGRSWT),0),ISNULL(SUM(ISSNETWT),0), "
        StrSql += vbCrLf + "  ISNULL(SUM(RECPCS),0),ISNULL(SUM(RECGRSWT),0),ISNULL(SUM(RECNETWT),0), ISNULL(SUM(RECEIPT),0), "
        StrSql += vbCrLf + "  ISNULL(SUM(PAYMENT),0), 2 RESULT1,'Z' COLHEAD, 2 RESULT2, 'Z' COLHEAD2, '' TYPE "
        StrSql += vbCrLf + "  FROM TEMP" & systemId & "SASRPU "
        StrSql += vbCrLf + "  WHERE COLHEAD = 'L' AND RESULT2 = 2 AND DESCRIPTION NOT LIKE '% TOTAL%' AND ISNULL(COLHEAD2,'') <> '' AND COLHEAD2 NOT IN ('Z')"
        StrSql += vbCrLf + "  GROUP BY CATCODE"
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        '''METAL TOTAL 
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "SASRPU(CATCODE, DESCRIPTION,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT1, COLHEAD, RESULT2, COLHEAD2, TYPE) "

        StrSql += vbCrLf + "  SELECT CATCODE, 'METAL TOTAL'DESCRIPTION,ISNULL(SUM(ISSPCS),0), ISNULL(SUM(ISSGRSWT),0),ISNULL(SUM(ISSNETWT),0), "
        StrSql += vbCrLf + "  ISNULL(SUM(RECPCS),0),ISNULL(SUM(RECGRSWT),0),ISNULL(SUM(RECNETWT),0), ISNULL(SUM(RECEIPT),0), "
        StrSql += vbCrLf + "  ISNULL(SUM(PAYMENT),0), 2 RESULT1,'Z' COLHEAD, 2 RESULT2, 'ZZ' COLHEAD2, '' TYPE "
        StrSql += vbCrLf + "  FROM TEMP" & systemId & "SASRPU "
        StrSql += vbCrLf + "  WHERE CATCODE NOT IN ('S') AND COLHEAD2 = 'Z' AND RESULT2 IN (1,2) AND COLHEAD2 IN ('Z') "
        StrSql += vbCrLf + "  GROUP BY CATCODE"
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        '''METAL TOTAL 
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "SASRPU(CATCODE, DESCRIPTION,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT1, COLHEAD, RESULT2, COLHEAD2, TYPE) "

        StrSql += vbCrLf + "  SELECT CATCODE, 'METAL TOTAL'DESCRIPTION,ISNULL(SUM(ISSPCS),0), ISNULL(SUM(ISSGRSWT),0),ISNULL(SUM(ISSNETWT),0), "
        StrSql += vbCrLf + "  ISNULL(SUM(RECPCS),0),ISNULL(SUM(RECGRSWT),0),ISNULL(SUM(RECNETWT),0), ISNULL(SUM(RECEIPT),0), "
        StrSql += vbCrLf + "  ISNULL(SUM(PAYMENT),0), 2 RESULT1,'Z' COLHEAD, 2 RESULT2, 'ZZ' COLHEAD2, '' TYPE "
        StrSql += vbCrLf + "  FROM TEMP" & systemId & "SASRPU "
        StrSql += vbCrLf + "  WHERE CATCODE = 'S' AND COLHEAD='L' AND RESULT2 IN (1,2) "
        StrSql += vbCrLf + "  AND DESCRIPTION NOT LIKE ('%WEIGHT TOTAL%') AND DESCRIPTION NOT LIKE ('%RATE TOTAL%') "
        StrSql += vbCrLf + "  GROUP BY CATCODE"
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

    End Sub

    Private Sub ProcRepairGoldSalesReturn_MetalGroup()
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSSALESRETURN') DROP TABLE TEMP" & systemId & "ABSSALESRETURN"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + "  SELECT CATCODE, "
        Else

            StrSql += vbCrLf + "  SELECT METALID AS CATCODE,"
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + " CATEGORYCODE,"
            End If
        End If
        StrSql += vbCrLf + "  CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, "
        StrSql += vbCrLf + "  RECGRSWT, RECNETWT, RECEIPT, "
        '''MODIFIED BY M
        'StrSql += vbCrLf + "  PAYMENT, AVERAGE, RATE, 'L' COLHEAD, RESULT, 4 RESULT1  INTO TEMP" & systemId & "ABSSALESRETURN FROM ("
        StrSql += vbCrLf + "  PAYMENT, AVERAGE, RATE,  COLHEAD, RESULT, 4 RESULT1  "

        If rbtmetalwisegrp.Checked = True Then
            StrSql += vbCrLf + "  ,2 RESULT2, 'A' COLHEAD2, '' TYPE"
        End If

        StrSql += vbCrLf + "  INTO TEMP" & systemId & "ABSSALESRETURN FROM ("

        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + "  SELECT R.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + "  (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            End If
        ElseIf rbtmetalwisegrp.Checked Then
            StrSql += vbCrLf + "  SELECT R.METALID, "
            StrSql += vbCrLf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            StrSql += vbCrLf + "  R.CATCODE AS CATEGORYCODE, "
        Else
            StrSql += vbCrLf + "  SELECT R.METALID, "
            StrSql += vbCrLf + "  (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = R.METALID)AS CATNAME, "
        End If
        StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
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
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
        StrSql += vbCrLf + " )AS R"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY R.CATCODE,C.METALID "
        ElseIf rbtmetalwisegrp.Checked Then
            StrSql += vbCrLf + " GROUP BY R.CATCODE,R.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY R.METALID "
        End If
        
        If RPT_SEPVAT_DABS Then
            StrSql += vbCrLf + "  UNION ALL "
            ''TAX
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.STAXID) AS CATNAME, "
            Else
                StrSql += vbCrLf + "  SELECT R.METALID, 'SALES RETURN TAX' AS CATNAME, "
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + "  R.CATCODE AS CATEGORYCODE,"
                End If
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(TAX+ISNULL(TT.TAXAMT,0)) AS PAYMENT, 1 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'VAT') COLHEAD "
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
            StrSql += vbCrLf + " ON TT.SNO=R.SNO and TT.BATCHNO=R.BATCHNO "
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE,R.CASHID,C.STAXID HAVING SUM(TAX) <> 0"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID,R.CASHID, R.CATCODE "
            End If
            StrSql += vbCrLf + "  UNION ALL "
            ''SURCHARGE
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.SSCID) AS CATNAME, "
            Else

                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNSURCHARGE' AS CATNAME, "
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + "  R.CATCODE AS CATEGORYCODE,"
                End If
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
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE,R.CASHID,C.SSCID HAVING SUM(SC) <> 0"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID,R.CASHID, R.CATCODE HAVING SUM(SC) <> 0 "
            End If
            StrSql += vbCrLf + "  UNION ALL "

            ''ADDTIONAL SURCHARGE
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.SASCID) AS CATNAME, "
            Else

                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNSURCHARGE' AS CATNAME, "
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + "  R.CATCODE AS CATEGORYCODE,"
                End If
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(ADSC) AS PAYMENT, 1 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE,R.CASHID, C.SASCID HAVING SUM(ADSC) <> 0"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID,R.CASHID, R.CATCODE HAVING SUM(ADSC) <> 0 "
            End If

            StrSql += vbCrLf + "  UNION ALL "


            StrSql += vbCrLf + "  SELECT  CATCODE,CATNAME "
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + "  ,CATEGORYCODE"
            End If
            StrSql += vbCrLf + "  , SUM(ISSPCS) AS ISSPCS, SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
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
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += vbCrLf + " '' CATEGORYCODE,"
                End If
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS,  0 AS ISSGRSWT, 0 AS ISSNETWT,  "
            StrSql += vbCrLf + "  SUM(S.STNPCS) AS RECPCS,  SUM(S.STNWT)  AS RECGRSWT,  SUM(S.STNWT)  AS RECNETWT,  "
            StrSql += vbCrLf + "  0 AS RECEIPT,  SUM(S.STNAMT) AS PAYMENT,  "
            StrSql += vbCrLf + "  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            StrSql += vbCrLf + "  'D' COLHEAD  FROM " & cnStockDb & "..RECEIPTSTONE S "
            StrSql += vbCrLf + "  WHERE S.TRANTYPE='SR' AND S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + "  " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(CANCEL,'') = ''  AND TRANTYPE = 'SR' AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + "  AND  (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            StrSql += vbCrLf + "  AND COMPANYID IN (" & SelectedCompanyId & ")"
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + "  GROUP BY S.CATCODE,S.BATCHNO  "
            Else
                StrSql += vbCrLf + "  GROUP BY S.BATCHNO, S.CATCODE  "
            End If
            StrSql += vbCrLf + "  ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
            If rbtmetalwisegrp.Checked = True Then
                StrSql += vbCrLf + "  ,CATEGORYCODE"
            End If
        End If

        StrSql += vbCrLf + "  ) X "
        'StrSql += vbCrLf + " WHERE CATNAME NOT LIKE('%(RT)%') AND CATNAME NOT LIKE '%VAT%' "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN( "
        If rbtmetalwisegrp.Checked = True Then
            StrSql += vbCrLf + "  CATEGORYCODE, RESULT2, COLHEAD2, TYPE,"
        End If
        StrSql += vbCrLf + "  CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT, RESULT1, COLHEAD, CATCODE) "

        StrSql += vbCrLf + "  SELECT "
        If rbtmetalwisegrp.Checked = True Then
            StrSql += vbCrLf + "  CATEGORYCODE, 2 RESULT2, 'A' COLHEAD2, '' TYPE, "
        End If
        StrSql += vbCrLf + "  CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "

        StrSql += vbCrLf + "  SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        StrSql += vbCrLf + "  SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "



        StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN WHERE COLHEAD <> 'VAT'  GROUP BY CATCODE,CATNAME"
        If rbtmetalwisegrp.Checked = True Then
            StrSql += vbCrLf + "  ,CATEGORYCODE ,RESULT2, COLHEAD2, TYPE"
        End If
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        'If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN(CATNAME, CATCODE, "
        StrSql += vbCrLf + "  ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT, RESULT1, COLHEAD, RESULT2, COLHEAD2, TYPE) "

        StrSql += vbCrLf + "  SELECT 'SALES RETURN'CATNAME, CATCODE, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += vbCrLf + "  0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT,  0 RESULT, 3 RESULT1, 'L' COLHEAD , 2 RESULT2, '' COLHEAD2, '' TYPE"
        StrSql += vbCrLf + "  FROM TEMP" & systemId & "ABSSALESRETURN"
        StrSql += vbCrLf + "  GROUP BY CATCODE"
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (CATEGORYCODE, DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,CATCODE,RESULT1,ORDERNO "
        StrSql += vbCrLf + " , RESULT2, COLHEAD2,  TYPE)"
        StrSql += vbCrLf + " SELECT CATEGORYCODE, '' + CATNAME, "
        StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END,"

        StrSql += vbCrLf + " COLHEAD,CATCODE, RESULT,1 "
        StrSql += vbCrLf + " ,2 AS RESULT2, 'A' AS COLHEAD2, '' TYPE"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN "
        StrSql += vbCrLf + " WHERE COLHEAD IN ('L')"
        StrSql += vbCrLf + " AND CATNAME NOT LIKE('%(RT)%') "
        StrSql += vbCrLf + " AND CATNAME NOT LIKE('%(WT)%') AND CATNAME NOT LIKE '%( WT)%'"
        StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        '''ONLY RT CATNAME
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
        StrSql += vbCrLf + " SELECT CATEGORYCODE, CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,1 RESULT1, 'L' COLHEAD,CATCODE "
        StrSql += vbCrLf + " ,2 AS RESULT2, 'B' AS COLHEAD2, 'R' TYPE"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN"
        'StrSql += vbCrLf + " WHERE  CATNAME LIKE('%(RT)%') AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
        StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND  CATNAME LIKE('%(RT)%') "
        StrSql += vbCrLf + " GROUP BY CATCODE,CATNAME, CATEGORYCODE,RESULT"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        '''RT TOTAL
        StrSql = vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + " RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
        StrSql += vbCrLf + " SELECT '' CATEGORYCODE, '  RATE TOTAL' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,1 RESULT1, 'L' COLHEAD,CATCODE "
        StrSql += vbCrLf + " ,2 AS RESULT2, 'B' AS COLHEAD2, 'R' TYPE"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN"
        StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND CATNAME LIKE('%(RT)%')  "
        StrSql += vbCrLf + " GROUP BY CATCODE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        '''ONLY WT CATNAME
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
        StrSql += vbCrLf + " SELECT CATEGORYCODE, CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,1 RESULT1, 'L' COLHEAD,CATCODE "
        StrSql += vbCrLf + " ,2 AS RESULT2, 'C' AS COLHEAD2, 'W' TYPE"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN"
        'StrSql += vbCrLf + " WHERE CATNAME LIKE('%(WT)%') OR CATNAME LIKE '% ( WT)%' AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
        StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND CATNAME LIKE('%( WT)%')  "
        StrSql += vbCrLf + " GROUP BY CATCODE,CATNAME, CATEGORYCODE,RESULT"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        '''WT TOTAL
        StrSql = " INSERT INTO TEMP" & systemId & "SASRPU(CATEGORYCODE, DESCRIPTION, "
        StrSql += vbCrLf + "  ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT1, COLHEAD,CATCODE, RESULT2, COLHEAD2,  TYPE)  "
        StrSql += vbCrLf + " SELECT '' CATEGORYCODE, ' WEIGHT TOTAL' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,1 RESULT1, 'L' COLHEAD,CATCODE "
        StrSql += vbCrLf + " ,2 AS RESULT2, 'C' AS COLHEAD2, 'W' TYPE"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALESRETURN"
        'StrSql += vbCrLf + " WHERE CATNAME LIKE('%(WT)%') OR CATNAME LIKE '% ( WT)%' AND CATNAME NOT LIKE '%VAT%' AND COLHEAD NOT IN ('VAT') "
        StrSql += vbCrLf + " WHERE COLHEAD = 'L' AND CATNAME LIKE('%( WT)%')  "
        StrSql += vbCrLf + " GROUP BY CATCODE"

        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        '''''SALES RETURN HEADING
        ''StrSql = "  INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION, "
        ''StrSql += vbCrLf + "  ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        ''StrSql += vbCrLf + "  ORDERNO, RESULT1, COLHEAD1, COLHEAD, CATCODE "
        ''If rbtmetalwisegrp.Checked = True Then
        ''    StrSql += vbCrLf + "  , RESULT2, COLHEAD2, TYPE"
        ''End If
        ''StrSql += vbCrLf + "  )SELECT 'SALES RETURN' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        ''StrSql += vbCrLf + "  0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT,  3 ORDERNO, 3 RESULT1, 'C' COLHEAD1, 'L' COLHEAD, CATCODE  "
        ''If rbtmetalwisegrp.Checked = True Then
        ''    StrSql += vbCrLf + "  ,1 RESULT2, 'T' COLHEAD2, '' TYPE"
        ''End If
        ''StrSql += vbCrLf + "  FROM TEMP" & systemId & "SASRPU  WHERE ISNULL(CATCODE,'') <> ''  GROUP BY CATCODE "
        ''Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        ''Cmd.ExecuteNonQuery()

        'StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1 "
        'If rbtmetalwisegrp.Checked = True Then
        '    StrSql += vbCrLf + "  , RESULT2, COLHEAD2, TYPE"
        'End If
        'StrSql += vbCrLf + " )SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
        'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
        'StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,CATCODE,'X' COLHEAD,5,2,'M' COLHEAD1 "
        'If rbtmetalwisegrp.Checked = True Then
        '    StrSql += vbCrLf + "  ,2 RESULT2, 'Z' COLHEAD2, 'M' TYPE"
        'End If
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPU WHERE ISNULL(CATCODE,'') <> '' GROUP BY CATCODE"
        ''StrSql += vbCrLf + " ORDER BY RESULT1"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

        '''TOTAL 
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "SASRPU(CATCODE, DESCRIPTION,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT1, COLHEAD, RESULT2, COLHEAD2, TYPE) "

        StrSql += vbCrLf + "  SELECT CATCODE, 'METAL TOTAL'DESCRIPTION,ISNULL(SUM(ISSPCS),0), ISNULL(SUM(ISSGRSWT),0),ISNULL(SUM(ISSNETWT),0), "
        StrSql += vbCrLf + "  ISNULL(SUM(RECPCS),0),ISNULL(SUM(RECGRSWT),0),ISNULL(SUM(RECNETWT),0), ISNULL(SUM(RECEIPT),0), "
        StrSql += vbCrLf + "  ISNULL(SUM(PAYMENT),0), 2 RESULT1,'Z' COLHEAD, 2 RESULT2, 'Z' COLHEAD2, '' TYPE "
        StrSql += vbCrLf + "  FROM TEMP" & systemId & "SASRPU "
        StrSql += vbCrLf + "  WHERE COLHEAD = 'L' AND RESULT2 = 2 AND DESCRIPTION NOT LIKE '% TOTAL%' AND ISNULL(COLHEAD2,'') <> '' AND COLHEAD2 NOT IN ('Z')"
        StrSql += vbCrLf + "  GROUP BY CATCODE"
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

    End Sub

    Private Sub ProcPurchase_MetalGroup()
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPURCHASE') DROP TABLE TEMP" & systemId & "ABSPURCHASE"
        If rbtCategoryWise.Checked = True Then
            StrSql += " SELECT CATCODE, "
        Else
            StrSql += " SELECT METALID AS CATCODE, "
        End If
        StrSql += " CATNAME,  "
        If rbtmetalwisegrp.Checked = True Then
            StrSql += " '' AS CATEGORYCODE,"
        End If
        StrSql += " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += " PAYMENT, AVERAGE, RATE, 'P' COLHEAD, RESULT,  RESULT1  "
        If rbtmetalwisegrp.Checked = True Then
            StrSql += " ,3 AS RESULT2, 'A' AS COLHEAD2, '' TYPE "
        End If

        StrSql += " INTO TEMP" & systemId & "ABSPURCHASE FROM ("
        If rbtCategoryWise.Checked = True Then
            StrSql += " SELECT R.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            Else
                StrSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            End If
        Else
            If rbtMetalwise.Checked = True Then
                StrSql += " SELECT R.METALID, "
                StrSql += " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = R.METALID)AS CATNAME, "
            End If

            If rbtmetalwisegrp.Checked = True Then
                StrSql += " SELECT R.METALID, "
                StrSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
                StrSql += " '' AS CATEGORYCODE,"
            End If
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
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
        StrSql += vbCrLf + " )AS R"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY R.CATCODE,C.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY R.METALID, R.CATCODE "
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
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += " '' AS CATEGORYCODE,"
                End If

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
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += " GROUP BY R.CATCODE, C.PTAXID "
            Else
                StrSql += " GROUP BY R.METALID, R.CATCODE "
            End If
            StrSql += " HAVING SUM(TAX) > 0"
            StrSql += " UNION ALL"

            StrSql += " SELECT CATCODE,CATNAME, "
            If rbtmetalwisegrp.Checked = True Then
                StrSql += "    '' AS CATEGORYCODE, "
            End If
            StrSql += "  SUM(ISSPCS) AS ISSPCS, SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
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
                StrSql += " WHERE CATCODE = S.CATCODE) CATNAME  "
            Else

                StrSql += " SELECT M.METALID AS CATCODE, M.METALNAME AS CATNAME  "
                If rbtmetalwisegrp.Checked = True Then
                    StrSql += "    ,'' AS CATEGORYCODE "
                End If
            End If
            StrSql += " ,0 AS ISSPCS,  0 AS ISSGRSWT, 0 AS ISSNETWT,  "
            StrSql += " SUM(S.STNPCS) AS RECPCS,  SUM(S.STNWT)  AS RECGRSWT,  SUM(S.STNWT)  AS RECNETWT,  "
            StrSql += " 0 AS RECEIPT,  SUM(S.STNAMT) AS PAYMENT,  "
            StrSql += " 2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            StrSql += " 'D' COLHEAD  FROM " & cnStockDb & "..RECEIPTSTONE S "
            StrSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON S.STNITEMID =I.ITEMID "
            StrSql += " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID=M.METALID"
            StrSql += " WHERE S.TRANTYPE='PU' AND S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += " " & Replace(StrFilter, "SYSTEMID", "S.SYSTEMID") & " AND ISNULL(CANCEL,'') = ''  AND TRANTYPE = 'PU' AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += " AND  (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            StrSql += " AND S.COMPANYID IN (" & SelectedCompanyId & ")"
            If rbtCategoryWise.Checked = True Then
                StrSql += " GROUP BY S.CATCODE,S.BATCHNO  "
            Else
                StrSql += " GROUP BY S.BATCHNO,M.METALID,M.METALNAME , I.CATCODE   "
            End If
            StrSql += " ) Y GROUP BY CATCODE, CATNAME,  AVERAGE, RATE, COLHEAD, RESULT"
            If rbtmetalwisegrp.Checked = True Then
                StrSql += " ,CATEGORYCODE"
            End If
        End If

        StrSql += " ) X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        'If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPURCHASE)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "ABSPURCHASE(CATNAME, "
        StrSql += " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += " RESULT, RESULT1, COLHEAD "
        If rbtmetalwisegrp.Checked = True Then
            StrSql += " , CATEGORYCODE, RESULT2, COLHEAD2, TYPE "
        End If
        StrSql += " )"
        StrSql += " SELECT 'PURCHASE'CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, 0 RESULT, 0 AS RESULT1, 'T' COLHEAD  "
        If rbtmetalwisegrp.Checked = True Then
            StrSql += " ,'' CATEGORYCODE, 3 AS RESULT2, '' AS COLHEAD2, '' TYPE "
        End If
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        'Else
        'StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPURCHASE)>0 "
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSPURCHASE(CATNAME, "
        'StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
        'StrSql += vbCrLf + " SELECT 'PURCHASE' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        'StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT,SUM(ISNULL(PAYMENT,0))PAYMENT, 1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSPURCHASE GROUP BY CATCODE"
        ''StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        'End If


        'If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPURCHASE)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "ABSPURCHASE(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD  "
        If rbtmetalwisegrp.Checked = True Then
            StrSql += "     ,CATEGORYCODE , RESULT2, COLHEAD2, TYPE"
        End If
        StrSql += " )"
        StrSql += " SELECT 'Z' AS CATCODE, 'PURCHASE TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += " SUM(PAYMENT),4 RESULT, 2 AS RESULT1, 'S' COLHEAD  "
        If rbtmetalwisegrp.Checked = True Then
            StrSql += "    ,''CATEGORYCODE , 3 AS RESULT2, 'Z' COLHEAD2, '' TYPE"
        End If
        StrSql += " FROM TEMP" & systemId & "ABSPURCHASE WHERE RESULT = 1 "
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        ' End If

        ' If rbtmetalwisegrp.Checked = False Then
        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPURCHASE)>0"
        StrSql += " BEGIN "


        'StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
        ' ''StrSql += " (DESCRIPTION, COLHEAD) VALUES('PURCHASE','T') "
        'StrSql += " (DESCRIPTION, COLHEAD) "
        'StrSql += " SELECT 'PURCHASE ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
        'StrSql += " FROM TEMP" & systemId & "ABSPURCHASE WHERE RESULT1 = 1"




        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION, CATCODE, ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,RESULT1, RESULT2, COLHEAD2, TYPE) "
        StrSql += " SELECT '  ' + CATNAME, 'Z' CATCODE,"
        StrSql += " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        StrSql += " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        StrSql += " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        StrSql += " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        StrSql += " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        StrSql += " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, COLHEAD, RESULT1  "
        StrSql += " ,RESULT2, COLHEAD2, TYPE "
        StrSql += " FROM TEMP" & systemId & "ABSPURCHASE "
        'StrSql += " WHERE RESULT = 1 "
        StrSql += " ORDER BY CATCODE, RESULT"
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        '''VAT GROUP DISPLAY
        '''SALES CATECODE HEADING
        'StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0"
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES"
        'StrSql += vbCrLf + " (CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD, CATCODE) "
        'StrSql += vbCrLf + " SELECT 'GOLD' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,'0' AVERAGE, 1 AS RESULT, 1 AS RESULT1, 'A' COLHEAD ,'G' CATCODE"
        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALES"
        'StrSql += vbCrLf + " (CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD, CATCODE) "
        'StrSql += vbCrLf + " SELECT 'SILVER' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,'0' AVERAGE, 1 AS RESULT, 1 AS RESULT1, 'A' COLHEAD ,'S' CATCODE"
        'StrSql += " END "


        'StrSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES WHERE CATCODE = 'P')>0"
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALES"
        'StrSql += vbCrLf + " (CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD, CATCODE) "
        'StrSql += vbCrLf + " SELECT 'PLATINUM' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,'0' AVERAGE, 1 AS RESULT, 1 AS RESULT1, 'A' COLHEAD ,'P' CATCODE"
        'StrSql += " END "

        'StrSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES WHERE CATCODE = 'T')>0"
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALES"
        'StrSql += vbCrLf + " (CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD, CATCODE) "
        'StrSql += vbCrLf + " SELECT 'STONE' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,'0' AVERAGE, 1 AS RESULT, 1 AS RESULT1, 'A' COLHEAD ,'T' CATCODE"
        'StrSql += " END "

        'StrSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES WHERE CATCODE = 'D')>0"
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALES"
        'StrSql += vbCrLf + " (CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD, CATCODE) "
        'StrSql += vbCrLf + " SELECT 'DIAMOND' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,'0' AVERAGE, 1 AS RESULT, 1 AS RESULT1, 'A' COLHEAD ,'D' CATCODE"
        'StrSql += " END "

        'StrSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES WHERE CATCODE = 'C')>0"
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALES"
        'StrSql += vbCrLf + " (CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD, CATCODE) "
        'StrSql += vbCrLf + " SELECT 'COPPER' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,'0' AVERAGE, 1 AS RESULT, 1 AS RESULT1, 'A' COLHEAD ,'C' CATCODE"
        'StrSql += " END "
        'StrSql += " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

        '''SALES RETURN CATECODE HEADING
        'StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN)>0"
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALESRETURN"
        'StrSql += vbCrLf + " (CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD, CATCODE) "
        'StrSql += vbCrLf + " SELECT 'GOLD' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,'0' AVERAGE, 1 AS RESULT, 1 AS RESULT1, 'A' COLHEAD ,'G' CATCODE"
        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN"
        'StrSql += vbCrLf + " (CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD, CATCODE) "
        'StrSql += vbCrLf + " SELECT 'SILVER' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,'0' AVERAGE, 1 AS RESULT, 1 AS RESULT1, 'A' COLHEAD ,'S' CATCODE"
        'StrSql += " END "

        'StrSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN WHERE CATCODE = 'P')>0"
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN"
        'StrSql += vbCrLf + " (CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD, CATCODE) "
        'StrSql += vbCrLf + " SELECT 'PLATINUM' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,'0' AVERAGE, 1 AS RESULT, 1 AS RESULT1, 'A' COLHEAD ,'P' CATCODE"
        'StrSql += " END "

        'StrSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN WHERE CATCODE = 'T')>0"
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN"
        'StrSql += vbCrLf + " (CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD, CATCODE) "
        'StrSql += vbCrLf + " SELECT 'STONE' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,'0' AVERAGE, 1 AS RESULT, 1 AS RESULT1, 'A' COLHEAD ,'T' CATCODE"
        'StrSql += " END "

        'StrSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN WHERE CATCODE = 'D')>0"
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN"
        'StrSql += vbCrLf + " (CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD, CATCODE) "
        'StrSql += vbCrLf + " SELECT 'DIAMOND' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,'0' AVERAGE, 1 AS RESULT, 1 AS RESULT1, 'A' COLHEAD ,'D' CATCODE"
        'StrSql += " END "

        'StrSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALESRETURN WHERE CATCODE = 'C')>0"
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ABSSALESRETURN"
        'StrSql += vbCrLf + " (CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD, CATCODE) "
        'StrSql += vbCrLf + " SELECT 'COPPER' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT,0 RECPCS, 0 RECGRSWT, 0 RECNETWT,"
        'StrSql += vbCrLf + " 0 RECEIPT, 0 PAYMENT,'0' AVERAGE, 1 AS RESULT, 1 AS RESULT1, 'A' COLHEAD ,'C' CATCODE"
        'StrSql += " END "

        'StrSql += " END "

        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

       

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
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
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
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
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
                StrSql += " SELECT M.METALID AS CATCODE, M.METALNAME AS CATNAME,  "
            End If
            StrSql += " 0 AS ISSPCS,  0 AS ISSGRSWT, 0 AS ISSNETWT,  "
            StrSql += " SUM(S.STNPCS) AS RECPCS,  SUM(S.STNWT)  AS RECGRSWT,  SUM(S.STNWT)  AS RECNETWT,  "
            StrSql += " 0 AS RECEIPT,  SUM(S.STNAMT) AS PAYMENT,  "
            StrSql += " 2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            StrSql += " 'D' COLHEAD  FROM " & cnStockDb & "..RECEIPTSTONE S "
            StrSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON S.STNITEMID =I.ITEMID "
            StrSql += " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID=M.METALID"
            StrSql += " WHERE S.TRANTYPE='PU' AND S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += " " & Replace(StrFilter, "SYSTEMID", "S.SYSTEMID") & " AND ISNULL(CANCEL,'') = ''  AND TRANTYPE = 'PU' AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += " AND  (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            StrSql += " AND S.COMPANYID IN (" & SelectedCompanyId & ")"
            If rbtCategoryWise.Checked = True Then
                StrSql += " GROUP BY S.CATCODE,S.BATCHNO  "
            Else
                StrSql += " GROUP BY S.BATCHNO,M.METALID,M.METALNAME  "
            End If
            StrSql += " ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        End If

        StrSql += " ) X "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        'If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPURCHASE)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "ABSPURCHASE(CATNAME, "
        StrSql += " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += " RESULT, RESULT1, COLHEAD) "
        StrSql += " SELECT 'PURCHASE'CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, 0 RESULT, 0 AS RESULT1, 'T' COLHEAD "
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        'Else
        'StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPURCHASE)>0 "
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSPURCHASE(CATNAME, "
        'StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
        'StrSql += vbCrLf + " SELECT 'PURCHASE' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        'StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT,SUM(ISNULL(PAYMENT,0))PAYMENT, 1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSPURCHASE GROUP BY CATCODE"
        ''StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        'End If


        'If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPURCHASE)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "ABSPURCHASE(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
        StrSql += " SELECT 'Z' AS CATCODE, 'PURCHASE TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += " SUM(PAYMENT),4 RESULT, 2 AS RESULT1, 'S' COLHEAD "
        StrSql += " FROM TEMP" & systemId & "ABSPURCHASE WHERE RESULT = 1 "
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        ' End If

        ' If rbtmetalwisegrp.Checked = False Then
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
        StrSql += " FROM TEMP" & systemId & "ABSPURCHASE "
        'StrSql += " WHERE RESULT = 1 "
        StrSql += " ORDER BY CATCODE, RESULT"
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        

    End Sub

    Private Sub ProcHomeSales()
        If Not chkHomeSale.Checked Then Exit Sub
        ' If rbtmetalwisegrp.Checked = False Then
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
        StrSql += " AND ISNULL(TAGNO,'') = ''/**/ " + StrFilter + StrUseridFtr
        StrSql += " GROUP BY CATCODE/**/"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        'Else
        '    StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSHOMESALE') DROP TABLE TEMP" & systemId & "ABSHOMESALE "
        '    StrSql += " SELECT METALID,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)CATNAME/**/"
        '    StrSql += " ,SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT,/**/"
        '    StrSql += " 1 AS RESULT1, CONVERT(VARCHAR,'') AS COLHEAD/**/"
        '    StrSql += " INTO TEMP" & systemId & "ABSHOMESALE FROM " & cnStockDb & "..ISSUE I/**/"
        '    StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'/**/"
        '    StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND  TRANTYPE = 'SA'/**/"
        '    StrSql += " AND ISNULL(I.CANCEL,'') = ''   AND ISNULL(FLAG,'') IN ('C', 'B') AND COMPANYID IN (" & SelectedCompanyId & ")/**/"
        '    StrSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(STOCKTYPE,'') = 'T')/**/"
        '    StrSql += " AND ISNULL(TAGNO,'') = ''/**/ " + StrFilter
        '    StrSql += " GROUP BY CATCODE,METALID/**/"
        '    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        '    Cmd.ExecuteNonQuery()

        'End If

        ' If rbtmetalwisegrp.Checked = False Then
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        'Else
        '    StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSHOMESALE)>0 "
        '    StrSql += " BEGIN "
        '    StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        '    StrSql += " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,CATCODE,COLHEAD,ORDERNO) "
        '    StrSql += " SELECT '  HOME SALES' CATNAME, "
        '    StrSql += " CASE WHEN SUM(ISNULL(ISSPCS,0))  <> 0 THEN SUM(ISNULL(ISSPCS,0)) ELSE NULL END ISSPCS, "
        '    StrSql += " CASE WHEN SUM(ISNULL(ISSGRSWT,0))  <> 0 THEN SUM(ISNULL(ISSGRSWT,0))  ELSE NULL END ISSGRSWT, "
        '    StrSql += " CASE WHEN SUM(ISNULL(ISSNETWT,0))  <> 0 THEN SUM(ISNULL(ISSNETWT,0))  ELSE NULL END ISSNETWT, "
        '    StrSql += " 1 RESULT1,METALID,'L' COLHEAD,3  "
        '    StrSql += " FROM TEMP" & systemId & "ABSHOMESALE GROUP BY METALID,COLHEAD ORDER BY RESULT1"
        '    StrSql += " END "
        '    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        '    Cmd.ExecuteNonQuery()
        'End If

    End Sub

    Private Sub ProcMiscIssue()
        If (RPT_ME_SUM_DABS = True And rbtMetalwise.Checked = True) Then Exit Sub
        If RPT_MIS_ISS_DABS = False Then Exit Sub
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
            StrSql += " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
            StrSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME,"
        End If
        StrSql += " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += " 0 AS RECEIPT, 0 AS PAYMENT, 1 AS RESULT1, ' ' COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSMISCISSUE FROM " & cnStockDb & "..ISSUE I "
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " TRANTYPE = 'MI' AND ISNULL(I.CANCEL,'') = ''  AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        If rbtCategoryWise.Checked = True Then
            StrSql += " GROUP BY CATCODE "
        Else
            StrSql += " GROUP BY METALID,CATCODE "
        End If
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        If rbtmetalwisegrp.Checked = False Then
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
        Else
            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSMISCISSUE)>0"
            StrSql += " BEGIN "
            StrSql += "    INSERT INTO TEMP" & systemId & "SASRPU (DESCRIPTION,"
            StrSql += " COLHEAD,CATCODE,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += " SELECT '  MISC ISSUE', "
            StrSql += " 'L' COLHEAD,METALID, RESULT1,3,'C' COLHEAD1"
            StrSql += "   FROM TEMP" & systemId & "ABSMISCISSUE"
            StrSql += "  ORDER BY COLHEAD "

            'StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            'StrSql += " (DESCRIPTION, COLHEAD) VALUES('MISC ISSUE','T') "
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,COLHEAD,CATCODE,RESULT1,ORDERNO) "
            StrSql += " SELECT CATNAME, "
            StrSql += " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            StrSql += " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += " 'L' COLHEAD,METALID, RESULT1,4 "
            StrSql += " FROM TEMP" & systemId & "ABSMISCISSUE "
            StrSql += " ORDER BY CATNAME,RESULT1"
            StrSql += " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSMISCISSUE)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,METALID,'L' COLHEAD,1,5,'M' COLHEAD1  "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSMISCISSUE GROUP BY METALID,COLHEAD,RESULT1"
            StrSql += vbCrLf + " ORDER BY COLHEAD"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

        End If
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
        StrSql += StrUseridFtr
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not chkMiscRecPaySummary.Checked Then StrSql += " AND TRANTYPE <> 'T'"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT 'REC' SEP,CASE WHEN SUBSTRING(RUNNO,6,1) IN ('O','R') THEN 'O' ELSE TRANTYPE END TRANTYPE,CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END AS AMOUNT,BATCHNO"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND RECPAY = 'R' AND ISNULL(CANCEL,'') = '' AND ISNULL(COMPANYID,'') IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not chkMiscRecPaySummary.Checked Then StrSql += " AND TRANTYPE <> 'T'"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT 'PAY' SEP,CASE WHEN SUBSTRING(RUNNO,6,1) IN ('O','R') THEN 'O' ELSE TRANTYPE END TRANTYPE,CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END AS AMOUNT,BATCHNO"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND RECPAY = 'P' AND ISNULL(CANCEL,'') = '' AND ISNULL(COMPANYID,'') IN (" & SelectedCompanyId & ")"
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not chkMiscRecPaySummary.Checked Then StrSql += " AND TRANTYPE <> 'T'"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " UPDATE TEMP" & systemId & "ADVDUE SET TRANTYPE = 'J'"
        StrSql += " FROM TEMP" & systemId & "ADVDUE AS O INNER JOIN " & cnAdminDb & "..ITEMDETAIL I ON I.BATCHNO = O.BATCHNO AND O.TRANTYPE ='D'"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVDUESUMMARY')"
        StrSql += vbCrLf + " DROP TABLE TEMP" & systemId & "ADVDUESUMMARY"
        StrSql += vbCrLf + " SELECT "
        StrSql += vbCrLf + " CASE TRANTYPE WHEN 'O' THEN 'ORDER ADVANCE' WHEN 'D' THEN 'DUE' WHEN 'T' THEN 'OTHERS' WHEN 'J' THEN 'JND' WHEN 'GV' THEN 'GIFT VOUCHER' ELSE 'ADVANCE' END AS TRANTYPE"
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
        StrSql += vbCrLf + " CASE TRANTYPE WHEN 'O' THEN 'ORDER ADVANCE' WHEN 'D' THEN 'DUE' WHEN 'T' THEN 'OTHERS' WHEN 'J' THEN 'JND' WHEN 'GV' THEN 'GIFT VOUCHER' ELSE 'ADVANCE' END AS TRANTYPE"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN AMOUNT ELSE 0 END) AS OPENING"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN AMOUNT ELSE 0 END)AS RECEIPT"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'PAY' THEN AMOUNT ELSE 0 END)AS PAYMENT"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN -1*AMOUNT ELSE AMOUNT END)AS CLOSING"
        StrSql += vbCrLf + " FROM"
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " SELECT SEP,TRANTYPE,AMOUNT FROM TEMP" & systemId & "ADVDUE WHERE TRANTYPE IN ('D','J')"
        StrSql += vbCrLf + " )X"
        StrSql += vbCrLf + " GROUP BY TRANTYPE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcAverageRate()
        StrSql = " SELECT CONVERT(NUMERIC(15,2),ISNULL(SUM(SRATE),0)/NULLIF(COUNT(*),0))AVGRATE FROM " & cnAdminDb & "..RATEMAST"
        'CASE WHEN SUM(GRSWT)<>0 THEN "
        'StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) "
        StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND METALID = 'G' AND PURITY = 91.6 "
        da = New OleDbDataAdapter(StrSql, cn)
        dtAverage.Clear()
        da.Fill(dtAverage)
        dtAverage.AcceptChanges()
        If dtAverage.Rows.Count > 0 Then
            AvgRate = Convert.ToDouble(dtAverage.Rows(0).Item(0).ToString)
        Else
            AvgRate = 0
        End If

        StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MELT_PURITY' "
        da = New OleDbDataAdapter(StrSql, cn)
        dtPurityRate.Clear()
        da.Fill(dtPurityRate)
        dtPurityRate.AcceptChanges()
        If dtPurityRate.Rows.Count > 0 Then
            PurityRate = Convert.ToDouble(dtPurityRate.Rows(0).Item(0).ToString)
        End If
        If PurityRate = 0.0 Then
            PurityRate = 91.6
        End If
        ''AvgRate = Math.Round(AvgRate / PurityRate * 100)
        'AvgRate = Format((AvgRate / PurityRate * 100), "0.00")
        AvgRate = Format((AvgRate / PurityRate), "0.00")

        'StrSql = "SELECT CATCODE FROM MASTER..TEMP" & systemId & "SALEABSTRACT "

        Dim RESULT As Decimal
        Dim SNO As Integer
        Dim dtAvgTotal As New DataTable

        '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' 

        'StrSql = " Exec " & cnStockDb & "..SP_RPT_SAL_DIFFERENCE "
        'StrSql += vbCrLf + " @FROMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        'StrSql += vbCrLf + " ,@TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        'StrSql += vbCrLf + " ,@METAL ='ALL' "
        'StrSql += vbCrLf + " ,@COSTCENTRE ='ALL'"
        'StrSql += vbCrLf + " ,@BILLNO ='0'"
        'StrSql += vbCrLf + " ,@BILLTOTAL ='N'"
        'StrSql += vbCrLf + " ,@BILLGRP ='N'"
        'StrSql += vbCrLf + " ,@CATGRP ='Y'"
        'StrSql += vbCrLf + " ,@SUMMARY ='Y'"
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        'StrSql = " SELECT  ISNULL(SUM(SD.DISCOUNT),0) AS DISCOUNT, SA.COL1, SA.CATEGORYCODE, SA.SNO, ISNULL(SA.COL8,0) AS RECEIPT, ISNULL(SA.COL4,0) AS NETWT FROM MASTER..TEMP" & systemId & "SALEABSTRACT SA"
        'StrSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMPSALDIFF SD ON SA.CATEGORYCODE = SD.CATCODE "
        'StrSql += vbCrLf + " WHERE ISNULL(SA.CATEGORYCODE,'') <> '' AND  SD.RESULT = 0 "
        'StrSql += vbCrLf + " GROUP BY SA.COL1, SA.CATEGORYCODE, SA.SNO, SA.COL8, SA.COL4"
        'da = New OleDbDataAdapter(StrSql, cn)

        'AVGRATE CALCULATED WITH DISCOUNT
        'StrSql = " SELECT  ISNULL(SUM(SD.DISCOUNT),0) AS DISCOUNT, SA.DESCRIPTION, SA.CATEGORYCODE, SA.SNO, ISNULL(SA.RECEIPT,0) "
        'StrSql += vbCrLf + " AS RECEIPT, ISNULL(SA.ISSNETWT,0) AS NETWT "
        'StrSql += vbCrLf + " FROM MASTER..TEMP" & systemId & "SASRPU SA"
        'StrSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMPSALDIFF SD ON SA.CATEGORYCODE = SD.CATCODE "
        'StrSql += vbCrLf + " WHERE ISNULL(SA.CATEGORYCODE,'') <> '' AND  SD.RESULT = 0 "
        'StrSql += vbCrLf + " GROUP BY SA.DESCRIPTION, SA.CATEGORYCODE, SA.SNO, SA.RECEIPT, SA.ISSNETWT"
        'da = New OleDbDataAdapter(StrSql, cn)
        'dtDiscount.Clear()
        'da.Fill(dtDiscount)
        'dtDiscount.AcceptChanges()
        'If dtDiscount.Rows.Count > 0 Then
        '    For CNT As Integer = 0 To dtDiscount.Rows.Count - 1
        '        DISCOUNT = Convert.ToDouble(dtDiscount.Rows(CNT).Item("DISCOUNT").ToString)
        '        RECEIPT = Convert.ToDouble(dtDiscount.Rows(CNT).Item("RECEIPT").ToString)
        '        NETWT = Convert.ToDouble(dtDiscount.Rows(CNT).Item("NETWT").ToString)
        '        SNO = Convert.ToInt32(dtDiscount.Rows(CNT).Item("SNO").ToString)
        '        If RECEIPT <> 0 And NETWT <> 0 Then
        '            Dim TOTAL As Decimal
        '            'RESULT = ((RECEIPT - DISCOUNT) / NETWT) / AvgRate * 100
        '            TOTAL = RECEIPT / NETWT
        '            RESULT = TOTAL / AvgRate
        '            RESULT = Format(RESULT, "0.00")
        '            StrSql = "UPDATE MASTER..TEMP" & systemId & "SASRPU SET AVGRATE = " & RESULT & " WHERE SNO = " & SNO & ""
        '            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        '            Cmd.ExecuteNonQuery()
        '        End If
        '    Next
        '    StrSql = "SELECT ISNULL(SUM(AVGRATE),0)AVGTOTAL, CATCODE FROM MASTER..TEMP" & systemId & "SASRPU"
        '    StrSql += vbCrLf + " WHERE CATCODE = 'G'"
        '    StrSql += vbCrLf + " GROUP BY CATCODE"
        '    da = New OleDbDataAdapter(StrSql, cn)
        '    dtAvgTotal.Clear()
        '    da.Fill(dtAvgTotal)
        '    If dtAvgTotal.Rows.Count > 0 Then
        '        RESULT = Nothing
        '        RESULT = Convert.ToDouble(dtAvgTotal.Rows(0).Item("AVGTOTAL").ToString)
        '        StrSql = "UPDATE MASTER..TEMP" & systemId & "SASRPU SET AVGRATE = " & RESULT & " WHERE CATCODE = 'G' AND DESCRIPTION = 'METAL TOTAL' AND RESULT2 = 2"
        '        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        '        Cmd.ExecuteNonQuery()
        '    End If

        '    StrSql = "SELECT ISNULL(SUM(AVGRATE),0)AVGTOTAL, CATCODE FROM MASTER..TEMP" & systemId & "SASRPU"
        '    StrSql += vbCrLf + " WHERE CATCODE = 'S'"
        '    StrSql += vbCrLf + " GROUP BY CATCODE"
        '    da = New OleDbDataAdapter(StrSql, cn)
        '    dtAvgTotal.Clear()
        '    da.Fill(dtAvgTotal)
        '    If dtAvgTotal.Rows.Count > 0 Then
        '        RESULT = Nothing
        '        RESULT = Convert.ToDouble(dtAvgTotal.Rows(0).Item("AVGTOTAL").ToString)
        '        StrSql = "UPDATE MASTER..TEMP" & systemId & "SASRPU SET AVGRATE = " & RESULT & " WHERE CATCODE = 'S' AND DESCRIPTION = 'METAL TOTAL' AND RESULT2 = 2"
        '        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        '        Cmd.ExecuteNonQuery()
        '    End If
        'End If

        StrSql = " SELECT SA.DESCRIPTION, SA.CATEGORYCODE, SA.SNO, ISNULL(SA.RECEIPT,0) "
        StrSql += vbCrLf + " AS RECEIPT, ISNULL(SA.ISSNETWT,0) AS NETWT "
        StrSql += vbCrLf + " FROM MASTER..TEMP" & systemId & "SASRPU SA"
        StrSql += vbCrLf + " WHERE ISNULL(SA.CATEGORYCODE,'') <> '' "
        StrSql += vbCrLf + " GROUP BY SA.DESCRIPTION, SA.CATEGORYCODE, SA.SNO, SA.RECEIPT, SA.ISSNETWT"
        da = New OleDbDataAdapter(StrSql, cn)
        dtDiscount.Clear()
        da.Fill(dtDiscount)
        dtDiscount.AcceptChanges()
        If dtDiscount.Rows.Count > 0 Then
            For CNT As Integer = 0 To dtDiscount.Rows.Count - 1
                'DISCOUNT = Convert.ToDouble(dtDiscount.Rows(CNT).Item("DISCOUNT").ToString)
                RECEIPT = Convert.ToDouble(dtDiscount.Rows(CNT).Item("RECEIPT").ToString)
                NETWT = Convert.ToDouble(dtDiscount.Rows(CNT).Item("NETWT").ToString)
                SNO = Convert.ToInt32(dtDiscount.Rows(CNT).Item("SNO").ToString)
                Dim TOTAL As Decimal
                If RECEIPT <> 0 And NETWT <> 0 Then
                    'RESULT = ((RECEIPT - DISCOUNT) / NETWT) / AvgRate * 100
                    TOTAL = RECEIPT / NETWT
                    RESULT = TOTAL / AvgRate
                    RESULT = Format(RESULT, "0.00")
                    StrSql = "UPDATE MASTER..TEMP" & systemId & "SASRPU SET AVGRATE = " & RESULT & " WHERE SNO = " & SNO & ""
                    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                    Cmd.ExecuteNonQuery()
                ElseIf RECEIPT <> 0 And NETWT = 0 Then
                    TOTAL = RECEIPT
                    RESULT = TOTAL / AvgRate
                    RESULT = Format(RESULT, "0.00")
                    StrSql = "UPDATE MASTER..TEMP" & systemId & "SASRPU SET AVGRATE = " & RESULT & " WHERE SNO = " & SNO & ""
                    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                    Cmd.ExecuteNonQuery()
                ElseIf RECEIPT = 0 And NETWT <> 0 Then
                    TOTAL = NETWT
                    RESULT = TOTAL / AvgRate
                    RESULT = Format(RESULT, "0.00")
                    StrSql = "UPDATE MASTER..TEMP" & systemId & "SASRPU SET AVGRATE = " & RESULT & " WHERE SNO = " & SNO & ""
                    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                    Cmd.ExecuteNonQuery()
                End If
            Next

            '''AVG TOTAL
            'StrSql = "SELECT ISNULL(AVGRATE,0)AVGTOTAL, CATCODE FROM MASTER..TEMP" & systemId & "SASRPU"
            'StrSql += vbCrLf + " WHERE CATCODE = 'G' AND DESCRIPTION = 'METAL TOTAL' AND RESULT2 = 2"

            'da = New OleDbDataAdapter(StrSql, cn)
            'dtAvgTotal.Clear()
            'da.Fill(dtAvgTotal)
            'If dtAvgTotal.Rows.Count > 0 Then
            '    RESULT = Nothing
            '    RESULT = Convert.ToDouble(dtAvgTotal.Rows(0).Item("AVGTOTAL").ToString)
            '    StrSql = "UPDATE MASTER..TEMP" & systemId & "SASRPU SET AVGRATE = " & RESULT & " WHERE CATCODE = 'G' AND DESCRIPTION = 'METAL TOTAL' AND RESULT2 = 2"
            '    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            '    Cmd.ExecuteNonQuery()
            'End If

            StrSql = " SELECT SA.CATCODE, SA.DESCRIPTION, SA.CATEGORYCODE, SA.SNO, ISNULL(SUM(SA.RECEIPT),0)AS RECEIPT,"
            StrSql += vbCrLf + " ISNULL(SUM(SA.ISSNETWT),0) AS NETWT"
            StrSql += vbCrLf + " FROM MASTER..TEMP" & systemId & "SASRPU SA"
            StrSql += vbCrLf + " WHERE DESCRIPTION = 'METAL TOTAL'"
            StrSql += vbCrLf + " GROUP BY SA.CATCODE, SA.DESCRIPTION, SA.CATEGORYCODE, SA.SNO"
            da = New OleDbDataAdapter(StrSql, cn)
            dtAvgTotal.Clear()
            da.Fill(dtAvgTotal)
            If dtAvgTotal.Rows.Count > 0 Then
                For CNT As Integer = 0 To dtAvgTotal.Rows.Count - 1
                    RECEIPT = Convert.ToDouble(dtAvgTotal.Rows(CNT).Item("RECEIPT").ToString)
                    NETWT = Convert.ToDouble(dtAvgTotal.Rows(CNT).Item("NETWT").ToString)
                    SNO = Convert.ToInt32(dtAvgTotal.Rows(CNT).Item("SNO").ToString)
                    If RECEIPT <> 0 And NETWT <> 0 Then
                        Dim TOTAL As Decimal
                        TOTAL = RECEIPT / NETWT
                        RESULT = TOTAL / AvgRate
                        RESULT = Format(RESULT, "0.00")
                        StrSql = "UPDATE MASTER..TEMP" & systemId & "SASRPU SET AVGRATE = " & RESULT & " WHERE SNO = " & SNO & ""
                        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                        Cmd.ExecuteNonQuery()
                    ElseIf RECEIPT <> 0 And NETWT = 0 Then
                        Dim TOTAL As Decimal
                        TOTAL = RECEIPT
                        RESULT = TOTAL / AvgRate
                        RESULT = Format(RESULT, "0.00")
                        StrSql = "UPDATE MASTER..TEMP" & systemId & "SASRPU SET AVGRATE = " & RESULT & " WHERE SNO = " & SNO & ""
                        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                        Cmd.ExecuteNonQuery()
                    ElseIf RECEIPT = 0 And NETWT <> 0 Then
                        Dim TOTAL As Decimal
                        TOTAL = NETWT
                        RESULT = TOTAL / AvgRate
                        RESULT = Format(RESULT, "0.00")
                        StrSql = "UPDATE MASTER..TEMP" & systemId & "SASRPU SET AVGRATE = " & RESULT & " WHERE SNO = " & SNO & ""
                        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                        Cmd.ExecuteNonQuery()
                    End If
                Next
            End If
        End If

        '''MELTING RATE

        'StrSql = " SELECT TOP 1 ISNULL(SNO,999) SNO FROM MASTER..TEMP" & systemId & "SALEABSTRACT SA"
        'StrSql += vbCrLf + " WHERE COL1 LIKE 'PURCHASE%' AND COLHEAD = 'T' "
        'da = New OleDbDataAdapter(StrSql, cn)
        'dtSnoFrom.Clear()
        'da.Fill(dtSnoFrom)

        'dtSnoFrom.AcceptChanges()
        'Dim iSnoFrom As Integer
        'If dtSnoFrom.Rows.Count > 0 Then
        '    iSnoFrom = Convert.ToInt32(dtSnoFrom.Rows(0).Item(0).ToString)
        'End If


        'StrSql = " SELECT TOP 1 ISNULL(SNO,999) SNO FROM MASTER..TEMP" & systemId & "SALEABSTRACT SA"
        'StrSql += vbCrLf + " WHERE COL1 = ' PURCHASE TOT%' AND COLHEAD = 'S' "
        'da = New OleDbDataAdapter(StrSql, cn)
        'dtSnoTo.Clear()
        'da.Fill(dtSnoTo)
        'dtSnoTo.AcceptChanges()

        'Dim iSnoTo As Integer
        'If dtSnoTo.Rows.Count > 0 Then
        '    iSnoTo = Convert.ToInt32(dtSnoTo.Rows(0).Item(0).ToString)
        'End If

        Dim iMeltingWt As Decimal
        Dim iGrsWt As Decimal
        StrSql = " SELECT ISNULL(RECNETWT,0) AS MELTINGWT, ISNULL(RECGRSWT,0) AS GRSWT, SNO FROM MASTER..TEMP" & systemId & "SASRPU SA"
        'StrSql += vbCrLf + " WHERE SNO BETWEEN " & iSnoFrom & " AND  " & iSnoTo & ""
        StrSql += vbCrLf + " WHERE COLHEAD = 'P'"
        da = New OleDbDataAdapter(StrSql, cn)

        Dim dtMeltingRate As New DataTable
        Dim dtMeltingTotal As New DataTable

        dtMeltingRate.Clear()
        da.Fill(dtMeltingRate)
        dtMeltingRate.AcceptChanges()

        If dtMeltingRate.Rows.Count > 0 Then

            For CNT As Integer = 0 To dtMeltingRate.Rows.Count - 1
                iMeltingWt = Convert.ToDouble(dtMeltingRate.Rows(CNT).Item("MELTINGWT").ToString)
                iGrsWt = Convert.ToDouble(dtMeltingRate.Rows(CNT).Item("GRSWT").ToString)
                SNO = Convert.ToInt32(dtMeltingRate.Rows(CNT).Item("SNO").ToString)

                If iMeltingWt <> 0 And iGrsWt <> 0 Then
                    RESULT = Nothing
                    RESULT = (iMeltingWt / iGrsWt) * 100
                    RESULT = Format(RESULT, "0.00")
                    StrSql = "UPDATE MASTER..TEMP" & systemId & "SASRPU SET AVGRATE = " & RESULT & " WHERE SNO = " & SNO & ""
                    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                    Cmd.ExecuteNonQuery()
                End If
            Next

            StrSql = "SELECT ISNULL(SUM(AVGRATE),0)MELTINGTOTAL, COLHEAD FROM MASTER..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " WHERE COLHEAD = 'P'"
            StrSql += vbCrLf + " GROUP BY COLHEAD"
            da = New OleDbDataAdapter(StrSql, cn)
            dtMeltingTotal.Clear()
            da.Fill(dtMeltingTotal)
            If dtMeltingTotal.Rows.Count > 0 Then
                RESULT = Nothing
                RESULT = Convert.ToDouble(dtMeltingTotal.Rows(0).Item("MELTINGTOTAL").ToString)
                StrSql = "UPDATE MASTER..TEMP" & systemId & "SASRPU SET AVGRATE = " & RESULT & " WHERE COLHEAD = 'S' AND DESCRIPTION  LIKE '%PURCHASE TOT%'"
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()
            End If
        End If
        'AvgRate = Math.Round(AvgRate / PurityRate * 100)
    End Sub

    Private Sub ProcSASRPU(ByVal DotMatrix As Boolean)

        PROCSAPSMI()
        If rbtmetalwisegrp.Checked = True Then
            ProcSales_MetalGroup()
        Else
            ProcSales()
        End If

        If RPT_SEPORD_DABS = True Then ProcSalesAndOrder()
        If rbtmetalwisegrp.Checked = False Then
            ProcRepairDelivery()
        End If

        If rbtmetalwisegrp.Checked = True Then
            ProcSalesReturn_MetalGroup()
        Else
            ProcSalesReturn()
        End If

        If rbtmetalwisegrp.Checked = True Then
            ProcPurchase_MetalGroup()
        Else
            ProcPurchase()
        End If

        If rbtmetalwisegrp.Checked = True Then
            ProcRepairGoldSales_MetalGroup()
        End If

        If RPT_SEPORD_DABS = True Then ProcOrderBooking()
        ProcOrderAdvanceAmountToWeight()
        ''If rbtmetalwisegrp.Checked = False Then
        ''    ProcMiscIssue()
        ''End If
        ProcMiscIssue()
        If chkDetaledRecPay.Checked = False Then
            ''EMPTY LINE
            StrSql = " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        '' ''EMPTY LINE
        StrSql = " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        ProcBeeds()
        ProcDiscount()
        ProcHandling()
        ProcroundOff()
        ProcApprovalIssue()
        ProcApprovalReceipt()
        ''
        ProcMaterialIssue()
        'If rbtmetalwisegrp.Checked = False Then
        '    ProcStockDetail()
        'End If
        ProcStockDetail()
        ProcLOTDetail()
        ProcHomeSales()
        'If rbtmetalwisegrp.Checked = False Then
        '    ProcPartlySales()
        'End If

        ProcWsSalesamt()
        ProcChitCollection()
        ProcWtSubtot()
        ProcCollection()
        ProcAmtSubtot()
        '''''ProcPartlySales()
        'If rbtmetalwisegrp.Checked = True Then
        '    ProcPartlySales()
        '    ProcStockDetail()
        'End If

        ProcPartlySales()


        If Chk_FinDiscount.Checked Then ProcDiscount_Fin()
        If chkAdvdueSummary.Checked Then ProcAdvDueSummary()
        If chkCancelBills.Checked Then ProcCancelBills()
        If rbtmetalwisegrp.Checked = False Then
            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SASRPU)>0"
            StrSql += " BEGIN "
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
            StrSql += " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COLHEAD) "
            StrSql += " SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,DISCPER,COLHEAD"
            StrSql += " FROM TEMP" & systemId & "SASRPU ORDER BY SNO" 'WHERE ISNULL(RESULT1,1) NOT IN (0,2) ORDER BY SNO"
            StrSql += " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SASRPU)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
            StrSql += vbCrLf + " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,CATCODE,COLHEAD,ORDERNO,COLHEAD1, CATEGORYCODE, RESULT2, COLHEAD2) "

            StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = CATCODE)AS DESCRIPTION,"
            StrSql += vbCrLf + " 0 ISSPCS,0 ISSGRSWT,0 ISSNETWT,0 RECPCS,0 RECGRSWT,0 RECNETWT,0 RECEIPT,0 PAYMENT,'0' AVERAGE,0 DISCPER,CATCODE,'T' COLHEAD,0 ORDERNO,'' COLHEAD1, '', 0 RESULT2, 'A' COLHEAD2"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPU WHERE CATCODE<>'' GROUP BY CATCODE"
            StrSql += vbCrLf + " UNION ALL"

            StrSql += vbCrLf + " SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,DISCPER,CATCODE,COLHEAD,ORDERNO, COLHEAD1, CATEGORYCODE, RESULT2,COLHEAD2  "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPU WHERE CATCODE<>'' "
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT2, COLHEAD2"

            'StrSql += vbCrLf + " UNION ALL"
            ''StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
            ''StrSql += vbCrLf + " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,CATCODE,COLHEAD,ORDERNO,COLHEAD1) "

            'StrSql += vbCrLf + " SELECT 'METAL TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
            'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
            'StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,NULL DISCPER,CATCODE,'S' COLHEAD,9 ORDERNO,'M ' COLHEAD1, '', '' RESULT2,'' COLHEAD2    "
            'StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPU WHERE CATCODE<>'' AND COLHEAD1='M' GROUP BY CATCODE,COLHEAD,COLHEAD1"
            'StrSql += vbCrLf + " ORDER BY CATCODE,ORDERNO"

            'StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
            'StrSql += vbCrLf + " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COLHEAD,ORDERNO, CATEGORYCODE, RESULT2, COLHEAD2) "
            'StrSql += vbCrLf + " SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,DISCPER,COLHEAD,8 , CATEGORYCODE, 8, 'Z' "
            'StrSql += vbCrLf + " FROM TEMP" & systemId & "SASRPU  WHERE COLHEAD NOT IN ('L','X','R') OR COLHEAD IS NULL AND DESCRIPTION NOT IN ('TOTAL') ORDER BY SNO"
            ''StrSql += " SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD "
            ''StrSql += " FROM TEMP" & systemId & "SASRPU ORDER BY SNO" 'WHERE ISNULL(RESULT1,1) NOT IN (0,2) ORDER BY SNO"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            'StrSql = " UPDATE TEMP" & systemId & "SALEABSTRACT SET CATCODE = 'Z' WHERE ISNULL(RESULT2,'') = ''"
            'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            'Cmd.ExecuteNonQuery()
        End If
        If chkAdvdueSummary.Checked Then

            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ADVDUESUMMARY)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1,COLHEAD) VALUES('ADV-DUE SUMMARY','T')"
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
            StrSql += " (COL1,COL6,COL7,COL8,COL9,COLHEAD,ORDERNO) "
            StrSql += " SELECT ' ','OPENING','RECEIPT','PAYMENT','CLOSING','S',9 ORDERNO"
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
            StrSql += " (COL1,COL6,COL7,COL8,COL9,ORDERNO) "
            StrSql += vbCrLf + " SELECT ' ' + TRANTYPE, "
            StrSql += vbCrLf + " CASE WHEN OPENING   <> 0 THEN OPENING   ELSE NULL END OPENING, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " CASE WHEN CLOSING   <> 0 THEN CLOSING  ELSE NULL END CLOSING,9 ORDERNO "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ADVDUESUMMARY "
            If RPT_HIDE_GIFTOTHERS Then
                StrSql += vbCrLf + " WHERE TRANTYPE NOT IN('OTHERS','GIFT VOUCHER') "
            End If
            StrSql += vbCrLf + " ORDER BY TRANTYPE"
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
            StrSql += " (COL1,COL6,COL7,COL8,COL9,COLHEAD,ORDERNO) "
            StrSql += vbCrLf + " SELECT 'TOTAL',SUM(OPENING),SUM(RECEIPT),SUM(PAYMENT),SUM(CLOSING),'S',9 "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ADVDUESUMMARY "
            If RPT_HIDE_GIFTOTHERS Then
                StrSql += vbCrLf + " WHERE TRANTYPE NOT IN('OTHERS','GIFT VOUCHER') "
            End If
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        If chkCancelBills.Checked Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSCANCELBILL )>0 "
            StrSql += " BEGIN "
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1,COLHEAD) VALUES('CANCEL BILLS','T')"
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
            StrSql += " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,ORDERNO) "
            StrSql += " SELECT '  ' + TRANS, "
            StrSql += " CASE WHEN IPCS   <> 0 THEN IPCS   ELSE NULL END IPCS, "
            StrSql += " CASE WHEN IGRSWT <> 0 THEN IGRSWT ELSE NULL END IGRSWT, "
            StrSql += " CASE WHEN INETWT <> 0 THEN INETWT ELSE NULL END INETWT, "
            StrSql += " CASE WHEN RPCS   <> 0 THEN RPCS   ELSE NULL END RPCS, "
            StrSql += " CASE WHEN RGRSWT <> 0 THEN RGRSWT ELSE NULL END RGRSWT, "
            StrSql += " CASE WHEN RNETWT <> 0 THEN RNETWT ELSE NULL END RNETWT, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT,10 ORDERNO FROM "
            StrSql += " TEMP" & systemId & "ABSCANCELBILL ORDER BY TRANTYPE, TRANNO"
            StrSql += " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
        If RPT_CNLINCL_DABS Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSCANCELBILL )=0 "
            StrSql += " BEGIN "
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1,COLHEAD) VALUES('CANCEL BILLS','T')"
            StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
            StrSql += " (COL1,COL2) "
            StrSql += " VALUES('  NO CANCEL TRANSACTION' ,'NIL') END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        If rbtmetalwisegrp.Checked = True Then
            ProcAverageRate()
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
            .Columns.Add("COLHEAD1", GetType(String))

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
        StrSql += " AVERAGE, COLHEAD AS COLHEAD,COLHEAD1 FROM TEMP" & systemId & "SASRPU WHERE COLHEAD<>'L' ORDER BY SNO, RESULT1 "
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dt)
        'dt.Columns.Remove("AVERAGE")
        dt.AcceptChanges()
        If dt.Rows.Count > 0 Then dsReportCol.Tables.Add(dt)



    End Sub

    Private Sub ProcCreditSales()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDSAL') DROP TABLE TEMP" & systemId & "ABSCREDSAL "
        StrSql += vbCrLf + " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += vbCrLf + " ISNULL((select top 1 '(JND)' from " & cnAdminDb & "..ITEMDETAIL where batchno=o.batchno),'')+"
        StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +')' AS CATNAME,'Z' CATCODE,   "
        'StrSql += vbCrLf + " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += vbCrLf + " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END) AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, 'D' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMP" & systemId & "ABSCREDSAL FROM " & cnAdminDb & "..OUTSTANDING O"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(O.CANCEL,'') = ''  "
        StrSql += vbCrLf + " AND O.COMPANYID IN (" & SelectedCompanyId & ") AND O.RECPAY = 'P' AND O.PAYMODE = 'DU'"
        StrSql += vbCrLf + " AND O.BATCHNO NOT IN (select BATCHNO from " & cnAdminDb & "..ITEMDETAIL WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(O.CANCEL,'') = '') "
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.TRANNO,O.TRANDATE, CATCODE"
        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += vbCrLf + " ISNULL((select top 1 '(JND)' from " & cnAdminDb & "..ITEMDETAIL where batchno=o.batchno),'')+"
        StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +')' AS CATNAME, CATCODE,  "
        'StrSql += vbCrLf + " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += vbCrLf + " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END) AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, 'J' AS COLHEAD "
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(O.CANCEL,'') = ''  "
        StrSql += vbCrLf + " AND O.COMPANYID IN (" & SelectedCompanyId & ") AND O.RECPAY = 'P' AND O.PAYMODE = 'DU'"
        StrSql += vbCrLf + " AND O.BATCHNO IN (select BATCHNO from " & cnAdminDb & "..ITEMDETAIL WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '') "
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.TRANNO,O.TRANDATE, CATCODE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSCREDSAL)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,PAYMENT, CATCODE) "
            StrSql += vbCrLf + " SELECT 'CREDIT BILL(DUE) ['+CONVERT(VARCHAR,ISNULL(SUM(ISNULL(PAYMENT,0)-ISNULL(RECEIPT,0)),0)) +']'"
            StrSql += vbCrLf + " ,'T',NULL,'Z' CATCODE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSCREDSAL WHERE COLHEAD = 'D'"
            StrSql += vbCrLf + " GROUP BY CATCODE"
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD, CATCODE) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, '' COLHEAD ,'Z' "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSCREDSAL WHERE COLHEAD = 'D'"
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,PAYMENT, CATCODE) "
            StrSql += vbCrLf + " SELECT 'CREDIT BILL(DUE)'"
            StrSql += vbCrLf + " ,'T',SUM(PAYMENT)-SUM(RECEIPT), 'Z'CATCODE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSCREDSAL WHERE COLHEAD = 'D'"
            StrSql += vbCrLf + " GROUP BY CATCODE"
        End If

        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,PAYMENT, CATCODE) "
            StrSql += vbCrLf + " SELECT 'CREDIT BILL(JND) ['+CONVERT(VARCHAR,ISNULL(SUM(ISNULL(PAYMENT,0)-ISNULL(RECEIPT,0)),0)) +']'"
            StrSql += vbCrLf + " ,'T',NULL, CATCODE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSCREDSAL WHERE COLHEAD ='J'"
            StrSql += vbCrLf + " GROUP BY CATCODE"
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, '' COLHEAD  "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSCREDSAL WHERE COLHEAD ='J'"
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,PAYMENT, CATCODE) "
            StrSql += vbCrLf + " SELECT 'CREDIT BILL(JND)'"
            StrSql += vbCrLf + " ,'T',SUM(PAYMENT)-SUM(RECEIPT),'' CATCODE"
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSCREDSAL WHERE COLHEAD ='J'"
            StrSql += vbCrLf + " GROUP BY CATCODE"
        End If


        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcCreditPurchase()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDPUR') DROP TABLE TEMP" & systemId & "ABSCREDPUR"
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSCREDPUR FROM " & cnAdminDb & "..OUTSTANDING O"
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''  "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND O.RECPAY = 'R' AND PAYMODE = 'DU'"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.TRANNO,O.TRANDATE"
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
        'StrSql += vbCrLf + " ISNULL((select top 1 '(JND)' from " & cnAdminDb & "..ITEMDETAIL where runno=o.runno),'')+"
        '        StrSql += " '(INV NO: '+SUBSTRING(O.RUNNO,6,20) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +' -ADJ NO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += " 1 AS RESULT1, 'D' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSCREDADJ FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += "  AND O.RECPAY = 'R' AND COMPANYID IN (" & SelectedCompanyId & ") AND PAYMODE = 'DR'"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " AND O.BATCHNO NOT IN (select BATCHNO from " & cnAdminDb & "..ITEMDETAIL WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '') "
        StrSql += " GROUP BY O.BATCHNO,O.TRANNO,O.RUNNO,O.TRANDATE"
        StrSql += " UNION ALL "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO  WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += vbCrLf + " ISNULL((select top 1 '(JND)' from " & cnAdminDb & "..ITEMDETAIL where runno=o.runno),'')+"
        '        StrSql += " '(INV NO: '+SUBSTRING(O.RUNNO,6,20) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +' -ADJ NO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += " 1 AS RESULT1, 'J' AS COLHEAD "
        StrSql += "  FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += "  AND O.RECPAY = 'R' AND COMPANYID IN (" & SelectedCompanyId & ") AND PAYMODE = 'DR'"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " AND O.BATCHNO IN (select BATCHNO from " & cnAdminDb & "..ITEMDETAIL WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '') "
        StrSql += " GROUP BY O.BATCHNO,O.TRANNO,O.RUNNO,O.TRANDATE"

        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSCREDADJ)>0 "
        StrSql += " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) "
            StrSql += " SELECT 'CREDIT RECEIPT(DUE) [' + CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),0))+']','T'"
            StrSql += " FROM TEMP" & systemId & "ABSCREDADJ WHERE COLHEAD='D'"
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  ' + CATNAME CATNAME, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += " 1 RESULT1, '' COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "ABSCREDADJ WHERE COLHEAD='D'"
            If rbtAmount.Checked = True Then
                StrSql += " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += " SELECT 'CREDIT RECEIPT(DUE)','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += " FROM TEMP" & systemId & "ABSCREDADJ WHERE COLHEAD='D'"
        End If

        If chkDetaledRecPay.Checked Then
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) "
            StrSql += " SELECT 'CREDIT RECEIPT(JND) [' + CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),0))+']','T'"
            StrSql += " FROM TEMP" & systemId & "ABSCREDADJ WHERE COLHEAD='J'"
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += " SELECT '  ' + CATNAME CATNAME, "
            StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += " 1 RESULT1, '' COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "ABSCREDADJ WHERE COLHEAD='J'"
            If rbtAmount.Checked = True Then
                StrSql += " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += " SELECT 'CREDIT RECEIPT(JND)','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += " FROM TEMP" & systemId & "ABSCREDADJ WHERE COLHEAD='J'"
        End If

        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcCreditPurchasePayment()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDPURPAY') DROP TABLE TEMP" & systemId & "ABSCREDPURPAY "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO  WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += " '(INV NO: '+SUBSTRING(O.RUNNO,6,20) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +', NETWT: '+ CONVERT (VARCHAR(20),(SELECT SUM(ISNULL(NETWT,0)) FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO IN(SELECT BATCHNO FROM  " & cnAdminDb & "..OUTSTANDING WHERE RUNNO=O.RUNNO))) +')' AS CATNAME,  "
        StrSql += " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN AMOUNT ELSE 0 END) AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSCREDPURPAY FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += "  AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += "  AND PAYMODE = 'DP' AND O.RECPAY = 'P'"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.TRANNO,O.TRANDATE,O.RUNNO"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcAdvanceReceived()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVREC') DROP TABLE TEMP" & systemId & "ADVREC "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += " CASE WHEN PAYMODE IN('AR','OR') THEN '(INV NO: ' ELSE '(GIFT VOU NO:' END + CONVERT(VARCHAR(12),O.TRANNO) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ADVREC FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('AR','GV','OR') AND O.RECPAY = 'R' "
        StrSql += " AND O.TRANTYPE IN ('A','GV')"
        StrSql += " AND NOT (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANNO,O.TRANDATE,PAYMODE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub


    Private Sub ProcAdvanceAdjustment()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVADJ') DROP TABLE TEMP" & systemId & "ADVADJ "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+' -ADJ NO:'+ SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END) AS PAYMENT, "
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ADVADJ FROM " & cnAdminDb & "..OUTSTANDING O"
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND PAYMODE IN ('AA','AP') AND O.RECPAY = 'P' "
        StrSql += " AND O.TRANTYPE = 'A'"
        StrSql += " AND NOT (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        'StrSql += " AND O.RUNNO LIKE 'A%'"
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcRepairAdvance()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "REPAIRADV') DROP TABLE TEMP" & systemId & "REPAIRADV "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE 0 END)  AS RECEIPT, 0 AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "REPAIRADV FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('OR','AR') AND O.RECPAY = 'R' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " AND SUBSTRING(O.RUNNO,6,1) = 'R'"
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcRepairAdvanceAdjusted()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "REPAIRADVADJ') DROP TABLE TEMP" & systemId & "REPAIRADVADJ "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END)  AS PAYMENT, 0 AS RECEIPT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "REPAIRADVADJ FROM " & cnAdminDb & "..OUTSTANDING AS O "

        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('AA','AP') AND O.RECPAY = 'P' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " AND SUBSTRING(O.RUNNO,6,1) = 'R'"
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        'StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += " (DESCRIPTION, COLHEAD) "
        'StrSql += " SELECT 'REPAIR ADVANCE ADJUSTED ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
        'StrSql += " FROM TEMP" & systemId & "REPAIRADVADJ "
        'StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        'StrSql += " SELECT '  ' + CATNAME CATNAME, "
        'StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        'StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        'StrSql += " 1 RESULT1, COLHEAD  "
        'StrSql += " FROM TEMP" & systemId & "REPAIRADVADJ "
        'If rbtAmount.Checked = True Then
        '    StrSql += " ORDER BY RESULT1,RECEIPT "
        'Else
        '    StrSql += " ORDER BY RESULT1,CATNAME "
        'End If
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcOrderAdvance()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ORDERADV') DROP TABLE TEMP" & systemId & "ORDERADV "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE 0 END)  AS RECEIPT, 0 AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ORDERADV FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('OR','AR') AND O.RECPAY = 'R' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " AND SUBSTRING(O.RUNNO,6,1) <> 'R'"
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
            StrSql += " FROM TEMP" & systemId & "ORDERADV  WHERE (ISNULL(RECEIPT,0)<>0 AND ISNULL(PAYMENT,0)=0) "
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcOrderAdvanceAdjusted()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ORDERADVADJ') DROP TABLE TEMP" & systemId & "ORDERADVADJ "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END)  AS PAYMENT, 0 AS RECEIPT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ORDERADVADJ FROM " & cnAdminDb & "..OUTSTANDING AS O "

        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('AA','AP') AND O.RECPAY = 'P' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " AND SUBSTRING(O.RUNNO,6,1) <> 'R'"
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANNO,O.TRANDATE"
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
        StrSql += StrUseridFtr
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
            StrSql += StrFilter & StrUseridFtr 'StrCostFiltration
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
        StrSql += StrUseridFtr
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
        StrSql += StrUseridFtr
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
        StrSql += StrUseridFtr
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
        StrSql += StrUseridFtr
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
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        StrSql += StrUseridFtr
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END),0) <> 0"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcDiscount_Fin()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "FIN_DISCOUNT') DROP TABLE TEMP" & systemId & "FIN_DISCOUNT "
        StrSql += " SELECT 'DISCOUNT' CATNAME,  "
        StrSql += " SUM(ISNULL(DISCOUNT,0)+ISNULL(FIN_DISCOUNT,0)) AS AMT, "
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "FIN_DISCOUNT FROM " & cnStockDb & "..ISSUE AS A  "
        StrSql += " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
        StrSql += " AND TRANTYPE IN ('SA','OD')  AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        'StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "FIN_DISCOUNT)>0 "
        StrSql += " BEGIN "
        'StrSql += " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += " SELECT '  ' + CATNAME CATNAME,  "
        StrSql += " NULL RECEIPT,  "
        StrSql += " ABS(AMT)  PAYMENT, "
        StrSql += " 1 RESULT1, COLHEAD  "
        StrSql += " FROM TEMP" & systemId & "FIN_DISCOUNT ORDER BY CATNAME  "
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        StrSql += StrUseridFtr
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END),0) <> 0"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        StrSql += StrUseridFtr
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END),0) <> 0"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub


    Private Sub ProcWtSubtot()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "WTSTOT') DROP TABLE TEMP" & systemId & "WTSTOT "
        StrSql += " SELECT * INTO TEMP" & systemId & "WTSTOT  FROM ("
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABSSALES WHERE ISNULL(RESULT1,1) NOT IN (0,2) AND ISNULL(COLHEAD,'') <> 'L' " 'AND ISNULL(COLHEAD,'') <> 'D'
        StrSql += " UNION ALL"
        If rbtmetalwisegrp.Checked = False Then
            StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABSREPAIR WHERE ISNULL(RESULT1,1) NOT IN (0,2) AND ISNULL(COLHEAD,'') <> 'D'"
        Else
            StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABSSALESRD WHERE ISNULL(RESULT1,1) NOT IN (0,2) AND ISNULL(COLHEAD,'') <> 'D'"
        End If

        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABSSALESRETURN WHERE ISNULL(RESULT1,1) NOT IN (0,2) AND ISNULL(COLHEAD,'') <> 'L'" 'AND ISNULL(COLHEAD,'') <> 'D'"
        StrSql += " UNION ALL"
        StrSql += " SELECT 0 RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABS_ORDAMTWT"
        StrSql += " UNION ALL"
        StrSql += " SELECT RECEIPT, PAYMENT  FROM TEMP" & systemId & "ABSPURCHASE WHERE ISNULL(RESULT1,1) NOT IN (0,2) AND ISNULL(COLHEAD,'') <> 'L'  " 'AND ISNULL(COLHEAD,'') <> 'D'"
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
        StrSql += " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT  FROM TEMP" & systemId & "ABSWSSALE"
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "WTSTOT)>0 "
        StrSql += " BEGIN "
        StrSql += "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "WTSTOTAL') DROP TABLE TEMP" & systemId & "WTSTOTAL "
        StrSql += " SELECT SUM(RECEIPT) AS RECEIPT, SUM(PAYMENT) AS PAYMENT INTO TEMP" & systemId
        StrSql += "WTSTOTAL FROM TEMP" & systemId & "WTSTOT  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcChitCollection()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITCOLLECTION') DROP TABLE TEMP" & systemId & "CHITCOLLECTION "
        StrSql += vbCrLf + " SELECT CONVERT(NUMERIC(15,2),0)RECEIPT,CONVERT(NUMERIC(15,2),0)PAYMENT"
        StrSql += vbCrLf + " INTO TEMP" & systemId & "CHITCOLLECTION"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
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
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
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
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
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
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                StrSql += StrCostFiltration
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"
                StrSql += vbCrLf + " AND INSTALLMENT = 1"
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT 'OTHER : Rs '+ CASE WHEN SUM(AMOUNT) > 0 THEN CONVERT(VARCHAR,ISNULL(CONVERT(NUMERIC(15,2),SUM(AMOUNT)),'')) + ' ('+CONVERT(VARCHAR,ISNULL(COUNT(*),'')) + ' NOs)' ELSE '' END AS PARTICULAR FROM " & cnChitTrandb & "..SCHEMETRAN"
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
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
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()

            End If
        End If
    End Sub

    Private Sub ProcOrderBooking()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ORDERBOOK') DROP TABLE TEMP" & systemId & "ORDERBOOK"
        StrSql += " SELECT M.METALNAME AS CATNAME "
        StrSql += " ,SUM(PCS) AS RECPCS,SUM(O.GRSWT) AS RECGRSWT,SUM(NETWT) AS RECNETWT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ORDERBOOK FROM " & cnAdminDb & "..ORMAST O"
        StrSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON O.ITEMID = I.ITEMID"
        StrSql += " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
        StrSql += " WHERE ORDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " AND ORTYPE = 'O' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND O.COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrCostFiltration
        StrSql += StrUseridFtr
        StrSql += StrFilter
        StrSql += " GROUP BY METALNAME"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ORDERBOOK)>0 "
        StrSql += " BEGIN "
        StrSql += "INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION, COLHEAD) VALUES('ORDER BOOKING','T') "
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,RECPCS,RECGRSWT,RECNETWT,RESULT1,COLHEAD) "
        StrSql += " SELECT '  ' + CATNAME CATNAME, "
        StrSql += " CASE WHEN RECPCS    <> 0 THEN RECPCS    ELSE NULL END RECPCS, "
        StrSql += " CASE WHEN RECGRSWT  <> 0 THEN RECGRSWT  ELSE NULL END RECGRSWT, "
        StrSql += " CASE WHEN RECNETWT  <> 0 THEN RECNETWT  ELSE NULL END RECNETWT, "
        StrSql += " 1 RESULT1, COLHEAD  "
        StrSql += " FROM TEMP" & systemId & "ORDERBOOK ORDER BY RESULT1 "
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


    End Sub


    Private Sub ProcOrderAdvanceAmountToWeight()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABS_ORDAMTWT') DROP TABLE TEMP" & systemId & "ABS_ORDAMTWT "
        StrSql += " SELECT "
        If chkAdvwtdetail.Checked Then
            StrSql += " TRANNO,'REF NO : (' + ISNULL((SELECT TOP 1 SUBSTRING(ORNO,6,20) FROM " & cnAdminDb & "..ORMAST WHERE BATCHNO = I.BATCHNO),'') + ') ' + CONVERT(VARCHAR,GRSWT) + '@' + CONVERT(VARCHAR,RATE) AS CATNAME"
            StrSql += " ,PCS AS RECPCS, GRSWT AS RECGRSWT, NETWT AS RECNETWT,0 PAYMENT,"
        Else
            StrSql += " 0 TRANNO, ISNULL((SELECT TOP 1 metalname FROM " & cnAdminDb & "..METALMAST M WHERE M.METALID = I.METALID),'') AS CATNAME"
            StrSql += " ,SUM(PCS) AS RECPCS, SUM(GRSWT) AS RECGRSWT, SUM(NETWT) AS RECNETWT,0 PAYMENT,"
        End If
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABS_ORDAMTWT FROM " & cnStockDb & "..RECEIPT I"
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " AND I.TRANTYPE = 'AD' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        If Not chkAdvwtdetail.Checked Then StrSql += " GROUP BY I.METALID"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


    End Sub

    Private Sub ProcApprovalIssue()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSAPPROVALISS') DROP TABLE TEMP" & systemId & "ABSAPPROVALISS "
        StrSql += " SELECT TRANNO"
        StrSql += " ,(SELECT (PNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))PARTYNAME"
        StrSql += " , (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        StrSql += " WHERE ITEMID = I.ITEMID)+CASE WHEN ISNULL(TAGNO,'') <> '' THEN ' - TAGNO('+TAGNO+')' ELSE '' END AS CATNAME, "
        StrSql += " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " ,(SELECT (PNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))PNAME"
        StrSql += " INTO TEMP" & systemId & "ABSAPPROVALISS FROM " & cnStockDb & "..ISSUE I"
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " AND I.TRANTYPE = 'AI' AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " GROUP BY ITEMID,TAGNO,TRANNO,BATCHNO"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSAPPROVALISS)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "ABSAPPROVALISS(PNAME,CATNAME,RESULT1,COLHEAD)"
        StrSql += " SELECT DISTINCT PNAME,PNAME,0 RESULT1,'P' COLHEAD FROM TEMP" & systemId & "ABSAPPROVALISS"
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
            StrSql += " 1 RESULT1,COLHEAD  "
            StrSql += " FROM TEMP" & systemId & "ABSAPPROVALISS ORDER BY PNAME,PARTYNAME,CATNAME "
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
        StrSql += " SELECT TRANNO"
        StrSql += " ,(SELECT (PNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))PARTYNAME"
        StrSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        StrSql += " WHERE ITEMID = I.ITEMID)+CASE WHEN ISNULL(TAGNO,'') <> '' THEN ' - TAGNO('+TAGNO+')' ELSE '' END + CASE WHEN ISNULL(RUNNO,'') <> '' THEN ' [RUNNO : ' +  SUBSTRING(RUNNO,6,20) + ']' ELSE '' END AS CATNAME, "
        StrSql += " SUM(PCS) AS RECPCS, SUM(GRSWT) AS RECGRSWT, SUM(NETWT) AS RECNETWT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " ,(SELECT (PNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))PNAME"
        StrSql += " INTO TEMP" & systemId & "ABSAPPROVALREC FROM " & cnStockDb & "..RECEIPT I"
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " AND I.TRANTYPE = 'AR' AND ISNULL(CANCEL,'') = ''"
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " GROUP BY ITEMID,TAGNO,TRANNO,RUNNO,BATCHNO"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSAPPROVALREC)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "ABSAPPROVALREC(PNAME,CATNAME,RESULT1,COLHEAD)"
        StrSql += " SELECT DISTINCT PNAME,PNAME,0 RESULT1,'P' COLHEAD FROM TEMP" & systemId & "ABSAPPROVALREC"
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
            StrSql += " FROM TEMP" & systemId & "ABSAPPROVALREC ORDER BY PNAME,PARTYNAME,CATNAME "
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
    Private Sub ProcStockDetail()
        If chkstockdetail.Checked Then
            Dim selectedcostid As String = GetSelectedCostId(chklstCostCentre, True)
            StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "STOCKDETAIL') DROP TABLE TEMP" & systemId & "STOCKDETAIL "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
            StrSql = vbCrLf + " SELECT CATCODE,CATNAME,SUM(ISSPCS)ISSPCS,SUM(ISSGRSWT)ISSGRSWT,SUM(ISSNETWT)ISSNETWT"
            StrSql += vbCrLf + " ,SUM(RECPCS)RECPCS,SUM(RECGRSWT)RECGRSWT,SUM(RECNETWT) RECNETWT,RECEIPT,PAYMENT,RESULT,RESULT1,AVERAGE,RATE,COLHEAD"
            StrSql += vbCrLf + "  INTO TEMP" & systemId & "STOCKDETAIL FROM("
            'StrSql += vbCrLf + " SELECT IM.CATCODE,C.CATNAME,SUM(I.PCS)ISSPCS,SUM(I.GRSWT)ISSGRSWT,SUM(I.NETWT)ISSNETWT"
            'StrSql += vbCrLf + " ,0 RECPCS,0 RECGRSWT,0 RECNETWT,0 AS RECEIPT,0 PAYMENT, 3 AS RESULT, 1 AS RESULT1,CONVERT(VARCHAR(20), '') AS AVERAGE"
            'StrSql += vbCrLf + " , 0 RATE, CONVERT(VARCHAR(3),'') COLHEAD FROM " & cnAdminDb & "..ITEMTAG AS I"
            'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON  I.ITEMID=IM.ITEMID"
            'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY AS C ON IM.CATCODE=C.CATCODE WHERE 1=1"
            'StrSql += vbCrLf + " AND I.ISSDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'AND'" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            'StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
            'StrSql += vbCrLf + " AND I.COSTID IN(" & selectedcostid & ")"
            'StrSql += vbCrLf + " GROUP BY IM.CATCODE,C.CATNAME"
            'StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT IM.CATCODE,C.CATNAME,0 ISSPCS,0 ISSGRSWT,0 ISSNETWT"
            StrSql += vbCrLf + " ,SUM(I.PCS) RECPCS,SUM(I.GRSWT) RECGRSWT,SUM(I.NETWT) RECNETWT,0 AS RECEIPT,0 PAYMENT, 3 AS RESULT, 1 AS RESULT1,CONVERT(VARCHAR(20), '') AS AVERAGE"
            StrSql += vbCrLf + " , 0 RATE, CONVERT(VARCHAR(3),'') COLHEAD FROM " & cnAdminDb & "..ITEMTAG AS I"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON  I.ITEMID=IM.ITEMID"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY AS C ON IM.CATCODE=C.CATCODE WHERE 1=1"
            StrSql += vbCrLf + " AND I.RECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'AND'" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
            If selectedcostid <> "''" Then StrSql += vbCrLf + " AND I.COSTID IN(" & selectedcostid & ")"
            StrSql += vbCrLf + " GROUP BY IM.CATCODE,C.CATNAME"
            'StrSql += vbCrLf + " UNION ALL"
            'StrSql += vbCrLf + " SELECT IM.CATCODE,C.CATNAME,SUM(I.PCS)ISSPCS,SUM(I.GRSWT)ISSGRSWT,SUM(I.NETWT)ISSNETWT"
            'StrSql += vbCrLf + " ,0 RECPCS,0 RECGRSWT,0 RECNETWT,0 AS RECEIPT,0 PAYMENT, 3 AS RESULT, 1 AS RESULT1,'' AVERAGE"
            'StrSql += vbCrLf + " , 0 RATE, CONVERT(VARCHAR(3),'') COLHEAD FROM " & cnAdminDb & "..ITEMNONTAG AS I"
            'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON  I.ITEMID=IM.ITEMID"
            'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY AS C ON IM.CATCODE=C.CATCODE WHERE 1=1"
            'StrSql += vbCrLf + " AND I.RECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'AND'" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            'StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
            'StrSql += vbCrLf + " AND I.COSTID IN(" & selectedcostid & ") AND RECISS='I'"
            'StrSql += vbCrLf + " GROUP BY IM.CATCODE,C.CATNAME"
            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT IM.CATCODE,C.CATNAME,0 ISSPCS,0 ISSGRSWT,0 ISSNETWT"
            StrSql += vbCrLf + " ,SUM(I.PCS) RECPCS,SUM(I.GRSWT) RECGRSWT,SUM(I.NETWT) RECNETWT,0 AS RECEIPT,0 PAYMENT, 3 AS RESULT, 1 AS RESULT1,'' AVERAGE"
            StrSql += vbCrLf + " , 0 RATE, CONVERT(VARCHAR(3),'') COLHEAD FROM " & cnAdminDb & "..ITEMNONTAG AS I"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON  I.ITEMID=IM.ITEMID"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY AS C ON IM.CATCODE=C.CATCODE WHERE 1=1"
            StrSql += vbCrLf + " AND I.RECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'AND'" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
            If selectedcostid <> "''" Then StrSql += vbCrLf + " AND I.COSTID IN(" & selectedcostid & ")  "
            StrSql += vbCrLf + " AND RECISS='R' AND ISNULL(I.SUMMARY,'')<>'Y'"
            StrSql += vbCrLf + " GROUP BY IM.CATCODE,C.CATNAME"
            StrSql += vbCrLf + ")X"
            StrSql += vbCrLf + " GROUP BY CATCODE,CATNAME,RECEIPT,PAYMENT,RESULT,RESULT1,AVERAGE,RATE,COLHEAD"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "STOCKDETAIL)>0 "
            StrSql += " BEGIN "
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION, COLHEAD) VALUES('STOCK RECEIPT:','T') "
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
            StrSql += " ''COLHEAD, RESULT1 "
            StrSql += " FROM TEMP" & systemId & "STOCKDETAIL "
            If rbtCategoryWise.Checked = True Then
                StrSql += " ORDER BY CATCODE,RESULT1"
            Else
                StrSql += " ORDER BY CATNAME,RESULT1"
            End If
            StrSql += " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
            StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "STOCKDETAIL)>0 "
            StrSql += " BEGIN "
            StrSql += vbCrLf + "INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,COLHEAD,RESULT1,ORDERNO,COLHEAD1)"
            StrSql += vbCrLf + "SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0))"
            StrSql += vbCrLf + ",SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),'S' COLHEAD,1,2,'M' COLHEAD1"
            StrSql += vbCrLf + "FROM TEMP" & systemId & "STOCKDETAIL"
            StrSql += " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub
    Private Sub ProcLOTDetail()
        If chkstockdetail.Checked Then
            Dim selectedcostid As String = GetSelectedCostId(chklstCostCentre, True)
            StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "LOTDETAIL') DROP TABLE TEMP" & systemId & "LOTDETAIL"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
            StrSql = vbCrLf + " SELECT '  ' + METALNAME AS PARTICULAR,DESIGNER,METALID,METALNAME,SUM(ISSPCS)ISSPCS,SUM(ISSGRSWT)ISSGRSWT,SUM(ISSNETWT)ISSNETWT"
            StrSql += vbCrLf + " ,SUM(RECPCS)RECPCS,SUM(RECGRSWT)RECGRSWT,SUM(RECNETWT) RECNETWT,RECEIPT,PAYMENT,RESULT1,AVERAGE,RATE,COLHEAD"
            StrSql += vbCrLf + "  INTO TEMP" & systemId & "LOTDETAIL FROM("
            StrSql += vbCrLf + " SELECT D.DESIGNERNAME AS DESIGNER,IM.METALID,M.METALNAME, CONVERT(INTEGER,0) ISSPCS, CONVERT(NUMERIC(15,3),0) ISSGRSWT, CONVERT(NUMERIC(15,3),0) ISSNETWT"
            StrSql += vbCrLf + " ,SUM(I.PCS) RECPCS,SUM(I.GRSWT) RECGRSWT,SUM(I.NETWT) RECNETWT, CONVERT(NUMERIC(15,2),0) AS RECEIPT, CONVERT(NUMERIC(15,2),0) PAYMENT, 1 AS RESULT1,CONVERT(VARCHAR(20), '') AS AVERAGE"
            StrSql += vbCrLf + " , CONVERT(NUMERIC(15,2),0) RATE, CONVERT(VARCHAR(3),'') COLHEAD FROM " & cnAdminDb & "..ITEMLOT AS I"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON  I.ITEMID=IM.ITEMID"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST AS M ON IM.METALID=M.METALID"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..DESIGNER AS D ON I.DESIGNERID =D.DESIGNERID "
            StrSql += vbCrLf + " WHERE I.LOTDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'AND'" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
            If selectedcostid <> "''" Then StrSql += vbCrLf + " AND I.COSTID IN(" & selectedcostid & ")"
            StrSql += vbCrLf + " GROUP BY IM.METALID,M.METALNAME,D.DESIGNERNAME"
            StrSql += vbCrLf + ")X"
            StrSql += vbCrLf + " GROUP BY METALID,METALNAME,DESIGNER,RECEIPT,PAYMENT,RESULT1,AVERAGE,RATE,COLHEAD"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "LOTDETAIL)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "LOTDETAIL (PARTICULAR,DESIGNER,RESULT1,COLHEAD)"
            StrSql += vbCrLf + " SELECT DISTINCT DESIGNER,DESIGNER,0 RESULT1,'D' FROM TEMP" & systemId & "LOTDETAIL"
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) VALUES('LOT DETAIL:','T') "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,COLHEAD,RESULT1) "
            StrSql += vbCrLf + " SELECT PARTICULAR"
            StrSql += vbCrLf + " ,CASE WHEN ISSPCS <> 0 THEN ISSPCS ELSE NULL END ISSPCS"
            StrSql += vbCrLf + " ,CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT"
            StrSql += vbCrLf + " ,CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT"
            StrSql += vbCrLf + " ,CASE WHEN RECPCS <> 0 THEN RECPCS ELSE NULL END RECPCS"
            StrSql += vbCrLf + " ,CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT"
            StrSql += vbCrLf + " ,CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT"
            StrSql += vbCrLf + " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END RECEIPT"
            StrSql += vbCrLf + " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END PAYMENT"
            StrSql += vbCrLf + " ,COLHEAD,RESULT1 "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "LOTDETAIL"
            StrSql += vbCrLf + " ORDER BY DESIGNER,RESULT1,METALNAME"
            StrSql += " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
            StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "STOCKDETAIL)>0 "
            StrSql += " BEGIN "
            StrSql += vbCrLf + "INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,COLHEAD,RESULT1,ORDERNO,COLHEAD1)"
            StrSql += vbCrLf + "SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0))"
            StrSql += vbCrLf + ",SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),'S' COLHEAD,1,2,'M' COLHEAD1"
            StrSql += vbCrLf + "FROM TEMP" & systemId & "LOTDETAIL"
            StrSql += " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub
    Private Sub ProcWsSalesamt()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSWSSALE') DROP TABLE TEMP" & systemId & "ABSWSSALE "

        StrSql += " SELECT  'Ws Amount' Catname,"
        StrSql += " sum(amount) AMT,1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSWSSALE  "
        StrSql += " FROM " & cnStockDb & "..ACCTRAN I "
        StrSql += " WHERE I.TRANMODE = 'D' AND I.TRANFLAG = 'CA' AND FROMFLAG = 'P' AND I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND ISNULL(I.CANCEL,'') = ''   "
        StrSql += " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND ACCODE IN (SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH')"
        StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += StrFilter
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        StrSql += " having SUM(amount) > 0 "
        StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSWSSALE)>0 "
        StrSql += " BEGIN "
        'StrSql += " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += " SELECT 'WS BILL ' + CATNAME CATNAME,  "
        StrSql += " CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT,  "
        StrSql += " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT, "
        StrSql += " 1 RESULT1, COLHEAD  "
        StrSql += " FROM TEMP" & systemId & "ABSWSSALE ORDER BY CATNAME  "
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcPartlySales()
        If (RPT_ME_SUM_DABS = True And rbtMetalwise.Checked = True) Then Exit Sub
        If rbtmetalwisegrp.Checked = False Then
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
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
            StrSql += " ) X "
            StrSql += " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
            StrSql += " ) GROUP BY CATNAME"
            StrSql += " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
            If RPT_SEPVAT_DABS Then
                StrSql += " UNION ALL"
                StrSql += " SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
                StrSql += " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
                StrSql += " FROM ("
                If chkCatShortname.Checked = True Then
                    StrSql += " SELECT (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
                Else
                    StrSql += " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
                End If
                StrSql += " WHERE CATCODE = ISS.CATCODE)AS CATNAME   "
                StrSql += " ,ISS.STNPCS AS ISSPCS, ISS.STNWT AS ISSGRSWT, ISS.STNWT AS ISSNETWT "
                StrSql += " ,STNPCS "
                StrSql += " ,TAGSTNPCS "
                StrSql += " ,STNWT "
                StrSql += " ,TAGSTNWT "
                StrSql += " ,TAGSTNPCS AS TAGPCS,TAGSTNWT AS TAGGRSWT,TAGSTNWT AS TAGNETWT FROM " & cnStockDb & "..ISSSTONE ISS "
                StrSql += " WHERE ISS.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += " AND ISSSNO IN(SELECT SNO FROM  " & cnStockDb & "..ISSUE I  "
                StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
                StrSql += " AND I.TAGNO <> ''"
                StrSql += " AND ISNULL(I.CANCEL,'') = ''   "
                StrSql += " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
                StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
                StrSql += StrFilter
                StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
                StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
                StrSql += " ) ) X "
                StrSql += " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
                StrSql += " ) GROUP BY CATNAME"
                StrSql += " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
            End If
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPARTLYSALE') DROP TABLE TEMP" & systemId & "ABSPARTLYSALE "
            StrSql += " SELECT METALID,CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
            StrSql += " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
            StrSql += " INTO TEMP" & systemId & "ABSPARTLYSALE FROM ("
            If chkCatShortname.Checked = True Then
                StrSql += " SELECT METALID,(SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
            Else
                StrSql += " SELECT METALID,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
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
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += " ) X "
            StrSql += " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
            StrSql += " ) GROUP BY CATNAME,METALID"
            StrSql += " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
            If RPT_SEPVAT_DABS Then
                StrSql += " UNION ALL"
                StrSql += " SELECT METALID,CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
                StrSql += " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
                StrSql += " FROM ("
                If chkCatShortname.Checked = True Then
                    StrSql += " SELECT IM.METALID,(SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
                Else
                    StrSql += " SELECT IM.METALID,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
                End If
                StrSql += " WHERE CATCODE = ISS.CATCODE)AS CATNAME   "
                StrSql += " ,ISS.STNPCS AS ISSPCS, ISS.STNWT AS ISSGRSWT, ISS.STNWT AS ISSNETWT "
                StrSql += " ,STNPCS "
                StrSql += " ,TAGSTNPCS "
                StrSql += " ,STNWT "
                StrSql += " ,TAGSTNWT "
                StrSql += " ,TAGSTNPCS AS TAGPCS,TAGSTNWT AS TAGGRSWT,TAGSTNWT AS TAGNETWT FROM " & cnStockDb & "..ISSSTONE ISS "
                StrSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID=ISS.STNITEMID"
                StrSql += " WHERE ISS.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += " AND ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..ISSUE I "
                StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                StrSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
                StrSql += " AND I.TAGNO <> ''"
                StrSql += " AND ISNULL(I.CANCEL,'') = ''   "
                StrSql += " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
                StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
                StrSql += StrFilter
                StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
                StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
                StrSql += " ) ) X "
                StrSql += " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
                StrSql += " ) GROUP BY CATNAME,METALID"
                StrSql += " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
            End If
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

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
        If rbtmetalwisegrp.Checked = False Then
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
        Else
            StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPARTLYSALE)>0 "
            StrSql += " BEGIN "
            'StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            'StrSql += " (DESCRIPTION, COLHEAD) VALUES('PARTLY SALES','T') "
            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,RESULT1,CATCODE,COLHEAD,ORDERNO,COLHEAD1) "
            StrSql += " SELECT 'PARTLY SALES'  CATNAME, "
            StrSql += " 1 RESULT1,METALID,'L' COLHEAD,6,'C' COLHEAD1  "
            StrSql += " FROM TEMP" & systemId & "ABSPARTLYSALE GROUP BY METALID,COLHEAD ORDER BY RESULT1 "

            StrSql += " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,CATCODE,COLHEAD,ORDERNO) "
            StrSql += " SELECT CATNAME, "
            StrSql += " CASE WHEN SUM(ISNULL(ISSPCS,0))  <> 0 THEN SUM(ISNULL(ISSPCS,0)) ELSE NULL END ISSPCS, "
            StrSql += " CASE WHEN SUM(ISNULL(ISSGRSWT,0))  <> 0 THEN SUM(ISNULL(ISSGRSWT,0))  ELSE NULL END ISSGRSWT,  "
            StrSql += " CASE WHEN SUM(ISNULL(ISSNETWT,0))  <> 0 THEN SUM(ISNULL(ISSNETWT,0))  ELSE NULL END ISSNETWT, "
            StrSql += " 1 RESULT1,METALID,'L' COLHEAD,7  "
            StrSql += " FROM TEMP" & systemId & "ABSPARTLYSALE GROUP BY METALID,COLHEAD,CATNAME ORDER BY RESULT1 "
            StrSql += " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPARTLYSALE)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
            StrSql += vbCrLf + " '' AVERAGE,METALID,'L' COLHEAD,1,8,'M' COLHEAD1 "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSPARTLYSALE GROUP BY METALID,COLHEAD,RESULT1"
            StrSql += vbCrLf + " ORDER BY RESULT1"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub ProcCollection()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "COLDET') DROP TABLE TEMP" & systemId & "COLDET "
        StrSql += " DECLARE @CASHID VARCHAR(7)"
        StrSql += " SELECT @CASHID = CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'"
        StrSql += vbCrLf + " SELECT CONVERT(VARCHAR(50),(CASE "
        StrSql += vbCrLf + " WHEN PAYMODE='CA' THEN 'CASH [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]'"
        StrSql += vbCrLf + " WHEN PAYMODE = 'CC' THEN 'CREDIT CARD [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        StrSql += vbCrLf + " WHEN PAYMODE = 'CH' THEN 'CHEQUE [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        StrSql += vbCrLf + " WHEN PAYMODE = 'SS' THEN 'SCHEME [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        StrSql += vbCrLf + " WHEN PAYMODE = 'GV' THEN 'GIFT VOUCHER [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        StrSql += vbCrLf + " WHEN PAYMODE = 'ET' THEN 'ETRANSFER [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        StrSql += vbCrLf + " WHEN PAYMODE = 'OT' THEN 'OTHERS [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        StrSql += vbCrLf + " END)) AS CATNAME,"
        StrSql += vbCrLf + " (CASE WHEN SUM(AMOUNT)> 0 THEN  SUM(AMOUNT) ELSE 0 END) AS PAYMENT, "
        StrSql += vbCrLf + " (CASE WHEN SUM(AMOUNT)< 0 THEN  ABS(SUM(AMOUNT)) ELSE 0 END) AS RECEIPT "
        StrSql += vbCrLf + " INTO TEMP" & systemId & "COLDET FROM ("

        StrSql += vbCrLf + " SELECT PAYMODE,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END)) AS AMOUNT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not hasChit Then StrSql += " AND FROMFLAG <> 'C'"
        'StrSql += "  AND GRSWT = 0"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND PAYMODE IN ('CC','CH')"
        StrSql += vbCrLf + " GROUP BY TRANMODE,PAYMODE"
        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT PAYMODE,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END)) AS AMOUNT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not hasChit Then StrSql += " AND FROMFLAG <> 'C'"
        'StrSql += "  AND GRSWT = 0"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND PAYMODE IN ('GV') AND TRANMODE = 'D'"
        StrSql += vbCrLf + " GROUP BY TRANMODE,PAYMODE"
        StrSql += vbCrLf + " UNION ALL "

        '602
        If chkwithcreditcommision.Checked Then
            StrSql += vbCrLf + " SELECT 'CC' PAYMODE,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END)) AS AMOUNT"
            StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
            StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A"
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
            StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
            If Not hasChit Then StrSql += " AND FROMFLAG <> 'C'"
            StrSql += StrFilter
            StrSql += StrUseridFtr
            StrSql += vbCrLf + " AND PAYMODE IN ('BC') AND ACCODE='BANKC' AND CONTRA IN(SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD)"
            StrSql += vbCrLf + " GROUP BY TRANMODE,PAYMODE"
            StrSql += vbCrLf + " UNION ALL "
        End If
        '602
        StrSql += vbCrLf + " SELECT 'SS' PAYMODE,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END)) AS AMOUNT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND PAYMODE IN ('SS','CB','CZ','CG','CD')"
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not hasChit Then StrSql += " AND FROMFLAG <> 'C'"
        StrSql += vbCrLf + " GROUP BY TRANMODE"

        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT 'CA' PAYMODE,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END)) AS AMOUNT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND PAYMODE IN ('CA') AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE = '1')"
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not hasChit Then StrSql += " AND FROMFLAG <> 'C'"
        If chkCashOpening.Checked Then
            StrSql += vbCrLf + " AND ACCODE = @CASHID"
        End If
        StrSql += vbCrLf + " GROUP BY TRANMODE"
        If chkCashOpening.Checked Then
            StrSql += " UNION ALL"
            StrSql += " SELECT 'CA' PAYMODE,-1*SUM(AMOUNT) AMOUNT,-1*SUM(AMOUNT) RECEIPT,0 PAYMENT FROM TEMPCASHOPEN"
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
        StrSql += vbCrLf + " )X "
        StrSql += vbCrLf + " GROUP BY PAYMODE "
        StrSql += vbCrLf + " HAVING(SUM(AMOUNT) <> 0)"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "COLDET)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION,COLHEAD) "
        StrSql += vbCrLf + " VALUES('COLLECTION DETAILS','T') "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT) "
        StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
        StrSql += vbCrLf + " CASE WHEN RECEIPT<> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT<> 0 THEN PAYMENT  ELSE NULL END PAYMENT "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "COLDET ORDER BY CATNAME "
        StrSql += vbCrLf + " END"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcAmtSubtot()
        StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "COLDET)>0 "
        StrSql += " BEGIN "
        StrSql += " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCOLDETTOT') DROP TABLE TEMP" & systemId & "ABSCOLDETTOT "
        StrSql += " SELECT   SUM(RECEIPT) AS RECEIPT, SUM(PAYMENT) AS PAYMENT INTO TEMP" & systemId
        StrSql += "ABSCOLDETTOT FROM TEMP" & systemId & "COLDET END"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
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
        StrSql += StrUseridFtr
        StrSql += " GROUP BY TRANNO, TRANDATE, TRANTYPE"
        StrSql += " UNION ALL"
        StrSql += " SELECT TRANNO, TRANDATE, TRANTYPE, 0 IPCS, 0 IGRSWT, 0 INETWT,  "
        StrSql += " SUM(PCS) RPCS, SUM(GRSWT) RGRSWT, SUM(NETWT) RNETWT, "
        StrSql += " 0 RECEIPT, SUM(AMOUNT) PAYMENT FROM " & cnStockDb & "..RECEIPT  "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND ISNULL(CANCEL,'') = 'Y'   AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += " GROUP BY TRANNO, TRANDATE, TRANTYPE"
        StrSql += " ) X "
        StrSql += " WHERE TRANTYPE IN ('SA','SR','PU','AD','OD','RD','AI')"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

    End Sub
    Private Sub ProcMaterialIssue()
        If ChkMatIssue.Checked = False Then Exit Sub
        StrSql = vbCrLf + "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSMATERIALISSUE') DROP TABLE TEMP" & systemId & "ABSMATERIALISSUE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " SELECT CATCODE,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE =I.CATCODE)CATNAME,"
        StrSql += vbCrLf + " SUM(PCS)ISSPCS,SUM(GRSWT)ISSGRSWT,SUM(NETWT)ISSNETWT,0 AS RECPCS,0 AS RECGRSWT,"
        StrSql += vbCrLf + " 0 AS RECNETWT,0 AS RECEIPT,0 AS PAYMENT,0 AS AVERAGE,RATE,''AS COLHEAD,1 AS RESULT,1 AS RESULT1"
        StrSql += vbCrLf + " INTO TEMP" & systemId & "ABSMATERIALISSUE "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + "  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + StrFilter
        StrSql += vbCrLf + StrUseridFtr
        StrSql += vbCrLf + "GROUP BY CATCODE,RATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSMATERIALISSUE)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "ABSMATERIALISSUE(CATNAME, "
        StrSql += " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += " AVERAGE,RESULT, RESULT1, COLHEAD) "
        StrSql += " SELECT 'MATERIAL ISSUE'CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, 0 AVERAGE,0 RESULT, 0 AS RESULT1, 'T' COLHEAD "
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSMATERIALISSUE)>0 "
        StrSql += " BEGIN "
        StrSql += " INSERT INTO TEMP" & systemId & "ABSMATERIALISSUE(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,AVERAGE,RESULT, RESULT1, COLHEAD) "
        StrSql += " SELECT 'Z' AS CATCODE, 'MATERIAL ISSUE TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += " SUM(PAYMENT),SUM(AVERAGE),4 RESULT, 2 AS RESULT1, 'S' COLHEAD "
        StrSql += " FROM TEMP" & systemId & "ABSMATERIALISSUE WHERE RESULT = 1 "
        StrSql += " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSMATERIALISSUE)>0"
        StrSql += " BEGIN "
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
        StrSql += " CASE WHEN AVERAGE  <> 0 THEN AVERAGE  ELSE NULL END AVERAGE, "
        StrSql += " COLHEAD, RESULT1  "
        StrSql += " FROM TEMP" & systemId & "ABSMATERIALISSUE "
        StrSql += " ORDER BY CATCODE, RESULT"
        StrSql += " END "
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
                    Case "T"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "D"
                        .DefaultCellStyle.ForeColor = Color.Blue
                    Case "P"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "A"
                        .Cells("COL1").Style.BackColor = Color.LightYellow
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "M"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "V"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .Cells("COL1").Style.BackColor = Color.LightYellow
                    Case "Z"
                        '.Cells("COL1").Style.BackColor = Color.LightYellow
                        '.Cells("COL2").Style.BackColor = Color.LightYellow
                        '.Cells("COL3").Style.BackColor = Color.LightYellow
                        '.Cells("COL4").Style.BackColor = Color.LightYellow
                        '.Cells("COL5").Style.BackColor = Color.LightYellow
                        '.Cells("COL6").Style.BackColor = Color.LightYellow
                        '.Cells("COL7").Style.BackColor = Color.LightYellow
                        '.Cells("COL8").Style.BackColor = Color.LightYellow
                        '.Cells("COL9").Style.BackColor = Color.LightYellow
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        '.Cells("COL1").Style.BackColor = Color.LightYellow
                    Case "V"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .Cells("COL1").Style.BackColor = Color.LightYellow
                End Select
                Select Case .Cells("COLHEAD1").Value.ToString
                    Case "M"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "C"
                        .Cells("COL1").Style.BackColor = Color.LightYellow
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
                Select Case .Cells("COL1").Value.ToString
                    Case "RATE TOTAL"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "WEIGHT TOTAL"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "SALES"
                        .Cells("COL1").Style.BackColor = Color.LightYellow
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "SALES RETURN"
                        .Cells("COL1").Style.BackColor = Color.LightYellow
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "ORDER/REPAIR"
                        .Cells("COL1").Style.BackColor = Color.LightBlue
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "PARTLY SALES"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .Cells("COL1").Style.BackColor = Color.LightYellow
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
            If .Columns.Contains("DISCPER") Then GridViewHead.Columns("COL11").Width = .Columns("DISCPER").Width
            If .Columns.Contains("DISC%") Then GridViewHead.Columns("COL11").HeaderText = "DISC%"
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
            If .Columns.Contains("COL1") Then GridViewHead.Columns("COL1").Width = .Columns("COL1").Width : GridViewHead.Columns("COL1").HeaderText = ""
            Dim TEMPCOLWIDTH As Integer = 0
            If chkPcs.Checked = True And .Columns.Contains("COL2") Then TEMPCOLWIDTH += .Columns("COL2").Width
            If chkGrswt.Checked = True And .Columns.Contains("COL3") Then TEMPCOLWIDTH += .Columns("COL3").Width
            If chkNetwt.Checked = True And .Columns.Contains("COL4") Then TEMPCOLWIDTH += .Columns("COL4").Width
            GridViewHead.Columns("COL2~COL3~COL4").Width = TEMPCOLWIDTH
            GridViewHead.Columns("COL2~COL3~COL4").HeaderText = "ISSUE"
            TEMPCOLWIDTH = 0
            If chkPcs.Checked = True And .Columns.Contains("COL5") Then TEMPCOLWIDTH += .Columns("COL5").Width
            If chkGrswt.Checked = True And .Columns.Contains("COL6") Then TEMPCOLWIDTH += .Columns("COL6").Width
            If chkNetwt.Checked = True And .Columns.Contains("COL7") Then TEMPCOLWIDTH += .Columns("COL7").Width
            GridViewHead.Columns("COL5~COL6~COL7").Width = TEMPCOLWIDTH
            GridViewHead.Columns("COL5~COL6~COL7").HeaderText = "RECEIPT"
            If .Columns.Contains("COL8") And .Columns.Contains("COL9") Then GridViewHead.Columns("COL8~COL9").Width = .Columns("COL8").Width + .Columns("COL9").Width
            GridViewHead.Columns("COL8~COL9").HeaderText = "AMOUNT"
            If .Columns.Contains("COL10") Then GridViewHead.Columns("COL10").Width = .Columns("COL10").Width
            GridViewHead.Columns("COL10").HeaderText = "AVERAGE"
            If .Columns.Contains("COL11") Then GridViewHead.Columns("COL11").Width = .Columns("COL11").Width
            GridViewHead.Columns("COL11").HeaderText = "DISC%"
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
            If .Columns.Contains("AVGRATE") Then
                .Columns("AVGRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
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

        chkChitInfo.Checked = IIf(GetAdmindbSoftValue("CHITDB", "N") = "Y", True, False)
        chkCashOpening.Checked = _CashOpening
        If RPT_HIDE_ADVSUMMARY Then chkAdvdueSummary.Visible = False : chkAdvdueSummary.Checked = False
        If RPT_HIDE_LOTDETAILS = True Then chkstockdetail.Enabled = False : chkstockdetail.Checked = False
        If RPT_CHKDIS_ROLEEDIT = True Then If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit, False) Then checkeddisable()
        dtpFrom.Select()
    End Function

    Private Sub checkeddisable()
        Panel1.Enabled = False
    End Sub
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

    Private Sub btnDotMatrix_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDotMatrix.Click
        hasChit = IIf(GetAdmindbSoftValue("CHITDB", "N") = "Y", True, False) And chkChitInfo.Checked
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("COL1", GetType(String))
            .Columns.Add("COL2~COL3~COL4", GetType(String))
            .Columns.Add("COL5~COL6~COL7", GetType(String))
            .Columns.Add("COL8~COL9", GetType(String))
            .Columns.Add("COL10", GetType(String))
            .Columns.Add("COL11", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("COL1").Caption = ""
            .Columns("COL2~COL3~COL4").Caption = "ISSUE"
            .Columns("COL5~COL6~COL7").Caption = "RECEIPT"
            .Columns("COL8~COL9").Caption = "AMOUNT"
            .Columns("COL10").Caption = "AVERAGE"
            .Columns("COL11").Caption = "DISC%"
            .Columns("SCROLL").Caption = ""
        End With
        SelectedCompanyId = GetSelectedCompanyId(chkLstCompany, True)
        GridViewHead.DataSource = dtMergeHeader
        Filteration()
        CostIdFiltration()
        Report1(True)
    End Sub

    Private Sub rbtmetalwisegrp_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtmetalwisegrp.CheckedChanged
        If rbtmetalwisegrp.Checked = True Then
            rbtMetalwise.Checked = False
            rbtCategoryWise.Checked = False
        End If
    End Sub

    Private Sub rbtCategoryWise_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtCategoryWise.CheckedChanged
        If rbtCategoryWise.Checked Then
            ChkMetalDisc.Enabled = False
            ChkMetalDisc.Checked = False
        Else
            ChkMetalDisc.Enabled = True
        End If
    End Sub
    Public Function SendMailRpt()
        funcNew()
        btnView_Search_Click(Me, New EventArgs)
        Dim strHtml As String = ""
        If gridView.Rows.Count > 0 Then
            strHtml = ConvertDataGridViewToHTML(gridView, GridViewHead)
            Return strHtml
        End If
    End Function
End Class





