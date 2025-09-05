Imports System.Data.OleDb
'CAL ID-602: CLIENT NAME- PRINCT: CORRECTION- CREDIT CARD COMMISSION SHOULD BE COME WITH COLLECTION : ALTER BY SATHYA
Public Class frmDailyAbstract
    Dim Cmd As New OleDbCommand
    Dim DT As New DataTable
    Dim StrSql As String
    Dim StrFilter As String
    Dim StrCostFiltration As String
    Dim StrCostFiltration1 As String
    Dim StrCashCounterFtr As String
    Dim StrUseridFtr As String
    Dim dsReportCol As New DataSet
    Dim SelectedCompanyId As String
    Dim hasChit As Boolean = False
    Dim RPT_SEPVAT_DABS As Boolean = IIf(GetAdmindbSoftValue("RPT_SEPVAT_DABS", "Y") = "Y", True, False)
    Dim RPT_SEPVAT_Cess As Boolean = IIf(GetAdmindbSoftValue("ISCESS", "Y") = "Y", True, False)
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
    Dim RPT_SEPSTUDSTN_DABS As Boolean = IIf(GetAdmindbSoftValue("RPT_SEPSTUDSTN_DABS", "N") = "Y", True, False)
    Dim RPT_DISC_BRKUP_DABS As Boolean = IIf(GetAdmindbSoftValue("RPT_DISC_BRKUP_DABS", "N") = "Y", True, False)
    Dim RPT_DABS_SEP_SRTYPE As Boolean = IIf(GetAdmindbSoftValue("RPT_DABS_SEP_SRTYPE", "N") = "Y", True, False)
    Dim RPT_DABS_ADVDATE As Boolean = IIf(GetAdmindbSoftValue("RPT_DABS_ADVDATE", "N") = "Y", True, False)
    Dim HOMECOLLECTINBASE As Boolean = IIf(GetAdmindbSoftValue("HOMECOLLECTINBASE", "N") = "Y", True, False)
    Dim RPT_REC_PAY As Boolean = IIf(GetAdmindbSoftValue("RPT_REC_PAY", "N") = "Y", True, False)
    Dim MULTICOSTID As Boolean = IIf(GetAdmindbSoftValue("MULTICOSTID", "N") = "Y", True, False)
    Dim SCHEME_TO_ADVANCE_NAME_CHANGE As String = GetAdmindbSoftValue("SCHEME_MASKING_NAME", "")
    Dim SCHEME_TO_ADVANCE_NAME_CHANGE_CASH As String = GetAdmindbSoftValue("SCHEME_MASKING_NAME_CASH", "")
    Dim RPT_GSTREGISTER_VIEW As Boolean = IIf(GetAdmindbSoftValue("DAILYABS_GSTREG_VIEW", "N") = "Y", True, False)
    Dim RPT_SHOW_SCHEMEWISEDETAILS As Boolean = IIf(GetAdmindbSoftValue("RPT_DABS_SHOW_SCHEMEWISE_WTDETAILS", "N") = "Y", True, False)
    Dim FREEINS_BOUNS As Boolean = IIf(GetAdmindbSoftValue("RPT_DABS_SHOW_BOUNS_FREEINS", "N") = "Y", True, False)
    Dim OnlineAccode As String = GetAdmindbSoftValue("RPT_BILLWISERECPAY_ONLACCODES", "",, True)
    Dim GstFlag As Boolean
    Dim RPT_GSTREGISTER_CASH_VIEW As Boolean = IIf(GetAdmindbSoftValue("DAILYABS_GSTREG_CASH_VIEW", "N") = "Y", True, False)
    Dim RPT_HIDE_REPAIRWTDET As Boolean = IIf(GetAdmindbSoftValue("RPT_DABS_HIDE_REPAIR_WTDET", "N") = "Y", True, False)
    Dim RPT_SEP_REPAIRTAXDET As Boolean = IIf(GetAdmindbSoftValue("RPT_DABS_SEP_REPAIR_TAXDET", "N") = "Y", True, False)


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
        If MULTICOSTID = True Then
            StrSql = "select usercostid  FROM " & cnAdminDb & "..usermaster where userid in(' " & userId & " ') "
            Dim dtt As New DataTable
            dtt = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dtt)
            Dim cnnt As Integer
            'chklstCostCentre.Items.Add("ALL", IIf(cnDefaultCostId, True, False))
            If dtt.Rows.Count > 0 Then
                StrSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE where COSTID in (" & GetQryString(dtt.Rows(cnnt).Item(0).ToString) & ")"
            End If
            Dim dtcostcentre As New DataTable
            dtcostcentre = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dtcostcentre)
            For cnnt = 0 To dtcostcentre.Rows.Count - 1
                'If cnCostName = DT.Rows(cnt).Item(0).ToString And cnDefaultCostId = False Then
                'If chklstCostCentre.Items.Contains(dtcostcentre.Rows(cnnt).Item(0).ToString) = dtcostcentre.Rows(cnnt).Item(0).ToString Then
                chklstCostCentre.Items.Remove(dtcostcentre.Rows(cnnt).Item(0).ToString)
                chklstCostCentre.Items.Add(dtcostcentre.Rows(cnnt).Item(0).ToString, True)
                ' End If

                'Else
                'chklstCostCentre.Items.Add(DT.Rows(cnt).Item(0).ToString, False)
                'End If
            Next
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chklstCostCentre.Enabled = False

            'Else
            '  chklstCostCentre.Items.Clear()
            ' chklstCostCentre.Enabled = False
        End If
        'End If
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
        StrSql += vbCrLf + " CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..USERCASH WHERE USERID='" & userId & "' )"
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
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            'Dim title As String
            'title = "DAILY ABSTRACT REPORT"
            'title += vbCrLf + Trim(lbltitle.Text)
            Dim rptusername As String = GetSqlValue(cn, "SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID ='" & userId & "'")
            Dim MobileNo As String = GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='DAILYABSOTP'").ToString
            Dim Optionid As String = GetSqlValue(cn, "SELECT COLID FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='DAILYABSOTP'").ToString
            If MobileNo <> "" Then
                Dim Msg As String
                Msg = " Dear Sir <OTP> is the OTP for Daily Abstract Report generated by User: " & rptusername.ToString & ". "
                Msg += " Pls Share the OTP to Print the Report."
                funcGenPwd(Optionid, MobileNo, Msg, True)
                Dim pwdid As Integer = GetuserPwd(Optionid, cnCostId, userId)
                If pwdid <> 0 Then
                    Dim objUpwd As New frmRptUserPassword(pwdid, Optionid, False, MobileNo, Optionid, Msg)
                    If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub
                    BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle.Text, gridView, BrightPosting.GExport.GExportType.Print, GridViewHead)
                Else
                    MsgBox("OTP Not Generated...", MsgBoxStyle.Information)
                    Exit Sub
                End If
            Else
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle.Text, gridView, BrightPosting.GExport.GExportType.Print, GridViewHead)
            End If
            'BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle.Text, gridView, BrightPosting.GExport.GExportType.Print, GridViewHead)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub BtnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            Dim rptusername As String = GetSqlValue(cn, "SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID ='" & userId & "'")
            Dim MobileNo As String = GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='DAILYABSOTP'").ToString
            Dim Optionid As String = GetSqlValue(cn, "SELECT COLID FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='DAILYABSOTP'").ToString
            If MobileNo <> "" Then
                Dim Msg As String
                Msg = " Dear Sir <OTP> is the OTP for Daily Abstract Report generated by User: " & rptusername.ToString & ". "
                Msg += " Pls Share the OTP to Excel Export this Report."
                funcGenPwd(Optionid, MobileNo, Msg, True)
                Dim pwdid As Integer = GetuserPwd(Optionid, cnCostId, userId)
                If pwdid <> 0 Then
                    Dim objUpwd As New frmRptUserPassword(pwdid, Optionid, False, MobileNo, Optionid, Msg)
                    If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub
                    BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle.Text, gridView, BrightPosting.GExport.GExportType.Export, GridViewHead)
                Else
                    MsgBox("OTP Not Generated...", MsgBoxStyle.Information)
                    Exit Sub
                End If
            Else
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle.Text, gridView, BrightPosting.GExport.GExportType.Export, GridViewHead)
            End If
            'BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle.Text, gridView, BrightPosting.GExport.GExportType.Export, GridViewHead)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        GstFlag = funcGstView(dtpFrom.Value)
        hasChit = IIf(GetAdmindbSoftValue("CHITDB", "N") = "Y", True, False) And chkChitInfo.Checked
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
        If SelectedCompanyId = "" Then
            SelectedCompanyId = "'" & strCompanyId & "'"
        End If
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
        tempchkitem = Nothing
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
        StrCostFiltration1 = ""
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
                StrCostFiltration1 = tempchkitem
            Else
                StrCostFiltration = ""
                StrCostFiltration1 = ""
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
            StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = "
            StrSql += vbCrLf + " 'TEMP" & systemId & "SALEABSTRACT') "
            StrSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT "
            StrSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT( "
            StrSql += vbCrLf + " COL1 VARCHAR(100),"
            StrSql += vbCrLf + " COL2 VARCHAR(100),"
            StrSql += vbCrLf + " COL3 VARCHAR(100),"
            StrSql += vbCrLf + " COL4 VARCHAR(100),"
            StrSql += vbCrLf + " COL5 VARCHAR(100),"
            StrSql += vbCrLf + " COL6 VARCHAR(100),"
            StrSql += vbCrLf + " COL7 VARCHAR(100),"
            StrSql += vbCrLf + " COL8 VARCHAR(100),"
            StrSql += vbCrLf + " COL9 VARCHAR(100),"
            StrSql += vbCrLf + " COL10 VARCHAR(100),"
            StrSql += vbCrLf + " COL11 VARCHAR(100),"
            StrSql += vbCrLf + " COLHEAD VARCHAR(1),"
            StrSql += vbCrLf + " CATCODE VARCHAR(5),"
            StrSql += vbCrLf + " ORDERNO INT,"
            StrSql += vbCrLf + " COLHEAD1 VARCHAR(1),"
            StrSql += vbCrLf + " SNO INT IDENTITY(1,1))"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = "
            StrSql += vbCrLf + " 'TEMP" & systemId & "SASRPU') "
            StrSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "SASRPU "
            StrSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "SASRPU( "
            StrSql += vbCrLf + " DESCRIPTION VARCHAR(100),"
            StrSql += vbCrLf + " ISSPCS INT,"
            StrSql += vbCrLf + " ISSGRSWT NUMERIC(15,3),"
            StrSql += vbCrLf + " ISSNETWT NUMERIC(15,3),"
            StrSql += vbCrLf + " RECPCS INT,"
            StrSql += vbCrLf + " RECGRSWT NUMERIC(15,3),"
            StrSql += vbCrLf + " RECNETWT NUMERIC(15,3),"
            StrSql += vbCrLf + " RECEIPT NUMERIC(20,2),"
            StrSql += vbCrLf + " PAYMENT NUMERIC(20,2),"
            StrSql += vbCrLf + " AVERAGE VARCHAR(50),"
            StrSql += vbCrLf + " DISCPER NUMERIC(20,2),"
            StrSql += vbCrLf + " COLHEAD VARCHAR(1),"
            StrSql += vbCrLf + " RESULT1 INT,"
            StrSql += vbCrLf + " CATCODE VARCHAR(5),"
            StrSql += vbCrLf + " ORDERNO INT,"
            StrSql += vbCrLf + " COLHEAD1 VARCHAR(1),"
            StrSql += vbCrLf + " SNO INT IDENTITY(1,1))"
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
                StrSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT SET COL2=NULL WHERE COL2='0'"
                StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT SET COL3=NULL WHERE COL3='0.000'"
                StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT SET COL4=NULL WHERE COL4='0.000'"
                StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT SET COL5=NULL WHERE COL5='0'"
                StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT SET COL6=NULL WHERE COL6='0.000'"
                StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT SET COL7=NULL WHERE COL7='0.000'"
                StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT SET COL8=NULL WHERE COL8='0.00'"
                StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT SET COL9=NULL WHERE COL9='0.00'"
                StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT SET COL10=NULL WHERE COL10='0'"
                StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT SET COL11=NULL WHERE COL11='0'"
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()
            End If

            StrSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT SET COLHEAD = 'X' WHERE COL1 = '  OLD GOLD'"
            StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT SET COLHEAD = 'X' WHERE COL1 LIKE '  CASH %'"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "SELECT COL1,COL2,COL3,COL4,COL5, "
            StrSql += vbCrLf + " COL6,COL7,COL8,COL9,COL10,COL11, "
            StrSql += vbCrLf + " COLHEAD,COLHEAD1 FROM TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT ORDER BY SNO "
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
            If chkWithHeaderTotal.Checked Then
                For cnt As Integer = 0 To gridView.Rows.Count - 1
                    If gridView.Rows(cnt).Cells("COLHEAD").Value.ToString = "T" Then
                        gridView.Rows(cnt).Cells("COL8").Style.ForeColor = Color.Red
                        gridView.Rows(cnt).Cells("COL9").Style.ForeColor = Color.Red
                    End If
                Next
            End If
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
        '' REPAIR DELIVERY SEP TAX 30JULY2022
        If RPT_SEP_REPAIRTAXDET Then
            StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND "
            StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSTAXTRAN_REP') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN_REP"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = "SELECT *,CONVERT(VARCHAR(7),NULL)AS CATCODE,CONVERT(VARCHAR(1),NULL)AS METALID,CONVERT(VARCHAR(10),NULL)AS SYSTEMID INTO TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN_REP "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN "
            StrSql += vbCrLf + " WHERE ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN('RD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += vbCrLf + " AND ISNULL(I.TAX,0)<>0"
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " )"
            StrSql += vbCrLf + " AND TRANTYPE IN('RD') "
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = "UPDATE T SET CATCODE=I.CATCODE ,METALID=I.METALID FROM TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN_REP T, " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.SNO=T.ISSSNO AND I.BATCHNO=T.BATCHNO AND I.TRANTYPE=T.TRANTYPE "
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = "UPDATE T SET SYSTEMID=I.SYSTEMID FROM TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN_REP T, " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.BATCHNO=T.BATCHNO AND I.TRANTYPE=T.TRANTYPE "
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
        End If
        ''END REPAIR DELIVERY SEP TAX 30JULY2022


        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSREPAIR') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT CATCODE, "
        Else
            StrSql += vbCrLf + " SELECT METALID AS CATCODE, "
        End If
        StrSql += vbCrLf + " CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + " PAYMENT, AVERAGE, RATE, COLHEAD, RESULT, RESULT1 INTO TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR FROM ("
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
        ''StrSql += vbCrLf + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        If RPT_HIDE_REPAIRWTDET Then
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
        Else
            StrSql += vbCrLf + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        End If
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

        ''REPAIR DELIVERY SEP TAX 30JULY2022
        If RPT_SEPVAT_DABS And RPT_SEP_REPAIRTAXDET Then
            ''TAX
            StrSql += vbCrLf + " UNION ALL "
            If GstFlag Then
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_IGSTID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_IGSTID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " SUM(TAXAMOUNT) AS RECEIPT"
                StrSql += vbCrLf + " , 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN_REP I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='IG'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_IGSTID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_IGSTID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.S_IGSTID"
                End If
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_CGSTID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_CGSTID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " SUM(TAXAMOUNT) AS RECEIPT"
                StrSql += vbCrLf + " , 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN_REP I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='CG'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_CGSTID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_CGSTID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.S_CGSTID"
                End If
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_SGSTID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_SGSTID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " SUM(TAXAMOUNT) AS RECEIPT"
                StrSql += vbCrLf + " , 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN_REP I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='SG'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_SGSTID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_SGSTID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.S_SGSTID"
                End If
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.CESSID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.CESSID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " SUM(TAXAMOUNT) AS RECEIPT"
                StrSql += vbCrLf + " , 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN_REP I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='CS'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.CESSID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.CESSID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.CESSID"
                End If
            End If
        End If
        ''END REPAIR DELIVERY SEP TAX 30JULY2022
        StrSql += vbCrLf + " ) X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If rbtmetalwisegrp.Checked = False Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR(CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
            StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD) "
            StrSql += vbCrLf + " SELECT 'REPAIR DEL ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
            StrSql += vbCrLf + " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, 0 RESULT, 0 RESULT1, 'T' COLHEAD "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR"
            StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR(CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
            StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
            StrSql += vbCrLf + " SELECT 'SALES' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT,SUM(ISNULL(PAYMENT,0))PAYMENT, 1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR GROUP BY CATCODE"
            'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If


        If rbtmetalwisegrp.Checked = False Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR(CATCODE,CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
            StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
            StrSql += vbCrLf + " SELECT 'Z' AS CATCODE, 'SALES TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
            StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
            StrSql += vbCrLf + " SUM(PAYMENT),4 RESULT,2 RESULT1, 'S' COLHEAD "
            If RPT_SEP_REPAIRTAXDET Then
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR WHERE RESULT IN (1,3) "
            Else
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR WHERE RESULT = 1 "
            End If
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        If rbtmetalwisegrp.Checked = False Then
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'REPAIR DEL ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'REPAIR DEL ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
            End If
            'strsql += vbcrlf + " (DESCRIPTION, COLHEAD) VALUES('SALES','T') "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR WHERE RESULT1 = 1 AND COLHEAD <> 'D'"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR)>0"
            StrSql += vbCrLf + " BEGIN "
            'StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            ''strsql += vbcrlf + " (DESCRIPTION, COLHEAD) VALUES('SALES','T') "
            'StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) "
            'StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
            'StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES "
            'StrSql += VBCRLF + " WHERE RESULT1 = 1"
            'StrSql += VBCRLF + " AND COLHEAD <> 'D'"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR"
            StrSql += vbCrLf + " WHERE RESULT1 = 1 AND COLHEAD='L'"
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub ProcSales()
        If (RPT_ME_SUM_DABS = True And rbtMetalwise.Checked = True) Then Exit Sub
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSTAXTRAN') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = "SELECT *,CONVERT(VARCHAR(7),NULL)AS CATCODE,CONVERT(VARCHAR(1),NULL)AS METALID,CONVERT(VARCHAR(10),NULL)AS SYSTEMID INTO TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN "
        StrSql += vbCrLf + " WHERE ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        '' REPAIR DELIVERY SEP TAX 30JULY2022
        If RPT_SEP_REPAIRTAXDET Then
            StrSql += vbCrLf + " I.TRANTYPE IN('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
        Else
            StrSql += vbCrLf + " I.TRANTYPE IN('SA','OD','RD') AND ISNULL(I.CANCEL,'') = '' "
        End If
        '' END REPAIR DELIVERY SEP TAX 30JULY2022
        StrSql += vbCrLf + " AND ISNULL(I.TAX,0)<>0"
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")

        StrSql += vbCrLf + " )"
        If RPT_SEP_REPAIRTAXDET Then
            StrSql += vbCrLf + " AND TRANTYPE IN('SA','OD') "
        Else
            StrSql += vbCrLf + " AND TRANTYPE IN('SA','OD','RD') "
        End If
        'StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = "UPDATE T SET CATCODE=I.CATCODE ,METALID=I.METALID FROM TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN T, " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.SNO=T.ISSSNO AND I.BATCHNO=T.BATCHNO AND I.TRANTYPE=T.TRANTYPE "
        'StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = "UPDATE T SET SYSTEMID=I.SYSTEMID FROM TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN T, " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.BATCHNO=T.BATCHNO AND I.TRANTYPE=T.TRANTYPE "
        'StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSSALES') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSSALES"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT CATCODE, "
        ElseIf rbtNodeWise.Checked = True Then
            StrSql += vbCrLf + " SELECT SYSTEMID AS CATCODE,"
        Else
            StrSql += vbCrLf + " SELECT METALID AS CATCODE, "
        End If
        StrSql += vbCrLf + " CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + " PAYMENT, AVERAGE, RATE,"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " DISCPER,"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + " SYSTEMID,"
        StrSql += vbCrLf + "  COLHEAD, RESULT, RESULT1 INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALES FROM ("
        If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
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
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(3),'') COLHEAD "
        If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
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
        If RPT_SEPSTUDSTN_DABS Then
            StrSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
            StrSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)"
        End If

        StrSql += vbCrLf + " AS AMOUNT,RATE"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,(ISNULL(FIN_DISCOUNT,0)+ISNULL(DISCOUNT,0)) AS DISC "
        StrSql += vbCrLf + " ,CATCODE,METALID"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        StrSql += vbCrLf + " I.TRANTYPE IN('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")

        StrSql += vbCrLf + " UNION ALL"
        ''''StrSql += vbCrLf + " SELECT (SELECT SUM(PCS) FROM " & cnStockDb & "..ISSUE  WHERE SNO=I.ISSSNO)PCS"
        StrSql += vbCrLf + " SELECT 0 PCS"
        StrSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
        StrSql += vbCrLf + " ,SUM(GRSWT)NETWT"
        StrSql += vbCrLf + " ,NULL AS AMOUNT,RATE"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,0 AS DISC "
        StrSql += vbCrLf + " ,CATCODE,METALID"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSMETAL I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") "
        StrSql += vbCrLf + " AND ISSSNO  IN(SELECT SNO FROM " & cnStockDb & "..ISSUE "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND TRANTYPE IN('SA','OD') AND ISNULL(CANCEL,'') = '' ) "
        StrSql += vbCrLf + " GROUP BY CATCODE,ISSSNO ,SNO,AMOUNT,RATE,METALID "
        If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
        StrSql += vbCrLf + " )AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY I.CATCODE,C.METALID "
        ElseIf rbtNodeWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY I.CATCODE,C.METALID,SYSTEMID "
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
            If GstFlag Then
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_IGSTID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_IGSTID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " SUM(TAXAMOUNT) AS RECEIPT"
                StrSql += vbCrLf + " , 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='IG'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_IGSTID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_IGSTID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.S_IGSTID"
                End If
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_CGSTID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_CGSTID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " SUM(TAXAMOUNT) AS RECEIPT"
                StrSql += vbCrLf + " , 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='CG'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_CGSTID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_CGSTID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.S_CGSTID"
                End If
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_SGSTID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_SGSTID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " SUM(TAXAMOUNT) AS RECEIPT"
                StrSql += vbCrLf + " , 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='SG'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_SGSTID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_SGSTID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.S_SGSTID"
                End If
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.CESSID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.CESSID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " SUM(TAXAMOUNT) AS RECEIPT"
                StrSql += vbCrLf + " , 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSTAXTRAN I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='CS'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.CESSID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.CESSID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.CESSID"
                End If
                '' TCS
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT 'TCS' CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = 'TCS') AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT '' METALID, 'TCS' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = 'TCS') AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " SUM(AMOUNT) AS RECEIPT"
                StrSql += vbCrLf + " , 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",I.SYSTEMID"
                StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN I"
                StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
                StrSql += vbCrLf + " I.ACCODE IN ('TCS') AND ISNULL(I.CANCEL,'') = '' "
                StrSql += vbCrLf + " AND BATCHNO IN(SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ISSUE I "
                StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
                StrSql += vbCrLf + " I.TRANTYPE IN('SA','OD','RD') AND ISNULL(I.CANCEL,'') = '' "
                StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
                StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
                StrSql += vbCrLf + " )"
                StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
                StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
                StrSql += vbCrLf + " GROUP BY I.ACCODE "
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",I.SYSTEMID"
                StrSql += vbCrLf + "HAVING SUM(AMOUNT) <> 0 "
            Else
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
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
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",I.SYSTEMID"
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
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.STAXID,I.SYSTEMID HAVING SUM(I.TAX) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME"
                End If
            End If

            '' SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
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
            If rbtNodeWise.Checked Then StrSql += vbCrLf + ",I.SYSTEMID"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SSCID HAVING SUM(SC) <> 0"
            ElseIf rbtNodeWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SSCID,I.SYSTEMID HAVING SUM(SC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME  HAVING SUM(SC) <> 0"
            End If

            '' ADDTIONAL SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
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
            If rbtNodeWise.Checked Then StrSql += vbCrLf + ",I.SYSTEMID"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SASCID HAVING SUM(ADSC) <> 0"
            ElseIf rbtNodeWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SASCID,I.SYSTEMID HAVING SUM(ADSC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME HAVING SUM(ADSC) <> 0 "
            End If
        End If
        If RPT_SEPSTUD_DABS Then
            StrSql += vbCrLf + " UNION ALL "
            StrSql += vbCrLf + " SELECT CATCODE"
            If rbtCategoryWise.Checked = False And rbtNodeWise.Checked = False Then
                StrSql += vbCrLf + ",METALNAME"
            End If
            StrSql += vbCrLf + ", CATNAME"
            StrSql += vbCrLf + ", SUM(ISSPCS) AS ISSPCS, "
            StrSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
            StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " , COLHEAD "
            If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
            StrSql += vbCrLf + " FROM ("
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..ISSUE "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO And COMPANYID IN (" & SelectedCompanyId & ") And CATCODE = S.CATCODE) CATCODE,"
                If chkCatShortname.Checked = True Then
                    StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
                Else
                    StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
                End If
                StrSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATNAME,  "
            Else
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..ISSUE "
                ''strsql += vbcrlf + " WHERE BATCHNO = S.BATCHNO And COMPANYID IN (" & SelectedCompanyId & ") And CATCODE = S.CATCODE)AS CATCODE, 'DIAMOND' AS CATNAME,  "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS METALNAME,  "
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=S.CATCODE)CATNAME, "
            End If
            StrSql += vbCrLf + " SUM(S.STNPCS) ISSPCS,SUM(S.STNWT) AS ISSGRSWT,SUM(S.STNWT) AS ISSNETWT,  "
            StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.STNAMT) AS RECEIPT,  "
            StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE,"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
            StrSql += vbCrLf + " 'D' COLHEAD  "
            If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE S "
            StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SA','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            'StrSql += StrFilter
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
            ElseIf rbtNodeWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO,SYSTEMID  "
            Else
                StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE "
            End If
            StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
            If rbtCategoryWise.Checked = False And rbtNodeWise.Checked = False Then
                StrSql += vbCrLf + " ,METALNAME"
            End If
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
            If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
            'If RPT_SEPVAT_DABS Then
            '    'TAX
            '    StrSql += vbCrLf + " UNION ALL "
            '    StrSql += vbCrLf + " SELECT CATCODE"
            '    If rbtCategoryWise.Checked = False Then
            '        StrSql += vbCrLf + ",METALNAME"
            '    End If
            '    StrSql += vbCrLf + ", CATNAME,SUM(ISSPCS) AS ISSPCS,SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            '    StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
            '    StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
            '    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            '    StrSql += vbCrLf + " ,COLHEAD FROM ("
            '    If rbtCategoryWise.Checked = True Then
            '        StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..ISSUE "
            '        StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
            '        'If chkCatShortname.Checked = True Then
            '        '    StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE = S.CATCODE)  "
            '        'Else
            '        '    StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE = S.CATCODE)  "
            '        'End If
            '        StrSql += vbCrLf + "(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.STAXID) AS CATNAME,  "
            '    Else
            '        StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..ISSUE "
            '        StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS METALNAME,  "
            '        StrSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.STAXID) AS CATNAME, "
            '    End If
            '    StrSql += vbCrLf + " 0 ISSPCS,0 AS ISSGRSWT,0 AS ISSNETWT,  "
            '    StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.TAX) AS RECEIPT,  "
            '    StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            '    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
            '    StrSql += vbCrLf + " 'D' COLHEAD  FROM " & cnStockDb & "..ISSSTONE S "
            '    StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = S.CATCODE"
            '    StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
            '    StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            '    StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            '    StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            '    StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SA','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
            '    StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            '    'StrSql += StrFilter
            '    If rbtCategoryWise.Checked = True Then
            '        StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO,C.STAXID  "
            '    Else
            '        StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE,C.STAXID  "
            '    End If
            '    StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
            '    If rbtCategoryWise.Checked = False Then
            '        StrSql += vbCrLf + " ,METALNAME"
            '    End If
            '    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            '    StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
            'End If
        End If
        If RPT_SEPSTUDSTN_DABS Then
            StrSql += vbCrLf + " UNION ALL "
            StrSql += vbCrLf + " SELECT CATCODE"
            If rbtCategoryWise.Checked = False And rbtNodeWise.Checked = False Then
                StrSql += vbCrLf + ",METALNAME"
            End If
            StrSql += vbCrLf + ", CATNAME"
            StrSql += vbCrLf + ", SUM(ISSPCS) AS ISSPCS, "
            StrSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
            StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " , COLHEAD "
            If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,SYSTEMID"
            StrSql += vbCrLf + " FROM ( "
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..ISSUE "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO And COMPANYID IN (" & SelectedCompanyId & ") And CATCODE = S.CATCODE) CATCODE,"
                If chkCatShortname.Checked = True Then
                    StrSql += vbCrLf + " (Select SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
                Else
                    StrSql += vbCrLf + " (Select CATNAME FROM " & cnAdminDb & "..CATEGORY  "
                End If
                StrSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATNAME,  "
            Else
                StrSql += vbCrLf + " Select (Select DISTINCT METALID FROM " & cnStockDb & "..ISSUE "
                ''strsql += vbcrlf + " WHERE BATCHNO = S.BATCHNO And COMPANYID In (" & SelectedCompanyId & ") And CATCODE = S.CATCODE)As CATCODE, 'DIAMOND' AS CATNAME,  "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'STONE' AS METALNAME,  "
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=S.CATCODE)CATNAME, "
            End If
            StrSql += vbCrLf + " SUM(S.STNPCS) ISSPCS,SUM(S.STNWT) AS ISSGRSWT,SUM(S.STNWT) AS ISSNETWT,  "
            StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.STNAMT) AS RECEIPT,  "
            StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE,"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
            StrSql += vbCrLf + " 'D' COLHEAD  "
            If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE S "
            StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SA','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'S'  "
            'StrSql += StrFilter
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
            ElseIf rbtNodeWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO,SYSTEMID "
            Else
                StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE "
            End If
            StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
            If rbtCategoryWise.Checked = False And rbtNodeWise.Checked = False Then
                StrSql += vbCrLf + " ,METALNAME"
            End If
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
            If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
            'If RPT_SEPVAT_DABS Then
            '    'TAX
            '    StrSql += vbCrLf + " UNION ALL "
            '    StrSql += vbCrLf + " SELECT CATCODE"
            '    If rbtCategoryWise.Checked = False Then
            '        StrSql += vbCrLf + ",METALNAME"
            '    End If
            '    StrSql += vbCrLf + ", CATNAME,SUM(ISSPCS) AS ISSPCS,SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            '    StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
            '    StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
            '    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            '    StrSql += vbCrLf + " ,COLHEAD FROM ("
            '    If rbtCategoryWise.Checked = True Then
            '        StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..ISSUE "
            '        StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
            '        'If chkCatShortname.Checked = True Then
            '        '    StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE = S.CATCODE)  "
            '        'Else
            '        '    StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE = S.CATCODE)  "
            '        'End If
            '        StrSql += vbCrLf + "(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.STAXID) AS CATNAME,  "
            '    Else
            '        StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..ISSUE "
            '        StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'STONE' AS METALNAME,  "
            '        StrSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.STAXID) AS CATNAME, "
            '    End If
            '    StrSql += vbCrLf + " 0 ISSPCS,0 AS ISSGRSWT,0 AS ISSNETWT,  "
            '    StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.TAX) AS RECEIPT,  "
            '    StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            '    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
            '    StrSql += vbCrLf + " 'D' COLHEAD  FROM " & cnStockDb & "..ISSSTONE S "
            '    StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = S.CATCODE"
            '    StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
            '    StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            '    StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            '    StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            '    StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SA','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
            '    StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'S'  "
            '    'StrSql += StrFilter
            '    If rbtCategoryWise.Checked = True Then
            '        StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO,C.STAXID  "
            '    Else
            '        StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE,C.STAXID  "
            '    End If
            '    StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
            '    If rbtCategoryWise.Checked = False Then
            '        StrSql += vbCrLf + " ,METALNAME"
            '    End If
            '    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            '    StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
            'End If
        End If
        StrSql += vbCrLf + " ) X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If rbtmetalwisegrp.Checked = False And rbtNodeWise.Checked = False Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALES(CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " DISCPER, "
            StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD) "
            'StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
            StrSql += vbCrLf + " SELECT 'SALES' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
            StrSql += vbCrLf + " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, "
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " 0 DISCPER, "
            StrSql += vbCrLf + " 0 RESULT, 0 RESULT1, 'T' COLHEAD "
            'StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES"
            'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        ElseIf rbtNodeWise.Checked Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALES(CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
            StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
            StrSql += vbCrLf + " SELECT  CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES GROUP BY CATCODE,CATNAME"
            'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALES(CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
            StrSql += vbCrLf + " SELECT  CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES GROUP BY CATCODE,CATNAME"
            'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        If rbtmetalwisegrp.Checked = False And rbtNodeWise.Checked = False Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALES(CATCODE,CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
            StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " , RESULT, RESULT1, COLHEAD) "
            StrSql += vbCrLf + " SELECT 'Z' AS CATCODE, 'SALES TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
            StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), SUM(PAYMENT)"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " ,0 DISCPER"
            StrSql += vbCrLf + " ,4 RESULT,2 RESULT1, 'S' COLHEAD "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES WHERE RESULT1 = 1 "
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        If rbtmetalwisegrp.Checked = False And rbtNodeWise.Checked = False Then
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) " 'VALUES('SALES','T') "
                StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES "
            StrSql += vbCrLf + " WHERE RESULT1 = 1"
            'StrSql += VBCRLF + " AND COLHEAD <> 'D'"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES "
            'StrSql += vbCrLf + " WHERE RESULT1 = 1 "
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        ElseIf rbtNodeWise.Checked Then
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,CATCODE,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT '  SALES' ,'L',CATCODE, RESULT1,0,'C' "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES "
            'StrSql += VBCRLF + " WHERE RESULT1 = 1"
            StrSql += vbCrLf + "  ORDER BY COLHEAD "

            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,CATCODE,RESULT1,ORDERNO) "
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
            StrSql += vbCrLf + " COLHEAD,CATCODE, RESULT1,1 "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES "
            StrSql += vbCrLf + " WHERE RESULT1 = 1 AND COLHEAD='L'"
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,CATCODE,'L' COLHEAD,1,2,'M' COLHEAD1 "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU GROUP BY CATCODE,COLHEAD,RESULT1"
            StrSql += vbCrLf + " ORDER BY RESULT1"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else

            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,CATCODE,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT '  SALES' ,'L',CATCODE, RESULT1,0,'C' "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES "
            'StrSql += VBCRLF + " WHERE RESULT1 = 1"
            StrSql += vbCrLf + "  ORDER BY COLHEAD "

            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES "
            StrSql += vbCrLf + " WHERE RESULT1 = 1 AND COLHEAD='L'"
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,CATCODE,'L' COLHEAD,1,2,'M' COLHEAD1 "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU GROUP BY CATCODE,COLHEAD,RESULT1"
            StrSql += vbCrLf + " ORDER BY RESULT1"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

    End Sub

    Private Sub ProcSalesAndOrder()
        If (RPT_ME_SUM_DABS = True And rbtMetalwise.Checked = True) Then Exit Sub
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSSALES') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSSALES"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT CATCODE, "
        Else
            StrSql += vbCrLf + " SELECT METALID AS CATCODE, "
        End If
        StrSql += vbCrLf + " CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + " PAYMENT, AVERAGE, RATE,"
        If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " DISCPER,"
        StrSql += vbCrLf + "  STYPE,COLHEAD, RESULT, RESULT1 INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALES FROM ("
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
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALES(CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " DISCPER, "
            StrSql += vbCrLf + " RESULT, RESULT1,STYPE, COLHEAD) "
            StrSql += vbCrLf + " SELECT CASE WHEN ISNULL(STYPE,'') ='O' THEN 'ORDER ' ELSE 'STOCK ' END +'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
            StrSql += vbCrLf + " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, "
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " 0 DISCPER, "
            StrSql += vbCrLf + " 0 RESULT, 0 RESULT1,STYPE, 'T' COLHEAD "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES GROUP BY STYPE"
            'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALES(CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,DISCPER,"
            StrSql += vbCrLf + " RESULT, RESULT1,STYPE, COLHEAD,CATCODE) "
            StrSql += vbCrLf + " SELECT  CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
            StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT, SUM(ISNULL(PAYMENT,0))PAYMENT,SUM(ISNULL(DISCPER,0)) DISCPER,1 RESULT,1 RESULT1,STYPE, 'L' COLHEAD,CATCODE "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES GROUP BY CATCODE,CATNAME,STYPE"
            'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        If rbtmetalwisegrp.Checked = False Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALES(CATCODE,CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
            StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " , RESULT, RESULT1,STYPE, COLHEAD) "
            StrSql += vbCrLf + " SELECT 'Z' AS CATCODE, CASE WHEN ISNULL(STYPE,'') ='O' THEN 'ORDER ' ELSE 'DIRECT ' END +'SALES TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
            StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), SUM(PAYMENT)"
            If rbtMetalwise.Checked Then StrSql += vbCrLf + " ,0 DISCPER"
            StrSql += vbCrLf + " ,4 RESULT,2 RESULT1,STYPE, 'S' COLHEAD "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES WHERE RESULT = 1  GROUP BY STYPE"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        If rbtmetalwisegrp.Checked = False Then
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            'strsql += vbcrlf + " (DESCRIPTION, COLHEAD) VALUES('SALES','T') "
            StrSql += vbCrLf + " (DESCRIPTION,COLHEAD) "
            StrSql += vbCrLf + " SELECT CASE WHEN ISNULL(STYPE,'') ='O' THEN 'ORDER ' ELSE 'STOCK ' END +'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES  "
            StrSql += vbCrLf + " WHERE RESULT1 = 1 AND ISNULL(STYPE,'') <>'O' GROUP BY STYPE"
            'StrSql += VBCRLF + " AND COLHEAD <> 'D'"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES "
            StrSql += vbCrLf + " WHERE RESULT1 = 1 AND ISNULL(STYPE,'')<>'O'"
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"

            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            'strsql += vbcrlf + " (DESCRIPTION, COLHEAD) VALUES('SALES','T') "
            StrSql += vbCrLf + " (DESCRIPTION,COLHEAD) "
            StrSql += vbCrLf + " SELECT CASE WHEN ISNULL(STYPE,'') ='O' THEN 'ORDER ' ELSE 'STOCK ' END +'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES  "
            StrSql += vbCrLf + " WHERE RESULT1 = 1 AND ISNULL(STYPE,'') ='O' GROUP BY STYPE"
            'StrSql += VBCRLF + " AND COLHEAD <> 'D'"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES "
            StrSql += vbCrLf + " WHERE RESULT1 = 1 AND ISNULL(STYPE,'') ='O'"
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else

            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,CATCODE,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT '  SALES' ,'L',CATCODE, RESULT1,0,'C' "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES"
            'StrSql += VBCRLF + " WHERE RESULT1 = 1"
            StrSql += vbCrLf + "  ORDER BY COLHEAD "

            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES "
            StrSql += vbCrLf + " WHERE RESULT1 = 1 AND COLHEAD='L'  "
            StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"

            'StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            'StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,CATCODE,RESULT1,ORDERNO,COLHEAD1) "
            'StrSql += vbCrLf + " SELECT CASE WHEN ISNULL(STYPE,'') ='O' THEN 'ORDER ' ELSE 'DIRECT ' END +'  SALES' ,'L',CATCODE, RESULT1,0,'C' "
            'StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES WHERE ISNULL(STYPE,'') ='O'"
            ''StrSql += VBCRLF + " WHERE RESULT1 = 1"
            'StrSql += VBCRLF + "  ORDER BY COLHEAD "
            'StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            'StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES "
            'StrSql += vbCrLf + " WHERE RESULT1 = 1 AND COLHEAD='L' AND  ISNULL(STYPE,'') ='O' "
            'StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,CATCODE,'L' COLHEAD,1,2,'M' COLHEAD1 "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU GROUP BY CATCODE,COLHEAD,RESULT1"
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
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = "
        StrSql += vbCrLf + " 'TEMP" & systemId & "SASRPUTEMP') "
        StrSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "SASRPUTEMP "
        StrSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "SASRPUTEMP( "
        StrSql += vbCrLf + " DESCRIPTION VARCHAR(100),"
        StrSql += vbCrLf + " ISSPCS INT,"
        StrSql += vbCrLf + " ISSGRSWT NUMERIC(15,3),"
        StrSql += vbCrLf + " ISSNETWT NUMERIC(15,3),"
        StrSql += vbCrLf + " RECPCS INT,"
        StrSql += vbCrLf + " RECGRSWT NUMERIC(15,3),"
        StrSql += vbCrLf + " RECNETWT NUMERIC(15,3),"
        StrSql += vbCrLf + " RECEIPT NUMERIC(20,2),"
        StrSql += vbCrLf + " PAYMENT NUMERIC(20,2),"
        StrSql += vbCrLf + " AVERAGE VARCHAR(50),"
        StrSql += vbCrLf + " COLHEAD VARCHAR(1),"
        StrSql += vbCrLf + " RESULT1 INT,"
        StrSql += vbCrLf + " SNO INT IDENTITY(1,1),"
        StrSql += vbCrLf + " METALID VARCHAR(2))"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        For cnt As Integer = 0 To Dtmetal.Rows.Count - 1
            ProcSalesMETAL(Dtmetal.Rows(cnt).Item(0).ToString)
            ProcPartlySalesMETAL(Dtmetal.Rows(cnt).Item(0).ToString)
            ProcMiscIssueMETAL(Dtmetal.Rows(cnt).Item(0).ToString)
        Next
        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "SASRPUTEMP)>0"
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(ISNULL(RECEIPT,0)))+']' ,'T' "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPUTEMP "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        For cnt As Integer = 0 To Dtmetal.Rows.Count - 1
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "SASRPUTEMP WHERE METALID like '%" + Dtmetal.Rows(cnt).Item(0).ToString + "')>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) "
            StrSql += vbCrLf + " SELECT '" + Dtmetal.Rows(cnt).Item(1).ToString + "' ,'T' "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT"
            StrSql += vbCrLf + " ,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,RESULT1)"
            StrSql += vbCrLf + " select DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS"
            StrSql += vbCrLf + " ,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,RESULT1"
            StrSql += vbCrLf + " from TEMPTABLEDB..TEMP" & systemId & "SASRPUTEMP WHERE METALID='" + Dtmetal.Rows(cnt).Item(0).ToString + "' ORDER BY METALID,SNO "
            If Dtmetal.Rows(cnt).Item(0).ToString.ToUpper() = "D" Then
                StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
                StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT"
                StrSql += vbCrLf + " ,RECNETWT,COLHEAD,RESULT1)"
                StrSql += vbCrLf + " select DESCRIPTION,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT),SUM(RECPCS)"
                StrSql += vbCrLf + " ,SUM(RECGRSWT),SUM(RECNETWT),COLHEAD,RESULT1"
                StrSql += vbCrLf + " from TEMPTABLEDB..TEMP" & systemId & "SASRPUTEMP WHERE METALID='SD' GROUP BY DESCRIPTION,COLHEAD,RESULT1"
            End If
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION,ISSPCS, ISSGRSWT, ISSNETWT, "
            StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT1, COLHEAD) "
            StrSql += vbCrLf + " SELECT '" + Dtmetal.Rows(cnt).Item(1).ToString + " TOT',SUM(isnull(ISSPCS,0)),SUM(isnull(ISSGRSWT,0)),SUM(isnull(ISSNETWT,0)), "
            StrSql += vbCrLf + " SUM(isnull(RECPCS,0)),SUM(isnull(RECGRSWT,0)),SUM(isnull(RECNETWT,0)), SUM(isnull(RECEIPT,0)), "
            StrSql += vbCrLf + " SUM(isnull(PAYMENT,0)),2 RESULT1, 'S' COLHEAD "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPUTEMP WHERE METALID like '%" + Dtmetal.Rows(cnt).Item(0).ToString + "'"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
        Next

        StrSql = vbCrLf + " UPDATE  TEMPTABLEDB..TEMP" & systemId & "SASRPU SET ISSPCS=NULL where ISSPCS=0  "
        StrSql += vbCrLf + " UPDATE  TEMPTABLEDB..TEMP" & systemId & "SASRPU SET ISSGRSWT=NULL where ISSGRSWT=0  "
        StrSql += vbCrLf + " UPDATE  TEMPTABLEDB..TEMP" & systemId & "SASRPU SET ISSNETWT=NULL where ISSNETWT=0  "
        StrSql += vbCrLf + " UPDATE  TEMPTABLEDB..TEMP" & systemId & "SASRPU SET RECPCS=NULL where RECPCS=0  "
        StrSql += vbCrLf + " UPDATE  TEMPTABLEDB..TEMP" & systemId & "SASRPU SET RECGRSWT=NULL where RECGRSWT=0  "
        StrSql += vbCrLf + " UPDATE  TEMPTABLEDB..TEMP" & systemId & "SASRPU SET RECNETWT=NULL where RECNETWT=0  "
        StrSql += vbCrLf + " UPDATE  TEMPTABLEDB..TEMP" & systemId & "SASRPU SET RECEIPT=NULL where RECEIPT=0  "
        StrSql += vbCrLf + " UPDATE  TEMPTABLEDB..TEMP" & systemId & "SASRPU SET PAYMENT=NULL where PAYMENT=0  "
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcMiscIssueMETAL(ByVal METALID As String)
        If RPT_MIS_ISS_DABS = False Then Exit Sub
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSMISCISSUE') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSMISCISSUE"

        StrSql += vbCrLf + " SELECT METALID, "
        StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS CATNAME, "

        StrSql += vbCrLf + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += vbCrLf + " 0 AS RECEIPT, 0 AS PAYMENT, 1 AS RESULT1, ' ' COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSMISCISSUE FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " TRANTYPE = 'MI' AND ISNULL(I.CANCEL,'') = ''  AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND I.METALID='" + METALID + "' "
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        StrSql += vbCrLf + " GROUP BY METALID "

        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSMISCISSUE)>0"
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPUTEMP"
        StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,COLHEAD,RESULT1,METALID) "
        StrSql += vbCrLf + " SELECT 'MISC ISSUE ' + CATNAME CATNAME, "
        StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
        StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
        StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
        StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " COLHEAD, RESULT1,'" + METALID + "'"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSMISCISSUE "
        StrSql += vbCrLf + " ORDER BY CATNAME,RESULT1"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

    End Sub

    Private Sub ProcPartlySalesMETAL(ByVal METALID As String)
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPARTLYSALE') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE "
        StrSql += vbCrLf + " SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
        StrSql += vbCrLf + " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE FROM ("
        If chkCatShortname.Checked = True Then
            StrSql += vbCrLf + " SELECT (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
        Else
            StrSql += vbCrLf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        End If
        StrSql += vbCrLf + " WHERE CATCODE = I.CATCODE)AS CATNAME   "
        StrSql += vbCrLf + " ,I.PCS AS ISSPCS, I.GRSWT AS ISSGRSWT, I.NETWT AS ISSNETWT "
        StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(STNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNPCS "
        StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(TAGSTNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNPCS "
        StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(STNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO )STNWT "
        StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(TAGSTNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNWT "
        StrSql += vbCrLf + " ,TAGPCS,TAGGRSWT,TAGNETWT FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
        StrSql += vbCrLf + " AND I.TAGNO <> ''"
        StrSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''   "
        StrSql += vbCrLf + " AND I.METALID='" + METALID + "' "
        StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        StrSql += vbCrLf + " ) X "
        StrSql += vbCrLf + " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
        StrSql += vbCrLf + " ) GROUP BY CATNAME"
        StrSql += vbCrLf + " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPUTEMP"
        StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD,METALID) "
        StrSql += vbCrLf + " SELECT 'PARTLY SALES ' + CATNAME CATNAME, "
        StrSql += vbCrLf + " CASE WHEN ISSPCS    <> 0 THEN ISSPCS    ELSE NULL END ISSPCS, "
        StrSql += vbCrLf + " CASE WHEN ISSGRSWT  <> 0 THEN ISSGRSWT  ELSE NULL END ISSGRSWT, "
        StrSql += vbCrLf + " CASE WHEN ISSNETWT  <> 0 THEN ISSNETWT  ELSE NULL END ISSNETWT, "
        StrSql += vbCrLf + " 1 RESULT1, COLHEAD,'" + METALID + "'"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE ORDER BY RESULT1 "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


    End Sub

    Private Sub ProcSalesMETAL(ByVal METALID As String)
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSSALES') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSSALES"
        StrSql += vbCrLf + " SELECT METALID AS CATCODE, "
        StrSql += vbCrLf + " CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + " PAYMENT, AVERAGE, RATE, COLHEAD, RESULT, RESULT1 INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALES FROM ("
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
            StrSql += vbCrLf + " SUM(TAX/*+ISNULL(TT.TAXAMT,0)*/) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
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

        StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALES(CATNAME, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += vbCrLf + " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, 0 RESULT, 0 RESULT1, 'T' COLHEAD "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES"
        'StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALES(CATCODE,CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'Z' AS CATCODE, 'SALES TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + " SUM(PAYMENT),4 RESULT,2 RESULT1, 'S' COLHEAD "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES WHERE RESULT = 1 "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0"
        StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPUTEMP"
        ''strsql += vbcrlf + " (DESCRIPTION, COLHEAD) VALUES('SALES','T') "
        'StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) "
        'StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
        'StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES "
        'StrSql += VBCRLF + " WHERE RESULT1 = 1"
        'StrSql += VBCRLF + " AND COLHEAD <> 'D'"
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPUTEMP"
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
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES "
        StrSql += vbCrLf + " WHERE RESULT1 = 1 "
        StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcSalesReturn19_04_2021()
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSSALESRETURN') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + "  SELECT CATCODE, "
        ElseIf rbtNodeWise.Checked = True Then
            StrSql += vbCrLf + "  SELECT SYSTEMID AS CATCODE, "
        Else
            StrSql += vbCrLf + "  SELECT METALID AS CATCODE, "
        End If
        StrSql += vbCrLf + "  CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, "
        StrSql += vbCrLf + "  RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + "  PAYMENT, AVERAGE, RATE, COLHEAD, RESULT,  RESULT1  "
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + " ,SRTYPE"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,SYSTEMID"
        StrSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN "
        StrSql += vbCrLf + "  FROM ("
        If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
            StrSql += vbCrLf + "  SELECT R.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + "  (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            End If
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
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + " ,SRTYPE"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,SYSTEMID"
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
        '*****
        If RPT_SEPSTUD_DABS Then
            StrSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
        End If
        If RPT_SEPSTUDSTN_DABS Then
            StrSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO "
            StrSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)"
        End If
        StrSql += vbCrLf + " AS AMOUNT,RATE,CATCODE,METALID"
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,R.SYSTEMID"
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
        ElseIf rbtNodeWise.Checked Then
            StrSql += vbCrLf + " GROUP BY R.CATCODE,C.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY R.METALID "
        End If
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,R.SRTYPE"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,SYSTEMID"

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
            If GstFlag Then
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                    StrSql += vbCrLf + "  WHERE ACCODE = C.STAXID) AS CATNAME, "
                Else
                    StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNTAX' AS CATNAME,"
                End If
                StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + "  0 AS RECEIPT, SUM(TAX+ISNULL(TT.TAXAMT,0)) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
                StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
                If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
                If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,R.SYSTEMID"
                StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
                StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
                StrSql += vbCrLf + " ON TT.SNO=R.SNO and TT.BATCHNO=R.BATCHNO "
                StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
                StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
                StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + "  GROUP BY R.CATCODE,C.STAXID "
                    If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
                    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
                    StrSql += vbCrLf + "  HAVING SUM(TAX) <> 0"

                Else
                    StrSql += vbCrLf + "  GROUP BY R.METALID  "
                    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
                End If
            Else
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                    StrSql += vbCrLf + "  WHERE ACCODE = C.STAXID) AS CATNAME, "
                Else
                    StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNTAX' AS CATNAME,"
                End If
                StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + "  0 AS RECEIPT, SUM(TAX+ISNULL(TT.TAXAMT,0)) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
                StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
                If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
                If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,R.SYSTEMID"
                StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
                StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
                StrSql += vbCrLf + " ON TT.SNO=R.SNO and TT.BATCHNO=R.BATCHNO "
                StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
                StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
                StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + "  GROUP BY R.CATCODE,C.STAXID "
                    If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
                    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
                    StrSql += vbCrLf + "  HAVING SUM(TAX) <> 0"
                Else
                    StrSql += vbCrLf + "  GROUP BY R.METALID  "
                    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
                End If
            End If
            StrSql += vbCrLf + "  UNION ALL "
            ''SURCHARGE
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.SSCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNSURCHARGE' AS CATNAME,"
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(SC) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE ,C.SSCID"
                If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID  "
            End If
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
            StrSql += vbCrLf + "  HAVING SUM(SC) <> 0 "
            StrSql += vbCrLf + "  UNION ALL "

            ''ADDTIONAL SURCHARGE
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.SASCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNSURCHARGE' AS CATNAME,"
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(ADSC) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE , C.SASCID "
                If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID  "
            End If
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
            StrSql += vbCrLf + "  HAVING SUM(ADSC) <> 0 "

            StrSql += vbCrLf + "  UNION ALL "
            StrSql += vbCrLf + "  SELECT CATCODE,CATNAME, SUM(ISSPCS) AS ISSPCS, "
            StrSql += vbCrLf + "  SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += vbCrLf + "  SUM(RECPCS) AS RECPCS, SUM(RECGRSWT) AS RECGRSWT, SUM(RECNETWT) AS RECNETWT, "
            StrSql += vbCrLf + "  SUM(RECEIPT) RECEIPT, SUM(PAYMENT) PAYMENT, "
            StrSql += vbCrLf + "  RESULT, 1 AS RESULT1, AVERAGE, RATE, COLHEAD "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",SYSTEMID"
            StrSql += vbCrLf + "  FROM ("
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  SELECT (SELECT TOP 1 CATCODE FROM " & cnStockDb & "..RECEIPT "
                StrSql += vbCrLf + "  WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
                If chkCatShortname.Checked = True Then
                    StrSql += vbCrLf + "  (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
                Else
                    StrSql += vbCrLf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
                End If
                StrSql += vbCrLf + "  WHERE CATCODE = S.CATCODE) CATNAME,  "
            Else
                StrSql += vbCrLf + "  SELECT (SELECT TOP 1 METALID FROM " & cnStockDb & "..RECEIPT "
                StrSql += vbCrLf + "  WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & "))AS CATCODE, 'DIAMOND' AS CATNAME,  "
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS,  0 AS ISSGRSWT, 0 AS ISSNETWT,  "
            StrSql += vbCrLf + "  SUM(S.STNPCS) AS RECPCS,  SUM(S.STNWT)  AS RECGRSWT,  SUM(S.STNWT)  AS RECNETWT,  "
            StrSql += vbCrLf + "  0 AS RECEIPT,  SUM(S.STNAMT) AS PAYMENT,  "
            StrSql += vbCrLf + "  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            StrSql += vbCrLf + "  'D' COLHEAD  "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,(SELECT TOP 1 CASE WHEN ISNULL(REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=S.BATCHNO) AS SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",S.SYSTEMID"
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE S "
            StrSql += vbCrLf + "  WHERE S.TRANTYPE='SR' AND S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + "  " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(CANCEL,'') = ''  AND TRANTYPE = 'SR' AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + "  AND  (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            StrSql += vbCrLf + "  AND COMPANYID IN (" & SelectedCompanyId & ")"
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  GROUP BY S.CATCODE,S.BATCHNO  "
                If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",S.SYSTEMID"
            Else
                StrSql += vbCrLf + "  GROUP BY S.BATCHNO  "
            End If
            StrSql += vbCrLf + "  ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",SYSTEMID"
        End If


        ''If RPT_SEPSTUD_DABS Then
        ''    StrSql += vbCrLf + " UNION ALL "
        ''    StrSql += vbCrLf + " SELECT CATCODE"
        ''    If rbtCategoryWise.Checked = False Then
        ''        StrSql += vbCrLf + ",METALNAME"
        ''    End If
        ''    StrSql += vbCrLf + ", CATNAME"
        ''    StrSql += vbCrLf + ", SUM(ISSPCS) AS ISSPCS, "
        ''    StrSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
        ''    StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
        ''    StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
        ''    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER"
        ''    StrSql += vbCrLf + " , COLHEAD "
        ''    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
        ''    StrSql += vbCrLf + " FROM ("
        ''    If rbtCategoryWise.Checked = True Then
        ''        StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..RECEIPT "
        ''        StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
        ''        If chkCatShortname.Checked = True Then
        ''            StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
        ''        Else
        ''            StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
        ''        End If
        ''        StrSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATNAME,  "
        ''    Else
        ''        StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..RECEIPT "
        ''        ''strsql += vbcrlf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE)AS CATCODE, 'DIAMOND' AS CATNAME,  "
        ''        StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS METALNAME,  "
        ''        StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=S.CATCODE)CATNAME, "
        ''    End If
        ''    StrSql += vbCrLf + " 0 ISSPCS,0 AS ISSGRSWT,0 AS ISSNETWT,  "
        ''    StrSql += vbCrLf + " SUM(S.STNPCS) RECPCS,  SUM(S.STNWT) AS RECGRSWT,  SUM(S.STNWT) AS RECNETWT,  0 AS RECEIPT,  "
        ''    StrSql += vbCrLf + " SUM(S.STNAMT) AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE,"
        ''    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
        ''    StrSql += vbCrLf + " 'D' COLHEAD "
        ''    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,(SELECT TOP 1 CASE WHEN ISNULL(REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=S.BATCHNO) AS SRTYPE"
        ''    StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE S "
        ''    StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
        ''    StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        ''    StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        ''    StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        ''    StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SR') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
        ''    StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
        ''    'StrSql += StrFilter
        ''    If rbtCategoryWise.Checked = True Then
        ''        StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
        ''    Else
        ''        StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE "
        ''    End If
        ''    StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
        ''    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
        ''    If rbtCategoryWise.Checked = False Then
        ''        StrSql += vbCrLf + " ,METALNAME"
        ''    End If
        ''    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
        ''    StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        ''End If

        If RPT_SEPSTUDSTN_DABS Then
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
            StrSql += vbCrLf + " , COLHEAD"
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",SYSTEMID"
            StrSql += vbCrLf + "  FROM ("
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..RECEIPT "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
                If chkCatShortname.Checked = True Then
                    StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
                Else
                    StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
                End If
                StrSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATNAME,  "
            Else
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..RECEIPT "
                ''strsql += vbcrlf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE)AS CATCODE, 'DIAMOND' AS CATNAME,  "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'STONE' AS METALNAME,  "
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=S.CATCODE)CATNAME, "
            End If
            StrSql += vbCrLf + " 0 ISSPCS,0 AS ISSGRSWT,0 AS ISSNETWT,  "
            StrSql += vbCrLf + " SUM(S.STNPCS) RECPCS,  SUM(S.STNWT) AS RECGRSWT,  SUM(S.STNWT) AS RECNETWT,  0 AS RECEIPT,  "
            StrSql += vbCrLf + " SUM(S.STNAMT) AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE,"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
            StrSql += vbCrLf + " 'D' COLHEAD  "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,(SELECT TOP 1 CASE WHEN ISNULL(REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=S.BATCHNO) AS SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",S.SYSTEMID"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE S "
            StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SR') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'S'  "
            'StrSql += StrFilter
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
                If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",S.SYSTEMID"
            Else
                StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE "
            End If
            StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",SYSTEMID"
            If rbtCategoryWise.Checked = False And rbtNodeWise.Checked = False Then
                StrSql += vbCrLf + " ,METALNAME"
            End If
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        End If
        StrSql += vbCrLf + "  ) X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        'If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN(CATNAME, "
        StrSql += vbCrLf + "  ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + "  SELECT 'SALES RETURN'CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += vbCrLf + "  0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT,  0 RESULT, 0 RESULT1, 'T' COLHEAD "
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        'Else
        'StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0 "
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN(CATNAME, "
        'StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
        'StrSql += vbCrLf + " SELECT 'SALES RETURN' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        'StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT,SUM(ISNULL(PAYMENT,0))PAYMENT, 1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
        'StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN GROUP BY CATCODE"
        ''StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        'End If

        ' If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + "  SELECT 'Z' AS CATCODE, 'SALES RETURN TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += vbCrLf + "  SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + "  SUM(PAYMENT), 4 RESULT, 2 RESULT1,'S' COLHEAD "
        StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT = 1 "
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  GROUP BY SRTYPE"
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        ' End If

        If RPT_DABS_SEP_SRTYPE Then
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0"
            StrSql += vbCrLf + "  BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + "  (DESCRIPTION, COLHEAD) "
            StrSql += vbCrLf + "  SELECT 'SALES RETURN WITHOUT ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1"
            StrSql += vbCrLf + "  AND SRTYPE='NO BILL'"
            StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1 "
            StrSql += vbCrLf + "  AND SRTYPE='NO BILL'"
            StrSql += vbCrLf + "  ORDER BY RESULT1, CATCODE, RESULT"
            StrSql += vbCrLf + "  END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0"
            StrSql += vbCrLf + "  BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + "  (DESCRIPTION, COLHEAD) "
            StrSql += vbCrLf + "  SELECT 'SALES RETURN WITH BILL ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1"
            StrSql += vbCrLf + "  AND SRTYPE='BILL'"
            StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1 "
            StrSql += vbCrLf + "  AND SRTYPE='BILL'"
            StrSql += vbCrLf + "  ORDER BY RESULT1, CATCODE, RESULT"
            StrSql += vbCrLf + "  END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0"
            StrSql += vbCrLf + "  BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + "  SELECT 'SALES RETURN ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + "  SELECT 'SALES RETURN ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
            End If
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1"
            StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN "
            StrSql += vbCrLf + "  WHERE RESULT1 = 1 "
            StrSql += vbCrLf + "  ORDER BY RESULT1, CATCODE, RESULT"
            StrSql += vbCrLf + "  END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

    End Sub

    Private Sub ProcSalesReturn()

        ''NEWLY ADDED
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "SRABSTAXTRAN') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = "SELECT *,CONVERT(VARCHAR(7),NULL)AS CATCODE,CONVERT(VARCHAR(1),NULL)AS METALID,CONVERT(VARCHAR(10),NULL)AS SYSTEMID INTO TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN"
        StrSql += vbCrLf + " WHERE ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..RECEIPT I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        StrSql += vbCrLf + " I.TRANTYPE IN('SR') AND ISNULL(I.CANCEL,'') = '' "
        StrSql += vbCrLf + " AND ISNULL(I.TAX,0)<>0"
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
        StrSql += vbCrLf + " )"
        StrSql += vbCrLf + " AND TRANTYPE IN('SR') "
        'StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = "UPDATE T SET CATCODE=I.CATCODE ,METALID=I.METALID FROM TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN T, " & cnStockDb & "..RECEIPT I "
        StrSql += vbCrLf + " WHERE I.SNO=T.ISSSNO AND I.BATCHNO=T.BATCHNO AND I.TRANTYPE=T.TRANTYPE "
        'StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = "UPDATE T SET SYSTEMID=I.SYSTEMID FROM TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN T, " & cnStockDb & "..RECEIPT I "
        StrSql += vbCrLf + " WHERE I.BATCHNO=T.BATCHNO AND I.TRANTYPE=T.TRANTYPE "
        'StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        ''END NEWLY ADDED

        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSSALESRETURN') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + "  SELECT CATCODE, "
        ElseIf rbtNodeWise.Checked = True Then
            StrSql += vbCrLf + "  SELECT SYSTEMID AS CATCODE, "
        Else
            StrSql += vbCrLf + "  SELECT METALID AS CATCODE, "
        End If
        StrSql += vbCrLf + "  CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, "
        StrSql += vbCrLf + "  RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + "  PAYMENT, AVERAGE, RATE, COLHEAD, RESULT,  RESULT1  "
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + " ,SRTYPE"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,SYSTEMID"
        StrSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN "
        StrSql += vbCrLf + "  FROM ("
        If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
            StrSql += vbCrLf + "  SELECT R.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + "  (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            End If
        Else
            StrSql += vbCrLf + "  SELECT R.METALID, "
            StrSql += vbCrLf + "  (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = R.METALID)AS METALNAME, "
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
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + " ,SRTYPE"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,SYSTEMID"
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
        '*****
        If RPT_SEPSTUD_DABS Then
            StrSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
        End If
        If RPT_SEPSTUDSTN_DABS Then
            StrSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO "
            StrSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)"
        End If
        StrSql += vbCrLf + " AS AMOUNT,RATE,CATCODE,METALID"
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,R.SYSTEMID"
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
        ElseIf rbtNodeWise.Checked Then
            StrSql += vbCrLf + " GROUP BY R.CATCODE,C.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY R.METALID "
        End If
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,R.SRTYPE"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,SYSTEMID"

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
            If GstFlag Then
                ''If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                ''    StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                ''    StrSql += vbCrLf + "  WHERE ACCODE = C.STAXID) AS CATNAME, "
                ''Else
                ''    StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNTAX' AS CATNAME,"
                ''End If
                ''StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                ''StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                ''StrSql += vbCrLf + "  0 AS RECEIPT, SUM(TAX+ISNULL(TT.TAXAMT,0)) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
                ''StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
                ''If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
                ''If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,R.SYSTEMID"
                ''StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
                ''StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                ''StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
                ''StrSql += vbCrLf + " ON TT.SNO=R.SNO and TT.BATCHNO=R.BATCHNO "
                ''StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                ''StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                ''StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
                ''StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
                ''StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
                ''If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                ''    StrSql += vbCrLf + "  GROUP BY R.CATCODE,C.STAXID "
                ''    If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
                ''    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
                ''    StrSql += vbCrLf + "  HAVING SUM(TAX) <> 0"

                ''Else
                ''    StrSql += vbCrLf + "  GROUP BY R.METALID  "
                ''    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
                ''End If
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_IGSTID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_IGSTID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " 0 AS RECEIPT"
                StrSql += vbCrLf + " , SUM(TAXAMOUNT) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                ''If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,'' SRTYPE"
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='IG'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_IGSTID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_IGSTID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.S_IGSTID"
                End If
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_CGSTID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_CGSTID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " 0 AS RECEIPT"
                StrSql += vbCrLf + " , SUM(TAXAMOUNT) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                ''If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,'' SRTYPE"
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='CG'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_CGSTID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_CGSTID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.S_CGSTID"
                End If
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_SGSTID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_SGSTID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " 0 AS RECEIPT"
                StrSql += vbCrLf + " , SUM(TAXAMOUNT) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                ''If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,'' SRTYPE"
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='SG'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_SGSTID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_SGSTID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.S_SGSTID"
                End If
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.CESSID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.CESSID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " 0 AS RECEIPT"
                StrSql += vbCrLf + " , SUM(TAXAMOUNT) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                '' If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,'' SRTYPE"
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='CS'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.CESSID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.CESSID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.CESSID"
                End If
                '' TCS
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT 'TCS' CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = 'TCS') AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT '' METALID, 'TCS' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = 'TCS') AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " 0 AS RECEIPT"
                StrSql += vbCrLf + " , SUM(AMOUNT) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                ''If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,'' SRTYPE"
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",I.SYSTEMID"
                StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN I"
                StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
                StrSql += vbCrLf + " I.ACCODE IN ('TCS') AND ISNULL(I.CANCEL,'') = '' "
                StrSql += vbCrLf + " AND BATCHNO IN(SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
                StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
                StrSql += vbCrLf + " I.TRANTYPE IN('SA','OD','RD') AND ISNULL(I.CANCEL,'') = '' "
                StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
                StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
                StrSql += vbCrLf + " )"
                StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
                StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
                StrSql += vbCrLf + " GROUP BY I.ACCODE "
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",I.SYSTEMID"
                StrSql += vbCrLf + " HAVING SUM(AMOUNT) <> 0 "
            Else
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                    StrSql += vbCrLf + "  WHERE ACCODE = C.STAXID) AS CATNAME, "
                Else
                    StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNTAX' AS METALNAME, 'SALESRETURNTAX' AS CATNAME,"
                End If
                StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + "  0 AS RECEIPT, SUM(TAX+ISNULL(TT.TAXAMT,0)) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
                StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
                If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
                If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,R.SYSTEMID"
                StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
                StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
                StrSql += vbCrLf + " ON TT.SNO=R.SNO and TT.BATCHNO=R.BATCHNO "
                StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
                StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
                StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + "  GROUP BY R.CATCODE,C.STAXID "
                    If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
                    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
                    StrSql += vbCrLf + "  HAVING SUM(TAX) <> 0"
                Else
                    StrSql += vbCrLf + "  GROUP BY R.METALID  "
                    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
                End If
            End If
            StrSql += vbCrLf + "  UNION ALL "
            ''SURCHARGE
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.SSCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNSURCHARGE' AS METALNAME, 'SALESRETURNSURCHARGE' AS CATNAME,"
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(SC) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE ,C.SSCID"
                If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID  "
            End If
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
            StrSql += vbCrLf + "  HAVING SUM(SC) <> 0 "
            StrSql += vbCrLf + "  UNION ALL "

            ''ADDTIONAL SURCHARGE
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.SASCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNSURCHARGE' AS METALNAME, 'SALESRETURNSURCHARGE' AS CATNAME,"
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(ADSC) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE , C.SASCID "
                If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID  "
            End If
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
            StrSql += vbCrLf + "  HAVING SUM(ADSC) <> 0 "

            StrSql += vbCrLf + "  UNION ALL "
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  SELECT CATCODE,CATNAME, SUM(ISSPCS) AS ISSPCS, "
            Else
                StrSql += vbCrLf + "  SELECT CATCODE METALID,CATNAME METALNAME ,CATNAME, SUM(ISSPCS) AS ISSPCS, "
            End If
            StrSql += vbCrLf + "  SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += vbCrLf + "  SUM(RECPCS) AS RECPCS, SUM(RECGRSWT) AS RECGRSWT, SUM(RECNETWT) AS RECNETWT, "
            StrSql += vbCrLf + "  SUM(RECEIPT) RECEIPT, SUM(PAYMENT) PAYMENT, "
            StrSql += vbCrLf + "  RESULT, 1 AS RESULT1, AVERAGE, RATE, COLHEAD "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",SYSTEMID"
            StrSql += vbCrLf + "  FROM ("
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  SELECT (SELECT TOP 1 CATCODE FROM " & cnStockDb & "..RECEIPT "
                StrSql += vbCrLf + "  WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
                If chkCatShortname.Checked = True Then
                    StrSql += vbCrLf + "  (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
                Else
                    StrSql += vbCrLf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
                End If
                StrSql += vbCrLf + "  WHERE CATCODE = S.CATCODE) CATNAME,  "
            Else
                StrSql += vbCrLf + "  SELECT (SELECT TOP 1 METALID FROM " & cnStockDb & "..RECEIPT "
                StrSql += vbCrLf + "  WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & "))AS CATCODE, 'DIAMOND' AS CATNAME,  "
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS,  0 AS ISSGRSWT, 0 AS ISSNETWT,  "
            StrSql += vbCrLf + "  SUM(S.STNPCS) AS RECPCS,  SUM(S.STNWT)  AS RECGRSWT,  SUM(S.STNWT)  AS RECNETWT,  "
            StrSql += vbCrLf + "  0 AS RECEIPT,  SUM(S.STNAMT) AS PAYMENT,  "
            StrSql += vbCrLf + "  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            StrSql += vbCrLf + "  'D' COLHEAD  "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,(SELECT TOP 1 CASE WHEN ISNULL(REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=S.BATCHNO) AS SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",S.SYSTEMID"
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE S "
            StrSql += vbCrLf + "  WHERE S.TRANTYPE='SR' AND S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + "  " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(CANCEL,'') = ''  AND TRANTYPE = 'SR' AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + "  AND  (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            StrSql += vbCrLf + "  AND COMPANYID IN (" & SelectedCompanyId & ")"
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  GROUP BY S.CATCODE,S.BATCHNO  "
                If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",S.SYSTEMID"
            Else
                StrSql += vbCrLf + "  GROUP BY S.BATCHNO  "
            End If
            StrSql += vbCrLf + "  ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",SYSTEMID"
        End If


        ''If RPT_SEPSTUD_DABS Then
        ''    StrSql += vbCrLf + " UNION ALL "
        ''    StrSql += vbCrLf + " SELECT CATCODE"
        ''    If rbtCategoryWise.Checked = False Then
        ''        StrSql += vbCrLf + ",METALNAME"
        ''    End If
        ''    StrSql += vbCrLf + ", CATNAME"
        ''    StrSql += vbCrLf + ", SUM(ISSPCS) AS ISSPCS, "
        ''    StrSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
        ''    StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
        ''    StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
        ''    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER"
        ''    StrSql += vbCrLf + " , COLHEAD "
        ''    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
        ''    StrSql += vbCrLf + " FROM ("
        ''    If rbtCategoryWise.Checked = True Then
        ''        StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..RECEIPT "
        ''        StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
        ''        If chkCatShortname.Checked = True Then
        ''            StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
        ''        Else
        ''            StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
        ''        End If
        ''        StrSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATNAME,  "
        ''    Else
        ''        StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..RECEIPT "
        ''        ''strsql += vbcrlf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE)AS CATCODE, 'DIAMOND' AS CATNAME,  "
        ''        StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS METALNAME,  "
        ''        StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=S.CATCODE)CATNAME, "
        ''    End If
        ''    StrSql += vbCrLf + " 0 ISSPCS,0 AS ISSGRSWT,0 AS ISSNETWT,  "
        ''    StrSql += vbCrLf + " SUM(S.STNPCS) RECPCS,  SUM(S.STNWT) AS RECGRSWT,  SUM(S.STNWT) AS RECNETWT,  0 AS RECEIPT,  "
        ''    StrSql += vbCrLf + " SUM(S.STNAMT) AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE,"
        ''    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
        ''    StrSql += vbCrLf + " 'D' COLHEAD "
        ''    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,(SELECT TOP 1 CASE WHEN ISNULL(REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=S.BATCHNO) AS SRTYPE"
        ''    StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE S "
        ''    StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
        ''    StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        ''    StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        ''    StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        ''    StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SR') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
        ''    StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
        ''    'StrSql += StrFilter
        ''    If rbtCategoryWise.Checked = True Then
        ''        StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
        ''    Else
        ''        StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE "
        ''    End If
        ''    StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
        ''    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
        ''    If rbtCategoryWise.Checked = False Then
        ''        StrSql += vbCrLf + " ,METALNAME"
        ''    End If
        ''    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
        ''    StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        ''End If

        If RPT_SEPSTUDSTN_DABS Then
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
            ''If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER"
            StrSql += vbCrLf + " , COLHEAD"
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",SYSTEMID"
            StrSql += vbCrLf + "  FROM ("
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..RECEIPT "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
                If chkCatShortname.Checked = True Then
                    StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
                Else
                    StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
                End If
                StrSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATNAME,  "
            Else
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..RECEIPT "
                ''strsql += vbcrlf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE)AS CATCODE, 'DIAMOND' AS CATNAME,  "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'STONE' AS METALNAME,  "
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=S.CATCODE)CATNAME, "
            End If
            StrSql += vbCrLf + " 0 ISSPCS,0 AS ISSGRSWT,0 AS ISSNETWT,  "
            StrSql += vbCrLf + " SUM(S.STNPCS) RECPCS,  SUM(S.STNWT) AS RECGRSWT,  SUM(S.STNWT) AS RECNETWT,  0 AS RECEIPT,  "
            StrSql += vbCrLf + " SUM(S.STNAMT) AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE,"
            ''If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
            StrSql += vbCrLf + " 'D' COLHEAD  "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,(SELECT TOP 1 CASE WHEN ISNULL(REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=S.BATCHNO) AS SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",S.SYSTEMID"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE S "
            StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SR') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'S'  "
            'StrSql += StrFilter
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
                If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",S.SYSTEMID"
            Else
                StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE "
            End If
            StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",SYSTEMID"
            If rbtCategoryWise.Checked = False And rbtNodeWise.Checked = False Then
                StrSql += vbCrLf + " ,METALNAME"
            End If
            ''If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        End If
        StrSql += vbCrLf + "  ) X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        'If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN(CATNAME, "
        StrSql += vbCrLf + "  ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + "  SELECT 'SALES RETURN'CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += vbCrLf + "  0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT,  0 RESULT, 0 RESULT1, 'T' COLHEAD "
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        'Else
        'StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0 "
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN(CATNAME, "
        'StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
        'StrSql += vbCrLf + " SELECT 'SALES RETURN' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        'StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT,SUM(ISNULL(PAYMENT,0))PAYMENT, 1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
        'StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN GROUP BY CATCODE"
        ''StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        'End If

        ' If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + "  SELECT 'Z' AS CATCODE, 'SALES RETURN TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += vbCrLf + "  SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + "  SUM(PAYMENT), 4 RESULT, 2 RESULT1,'S' COLHEAD "
        StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT = 1 "
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  GROUP BY SRTYPE"
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        ' End If

        If RPT_DABS_SEP_SRTYPE Then
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0"
            StrSql += vbCrLf + "  BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + "  (DESCRIPTION, COLHEAD) "
            StrSql += vbCrLf + "  SELECT 'SALES RETURN WITHOUT ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1"
            StrSql += vbCrLf + "  AND SRTYPE='NO BILL'"
            StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1 "
            StrSql += vbCrLf + "  AND SRTYPE='NO BILL'"
            StrSql += vbCrLf + "  ORDER BY RESULT1, CATCODE, RESULT"
            StrSql += vbCrLf + "  END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0"
            StrSql += vbCrLf + "  BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + "  (DESCRIPTION, COLHEAD) "
            StrSql += vbCrLf + "  SELECT 'SALES RETURN WITH BILL ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1"
            StrSql += vbCrLf + "  AND SRTYPE='BILL'"
            StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1 "
            StrSql += vbCrLf + "  AND SRTYPE='BILL'"
            StrSql += vbCrLf + "  ORDER BY RESULT1, CATCODE, RESULT"
            StrSql += vbCrLf + "  END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0"
            StrSql += vbCrLf + "  BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + "  SELECT 'SALES RETURN ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + "  SELECT 'SALES RETURN ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
            End If
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1"
            StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN "
            StrSql += vbCrLf + "  WHERE RESULT1 = 1 "
            StrSql += vbCrLf + "  ORDER BY RESULT1, CATCODE, RESULT"
            StrSql += vbCrLf + "  END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

    End Sub

    Private Sub ProcSalesReturn06_JULY_2021()

        ''NEWLY ADDED
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "SRABSTAXTRAN') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = "SELECT *,CONVERT(VARCHAR(7),NULL)AS CATCODE,CONVERT(VARCHAR(1),NULL)AS METALID,CONVERT(VARCHAR(10),NULL)AS SYSTEMID INTO TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN"
        StrSql += vbCrLf + " WHERE ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..RECEIPT I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        StrSql += vbCrLf + " I.TRANTYPE IN('SR') AND ISNULL(I.CANCEL,'') = '' "
        StrSql += vbCrLf + " AND ISNULL(I.TAX,0)<>0"
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
        StrSql += vbCrLf + " )"
        StrSql += vbCrLf + " AND TRANTYPE IN('SR') "
        'StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = "UPDATE T SET CATCODE=I.CATCODE ,METALID=I.METALID FROM TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN T, " & cnStockDb & "..RECEIPT I "
        StrSql += vbCrLf + " WHERE I.SNO=T.ISSSNO AND I.BATCHNO=T.BATCHNO AND I.TRANTYPE=T.TRANTYPE "
        'StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = "UPDATE T SET SYSTEMID=I.SYSTEMID FROM TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN T, " & cnStockDb & "..RECEIPT I "
        StrSql += vbCrLf + " WHERE I.BATCHNO=T.BATCHNO AND I.TRANTYPE=T.TRANTYPE "
        'StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        ''END NEWLY ADDED

        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSSALESRETURN') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + "  SELECT CATCODE, "
        ElseIf rbtNodeWise.Checked = True Then
            StrSql += vbCrLf + "  SELECT SYSTEMID AS CATCODE, "
        Else
            StrSql += vbCrLf + "  SELECT METALID AS CATCODE, "
        End If
        StrSql += vbCrLf + "  CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, "
        StrSql += vbCrLf + "  RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + "  PAYMENT, AVERAGE, RATE, COLHEAD, RESULT,  RESULT1  "
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + " ,SRTYPE"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,SYSTEMID"
        StrSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN "
        StrSql += vbCrLf + "  FROM ("
        If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
            StrSql += vbCrLf + "  SELECT R.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + "  (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            End If
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
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + " ,SRTYPE"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,SYSTEMID"
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
        '*****
        If RPT_SEPSTUD_DABS Then
            StrSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
        End If
        If RPT_SEPSTUDSTN_DABS Then
            StrSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO "
            StrSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)"
        End If
        StrSql += vbCrLf + " AS AMOUNT,RATE,CATCODE,METALID"
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,R.SYSTEMID"
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
        ElseIf rbtNodeWise.Checked Then
            StrSql += vbCrLf + " GROUP BY R.CATCODE,C.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY R.METALID "
        End If
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,R.SRTYPE"
        If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,SYSTEMID"

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
            If GstFlag Then
                ''If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                ''    StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                ''    StrSql += vbCrLf + "  WHERE ACCODE = C.STAXID) AS CATNAME, "
                ''Else
                ''    StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNTAX' AS CATNAME,"
                ''End If
                ''StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                ''StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                ''StrSql += vbCrLf + "  0 AS RECEIPT, SUM(TAX+ISNULL(TT.TAXAMT,0)) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
                ''StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
                ''If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
                ''If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,R.SYSTEMID"
                ''StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
                ''StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                ''StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
                ''StrSql += vbCrLf + " ON TT.SNO=R.SNO and TT.BATCHNO=R.BATCHNO "
                ''StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                ''StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                ''StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
                ''StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
                ''StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
                ''If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                ''    StrSql += vbCrLf + "  GROUP BY R.CATCODE,C.STAXID "
                ''    If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
                ''    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
                ''    StrSql += vbCrLf + "  HAVING SUM(TAX) <> 0"

                ''Else
                ''    StrSql += vbCrLf + "  GROUP BY R.METALID  "
                ''    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
                ''End If
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_IGSTID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_IGSTID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " 0 AS RECEIPT"
                StrSql += vbCrLf + " , SUM(TAXAMOUNT) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,'' SRTYPE"
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='IG'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_IGSTID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_IGSTID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.S_IGSTID"
                End If
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_CGSTID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_CGSTID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " 0 AS RECEIPT"
                StrSql += vbCrLf + " , SUM(TAXAMOUNT) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,'' SRTYPE"
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='CG'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_CGSTID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_CGSTID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.S_CGSTID"
                End If
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_SGSTID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.S_SGSTID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " 0 AS RECEIPT"
                StrSql += vbCrLf + " , SUM(TAXAMOUNT) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,'' SRTYPE"
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='SG'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_SGSTID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.S_SGSTID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.S_SGSTID"
                End If
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT I.CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.CESSID) AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.CESSID) AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " 0 AS RECEIPT"
                StrSql += vbCrLf + " , SUM(TAXAMOUNT) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,'' SRTYPE"
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SRABSTAXTRAN I"
                StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                StrSql += vbCrLf + " WHERE I.TAXID='CS'"
                StrSql += vbCrLf + " AND ISNULL(STUDDED,'N')<>'Y'"
                If rbtCategoryWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.CESSID HAVING SUM(I.TAXAMOUNT) <> 0"
                ElseIf rbtNodeWise.Checked = True Then
                    StrSql += vbCrLf + " GROUP BY I.CATCODE, C.CESSID,SYSTEMID HAVING SUM(I.TAXAMOUNT) <> 0"
                Else
                    StrSql += vbCrLf + " GROUP BY I.METALID,C.CATNAME, C.CESSID"
                End If
                '' TCS
                StrSql += vbCrLf + " UNION ALL "
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + " SELECT 'TCS' CATCODE"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = 'TCS') AS CATNAME,"
                Else
                    StrSql += vbCrLf + " SELECT '' METALID, 'TCS' AS METALNAME"
                    StrSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = 'TCS') AS CATNAME,"
                End If
                StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + " 0 AS RECEIPT"
                StrSql += vbCrLf + " , SUM(AMOUNT) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE"
                If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,NULL AS DISCPER"
                StrSql += vbCrLf + " , CONVERT(VARCHAR(3),'') COLHEAD "
                If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,'' SRTYPE"
                If rbtNodeWise.Checked Then StrSql += vbCrLf + ",SYSTEMID"
                StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN I"
                StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
                StrSql += vbCrLf + " I.ACCODE IN ('TCS') AND ISNULL(I.CANCEL,'') = '' "
                StrSql += vbCrLf + " AND BATCHNO IN(SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
                StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
                StrSql += vbCrLf + " I.TRANTYPE IN('SA','OD','RD') AND ISNULL(I.CANCEL,'') = '' "
                StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
                StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
                StrSql += vbCrLf + " )"
                StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
                StrSql += Replace(StrUseridFtr, "USERID", "I.USERID")
                StrSql += vbCrLf + " GROUP BY I.ACCODE HAVING SUM(AMOUNT) <> 0 "
            Else
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                    StrSql += vbCrLf + "  WHERE ACCODE = C.STAXID) AS CATNAME, "
                Else
                    StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNTAX' AS CATNAME,"
                End If
                StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
                StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
                StrSql += vbCrLf + "  0 AS RECEIPT, SUM(TAX+ISNULL(TT.TAXAMT,0)) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
                StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
                If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
                If rbtNodeWise.Checked Then StrSql += vbCrLf + " ,R.SYSTEMID"
                StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
                StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                StrSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
                StrSql += vbCrLf + " ON TT.SNO=R.SNO and TT.BATCHNO=R.BATCHNO "
                StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
                StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
                StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
                If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                    StrSql += vbCrLf + "  GROUP BY R.CATCODE,C.STAXID "
                    If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
                    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
                    StrSql += vbCrLf + "  HAVING SUM(TAX) <> 0"
                Else
                    StrSql += vbCrLf + "  GROUP BY R.METALID  "
                    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
                End If
            End If
            StrSql += vbCrLf + "  UNION ALL "
            ''SURCHARGE
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.SSCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNSURCHARGE' AS CATNAME,"
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(SC) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE ,C.SSCID"
                If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID  "
            End If
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
            StrSql += vbCrLf + "  HAVING SUM(SC) <> 0 "
            StrSql += vbCrLf + "  UNION ALL "

            ''ADDTIONAL SURCHARGE
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + "  WHERE ACCODE = C.SASCID) AS CATNAME, "
            Else
                StrSql += vbCrLf + "  SELECT R.METALID, 'SALESRETURNSURCHARGE' AS CATNAME,"
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + "  0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + "  0 AS RECEIPT, SUM(ADSC) AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, "
            StrSql += vbCrLf + "  '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END AS SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "  JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + "  R.TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  GROUP BY R.CATCODE , C.SASCID "
                If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",R.SYSTEMID"
            Else
                StrSql += vbCrLf + "  GROUP BY R.METALID  "
            End If
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,CASE WHEN ISNULL(R.REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END "
            StrSql += vbCrLf + "  HAVING SUM(ADSC) <> 0 "

            StrSql += vbCrLf + "  UNION ALL "
            StrSql += vbCrLf + "  SELECT CATCODE,CATNAME, SUM(ISSPCS) AS ISSPCS, "
            StrSql += vbCrLf + "  SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += vbCrLf + "  SUM(RECPCS) AS RECPCS, SUM(RECGRSWT) AS RECGRSWT, SUM(RECNETWT) AS RECNETWT, "
            StrSql += vbCrLf + "  SUM(RECEIPT) RECEIPT, SUM(PAYMENT) PAYMENT, "
            StrSql += vbCrLf + "  RESULT, 1 AS RESULT1, AVERAGE, RATE, COLHEAD "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",SYSTEMID"
            StrSql += vbCrLf + "  FROM ("
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  SELECT (SELECT TOP 1 CATCODE FROM " & cnStockDb & "..RECEIPT "
                StrSql += vbCrLf + "  WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
                If chkCatShortname.Checked = True Then
                    StrSql += vbCrLf + "  (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
                Else
                    StrSql += vbCrLf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
                End If
                StrSql += vbCrLf + "  WHERE CATCODE = S.CATCODE) CATNAME,  "
            Else
                StrSql += vbCrLf + "  SELECT (SELECT TOP 1 METALID FROM " & cnStockDb & "..RECEIPT "
                StrSql += vbCrLf + "  WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & "))AS CATCODE, 'DIAMOND' AS CATNAME,  "
            End If
            StrSql += vbCrLf + "  0 AS ISSPCS,  0 AS ISSGRSWT, 0 AS ISSNETWT,  "
            StrSql += vbCrLf + "  SUM(S.STNPCS) AS RECPCS,  SUM(S.STNWT)  AS RECGRSWT,  SUM(S.STNWT)  AS RECNETWT,  "
            StrSql += vbCrLf + "  0 AS RECEIPT,  SUM(S.STNAMT) AS PAYMENT,  "
            StrSql += vbCrLf + "  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            StrSql += vbCrLf + "  'D' COLHEAD  "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,(SELECT TOP 1 CASE WHEN ISNULL(REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=S.BATCHNO) AS SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",S.SYSTEMID"
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE S "
            StrSql += vbCrLf + "  WHERE S.TRANTYPE='SR' AND S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + "  AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + "  " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(CANCEL,'') = ''  AND TRANTYPE = 'SR' AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + "  AND  (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            StrSql += vbCrLf + "  AND COMPANYID IN (" & SelectedCompanyId & ")"
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + "  GROUP BY S.CATCODE,S.BATCHNO  "
                If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",S.SYSTEMID"
            Else
                StrSql += vbCrLf + "  GROUP BY S.BATCHNO  "
            End If
            StrSql += vbCrLf + "  ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",SYSTEMID"
        End If


        ''If RPT_SEPSTUD_DABS Then
        ''    StrSql += vbCrLf + " UNION ALL "
        ''    StrSql += vbCrLf + " SELECT CATCODE"
        ''    If rbtCategoryWise.Checked = False Then
        ''        StrSql += vbCrLf + ",METALNAME"
        ''    End If
        ''    StrSql += vbCrLf + ", CATNAME"
        ''    StrSql += vbCrLf + ", SUM(ISSPCS) AS ISSPCS, "
        ''    StrSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
        ''    StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
        ''    StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE"
        ''    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER"
        ''    StrSql += vbCrLf + " , COLHEAD "
        ''    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
        ''    StrSql += vbCrLf + " FROM ("
        ''    If rbtCategoryWise.Checked = True Then
        ''        StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..RECEIPT "
        ''        StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
        ''        If chkCatShortname.Checked = True Then
        ''            StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
        ''        Else
        ''            StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
        ''        End If
        ''        StrSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATNAME,  "
        ''    Else
        ''        StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..RECEIPT "
        ''        ''strsql += vbcrlf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE)AS CATCODE, 'DIAMOND' AS CATNAME,  "
        ''        StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'DIAMOND' AS METALNAME,  "
        ''        StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=S.CATCODE)CATNAME, "
        ''    End If
        ''    StrSql += vbCrLf + " 0 ISSPCS,0 AS ISSGRSWT,0 AS ISSNETWT,  "
        ''    StrSql += vbCrLf + " SUM(S.STNPCS) RECPCS,  SUM(S.STNWT) AS RECGRSWT,  SUM(S.STNWT) AS RECNETWT,  0 AS RECEIPT,  "
        ''    StrSql += vbCrLf + " SUM(S.STNAMT) AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE,"
        ''    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
        ''    StrSql += vbCrLf + " 'D' COLHEAD "
        ''    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,(SELECT TOP 1 CASE WHEN ISNULL(REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=S.BATCHNO) AS SRTYPE"
        ''    StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE S "
        ''    StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
        ''    StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        ''    StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        ''    StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        ''    StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SR') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
        ''    StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
        ''    'StrSql += StrFilter
        ''    If rbtCategoryWise.Checked = True Then
        ''        StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
        ''    Else
        ''        StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE "
        ''    End If
        ''    StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
        ''    If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
        ''    If rbtCategoryWise.Checked = False Then
        ''        StrSql += vbCrLf + " ,METALNAME"
        ''    End If
        ''    If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
        ''    StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        ''End If

        If RPT_SEPSTUDSTN_DABS Then
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
            StrSql += vbCrLf + " , COLHEAD"
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",SYSTEMID"
            StrSql += vbCrLf + "  FROM ("
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..RECEIPT "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
                If chkCatShortname.Checked = True Then
                    StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
                Else
                    StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
                End If
                StrSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATNAME,  "
            Else
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALID FROM " & cnStockDb & "..RECEIPT "
                ''strsql += vbcrlf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE)AS CATCODE, 'DIAMOND' AS CATNAME,  "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ")AND S.ISSSNO = SNO)AS CATCODE, 'STONE' AS METALNAME,  "
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=S.CATCODE)CATNAME, "
            End If
            StrSql += vbCrLf + " 0 ISSPCS,0 AS ISSGRSWT,0 AS ISSNETWT,  "
            StrSql += vbCrLf + " SUM(S.STNPCS) RECPCS,  SUM(S.STNWT) AS RECGRSWT,  SUM(S.STNWT) AS RECNETWT,  0 AS RECEIPT,  "
            StrSql += vbCrLf + " SUM(S.STNAMT) AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE,"
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " NULL AS DISCPER, "
            StrSql += vbCrLf + " 'D' COLHEAD  "
            If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  ,(SELECT TOP 1 CASE WHEN ISNULL(REFNO,'')='' THEN 'NO BILL'ELSE 'BILL' END FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=S.BATCHNO) AS SRTYPE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",S.SYSTEMID"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE S "
            StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN ('SR') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'S'  "
            'StrSql += StrFilter
            If rbtCategoryWise.Checked = True Or rbtNodeWise.Checked Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
                If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",S.SYSTEMID"
            Else
                StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE "
            End If
            StrSql += vbCrLf + " ) Y GROUP BY CATCODE"
            If rbtNodeWise.Checked = True Then StrSql += vbCrLf + ",SYSTEMID"
            If rbtCategoryWise.Checked = False And rbtNodeWise.Checked = False Then
                StrSql += vbCrLf + " ,METALNAME"
            End If
            If rbtMetalwise.Checked Or rbtmetalwisegrp.Checked Then StrSql += vbCrLf + " ,DISCPER "
            StrSql += vbCrLf + " , CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        End If
        StrSql += vbCrLf + "  ) X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        'If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN(CATNAME, "
        StrSql += vbCrLf + "  ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + "  RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + "  SELECT 'SALES RETURN'CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += vbCrLf + "  0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT,  0 RESULT, 0 RESULT1, 'T' COLHEAD "
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        'Else
        'StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0 "
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN(CATNAME, "
        'StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
        'StrSql += vbCrLf + " SELECT 'SALES RETURN' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        'StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT,SUM(ISNULL(PAYMENT,0))PAYMENT, 1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
        'StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN GROUP BY CATCODE"
        ''StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        'End If

        ' If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0 "
        StrSql += vbCrLf + "  BEGIN "
        StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + "  RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + "  SELECT 'Z' AS CATCODE, 'SALES RETURN TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += vbCrLf + "  SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + "  SUM(PAYMENT), 4 RESULT, 2 RESULT1,'S' COLHEAD "
        StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT = 1 "
        If RPT_DABS_SEP_SRTYPE Then StrSql += vbCrLf + "  GROUP BY SRTYPE"
        StrSql += vbCrLf + "  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        ' End If

        If RPT_DABS_SEP_SRTYPE Then
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0"
            StrSql += vbCrLf + "  BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + "  (DESCRIPTION, COLHEAD) "
            StrSql += vbCrLf + "  SELECT 'SALES RETURN WITHOUT ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1"
            StrSql += vbCrLf + "  AND SRTYPE='NO BILL'"
            StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1 "
            StrSql += vbCrLf + "  AND SRTYPE='NO BILL'"
            StrSql += vbCrLf + "  ORDER BY RESULT1, CATCODE, RESULT"
            StrSql += vbCrLf + "  END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0"
            StrSql += vbCrLf + "  BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + "  (DESCRIPTION, COLHEAD) "
            StrSql += vbCrLf + "  SELECT 'SALES RETURN WITH BILL ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1"
            StrSql += vbCrLf + "  AND SRTYPE='BILL'"
            StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1 "
            StrSql += vbCrLf + "  AND SRTYPE='BILL'"
            StrSql += vbCrLf + "  ORDER BY RESULT1, CATCODE, RESULT"
            StrSql += vbCrLf + "  END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN)>0"
            StrSql += vbCrLf + "  BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + "  SELECT 'SALES RETURN ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + "  SELECT 'SALES RETURN ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
            End If
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT1 = 1"
            StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN "
            StrSql += vbCrLf + "  WHERE RESULT1 = 1 "
            StrSql += vbCrLf + "  ORDER BY RESULT1, CATCODE, RESULT"
            StrSql += vbCrLf + "  END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

    End Sub

    Private Sub ProcPurchase()
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPURCHASE') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE"
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT CATCODE, "
        Else
            StrSql += vbCrLf + " SELECT METALID AS CATCODE, "
        End If
        StrSql += vbCrLf + " CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, "
        StrSql += vbCrLf + " RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + " PAYMENT, AVERAGE, RATE, COLHEAD, RESULT,  RESULT1  INTO TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE FROM ("
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " SELECT R.CATCODE, "
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
            End If
        Else
            StrSql += vbCrLf + " SELECT R.METALID, "
            StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = R.METALID)AS CATNAME, "
        End If
        StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
        StrSql += vbCrLf + " SUM(PCS) AS RECPCS, SUM(GRSWT) AS RECGRSWT, SUM(NETWT) AS RECNETWT, "
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
        If RPT_SEPSTUD_DABS Then
            StrSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
        End If
        If RPT_SEPSTUDSTN_DABS Then
            StrSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)"
        End If
        StrSql += vbCrLf + " AS AMOUNT,RATE,CATCODE,METALID"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R"
        StrSql += vbCrLf + " WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " TRANTYPE = 'PU' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
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
        '    StrSql += VBCRLF + " 0 AS RECEIPT, SUM(AMOUNT-ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0)) "
        '    StrSql += VBCRLF + " AS PAYMENT, 1 AS RESULT, 1 AS RESULT1, "
        '    StrSql += VBCRLF + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        '    StrSql += VBCRLF + " (SUM(AMOUNT-ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0))/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        '    StrSql += VBCRLF + " ELSE "
        '    StrSql += VBCRLF + " (SUM(AMOUNT-ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0))/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        '    StrSql += VBCRLF + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        'Else
        '    StrSql += VBCRLF + " 0 AS RECEIPT, SUM(AMOUNT) "
        '    'If rbtCategoryWise.Checked Then
        '    '    StrSql += VBCRLF + " - ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE "
        '    '    StrSql += VBCRLF + " ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT "
        '    '    StrSql += VBCRLF + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'  AND  "
        '    '    StrSql += VBCRLF + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND "
        '    '    StrSql += VBCRLF + " CATCODE = R.CATCODE AND ISNULL(CANCEL,'') = '' AND COMPANYID IN (" & SelectedCompanyId & "))"
        '    '    StrSql += VBCRLF + " " & StrFilter & " AND TRANTYPE = 'PU'  AND COMPANYID IN (" & SelectedCompanyId & ")"
        '    '    StrSql += VBCRLF + " AND CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = 'D' AND CATCODE = R.CATCODE)),0)"
        '    'Else
        '    '    StrSql += VBCRLF + " - ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE "
        '    '    StrSql += VBCRLF + " ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT "
        '    '    StrSql += VBCRLF + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'  AND  "
        '    '    StrSql += VBCRLF + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        '    '    StrSql += VBCRLF + " " & StrFilter & " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
        '    '    StrSql += VBCRLF + " WHERE METALID = R.METALID) AND ISNULL(CANCEL,'') = '')"
        '    '    StrSql += VBCRLF + " AND TRANTYPE = 'PU'  AND COMPANYID IN (" & SelectedCompanyId & ") ),0)"
        '    '    ''StrSql += VBCRLF + " AND CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = 'D' AND CATCODE = R.CATCODE)),0)"
        '    'End If
        '    StrSql += VBCRLF + " AS PAYMENT, 1 AS RESULT, 1 AS RESULT1, "
        '    StrSql += VBCRLF + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        '    StrSql += VBCRLF + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        '    StrSql += VBCRLF + " ELSE "
        '    StrSql += VBCRLF + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        '    StrSql += VBCRLF + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        'End If

        'StrSql += VBCRLF + " FROM " & cnStockDb & "..RECEIPT R"
        'StrSql += VBCRLF + " WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        'StrSql += VBCRLF + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        'StrSql += VBCRLF + " TRANTYPE = 'PU' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
        'StrSql += StrFilter
        'If rbtCategoryWise.Checked = True Then
        '    StrSql += VBCRLF + " GROUP BY CATCODE,CASHID "
        'Else
        '    StrSql += VBCRLF + " GROUP BY METALID,CASHID "
        'End If
        If RPT_SEPVAT_DABS Then
            StrSql += vbCrLf + " UNION ALL"
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.PTAXID) AS CATNAME, "
            Else
                StrSql += vbCrLf + " SELECT R.METALID, 'PURCHASETAX' AS CATNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(TAX) AS RECEIPT,0 PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
            StrSql += vbCrLf + " WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + " R.TRANTYPE = 'PU' AND ISNULL(R.CANCEL,'') = ''  AND R.COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY R.CATCODE, C.PTAXID "
            Else
                StrSql += vbCrLf + " GROUP BY R.METALID "
            End If
            StrSql += vbCrLf + " HAVING SUM(TAX) > 0"
            StrSql += vbCrLf + " UNION ALL"

            StrSql += vbCrLf + " SELECT CATCODE,CATNAME, SUM(ISSPCS) AS ISSPCS, "
            StrSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
            StrSql += vbCrLf + " SUM(RECPCS) AS RECPCS, SUM(RECGRSWT) AS RECGRSWT, SUM(RECNETWT) AS RECNETWT, "
            StrSql += vbCrLf + " SUM(RECEIPT) RECEIPT, SUM(PAYMENT) PAYMENT, "
            StrSql += vbCrLf + " RESULT, 1 AS RESULT1, AVERAGE, RATE, COLHEAD FROM ("
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..RECEIPT "
                StrSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
                If chkCatShortname.Checked = True Then
                    StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
                Else
                    StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
                End If
                StrSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATNAME,  "
            Else
                StrSql += vbCrLf + " SELECT M.METALID AS CATCODE, M.METALNAME AS CATNAME,  "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS,  0 AS ISSGRSWT, 0 AS ISSNETWT,  "
            StrSql += vbCrLf + " SUM(S.STNPCS) AS RECPCS,  SUM(S.STNWT)  AS RECGRSWT,  SUM(S.STNWT)  AS RECNETWT,  "
            StrSql += vbCrLf + " 0 AS RECEIPT,  SUM(S.STNAMT) AS PAYMENT,  "
            StrSql += vbCrLf + " 2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
            StrSql += vbCrLf + " 'D' COLHEAD  FROM " & cnStockDb & "..RECEIPTSTONE S "
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON S.STNITEMID =I.ITEMID "
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID=M.METALID"
            StrSql += vbCrLf + " WHERE S.TRANTYPE='PU' AND S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " " & Replace(StrFilter, "SYSTEMID", "S.SYSTEMID") & " AND ISNULL(CANCEL,'') = ''  AND TRANTYPE = 'PU' AND COMPANYID IN (" & SelectedCompanyId & "))"
            StrSql += vbCrLf + " AND  (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
            StrSql += vbCrLf + " AND S.COMPANYID IN (" & SelectedCompanyId & ")"
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
            Else
                StrSql += vbCrLf + " GROUP BY S.BATCHNO,M.METALID,M.METALNAME  "
            End If
            StrSql += vbCrLf + " ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        End If

        StrSql += vbCrLf + " ) X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        'If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE(CATNAME, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'PURCHASE'CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += vbCrLf + " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, 0 RESULT, 0 AS RESULT1, 'T' COLHEAD "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        'Else
        'StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE)>0 "
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE(CATNAME, "
        'StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        'StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD,CATCODE) "
        'StrSql += vbCrLf + " SELECT 'PURCHASE' CATNAME,SUM(ISNULL(ISSPCS,0))ISSPCS,SUM(ISNULL(ISSGRSWT,0))ISSGRSWT,SUM(ISNULL(ISSNETWT,0))ISSNETWT, "
        'StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0))RECPCS,SUM(ISNULL(RECGRSWT,0))RECGRSWT,SUM(ISNULL(RECNETWT,0))RECNETWT,"
        'StrSql += vbCrLf + " SUM(ISNULL(RECEIPT,0))RECEIPT,SUM(ISNULL(PAYMENT,0))PAYMENT, 1 RESULT,1 RESULT1, 'L' COLHEAD,CATCODE "
        'StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE GROUP BY CATCODE"
        ''StrSql += vbCrLf + " WHERE COLHEAD <> 'D'"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        'End If


        'If rbtmetalwisegrp.Checked = False Then
        StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'Z' AS CATCODE, 'PURCHASE TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + " SUM(PAYMENT),4 RESULT, 2 AS RESULT1, 'S' COLHEAD "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE WHERE RESULT = 1 "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        ' End If

        ' If rbtmetalwisegrp.Checked = False Then
        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE)>0"
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        If chkWithHeaderTotal.Checked Then
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
            StrSql += vbCrLf + "  SELECT 'PURCHASE ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
            StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
        Else
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)  "
            StrSql += vbCrLf + "  SELECT 'PURCHASE ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
        End If
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE WHERE RESULT1 = 1"
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
        StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, COLHEAD, RESULT1  "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE "
        'StrSql += vbCrLf + " WHERE RESULT = 1 "
        StrSql += vbCrLf + " ORDER BY CATCODE, RESULT"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        'Else
        'StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES)>0"
        'StrSql += vbCrLf + " BEGIN "
        ''StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        ' ''strsql += vbcrlf + " (DESCRIPTION, COLHEAD) VALUES('SALES','T') "
        ''StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) "
        ''StrSql += vbCrLf + " SELECT 'SALES ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' ,'T' "
        ''StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES "
        ''StrSql += VBCRLF + " WHERE RESULT1 = 1"
        ''StrSql += VBCRLF + " AND COLHEAD <> 'D'"
        'StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
        'StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, COLHEAD,CATCODE, RESULT1,6 "
        'StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE "
        'StrSql += vbCrLf + " WHERE RESULT1 = 1 AND COLHEAD='L'"
        'StrSql += vbCrLf + " ORDER BY CATCODE,RESULT"
        'StrSql += vbCrLf + " END "
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        'End If

    End Sub



    Private Sub ProcHomeSales()
        If Not chkHomeSale.Checked Then Exit Sub
        ' If rbtmetalwisegrp.Checked = False Then
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSHOMESALE') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSHOMESALE "
        ''StrSql += VBCRLF + " SELECT ' BILLNO-'+CONVERT(VARCHAR(5),TRANNO) "
        ''StrSql += VBCRLF + " AS CATNAME, "
        ''StrSql += VBCRLF + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT,  "
        ''StrSql += VBCRLF + " 1 AS RESULT1, ' ' AS COLHEAD "
        ''StrSql += VBCRLF + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSHOMESALE FROM " & cnStockDb & "..ISSUE I  "
        ''StrSql += VBCRLF + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'  AND ' "
        ''StrSql += dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        ''StrSql += VBCRLF + " AND  TRANTYPE = 'SA' AND ISNULL(I.CANCEL,'') = ''  "
        ''StrSql += VBCRLF + " AND ISNULL(FLAG,'') IN ('C', 'B')"
        ''StrSql += VBCRLF + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        ''StrSql += VBCRLF + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(STOCKTYPE,'') = 'T')"
        ''StrSql += VBCRLF + " GROUP BY BATCHNO,TRANNO,TRANDATE  "
        StrSql += vbCrLf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)CATNAME/**/"
        StrSql += vbCrLf + " ,SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT,/**/"
        StrSql += vbCrLf + " 1 AS RESULT1, CONVERT(VARCHAR,'') AS COLHEAD/**/"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSHOMESALE FROM " & cnStockDb & "..ISSUE I/**/"
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'/**/"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND  TRANTYPE = 'SA'/**/"
        StrSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''   AND ISNULL(FLAG,'') IN ('C', 'B') AND COMPANYID IN (" & SelectedCompanyId & ")/**/"
        StrSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(STOCKTYPE,'') = 'T')/**/"
        StrSql += vbCrLf + " AND ISNULL(TAGNO,'') = ''/**/ " + StrFilter + StrUseridFtr
        StrSql += vbCrLf + " GROUP BY CATCODE/**/"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        'Else
        '    StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMPTABLEDB..TEMP" & systemId & "ABSHOMESALE') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSHOMESALE "
        '    StrSql += VBCRLF + " SELECT METALID,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)CATNAME/**/"
        '    StrSql += VBCRLF + " ,SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT,/**/"
        '    StrSql += VBCRLF + " 1 AS RESULT1, CONVERT(VARCHAR,'') AS COLHEAD/**/"
        '    StrSql += VBCRLF + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSHOMESALE FROM " & cnStockDb & "..ISSUE I/**/"
        '    StrSql += VBCRLF + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'/**/"
        '    StrSql += VBCRLF + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND  TRANTYPE = 'SA'/**/"
        '    StrSql += VBCRLF + " AND ISNULL(I.CANCEL,'') = ''   AND ISNULL(FLAG,'') IN ('C', 'B') AND COMPANYID IN (" & SelectedCompanyId & ")/**/"
        '    StrSql += VBCRLF + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(STOCKTYPE,'') = 'T')/**/"
        '    StrSql += VBCRLF + " AND ISNULL(TAGNO,'') = ''/**/ " + StrFilter
        '    StrSql += VBCRLF + " GROUP BY CATCODE,METALID/**/"
        '    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        '    Cmd.ExecuteNonQuery()

        'End If

        ' If rbtmetalwisegrp.Checked = False Then
        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSHOMESALE)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) VALUES('HOME SALES','T') "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
        StrSql += vbCrLf + " CASE WHEN ISSPCS    <> 0 THEN ISSPCS    ELSE NULL END ISSPCS, "
        StrSql += vbCrLf + " CASE WHEN ISSGRSWT  <> 0 THEN ISSGRSWT  ELSE NULL END ISSGRSWT, "
        StrSql += vbCrLf + " CASE WHEN ISSNETWT  <> 0 THEN ISSNETWT  ELSE NULL END ISSNETWT, "
        StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSHOMESALE ORDER BY RESULT1"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        'Else
        '    StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSHOMESALE)>0 "
        '    StrSql += VBCRLF + " BEGIN "
        '    StrSql += VBCRLF + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        '    StrSql += VBCRLF + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,CATCODE,COLHEAD,ORDERNO) "
        '    StrSql += VBCRLF + " SELECT '  HOME SALES' CATNAME, "
        '    StrSql += VBCRLF + " CASE WHEN SUM(ISNULL(ISSPCS,0))  <> 0 THEN SUM(ISNULL(ISSPCS,0)) ELSE NULL END ISSPCS, "
        '    StrSql += VBCRLF + " CASE WHEN SUM(ISNULL(ISSGRSWT,0))  <> 0 THEN SUM(ISNULL(ISSGRSWT,0))  ELSE NULL END ISSGRSWT, "
        '    StrSql += VBCRLF + " CASE WHEN SUM(ISNULL(ISSNETWT,0))  <> 0 THEN SUM(ISNULL(ISSNETWT,0))  ELSE NULL END ISSNETWT, "
        '    StrSql += VBCRLF + " 1 RESULT1,METALID,'L' COLHEAD,3  "
        '    StrSql += VBCRLF + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSHOMESALE GROUP BY METALID,COLHEAD ORDER BY RESULT1"
        '    StrSql += VBCRLF + " END "
        '    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        '    Cmd.ExecuteNonQuery()
        'End If

    End Sub

    Private Sub ProcMiscIssue()
        If (RPT_ME_SUM_DABS = True And rbtMetalwise.Checked = True) Then Exit Sub
        If RPT_MIS_ISS_DABS = False Then Exit Sub
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSMISCISSUE') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSMISCISSUE"
        If rbtCategoryWise.Checked = True Then
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + " SELECT CATCODE, "
                StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + " SELECT CATCODE, "
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            End If
        Else
            StrSql += vbCrLf + " SELECT METALID, "
            StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
            StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME,"
        End If
        StrSql += vbCrLf + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += vbCrLf + " 0 AS RECEIPT, 0 AS PAYMENT, 1 AS RESULT1, ' ' COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSMISCISSUE FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " TRANTYPE = 'MI' AND ISNULL(I.CANCEL,'') = ''  AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        If rbtCategoryWise.Checked = True Then
            StrSql += vbCrLf + " GROUP BY CATCODE "
        Else
            StrSql += vbCrLf + " GROUP BY METALID,CATCODE "
        End If
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        If rbtmetalwisegrp.Checked = False Then
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSMISCISSUE)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) VALUES('MISC ISSUE','T') "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,COLHEAD,RESULT1) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " COLHEAD, RESULT1 "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSMISCISSUE "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " ORDER BY CATCODE,RESULT1"
            Else
                StrSql += vbCrLf + " ORDER BY CATNAME,RESULT1"
            End If
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSMISCISSUE)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU (DESCRIPTION,"
            StrSql += vbCrLf + " COLHEAD,CATCODE,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT '  MISC ISSUE', "
            StrSql += vbCrLf + " 'L' COLHEAD,METALID, RESULT1,3,'C' COLHEAD1"
            StrSql += vbCrLf + "   FROM TEMPTABLEDB..TEMP" & systemId & "ABSMISCISSUE"
            StrSql += vbCrLf + "  ORDER BY COLHEAD "

            'StrSql += VBCRLF + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            'StrSql += VBCRLF + " (DESCRIPTION, COLHEAD) VALUES('MISC ISSUE','T') "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,COLHEAD,CATCODE,RESULT1,ORDERNO) "
            StrSql += vbCrLf + " SELECT CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 'L' COLHEAD,METALID, RESULT1,4 "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSMISCISSUE "
            StrSql += vbCrLf + " ORDER BY CATNAME,RESULT1"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSMISCISSUE)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,METALID,'L' COLHEAD,1,5,'M' COLHEAD1  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSMISCISSUE GROUP BY METALID,COLHEAD,RESULT1"
            StrSql += vbCrLf + " ORDER BY COLHEAD"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

        End If
    End Sub

    Private Sub ProcAdvDueSummary()
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVDUE')"
        StrSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ADVDUE"
        StrSql += vbCrLf + " SELECT 'OPE' SEP"
        StrSql += vbCrLf + " ,CASE WHEN SUBSTRING(RUNNO,6,1) IN ('O','R') THEN 'O' ELSE TRANTYPE END TRANTYPE"
        'StrSql += vbCrLf + " ,CASE WHEN TRANTYPE IN ('D','J') THEN CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END ELSE CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END END AS AMOUNT"
        StrSql += vbCrLf + " ,CASE WHEN TRANTYPE IN ('D','J') THEN CASE WHEN RECPAY = 'P' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE -1*(AMOUNT+ISNULL(GSTVAL,0)) END ELSE CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE -1*(AMOUNT+ISNULL(GSTVAL,0)) END END AS AMOUNT"
        StrSql += vbCrLf + " ,BATCHNO"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ADVDUE"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' AND ISNULL(COMPANYID,'') IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not chkMiscRecPaySummary.Checked Then StrSql += vbCrLf + " AND TRANTYPE <> 'T'"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT 'REC' SEP,CASE WHEN SUBSTRING(RUNNO,6,1) IN ('O','R') THEN 'O' ELSE TRANTYPE END TRANTYPE,CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END AS AMOUNT,BATCHNO"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND RECPAY = 'R' AND ISNULL(CANCEL,'') = '' AND ISNULL(COMPANYID,'') IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not chkMiscRecPaySummary.Checked Then StrSql += vbCrLf + " AND TRANTYPE <> 'T'"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT 'PAY' SEP,CASE WHEN SUBSTRING(RUNNO,6,1) IN ('O','R') THEN 'O' ELSE TRANTYPE END TRANTYPE,CASE WHEN RECPAY = 'P' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END AS AMOUNT,BATCHNO"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND RECPAY = 'P' AND ISNULL(CANCEL,'') = '' AND ISNULL(COMPANYID,'') IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not chkMiscRecPaySummary.Checked Then StrSql += vbCrLf + " AND TRANTYPE <> 'T'"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "ADVDUE SET TRANTYPE = 'J'"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ADVDUE AS O INNER JOIN " & cnAdminDb & "..ITEMDETAIL I ON I.BATCHNO = O.BATCHNO AND O.TRANTYPE ='D'"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVDUESUMMARY')"
        StrSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ADVDUESUMMARY"
        StrSql += vbCrLf + " SELECT "
        StrSql += vbCrLf + " CASE TRANTYPE WHEN 'O' THEN 'ORDER ADVANCE' WHEN 'D' THEN 'DUE' WHEN 'T' THEN 'OTHERS' WHEN 'J' THEN 'JND' WHEN 'GV' THEN 'GIFT VOUCHER' ELSE 'ADVANCE' END AS TRANTYPE"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN AMOUNT ELSE 0 END) AS OPENING"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN AMOUNT ELSE 0 END)AS RECEIPT"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'PAY' THEN AMOUNT ELSE 0 END)AS PAYMENT"
        StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'PAY' THEN -1*AMOUNT ELSE AMOUNT END)AS CLOSING"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ADVDUESUMMARY"
        StrSql += vbCrLf + " FROM"
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " SELECT SEP,TRANTYPE,AMOUNT FROM TEMPTABLEDB..TEMP" & systemId & "ADVDUE WHERE TRANTYPE NOT IN ('D','J')"
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
        StrSql += vbCrLf + " SELECT SEP,TRANTYPE,AMOUNT FROM TEMPTABLEDB..TEMP" & systemId & "ADVDUE WHERE TRANTYPE IN ('D','J')"
        StrSql += vbCrLf + " )X"
        StrSql += vbCrLf + " GROUP BY TRANTYPE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcSASRPU(ByVal DotMatrix As Boolean)
        PROCSAPSMI()
        ProcSales()
        If RPT_SEPORD_DABS = True Then ProcSalesAndOrder()
        ProcRepairDelivery()
        ProcSalesReturn()
        ProcPurchase()
        If RPT_SEPORD_DABS = True Then ProcOrderBooking()
        ProcOrderAdvanceAmountToWeight()
        ProcMiscIssue()
        If chkDetaledRecPay.Checked = False Then
            ''EMPTY LINE
            StrSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
        ProcCreditSales()
        ProcCreditPurchase()
        ProcCreditAdjustment()
        ProcCreditPurchasePayment()
        ProcAdvanceReceived()
        ProcGiftVoucher()
        ProcAdvanceAdjustment()
        ProcOrderAdvance()
        ProcOrderAdvanceAdjusted()
        ProcRepairAdvance()
        ProcRepairAdvanceAdjusted()
        ProcChitPayment()
        ProcMiscReceipt()
        ProcMiscPayment()

        ''EMPTY LINE
        StrSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
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
        ProcStockDetail()
        ProcLOTDetail()
        ProcHomeSales()
        ProcPartlySales()
        ProcPartlySales_AI()
        ProcWsSalesamt()
        ProcChitCollection()
        If chkRecPay.Checked Then
            ProcAccReceipt()
            ProcAccPayment()
        End If
        If RPT_GSTREGISTER_CASH_VIEW Then ProcGstRegister_Cash()
        ProcWtSubtot()

        ProcCollection()
        If chkWithEditedtags.Checked = True Then
            ProcTagEdited()
        End If
        ProcAmtSubtot()
        If RPT_GSTREGISTER_VIEW Then ProcGstRegister()
        If Chk_FinDiscount.Checked Then ProcDiscount_Fin()
        If chkAdvdueSummary.Checked Then ProcAdvDueSummary()
        If chkCancelBills.Checked Then ProcCancelBills()
        If chkPrivilegePoints.Checked Then ProcPrivilegePoints()
        ProcPRIVILEGE()
        If RPT_SEPVAT_Cess = True Then
            ProcCess()
        End If
        If rbtmetalwisegrp.Checked = False And rbtNodeWise.Checked = False Then
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT"
            StrSql += vbCrLf + " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COLHEAD) "
            StrSql += vbCrLf + " SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,DISCPER,COLHEAD"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU ORDER BY SNO" 'WHERE ISNULL(RESULT1,1) NOT IN (0,2) ORDER BY SNO"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        ElseIf rbtNodeWise.Checked = True Then
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT"
            StrSql += vbCrLf + " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,CATCODE,COLHEAD,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT  CATCODE DESCRIPTION,"
            StrSql += vbCrLf + " NULL ISSPCS,NULL ISSGRSWT,NULL ISSNETWT,NULL RECPCS,NULL RECGRSWT,NULL RECNETWT,NULL RECEIPT,NULL PAYMENT,0 AVERAGE,NULL DISCPER,CATCODE,'T' COLHEAD,0 ORDERNO,'' COLHEAD1"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU WHERE CATCODE<>'' GROUP BY CATCODE"
            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,NULL DISCPER,CATCODE,COLHEAD,ORDERNO, COLHEAD1  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU WHERE CATCODE<>'' GROUP BY CATCODE,DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,"
            StrSql += vbCrLf + " RECEIPT, PAYMENT, AVERAGE,DISCPER, COLHEAD,ORDERNO,COLHEAD1"
            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT 'ISSUE TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,NULL DISCPER,'9999' CATCODE,'S' COLHEAD,13 ORDERNO,'M ' COLHEAD1  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU WHERE CATCODE<>'' AND COLHEAD1='M' GROUP BY /*CATCODE,*/COLHEAD,COLHEAD1"
            StrSql += vbCrLf + " ORDER BY CATCODE,ORDERNO"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT"
            StrSql += vbCrLf + " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COLHEAD,ORDERNO) "
            StrSql += vbCrLf + " SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,NULL DISCPER,COLHEAD,8 "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU  WHERE COLHEAD<>'L' OR COLHEAD IS NULL ORDER BY SNO"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT"
            StrSql += vbCrLf + " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,CATCODE,COLHEAD,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT (SELECT DISTINCT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = CATCODE)AS DESCRIPTION,"
            StrSql += vbCrLf + " 0 ISSPCS,0 ISSGRSWT,0 ISSNETWT,0 RECPCS,0 RECGRSWT,0 RECNETWT,0 RECEIPT,0 PAYMENT,0 AVERAGE,0 DISCPER,CATCODE,'T' COLHEAD,0 ORDERNO,'' COLHEAD1"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU WHERE CATCODE<>'' GROUP BY CATCODE"
            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,DISCPER,CATCODE,COLHEAD,ORDERNO, COLHEAD1  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU WHERE CATCODE<>'' GROUP BY CATCODE,DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,"
            StrSql += vbCrLf + " RECEIPT, PAYMENT, AVERAGE,DISCPER, COLHEAD,ORDERNO,COLHEAD1"
            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT 'ISSUE TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),SUM(ISNULL(RECEIPT,0)),"
            StrSql += vbCrLf + " SUM(ISNULL(PAYMENT,0)),'' AVERAGE,NULL DISCPER,CATCODE,'S' COLHEAD,13 ORDERNO,'M ' COLHEAD1  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU WHERE CATCODE<>'' AND COLHEAD1='M' GROUP BY CATCODE,COLHEAD,COLHEAD1"
            StrSql += vbCrLf + " ORDER BY CATCODE,ORDERNO"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT"
            StrSql += vbCrLf + " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COLHEAD,ORDERNO) "
            StrSql += vbCrLf + " SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,DISCPER,COLHEAD,8 "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU  WHERE COLHEAD<>'L' OR COLHEAD IS NULL ORDER BY SNO"
            'StrSql += VBCRLF + " SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD "
            'StrSql += VBCRLF + " FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU ORDER BY SNO" 'WHERE ISNULL(RESULT1,1) NOT IN (0,2) ORDER BY SNO"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
        If chkAdvdueSummary.Checked Then

            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ADVDUESUMMARY)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT(COL1,COLHEAD) VALUES('ADV-DUE SUMMARY','T')"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT"
            StrSql += vbCrLf + " (COL1,COL6,COL7,COL8,COL9,COLHEAD,ORDERNO) "
            StrSql += vbCrLf + " SELECT ' ','OPENING','RECEIPT','PAYMENT','CLOSING','S',9 ORDERNO"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT"
            StrSql += vbCrLf + " (COL1,COL6,COL7,COL8,COL9,ORDERNO) "
            StrSql += vbCrLf + " SELECT ' ' + TRANTYPE, "
            StrSql += vbCrLf + " CASE WHEN OPENING   <> 0 THEN OPENING   ELSE NULL END OPENING, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " CASE WHEN CLOSING   <> 0 THEN CLOSING  ELSE NULL END CLOSING,9 ORDERNO "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ADVDUESUMMARY "
            If RPT_HIDE_GIFTOTHERS Then
                StrSql += vbCrLf + " WHERE TRANTYPE NOT IN('OTHERS','GIFT VOUCHER') "
            End If
            StrSql += vbCrLf + " ORDER BY TRANTYPE"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT"
            StrSql += vbCrLf + " (COL1,COL6,COL7,COL8,COL9,COLHEAD,ORDERNO) "
            StrSql += vbCrLf + " SELECT 'TOTAL',SUM(OPENING),SUM(RECEIPT),SUM(PAYMENT),SUM(CLOSING),'S',9 "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ADVDUESUMMARY "
            If RPT_HIDE_GIFTOTHERS Then
                StrSql += vbCrLf + " WHERE TRANTYPE NOT IN('OTHERS','GIFT VOUCHER') "
            End If
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        If chkCancelBills.Checked Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSCANCELBILL )>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT(COL1,COLHEAD) VALUES('CANCEL BILLS','T')"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT"
            StrSql += vbCrLf + " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,ORDERNO) "
            StrSql += vbCrLf + " SELECT '  ' + TRANS, "
            StrSql += vbCrLf + " CASE WHEN IPCS   <> 0 THEN IPCS   ELSE NULL END IPCS, "
            StrSql += vbCrLf + " CASE WHEN IGRSWT <> 0 THEN IGRSWT ELSE NULL END IGRSWT, "
            StrSql += vbCrLf + " CASE WHEN INETWT <> 0 THEN INETWT ELSE NULL END INETWT, "
            StrSql += vbCrLf + " CASE WHEN RPCS   <> 0 THEN RPCS   ELSE NULL END RPCS, "
            StrSql += vbCrLf + " CASE WHEN RGRSWT <> 0 THEN RGRSWT ELSE NULL END RGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RNETWT <> 0 THEN RNETWT ELSE NULL END RNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT,10 ORDERNO FROM "
            StrSql += vbCrLf + " TEMPTABLEDB..TEMP" & systemId & "ABSCANCELBILL ORDER BY TRANTYPE, TRANNO"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
        If RPT_CNLINCL_DABS Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSCANCELBILL )=0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT(COL1,COLHEAD) VALUES('CANCEL BILLS','T')"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SALEABSTRACT"
            StrSql += vbCrLf + " (COL1,COL2) "
            StrSql += vbCrLf + " VALUES('  NO CANCEL TRANSACTION' ,'NIL') END "
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
            'StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU)>0 "
            'StrSql += VBCRLF + " BEGIN "
            '.Columns("AVERAGE").MaxLength = objGPack.GetSqlValue(StrSql & "SELECT MAX(LEN(ISNULL(AVERAGE,0))) AS AA FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU END", , "-1")
            '.Columns("COLHEAD").MaxLength = objGPack.GetSqlValue(StrSql & "SELECT MAX(LEN(ISNULL(COLHEAD,0))) AS AA FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU END", , "-1")
        End With

        StrSql = "SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT, "
        StrSql += vbCrLf + " AVERAGE, COLHEAD AS COLHEAD,COLHEAD1 FROM TEMPTABLEDB..TEMP" & systemId & "SASRPU WHERE COLHEAD<>'L' ORDER BY SNO, RESULT1 "
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dt)
        'dt.Columns.Remove("AVERAGE")
        dt.AcceptChanges()
        If dt.Rows.Count > 0 Then dsReportCol.Tables.Add(dt)
    End Sub

    Private Sub ProcCreditSales()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDSAL') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSCREDSAL "
        StrSql += vbCrLf + " SELECT SUBSTRING(O.RUNNO,1,20) + ' - ' + (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO)) "
        'StrSql += vbCrLf + " ISNULL((select top 1 '(JND)' from " & cnAdminDb & "..ITEMDETAIL where batchno=o.batchno),'')+"
        '  StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) + ' ' +'- REFNO: '+SUBSTRING(O.RUNNO,3,20) +')' AS CATNAME,  "
        'StrSql += vbCrLf + " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += vbCrLf + " AS CATNAME,  "
        StrSql += vbCrLf + " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END) AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, 'D' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSCREDSAL FROM " & cnAdminDb & "..OUTSTANDING O"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(O.CANCEL,'') = ''  "
        StrSql += vbCrLf + " AND O.COMPANYID IN (" & SelectedCompanyId & ") AND O.RECPAY = 'P' AND O.PAYMODE = 'DU'"
        StrSql += vbCrLf + " AND O.BATCHNO NOT IN (select BATCHNO from " & cnAdminDb & "..ITEMDETAIL WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(O.CANCEL,'') = '') "
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.TRANNO,O.RUNNO,O.TRANDATE"
        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT SUBSTRING(O.RUNNO,1,20) + ' - ' + (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO)) "
        'StrSql += vbCrLf + " ISNULL((select top 1 '(JND)' from " & cnAdminDb & "..ITEMDETAIL where batchno=o.batchno),'')+"
        ' StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +' ' +'- REFNO: '+SUBSTRING(O.RUNNO,3,20) +')' AS CATNAME,  "
        StrSql += vbCrLf + " AS CATNAME,  "
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
        StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.TRANNO,O.RUNNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDSAL)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'CREDIT BILL(DUE) ['+CONVERT(VARCHAR,ISNULL(SUM(ISNULL(PAYMENT,0)-ISNULL(RECEIPT,0)),0)) +']'"
                StrSql += vbCrLf + " ,'T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'CREDIT BILL(DUE) ['+CONVERT(VARCHAR,ISNULL(SUM(ISNULL(PAYMENT,0)-ISNULL(RECEIPT,0)),0)) +']'"
                StrSql += vbCrLf + " ,'T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDSAL WHERE COLHEAD = 'D'"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, '' COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDSAL WHERE COLHEAD = 'D'"
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += vbCrLf + " SELECT 'CREDIT BILL(DUE)'"
            StrSql += vbCrLf + " ,'T',SUM(PAYMENT)-SUM(RECEIPT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDSAL WHERE COLHEAD = 'D'"
        End If

        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'CREDIT BILL(JND) ['+CONVERT(VARCHAR,ISNULL(SUM(ISNULL(PAYMENT,0)-ISNULL(RECEIPT,0)),0)) +']'"
                StrSql += vbCrLf + " ,'T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'CREDIT BILL(JND) ['+CONVERT(VARCHAR,ISNULL(SUM(ISNULL(PAYMENT,0)-ISNULL(RECEIPT,0)),0)) +']'"
                StrSql += vbCrLf + " ,'T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDSAL WHERE COLHEAD ='J'"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, '' COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDSAL WHERE COLHEAD ='J'"
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += vbCrLf + " SELECT 'CREDIT BILL(JND)'"
            StrSql += vbCrLf + " ,'T',SUM(PAYMENT)-SUM(RECEIPT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDSAL WHERE COLHEAD ='J'"
        End If


        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcCreditPurchase()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDPUR') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSCREDPUR"
        StrSql += vbCrLf + " SELECT SUBSTRING(O.RUNNO,1,20) + ' - ' + (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO)) "
        'StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) + ' ' +'- REFNO: '+SUBSTRING(O.RUNNO,3,20)+')' AS CATNAME,  "
        StrSql += vbCrLf + " AS CATNAME,  "
        StrSql += vbCrLf + " SUM(CASE WHEN O.RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSCREDPUR FROM " & cnAdminDb & "..OUTSTANDING O"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''  "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND O.RECPAY = 'R' AND PAYMODE IN('DU','RP')"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.TRANNO,O.RUNNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDPUR)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'CREDIT PURCHASE(DUE) [' + CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),'')) + ']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'CREDIT PURCHASE(DUE) [' + CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),'')) + ']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDPUR"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDPUR"
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,COLHEAD,RECEIPT) "
            StrSql += vbCrLf + " SELECT 'CREDIT PURCHASE(DUE)','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDPUR"
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcCreditAdjustment()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDADJ') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSCREDADJ "
        StrSql += vbCrLf + " SELECT SUBSTRING(O.RUNNO,1,20) + ' - ' + (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO  WHERE BATCHNO = O.BATCHNO)) "
        'StrSql += vbCrLf + " ISNULL((select top 1 '(JND)' from " & cnAdminDb & "..ITEMDETAIL where runno=o.runno),'')+"
        '        StrSql += VBCRLF + " '(INV NO: '+SUBSTRING(O.RUNNO,6,20) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        ' StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +' - ADJ NO: '+SUBSTRING(O.RUNNO,3,20) +')' AS CATNAME,  "
        StrSql += vbCrLf + " AS CATNAME,  "
        StrSql += vbCrLf + " SUM(CASE WHEN O.RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, 'D' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSCREDADJ FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + "  AND O.RECPAY = 'R' AND COMPANYID IN (" & SelectedCompanyId & ") AND PAYMODE = 'DR'"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " AND O.BATCHNO NOT IN (select BATCHNO from " & cnAdminDb & "..ITEMDETAIL WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '') "
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.TRANNO,O.RUNNO,O.TRANDATE"
        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT SUBSTRING(O.RUNNO,1,20) + ' - ' + (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO  WHERE BATCHNO = O.BATCHNO)) "
        'StrSql += vbCrLf + " ISNULL((select top 1 '(JND)' from " & cnAdminDb & "..ITEMDETAIL where runno=o.runno),'')+"
        '        StrSql += VBCRLF + " '(INV NO: '+SUBSTRING(O.RUNNO,6,20) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        'StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +' - ADJ NO: '+SUBSTRING(O.RUNNO,3,20) +')' AS CATNAME,  "
        StrSql += vbCrLf + " AS CATNAME,  "
        StrSql += vbCrLf + " SUM(CASE WHEN O.RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, 'J' AS COLHEAD "
        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + "  AND O.RECPAY = 'R' AND COMPANYID IN (" & SelectedCompanyId & ") AND PAYMODE = 'DR'"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " AND O.BATCHNO IN (select BATCHNO from " & cnAdminDb & "..ITEMDETAIL WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '') "
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.TRANNO,O.RUNNO,O.TRANDATE"

        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDADJ)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'CREDIT RECEIPT(DUE) [' + CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),0))+']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'CREDIT RECEIPT(DUE) [' + CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),0))+']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDADJ WHERE COLHEAD='D'"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, '' COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDADJ WHERE COLHEAD='D'"
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += vbCrLf + " SELECT 'CREDIT RECEIPT(DUE)','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDADJ WHERE COLHEAD='D'"
        End If

        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'CREDIT RECEIPT(JND) [' + CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),0))+']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'CREDIT RECEIPT(JND) [' + CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),0))+']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDADJ WHERE COLHEAD='J'"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, '' COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDADJ WHERE COLHEAD='J'"
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += vbCrLf + " SELECT 'CREDIT RECEIPT(JND)','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDADJ WHERE COLHEAD='J'"
        End If

        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcCreditPurchasePayment()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDPURPAY') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSCREDPURPAY "
        StrSql += vbCrLf + " SELECT SUBSTRING(O.RUNNO,1,20) + ' - ' + (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO  WHERE BATCHNO = O.BATCHNO)) "
        'StrSql += VBCRLF + " '(INV NO: '+SUBSTRING(O.RUNNO,6,20) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += vbCrLf + "  AS CATNAME,  "
        'StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) + '- REF NO: '+SUBSTRING(O.RUNNO,3,20)+', NETWT: '+ CONVERT (VARCHAR(20),(SELECT SUM(ISNULL(NETWT,0)) FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO IN(SELECT BATCHNO FROM  " & cnAdminDb & "..OUTSTANDING WHERE RUNNO=O.RUNNO))) +')' AS CATNAME,  "
        StrSql += vbCrLf + " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN AMOUNT ELSE 0 END) AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSCREDPURPAY FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + "  AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + "  AND PAYMODE IN('DP','RA') AND O.RECPAY = 'P'"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.TRANNO,O.TRANDATE,O.RUNNO"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDPURPAY)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'PURCHASE/SALESRETURN PAYMENT['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'PURCHASE/SALESRETURN PAYMENT['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDPURPAY "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDPURPAY "
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += vbCrLf + " SELECT 'PURCHASE/SALESRETURN PAYMENT','T',SUM(PAYMENT)-SUM(RECEIPT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDPURPAY "
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcAdvanceReceived()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVREC') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ADVREC "
        StrSql += vbCrLf + " SELECT SUBSTRING(O.RUNNO,1,20) +'- '+ (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += vbCrLf + " CASE WHEN PAYMODE IN('AR','OR') THEN '' ELSE '' END AS CATNAME,  "
        'StrSql += vbCrLf + " CASE WHEN PAYMODE IN('AR','OR') THEN '(INV NO: ' ELSE '(GIFT VOU NO:' END + CONVERT(VARCHAR(12),O.TRANNO)  +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+' - ADJ NO: '+SUBSTRING(O.RUNNO,3,20)+')' AS CATNAME,  "

        StrSql += vbCrLf + " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT+ISNULL(O.GSTVAL,0) ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ADVREC FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND PAYMODE IN ('AR','GV','OR') AND O.RECPAY = 'R' "
        'StrSql += vbCrLf + " AND O.TRANTYPE IN ('A','GV')"
        StrSql += vbCrLf + " AND O.TRANTYPE IN ('A')"
        StrSql += vbCrLf + " AND NOT (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.TRANNO,O.RUNNO,O.TRANDATE,PAYMODE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ADVREC)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'ADVANCE RECEIVED ['+CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),''))+']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'ADVANCE RECEIVED ['+CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),''))+']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ADVREC "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ADVREC "
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += vbCrLf + " SELECT 'ADVANCE RECEIVED','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ADVREC "
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcGiftVoucher()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "GiftVoucher') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "GiftVoucher "
        StrSql += vbCrLf + " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += vbCrLf + " CASE WHEN PAYMODE IN('AR','OR') THEN '(INV NO: ' ELSE '(GIFT VOU NO:' END + CONVERT(VARCHAR(12),O.TRANNO) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += vbCrLf + " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT+ISNULL(O.GSTVAL,0) ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "GiftVoucher FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND PAYMODE IN ('AR','GV','OR') AND O.RECPAY = 'R' "
        'StrSql += vbCrLf + " AND O.TRANTYPE IN ('A','GV')"
        StrSql += vbCrLf + " AND O.TRANTYPE IN ('GV')"
        StrSql += vbCrLf + " AND NOT (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.RUNNO,O.TRANNO,O.TRANDATE,PAYMODE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "GiftVoucher)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'GIFT VOUCHER ['+CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),''))+']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'GIFT VOUCHER ['+CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),''))+']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "GiftVoucher "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "GiftVoucher "
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += vbCrLf + " SELECT 'GIFT VOUCHER ','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "GiftVoucher "
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub


    Private Sub ProcAdvanceAdjustment()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVADJ') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ADVADJ "
        StrSql += vbCrLf + " SELECT "
        If RPT_DABS_ADVDATE Then
            StrSql += vbCrLf + " (SELECT SUBSTRING(O.RUNNO,1,20) + '- '+ (PNAME+' '+INITIAL) CATNAME FROM "
            StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO)) "
            StrSql += vbCrLf + " AS CATNAME,  "
            ' StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +' - '+ (SELECT TOP 1 CONVERT(VARCHAR,TRANDATE,103) FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO=O.RUNNO AND RECPAY='R')+' - ADJ NO:'+ SUBSTRING(O.RUNNO,3,20) +')' AS CATNAME,  "

        Else
            StrSql += vbCrLf + "   (SELECT SUBSTRING(O.RUNNO,1,20) + '- '+ (PNAME+' '+INITIAL) CATNAME FROM "
            StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO)) "
            StrSql += vbCrLf + " AS CATNAME,  "
            ' StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+' -ADJ NO:'+ SUBSTRING(O.RUNNO,3,20) +')' AS CATNAME,  "
            'StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+' -ADJ NO:'+ SUBSTRING(O.RUNNO,3,20) +')' AS CATNAME,  "
        End If
        StrSql += vbCrLf + " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT+ISNULL(O.GSTVAL,0) ELSE 0 END) AS PAYMENT, "
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ADVADJ FROM " & cnAdminDb & "..OUTSTANDING O"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND PAYMODE IN ('AA','AP') AND O.RECPAY = 'P' "
        StrSql += vbCrLf + " AND O.TRANTYPE = 'A'"
        StrSql += vbCrLf + " AND NOT (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        'StrSql += VBCRLF + " AND O.RUNNO LIKE 'A%'"
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.RUNNO,O.TRANNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ADVADJ)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'ADVANCE ADJUSTED ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'ADVANCE ADJUSTED ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ADVADJ "

            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ADVADJ "
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,PAYMENT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += vbCrLf + " SELECT 'ADVANCE ADJUSTED','T',SUM(PAYMENT)-SUM(RECEIPT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ADVADJ "
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcRepairAdvance()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "REPAIRADV') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "REPAIRADV "
        StrSql += vbCrLf + " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += VBCRLF + " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +')' AS CATNAME,  "
        StrSql += vbCrLf + " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE 0 END)  AS RECEIPT, 0 AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "REPAIRADV FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND PAYMODE IN ('OR','AR') AND O.RECPAY = 'R' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND SUBSTRING(O.RUNNO,6,1) = 'R'"
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.RUNNO,O.TRANNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "REPAIRADV)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'REPAIR ADVANCE RECEIVED ['+CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),''))+']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'REPAIR ADVANCE RECEIVED ['+CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),''))+']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "REPAIRADV "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "REPAIRADV "
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += vbCrLf + " SELECT 'REPAIR ADVANCE RECEIVED','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "REPAIRADV "
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcRepairAdvanceAdjusted()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "REPAIRADVADJ') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "REPAIRADVADJ "
        StrSql += vbCrLf + " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += VBCRLF + " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +')' AS CATNAME,  "
        StrSql += vbCrLf + " SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END)  AS PAYMENT, 0 AS RECEIPT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "REPAIRADVADJ FROM " & cnAdminDb & "..OUTSTANDING AS O "

        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND PAYMODE IN ('AA','AP') AND O.RECPAY = 'P' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND SUBSTRING(O.RUNNO,6,1) = 'R'"
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.RUNNO,O.TRANNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "REPAIRADVADJ)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'REPAIR ADVANCE ADJUSTED ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'REPAIR ADVANCE ADJUSTED ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "REPAIRADVADJ "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "REPAIRADVADJ "
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += vbCrLf + " SELECT 'REPAIR ADVANCE ADJUSTED','T',SUM(PAYMENT)-SUM(RECEIPT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "REPAIRADVADJ "
        End If
        'StrSql += VBCRLF + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        'StrSql += VBCRLF + " (DESCRIPTION, COLHEAD) "
        'StrSql += VBCRLF + " SELECT 'REPAIR ADVANCE ADJUSTED ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
        'StrSql += VBCRLF + " FROM TEMPTABLEDB..TEMP" & systemId & "REPAIRADVADJ "
        'StrSql += VBCRLF + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        'StrSql += VBCRLF + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        'StrSql += VBCRLF + " SELECT '  ' + CATNAME CATNAME, "
        'StrSql += VBCRLF + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        'StrSql += VBCRLF + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        'StrSql += VBCRLF + " 1 RESULT1, COLHEAD  "
        'StrSql += VBCRLF + " FROM TEMPTABLEDB..TEMP" & systemId & "REPAIRADVADJ "
        'If rbtAmount.Checked = True Then
        '    StrSql += VBCRLF + " ORDER BY RESULT1,RECEIPT "
        'Else
        '    StrSql += VBCRLF + " ORDER BY RESULT1,CATNAME "
        'End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcOrderAdvance()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ORDERADV') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ORDERADV "
        StrSql += vbCrLf + " SELECT SUBSTRING(O.RUNNO,1,20) + ' - '+ (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO)) "
        'StrSql += VBCRLF + " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        'StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +')' AS CATNAME,  "
        StrSql += vbCrLf + " AS CATNAME,  "
        StrSql += vbCrLf + " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END)  AS RECEIPT, 0 AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ORDERADV FROM " & cnAdminDb & "..OUTSTANDING AS O "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND PAYMODE IN ('OR','AR') AND O.RECPAY = 'R' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND SUBSTRING(O.RUNNO,6,1) <> 'R'"
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.RUNNO,O.TRANNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ORDERADV)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'ORDER ADVANCE RECEIVED ['+CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),''))+']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'ORDER ADVANCE RECEIVED ['+CONVERT(VARCHAR,ISNULL(SUM(RECEIPT-PAYMENT),''))+']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ORDERADV "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ORDERADV  WHERE (ISNULL(RECEIPT,0)<>0 AND ISNULL(PAYMENT,0)=0) "
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += vbCrLf + " SELECT 'ORDER ADVANCE RECEIVED','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ORDERADV "
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcOrderAdvanceAdjusted()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ORDERADVADJ') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ORDERADVADJ "
        StrSql += vbCrLf + " SELECT SUBSTRING(O.RUNNO,1,20) + ' - '+ (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO)) "
        'StrSql += VBCRLF + " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        ' StrSql += vbCrLf + " '(INV NO: '+ CONVERT(VARCHAR(12),O.TRANNO) +'-'+CONVERT(VARCHAR,O.TRANDATE,103)+'-ORNO:'+ SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += vbCrLf + " AS CATNAME,  "
        StrSql += vbCrLf + " SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END)  AS PAYMENT, 0 AS RECEIPT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ORDERADVADJ FROM " & cnAdminDb & "..OUTSTANDING AS O "

        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND PAYMODE IN ('AA','AP') AND O.RECPAY = 'P' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND SUBSTRING(O.RUNNO,6,1) <> 'R'"
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.RUNNO,O.TRANNO,O.TRANDATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ORDERADVADJ)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'ORDER ADVANCE ADJUSTED ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'ORDER ADVANCE ADJUSTED ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ORDERADVADJ "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ORDERADVADJ "
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += vbCrLf + " SELECT 'ORDER ADVANCE ADJUSTED','T',SUM(PAYMENT)-SUM(RECEIPT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ORDERADVADJ "
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcMiscReceipt()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "MISCRECEIPTS') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "MISCRECEIPTS "
        StrSql += vbCrLf + " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += vbCrLf + " CASE WHEN ISNULL(O.REMARK1,'') <> '' THEN '('+O.REMARK1+')' ELSE '' END AS CATNAME,"
        'strsql += vbcrlf + " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += vbCrLf + " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT, "
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "MISCRECEIPTS FROM " & cnAdminDb & "..OUTSTANDING O"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND PAYMODE IN ('MR') AND O.RECPAY = 'R'"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        StrSql += vbCrLf + " ,O.REMARK1"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If chkCashOpening.Checked Then
            StrSql = " IF (sELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCASHOPEN')>0 DROP TABLE TEMPTABLEDB..TEMPCASHOPEN"
            StrSql += vbCrLf + " DECLARE @CASHID VARCHAR(7)"
            StrSql += vbCrLf + " SELECT @CASHID = CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'"
            StrSql += vbCrLf + " SELECT SUM(ISNULL(AMOUNT,0)) AS AMOUNT"
            StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMPCASHOPEN"
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
            If Not chkRecPay.Checked And Not RPT_GSTREGISTER_CASH_VIEW Then StrSql += vbCrLf + "     AND PAYMODE IN ('CA')"
            StrSql += vbCrLf + "     AND ACCODE = @CASHID"
            If Not chkRecPay.Checked And Not RPT_GSTREGISTER_CASH_VIEW Then StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
            If Not hasChit Then StrSql += vbCrLf + " AND FROMFLAG <> 'C'"
            StrSql += vbCrLf + "     )X"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            'StrSql = " IF (SELECT COUNT(*) FROM TEMPCASHOPEN)>0"
            'strsql += vbcrlf + " BEGIN"
            'StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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


        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "MISCRECEIPTS)>0 "
        If chkCashOpening.Checked Then
            StrSql += vbCrLf + " OR (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPCASHOPEN)>0"
        End If
        StrSql += vbCrLf + " BEGIN "

        If chkCashOpening.Checked Then
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT 'CASH OPENING..'CATNAME, "
            StrSql += vbCrLf + " CASE WHEN AMOUNT  > 0 THEN AMOUNT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN AMOUNT  < 0 THEN AMOUNT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1,'T' COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPCASHOPEN"
        End If

        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'MISC RECEIPTS ['+ISNULL(CONVERT(VARCHAR,SUM(RECEIPT-PAYMENT)),'')+']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'MISC RECEIPTS ['+ISNULL(CONVERT(VARCHAR,SUM(RECEIPT-PAYMENT)),'')+']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "MISCRECEIPTS"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "MISCRECEIPTS ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "MISCRECEIPTS ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += vbCrLf + " SELECT 'MISC RECEIPTS','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "MISCRECEIPTS"
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub



    Private Sub ProcAccReceipt()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ACCRECEIPTS') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ACCRECEIPTS "
        StrSql += vbCrLf + " DECLARE @CASHID VARCHAR(7)"
        StrSql += vbCrLf + " SELECT @CASHID = CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'"
        StrSql += vbCrLf + " SELECT (SELECT (ACNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = O.ACCODE)+CASE WHEN ISNULL(O.REMARK1,'') <> '' THEN '('+O.REMARK1+')' ELSE '' END AS CATNAME, "
        StrSql += vbCrLf + " SUM(CASE WHEN O.TRANMODE = 'C' THEN O.AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT, "
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ACCRECEIPTS FROM " & cnStockDb & "..ACCTRAN O"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND PAYMODE IN ('CR') AND O.TRANMODE = 'C'"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND CONTRA = @CASHID"
        StrSql += vbCrLf + " AND O.FROMFLAG IN ('A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.ACCODE,O.TRANDATE"
        StrSql += vbCrLf + " ,O.REMARK1"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        'If chkCashOpening.Checked Then
        '    StrSql = " IF (sELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCASHOPEN')>0 DROP TABLE TEMPCASHOPEN"
        '    StrSql += vbCrLf + " DECLARE @CASHID VARCHAR(7)"
        '    StrSql += vbCrLf + " SELECT @CASHID = CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'"
        '    StrSql += vbCrLf + " SELECT SUM(ISNULL(AMOUNT,0)) AS AMOUNT"
        '    StrSql += vbCrLf + " INTO TEMPCASHOPEN"
        '    StrSql += vbCrLf + " FROM"
        '    StrSql += vbCrLf + "     ("
        '    StrSql += vbCrLf + "     SELECT DEBIT-CREDIT AS AMOUNT FROM " & cnStockDb & "..OPENTRAILBALANCE"
        '    StrSql += vbCrLf + "     WHERE ACCODE = @CASHID"
        '    StrSql += vbCrLf + "     AND COMPANYID IN (" & SelectedCompanyId & ")"
        '    StrSql += StrCostFiltration
        '    StrSql += vbCrLf + "     UNION ALL"
        '    StrSql += vbCrLf + "     SELECT CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT"
        '    StrSql += vbCrLf + "     FROM " & cnStockDb & "..ACCTRAN A"
        '    StrSql += vbCrLf + "     WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        '    StrSql += vbCrLf + "     AND COMPANYID IN (" & SelectedCompanyId & ")"
        '    StrSql += vbCrLf + "     AND ISNULL(A.CANCEL,'') = ''"
        '    StrSql += StrFilter & StrUseridFtr 'StrCostFiltration
        '    StrSql += vbCrLf + "     AND PAYMODE IN ('CA')"
        '    StrSql += vbCrLf + "     AND ACCODE = @CASHID"
        '    StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
        '    If Not hasChit Then StrSql += vbCrLf + " AND FROMFLAG <> 'C'"
        '    StrSql += vbCrLf + "     )X"
        '    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        '    Cmd.ExecuteNonQuery()
        'End If


        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ACCRECEIPTS)>0 "
        'If chkCashOpening.Checked Then
        '    StrSql += vbCrLf + " OR (SELECT COUNT(*) FROM TEMPCASHOPEN)>0"
        'End If
        StrSql += vbCrLf + " BEGIN "

        'If chkCashOpening.Checked Then
        '    StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        '    StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        '    StrSql += vbCrLf + " SELECT 'CASH OPENING..'CATNAME, "
        '    StrSql += vbCrLf + " CASE WHEN AMOUNT  > 0 THEN AMOUNT  ELSE NULL END RECEIPT, "
        '    StrSql += vbCrLf + " CASE WHEN AMOUNT  < 0 THEN AMOUNT  ELSE NULL END PAYMENT, "
        '    StrSql += vbCrLf + " 1 RESULT1,'T' COLHEAD  "
        '    StrSql += vbCrLf + " FROM TEMPCASHOPEN"
        'End If

        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'RECEIPTS [ACC] ['+ISNULL(CONVERT(VARCHAR,SUM(RECEIPT-PAYMENT)),'')+']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'RECEIPTS [ACC] ['+ISNULL(CONVERT(VARCHAR,SUM(RECEIPT-PAYMENT)),'')+']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCRECEIPTS"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCRECEIPTS ORDER BY RESULT1,RECEIPT "
            Else
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCRECEIPTS ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT) "
            StrSql += vbCrLf + " SELECT 'RECEIPTS [ACC]','T',SUM(RECEIPT)-SUM(PAYMENT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCRECEIPTS"
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcChitPayment()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITPAYMENT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "CHITPAYMENT "
        StrSql += vbCrLf + " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += vbCrLf + " '(GRP:' + CHQCARDREF + ' REGNO: ' + CHQCARDNO + ')' AS CATNAME,  "
        StrSql += vbCrLf + " 0 AS RECEIPT, SUM(CASE WHEN TRANMODE = 'D' THEN O.AMOUNT ELSE -1*AMOUNT END) AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "CHITPAYMENT FROM " & cnStockDb & "..ACCTRAN O"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND PAYMODE IN ('HP','HG','HZ','HB','HD')"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.CHQCARDREF,O.CHQCARDNO"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "CHITPAYMENT)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        If chkWithHeaderTotal.Checked Then
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
            StrSql += vbCrLf + " SELECT 'SCHEME PAYMENTS ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
            StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
        Else
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
            StrSql += vbCrLf + " SELECT 'SCHEME PAYMENTS ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
        End If
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "CHITPAYMENT"
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
        StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
        If rbtAmount.Checked = True Then
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "CHITPAYMENT ORDER BY RESULT1,PAYMENT "
        Else
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "CHITPAYMENT ORDER BY RESULT1,CATNAME "
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcMiscPayment()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "MISCPAYMENT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "MISCPAYMENT "
        StrSql += vbCrLf + " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += vbCrLf + " CASE WHEN ISNULL(O.REMARK1,'') <> '' THEN '('+O.REMARK1+')' ELSE '' END AS CATNAME,"
        'StrSql += VBCRLF + " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += vbCrLf + " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END) AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "MISCPAYMENT FROM " & cnAdminDb & "..OUTSTANDING O"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND PAYMODE IN ('MP') AND O.RECPAY = 'P'"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        StrSql += vbCrLf + " ,O.REMARK1"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "MISCPAYMENT)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'MISC PAYMENTS ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'MISC PAYMENTS ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "MISCPAYMENT"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "MISCPAYMENT ORDER BY RESULT1,PAYMENT "
            Else
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "MISCPAYMENT ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += vbCrLf + " SELECT 'MISC PAYMENTS','T',SUM(PAYMENT)-SUM(RECEIPT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "MISCPAYMENT"
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcAccPayment()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ACCPAYMENT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ACCPAYMENT "
        StrSql += vbCrLf + " DECLARE @CASHID VARCHAR(7)"
        StrSql += vbCrLf + " SELECT @CASHID = CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'"
        StrSql += vbCrLf + " SELECT (SELECT (ACNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = O.ACCODE)+CASE WHEN ISNULL(O.REMARK1,'') <> '' THEN '('+O.REMARK1+')' ELSE '' END AS CATNAME,"
        StrSql += vbCrLf + " 0 AS RECEIPT, SUM(CASE WHEN O.TRANMODE = 'D' THEN O.AMOUNT ELSE 0 END) AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ACCPAYMENT FROM " & cnStockDb & "..ACCTRAN O"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND PAYMODE IN ('CP') AND O.TRANMODE = 'D'"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND CONTRA = @CASHID"
        StrSql += vbCrLf + " AND O.FROMFLAG IN ('A')"
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.ACCODE,O.TRANDATE"
        StrSql += vbCrLf + " ,O.REMARK1"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ACCPAYMENT)>0 "
        StrSql += vbCrLf + " BEGIN "
        If chkDetaledRecPay.Checked Then
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            If chkWithHeaderTotal.Checked Then
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT 'PAYMENTS [ACC] ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
                StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(RECEIPT) END RECEIPT, CASE WHEN SUM(ISNULL(PAYMENT,0)) > 0 THEN SUM(PAYMENT) END PAYMENT "
            Else
                StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
                StrSql += vbCrLf + " SELECT 'PAYMENTS [ACC] ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
            End If
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCPAYMENT"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            If rbtAmount.Checked = True Then
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCPAYMENT ORDER BY RESULT1,PAYMENT "
            Else
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCPAYMENT ORDER BY RESULT1,CATNAME "
            End If
        Else
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD,PAYMENT) "
            StrSql += vbCrLf + " SELECT 'PAYMENTS [ACC]','T',SUM(PAYMENT)-SUM(RECEIPT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCPAYMENT"
        End If
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcBeeds()
        If chkSeperateBeeds.Checked = False Then Exit Sub
        StrSql = " SELECT "
        StrSql += vbCrLf + " SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) AS GRSWT "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE AS ISS WHERE "
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
        StrSql += vbCrLf + " SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) AS GRSWT "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE AS ISS WHERE "
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
            StrSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSGRSWT,RECGRSWT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  BEEDS' CATNAME,  "
            StrSql += vbCrLf + " " & sBeeds & "," & rBeeds & ""
            StrSql += vbCrLf + " ,1 RESULT1,'T'COLHEAD  "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub ProcDiscount()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "DISCOUNT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DISCOUNT "
        StrSql += vbCrLf + " SELECT 'DISCOUNT' CATNAME,  "
        StrSql += vbCrLf + " SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END) AS AMT, "
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "DISCOUNT FROM " & cnStockDb & "..ACCTRAN AS A  "
        StrSql += vbCrLf + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
        StrSql += vbCrLf + " AND PAYMODE = 'DI'   AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END),0) <> 0"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DISCOUNT)>0 "
        StrSql += vbCrLf + " BEGIN "
        'StrSql += VBCRLF + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME,  "
        StrSql += vbCrLf + " CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT,  "
        StrSql += vbCrLf + " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "DISCOUNT ORDER BY CATNAME  "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcPRIVILEGE()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "DISCOUNT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DISCOUNT "
        StrSql += vbCrLf + " SELECT 'PRIVILEGE ' CATNAME,  "
        StrSql += vbCrLf + " CASE TRANTYPE WHEN 'I' THEN -SUM(PVALUE) WHEN 'R' THEN SUM(PVALUE) END AMT,  "
        'StrSql += vbCrLf + " SUM(PVALUE) AS , "
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "DISCOUNT FROM " & cnAdminDb & "..PRIVILEGETRAN"
        StrSql += vbCrLf + " WHERE BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE AS A  "
        StrSql += vbCrLf + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + ") GROUP BY TRANTYPE"
        'StrSql += vbCrLf + " HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END),0) <> 0)"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DISCOUNT)>0 "
        StrSql += vbCrLf + " BEGIN "
        'StrSql += VBCRLF + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + " SELECT CATNAME,SUM(RECEIPT),SUM(PAYMENT),RESULT1,COLHEAD FROM(SELECT '  ' + CATNAME CATNAME,  "
        StrSql += vbCrLf + " CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT,  "
        StrSql += vbCrLf + " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " 1 RESULT1, COLHEAD"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "DISCOUNT )X GROUP BY CATNAME,RESULT1,COLHEAD ORDER BY CATNAME "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    'Private Sub ProcDiscount_Fin()
    '    If RPT_DISC_BRKUP_DABS Then
    '        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "FIN_DISCOUNT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT "
    '        StrSql += vbCrLf + "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITADJAMOUNT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "CHITADJAMOUNT "
    '        StrSql += vbCrLf + " SELECT 'OTP DISCOUNT' CATNAME,  "
    '        StrSql += vbCrLf + " SUM(ISNULL(DISCOUNT,0)+ISNULL(FIN_DISCOUNT,0)) AS AMT,  "
    '        StrSql += VBCRLF + " 1 AS RESULT1, ' ' AS COLHEAD,BATCHNO "
    '        StrSql += VBCRLF + " INTO TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT FROM " & cnStockDb & "..ISSUE AS A  "
    '        StrSql += VBCRLF + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
    '        StrSql += VBCRLF + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
    '        StrSql += VBCRLF + " AND TRANTYPE IN ('SA','OD')  AND COMPANYID IN (" & SelectedCompanyId & ")"
    '        StrSql += StrFilter
    '        StrSql += StrUseridFtr
    '        StrSql += VBCRLF + " AND BATCHNO IN(SELECT BATCHNO FROM " & cnAdminDb & "..EXEMPTION )"
    '        StrSql += VBCRLF + " GROUP BY BATCHNO"
    '        StrSql += VBCRLF + " UNION ALL"
    '        StrSql += VBCRLF + " SELECT 'SCHEME DISCOUNT' CATNAME,  "
    '        StrSql += vbCrLf + " SUM(ISNULL(DISCOUNT,0)+ISNULL(FIN_DISCOUNT,0)) AS AMT, "
    '        StrSql += VBCRLF + " 1 AS RESULT1, ' ' AS COLHEAD,BATCHNO "
    '        StrSql += VBCRLF + " FROM " & cnStockDb & "..ISSUE AS A  "
    '        StrSql += VBCRLF + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
    '        StrSql += VBCRLF + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
    '        StrSql += VBCRLF + " AND TRANTYPE IN ('SA','OD')  AND COMPANYID IN (" & SelectedCompanyId & ")"
    '        StrSql += StrFilter
    '        StrSql += StrUseridFtr
    '        StrSql += VBCRLF + " AND BATCHNO IN(SELECT BATCHNO FROM " & cnStockDb & "..CHITOFFER )"
    '        StrSql += VBCRLF + " GROUP BY BATCHNO"
    '        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
    '        Cmd.ExecuteNonQuery()

    '        StrSql = vbCrLf + " SELECT BATCHNO, "
    '        StrSql += vbCrLf + " (SELECT SUM(ISNULL(DISCOUNT,0)+ISNULL(FIN_DISCOUNT,0)) -SUM(ISNULL(ADJOFFAMT,0)+ISNULL(ADJOFFVAT,0)) FROM " & cnStockDb & "..CHITOFFER WHERE BATCHNO=A.BATCHNO) AS OFFERDIFAMT,"
    '        StrSql += vbCrLf + " (SELECT SUM(ISNULL(ADJOFFAMT,0)+ISNULL(ADJOFFVAT,0)) FROM " & cnStockDb & "..CHITOFFER WHERE BATCHNO=A.BATCHNO) AS OFFERAMT"
    '        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "CHITADJAMOUNT FROM " & cnStockDb & "..ISSUE AS A  "
    '        StrSql += vbCrLf + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
    '        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
    '        StrSql += vbCrLf + " AND TRANTYPE IN ('SA','OD')  AND COMPANYID IN (" & SelectedCompanyId & ")"
    '        StrSql += StrFilter
    '        StrSql += StrUseridFtr
    '        StrSql += vbCrLf + " AND BATCHNO IN(SELECT BATCHNO FROM " & cnStockDb & "..CHITOFFER )"
    '        StrSql += vbCrLf + " GROUP BY BATCHNO"
    '        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
    '        Cmd.ExecuteNonQuery()

    '        StrSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT"
    '        StrSql += VBCRLF + " SELECT 'DISCOUNT' CATNAME,  "
    '        StrSql += vbCrLf + " SUM(ISNULL(DISCOUNT,0)+ISNULL(FIN_DISCOUNT,0)) AS AMT, "

    '        StrSql += VBCRLF + " 1 AS RESULT1, ' ' AS COLHEAD,BATCHNO "
    '        StrSql += VBCRLF + " FROM " & cnStockDb & "..ISSUE AS A  "
    '        StrSql += VBCRLF + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
    '        StrSql += VBCRLF + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
    '        StrSql += VBCRLF + " AND TRANTYPE IN ('SA','OD')  AND COMPANYID IN (" & SelectedCompanyId & ")"
    '        StrSql += StrFilter
    '        StrSql += StrUseridFtr
    '        StrSql += vbCrLf + " AND BATCHNO NOT IN(SELECT BATCHNO FROM TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT )"
    '        StrSql += VBCRLF + " GROUP BY BATCHNO"
    '        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
    '        Cmd.ExecuteNonQuery()


    '        Dim DIFFAMT As Double = 0
    '        StrSql = "SELECT  SUM(F.AMT) - SUM(C.OFFERAMT ) AS DIFAMT FROM TEMPLAXFIN_DISCOUNT AS F,  TEMPLAXCHITADJAMOUNT C WHERE C.BATCHNO = F.BATCHNO  AND F.CATNAME = 'SCHEME DISCOUNT'"
    '        DIFFAMT = Val(objGPack.GetSqlValue(StrSql).ToString)

    '        If DIFFAMT > 0 Then
    '            StrSql = "UPDATE F SET AMT = AMT - OFFERDIFAMT  FROM TEMPLAXFIN_DISCOUNT AS F,  TEMPLAXCHITADJAMOUNT C WHERE C.BATCHNO = F.BATCHNO  AND F.CATNAME = 'SCHEME DISCOUNT'"
    '            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
    '            Cmd.ExecuteNonQuery()

    '            StrSql = "UPDATE F SET AMT = AMT + OFFERDIFAMT  FROM TEMPLAXFIN_DISCOUNT AS F,  TEMPLAXCHITADJAMOUNT C WHERE C.BATCHNO = F.BATCHNO  AND F.CATNAME = 'DISCOUNT'"
    '            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
    '            Cmd.ExecuteNonQuery()

    '            StrSql = "UPDATE F SET AMT = AMT - OFFERAMT  FROM TEMPLAXFIN_DISCOUNT AS F,  TEMPLAXCHITADJAMOUNT C WHERE C.BATCHNO = F.BATCHNO  AND F.CATNAME = 'OTP DISCOUNT'"
    '            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
    '            Cmd.ExecuteNonQuery()
    '        End If

    '        StrSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT"
    '        StrSql += vbCrLf + " SELECT 'TOTAL' CATNAME,  "
    '        StrSql += vbCrLf + " SUM(ISNULL(AMT,0)) AS AMT, "
    '        StrSql += vbCrLf + " 1 AS RESULT1, 'S' AS COLHEAD,'' "
    '        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT AS A  "
    '        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
    '        Cmd.ExecuteNonQuery()

    '    Else
    '        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT "
    '        StrSql += vbCrLf + " SELECT 'DISCOUNT' CATNAME,  "
    '        StrSql += vbCrLf + " SUM(ISNULL(DISCOUNT,0)+ISNULL(FIN_DISCOUNT,0)) AS AMT, "
    '        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
    '        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT FROM " & cnStockDb & "..ISSUE AS A  "
    '        StrSql += vbCrLf + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
    '        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
    '        StrSql += vbCrLf + " AND TRANTYPE IN ('SA','OD')  AND COMPANYID IN (" & SelectedCompanyId & ")"
    '        StrSql += StrFilter
    '        StrSql += StrUseridFtr
    '        StrSql += vbCrLf + " UNION ALL"
    '        StrSql += vbCrLf + " SELECT 'CHIT DISCOUNT' CATNAME,  "
    '        StrSql += vbCrLf + " SUM(ISNULL(CHIT_DISCOUNT,0)) AS AMT, "
    '        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
    '        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS A  "
    '        StrSql += vbCrLf + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
    '        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
    '        StrSql += vbCrLf + " AND TRANTYPE IN ('SA','OD')  AND COMPANYID IN (" & SelectedCompanyId & ")"
    '        StrSql += StrFilter
    '        StrSql += StrUseridFtr
    '        StrSql += vbCrLf + " HAVING SUM(ISNULL(CHIT_DISCOUNT,0))<>0"
    '        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
    '        Cmd.ExecuteNonQuery()
    '    End If

    '    StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT)>0 "
    '    StrSql += vbCrLf + " BEGIN "
    '    'StrSql += VBCRLF + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
    '    StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
    '    StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
    '    StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME,  "
    '    StrSql += vbCrLf + " NULL RECEIPT,  "
    '    StrSql += vbCrLf + " SUM(ABS(AMT))  PAYMENT, "
    '    StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
    '    StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT GROUP BY CATNAME,COLHEAD ORDER BY CATNAME  "
    '    StrSql += vbCrLf + " END "
    '    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
    '    Cmd.ExecuteNonQuery()
    'End Sub

    Private Sub ProcDiscount_Fin()
        If RPT_DISC_BRKUP_DABS Then
            StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "FIN_DISCOUNT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT "
            StrSql += vbCrLf + "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITADJAMOUNT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "CHITADJAMOUNT "
            StrSql += vbCrLf + "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "FIN_DISC') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "FIN_DISC "

            StrSql += vbCrLf & " DECLARE @FROM VARCHAR(10) = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf & " DECLARE @TO VARCHAR(10) = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "

            StrSql += vbCrLf & " SELECT CONVERT(VARCHAR(20),'DISCOUNT') CATNAME"
            StrSql += vbCrLf & " ,CONVERT(NUMERIC(10,2),SUM(ISNULL(DISCOUNT,0))) -(SELECT ISNULL(SUM(DISCALLOWAMT),0) FROM  " & cnStockDb & "..DISCHISTORY "
            StrSql += vbCrLf & " WHERE BATCHNO IN (SELECT DISTINCT  BATCHNO FROM " & cnAdminDb & "..EXEMPTION)AND BATCHNO = I.BATCHNO )AMT"
            StrSql += vbCrLf & " INTO TEMPTABLEDB..TEMP" & systemId & "FIN_DISC FROM " & cnStockDb & "..ISSUE I WHERE TRANDATE BETWEEN @FROM AND @TO "
            StrSql += vbCrLf + " AND TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'')=''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += vbCrLf + StrFilter
            StrSql += vbCrLf + StrUseridFtr
            StrSql += vbCrLf & " GROUP BY BATCHNO"
            StrSql += vbCrLf & " HAVING SUM(ISNULL(DISCOUNT,0)) <> 0"

            StrSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "FIN_DISC "
            StrSql += vbCrLf & " SELECT 'DISCOUNT' ,SUM(ISNULL(FIN_DISCOUNT,0))"
            StrSql += vbCrLf & " FROM " & cnStockDb & "..ISSUE I WHERE TRANDATE BETWEEN @FROM AND @TO "
            StrSql += vbCrLf & " AND BATCHNO NOT IN (SELECT DISTINCT  BATCHNO FROM " & cnStockDb & "..CHITOFFER)"
            StrSql += vbCrLf & " AND BATCHNO NOT IN (SELECT DISTINCT  BATCHNO FROM " & cnAdminDb & "..EXEMPTION WHERE BATCHNO IN (SELECT DISTINCT  BATCHNO FROM " & cnStockDb & "..DISCHISTORY))"
            StrSql += vbCrLf + " AND TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'')=''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += vbCrLf + StrFilter
            StrSql += vbCrLf + StrUseridFtr
            StrSql += vbCrLf & " GROUP BY BATCHNO "
            StrSql += vbCrLf & " HAVING SUM(ISNULL(FIN_DISCOUNT,0)) <> 0"

            StrSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "FIN_DISC "
            'StrSql += vbCrLf & " SELECT 'OTP DISCOUNT' ,ISNULL(SUM(DISCALLOWAMT),0) FROM  " & cnStockDb & "..DISCHISTORY "
            'StrSql += vbCrLf & " WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..EXEMPTION)"
            'StrSql += vbCrLf & " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN @FROM AND @TO "
            'StrSql += vbCrLf + " AND TRANTYPE IN ('SA','OD') AND ISNULL(CANCEL,'')=''  AND COMPANYID IN (" & SelectedCompanyId & ")"

            StrSql += vbCrLf + " SELECT 'OTP DISCOUNT' ,ISNULL(SUM(DISCALLOWAMT),0) FROM(SELECT DISTINCT BATCHNO"
            StrSql += vbCrLf + " ,ISNULL((SELECT TOP 1 DISCALLOWAMT FROM  " & cnStockDb & "..DISCHISTORY S WHERE S.BATCHNO =D.BATCHNO ORDER BY ISNULL(S.ENTRYORDER,0) DESC),0)DISCALLOWAMT"
            StrSql += vbCrLf + " FROM  " & cnStockDb & "..DISCHISTORY D "
            StrSql += vbCrLf + " WHERE D.BATCHNO IN (SELECT DISTINCT  BATCHNO FROM " & cnAdminDb & "..EXEMPTION)"
            StrSql += vbCrLf + " AND D.BATCHNO IN (SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN @FROM AND @TO "
            StrSql += vbCrLf + " AND TRANTYPE IN ('SA','OD') AND ISNULL(CANCEL,'')=''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += vbCrLf + StrFilter
            StrSql += vbCrLf + StrUseridFtr + " ))X"

            StrSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "FIN_DISC "
            StrSql += vbCrLf & " SELECT 'CHIT DISCOUNT' ,SUM(ISNULL(FIN_DISCOUNT,0)) FROM " & cnStockDb & "..ISSUE "
            StrSql += vbCrLf & " WHERE TRANDATE BETWEEN @FROM AND @TO "
            StrSql += vbCrLf & " AND BATCHNO IN (SELECT DISTINCT  BATCHNO FROM " & cnStockDb & "..CHITOFFER )"
            StrSql += vbCrLf + " AND TRANTYPE IN ('SA','OD') AND ISNULL(CANCEL,'')=''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += vbCrLf + StrFilter
            StrSql += vbCrLf + StrUseridFtr
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = vbCrLf & " SELECT CATNAME,SUM(AMT)AMT ,1 RESULT1 , ''COLHEAD"
            StrSql += vbCrLf & " INTO TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT FROM TEMPTABLEDB..TEMP" & systemId & "FIN_DISC"
            StrSql += vbCrLf & " GROUP BY CATNAME "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "FIN_DISCOUNT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT "
            StrSql += vbCrLf + " SELECT 'DISCOUNT' CATNAME,  "
            StrSql += vbCrLf + " SUM(ISNULL(DISCOUNT,0)+ISNULL(FIN_DISCOUNT,0)) AS AMT, "
            StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
            StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT FROM " & cnStockDb & "..ISSUE AS A  "
            StrSql += vbCrLf + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
            StrSql += vbCrLf + " AND TRANTYPE IN ('SA','OD') AND ISNULL(A.CANCEL,'')=''  AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += vbCrLf + StrFilter
            StrSql += vbCrLf + StrUseridFtr
            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT 'CHIT DISCOUNT' CATNAME,  "
            StrSql += vbCrLf + " SUM(ISNULL(CHIT_DISCOUNT,0)) AS AMT, "
            StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS A  "
            StrSql += vbCrLf + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
            StrSql += vbCrLf + " AND TRANTYPE IN ('SA','OD') AND ISNULL(A.CANCEL,'')='' AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += vbCrLf + StrFilter
            StrSql += vbCrLf + StrUseridFtr
            StrSql += vbCrLf + " HAVING SUM(ISNULL(CHIT_DISCOUNT,0))<>0"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT)>0 "
        StrSql += vbCrLf + " BEGIN "

        StrSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + "    (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + "    SELECT TOP 2 NULL CATNAME,  "
        StrSql += vbCrLf + "    NULL RECEIPT,  "
        StrSql += vbCrLf + "    NULL PAYMENT, "
        StrSql += vbCrLf + "    1 RESULT1,'' COLHEAD  "
        StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT"

        StrSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + "    (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + "    SELECT 'DISCOUNT ' CATNAME,  "
        StrSql += vbCrLf + "    NULL RECEIPT,  "
        StrSql += vbCrLf + "    NULL PAYMENT, "
        StrSql += vbCrLf + "    1 RESULT1,'T' COLHEAD  "

        StrSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + "    (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + "    SELECT '  ' + CATNAME CATNAME,  "
        StrSql += vbCrLf + "    NULL RECEIPT,  "
        StrSql += vbCrLf + "    SUM(ABS(AMT))  PAYMENT, "
        StrSql += vbCrLf + "    1 RESULT1, COLHEAD  "
        StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT GROUP BY CATNAME,COLHEAD ORDER BY CATNAME  "

        StrSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + "    (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + "    SELECT 'TOTAL' CATNAME,  "
        StrSql += vbCrLf + "    NULL RECEIPT,  "
        StrSql += vbCrLf + "    SUM(ABS(AMT))  PAYMENT, "
        StrSql += vbCrLf + "    1 RESULT1,'S' COLHEAD  "
        StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMP" & systemId & "FIN_DISCOUNT ORDER BY CATNAME  "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcHandling()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "HANDLING') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "HANDLING "
        StrSql += vbCrLf + " SELECT 'HANDLING CHARGES' CATNAME,  "
        StrSql += vbCrLf + " SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END) AS AMT, "
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "HANDLING FROM " & cnStockDb & "..ACCTRAN AS A  "
        StrSql += vbCrLf + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
        StrSql += vbCrLf + " AND PAYMODE = 'HC'   AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END),0) <> 0"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "HANDLING)>0 "
        StrSql += vbCrLf + " BEGIN "
        'StrSql += VBCRLF + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME,  "
        StrSql += vbCrLf + " CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT,  "
        StrSql += vbCrLf + " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "HANDLING ORDER BY CATNAME  "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcroundOff()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ROUNDOFF') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ROUNDOFF "
        StrSql += vbCrLf + " SELECT 'ROUNDOFF' CATNAME, "
        StrSql += vbCrLf + " SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END) AS AMT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ROUNDOFF FROM " & cnStockDb & "..ACCTRAN AS A "
        StrSql += vbCrLf + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(A.CANCEL,'') = '' "
        StrSql += vbCrLf + " AND PAYMODE = 'RO'  AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END),0) <> 0"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ROUNDOFF)>0 "
        StrSql += vbCrLf + " BEGIN "
        'StrSql += VBCRLF + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
        StrSql += vbCrLf + " CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ROUNDOFF ORDER BY CATNAME "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub


    Private Sub ProcWtSubtot()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "WTSTOT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "WTSTOT "
        StrSql += vbCrLf + " SELECT * INTO TEMPTABLEDB..TEMP" & systemId & "WTSTOT  FROM ("
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALES WHERE ISNULL(RESULT1,1) NOT IN (0,2) AND ISNULL(COLHEAD,'') <> 'L' " 'AND ISNULL(COLHEAD,'') <> 'D'
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ABSREPAIR WHERE ISNULL(RESULT1,1) NOT IN (0,2) AND ISNULL(COLHEAD,'') <> 'D'"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ABSSALESRETURN WHERE ISNULL(RESULT1,1) NOT IN (0,2) AND ISNULL(COLHEAD,'') <> 'L'" 'AND ISNULL(COLHEAD,'') <> 'D'"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT 0 RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ABS_ORDAMTWT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ABSPURCHASE WHERE ISNULL(RESULT1,1) NOT IN (0,2) AND ISNULL(COLHEAD,'') <> 'L'  " 'AND ISNULL(COLHEAD,'') <> 'D'"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ADVADJ"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ADVREC"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDSAL"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDPUR"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDADJ"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ABSCREDPURPAY"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "CHITPAYMENT"

        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "MISCRECEIPTS"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "MISCPAYMENT"

        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ORDERADV"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ORDERADVADJ"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "REPAIRADV"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "REPAIRADVADJ"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "Giftvoucher"
        StrSql += vbCrLf + " UNION ALL"

        StrSql += vbCrLf + " SELECT CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "DISCOUNT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ABSWSSALE"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "HANDLING"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ROUNDOFF"
        If chkCashOpening.Checked Then
            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT CASE WHEN AMOUNT > 0 THEN AMOUNT ELSE 0 END RECEIPT"
            StrSql += vbCrLf + " ,CASE WHEN AMOUNT < 0 THEN AMOUNT ELSE 0 END PAYMENT FROM TEMPTABLEDB..TEMPCASHOPEN"
        End If
        If chkRecPay.Checked Then
            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ACCRECEIPTS"
            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "ACCPAYMENT"
        End If
        If RPT_GSTREGISTER_CASH_VIEW Then
            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT RECEIPT, PAYMENT  FROM TEMPTABLEDB..TEMP" & systemId & "GSTREGISTER_CASH"
        End If
        StrSql += vbCrLf + " )X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "WTSTOT)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "WTSTOTAL') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "WTSTOTAL "
        StrSql += vbCrLf + " SELECT SUM(RECEIPT) AS RECEIPT, SUM(PAYMENT) AS PAYMENT INTO TEMPTABLEDB..TEMP" & systemId & "WTSTOTAL"
        StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "WTSTOT  END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        DT = New DataTable
        StrSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "WTSTOT"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(DT)
        If DT.Rows.Count <= 0 Then Exit Sub

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "WTSTOTAL)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,COLHEAD) "
        StrSql += vbCrLf + " SELECT 'TOTAL', RECEIPT, PAYMENT,'S' FROM TEMPTABLEDB..TEMP" & systemId & "WTSTOTAL"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "WTSTOTAL)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,COLHEAD) "
        StrSql += vbCrLf + " SELECT 'GRAND TOTAL', "
        StrSql += vbCrLf + " ISNULL((CASE WHEN X>0 THEN X END),NULL) AS RECEIPT,"
        StrSql += vbCrLf + " ISNULL((CASE WHEN X<0 THEN X END),NULL) AS PAYMENT, 'G'"
        StrSql += vbCrLf + " FROM ("
        StrSql += vbCrLf + " SELECT RECEIPT-PAYMENT  AS X FROM TEMPTABLEDB..TEMP" & systemId & "WTSTOTAL) Y"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcChitCollection()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITCOLLECTION') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION "
        StrSql += vbCrLf + " SELECT CONVERT(NUMERIC(15,2),0)RECEIPT,CONVERT(NUMERIC(15,2),0)PAYMENT"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If hasChit Then
            StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CHITDBPREFIX'"
            If (Trim(objGPack.GetSqlValue(StrSql, , ""))) <> "" Then
                StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITCOLLECTION') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION "
                StrSql += vbCrLf + " SELECT CATNAME,SUM(RECEIPT)RECEIPT ,SUM(PAYMENT)PAYMENT "
                StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION FROM ("
                StrSql += vbCrLf + " SELECT  CASE "
                StrSql += vbCrLf + " WHEN MODEPAY = 'C' THEN 'SCHEME CASH' "
                StrSql += vbCrLf + " WHEN MODEPAY IN('Q','D') THEN 'CHEQUE' "
                If OnlineAccode <> "" And FREEINS_BOUNS = False Then
                    StrSql += vbCrLf + " WHEN MODEPAY = 'R' THEN CASE WHEN T.ACCODE IN ('" & OnlineAccode.Replace(",", "','") & "') THEN 'ONLINE' "
                    StrSql += vbCrLf + " WHEN T.ACCODE IN (SELECT PURCHASECODE FROM " & cnChitCompanyid & "SAVINGS..SCHEME WHERE SCHEMEID IN "
                    StrSql += vbCrLf + " (SELECT TOP 1 SCHEMEID FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = T.GROUPCODE AND REGNO = T.REGNO AND COMPANYID IN"
                    StrSql += vbCrLf + " (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))) THEN "
                    StrSql += vbCrLf + " (SELECT NAME FROM " & cnChitCompanyid & "SAVINGS..CREDITCARD WHERE ACCOUNT=T.ACCODE)"
                    StrSql += vbCrLf + " ELSE  'CREDITCARD' END"
                ElseIf OnlineAccode <> "" And FREEINS_BOUNS = True Then
                    StrSql += vbCrLf + " WHEN MODEPAY = 'R' THEN CASE  WHEN S.FREEINSCODE=T.CHQBANKCODE THEN 'BONUS' "
                    StrSql += vbCrLf + " WHEN T.ACCODE IN ('" & OnlineAccode.Replace(",", "','") & "') THEN 'ONLINE' "
                    StrSql += vbCrLf + " WHEN T.ACCODE IN (SELECT PURCHASECODE FROM " & cnChitCompanyid & "SAVINGS..SCHEME WHERE SCHEMEID IN "
                    StrSql += vbCrLf + " (SELECT TOP 1 SCHEMEID FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = T.GROUPCODE AND REGNO = T.REGNO AND COMPANYID IN"
                    StrSql += vbCrLf + " (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))) THEN "
                    StrSql += vbCrLf + " (SELECT NAME FROM " & cnChitCompanyid & "SAVINGS..CREDITCARD WHERE ACCOUNT=T.ACCODE)"
                    StrSql += vbCrLf + " ELSE  'CREDITCARD' END"
                ElseIf FREEINS_BOUNS = False Then
                    StrSql += vbCrLf + " WHEN MODEPAY = 'R' THEN CASE"
                    StrSql += vbCrLf + " WHEN T.ACCODE IN (SELECT PURCHASECODE FROM " & cnChitCompanyid & "SAVINGS..SCHEME WHERE SCHEMEID IN "
                    StrSql += vbCrLf + " (SELECT TOP 1 SCHEMEID FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = T.GROUPCODE AND REGNO = T.REGNO AND COMPANYID IN"
                    StrSql += vbCrLf + " (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))) THEN "
                    StrSql += vbCrLf + " (SELECT NAME FROM " & cnChitCompanyid & "SAVINGS..CREDITCARD WHERE ACCOUNT=T.ACCODE)"
                    StrSql += vbCrLf + " ELSE  'CREDITCARD' END"
                Else
                    StrSql += vbCrLf + " WHEN MODEPAY = 'R' THEN CASE WHEN S.FREEINSCODE=T.CHQBANKCODE THEN 'BONUS' "
                    StrSql += vbCrLf + " WHEN T.ACCODE IN (SELECT PURCHASECODE FROM " & cnChitCompanyid & "SAVINGS..SCHEME WHERE SCHEMEID IN "
                    StrSql += vbCrLf + " (SELECT TOP 1 SCHEMEID FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = T.GROUPCODE AND REGNO = T.REGNO AND COMPANYID IN"
                    StrSql += vbCrLf + " (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))) THEN "
                    StrSql += vbCrLf + " (SELECT NAME FROM " & cnChitCompanyid & "SAVINGS..CREDITCARD WHERE ACCOUNT=T.ACCODE)"
                    StrSql += vbCrLf + " ELSE  'CREDITCARD' END"
                End If
                StrSql += vbCrLf + " WHEN MODEPAY = 'E' THEN 'ETRANSFER'"
                StrSql += vbCrLf + " WHEN MODEPAY = 'O' THEN 'OTHERS'"
                StrSql += vbCrLf + " END CATNAME,"
                StrSql += vbCrLf + " SUM(AMOUNT) AS RECEIPT, 0 AS PAYMENT "
                ''StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
                StrSql += vbCrLf + " FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
                If FREEINS_BOUNS Then
                    StrSql += vbCrLf + " LEFT JOIN " & cnChitCompanyid & "SAVINGS..SCHEMEMAST SM ON T.GROUPCODE = SM.GROUPCODE And T.REGNO = SM.REGNO "
                    StrSql += vbCrLf + " LEFT JOIN " & cnChitCompanyid & "SAVINGS..SCHEME S ON S.SCHEMEID=SM.SCHEMEID "
                    StrSql += vbCrLf + " AND SM.COMPANYID IN"
                    StrSql += vbCrLf + " (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))"
                End If
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                If HOMECOLLECTINBASE = True Then
                    StrSql += StrCostFiltration
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                        StrSql += "AND ISNULL(COSTID,'') NOT IN ('IN')"
                    End If
                Else
                    StrSql += StrCostFiltration
                End If
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))AND T.GROUPCODE = SC.GROUPCODE AND T.REGNO = SC.REGNO)"
                StrSql += vbCrLf + " GROUP BY MODEPAY"
                StrSql += vbCrLf + " ,ACCODE,T.GROUPCODE,T.REGNO "
                If FREEINS_BOUNS Then
                    StrSql += vbCrLf + " ,CHQBANKCODE,FREEINSCODE"
                End If
                StrSql += vbCrLf + " )X GROUP BY CATNAME"
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT CATNAME,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT  FROM"
                StrSql += vbCrLf + " (SELECT  'CC COMMISION' CATNAME,"
                StrSql += vbCrLf + " (SELECT SUM(AMOUNT) FROM " & cnChitCompanyid & "SAVINGS..RECPAY WHERE T.ENTREFNO = EntRefNo) AS RECEIPT, 0 AS PAYMENT "
                StrSql += vbCrLf + " FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND MODEPAY='R' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                If HOMECOLLECTINBASE = True Then
                    StrSql += StrCostFiltration
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                        StrSql += "AND ISNULL(COSTID,'') NOT IN ('IN')"
                    End If
                Else
                    StrSql += StrCostFiltration
                End If
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))AND T.GROUPCODE = SC.GROUPCODE AND T.REGNO = SC.REGNO))X"
                StrSql += vbCrLf + " group by X.CATNAME "
                If HOMECOLLECTINBASE = True Then
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                        StrSql += vbCrLf + " UNION ALL"
                        StrSql += vbCrLf + " SELECT CATNAME,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT  FROM"
                        StrSql += vbCrLf + " ("
                        StrSql += vbCrLf + " SELECT  'HOME COLLECTION' CATNAME,"
                        StrSql += vbCrLf + " SUM(AMOUNT) AS RECEIPT, 0 AS PAYMENT,MODEPAY "
                        StrSql += vbCrLf + " FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
                        StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                        StrSql += vbCrLf + " AND MODEPAY IN ('R','C') "
                        StrSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y'"
                        If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                        StrSql += vbCrLf + "  AND COSTID IN ('IN') "
                        If StrCostFiltration1 <> "" Then StrSql += vbCrLf + "  AND ISNULL(SUBSTRING(GROUPCODE,1,2),'') IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & StrCostFiltration1.Replace("'INTERNET',", "") & "))"
                        StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                        StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))AND T.GROUPCODE = SC.GROUPCODE AND T.REGNO = SC.REGNO)"
                        StrSql += vbCrLf + "  GROUP BY MODEPAY"
                        StrSql += vbCrLf + " )X"
                        StrSql += vbCrLf + " group by X.CATNAME,MODEPAY "
                    End If
                End If
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()

                Dim HomeCollect As Double = 0
                Dim HomeCount As Double = 0
                If HOMECOLLECTINBASE = True Then
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                        StrSql = vbCrLf + " SELECT CATNAME,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT,SUM(RECEIPTCOUNT)RECEIPTCOUNT  FROM"
                        StrSql += vbCrLf + " ("
                        StrSql += vbCrLf + " SELECT  'HOME COLLECTION' CATNAME,"
                        StrSql += vbCrLf + " SUM(AMOUNT) AS RECEIPT, 0 AS PAYMENT,MODEPAY,COUNT(*)RECEIPTCOUNT "
                        StrSql += vbCrLf + " FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
                        StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                        StrSql += vbCrLf + " AND MODEPAY IN ('R','C') "
                        StrSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y'"
                        If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                        StrSql += vbCrLf + "  AND COSTID IN ('IN') "
                        If StrCostFiltration1 <> "" Then StrSql += vbCrLf + "  AND ISNULL(SUBSTRING(GROUPCODE,1,2),'') IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & StrCostFiltration1.Replace("'INTERNET',", "") & "))"
                        StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                        StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))AND T.GROUPCODE = SC.GROUPCODE AND T.REGNO = SC.REGNO)"
                        StrSql += vbCrLf + "  GROUP BY MODEPAY"
                        StrSql += vbCrLf + " )X"
                        StrSql += vbCrLf + " group by X.CATNAME,MODEPAY "
                        Dim dr As DataRow = Nothing
                        dr = GetSqlRow(StrSql, cn)
                        If Not dr Is Nothing Then
                            HomeCollect = Val(dr.Item("RECEIPT").ToString)
                            HomeCount = Val(dr.Item("RECEIPTCOUNT").ToString)
                        End If
                    End If
                End If

                StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION)>0 "
                StrSql += vbCrLf + " BEGIN "
                StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
                StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION,COLHEAD) "
                'StrSql += VBCRLF + " VALUES('SCHEME COLLECTION ','T') "
                If SCHEME_TO_ADVANCE_NAME_CHANGE <> "" Then
                    StrSql += vbCrLf + " SELECT   '" & SCHEME_TO_ADVANCE_NAME_CHANGE & "[' +  CONVERT(VARCHAR,CONVERT(NUMERIC(15,2)," & HomeCollect & "+SUM(AMOUNT)+ISNULL(SUM(TAX),0))) + ']' AS PARTICULAR,'T' "
                Else
                    StrSql += vbCrLf + " SELECT 'SCHEME COLLECTION [' +  CONVERT(VARCHAR,CONVERT(NUMERIC(15,2)," & HomeCollect & "+SUM(AMOUNT)+ISNULL(SUM(TAX),0))) + ']' AS PARTICULAR,'T' "
                End If

                StrSql += vbCrLf + " FROM " & cnChitTrandb & "..SCHEMETRAN "
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                If SCashCounterName <> "" Then
                    StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER "
                    StrSql += vbCrLf + " WHERE CASHNAME IN (" & SCashCounterName & ")) "
                End If
                If HOMECOLLECTINBASE = True Then
                    StrSql += StrCostFiltration
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                        StrSql += " AND ISNULL(COSTID,'') NOT IN ('IN')"
                    End If
                Else
                    StrSql += StrCostFiltration
                End If
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"
                StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) "
                StrSql += vbCrLf + " SELECT PARTICULAR FROM "
                StrSql += vbCrLf + " ("
                StrSql += vbCrLf + " SELECT 'NEW : Rs '+ CASE WHEN SUM(AMOUNT) > 0 THEN CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),SUM(AMOUNT))) + ' ('+CONVERT(VARCHAR,COUNT(*)) + ' NOs)' ELSE '' END AS PARTICULAR FROM " & cnChitTrandb & "..SCHEMETRAN "
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                If HOMECOLLECTINBASE = True Then
                    StrSql += StrCostFiltration
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                        StrSql += "AND ISNULL(COSTID,'') NOT IN ('IN')"
                    End If
                Else
                    StrSql += StrCostFiltration
                End If
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"
                StrSql += vbCrLf + " AND INSTALLMENT = 1"
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT 'OTHER : Rs '+ CASE WHEN SUM(AMOUNT) > 0 THEN CONVERT(VARCHAR,ISNULL(CONVERT(NUMERIC(15,2)," & HomeCollect & "+SUM(AMOUNT)),'')) + ' ('+CONVERT(VARCHAR,ISNULL(COUNT(*),'')) + ' NOs)' ELSE '' END AS PARTICULAR FROM " & cnChitTrandb & "..SCHEMETRAN"
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                If HOMECOLLECTINBASE = True Then
                    StrSql += StrCostFiltration
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                        StrSql += "AND ISNULL(COSTID,'') NOT IN ('IN')"
                    End If
                Else
                    StrSql += StrCostFiltration
                End If
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"
                StrSql += vbCrLf + " AND INSTALLMENT <> 1"
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT 'GST : Rs '+ CASE WHEN SUM(TAX) > 0 THEN CONVERT(VARCHAR,ISNULL(CONVERT(NUMERIC(15,2),SUM(TAX)),'')) + ' ('+CONVERT(VARCHAR," & HomeCount & "+ISNULL(COUNT(*),'')) + ' NOs)' ELSE '' END AS PARTICULAR FROM " & cnChitTrandb & "..SCHEMETRAN"
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                If HOMECOLLECTINBASE = True Then
                    StrSql += StrCostFiltration
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                    End If
                Else
                    StrSql += StrCostFiltration
                End If
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"
                StrSql += vbCrLf + " )C"
                StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
                StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME,  "
                StrSql += vbCrLf + " CASE WHEN RECEIPT<> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
                StrSql += vbCrLf + " CASE WHEN PAYMENT<> 0 THEN PAYMENT  ELSE NULL END PAYMENT "
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION ORDER BY CATNAME "

                StrSql += vbCrLf + " End"
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()

                If RPT_SHOW_SCHEMEWISEDETAILS Then
                    StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION)>0 "
                    StrSql += vbCrLf + " BEGIN "
                    StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
                    StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION,COLHEAD) VALUES('SCHEME NAME','T') "
                    StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION,RECGRSWT) "
                    StrSql += vbCrLf + "  SELECT DISTINCT SCHNAME + ' : RS ' + CONVERT(varchar(20),SUM(AMT)) SCHNAME,SUM(WT)WT FROM ( "
                    StrSql += vbCrLf + "  SELECT (SELECT SCHEMENAME FROM " & cnChitCompanyid & "SAVINGS..SCHEME WHERE SCHEMEID IN "
                    StrSql += vbCrLf + "  (SELECT DISTINCT SCHEMEID FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE=C.GROUPCODE AND REGNO=C.REGNO))SCHNAME "
                    StrSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),AMOUNT)AMT,CONVERT(NUMERIC(15,3),WEIGHT)WT "
                    StrSql += vbCrLf + "  FROM " & cnChitTrandb & "..SCHEMETRAN C "
                    StrSql += vbCrLf + "  WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                    StrSql += vbCrLf + "  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                    StrSql += vbCrLf + "  ISNULL(CANCEL,'') <> 'Y'"
                    StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEME WHERE SCHEMEID IN "
                    StrSql += vbCrLf + "  (SELECT DISTINCT SCHEMEID FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE=C.GROUPCODE) AND WEIGHTLEDGER='Y')"
                    If SCashCounterName <> "" Then
                        StrSql += vbCrLf + "  AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER "
                        StrSql += vbCrLf + "  WHERE CASHNAME IN (" & SCashCounterName & ")) "
                    End If
                    If HOMECOLLECTINBASE = True Then
                        StrSql += StrCostFiltration
                        If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                            StrSql += "  AND ISNULL(COSTID,'') NOT IN ('IN')"
                        End If
                    Else
                        StrSql += StrCostFiltration
                    End If
                    StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                    StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) ) ) X GROUP BY SCHNAME"
                    StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
                    StrSql += vbCrLf + " END"
                    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                    Cmd.ExecuteNonQuery()
                End If

                If SCHEME_TO_ADVANCE_NAME_CHANGE_CASH <> "" Then
                    StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SASRPU SET DESCRIPTION='" & SCHEME_TO_ADVANCE_NAME_CHANGE_CASH & "' WHERE DESCRIPTION='  SCHEME CASH'"
                    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                    Cmd.ExecuteNonQuery()
                End If
            End If
        End If
    End Sub

    Private Sub ProcChitCollection_07_FEB_2022()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITCOLLECTION') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION "
        StrSql += vbCrLf + " SELECT CONVERT(NUMERIC(15,2),0)RECEIPT,CONVERT(NUMERIC(15,2),0)PAYMENT"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If hasChit Then
            StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CHITDBPREFIX'"
            If (Trim(objGPack.GetSqlValue(StrSql, , ""))) <> "" Then
                StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITCOLLECTION') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION "
                StrSql += vbCrLf + " SELECT CATNAME,SUM(RECEIPT)RECEIPT ,SUM(PAYMENT)PAYMENT "
                StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION FROM ("
                StrSql += vbCrLf + " SELECT  CASE "
                StrSql += vbCrLf + " WHEN MODEPAY = 'C' THEN 'SCHEME CASH' "
                StrSql += vbCrLf + " WHEN MODEPAY IN('Q','D') THEN 'CHEQUE' "
                If OnlineAccode <> "" And FREEINS_BOUNS = False Then
                    StrSql += vbCrLf + " WHEN MODEPAY = 'R' THEN CASE WHEN T.ACCODE IN ('" & OnlineAccode.Replace(",", "','") & "') THEN 'ONLINE' ELSE  'CREDITCARD' END"
                ElseIf OnlineAccode <> "" And FREEINS_BOUNS = True Then
                    StrSql += vbCrLf + " WHEN MODEPAY = 'R' THEN CASE  WHEN S.FREEINSCODE=T.CHQBANKCODE THEN 'BONUS' "
                    StrSql += vbCrLf + " WHEN T.ACCODE IN ('" & OnlineAccode.Replace(",", "','") & "') THEN 'ONLINE' ELSE  'CREDITCARD' END"
                ElseIf FREEINS_BOUNS = False Then
                    StrSql += vbCrLf + " WHEN MODEPAY = 'R' THEN 'CREDITCARD'"
                Else
                    StrSql += vbCrLf + " WHEN MODEPAY = 'R' THEN CASE WHEN S.FREEINSCODE=T.CHQBANKCODE THEN 'BONUS' ELSE  'CREDITCARD' END"
                End If
                StrSql += vbCrLf + " WHEN MODEPAY = 'E' THEN 'ETRANSFER'"
                StrSql += vbCrLf + " WHEN MODEPAY = 'O' THEN 'OTHERS'"
                StrSql += vbCrLf + " END CATNAME,"
                StrSql += vbCrLf + " SUM(AMOUNT) AS RECEIPT, 0 AS PAYMENT "
                ''StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
                StrSql += vbCrLf + " FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
                If FREEINS_BOUNS Then
                    StrSql += vbCrLf + " LEFT JOIN " & cnChitCompanyid & "SAVINGS..SCHEMEMAST SM ON T.GROUPCODE = SM.GROUPCODE And T.REGNO = SM.REGNO "
                    StrSql += vbCrLf + " LEFT JOIN " & cnChitCompanyid & "SAVINGS..SCHEME S ON S.SCHEMEID=SM.SCHEMEID "
                    StrSql += vbCrLf + " AND SM.COMPANYID IN"
                    StrSql += vbCrLf + " (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))"
                End If
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                If HOMECOLLECTINBASE = True Then
                    StrSql += StrCostFiltration
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                        StrSql += "AND ISNULL(COSTID,'') NOT IN ('IN')"
                    End If
                Else
                    StrSql += StrCostFiltration
                End If
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))AND T.GROUPCODE = SC.GROUPCODE AND T.REGNO = SC.REGNO)"
                StrSql += vbCrLf + " GROUP BY MODEPAY"
                If OnlineAccode <> "" Then
                    StrSql += vbCrLf + " ,ACCODE"
                End If
                If FREEINS_BOUNS Then
                    StrSql += vbCrLf + " ,CHQBANKCODE,FREEINSCODE"
                End If
                StrSql += vbCrLf + " )X GROUP BY CATNAME"
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT CATNAME,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT  FROM"
                StrSql += vbCrLf + " (SELECT  'CC COMMISION' CATNAME,"
                StrSql += vbCrLf + " (SELECT SUM(AMOUNT) FROM " & cnChitCompanyid & "SAVINGS..RECPAY WHERE T.ENTREFNO = EntRefNo) AS RECEIPT, 0 AS PAYMENT "
                StrSql += vbCrLf + " FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND MODEPAY='R' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                If HOMECOLLECTINBASE = True Then
                    StrSql += StrCostFiltration
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                        StrSql += "AND ISNULL(COSTID,'') NOT IN ('IN')"
                    End If
                Else
                    StrSql += StrCostFiltration
                End If
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))AND T.GROUPCODE = SC.GROUPCODE AND T.REGNO = SC.REGNO))X"
                StrSql += vbCrLf + " group by X.CATNAME "
                If HOMECOLLECTINBASE = True Then
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                        StrSql += vbCrLf + " UNION ALL"
                        StrSql += vbCrLf + " SELECT CATNAME,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT  FROM"
                        StrSql += vbCrLf + " ("
                        StrSql += vbCrLf + " SELECT  'HOME COLLECTION' CATNAME,"
                        StrSql += vbCrLf + " SUM(AMOUNT) AS RECEIPT, 0 AS PAYMENT,MODEPAY "
                        StrSql += vbCrLf + " FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
                        StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                        StrSql += vbCrLf + " AND MODEPAY IN ('R','C') "
                        StrSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y'"
                        If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                        StrSql += vbCrLf + "  AND COSTID IN ('IN') "
                        If StrCostFiltration1 <> "" Then StrSql += vbCrLf + "  AND ISNULL(SUBSTRING(GROUPCODE,1,2),'') IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & StrCostFiltration1.Replace("'INTERNET',", "") & "))"
                        StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                        StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))AND T.GROUPCODE = SC.GROUPCODE AND T.REGNO = SC.REGNO)"
                        StrSql += vbCrLf + "  GROUP BY MODEPAY"
                        StrSql += vbCrLf + " )X"
                        StrSql += vbCrLf + " group by X.CATNAME,MODEPAY "
                    End If
                End If
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()

                Dim HomeCollect As Double = 0
                Dim HomeCount As Double = 0
                If HOMECOLLECTINBASE = True Then
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                        StrSql = vbCrLf + " SELECT CATNAME,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT,SUM(RECEIPTCOUNT)RECEIPTCOUNT  FROM"
                        StrSql += vbCrLf + " ("
                        StrSql += vbCrLf + " SELECT  'HOME COLLECTION' CATNAME,"
                        StrSql += vbCrLf + " SUM(AMOUNT) AS RECEIPT, 0 AS PAYMENT,MODEPAY,COUNT(*)RECEIPTCOUNT "
                        StrSql += vbCrLf + " FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
                        StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                        StrSql += vbCrLf + " AND MODEPAY IN ('R','C') "
                        StrSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y'"
                        If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                        StrSql += vbCrLf + "  AND COSTID IN ('IN') "
                        If StrCostFiltration1 <> "" Then StrSql += vbCrLf + "  AND ISNULL(SUBSTRING(GROUPCODE,1,2),'') IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & StrCostFiltration1.Replace("'INTERNET',", "") & "))"
                        StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                        StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))AND T.GROUPCODE = SC.GROUPCODE AND T.REGNO = SC.REGNO)"
                        StrSql += vbCrLf + "  GROUP BY MODEPAY"
                        StrSql += vbCrLf + " )X"
                        StrSql += vbCrLf + " group by X.CATNAME,MODEPAY "
                        Dim dr As DataRow = Nothing
                        dr = GetSqlRow(StrSql, cn)
                        If Not dr Is Nothing Then
                            HomeCollect = Val(dr.Item("RECEIPT").ToString)
                            HomeCount = Val(dr.Item("RECEIPTCOUNT").ToString)
                        End If
                    End If
                End If

                StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION)>0 "
                StrSql += vbCrLf + " BEGIN "
                StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
                StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION,COLHEAD) "
                'StrSql += VBCRLF + " VALUES('SCHEME COLLECTION ','T') "
                If SCHEME_TO_ADVANCE_NAME_CHANGE <> "" Then
                    StrSql += vbCrLf + " SELECT   '" & SCHEME_TO_ADVANCE_NAME_CHANGE & "[' +  CONVERT(VARCHAR,CONVERT(NUMERIC(15,2)," & HomeCollect & "+SUM(AMOUNT)+ISNULL(SUM(TAX),0))) + ']' AS PARTICULAR,'T' "
                Else
                    StrSql += vbCrLf + " SELECT 'SCHEME COLLECTION [' +  CONVERT(VARCHAR,CONVERT(NUMERIC(15,2)," & HomeCollect & "+SUM(AMOUNT)+ISNULL(SUM(TAX),0))) + ']' AS PARTICULAR,'T' "
                End If

                StrSql += vbCrLf + " FROM " & cnChitTrandb & "..SCHEMETRAN "
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                If SCashCounterName <> "" Then
                    StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER "
                    StrSql += vbCrLf + " WHERE CASHNAME IN (" & SCashCounterName & ")) "
                End If
                If HOMECOLLECTINBASE = True Then
                    StrSql += StrCostFiltration
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                        StrSql += " AND ISNULL(COSTID,'') NOT IN ('IN')"
                    End If
                Else
                    StrSql += StrCostFiltration
                End If
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"
                StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) "
                StrSql += vbCrLf + " SELECT PARTICULAR FROM "
                StrSql += vbCrLf + " ("
                StrSql += vbCrLf + " SELECT 'NEW : Rs '+ CASE WHEN SUM(AMOUNT) > 0 THEN CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),SUM(AMOUNT))) + ' ('+CONVERT(VARCHAR,COUNT(*)) + ' NOs)' ELSE '' END AS PARTICULAR FROM " & cnChitTrandb & "..SCHEMETRAN "
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                If HOMECOLLECTINBASE = True Then
                    StrSql += StrCostFiltration
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                        StrSql += "AND ISNULL(COSTID,'') NOT IN ('IN')"
                    End If
                Else
                    StrSql += StrCostFiltration
                End If
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"
                StrSql += vbCrLf + " AND INSTALLMENT = 1"
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT 'OTHER : Rs '+ CASE WHEN SUM(AMOUNT) > 0 THEN CONVERT(VARCHAR,ISNULL(CONVERT(NUMERIC(15,2)," & HomeCollect & "+SUM(AMOUNT)),'')) + ' ('+CONVERT(VARCHAR,ISNULL(COUNT(*),'')) + ' NOs)' ELSE '' END AS PARTICULAR FROM " & cnChitTrandb & "..SCHEMETRAN"
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                If HOMECOLLECTINBASE = True Then
                    StrSql += StrCostFiltration
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                        StrSql += "AND ISNULL(COSTID,'') NOT IN ('IN')"
                    End If
                Else
                    StrSql += StrCostFiltration
                End If
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"
                StrSql += vbCrLf + " AND INSTALLMENT <> 1"
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT 'GST : Rs '+ CASE WHEN SUM(TAX) > 0 THEN CONVERT(VARCHAR,ISNULL(CONVERT(NUMERIC(15,2),SUM(TAX)),'')) + ' ('+CONVERT(VARCHAR," & HomeCount & "+ISNULL(COUNT(*),'')) + ' NOs)' ELSE '' END AS PARTICULAR FROM " & cnChitTrandb & "..SCHEMETRAN"
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                StrSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
                If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                If HOMECOLLECTINBASE = True Then
                    StrSql += StrCostFiltration
                    If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                    End If
                Else
                    StrSql += StrCostFiltration
                End If
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"
                StrSql += vbCrLf + " )C"
                StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
                StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT) "
                StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME,  "
                StrSql += vbCrLf + " CASE WHEN RECEIPT<> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
                StrSql += vbCrLf + " CASE WHEN PAYMENT<> 0 THEN PAYMENT  ELSE NULL END PAYMENT "
                StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION ORDER BY CATNAME "

                StrSql += vbCrLf + " End"
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()

                If RPT_SHOW_SCHEMEWISEDETAILS Then
                    StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "CHITCOLLECTION)>0 "
                    StrSql += vbCrLf + " BEGIN "
                    StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
                    StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION,COLHEAD) VALUES('SCHEME NAME','T') "
                    StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION,RECGRSWT) "
                    StrSql += vbCrLf + "  SELECT DISTINCT SCHNAME + ' : RS ' + CONVERT(varchar(20),SUM(AMT)) SCHNAME,SUM(WT)WT FROM ( "
                    StrSql += vbCrLf + "  SELECT (SELECT SCHEMENAME FROM " & cnChitCompanyid & "SAVINGS..SCHEME WHERE SCHEMEID IN "
                    StrSql += vbCrLf + "  (SELECT DISTINCT SCHEMEID FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE=C.GROUPCODE AND REGNO=C.REGNO))SCHNAME "
                    StrSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),AMOUNT)AMT,CONVERT(NUMERIC(15,3),WEIGHT)WT "
                    StrSql += vbCrLf + "  FROM " & cnChitTrandb & "..SCHEMETRAN C "
                    StrSql += vbCrLf + "  WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                    StrSql += vbCrLf + "  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                    StrSql += vbCrLf + "  ISNULL(CANCEL,'') <> 'Y'"
                    StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEME WHERE SCHEMEID IN "
                    StrSql += vbCrLf + "  (SELECT DISTINCT SCHEMEID FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE=C.GROUPCODE) AND WEIGHTLEDGER='Y')"
                    If SCashCounterName <> "" Then
                        StrSql += vbCrLf + "  AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER "
                        StrSql += vbCrLf + "  WHERE CASHNAME IN (" & SCashCounterName & ")) "
                    End If
                    If HOMECOLLECTINBASE = True Then
                        StrSql += StrCostFiltration
                        If (StrCostFiltration1.Contains("INTERNET") And StrCostFiltration1.Split(",").Length > 1) Or StrCostFiltration1 = "" Then
                            StrSql += "  AND ISNULL(COSTID,'') NOT IN ('IN')"
                        End If
                    Else
                        StrSql += StrCostFiltration
                    End If
                    StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                    StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) ) ) X GROUP BY SCHNAME"
                    StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
                    StrSql += vbCrLf + " END"
                    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                    Cmd.ExecuteNonQuery()
                End If

                If SCHEME_TO_ADVANCE_NAME_CHANGE_CASH <> "" Then
                    StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SASRPU SET DESCRIPTION='" & SCHEME_TO_ADVANCE_NAME_CHANGE_CASH & "' WHERE DESCRIPTION='  SCHEME CASH'"
                    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                    Cmd.ExecuteNonQuery()
                End If
            End If
        End If
    End Sub

    Private Sub ProcOrderBooking()
        StrSql = "  If EXISTS(Select 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ORDERBOOK') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ORDERBOOK"
        StrSql += vbCrLf + " SELECT M.METALNAME AS CATNAME "
        StrSql += vbCrLf + " ,SUM(PCS) AS RECPCS,SUM(O.GRSWT) AS RECGRSWT,SUM(NETWT) AS RECNETWT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ORDERBOOK FROM " & cnAdminDb & "..ORMAST O"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON O.ITEMID = I.ITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
        StrSql += vbCrLf + " WHERE ORDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND ORTYPE = 'O' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND O.COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrCostFiltration
        StrSql += StrUseridFtr
        StrSql += StrFilter
        StrSql += vbCrLf + " GROUP BY METALNAME"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ORDERBOOK)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) VALUES('ORDER BOOKING','T') "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECPCS,RECGRSWT,RECNETWT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
        StrSql += vbCrLf + " CASE WHEN RECPCS    <> 0 THEN RECPCS    ELSE NULL END RECPCS, "
        StrSql += vbCrLf + " CASE WHEN RECGRSWT  <> 0 THEN RECGRSWT  ELSE NULL END RECGRSWT, "
        StrSql += vbCrLf + " CASE WHEN RECNETWT  <> 0 THEN RECNETWT  ELSE NULL END RECNETWT, "
        StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ORDERBOOK ORDER BY RESULT1 "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


    End Sub


    Private Sub ProcOrderAdvanceAmountToWeight()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABS_ORDAMTWT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABS_ORDAMTWT "
        StrSql += vbCrLf + " SELECT "
        If chkAdvwtdetail.Checked Then
            StrSql += vbCrLf + " TRANNO,'REF NO : (' + ISNULL((SELECT TOP 1 SUBSTRING(ORNO,6,20) FROM " & cnAdminDb & "..ORMAST WHERE BATCHNO = I.BATCHNO),'') + ') ' + CONVERT(VARCHAR,GRSWT) + '@' + CONVERT(VARCHAR,RATE) AS CATNAME"
            StrSql += vbCrLf + " ,PCS AS RECPCS, GRSWT AS RECGRSWT, NETWT AS RECNETWT,0 PAYMENT,"
        Else
            StrSql += vbCrLf + " 0 TRANNO, ISNULL((SELECT TOP 1 metalname FROM " & cnAdminDb & "..METALMAST M WHERE M.METALID = I.METALID),'') AS CATNAME"
            StrSql += vbCrLf + " ,SUM(PCS) AS RECPCS, SUM(GRSWT) AS RECGRSWT, SUM(NETWT) AS RECNETWT,0 PAYMENT,"
        End If
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABS_ORDAMTWT FROM " & cnStockDb & "..RECEIPT I"
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND I.TRANTYPE = 'AD' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        If Not chkAdvwtdetail.Checked Then StrSql += vbCrLf + " GROUP BY I.METALID"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABS_ORDAMTWT)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) VALUES('ORDER ADV REC AMT TO WT','T') "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECPCS,RECGRSWT,RECNETWT,RECEIPT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
        StrSql += vbCrLf + " CASE WHEN RECPCS    <> 0 THEN RECPCS    ELSE NULL END RECPCS, "
        StrSql += vbCrLf + " CASE WHEN RECGRSWT  <> 0 THEN RECGRSWT  ELSE NULL END RECGRSWT, "
        StrSql += vbCrLf + " CASE WHEN RECNETWT  <> 0 THEN RECNETWT  ELSE NULL END RECNETWT, "
        StrSql += vbCrLf + " PAYMENT,"
        StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABS_ORDAMTWT ORDER BY RESULT1,TRANNO "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


    End Sub

    Private Sub ProcApprovalIssue()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSAPPROVALISS') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALISS "
        StrSql += vbCrLf + " SELECT TRANNO"
        StrSql += vbCrLf + " ,(SELECT (PNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))PARTYNAME"
        StrSql += vbCrLf + " , (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        StrSql += vbCrLf + " WHERE ITEMID = I.ITEMID)+CASE WHEN ISNULL(TAGNO,'') <> '' THEN ' - TAGNO('+TAGNO+')' ELSE '' END AS CATNAME, "
        StrSql += vbCrLf + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " ,(SELECT (PNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))PNAME"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALISS FROM " & cnStockDb & "..ISSUE I"
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND I.TRANTYPE = 'AI' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " GROUP BY ITEMID,TAGNO,TRANNO,BATCHNO"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALISS)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALISS(PNAME,CATNAME,RESULT1,COLHEAD)"
        StrSql += vbCrLf + " SELECT DISTINCT PNAME,PNAME,0 RESULT1,'P' COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALISS"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If chkDetaledApproval.Checked = True Then
            StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALISS)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) VALUES('APPROVAL ISSUE','T') "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS    <> 0 THEN ISSPCS    ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT  <> 0 THEN ISSGRSWT  ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT  <> 0 THEN ISSNETWT  ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " 1 RESULT1,COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALISS ORDER BY PNAME,PARTYNAME,CATNAME "
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALISS)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT 'APPROVAL ISSUE', "
            StrSql += vbCrLf + " CASE WHEN SUM(ISSPCS)    <> 0 THEN SUM(ISSPCS)    ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN sum(ISSGRSWT)  <> 0 THEN sum(ISSGRSWT)  ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN sum(ISSNETWT)  <> 0 THEN sum(ISSNETWT)  ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " 1 RESULT1, 'T' COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALISS"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub ProcApprovalReceipt()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSAPPROVALREC') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALREC "
        StrSql += vbCrLf + " SELECT TRANNO"
        StrSql += vbCrLf + " ,(SELECT (PNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))PARTYNAME"
        StrSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        StrSql += vbCrLf + " WHERE ITEMID = I.ITEMID)+CASE WHEN ISNULL(TAGNO,'') <> '' THEN ' - TAGNO('+TAGNO+')' ELSE '' END + CASE WHEN ISNULL(RUNNO,'') <> '' THEN ' [RUNNO : ' +  SUBSTRING(RUNNO,6,20) + ']' ELSE '' END AS CATNAME, "
        StrSql += vbCrLf + " SUM(PCS) AS RECPCS, SUM(GRSWT) AS RECGRSWT, SUM(NETWT) AS RECNETWT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " ,(SELECT (PNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))PNAME"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALREC FROM " & cnStockDb & "..RECEIPT I"
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND I.TRANTYPE = 'AR' AND ISNULL(CANCEL,'') = ''"
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " GROUP BY ITEMID,TAGNO,TRANNO,RUNNO,BATCHNO"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALREC)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALREC(PNAME,CATNAME,RESULT1,COLHEAD)"
        StrSql += vbCrLf + " SELECT DISTINCT PNAME,PNAME,0 RESULT1,'P' COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALREC"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If chkDetaledApproval.Checked = True Then
            StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALREC)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) VALUES('APPROVAL RECEIPT','T') "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECPCS,RECGRSWT,RECNETWT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN RECPCS    <> 0 THEN RECPCS    ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT  <> 0 THEN RECGRSWT  ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT  <> 0 THEN RECNETWT  ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALREC ORDER BY PNAME,PARTYNAME,CATNAME "
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALREC)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RECPCS,RECGRSWT,RECNETWT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT 'APPROVAL RECEIPT', "
            StrSql += vbCrLf + " CASE WHEN SUM(RECPCS)    <> 0 THEN SUM(RECPCS)    ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN SUM(RECGRSWT)  <> 0 THEN SUM(RECGRSWT)  ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN SUM(RECNETWT)  <> 0 THEN SUM(RECNETWT)  ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " 1 RESULT1, 'T' COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSAPPROVALREC "
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub
    Private Sub ProcStockDetail()
        If chkstockdetail.Checked Then
            Dim selectedcostid As String = GetSelectedCostId(chklstCostCentre, True)
            StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "STOCKDETAIL') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "STOCKDETAIL "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
            StrSql = vbCrLf + " SELECT CATCODE,CATNAME,SUM(ISSPCS)ISSPCS,SUM(ISSGRSWT)ISSGRSWT,SUM(ISSNETWT)ISSNETWT"
            StrSql += vbCrLf + " ,SUM(RECPCS)RECPCS,SUM(RECGRSWT)RECGRSWT,SUM(RECNETWT) RECNETWT,RECEIPT,PAYMENT,RESULT,RESULT1,AVERAGE,RATE,COLHEAD"
            StrSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & "STOCKDETAIL FROM("
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

            StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "STOCKDETAIL)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) VALUES('STOCK RECEIPT:','T') "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,COLHEAD,RESULT1) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " ''COLHEAD, RESULT1 "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "STOCKDETAIL "
            If rbtCategoryWise.Checked = True Then
                StrSql += vbCrLf + " ORDER BY CATCODE,RESULT1"
            Else
                StrSql += vbCrLf + " ORDER BY CATNAME,RESULT1"
            End If
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
            StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "STOCKDETAIL)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,COLHEAD,RESULT1,ORDERNO,COLHEAD1)"
            StrSql += vbCrLf + "SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0))"
            StrSql += vbCrLf + ",SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),'S' COLHEAD,1,2,'M' COLHEAD1"
            StrSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "STOCKDETAIL"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub
    Private Sub ProcLOTDetail()
        If chkstockdetail.Checked Then
            Dim selectedcostid As String = GetSelectedCostId(chklstCostCentre, True)
            StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "LOTDETAIL') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "LOTDETAIL"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
            StrSql = vbCrLf + " SELECT '  ' + METALNAME AS PARTICULAR,DESIGNER,METALID,METALNAME,SUM(ISSPCS)ISSPCS,SUM(ISSGRSWT)ISSGRSWT,SUM(ISSNETWT)ISSNETWT"
            StrSql += vbCrLf + " ,SUM(RECPCS)RECPCS,SUM(RECGRSWT)RECGRSWT,SUM(RECNETWT) RECNETWT,RECEIPT,PAYMENT,RESULT1,AVERAGE,RATE,COLHEAD"
            StrSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & "LOTDETAIL FROM("
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

            StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "LOTDETAIL)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "LOTDETAIL (PARTICULAR,DESIGNER,RESULT1,COLHEAD)"
            StrSql += vbCrLf + " SELECT DISTINCT DESIGNER,DESIGNER,0 RESULT1,'D' FROM TEMPTABLEDB..TEMP" & systemId & "LOTDETAIL"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) VALUES('LOT DETAIL:','T') "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "LOTDETAIL"
            StrSql += vbCrLf + " ORDER BY DESIGNER,RESULT1,METALNAME"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
            StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "STOCKDETAIL)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,COLHEAD,RESULT1,ORDERNO,COLHEAD1)"
            StrSql += vbCrLf + "SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0))"
            StrSql += vbCrLf + ",SUM(ISNULL(RECPCS,0)),SUM(ISNULL(RECGRSWT,0)),SUM(ISNULL(RECNETWT,0)),'S' COLHEAD,1,2,'M' COLHEAD1"
            StrSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "LOTDETAIL"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub
    Private Sub ProcWsSalesamt()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSWSSALE') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSWSSALE "

        StrSql += vbCrLf + " SELECT  'Ws Amount' Catname,"
        StrSql += vbCrLf + " sum(amount) AMT,1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSWSSALE  "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN I "
        StrSql += vbCrLf + " WHERE I.TRANMODE = 'D' AND I.TRANFLAG = 'CA' AND FROMFLAG = 'P' AND I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''   "
        StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND ACCODE IN (SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH')"
        StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += StrFilter
        StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        StrSql += vbCrLf + " having SUM(amount) > 0 "
        StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSWSSALE)>0 "
        StrSql += vbCrLf + " BEGIN "
        'StrSql += VBCRLF + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + " SELECT 'WS BILL ' + CATNAME CATNAME,  "
        StrSql += vbCrLf + " CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT,  "
        StrSql += vbCrLf + " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSWSSALE ORDER BY CATNAME  "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcPartlySales()
        If (RPT_ME_SUM_DABS = True And rbtMetalwise.Checked = True) Then Exit Sub
        If rbtmetalwisegrp.Checked = False Then
            StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPARTLYSALE') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE "
            ''StrSql += VBCRLF + " SELECT CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD,STNPCS,STNWT"
            ''StrSql += VBCRLF + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE FROM ("
            ''If chkCatShortname.Checked = True Then
            ''    StrSql += VBCRLF + " SELECT (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
            ''Else
            ''    StrSql += VBCRLF + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            ''End If
            ''StrSql += VBCRLF + " WHERE CATCODE = I.CATCODE)AS CATNAME  "
            ''StrSql += VBCRLF + " ,SUM(I.PCS) AS ISSPCS, SUM(I.GRSWT) AS ISSGRSWT"
            ''StrSql += VBCRLF + " ,SUM(I.NETWT) AS ISSNETWT,   1 AS RESULT1, ' ' AS COLHEAD  "
            ''StrSql += VBCRLF + " ,(SELECT ISNULL(SUM(STNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNPCS"
            ''StrSql += VBCRLF + " ,(SELECT ISNULL(SUM(STNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNWT"
            ''StrSql += VBCRLF + " ,(SELECT ISNULL(SUM(TAGSTNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNPCS"
            ''StrSql += VBCRLF + " ,(SELECT ISNULL(SUM(TAGSTNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNWT"
            ''StrSql += VBCRLF + " ,SUM(TAGPCS) TAGPCS, SUM(TAGGRSWT) TAGGRSWT,SUM(TAGNETWT) TAGNETWT"
            ''StrSql += VBCRLF + " FROM " & cnStockDb & "..ISSUE I"
            ''StrSql += VBCRLF + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            ''StrSql += VBCRLF + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            ''StrSql += VBCRLF + " AND  I.TRANTYPE = 'SA' "
            ''StrSql += VBCRLF + " AND ISNULL(I.CANCEL,'') = ''   AND I.COMPANYID IN (" & SelectedCompanyId & ")"
            ''StrSql += VBCRLF + " AND ISNULL(TAGNO,'') <> ''"
            ''StrSql += VBCRLF + " GROUP BY CATCODE,TRANNO,SNO"
            StrSql += vbCrLf + " SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
            StrSql += vbCrLf + " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
            StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE FROM ("
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + " SELECT (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
            Else
                StrSql += vbCrLf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            End If
            StrSql += vbCrLf + " WHERE CATCODE = I.CATCODE)AS CATNAME   "
            StrSql += vbCrLf + " ,I.PCS AS ISSPCS, I.GRSWT AS ISSGRSWT, I.NETWT AS ISSNETWT "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(STNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNPCS "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(TAGSTNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNPCS "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(STNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO )STNWT "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(TAGSTNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNWT "
            StrSql += vbCrLf + " ,TAGPCS,TAGGRSWT,TAGNETWT FROM " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
            StrSql += vbCrLf + " AND I.TAGNO <> ''"
            StrSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''   "
            StrSql += vbCrLf + " AND TRANTYPE <> 'AI'   "
            StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
            StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += StrFilter
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
            StrSql += vbCrLf + " ) X "
            StrSql += vbCrLf + " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
            StrSql += vbCrLf + " ) GROUP BY CATNAME"
            StrSql += vbCrLf + " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
            If RPT_SEPVAT_DABS Then
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
                StrSql += vbCrLf + " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
                StrSql += vbCrLf + " FROM ("
                If chkCatShortname.Checked = True Then
                    StrSql += vbCrLf + " SELECT (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
                Else
                    StrSql += vbCrLf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
                End If
                StrSql += vbCrLf + " WHERE CATCODE = ISS.CATCODE)AS CATNAME   "
                StrSql += vbCrLf + " ,ISS.STNPCS AS ISSPCS, ISS.STNWT AS ISSGRSWT, ISS.STNWT AS ISSNETWT "
                StrSql += vbCrLf + " ,STNPCS "
                StrSql += vbCrLf + " ,TAGSTNPCS "
                StrSql += vbCrLf + " ,STNWT "
                StrSql += vbCrLf + " ,TAGSTNWT "
                StrSql += vbCrLf + " ,TAGSTNPCS AS TAGPCS,TAGSTNWT AS TAGGRSWT,TAGSTNWT AS TAGNETWT FROM " & cnStockDb & "..ISSSTONE ISS "
                StrSql += vbCrLf + " WHERE ISS.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND ISSSNO IN(SELECT SNO FROM  " & cnStockDb & "..ISSUE I  "
                StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
                StrSql += vbCrLf + " AND TRANTYPE <> 'AI'   "
                StrSql += vbCrLf + " AND I.TAGNO <> ''"
                StrSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''   "
                StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
                StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
                StrSql += StrFilter
                StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
                StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
                StrSql += vbCrLf + " ) ) X "
                StrSql += vbCrLf + " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
                StrSql += vbCrLf + " ) GROUP BY CATNAME"
                StrSql += vbCrLf + " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
            End If
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPARTLYSALE') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE "
            StrSql += vbCrLf + " SELECT METALID,CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
            StrSql += vbCrLf + " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
            StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE FROM ("
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + " SELECT METALID,(SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
            Else
                StrSql += vbCrLf + " SELECT METALID,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            End If
            StrSql += vbCrLf + " WHERE CATCODE = I.CATCODE)AS CATNAME   "
            StrSql += vbCrLf + " ,I.PCS AS ISSPCS, I.GRSWT AS ISSGRSWT, I.NETWT AS ISSNETWT "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(STNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNPCS "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(TAGSTNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNPCS "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(STNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO )STNWT "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(TAGSTNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNWT "
            StrSql += vbCrLf + " ,TAGPCS,TAGGRSWT,TAGNETWT FROM " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
            StrSql += vbCrLf + " AND I.TAGNO <> ''"
            StrSql += vbCrLf + " AND I.TRANTYPE <> 'AI'"
            StrSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''   "
            StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
            StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += StrFilter
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " ) X "
            StrSql += vbCrLf + " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
            StrSql += vbCrLf + " ) GROUP BY CATNAME,METALID"
            StrSql += vbCrLf + " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
            If RPT_SEPVAT_DABS Then
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT METALID,CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
                StrSql += vbCrLf + " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
                StrSql += vbCrLf + " FROM ("
                If chkCatShortname.Checked = True Then
                    StrSql += vbCrLf + " SELECT IM.METALID,(SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
                Else
                    StrSql += vbCrLf + " SELECT IM.METALID,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
                End If
                StrSql += vbCrLf + " WHERE CATCODE = ISS.CATCODE)AS CATNAME   "
                StrSql += vbCrLf + " ,ISS.STNPCS AS ISSPCS, ISS.STNWT AS ISSGRSWT, ISS.STNWT AS ISSNETWT "
                StrSql += vbCrLf + " ,STNPCS "
                StrSql += vbCrLf + " ,TAGSTNPCS "
                StrSql += vbCrLf + " ,STNWT "
                StrSql += vbCrLf + " ,TAGSTNWT "
                StrSql += vbCrLf + " ,TAGSTNPCS AS TAGPCS,TAGSTNWT AS TAGGRSWT,TAGSTNWT AS TAGNETWT FROM " & cnStockDb & "..ISSSTONE ISS "
                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID=ISS.STNITEMID"
                StrSql += vbCrLf + " WHERE ISS.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..ISSUE I "
                StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
                StrSql += vbCrLf + " AND I.TAGNO <> ''"
                StrSql += vbCrLf + " AND I.TRANTYPE <> 'AI'"
                StrSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''   "
                StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
                StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
                StrSql += StrFilter
                StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
                StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
                StrSql += vbCrLf + " ) ) X "
                StrSql += vbCrLf + " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
                StrSql += vbCrLf + " ) GROUP BY CATNAME,METALID"
                StrSql += vbCrLf + " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
            End If
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        'StrSql += VBCRLF + " WHERE CATCODE = I.CATCODE)AS CATNAME, "
        'StrSql += VBCRLF + " SUM(I.PCS) AS ISSPCS, SUM(I.GRSWT) AS ISSGRSWT, SUM(I.NETWT) AS ISSNETWT,  "
        'StrSql += VBCRLF + " 1 AS RESULT1, ' ' AS COLHEAD "
        'StrSql += VBCRLF + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE FROM "
        'StrSql += cnStockDb & "..ISSUE I, " & cnAdminDb & "..ITEMTAG T "
        'StrSql += VBCRLF + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        'StrSql += VBCRLF + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        'StrSql += VBCRLF + " AND  I.TRANTYPE = 'SA' AND ISNULL(I.CANCEL,'') = ''  "
        'StrSql += VBCRLF + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        'StrSql += VBCRLF + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO AND"
        'StrSql += VBCRLF + " ((T.GRSWT-I.GRSWT)<> 0 OR (T.NETWT-I.NETWT)<>0)"
        'StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        'StrSql += StrFilter
        'StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
        'StrSql += VBCRLF + " GROUP BY CATCODE"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        If rbtmetalwisegrp.Checked = False Then
            StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) VALUES('PARTLY SALES','T') "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS    <> 0 THEN ISSPCS    ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT  <> 0 THEN ISSGRSWT  ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT  <> 0 THEN ISSNETWT  ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE ORDER BY RESULT1 "
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE)>0 "
            StrSql += vbCrLf + " BEGIN "
            'StrSql += VBCRLF + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            'StrSql += VBCRLF + " (DESCRIPTION, COLHEAD) VALUES('PARTLY SALES','T') "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RESULT1,CATCODE,COLHEAD,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT '  PARTLY SALES'  CATNAME, "
            StrSql += vbCrLf + " 1 RESULT1,METALID,'L' COLHEAD,6,'C' COLHEAD1  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE GROUP BY METALID,COLHEAD ORDER BY RESULT1 "

            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,CATCODE,COLHEAD,ORDERNO) "
            StrSql += vbCrLf + " SELECT CATNAME, "
            StrSql += vbCrLf + " CASE WHEN SUM(ISNULL(ISSPCS,0))  <> 0 THEN SUM(ISNULL(ISSPCS,0)) ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN SUM(ISNULL(ISSGRSWT,0))  <> 0 THEN SUM(ISNULL(ISSGRSWT,0))  ELSE NULL END ISSGRSWT,  "
            StrSql += vbCrLf + " CASE WHEN SUM(ISNULL(ISSNETWT,0))  <> 0 THEN SUM(ISNULL(ISSNETWT,0))  ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " 1 RESULT1,METALID,'L' COLHEAD,7  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE GROUP BY METALID,COLHEAD,CATNAME ORDER BY RESULT1 "
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
            StrSql += vbCrLf + " '' AVERAGE,METALID,'L' COLHEAD,1,8,'M' COLHEAD1 "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE GROUP BY METALID,COLHEAD,RESULT1"
            StrSql += vbCrLf + " ORDER BY RESULT1"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub
    Private Sub ProcPartlySales_AI()
        If (RPT_ME_SUM_DABS = True And rbtMetalwise.Checked = True) Then Exit Sub
        If rbtmetalwisegrp.Checked = False Then
            StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPARTLYSALE') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE "
            ''StrSql += VBCRLF + " SELECT CATNAME,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD,STNPCS,STNWT"
            ''StrSql += VBCRLF + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE FROM ("
            ''If chkCatShortname.Checked = True Then
            ''    StrSql += VBCRLF + " SELECT (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
            ''Else
            ''    StrSql += VBCRLF + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            ''End If
            ''StrSql += VBCRLF + " WHERE CATCODE = I.CATCODE)AS CATNAME  "
            ''StrSql += VBCRLF + " ,SUM(I.PCS) AS ISSPCS, SUM(I.GRSWT) AS ISSGRSWT"
            ''StrSql += VBCRLF + " ,SUM(I.NETWT) AS ISSNETWT,   1 AS RESULT1, ' ' AS COLHEAD  "
            ''StrSql += VBCRLF + " ,(SELECT ISNULL(SUM(STNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNPCS"
            ''StrSql += VBCRLF + " ,(SELECT ISNULL(SUM(STNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNWT"
            ''StrSql += VBCRLF + " ,(SELECT ISNULL(SUM(TAGSTNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNPCS"
            ''StrSql += VBCRLF + " ,(SELECT ISNULL(SUM(TAGSTNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNWT"
            ''StrSql += VBCRLF + " ,SUM(TAGPCS) TAGPCS, SUM(TAGGRSWT) TAGGRSWT,SUM(TAGNETWT) TAGNETWT"
            ''StrSql += VBCRLF + " FROM " & cnStockDb & "..ISSUE I"
            ''StrSql += VBCRLF + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            ''StrSql += VBCRLF + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            ''StrSql += VBCRLF + " AND  I.TRANTYPE = 'SA' "
            ''StrSql += VBCRLF + " AND ISNULL(I.CANCEL,'') = ''   AND I.COMPANYID IN (" & SelectedCompanyId & ")"
            ''StrSql += VBCRLF + " AND ISNULL(TAGNO,'') <> ''"
            ''StrSql += VBCRLF + " GROUP BY CATCODE,TRANNO,SNO"
            StrSql += vbCrLf + " SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
            StrSql += vbCrLf + " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
            StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE FROM ("
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + " SELECT (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
            Else
                StrSql += vbCrLf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            End If
            StrSql += vbCrLf + " WHERE CATCODE = I.CATCODE)AS CATNAME   "
            StrSql += vbCrLf + " ,I.PCS AS ISSPCS, I.GRSWT AS ISSGRSWT, I.NETWT AS ISSNETWT "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(STNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNPCS "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(TAGSTNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNPCS "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(STNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO )STNWT "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(TAGSTNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNWT "
            StrSql += vbCrLf + " ,TAGPCS,TAGGRSWT,TAGNETWT FROM " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
            StrSql += vbCrLf + " AND I.TAGNO <> ''"
            StrSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''   "
            StrSql += vbCrLf + " AND TRANTYPE = 'AI'   "
            StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
            StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += StrFilter
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
            StrSql += vbCrLf + " ) X "
            StrSql += vbCrLf + " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
            StrSql += vbCrLf + " ) GROUP BY CATNAME"
            StrSql += vbCrLf + " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
            If RPT_SEPVAT_DABS Then
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
                StrSql += vbCrLf + " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
                StrSql += vbCrLf + " FROM ("
                If chkCatShortname.Checked = True Then
                    StrSql += vbCrLf + " SELECT (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
                Else
                    StrSql += vbCrLf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
                End If
                StrSql += vbCrLf + " WHERE CATCODE = ISS.CATCODE)AS CATNAME   "
                StrSql += vbCrLf + " ,ISS.STNPCS AS ISSPCS, ISS.STNWT AS ISSGRSWT, ISS.STNWT AS ISSNETWT "
                StrSql += vbCrLf + " ,STNPCS "
                StrSql += vbCrLf + " ,TAGSTNPCS "
                StrSql += vbCrLf + " ,STNWT "
                StrSql += vbCrLf + " ,TAGSTNWT "
                StrSql += vbCrLf + " ,TAGSTNPCS AS TAGPCS,TAGSTNWT AS TAGGRSWT,TAGSTNWT AS TAGNETWT FROM " & cnStockDb & "..ISSSTONE ISS "
                StrSql += vbCrLf + " WHERE ISS.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND ISSSNO IN(SELECT SNO FROM  " & cnStockDb & "..ISSUE I  "
                StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
                StrSql += vbCrLf + " AND TRANTYPE = 'AI'   "
                StrSql += vbCrLf + " AND I.TAGNO <> ''"
                StrSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''   "
                StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
                StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
                StrSql += StrFilter
                StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
                StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
                StrSql += vbCrLf + " ) ) X "
                StrSql += vbCrLf + " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
                StrSql += vbCrLf + " ) GROUP BY CATNAME"
                StrSql += vbCrLf + " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
            End If
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPARTLYSALE') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE "
            StrSql += vbCrLf + " SELECT METALID,CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
            StrSql += vbCrLf + " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
            StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE FROM ("
            If chkCatShortname.Checked = True Then
                StrSql += vbCrLf + " SELECT METALID,(SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
            Else
                StrSql += vbCrLf + " SELECT METALID,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            End If
            StrSql += vbCrLf + " WHERE CATCODE = I.CATCODE)AS CATNAME   "
            StrSql += vbCrLf + " ,I.PCS AS ISSPCS, I.GRSWT AS ISSGRSWT, I.NETWT AS ISSNETWT "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(STNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNPCS "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(TAGSTNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNPCS "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(STNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO )STNWT "
            StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(TAGSTNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNWT "
            StrSql += vbCrLf + " ,TAGPCS,TAGGRSWT,TAGNETWT FROM " & cnStockDb & "..ISSUE I "
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
            StrSql += vbCrLf + " AND I.TAGNO <> ''"
            StrSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
            StrSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''   "
            StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
            StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += StrFilter
            StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
            StrSql += vbCrLf + " ) X "
            StrSql += vbCrLf + " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
            StrSql += vbCrLf + " ) GROUP BY CATNAME,METALID"
            StrSql += vbCrLf + " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
            If RPT_SEPVAT_DABS Then
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT METALID,CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
                StrSql += vbCrLf + " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
                StrSql += vbCrLf + " FROM ("
                If chkCatShortname.Checked = True Then
                    StrSql += vbCrLf + " SELECT IM.METALID,(SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
                Else
                    StrSql += vbCrLf + " SELECT IM.METALID,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
                End If
                StrSql += vbCrLf + " WHERE CATCODE = ISS.CATCODE)AS CATNAME   "
                StrSql += vbCrLf + " ,ISS.STNPCS AS ISSPCS, ISS.STNWT AS ISSGRSWT, ISS.STNWT AS ISSNETWT "
                StrSql += vbCrLf + " ,STNPCS "
                StrSql += vbCrLf + " ,TAGSTNPCS "
                StrSql += vbCrLf + " ,STNWT "
                StrSql += vbCrLf + " ,TAGSTNWT "
                StrSql += vbCrLf + " ,TAGSTNPCS AS TAGPCS,TAGSTNWT AS TAGGRSWT,TAGSTNWT AS TAGNETWT FROM " & cnStockDb & "..ISSSTONE ISS "
                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID=ISS.STNITEMID"
                StrSql += vbCrLf + " WHERE ISS.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..ISSUE I "
                StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
                StrSql += vbCrLf + " AND I.TAGNO <> ''"
                StrSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
                StrSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''   "
                StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
                StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
                StrSql += StrFilter
                StrSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
                StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
                StrSql += vbCrLf + " ) ) X "
                StrSql += vbCrLf + " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
                StrSql += vbCrLf + " ) GROUP BY CATNAME,METALID"
                StrSql += vbCrLf + " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
            End If
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        'StrSql += VBCRLF + " WHERE CATCODE = I.CATCODE)AS CATNAME, "
        'StrSql += VBCRLF + " SUM(I.PCS) AS ISSPCS, SUM(I.GRSWT) AS ISSGRSWT, SUM(I.NETWT) AS ISSNETWT,  "
        'StrSql += VBCRLF + " 1 AS RESULT1, ' ' AS COLHEAD "
        'StrSql += VBCRLF + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE FROM "
        'StrSql += cnStockDb & "..ISSUE I, " & cnAdminDb & "..ITEMTAG T "
        'StrSql += VBCRLF + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        'StrSql += VBCRLF + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        'StrSql += VBCRLF + " AND  I.TRANTYPE = 'SA' AND ISNULL(I.CANCEL,'') = ''  "
        'StrSql += VBCRLF + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        'StrSql += VBCRLF + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO AND"
        'StrSql += VBCRLF + " ((T.GRSWT-I.GRSWT)<> 0 OR (T.NETWT-I.NETWT)<>0)"
        'StrFilter = Replace(StrFilter, "SYSTEMID", "I.SYSTEMID")
        'StrSql += StrFilter
        'StrFilter = Replace(StrFilter, "I.SYSTEMID", "SYSTEMID")
        'StrSql += VBCRLF + " GROUP BY CATCODE"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        If rbtmetalwisegrp.Checked = False Then
            StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE)>0 "
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION, COLHEAD) VALUES('PARTLY SALES (APROVAL)','T') "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD) "
            StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS    <> 0 THEN ISSPCS    ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT  <> 0 THEN ISSGRSWT  ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT  <> 0 THEN ISSNETWT  ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE ORDER BY RESULT1 "
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE)>0 "
            StrSql += vbCrLf + " BEGIN "
            'StrSql += VBCRLF + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            'StrSql += VBCRLF + " (DESCRIPTION, COLHEAD) VALUES('PARTLY SALES','T') "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,RESULT1,CATCODE,COLHEAD,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT '  PARTLY SALES (APROVAL)'  CATNAME, "
            StrSql += vbCrLf + " 1 RESULT1,METALID,'L' COLHEAD,9,'C' COLHEAD1  "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE GROUP BY METALID,COLHEAD ORDER BY RESULT1 "

            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,CATCODE,COLHEAD,ORDERNO) "
            StrSql += vbCrLf + " SELECT CATNAME, "
            StrSql += vbCrLf + " CASE WHEN SUM(ISNULL(ISSPCS,0))  <> 0 THEN SUM(ISNULL(ISSPCS,0)) ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN SUM(ISNULL(ISSGRSWT,0))  <> 0 THEN SUM(ISNULL(ISSGRSWT,0))  ELSE NULL END ISSGRSWT,  "
            StrSql += vbCrLf + " CASE WHEN SUM(ISNULL(ISSNETWT,0))  <> 0 THEN SUM(ISNULL(ISSNETWT,0))  ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " 1 RESULT1,METALID,'L' COLHEAD, 10 "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE GROUP BY METALID,COLHEAD,CATNAME ORDER BY RESULT1 "
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE)>0"
            StrSql += vbCrLf + " BEGIN "
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
            StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,AVERAGE,CATCODE,COLHEAD,RESULT1,ORDERNO,COLHEAD1) "
            StrSql += vbCrLf + " SELECT 'TOTAL' DESCRIPTION,SUM(ISNULL(ISSPCS,0)),SUM(ISNULL(ISSGRSWT,0)),SUM(ISNULL(ISSNETWT,0)),"
            StrSql += vbCrLf + " '' AVERAGE,METALID,'L' COLHEAD,1,11,'M' COLHEAD1 "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSPARTLYSALE GROUP BY METALID,COLHEAD,RESULT1"
            StrSql += vbCrLf + " ORDER BY RESULT1"
            StrSql += vbCrLf + " END "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub
    Private Sub ProcCess()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ACCTRANCHESS') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ACCTRANCHESS "
        StrSql += vbCrLf + " SELECT * INTO TEMPTABLEDB..TEMP" & systemId & "ACCTRANCHESS "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ") AND PAYMODE IN ('SV')"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "COLDETCESS') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "COLDETCESS "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " SELECT PAYMODE,FLAG,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END)) AS AMOUNT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "COLDETCESS "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCTRANCHESS A"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
        StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND PAYMODE IN ('SV')  AND ACCODE IN(SELECT TOP 1 ACCODE FROM " & cnAdminDb & ".. ACHEAD WHERE ACNAME LIKE ('%KERALA FLOOD CESS%'))"
        StrSql += vbCrLf + " GROUP BY TRANMODE,PAYMODE,FLAG"
        StrSql += vbCrLf + " END"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "COLDETCESS)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION,COLHEAD) "
        StrSql += vbCrLf + " VALUES('KERALA FLOOD CESS DETAILS','T') "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT) "
        StrSql += vbCrLf + " SELECT ' KERALA FLOOD CESS '  CATNAME, "
        StrSql += vbCrLf + " CASE WHEN RECEIPT<> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT<> 0 THEN PAYMENT  ELSE NULL END PAYMENT "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "COLDETCESS ORDER BY CATNAME "
        StrSql += vbCrLf + " END"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcCollection()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ACCTRAN') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ACCTRAN "
        StrSql += vbCrLf + " SELECT * INTO TEMPTABLEDB..TEMP" & systemId & "ACCTRAN "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        If hasChit Then
            StrSql = "UPDATE A SET PAYMODE='ET' FROM TEMPTABLEDB..TEMP" & systemId & "ACCTRAN A WHERE EXISTS (SELECT 1 FROM "
            StrSql += vbCrLf + " " & cnChitTrandb & "..SCHEMECOLLECT WHERE "
            StrSql += vbCrLf + " RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND MODEPAY='E' AND ENTREFNO=A.ENTREFNO AND ACCODE=A.ACCODE"
            StrSql += vbCrLf + " AND AMOUNT=A.AMOUNT " ''FOR BALAGOPALAN ETRANFER AND CARD UPDATED AS ETRANSFER SO CHANGED on[05-JULY-2021]
            StrSql += vbCrLf + " )AND TRANMODE='D'"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "COLDET') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "COLDET "
        StrSql += vbCrLf + " DECLARE @CASHID VARCHAR(7)"
        StrSql += vbCrLf + " SELECT @CASHID = CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'"
        StrSql += vbCrLf + " SELECT CONVERT(VARCHAR(50),(CASE "
        StrSql += vbCrLf + " WHEN PAYMODE='CA' THEN 'CASH [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]'"
        StrSql += vbCrLf + " WHEN PAYMODE = 'CC' THEN 'CREDIT CARD [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        StrSql += vbCrLf + " WHEN PAYMODE = 'ONL' THEN 'ONLINE [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        ''StrSql += vbCrLf + " WHEN PAYMODE = 'CH' AND ISNULL(FLAG,'C') = 'C' THEN 'CHEQUE [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        ''StrSql += vbCrLf + " WHEN PAYMODE = 'CH' AND ISNULL(FLAG,'C') = 'N' THEN 'NEFT [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        ''StrSql += vbCrLf + " WHEN PAYMODE = 'CH' AND ISNULL(FLAG,'C') = 'I' THEN 'IMPS [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        ''StrSql += vbCrLf + " WHEN PAYMODE = 'CH' AND ISNULL(FLAG,'C') = 'R' THEN 'RTGS [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        ''StrSql += vbCrLf + " WHEN PAYMODE = 'CH' AND ISNULL(FLAG,'C') = 'F' THEN 'FUND TRANSFER [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        StrSql += vbCrLf + " WHEN PAYMODE = 'CH' THEN FLAG + ' [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        StrSql += vbCrLf + " WHEN PAYMODE = 'SS' THEN 'SCHEME [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        StrSql += vbCrLf + " WHEN PAYMODE = 'GV' THEN 'GIFT VOUCHER [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        StrSql += vbCrLf + " WHEN PAYMODE = 'ET' THEN 'ETRANSFER [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        StrSql += vbCrLf + " WHEN PAYMODE = 'OT' THEN 'OTHERS [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        StrSql += vbCrLf + " WHEN PAYMODE = 'HO' THEN 'HOME COLLECTION [(R) '+CONVERT(VARCHAR,SUM(CASE WHEN RECEIPT> 0 THEN RECEIPT ELSE 0 END))+'-'+CONVERT(VARCHAR,SUM(PAYMENT))+' (P)]' "
        StrSql += vbCrLf + " END)) AS CATNAME,"
        If RPT_REC_PAY = True Then
            StrSql += vbCrLf + " (CASE WHEN (PAYMODE='CH' OR PAYMODE='CC' OR PAYMODE = 'SS' OR PAYMODE = 'GV' OR PAYMODE = 'ET' OR PAYMODE = 'OT' OR PAYMODE = 'HO') AND SUM(PAYMENT)> 0 THEN  SUM(PAYMENT) ELSE 0 END) AS PAYMENT, "
            StrSql += vbCrLf + " (CASE WHEN (PAYMODE='CH' OR PAYMODE='CC' OR PAYMODE = 'SS' OR PAYMODE = 'GV' OR PAYMODE = 'ET' OR PAYMODE = 'OT' OR PAYMODE = 'HO') AND SUM(RECEIPT)> 0 THEN  SUM(RECEIPT) ELSE 0 END) AS RECEIPT, "
            StrSql += vbCrLf + " (CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN RECEIPT > 0 THEN RECEIPT ELSE 0 END) - SUM(PAYMENT) ELSE 0 END) AS [TTT]"
        Else
            StrSql += vbCrLf + " (CASE WHEN SUM(AMOUNT)> 0 THEN  SUM(AMOUNT) ELSE 0 END) AS PAYMENT, "
            StrSql += vbCrLf + " (CASE WHEN SUM(AMOUNT)< 0 THEN  ABS(SUM(AMOUNT)) ELSE 0 END) AS RECEIPT "
        End If
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "COLDET FROM ("
        StrSql += vbCrLf + " SELECT PAYMODE,'' FLAG,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END)) AS AMOUNT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCTRAN A"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
        StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not hasChit Then StrSql += vbCrLf + " AND FROMFLAG <> 'C'"
        'StrSql += VBCRLF + "  AND GRSWT = 0"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND PAYMODE IN ('CC')"
        If OnlineAccode.ToString <> "" Then
            StrSql += vbCrLf + " AND ACCODE NOT IN ('" & OnlineAccode.Replace(",", "','") & "')"
        End If
        StrSql += vbCrLf + " GROUP BY TRANMODE,PAYMODE,FLAG"
        StrSql += vbCrLf + " UNION ALL "
        If OnlineAccode.ToString <> "" Then
            StrSql += vbCrLf + " SELECT 'ONL' PAYMODE,'' FLAG,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END)) AS AMOUNT"
            StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
            StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCTRAN A"
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
            StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
            If Not hasChit Then StrSql += vbCrLf + " AND FROMFLAG <> 'C'"
            StrSql += StrFilter
            StrSql += StrUseridFtr
            StrSql += vbCrLf + " AND PAYMODE IN ('CC')"
            StrSql += vbCrLf + " AND ACCODE IN ('" & OnlineAccode.Replace(",", "','") & "')"
            StrSql += vbCrLf + " GROUP BY TRANMODE,PAYMODE,FLAG"
            StrSql += vbCrLf + " UNION ALL "
        End If

        StrSql += vbCrLf + " SELECT PAYMODE" ',FLAG
        StrSql += vbCrLf + "  ,(CASE WHEN ISNULL((SELECT MODENAME FROM " & cnAdminDb & "..PAYMENTMODE WHERE CONVERT(VARCHAR,MODEID)=A.FLAG),'') <>''  "
        StrSql += vbCrLf + "  THEN (SELECT MODENAME FROM " & cnAdminDb & "..PAYMENTMODE WHERE CONVERT(VARCHAR,MODEID)=A.FLAG)"
        StrSql += vbCrLf + "  ELSE 'CHEQUE' END) FLAG"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END)) AS AMOUNT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCTRAN A"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
        StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not hasChit Then StrSql += vbCrLf + " AND FROMFLAG <> 'C'"
        'StrSql += VBCRLF + "  AND GRSWT = 0"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND PAYMODE IN ('CH','ET')"
        StrSql += vbCrLf + " GROUP BY TRANMODE,PAYMODE,FLAG"


        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT PAYMODE,FLAG,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END)) AS AMOUNT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCTRAN A"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
        StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not hasChit Then StrSql += vbCrLf + " AND FROMFLAG <> 'C'"
        'StrSql += VBCRLF + "  AND GRSWT = 0"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND PAYMODE IN ('GV') AND TRANMODE = 'D'"
        StrSql += vbCrLf + " GROUP BY TRANMODE,PAYMODE,FLAG"
        StrSql += vbCrLf + " UNION ALL "
        '602
        If chkwithcreditcommision.Checked Then
            StrSql += vbCrLf + " SELECT 'CC' PAYMODE,''FLAG,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END)) AS AMOUNT"
            StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
            StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCTRAN A"
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
            StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
            If Not hasChit Then StrSql += vbCrLf + " AND FROMFLAG <> 'C'"
            StrSql += StrFilter
            StrSql += StrUseridFtr
            StrSql += vbCrLf + " AND PAYMODE IN ('BC') AND ACCODE='BANKC' AND CONTRA IN(SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD)"
            StrSql += vbCrLf + " GROUP BY TRANMODE,PAYMODE"
            StrSql += vbCrLf + " UNION ALL "
        End If
        '602
        StrSql += vbCrLf + " SELECT 'SS' PAYMODE,''FLAG,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END)) AS AMOUNT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
        StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCTRAN A"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " AND PAYMODE IN ('SS','CB','CZ','CG','CD')"
        StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
        If Not hasChit Then StrSql += vbCrLf + " AND FROMFLAG <> 'C'"
        StrSql += vbCrLf + " GROUP BY TRANMODE"
        StrSql += vbCrLf + " UNION ALL "
        If chkRecPay.Checked Then
            StrSql += vbCrLf + " SELECT 'CA' PAYMODE,''FLAG,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END)) AS AMOUNT"
            StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
            StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCTRAN A"
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
            StrSql += StrFilter
            StrSql += StrUseridFtr
            StrSql += vbCrLf + " AND PAYMODE IN ('CA','CR','CP') /*AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE = '1')*/"
            StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O')" ','A'
            If Not hasChit Then StrSql += vbCrLf + " AND FROMFLAG <> 'C'"
            If chkCashOpening.Checked Then
                StrSql += vbCrLf + " AND ACCODE = @CASHID"
            End If
            StrSql += vbCrLf + " GROUP BY TRANMODE"
        Else
            StrSql += vbCrLf + " SELECT 'CA' PAYMODE,''FLAG,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END)) AS AMOUNT"
            StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
            StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCTRAN A"
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
            StrSql += StrFilter
            StrSql += StrUseridFtr
            StrSql += vbCrLf + " AND PAYMODE IN ('CA') /*AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE = '1')*/"
            StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A')"
            If Not hasChit Then StrSql += vbCrLf + " AND FROMFLAG <> 'C'"
            If chkCashOpening.Checked Then
                StrSql += vbCrLf + " AND ACCODE = @CASHID"
            End If
            StrSql += vbCrLf + " GROUP BY TRANMODE"
        End If
        If chkCashOpening.Checked Then
            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT 'CA' PAYMODE,'' FLAG,-1*SUM(AMOUNT) AMOUNT,-1*SUM(AMOUNT) RECEIPT,0 PAYMENT FROM TEMPTABLEDB..TEMPCASHOPEN"
        End If

        If RPT_GSTREGISTER_CASH_VIEW Then
            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT 'CA' PAYMODE,''FLAG,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END)) AS AMOUNT"
            StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE 0 END)) AS RECEIPT"
            StrSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE 0 END)) AS PAYMENT"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ACCTRAN A"
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = ''"
            StrSql += vbCrLf + " AND EXISTS(SELECT 1 FROM " & cnStockDb & "..GSTREGISTER WHERE "
            StrSql += vbCrLf + " TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO=A.BATCHNO AND ISNULL(CANCEL,'') = '' "
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") "
            StrSql += vbCrLf + " ) "
            StrSql += StrFilter
            StrSql += StrUseridFtr
            StrSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O')" ','A'
            If Not hasChit Then StrSql += vbCrLf + " AND FROMFLAG <> 'C'"
            StrSql += vbCrLf + " AND ACCODE = @CASHID"
            StrSql += vbCrLf + " GROUP BY TRANMODE"
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
        If HOMECOLLECTINBASE = True Then
            If StrCostFiltration1.Contains("INTERNET") Or StrCostFiltration1 = "" Then
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT 'HO' PAYMODE,''FLAG,-1 * SUM(RECEIPT)AMOUNT,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT  FROM"
                StrSql += vbCrLf + " ("
                StrSql += vbCrLf + " SELECT  'HOME COLLECTION' CATNAME,"
                StrSql += vbCrLf + " SUM(AMOUNT) AS RECEIPT, 0  AS PAYMENT,MODEPAY "
                StrSql += vbCrLf + " FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
                StrSql += vbCrLf + " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                StrSql += vbCrLf + " AND MODEPAY IN ('R','C') "
                StrSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y'"
                'If SCashCounterName <> "" Then StrSql += vbCrLf + " AND CASHCOUNTERID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & SCashCounterName & "))"
                StrSql += vbCrLf + "  AND COSTID IN ('IN') "
                If StrCostFiltration1 <> "" Then StrSql += vbCrLf + "  AND ISNULL(SUBSTRING(GROUPCODE,1,2),'') IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & StrCostFiltration1.Replace("'INTERNET',", "") & "))"
                StrSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                StrSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))AND T.GROUPCODE = SC.GROUPCODE AND T.REGNO = SC.REGNO)"
                StrSql += vbCrLf + "  GROUP BY MODEPAY"
                StrSql += vbCrLf + " )X"
                StrSql += vbCrLf + " group by X.CATNAME,MODEPAY "
            End If
        End If
        StrSql += vbCrLf + " )X "
        StrSql += vbCrLf + " GROUP BY PAYMODE,FLAG "
        StrSql += vbCrLf + " HAVING(SUM(AMOUNT) <> 0)"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "COLDET"
        StrSql += vbCrLf + " (CATNAME,RECEIPT,PAYMENT) "
        StrSql += vbCrLf + $" select 'BR TRF ADVANCE' DESCRIPTION,sum(Amount)Receipt,null Payment from {cnAdminDb}.dbo.OUTSTANDING where 1=1 and ISNULL(COSTID,'') <> ''"
        StrSql += vbCrLf + $" and TRANDATE between '{dtpFrom.Value}' and '{dtpTo.Value}' and RECPAY = 'R' and COSTID <> substring(RUNNO,0,3)"
        StrSql += vbCrLf + $" and isnull(CANCEL,'') = ''"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If RPT_REC_PAY = True Then
            StrSql = " UPDATE  TEMPTABLEDB..TEMP" & systemId & "COLDET SET RECEIPT = TTT where TTT > 0 "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
            StrSql = "ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "COLDET DROP COLUMN TTT"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "COLDET)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION,COLHEAD) "
        StrSql += vbCrLf + " VALUES('COLLECTION DETAILS','T') "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT) "
        StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
        StrSql += vbCrLf + " CASE WHEN RECEIPT<> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT<> 0 THEN PAYMENT  ELSE NULL END PAYMENT "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "COLDET WHERE (RECEIPT <> 0 OR PAYMENT <> 0) ORDER BY CATNAME "
        StrSql += vbCrLf + " END"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcTagEdited()
        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "TAGEDIT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "TAGEDIT"
        StrSql += vbCrLf + " SELECT * INTO  TEMPTABLEDB..TEMP" & systemId & "TAGEDIT "
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..EITEMTAG A"
        StrSql += vbCrLf + " WHERE RECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompanyId & ")"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "TAGEDITCOL') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "TAGEDITCOL "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " SELECT  DISTINCT ''CATNAME,I.TAGNO AS RECEIPT,NULL AS PAYMENT ,'' COLHEAD INTO TEMPTABLEDB..TEMP" & systemId & "TAGEDITCOL FROM   TEMPTABLEDB..TEMP" & systemId & "TAGEDIT I"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "TAGEDITCOL)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU(DESCRIPTION,COLHEAD) "
        StrSql += vbCrLf + " VALUES('TAG EDITED DETAILS','T') "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT) "
        StrSql += vbCrLf + " SELECT RECEIPT, NULL CATNAME, "
        StrSql += vbCrLf + "CASE WHEN PAYMENT<= 0 THEN ABS(PAYMENT)  ELSE NULL END PAYMENT "
        StrSql += vbCrLf + " FROM  TEMPTABLEDB..TEMP" & systemId & "TAGEDITCOL"
        StrSql += vbCrLf + " END"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcGstRegister()

        Dim _cashaccode As String = ""
        StrSql = vbCrLf + " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'"
        _cashaccode = objGPack.GetSqlValue(StrSql, "CTLTEXT", "CASH")

        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "GSTREGISTER') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "GSTREGISTER "
        StrSql += vbCrLf + " SELECT (SELECT (ACNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = "
        StrSql += vbCrLf + " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..GSTREGISTER WHERE BATCHNO = O.BATCHNO AND SNO = O.SNO))+ "
        StrSql += vbCrLf + " '(INV NO: '  + CONVERT(VARCHAR(12),O.TRANNO) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += vbCrLf + " SUM(ISNULL(AMOUNT,0)) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "GSTREGISTER FROM " & cnStockDb & "..GSTREGISTER AS O "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") "
        StrSql += vbCrLf + " AND ISNULL(REFNO,0)<>0 AND ISNULL(GSTCLAIM,'')<>''"
        If _cashaccode <> "" And RPT_GSTREGISTER_CASH_VIEW Then
            StrSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..ACCTRAN WHERE "
            StrSql += vbCrLf + " TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO=O.BATCHNO AND ISNULL(CANCEL,'') = '' "
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") "
            StrSql += vbCrLf + " AND ACCODE = '" & _cashaccode & "') "
        End If
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.TRANNO,O.TRANDATE,O.SNO "
        StrSql += vbCrLf + " ORDER BY TRANDATE,TRANNO "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "GSTREGISTER)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
        StrSql += vbCrLf + " SELECT 'GST REGISTER ','T'"

        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
        StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "GSTREGISTER "

        StrSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + "    (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + "    SELECT 'GST REGISTER TOTAL' CATNAME,  "
        StrSql += vbCrLf + "    SUM(RECEIPT) RECEIPT,  "
        StrSql += vbCrLf + "    NULL PAYMENT, "
        StrSql += vbCrLf + "    1 RESULT1,'S' COLHEAD  "
        StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMP" & systemId & "GSTREGISTER "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcGstRegister_Cash()

        Dim _cashaccode As String = ""
        StrSql = vbCrLf + " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'"
        _cashaccode = objGPack.GetSqlValue(StrSql, "CTLTEXT", "CASH")

        StrSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "GSTREGISTER_CASH') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "GSTREGISTER_CASH "
        StrSql += vbCrLf + " SELECT (SELECT (ACNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = "
        StrSql += vbCrLf + " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..GSTREGISTER WHERE BATCHNO = O.BATCHNO AND SNO = O.SNO))+ "
        StrSql += vbCrLf + " '(INV NO: '  + CONVERT(VARCHAR(12),O.TRANNO) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += vbCrLf + " SUM(ISNULL(AMOUNT,0)) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "GSTREGISTER_CASH FROM " & cnStockDb & "..GSTREGISTER AS O "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") "
        StrSql += vbCrLf + " AND ISNULL(REFNO,0)<>0 AND ISNULL(GSTCLAIM,'')<>''"
        If _cashaccode <> "" Then
            StrSql += vbCrLf + " AND EXISTS(SELECT 1 FROM " & cnStockDb & "..ACCTRAN WHERE "
            StrSql += vbCrLf + " TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
            StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO=O.BATCHNO AND ISNULL(CANCEL,'') = '' "
            StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") "
            StrSql += vbCrLf + " AND ACCODE = '" & _cashaccode & "') "
        End If
        StrSql += vbCrLf + " GROUP BY O.BATCHNO,O.TRANNO,O.TRANDATE,O.SNO "
        StrSql += vbCrLf + " ORDER BY TRANDATE,TRANNO "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "GSTREGISTER_CASH)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION, COLHEAD)"
        StrSql += vbCrLf + " SELECT 'GST REGISTER [CASH]','T'"

        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
        StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " 1 RESULT1, COLHEAD  "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "GSTREGISTER_CASH "

        StrSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + "    (DESCRIPTION,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += vbCrLf + "    SELECT 'GST REGISTER [CASH] TOTAL' CATNAME,  "
        StrSql += vbCrLf + "    SUM(RECEIPT) RECEIPT,  "
        StrSql += vbCrLf + "    NULL PAYMENT, "
        StrSql += vbCrLf + "    1 RESULT1,'S' COLHEAD  "
        StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMP" & systemId & "GSTREGISTER_CASH "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcAmtSubtot()
        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "COLDET)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCOLDETTOT') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSCOLDETTOT "
        StrSql += vbCrLf + " SELECT   SUM(RECEIPT) AS RECEIPT, SUM(PAYMENT) AS PAYMENT INTO TEMPTABLEDB..TEMP" & systemId & "ABSCOLDETTOT"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "COLDET END"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        DT = New DataTable
        StrSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "COLDET"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(DT)
        If DT.Rows.Count <= 0 Then Exit Sub

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSCOLDETTOT)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,COLHEAD) "
        StrSql += vbCrLf + " SELECT  'TOTAL', "
        StrSql += vbCrLf + " CASE WHEN RECEIPT<> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN PAYMENT<> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += vbCrLf + " 'S' FROM TEMPTABLEDB..TEMP" & systemId & "ABSCOLDETTOT "
        StrSql += vbCrLf + " END"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSCOLDETTOT)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,COLHEAD) "
        StrSql += vbCrLf + " SELECT 'GRAND TOTAL', ISNULL((CASE WHEN X>0 THEN X END),NULL) AS RECEIPT,"
        StrSql += vbCrLf + " ISNULL((CASE WHEN X<0 THEN X END),NULL) AS PAYMENT, 'G'"
        StrSql += vbCrLf + " FROM ("
        StrSql += vbCrLf + " SELECT RECEIPT-PAYMENT  AS X FROM TEMPTABLEDB..TEMP" & systemId & "ABSCOLDETTOT) Y"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcCancelBills()
        StrSql = " IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCANCELBILL') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSCANCELBILL "
        StrSql += vbCrLf + " SELECT  TRANNO, TRANTYPE, (CASE "
        StrSql += vbCrLf + " WHEN TRANTYPE = 'SA' THEN 'SAL BILL-'  "
        StrSql += vbCrLf + " WHEN TRANTYPE = 'PU' THEN 'PUR BILL-'  "
        StrSql += vbCrLf + " WHEN TRANTYPE = 'SR' THEN 'SAL RET BILL-'  "
        StrSql += vbCrLf + " WHEN TRANTYPE = 'AD' THEN 'ORD ADV BILL-'  "
        StrSql += vbCrLf + " WHEN TRANTYPE = 'OD' THEN 'ORD DEL BILL-'  "
        StrSql += vbCrLf + " WHEN TRANTYPE = 'RD' THEN 'REP DEL BILL-'  "
        StrSql += vbCrLf + " WHEN TRANTYPE = 'AI' THEN 'APP ISS BILL-'  END ) "
        'StrSql += VBCRLF + " + CONVERT(VARCHAR(5),TRANNO) + ' - ' + CONVERT(VARCHAR(11),TRANDATE,103) AS TRANS, "
        StrSql += vbCrLf + " + CONVERT(VARCHAR(5),TRANNO) AS TRANS, "
        StrSql += vbCrLf + " IPCS, IGRSWT, INETWT, RPCS, RGRSWT, RNETWT, RECEIPT, PAYMENT"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSCANCELBILL FROM (  "
        StrSql += vbCrLf + " SELECT TRANNO, TRANDATE, TRANTYPE, SUM(PCS) IPCS, SUM(GRSWT) IGRSWT,  "
        StrSql += vbCrLf + " SUM(NETWT) INETWT, 0 RPCS,  0 RGRSWT, 0 RNETWT, SUM(AMOUNT) RECEIPT, 0 PAYMENT  "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE  "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND ISNULL(CANCEL,'') = 'Y'  AND COMPANYID IN (" & SelectedCompanyId & ") "
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " GROUP BY TRANNO, TRANDATE, TRANTYPE"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT TRANNO, TRANDATE, TRANTYPE, 0 IPCS, 0 IGRSWT, 0 INETWT,  "
        StrSql += vbCrLf + " SUM(PCS) RPCS, SUM(GRSWT) RGRSWT, SUM(NETWT) RNETWT, "
        StrSql += vbCrLf + " 0 RECEIPT, SUM(AMOUNT) PAYMENT FROM " & cnStockDb & "..RECEIPT  "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND ISNULL(CANCEL,'') = 'Y'   AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += StrFilter
        StrSql += StrUseridFtr
        StrSql += vbCrLf + " GROUP BY TRANNO, TRANDATE, TRANTYPE"
        StrSql += vbCrLf + " ) X "
        StrSql += vbCrLf + " WHERE TRANTYPE IN ('SA','SR','PU','AD','OD','RD','AI')"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

    End Sub
    Private Sub ProcMaterialIssue()
        If ChkMatIssue.Checked = False Then Exit Sub
        StrSql = vbCrLf + "IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSMATERIALISSUE') DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ABSMATERIALISSUE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " SELECT CATCODE,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE =I.CATCODE)CATNAME,"
        StrSql += vbCrLf + " SUM(PCS)ISSPCS,SUM(GRSWT)ISSGRSWT,SUM(NETWT)ISSNETWT,0 AS RECPCS,0 AS RECGRSWT,"
        StrSql += vbCrLf + " 0 AS RECNETWT,0 AS RECEIPT,0 AS PAYMENT,0 AS AVERAGE,RATE,''AS COLHEAD,1 AS RESULT,1 AS RESULT1"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ABSMATERIALISSUE "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + "  '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += vbCrLf + StrFilter
        StrSql += vbCrLf + StrUseridFtr
        StrSql += vbCrLf + "GROUP BY CATCODE,RATE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSMATERIALISSUE)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSMATERIALISSUE(CATNAME, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + " AVERAGE,RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'MATERIAL ISSUE'CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += vbCrLf + " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, 0 AVERAGE,0 RESULT, 0 AS RESULT1, 'T' COLHEAD "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSMATERIALISSUE)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ABSMATERIALISSUE(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,AVERAGE,RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'Z' AS CATCODE, 'MATERIAL ISSUE TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + " SUM(PAYMENT),SUM(AVERAGE),4 RESULT, 2 AS RESULT1, 'S' COLHEAD "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSMATERIALISSUE WHERE RESULT = 1 "
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ABSMATERIALISSUE)>0"
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
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
        StrSql += vbCrLf + " CASE WHEN AVERAGE  <> 0 THEN AVERAGE  ELSE NULL END AVERAGE, "
        StrSql += vbCrLf + " COLHEAD, RESULT1  "
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ABSMATERIALISSUE "
        StrSql += vbCrLf + " ORDER BY CATCODE, RESULT"
        StrSql += vbCrLf + " END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

    End Sub

    Private Sub ProcPrivilegePoints()
        StrSql = ""
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RESULT1,COLHEAD,COLHEAD1) "
        StrSql += vbCrLf + " SELECT 'PRIVILEGE POINTS'  DESCRIPTION"
        StrSql += vbCrLf + " ,1 RESULT1,'T' COLHEAD,'C' COLHEAD1  "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = ""
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SASRPU"
        StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT) "
        StrSql += vbCrLf + " SELECT "
        StrSql += vbCrLf + " CASE WHEN ENTRYTYPE='O' THEN 'OPENING' "
        StrSql += vbCrLf + " ELSE CASE WHEN ENTRYTYPE='T' THEN 'TRANSACTION' "
        StrSql += vbCrLf + " ELSE CASE WHEN ENTRYTYPE='C' THEN 'CLOSING' "
        StrSql += vbCrLf + " END END END DESCRIPTION"
        StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0))  <> 0 THEN SUM(ISNULL(RECEIPT,0)) ELSE NULL END RECEIPT "
        StrSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(PAYMENT,0))  <> 0 THEN SUM(ISNULL(PAYMENT,0)) ELSE NULL END PAYMENT "
        StrSql += vbCrLf + " FROM ( "
        StrSql += vbCrLf + " SELECT SUM(OPENING) RECEIPT, 0 PAYMENT,'O'ENTRYTYPE,1 RESULT FROM( "
        StrSql += vbCrLf + " SELECT CASE WHEN TRANTYPE='R' THEN SUM(POINTS) ELSE -1*SUM(POINTS) END OPENING "
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILEGETRAN WHERE TRANDATE <'" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND ISNULL(PREVILEGEID,'') <> '' "
        StrSql += vbCrLf + " AND ISNULL(CANCEL,'')='' "
        StrSql += vbCrLf + " GROUP BY TRANTYPE "
        StrSql += vbCrLf + " )X "
        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT SUM(POINTS)RECEIPT,0 PAYMENT,'T'ENTRYTYPE,2 RESULT  FROM " & cnAdminDb & "..PRIVILEGETRAN "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  "
        StrSql += vbCrLf + " AND ISNULL(CANCEL,'')='' AND TRANTYPE='R' "
        StrSql += vbCrLf + " AND ISNULL(PREVILEGEID,'') <> '' "
        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT  0 RECEIPT,SUM(POINTS) PAYMENT,'T'ENTRYTYPE ,2 RESULT  FROM " & cnAdminDb & "..PRIVILEGETRAN "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  "
        StrSql += vbCrLf + " AND ISNULL(CANCEL,'')='' AND TRANTYPE='I' "
        StrSql += vbCrLf + " AND ISNULL(PREVILEGEID,'') <> '' "
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT SUM(CLOSING) RECEIPT, 0 PAYMENT,'C'ENTRYTYPE,4 RESULT FROM( "
        StrSql += vbCrLf + " SELECT CASE WHEN TRANTYPE='R' THEN SUM(POINTS) ELSE -1*SUM(POINTS) END CLOSING "
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILEGETRAN WHERE  TRANDATE <='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND ISNULL(PREVILEGEID,'') <> '' "
        StrSql += vbCrLf + " AND ISNULL(CANCEL,'')='' "
        StrSql += vbCrLf + " GROUP BY TRANTYPE "
        StrSql += vbCrLf + " )X "
        StrSql += vbCrLf + " ) Y GROUP BY ENTRYTYPE,RESULT ORDER BY RESULT"

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
                    Case "P"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "X"
                        StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='DN_DIALYABS_COLOR'"
                        Cmd = New OleDbCommand(StrSql, cn)
                        Dim val As String = Cmd.ExecuteScalar()
                        If val = "Y" Then
                            For Each _cell As DataGridViewCell In .Cells
                                _cell.Style.BackColor = Color.Green
                                _cell.Style.ForeColor = Color.White
                            Next
                        End If
                End Select
                Select Case .Cells("COLHEAD1").Value.ToString
                    Case "M"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "C"
                        .Cells("COL1").Style.BackColor = Color.LightYellow
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
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
        If GetAdmindbSoftValue("RPTWITH_CASH", "") = "M" Then
            chkCashOpening.Checked = True
            chkCashOpening.Enabled = False
        Else
            chkCashOpening.Checked = _CashOpening
            chkCashOpening.Enabled = True
        End If
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


    Private Sub Prop_Gets()
        Dim obj As New frmDailyAbstract_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmDailyAbstract_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        'SetChecked_CheckedList(chklstCostCentre, obj.p_chklstCostCentre, "ALL")
        If RPT_SELFCTR_DABS = False Then SetChecked_CheckedList(chkLstCashCounter, obj.p_chkLstCashCounter, "ALL")
        If RPT_SELFCTR_DABS = False Then SetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId, "ALL")
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
        chkstockdetail.Checked = obj.p_chkstockdetail
        chkwithcreditcommision.Checked = obj.p_chkWithCreditcardComm
        Chk_FinDiscount.Checked = obj.p_chkFinDiscount
        ChkMatIssue.Checked = obj.p_ChkMatIssue
        chkAdvdueSummary.Checked = obj.p_chkAdvdueSummary
        chkWithHeaderTotal.Checked = obj.p_chkWithHeaderTotal
        chkPrivilegePoints.Checked = obj.P_chkPrivilegePoints
        chkRecPay.Checked = obj.P_chkRecPay
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmDailyAbstract_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        'GetChecked_CheckedList(chklstCostCentre, obj.p_chklstCostCentre)
        If RPT_SELFCTR_DABS = False Then GetChecked_CheckedList(chkLstCashCounter, obj.p_chkLstCashCounter)
        If RPT_SELFCTR_DABS = False Then GetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId)
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
        obj.p_chkstockdetail = chkstockdetail.Checked
        obj.p_chkWithCreditcardComm = chkwithcreditcommision.Checked
        obj.p_chkFinDiscount = Chk_FinDiscount.Checked
        obj.p_ChkMatIssue = ChkMatIssue.Checked
        obj.p_chkAdvdueSummary = chkAdvdueSummary.Checked
        obj.p_chkWithHeaderTotal = chkWithHeaderTotal.Checked
        obj.P_chkPrivilegePoints = chkPrivilegePoints.Checked
        obj.P_chkRecPay = chkRecPay.Checked
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
    Public Function SendMailRpt(Optional ByVal Trptdate As Date = Nothing)
        funcNew()
        ProcAddCostCentre()
        If Not Trptdate = Nothing Then
            dtpFrom.Value = Trptdate
            dtpTo.Value = Trptdate
        End If
        btnView_Search_Click(Me, New EventArgs)
        Dim strHtml As String = ""
        If gridView.Rows.Count > 0 Then
            strHtml = ConvertDataGridViewToHTML(gridView, GridViewHead)
            Return strHtml
        End If
    End Function


End Class

Public Class frmDailyAbstract_Properties

    Private chkAdvdueSummary As Boolean = False
    Public Property p_chkAdvdueSummary() As Boolean
        Get
            Return chkAdvdueSummary
        End Get
        Set(ByVal value As Boolean)
            chkAdvdueSummary = value
        End Set
    End Property
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
    Private chkWithCreditcardComm As Boolean = False
    Public Property p_chkWithCreditcardComm() As Boolean
        Get
            Return chkWithCreditcardComm
        End Get
        Set(ByVal value As Boolean)
            chkWithCreditcardComm = value
        End Set
    End Property
    Private chkstockdetail As Boolean = False
    Public Property p_chkstockdetail() As Boolean
        Get
            Return chkstockdetail
        End Get
        Set(ByVal value As Boolean)
            chkstockdetail = value
        End Set
    End Property
    Private chkFinDiscount As Boolean = False
    Public Property p_chkFinDiscount() As Boolean
        Get
            Return chkFinDiscount
        End Get
        Set(ByVal value As Boolean)
            chkFinDiscount = value
        End Set
    End Property
    Private ChkMatIssue As Boolean = False
    Public Property p_ChkMatIssue() As Boolean
        Get
            Return ChkMatIssue
        End Get
        Set(ByVal value As Boolean)
            ChkMatIssue = value
        End Set
    End Property
    Private chkWithHeaderTotal As Boolean = False
    Public Property p_chkWithHeaderTotal() As Boolean
        Get
            Return chkWithHeaderTotal
        End Get
        Set(ByVal value As Boolean)
            chkWithHeaderTotal = value
        End Set
    End Property
    Private chkPrivilegePoints As Boolean = False
    Public Property P_chkPrivilegePoints() As Boolean
        Get
            Return chkPrivilegePoints
        End Get
        Set(ByVal value As Boolean)
            chkPrivilegePoints = value
        End Set
    End Property

    Private chkRecPay As Boolean = False
    Public Property P_chkRecPay() As Boolean
        Get
            Return chkRecPay
        End Get
        Set(ByVal value As Boolean)
            chkRecPay = value
        End Set
    End Property

End Class

