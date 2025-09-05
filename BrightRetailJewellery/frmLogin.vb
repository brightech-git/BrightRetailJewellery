Imports System.IO
Imports System.Data.OleDb
Imports System.Security.Cryptography
Public Class frmLogin
    'Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Control Panel\International", "sShortDate", "dd/MM/yyyy")
    Dim dbPath As String
    Dim passWord As String
    Dim strLen As Integer
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim tran As OleDbTransaction = Nothing
    Dim cmd As OleDbCommand
    Dim ent As Integer = 0
    Dim _ChangePwd As Boolean
    Dim LOCKPREVIOUSYEARS() As String
    Dim LOCKPREVIOUSYEAR As Boolean = False
    Dim ACCESSUSERIDS() As String = {"999"}

    Private Sub HideMenus(ByVal tStrip As ToolStripMenuItem)
        'Dim objKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.
        'Dim objSubKey As Microsoft.Win32.RegistryKey = objKey.OpenSubKey("Excel.Application")
        'If objSubKey Is Nothing Then
        '    MsgBox("Please Install Miscrosoft Office Excel", MsgBoxStyle.Information)
        '    Exit Sub
        'End If

        For Each mnu As ToolStripMenuItem In tStrip.DropDownItems
            mnu.Visible = False
            HideMenus(mnu)
        Next
    End Sub
    Private Sub HideMenus()
        For Each mnu As ToolStripMenuItem In Main.MenuStrip1.Items
            mnu.Visible = False
            HideMenus(mnu)
        Next
    End Sub

    Private Sub VisibleUserRightsForms(ByVal tStrip As ToolStripMenuItem)
        For Each mnu As ToolStripMenuItem In tStrip.DropDownItems
            Dim ro() As DataRow = _DtUserRights.Select("MENUID = '" & mnu.Name & "'", "MENUID")
            If Not ro.Length > 0 Then Continue For
            mnu.Text = ro(0).Item("MENUTEXT").ToString
            If ro(0).Item("ACCESSKEY").ToString.Length > 0 Then mnu.Text += " (" & ro(0).Item("ACCESSKEY").ToString & ")"
            mnu.Visible = True
            VisibleUserRightsForms(mnu)
        Next
    End Sub

    Private Sub UserRightForms()

        If userId = 999 Then
            strSql = " SELECT MENUID,MENUTEXT,ACCESSKEY,ACCESSKEY1,SHORTCUT"
            strSql += " ,(SELECT TOP 1 ACCESSID FROM " & cnAdminDb & "..PRJMENUS WHERE MENUID = M.MENUID)AS ACCESSID"
            strSql += " ,'Y' _ADD,'Y' _EDIT,'Y' _VIEW,'Y' _DEL,'Y' _EXCEL,'Y' _PRINT,'Y' _AUTHORIZE,'Y' _MISCISSUE,'Y' _CANCEL"
            strSql += " ,'Y' _SALE,'Y' _PURCHASE,'Y' _RETURN,'Y' _QSALE,REF_DLL "
            strSql += "  FROM " & cnAdminDb & "..MENUMASTER AS M"
        Else
            strSql = " SELECT M.MENUID,M.MENUTEXT,M.ACCESSKEY,M.ACCESSKEY1,M.SHORTCUT,P.ACCESSID,_ADD,_EDIT,_VIEW,_DEL,_EXCEL,_PRINT,_AUTHORIZE,_MISCISSUE,_CANCEL,_SALE,_PURCHASE,_RETURN,_QSALE, REF_DLL FROM " & cnAdminDb & "..MENUMASTER AS M"
            strSql += " INNER JOIN " & cnAdminDb & "..ROLETRAN T ON M.MENUID = T.MENUID"
            strSql += " INNER JOIN " & cnAdminDb & "..PRJMENUS P ON M.MENUID = P.MENUID"
            strSql += " WHERE T.ROLEID = (SELECT TOP 1 ROLEID FROM " & cnAdminDb & "..USERROLE WHERE USERID = (SELECT USERID FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbUserName.Text & "'))"
        End If
        _DtUserRights.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(_DtUserRights)

        If _DtUserRights.Rows.Count > 0 Then
            For Each mnu As ToolStripMenuItem In Main.MenuStrip1.Items
                Dim ro() As DataRow = _DtUserRights.Select("MENUID = '" & mnu.Name & "'", "MENUID")
                If Not ro.Length > 0 Then Continue For
                mnu.Text = ro(0).Item("MENUTEXT").ToString
                If ro(0).Item("ACCESSKEY").ToString.Length > 0 Then mnu.Text += " (" & ro(0).Item("ACCESSKEY").ToString & ")"
                mnu.Visible = True
                VisibleUserRightsForms(mnu)
            Next
        End If
    End Sub
    Private Function funcGetToolStripMenuItem(ByVal tStrip As ToolStripMenuItem) As Integer
        For Each mnu As ToolStripMenuItem In tStrip.DropDownItems
            If Not mnu.Tag Is Nothing And _PrjModules.Count > 0 Then
                If mnu.Tag.ToString <> "" Then
                    If Not _PrjModules.Contains(mnu.Tag.ToString.ToUpper) Then
                        mnu.Visible = False
                        Continue For
                    End If
                End If
            End If
            If mnu.Name = Main.OtherReportsToolStripMenuItem.Name Then
                If Not IO.File.Exists(Application.StartupPath & "\AGReports\AGOLDREPORT.exe") Then
                    mnu.Visible = False
                    Continue For
                End If
            ElseIf mnu.Name = Main.XmlGenerationToolStripMenuItem.Name Then
                If Not IO.File.Exists(Application.StartupPath & "\XmlGenerator.exe") Then
                    mnu.Visible = False
                    Continue For
                End If
            End If
            Dim str As String = mnu.Text
            str = str.Replace("&", "")
            strSql = " INSERT INTO " & cnAdminDb & "..PRJMENUS(MENUID,MENUTEXT,CHILD,ACCESSID)"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & mnu.Name & "','" & str & "'"
            strSql += " ,'" & IIf(mnu.DropDownItems.Count > 0, "", "Y") & "'"
            strSql += " ,'" & mnu.AccessibleDescription & "'"
            strSql += " )"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            If mnu.DropDownItems.Count > 0 Then
                funcGetToolStripMenuItem(mnu)
            End If
        Next
    End Function

    'Private Sub LoadMdiMenusNames()
    'Try
    '    tran = cn.BeginTransaction
    '    strSql = " DELETE FROM " & cnAdminDb & "..PRJMENUS"
    '    cmd = New OleDbCommand(strSql, cn, tran)
    '    cmd.ExecuteNonQuery()
    '    For Each mnu As ToolStripMenuItem In Main.MenuStrip1.Items
    '        If Not mnu.Tag Is Nothing And _PrjModules.Count > 0 Then
    '            If mnu.Tag.ToString <> "" Then
    '                If Not _PrjModules.Contains(mnu.Tag.ToString.ToUpper) Then
    '                    mnu.Visible = False
    '                    Continue For
    '                End If
    '            End If
    '        End If
    '        If mnu.Name = Main.tStripSavingsScheme.Name Then
    '            If Not IO.File.Exists(Application.StartupPath & "\SAVINGS\CHIT.EXE") Then
    '                mnu.Visible = False
    '                Continue For
    '            End If
    '        End If
    '        Dim str As String = Nothing
    '        str = mnu.Text
    '        str = str.Replace("&", "")

    '        strSql = " INSERT INTO " & cnAdminDb & "..PRJMENUS(MENUID,MENUTEXT,CHILD,ACCESSID)"
    '        strSql += " VALUES"
    '        strSql += " ("
    '        strSql += " '" & mnu.Name & "','" & str & "'"
    '        strSql += " ,'" & IIf(mnu.DropDownItems.Count > 0, "", "Y") & "'"
    '        strSql += " ,'" & mnu.AccessibleDescription & "'"
    '        strSql += " )"
    '        cmd = New OleDbCommand(strSql, cn, tran)
    '        cmd.ExecuteNonQuery()

    '        If mnu.DropDownItems.Count > 0 Then
    '            funcGetToolStripMenuItem(mnu)
    '        End If
    '    Next
    '    tran.Commit()
    'Catch ex As Exception
    '    If Not tran Is Nothing Then
    '        tran.Rollback()
    '    End If
    '    MsgBox("Message :" + ex.Message + " StackTrack :" + ex.StackTrace)
    'End Try
    'End Sub

    Private Sub frmLogin_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.R Then
            ent += 1
        End If
    End Sub
    Private Sub frmLogin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If AscW(e.KeyChar) = 13 Then
            If cmbUserName.Focused Then Exit Sub
            If txtPassword.Focused Then Exit Sub
            If cmbTransactionYear.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub LoadTransactionDb()
        strSql = " SELECT "
        strSql += " CONVERT(VARCHAR(10),STARTDATE,103) + ' - ' + CONVERT(VARCHAR(10),ENDDATE,103) [TRANSACTION PERIOD]"
        strSql += " ,SUBSTRING(CONVERT(VARCHAR,STARTDATE,102),1,4)+'-'"
        strSql += " +SUBSTRING(CONVERT(VARCHAR,ENDDATE,102),3,2)+ ' ('+DBNAME+')' AS TRANYEAR"
        strSql += " ,DBNAME,STARTDATE,ENDDATE FROM " & cnAdminDb & "..DBMASTER"
        strSql += " ORDER BY ENDDATE"
        da = New OleDbDataAdapter(strSql, cn)
        _DtTransactionYear = New DataTable
        da.Fill(_DtTransactionYear)
        Dim dd As Date = GetServerDate()
        For Each ro As DataRow In _DtTransactionYear.Rows
            cmbTransactionYear.Items.Add(ro.Item("TRANYEAR").ToString)
            If dd >= ro.Item("STARTDATE") And dd <= ro.Item("ENDDATE") Then
                cmbTransactionYear.Text = ro.Item("TRANYEAR").ToString
            End If
        Next
    End Sub
    Private Sub frmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Control Panel\International", "sShortDate", "yyyy/MM/dd")
        Catch ex As Exception
        End Try
        If IO.File.Exists(Application.StartupPath + "\ConInfo.ini") = False Then
            MsgBox("Company file not Found", MsgBoxStyle.Critical)
            Application.Exit()
            Exit Sub
        End If
        ConInfo = New BrighttechPack.Coninfo(Application.StartupPath + "\ConInfo.ini")
        Dim messFlag As Boolean = False
        For Each ps As System.Diagnostics.Process In System.Diagnostics.Process.GetProcessesByName("AKSMESSENGER")
            messFlag = True
            Exit For
        Next
        If IO.File.Exists(Application.StartupPath + "\AKSMESSENGER.EXE") Then
            If Not messFlag Then System.Diagnostics.Process.Start(Application.StartupPath + "\AKSMESSENGER.EXE")
        Else
            Main.tStripMessenger.Visible = False
            'MsgBox("AksMessenger not found", MsgBoxStyle.Information)
        End If
        If IO.File.Exists(Application.StartupPath + "\AskRemainder.EXE") Then
            Main.RemainderToolStripMenuItem.Visible = True
        End If
        cnCompanyId = ConInfo.lCompanyId
        cnCompanyName = ConInfo.lCompanyName
        cnDataSource = ConInfo.lServerName
        dbPath = ConInfo.lDbPath
        passWord = ConInfo.lDbPwd
        If passWord <> "" Then passWord = BrighttechPack.Methods.Decrypt(passWord)
        cnCostId = ConInfo.lCostId
        CurrencyDecimal = ConInfo.lDecimal
        cnAdminDb = cnCompanyId + "ADMINDB"
        If ConInfo.lDbLoginType.ToUpper = "W" Then
            cn = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source=" & ConInfo.lServerName & "")
        Else
            cn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=master;Data Source={0};User Id=" & IIf(ConInfo.lDbUserId <> "", ConInfo.lDbUserId, "SA") & ";password=" & passWord & ";", ConInfo.lServerName))
        End If
        'If ConInfo.lDbLoginType.ToUpper = "S" Then
        '    _CnAdmin = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & ConInfo.lCompanyId & "ADMINDB;Data Source={0};User Id=" & IIf(ConInfo.lDbUserId <> "", ConInfo.lDbUserId, "sa") & ";password=" & ConInfo.lDbPwd & ";", ConInfo.lServerName))
        'Else
        '    _CnAdmin = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & ConInfo.lCompanyId & "ADMINDB;Data Source=" & ConInfo.lServerName & "")
        'End If
        Try
            cn.Open()
        Catch ex As Exception
            MsgBox("Connection Problem" + vbCrLf + ex.Message)
            Application.Exit()
            Exit Sub
        End Try

CheckAdmindb:
        strSql = " SELECT NAME FROM SYSDATABASES WHERE NAME = '" & cnAdminDb & "'"
        Dim dtDbCheck As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDbCheck)
        If Not dtDbCheck.Rows.Count > 0 Then
            If MessageBox.Show("Admindb Database not found", "Attach Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Dim obj As New BrightUtility.SqlUtility(
                ConInfo.lServerName _
                , ConInfo.lDbLoginType _
                , ConInfo.lDbUserId _
                , passWord _
                , "A" _
                , "" _
                , ConInfo.lDbPath & "\" & ConInfo.lCompanyId & "ADMINDB.MDF"
                )
                If obj.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                    MsgBox("Admindb Attach failed", MsgBoxStyle.Information)
                    Application.Exit()
                    Exit Sub
                End If
                GoTo CheckAdmindb
            End If
            Application.Exit()
        End If
        BrighttechPack.GlobalVariables.G_SearchDialogColAutoFit = False


        objGPack = New BrighttechPack.Methods(cn, cnAdminDb, LangId _
       , focusColor, lostFocusColor, Radio_Check_LostFocusColor _
       , Button_LostFocusColor, grdBackGroundColor, bakImage _
       , textCharacterCasing, cnTranFromDate, cnTranToDate, CurrencyDecimal)

        txtPassword.CharacterCasing = CharacterCasing.Normal
        ALERTBASE_MENU = IIf(GetAdmindbSoftValue("ALERTBASE", "M") = "M", True, False)
        CENTR_DB_BR = IIf(GetAdmindbSoftValue("CENTR_DB_ALLCOSTID", "N") = "Y", True, False)
        If Not CENTR_DB_BR Or cnCostId = "" Then
            cnCostId = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='COSTID'")
        End If
        Dim lockPre As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='LOCKPREVIOUSYEARS'")
        LOCKPREVIOUSYEARS = lockPre.Split("/")
        If LOCKPREVIOUSYEARS.Length >= 2 Then
            LOCKPREVIOUSYEAR = IIf(LOCKPREVIOUSYEARS(0) = "N", False, True)
            ACCESSUSERIDS = LOCKPREVIOUSYEARS(1).Split(",")
        End If

        ''temp
        'strSql = "SELECT DATEDIFF(DD,'2017-04-04',GETDATE()) "
        'If Val(objGPack.GetSqlValue(strSql)) > 0 Then
        '    My.Settings.Demo = True
        '    DemoLogin = True
        '    ''strSql = " DELETE FROM " & cnAdminDb & "..PRJCTRL "
        '    ''cmd = New OleDbCommand(strSql, cn, tran)
        '    ''cmd.ExecuteNonQuery()
        '    ''Application.Exit() : Exit Sub
        'End If
        If cnCompanyId = "MJT" Then
            DemoLogin = True
        End If

        ''Check Application Detail
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..PRJCTRL WHERE CTLID = 2"
        Dim dtPrjDetail As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtPrjDetail)
        If Not dtPrjDetail.Rows.Count > 0 Then
            MsgBox("Invalid Version Info", MsgBoxStyle.Critical, "Application Checking Xa39292331")
            Application.Exit()
            Exit Sub
        Else
            Dim str As String = BrighttechPack.GlobalMethods.DecryptStr(dtPrjDetail.Rows(0).Item(0).ToString)
            Dim sp() As String = str.Split("=")
#If __DEBUG__ Then
            If sp(0).ToUpper <> cnCompanyName.ToUpper Then
                MsgBox("Invalid Version Info", MsgBoxStyle.Critical, "Application Checking Xa3281912")
                Application.Exit()
                Exit Sub
            End If
#End If
            _UserLimit = Val(sp(1))
            For Each s As String In sp(2).Split("-")
                Select Case s.ToUpper
                    Case "STOCK"
                        _PrjModules.Add("A")
                    Case "BILL"
                        _PrjModules.Add("B")
                    Case "ESTIMATION"
                        _PrjModules.Add("C")
                    Case "ORDER & REPAIR"
                        _PrjModules.Add("D")
                    Case "ACCOUNTS"
                        _PrjModules.Add("E")
                    Case "STORE MANAGEMENT"
                        _PrjModules.Add("F")
                    Case "SAVINGS SCHEME"
                        _PrjModules.Add("G")
                End Select
            Next
            ''Get Trail Pack Info
            If sp.Length >= 4 Then
                If sp(3).ToUpper = "Y" Then
                    _Demo = True
                End If
            End If
            If sp.Length >= 5 Then
                _DemoExpiredDate = Convert.ToDateTime(sp(4)).AddDays(_DemoLImitDays)
            End If
        End If

        LoadTransactionDb()
        AddAccEntryMenus()
        AddOtherMasterEntryMenus()
        'LoadMdiMenusNames()
        Dim AdminHide As Boolean = IIf(GetAdmindbSoftValue("HIDEADMIN", "N") = "Y", True, False)
        Dim ds As New Data.DataSet
        ds.Clear()
        Try
            If GetAdmindbSoftValue("HIDEUSER", "N") = "Y" Then
                'cmbUserName.DropDownStyle = ComboBoxStyle.Simple
                'cmbUserName.MaxDropDownItems = 1
                cmbUserName.DropDownStyle = ComboBoxStyle.DropDown
            Else
                cmbUserName.DropDownStyle = ComboBoxStyle.DropDown
                strSql = " SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE ISNULL(ACTIVE,'') = 'Y'"
                If CENTR_DB_BR = False Then
                    strSql += " AND (ISNULL(COSTID,'') = '" & cnCostId & "' OR ISNULL(CENTLOGIN,'') = 'Y' OR CHARINDEX('" & cnCostId & "',USERCOSTID) > 0 ) "
                End If
                strSql += " AND USERID <> '999' "
                If Not AdminHide Then
                    strSql += " UNION ALL"
                    strSql += " SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = 999 AND ISNULL(ACTIVE,'Y') = 'Y'"
                End If
                strSql += " ORDER BY USERNAME"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(ds, "log")
                If ds.Tables("log").Rows.Count > 0 Then
                    Dim i
                    For i = 0 To ds.Tables("log").Rows.Count - 1
                        cmbUserName.Items.Add(ds.Tables("log").Rows(i).Item("USERNAME").ToString)
                    Next
                End If
            End If
        Catch ex As Exception
            MsgBox("CONNECTION FAILED", MsgBoxStyle.Critical)
            MsgBox(ex.Message + vbCrLf + "StackTrace    :" + ex.StackTrace, MsgBoxStyle.Critical)
            Me.Close()
        End Try

        Dim _AuditMsgHide As Boolean = IIf(GetAdmindbSoftValue("HIDEAUDIT_MSG", "N") = "Y", True, False)
        If _AuditMsgHide Then
            LblAuditMsg.Visible = False
        Else
            LblAuditMsg.Visible = True
        End If

        If _Demo Then
            strSql = " SELECT CRDATE,DATEDIFF(DAY,CRDATE,'" & Today.Date.ToString("yyyy-MM-dd") & "')AS DIFF FROM MASTER..SYSDATABASES WHERE NAME = '" & cnAdminDb & "'"
            Dim dtDemo As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtDemo)
            If dtDemo.Rows.Count > 0 Then
                With dtDemo.Rows(0)
                    If _DemoExpiredDate.Year = 1 Then
                        _DemoExpiredDate = Convert.ToDateTime(.Item("CRDATE")).AddDays(_DemoLImitDays).Date
                    End If
                    If _DemoExpiredDate.Date <> Convert.ToDateTime(.Item("CRDATE")).AddDays(_DemoLImitDays).Date Then
                        MsgBox("Invalid Version Info" + vbCrLf + " Db Crdate Conflict", MsgBoxStyle.Critical, "Application Checking Xa39298931")
                        Application.Exit()
                        Exit Sub
                    End If
                    If Val(.Item("DIFF").ToString) > _DemoLImitDays Then
                        MsgBox("Your " & _DemoLImitDays & " days trail period expired. Please try evaluate version", MsgBoxStyle.Information)
                        Application.Exit()
                        Exit Sub
                    ElseIf Val(.Item("Diff").ToString) < 0 Then
                        MsgBox("Please change the system date" + vbCrLf + "Your trail period version should allow " & CType(.Item("CRDATE"), Date).ToString("dd/MM/yyyy") & " Onwards only", MsgBoxStyle.Information)
                        Application.Exit()
                        Exit Sub
                    Else
                        MsgBox("" & _DemoLImitDays & " days Trail verion pack" + vbCrLf + .Item("DIFF").ToString + " Day(s) left..", MsgBoxStyle.Information)
                    End If
                End With
            Else
                MsgBox("Database not found", MsgBoxStyle.Information)
                Application.Exit()
                Exit Sub
            End If
        End If

        cmbLanguage.Items.Clear()
        cmbLanguage.Items.Add("English")
        strSql = " SELECT LANGNAME FROM " & cnAdminDb & "..LANGMASTER ORDER BY LANGNAME"
        objGPack.FillCombo(strSql, cmbLanguage, False, False)
        cmbLanguage.Text = "English"
    End Sub


    'Private Sub SingleTranDbMaintanance()
    '    ''Single Trandb Maintanens
    '    cnStockDb = ConInfo.lCompanyId + "T" + "0910"
    '    cnTranFromDate = "2009-04-01"
    '    cnTranToDate = "2098-03-31"
    '    objGPack = New BrighttechPack.Methods(cn, cnAdminDb, LangId _
    '  , focusColor, lostFocusColor, Radio_Check_LostFocusColor _
    '  , Button_LostFocusColor, grdBackGroundColor, bakImage _
    '  , textCharacterCasing, cnTranFromDate, cnTranToDate)
    '    Me.Hide()
    '    Dim myBuildInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath)
    '    VERSION = myBuildInfo.FileVersion
    '    ''COMPANY SELECTION**********
    '    strSql = " SELECT * FROM " & cnAdminDb & "..COMPANY"
    '    Dim dtComp As New DataTable
    '    da = New OleDbDataAdapter(strSql, cn)
    '    da.Fill(dtComp)
    '    If dtComp.Rows.Count > 0 Then
    '        If dtComp.Rows.Count > 1 Then
    '            frmCompany.ShowDialog()
    '        Else
    '            EstPost()
    '            Main.tStripCompanySelection.Enabled = False
    '            Main.tStripCompanySelection.Visible = False
    '            Main.Text = dtComp.Rows(0).Item("COMPANYNAME").ToString + " Version : " + VERSION
    '            strCompanyId = dtComp.Rows(0).Item("COMPANYID").ToString
    '            strCompanyName = dtComp.Rows(0).Item("COMPANYNAME").ToString
    '        End If
    '    Else
    '        MsgBox("Company Details Not Found,Contact System Administrator", MsgBoxStyle.Information)
    '        Application.Exit()
    '        Exit Sub
    '    End If
    '    Main.Show()
    'End Sub
    Private Sub MainEntry()
TranDbExistcheck:
        If UCase(objGPack.GetSqlValue("SELECT NAME FROM MASTER..SYSDATABASES WHERE NAME = '" & cnStockDb & "'")).ToString = "" Then
            Me.Hide()
            Dim PASSWORD As String = ConInfo.lDbPwd
            If PASSWORD <> "" Then PASSWORD = BrighttechPack.Methods.Decrypt(PASSWORD)
            Dim obj As New BrightUtility.SqlUtility(
                        ConInfo.lServerName _
                        , ConInfo.lDbLoginType _
                        , ConInfo.lDbUserId _
                        , PASSWORD _
                        , "A" _
                        , "" _
                        , ConInfo.lDbPath & " \" & cnStockDb & ".MDF"
                       )
            If obj.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                MsgBox("Transaction Attach failed", MsgBoxStyle.Information)
                Application.Exit()
                Exit Sub
            Else
                GoTo TranDbExistcheck
            End If
        End If

        objGPack = New BrighttechPack.Methods(cn, cnAdminDb, LangId _
            , focusColor, lostFocusColor, Radio_Check_LostFocusColor _
            , Button_LostFocusColor, grdBackGroundColor, bakImage _
            , textCharacterCasing, cnTranFromDate, cnTranToDate, CurrencyDecimal)

        Dim myBuildInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath)
        Dim myBuildInfoMaster As FileVersionInfo = FileVersionInfo.GetVersionInfo(Application.StartupPath & "\BrightMaster.DLL")
        Dim myBuildInfoRep As FileVersionInfo = FileVersionInfo.GetVersionInfo(Application.StartupPath & "\BrightReport.DLL")
        VERSION = myBuildInfo.FileVersion
        Dim MASTVERSION As String = myBuildInfoMaster.FileVersion
        Dim REPVERSION As String = myBuildInfoRep.FileVersion
        If VERSION <> MASTVERSION Then MsgBox("BrightMaster Conflicts", MsgBoxStyle.Information) : Me.Close() : Exit Sub
        If VERSION <> REPVERSION Then MsgBox("BrightReport Conflicts", MsgBoxStyle.Information) : Me.Close() : Exit Sub
        Dim preVersion As String = GetAdmindbSoftValue("APPVERSION", "")
        If preVersion <> "" Then
            Dim preVer() As String = preVersion.Split(".")
            Dim curVer() As String = VERSION.Split(".")
            If Val(curVer(0)) < Val(preVer(0)) Then
                MsgBox("Conflict Application Version" + vbCrLf + "New Version : " + preVersion + vbCrLf + "Current Version : " + VERSION + vbCrLf + "Please Copy New Version", MsgBoxStyle.Information)
                Me.Close()
                Exit Sub
            End If
            If (Val(curVer(1)) < Val(preVer(1))) And Val(curVer(0)) <= Val(preVer(0)) Then
                MsgBox("Conflict Application Version" + vbCrLf + "New Version : " + preVersion + vbCrLf + "Current Version : " + VERSION + vbCrLf + "Please Copy New Version", MsgBoxStyle.Information)
                Me.Close()
                Exit Sub
            End If
            If (Val(curVer(2)) < Val(preVer(2))) And Val(curVer(1)) <= Val(preVer(1)) And Val(curVer(0)) <= Val(preVer(0)) Then
                MsgBox("Conflict Application Version" + vbCrLf + "New Version : " + preVersion + vbCrLf + "Current Version : " + VERSION + vbCrLf + "Please Copy New Version", MsgBoxStyle.Information)
                Me.Close()
                Exit Sub
            End If
            If (Val(curVer(3)) < Val(preVer(3))) And Val(curVer(2)) <= Val(preVer(2)) And Val(curVer(1)) <= Val(preVer(1)) And Val(curVer(0)) <= Val(preVer(0)) Then
                MsgBox("Conflict Application Version" + vbCrLf + "New Version : " + preVersion + vbCrLf + "Current Version : " + VERSION + vbCrLf + "Please Copy New Version", MsgBoxStyle.Information)
                Me.Close()
                Exit Sub
            End If
        End If
        Me.Hide()

        cnCostName = objGPack.GetSqlValue("SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & cnCostId & "'")

COMPANYSELECT:
        ''COMPANY SELECTION**********
        strSql = " SELECT * FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'Y') = 'Y'"
        Dim dtComp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtComp)
        If dtComp.Rows.Count > 0 Then
            If dtComp.Rows.Count > 1 Then
                frmCompany.ShowDialog()
            Else
                EstPost()
                Main.tStripCompanySelection.Enabled = False
                Main.tStripCompanySelection.Visible = False
                Main.Text = dtComp.Rows(0).Item("COMPANYNAME").ToString + IIf(cnCostName <> "", "_" + cnCostName, "") + IIf(_Demo, " " & _DemoLImitDays & " Days Trial", "") + " Version : " + VERSION
                strCompanyId = dtComp.Rows(0).Item("COMPANYID").ToString
                strCompanyName = dtComp.Rows(0).Item("COMPANYNAME").ToString
                BrighttechREPORT.Globalvariables.strCompanyName = strCompanyName
                BrighttechREPORT.Globalvariables.strCompanyId = strCompanyId
                CompanyStateId = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = '" & strCompanyId & "'").ToString)
                CompanyState = objGPack.GetSqlValue("SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID = '" & CompanyStateId & "'").ToString
            End If
        Else
            strSql = " SELECT * FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'Y') = 'N'"
            Dim dtDComp As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtDComp)
            If dtDComp.Rows.Count > 0 Then
                MsgBox("Atleast one company is active. Please activate the company..", MsgBoxStyle.Information)
                frmCompany.Statuschange = True
                frmCompany.ShowDialog()
                GoTo COMPANYSELECT
            Else
                MsgBox("Company Details Not Found,Contact System Administrator", MsgBoxStyle.Information)
                Application.Exit()
                Exit Sub
            End If
        End If
        ''***************************
        ''COSTCENTRE SELECTION**********


        If CENTR_DB_BR Then
            strSql = "SELECT COUNT(*) FROM " & cnAdminDb & "..COSTCENTRE"
            If Val(objGPack.GetSqlValue(strSql, , "", )) > 0 Then
                Dim UserCostid As String = ""
                strSql = "SELECT ISNULL(COSTID,''),ISNULL(CENTLOGIN,'N'),ISNULL(USERCOSTID,'')  FROM " & cnAdminDb & "..USERMASTER WHERE USERID=" & userId & ""
                Dim druser As DataRow = GetSqlRow(strSql, cn)
                strBCostid = druser.Item(0).ToString
                strUserCentrailsed = druser.Item(1).ToString
                UserCostid = druser.Item(2).ToString
                If userId = 999 Then strUserCentrailsed = "Y"
                If strBCostid <> "" Or (UserCostid <> "" And UserCostid.Split(",").Length = 1) And strUserCentrailsed <> "Y" Then
                    strSql = "SELECT COSTID,COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE 1=1"
                    If strBCostid <> "" Then strSql += " and COSTID='" & strBCostid & "'"
                    If UserCostid <> "" Then strSql += " and COSTID='" & UserCostid & "'"

                    Dim dtCost As New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtCost)
                    If dtCost.Rows.Count > 0 Then
                        strBCostid = dtCost.Rows(0).Item("COSTID").ToString
                    Else
                        frmCostcentreSelection.Usercostids = UserCostid
                        frmCostcentreSelection.ShowDialog()
                    End If
                Else
                    frmCostcentreSelection.Usercostids = UserCostid
                    frmCostcentreSelection.ShowDialog()
                End If
            End If
            cnCostId = strBCostid
            cnCostName = objGPack.GetSqlValue("SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & strBCostid & "'")
            BrighttechREPORT.Globalvariables.cnCostId = strBCostid
            BrighttechREPORT.Globalvariables.cnCostName = cnCostName
            Main.Text = strCompanyName + IIf(cnCostName <> "", "_" + cnCostName, "") + IIf(_Demo, " " & _DemoLImitDays & " Days Trial", "") + " Version : " + VERSION
        Else
            strSql = "SELECT ISNULL(COSTID,''),ISNULL(CENTLOGIN,'N')  FROM " & cnAdminDb & "..USERMASTER WHERE USERID=" & userId & ""
            Dim druser As DataRow = GetSqlRow(strSql, cn)
            strUserCentrailsed = druser.Item(1).ToString

        End If
        ''***************************

        If COSTCENTRE_SINGLE And strUserCentrailsed <> "Y" Then
            cnDefaultCostId = IIf(cnCostId = cnHOCostId, True, False)
        End If

        Temprefresh()
        EstPost()
        'If Not CENTR_DB_BR Then SyncPost()
        Datacheck()
        Main.Show()
        strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & VERSION & "' WHERE CTLID = 'APPVERSION'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        Call GetandSetIpname()
        ''****************************
        strSql = "UPDATE " & cnAdminDb & "..PWDMASTER SET PWDSTATUS = 'E' WHERE PWDDATE < DATEADD(DD,-PWDEXPIRY,'" & GetServerDate() & "') AND PWDSTATUS='N' "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Public Sub Temprefresh()
        Try
            Dim TempFilePath As String = IO.Path.GetTempPath
            Dim xmlFiles = Directory.GetFiles(TempFilePath, "*.xml", SearchOption.TopDirectoryOnly)
            Dim FileDate As Date
            For Each Filename As String In xmlFiles
                FileDate = IO.File.GetLastWriteTime(Filename).Date
                If FileDate < Today Then IO.File.Delete(Filename)
            Next
            Dim tempFiles = Directory.GetFiles(TempFilePath, "*.tmp", SearchOption.TopDirectoryOnly)
            For Each Filename As String In tempFiles
                FileDate = IO.File.GetLastWriteTime(Filename).Date
                If FileDate < Today Then IO.File.Delete(Filename)
            Next
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Login()
        '_SingTranDb = GetAdmindbSoftValue("SING-TRANDB", "N")
        'If _SingTranDb = "Y" Then
        '    SingleTranDbMaintanance()
        '    Main.TransactionYearTStripLabel.Text = ""
        '    Exit Sub
        'End If
DbCheck:
        For Each ro As DataRow In _DtTransactionYear.Rows
            If ro.Item("TRANYEAR").ToString = cmbTransactionYear.Text Then
                cnStockDb = ro.Item("DBNAME").ToString
                cnTranFromDate = ro.Item("STARTDATE").ToString
                cnTranToDate = ro.Item("ENDDATE").ToString
                Main.TransactionYearTStripLabel.Text = cmbTransactionYear.Text
                MainEntry()
                Exit Sub
            End If
        Next
        If cnStockDb = "" Then
            Dim ro() As DataRow = _DtUserRights.Select("MENUID = '" & Main.YearEndProcessTStrip.Name & "'", "MENUID")
            If Not ro.Length > 0 Then
                MsgBox("Transaction Database not found", MsgBoxStyle.Information)
                Application.Exit()
                Exit Sub
            End If
            If MessageBox.Show("Transaction Database Does not exist" + vbCrLf + "Do you want to create new transaction database?", "Database not found", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Dim dd As Date = GetServerDate(tran)
                Dim objYEnd As New DatabaseCreator(dd, dd.AddYears(1), True)
                If objYEnd.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                    Application.Exit()
                    Exit Sub
                End If
            Else
                Application.Exit()
                Exit Sub
            End If
            ''After Year End Creation
            LoadTransactionDb()
            GoTo DbCheck
        End If
    End Sub
    Public Sub GetandSetIpname()
        HostName = System.Net.Dns.GetHostName()
        IpAddress = System.Net.Dns.GetHostByName(HostName).AddressList(0).ToString()
        'MessageBox.Show("Host Name: " & HostName & "; IP Address: " & IpAddress)
        strSql = "INSERT INTO " & cnAdminDb & "..LOGINDETAIL(SYSTEMIP ,SYSTEMNAME ,USERID ,LOGINTIME ,LOGINSTATUS) "
        strSql += " values ('" & IpAddress & "','" & HostName & "'," & userId & ",'" & Now & "','R')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub



    Public Sub SyncPost()
        Dim mSyncPost() As String = Split(GetAdmindbSoftValue("SYNCPOST", "N,0"), ",")

        If mSyncPost(0).ToString <> "Y" Then Exit Sub
        Dim Todaydate As Date = DateAdd(DateInterval.Day, Val(mSyncPost(1).ToString) * (-1), GetEntryDate(GetServerDate))
        Dim syncdb As String = cnAdminDb
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix) <> 0 Then syncdb = uprefix + usuffix
        End If
        Dim currcostid As String = GetAdmindbSoftValue("COSTID", "", tran)
        strSql = " SELECT 'EXIST' FROM "
        strSql += " ("
        strSql += " SELECT 'EXIST' CHECKS FROM " & syncdb & "..SENDSYNC WHERE STATUS ='Y'"
        strSql += " AND UPDATED < '" & Todaydate.ToString("dd-MMM-yyyy") & "'"
        strSql += " UNION"
        strSql += " SELECT 'EXIST' FROM " & syncdb & "..RECEIVESYNC WHERE STATUS ='Y'"
        strSql += " AND UPDATED < '" & Todaydate.ToString("dd-MMM-yyyy") & "'"
        strSql += " )x"
        If objGPack.GetSqlValue(strSql, , , tran) = "" Then Exit Sub
        Dim dtstr = GetEntryDate(GetServerDate).ToString("ddMMyy")
        Dim Syncbackfiles As String = "SYNC" + currcostid + dtstr + "S"
        Dim Bakpath As String = "\\" + ConInfo.lServerName + "\" + Replace(ConInfo.lDbPath, ":", "") + "\SYNCBACK"
        If Not IO.Directory.Exists(Bakpath) Then IO.Directory.CreateDirectory(Bakpath)
        Bakpath = Bakpath + "\"
        Dim DsSend As New DataSet
        Dim XmlFilePath As String = Nothing
        Dim ZipFilePath As String = Nothing
        XmlFilePath = Bakpath & Syncbackfiles & ".xml"
        ZipFilePath = Bakpath & Syncbackfiles & ".zip"
        Dim S_Dt As New DataTable
        strSql = " SELECT * FROM " & syncdb & "..SENDSYNC WHERE STATUS ='Y'"
        strSql += " AND UPDATED < '" & Todaydate.ToString("dd-MMM-yyyy") & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(S_Dt)
        DsSend.Tables.Add(S_Dt)
        DsSend.WriteXml(XmlFilePath, XmlWriteMode.WriteSchema)
        Dim Syncbackfiler As String = "SYNC" + currcostid + dtstr + "R"
        Dim DsRecv As New DataSet
        strSql = " DELETE FROM " & syncdb & "..SENDSYNC WHERE STATUS ='Y'"
        strSql += " AND UPDATED < '" & Todaydate.ToString("dd-MMM-yyyy") & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        XmlFilePath = Bakpath & Syncbackfiler & ".xml"
        ZipFilePath = Bakpath & Syncbackfiler & ".zip"
        strSql = " SELECT  * FROM " & syncdb & "..RECEIVESYNC WHERE STATUS ='Y'"
        strSql += " AND UPDATED < '" & Todaydate.ToString("dd-MMM-yyyy") & "'"
        Dim R_Dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(R_Dt)
        DsRecv.Tables.Add(R_Dt)
        DsRecv.WriteXml(XmlFilePath, XmlWriteMode.WriteSchema)
        strSql = " DELETE FROM " & syncdb & "..RECEIVESYNC WHERE STATUS ='Y'"
        strSql += " AND UPDATED < '" & Todaydate.ToString("dd-MMM-yyyy") & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
    End Sub

    Public Sub EstPost()
        If GetAdmindbSoftValue("ESTPOST", "Y") <> "Y" Then Exit Sub
        Dim ro() As DataRow = _DtUserRights.Select("MENUID = '" & Main.EstimatePostTrip.Name & "'", "MENUID")
        If Not ro.Length > 0 Then Exit Sub
        strSql = " SELECT 'EXIST' FROM "
        strSql += " ("
        strSql += " SELECT 'EXIST' CHECKS FROM " & cnStockDb & "..ESTISSUE WHERE TRANDATE < '" & GetEntryDate(GetServerDate) & "'"
        strSql += " UNION"
        strSql += " SELECT 'EXIST' FROM " & cnStockDb & "..ESTRECEIPT  WHERE TRANDATE < '" & GetEntryDate(GetServerDate) & "'"
        strSql += " )x"
        If objGPack.GetSqlValue(strSql, , , tran) <> "" Then
            Dim obj As New EstimatePostingFrm
            obj.ShowDialog()
        End If
    End Sub


    Public Sub Datacheck()
        Dim ro() As DataRow = _DtUserRights.Select("MENUID = '" & Main.tStripAccountsEnt.Name & "'", "MENUID")
        If Not ro.Length > 0 Then Exit Sub
        If GetAdmindbSoftValue("DATACHECK", "N") <> "Y" Then Exit Sub
        Dim chkdate As DateTime = GetEntryDate(GetServerDate)
        Dim balAmt As Double = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN TRANMODE = 'C' THEN isnull(AMOUNT,0) ELSE isnull(AMOUNT,0)*(-1) END) AS BALANCE FROM " & cnStockDb & "..ACCTRAN WHERE isnull(CANCEL,'') <> 'Y' AND TRANDATE < '" & chkdate & "'", , "0"))
        Dim minVal As Decimal = Val(GetAdmindbSoftValue("CHECKTRAN_VAL", ".01"))
        If Math.Abs(balAmt) > minVal Then
            If MsgBox("Your Accounts Not Tally As on Date : " & DateAdd(DateInterval.Day, -1, chkdate) & " Difference was " & balAmt.ToString & vbCrLf & "Can you verify the datewise detail ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                strSql = "SELECT COSTID,TRANDATE,SUM(CASE WHEN TRANMODE = 'C' THEN isnull(AMOUNT,0) ELSE isnull(AMOUNT,0)*(-1) END) AS BALANCE FROM " & cnStockDb & "..ACCTRAN WHERE isnull(CANCEL,'') <> 'Y' AND TRANDATE < '" & chkdate & "' GROUP BY COSTID,TRANDATE having SUM(CASE WHEN TRANMODE = 'C' THEN isnull(AMOUNT,0) ELSE isnull(AMOUNT,0)*(-1) END) not between " & minVal * (-1) & " and " & minVal & " ORDER BY COSTID,TRANDATE"

                BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn)
            End If
            Exit Sub
        End If
    End Sub

    'Private Sub AddAppVersionCol(ByVal VERSION As String)
    '    Try
    '        tran = cn.BeginTransaction
    '        strSql = "UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & VERSION & "' WHERE CTLID = 'APPVER'"
    '        cmd = New OleDbCommand(strSql, cn, tran)
    '        cmd.ExecuteNonQuery()

    '        Dim dtTableNames As New DataTable
    '        strSql = "SELECT NAME FROM " & cnStockDb & "..SYSOBJECTS WHERE XTYPE = 'U'"
    '        cmd = New OleDbCommand(strSql, cn, tran)
    '        da = New OleDbDataAdapter(cmd)
    '        da.Fill(dtTableNames)

    '        For Each roTable As DataRow In dtTableNames.Rows
    '            strSql = " if (SELECT COUNT(*) FROM " & CNSTOCKDB & "..SYSCOLUMNS "
    '            strSql += " WHERE ID = (SELECT ID FROM " & CNSTOCKDB & "..SYSOBJECTS WHERE NAME = '" & roTable!name.ToString & "')"
    '            strSql += " AND NAME = 'APPVER')>0"
    '            strSql += " alter table " & CNSTOCKDB & ".." & roTable!name.ToString & " drop column appver"

    '            'strSql = " IF (SELECT COUNT(*) FROM " & cnStockDb & "..SYSCOLUMNS "
    '            'strSql += " WHERE ID = (SELECT ID FROM " & cnStockDb & "..SYSOBJECTS WHERE NAME = '" & roTable!NAME.ToString & "') "
    '            'strSql += " AND NAME = 'APPVER') > 0"
    '            'strSql += " BEGIN"            ''APP VER COLUMN EXIST
    '            'strSql += "     IF (SELECT COUNT(*) FROM " & cnStockDb & "..SYSOBJECTS WHERE XTYPE = 'D' AND PARENT_OBJ = (SELECT ID FROM " & cnStockDb & "..SYSOBJECTS WHERE NAME = '" & roTable!NAME.ToString & "')"
    '            'strSql += "     AND ID IN (SELECT CDEFAULT FROM " & cnStockDb & "..SYSCOLUMNS WHERE  NAME = 'APPVER'))>0"
    '            'strSql += "     BEGIN" '' IF CONSTRAINTS EXIST
    '            'strSql += "     ALTER TABLE " & cnStockDb & ".." & roTable!NAME.ToString & ""
    '            'strSql += "     DROP CONSTRAINT " & objGPack.GetSqlValue("SELECT NAME FROM " & cnStockDb & "..SYSOBJECTS WHERE XTYPE = 'D' AND PARENT_OBJ = (SELECT ID FROM " & cnStockDb & "..SYSOBJECTS WHERE NAME = '" & roTable!NAME.ToString & "') AND ID IN (SELECT CDEFAULT FROM " & cnStockDb & "..SYSCOLUMNS WHERE  NAME = 'APPVER')", , "DEF", tran) & ""
    '            ''strSql += "     ALTER TABLE " & cnStockDb & ".." & roTable!NAME.ToString & ""
    '            ''strSql += "     ALTER COLUMN APPVER VARCHAR(12)"
    '            ''strSql += "     ALTER TABLE " & cnStockDb & ".." & roTable!NAME.ToString & ""
    '            ''strSql += "     ADD CONSTRAINT DF_" & roTable!NAME.ToString & "_APPVER DEFAULT ('" & VERSION & "') FOR APPVER"
    '            'strSql += "     END"
    '            ''strSql += "     ELSE"
    '            ''strSql += "     BEGIN"  ' IF CONSTRAINTS NOT EXIST
    '            ''strSql += "     ALTER TABLE " & cnStockDb & ".." & roTable!NAME.ToString & ""
    '            ''strSql += "     ADD CONSTRAINT DF_" & roTable!NAME.ToString & "_APPVER DEFAULT ('" & VERSION & "') FOR APPVER"
    '            ''strSql += "     END"
    '            'strSql += " END"
    '            ''strSql += " ELSE"
    '            ''strSql += " BEGIN" ''APP VER COLUMN NOT EXIST
    '            ''strSql += " ALTER TABLE " & cnStockDb & ".." & roTable!NAME.ToString & " ADD APPVER VARCHAR(11) DEFAULT '" & VERSION & "'"
    '            ''strSql += " END"
    '            cmd = New OleDbCommand(strSql, cn, tran)
    '            cmd.ExecuteNonQuery()
    '        Next
    '        tran.Commit()
    '        MsgBox("Version Updated..")
    '    Catch ex As Exception
    '        If Not tran Is Nothing Then
    '            tran.Dispose()
    '        End If
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub

    Private Sub LoginToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoginToolStripMenuItem.Click
        If ent <> 5 Then
            Exit Sub
        End If
        userId = 999
        HideMenus()
        UserRightForms()
        cnUserName = cmbUserName.Text
        cnPassword = txtPassword.Text
        Main.tStripExit.Visible = True
        Login()
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CHITDBPREFIX'"
        If (Trim(objGPack.GetSqlValue(strSql, , ""))) <> "" Then
            cnChitTrandb = Trim(objGPack.GetSqlValue(strSql, , "")) + "SH0708"
            cnChitCompanyid = Trim(objGPack.GetSqlValue(strSql, , ""))
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
        If Not cn.State = ConnectionState.Closed Then
            cn.Close()
        End If
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        'Exit Sub
        If _PrjModules.Count > 0 Then
            strSql = " SELECT COUNT(*) FROM " & cnAdminDb & "..USERS"
            If (Val(BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql)) + 1) > _UserLimit Then
                If MessageBox.Show("Exceed User Limit" + vbCrLf + "Do you want to clear previous application", "User Limit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    strSql = " DELETE FROM " & cnAdminDb & "..USERS"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                Else
                    Exit Sub
                End If
            End If
        End If

        Dim ds As New DataSet
        ds.Clear()
        If cmbUserName.Text = "" Then
            MsgBox("Invalid UserName", MsgBoxStyle.Information)
            cmbUserName.Select()
            Exit Sub
        End If
        Try
            'Dim pWord As String = UCase(objGPack.GetSqlValue("SELECT PWD FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbUserName.Text & "'", , Nothing))
            'Dim USERNAME As String = UCase(objGPack.GetSqlValue("SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbUserName.Text & "' AND (ACTIVE='Y' OR USERID=999)", , Nothing))
            Dim USERNAME As String = UCase(objGPack.GetSqlValue("SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbUserName.Text & "' ", , Nothing))
            If USERNAME = Nothing Or USERNAME = "" Then
                MsgBox("Invalid UserName", MsgBoxStyle.Information)
                cmbUserName.Focus()
                cmbUserName.SelectAll()
                Exit Sub
            End If
            USERNAME = UCase(objGPack.GetSqlValue("SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbUserName.Text & "' AND ISNULL(ACTIVE,'Y')<>'N' ", , Nothing))
            If USERNAME = Nothing Or USERNAME = "" Then
                MsgBox("InActive User", MsgBoxStyle.Information)
                cmbUserName.Focus()
                cmbUserName.SelectAll()
                Exit Sub
            End If
            Dim _usertxtpwd As String = BrighttechPack.Methods.Encrypt(txtPassword.Text)
            Dim pwd As String = objGPack.GetSqlValue("SELECT PWD FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbUserName.Text & "' AND PWD = '" & _usertxtpwd.ToString & "'", , Nothing)     ''BrighttechPack.Methods.Encrypt(txtPassword.Text)
            'pwd = objGPack.GetSqlValue("SELECT PWD FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbUserName.Text & "'", , Nothing)
            'pwd = BrighttechPack.Methods.Decrypt(pwd)
            If pwd = Nothing Then
                MsgBox("Invalid Password", MsgBoxStyle.Information)
                txtPassword.Focus()
                txtPassword.SelectAll()
                Exit Sub
            End If
            ''If BrighttechPack.Methods.Encrypt(txtPassword.Text.ToString) <> pwd.ToString Then
            If _usertxtpwd.ToString <> pwd.ToString Then
                MsgBox("Invalid Password", MsgBoxStyle.Information)
                txtPassword.Focus()
                txtPassword.SelectAll()
                Exit Sub
            End If

        Catch ex As Exception
            MsgBox("Message :" + ex.Message + vbCrLf + "StackTrace  :" + ex.StackTrace)
            Exit Sub
        End Try

        If Not CheckPwdExpiry() Then
            MsgBox("Password Expired.", MsgBoxStyle.Information)
            lblChangePassword_Click(Me, New EventArgs)
            Exit Sub
        End If



        LangId = objGPack.GetSqlValue("SELECT TOP 1 LANGID FROM " & cnAdminDb & "..LANGMASTER WHERE LANGNAME = '" & lblLanguage.Text & "'", "LANGID", "ENG")
        If LangId = "ENG" Then Main.tStripLanguageChange.Enabled = True Else Main.tStripLanguageChange.Enabled = False

        strSql = " INSERT INTO " & cnAdminDb & "..MENUMASTER(MENUID,MENUTEXT)"
        strSql += " SELECT MENUID,MENUTEXT FROM " & cnAdminDb & "..PRJMENUS AS P"
        strSql += " WHERE NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..MENUMASTER WHERE MENUID = P.MENUID)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " DELETE FROM " & cnAdminDb & "..MENUMASTER WHERE MENUID IN ("
        strSql += " SELECT MENUID FROM " & cnAdminDb & "..MENUMASTER AS M"
        strSql += " WHERE NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..PRJMENUS WHERE MENUID = M.MENUID)"
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        SetGlobalVariables()
        userId = Val(objGPack.GetSqlValue("SELECT USERID FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbUserName.Text & "' AND PWD = '" & BrighttechPack.Methods.Encrypt(txtPassword.Text) & "'", , ""))
        If objGPack.GetSqlValue("SELECT CENTLOGIN FROM " & cnAdminDb & "..USERMASTER WHERE USERID=" & userId & "", , "N").ToString.ToUpper = "N" Then
            If objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..USERMASTER WHERE USERID=" & userId & "", , "").ToString.ToUpper <> cnCostId Then
                If objGPack.GetSqlValue("SELECT USERCOSTID FROM " & cnAdminDb & "..USERMASTER WHERE USERID=" & userId & "", , "").ToString.ToUpper.Contains(cnCostId) = False Then
                    MsgBox("USER CANNOT BE LOGGED IN THIS COST CENTRE")
                    Exit Sub
                End If
            End If
        End If
        If userId <> 999 Then
            strSql = "SELECT ISNULL(ADMINACCESS,'N') ADMINACCESS FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLEID = (SELECT TOP 1 ROLEID FROM " & cnAdminDb & "..USERROLE WHERE USERID = " & userId & ")"
            If objGPack.GetSqlValue(strSql) <> "Y" Then _IsAdmin = False Else _IsAdmin = True
            If GetAdmindbSoftValue("ADMINACCESS", "N").ToUpper <> "Y" Then _IsAdmin = False
        End If

        If LOCKPREVIOUSYEAR Then
            If ACCESSUSERIDS.Length > 0 Then
                If cmbTransactionYear.SelectedIndex <> cmbTransactionYear.Items.Count - 1 Then
                    For Each str As String In ACCESSUSERIDS
                        If str = userId.ToString Then
                            GoTo nxt
                        End If
                    Next
                    MsgBox("USER CANNOT ACCESS TO LOGGED IN THIS FINANCIAL YEAR")
                    Application.Exit()
                End If
            End If
        End If
nxt:
        Dim mLogincontrol As String = GetAdmindbSoftValue("MULTILOGINCONTROL", "N").ToUpper
        If mLogincontrol <> "N" Then
            Dim dtLogin As New DataTable
            strSql = " SELECT isnull(systemip,'')  FROM " & cnAdminDb & "..LOGINDETAIL where userid = " & userId & " and Loginstatus ='R' ORDER BY LOGINTIME DESC"
            Dim stripaddr As String = objGPack.GetSqlValue(strSql)
            If stripaddr <> "" Then
                Dim memfile As String = Application.StartupPath & "\" & stripaddr & userId.ToString & "Errorlog.err"
                If Not IO.File.Exists(memfile) Then
                    MsgBox("Please check.User already running on (IP: " & stripaddr & ")")
                    If mLogincontrol = "R" Then Exit Sub
                Else
                    IO.File.Delete(memfile)
                End If
            End If
        End If

        If GetAdmindbSoftValue("GLOBALDATE", "N").ToUpper = "Y" Then
            If GetAdmindbSoftValue("GLOBALDATEVAL") = "" Then MsgBox("GLOBAL DATE VALUE IS EMPTY" & vbCrLf & "REINTIALIZED THE VALUE IN SOFTCONTROL")
        End If
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMSURL'"
        SMSURL = objGPack.GetSqlValue(strSql, , "")
        strSql = " SELECT 1 FROM " & cnAdminDb & "..USERMASTER WHERE USERID =" & userId & " AND ISNULL(AUTHPWD,'') = ''"
        If (Val(BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql))) = 1 Then
            strSql = " UPDATE " & cnAdminDb & "..USERMASTER SET AUTHPWD = PWD WHERE USERID =" & userId
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        'DbTabledrop("TEMPTABLEDB")

        HideMenus()
        UserRightForms()
        cnUserName = cmbUserName.Text
        cnPassword = txtPassword.Text

        BrightPosting.GExport.UserId = userId
        BrightPosting.GExport.UserName = cnUserName

        Main.tStripExit.Visible = True
        Login()
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CHITDBPREFIX'"
        If (Trim(objGPack.GetSqlValue(strSql, , ""))) <> "" Then
            cnChitTrandb = Trim(objGPack.GetSqlValue(strSql, , "")) + "SH0708"
            cnChitCompanyid = Trim(objGPack.GetSqlValue(strSql, , ""))
        End If
        LogonServer()
    End Sub

    Private Sub lblLanguage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblLanguage.Click
        cmbLanguage.Visible = False
        ' cmbLanguage.Text = lblLanguage.Text
    End Sub

    Private Sub cmbLanguage_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLanguage.SelectedValueChanged
        'lblLanguage.Text = cmbLanguage.Text
        cmbLanguage.Visible = False
    End Sub

    Private Sub cmbUserName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbUserName.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbUserName.Text = "" Then
                cmbUserName.Focus()
            Else
                '' BELOW 3 LINES CHANGE BY KALAI SIR
                'Dim isactive As String
                'isactive = objGPack.GetSqlValue("SELECT ACTIVE FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbUserName.Text & "'", , "N")
                'If isactive = "N" Then MsgBox("Please Check user De-Activated", MsgBoxStyle.Critical) : cmbUserName.Focus() : Exit Sub

                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub cmbUserName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbUserName.KeyPress
        e.KeyChar = UCase(e.KeyChar)
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub cmbUserName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbUserName.KeyUp
        ' ComboScript(cmbUserName, e)
        'Dim sComboText As String = ""
        'Dim iLoop As Integer
        'Dim sTempString As String
        'If e.KeyCode >= 65 And e.KeyCode <= 90 Then
        '    'only look at letters A-Z
        '    sTempString = cmbUserName.Text
        '    If Len(sTempString) = 1 Then sComboText = sTempString
        '    For iLoop = 0 To (cmbUserName.Items.Count - 1)
        '        If UCase((sTempString & Mid$(cmbUserName.Items.Item(iLoop), _
        '          Len(sTempString) + 1))) = UCase(cmbUserName.Items.Item(iLoop)) Then
        '            cmbUserName.SelectedIndex = iLoop
        '            cmbUserName.Text = cmbUserName.Items.Item(iLoop)
        '            cmbUserName.SelectionStart = Len(sTempString)
        '            cmbUserName.SelectionLength = Len(cmbUserName.Text) - (Len(sTempString))
        '            sComboText = sComboText & Mid$(sTempString, Len(sComboText) + 1)
        '            Exit For
        '        Else
        '            If InStr(UCase(sTempString), UCase(sComboText)) Then
        '                sComboText = sComboText & Mid$(sTempString, Len(sComboText) _
        '                + 1)
        '                cmbUserName.Text = sComboText
        '                cmbUserName.SelectionStart = Len(cmbUserName.Text)
        '            Else
        '                sComboText = sTempString
        '            End If
        '        End If
        '    Next iLoop
        'End If
    End Sub

    Private Sub txtPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPassword.Text = "" Then
                txtPassword.Focus()
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub
    Function CheckPwdExpiry() As Boolean
        _ChangePwd = IIf(GetAdmindbSoftValue("USERPWDCHANGE").ToUpper.ToString = "Y", True, False)
        Dim Days As Integer
        Dim DaysDiffer As Integer
        If _ChangePwd Then
            Days = Val(objGPack.GetSqlValue("SELECT ISNULL(PWDCHANGE,0) FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbUserName.Text & "' AND PWD = '" & BrighttechPack.Methods.Encrypt(txtPassword.Text) & "'", , 0))
            If Days = 0 Then
                Return True
            Else
                DaysDiffer = Val(objGPack.GetSqlValue("SELECT DATEDIFF(DD,PWDUPDATED,'" & GetServerDate() & "') FROM " & cnAdminDb & "..USERMASTER ", , 0))
                If DaysDiffer >= Days Then
                    Return False
                Else
                    Return True
                End If
            End If
        End If
        Return True
    End Function
    Private Sub AddAccEntryMenus()
        ''Adding Accounts EntryMenus Based on AccEntryMaster
        strSql = " SELECT * FROM " & cnAdminDb & "..ACCENTRYMASTER ORDER BY DISPLAYORDER,CAPTION"
        Dim dtAccMenus As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAccMenus)

        For Each ro As DataRow In dtAccMenus.Rows
            Dim tStrip As New ToolStripMenuItem
            tStrip.Name = "TSTRIPACC" & ro!SNO.ToString
            tStrip.Text = ro!CAPTION.ToString
            tStrip.AccessibleDescription = ro!SNO.ToString & "frmAccountsEnt~OWN"
            tStrip.Size = New System.Drawing.Size(140, 22)
            AddHandler tStrip.Click, AddressOf AccountsEntry
            Main.tStripAccountsEnt.DropDownItems.Add(tStrip)
        Next
    End Sub

    Private Sub AccountsEntry(ByVal sender As Object, ByVal e As EventArgs)
        strSql = " SELECT TYPE FROM " & cnAdminDb & "..ACCENTRYMASTER "
        strSql += " WHERE SNO = '" & CType(sender, ToolStripMenuItem).Name.Replace("TSTRIPACC", "") & "'"
        Dim type As String = objGPack.GetSqlValue(strSql)

        strSql = " SELECT DISPLAYTEXT FROM " & cnAdminDb & "..ACCENTRYMASTER "
        strSql += " WHERE SNO = '" & CType(sender, ToolStripMenuItem).Name.Replace("TSTRIPACC", "") & "'"
        Dim dispText As String = objGPack.GetSqlValue(strSql)

        strSql = " SELECT PAYMODE FROM " & cnAdminDb & "..ACCENTRYMASTER "
        strSql += " WHERE SNO = '" & CType(sender, ToolStripMenuItem).Name.Replace("TSTRIPACC", "") & "'"
        Dim ctlId As String = "GEN-" & objGPack.GetSqlValue(strSql)
        strSql = " SELECT 'CHECK' FROM " & cnStockDb & "..BILLCONTROL"
        strSql += " WHERE CTLID = '" & ctlId & "'"
        strSql += " AND COMPANYID = '" & strCompanyId & "'"
        If Not objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox(dispText & " Billno Generation Controlid Not Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim Approval As String = IIf(objGPack.GetSqlValue("SELECT APPROVAL FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE SNO = '" & CType(sender, ToolStripMenuItem).Name.Replace("TSTRIPACC", "") & "'", , "N") = "Y", True, False)
        Dim mnuId As String = CType(sender, ToolStripMenuItem).Name.Replace("TSTRIPACC", "")
        Dim obj As frmAccountsEnt = Nothing
        If Approval Then
            ctlId = ctlId & "-APP"
        End If
        Select Case type
            Case "R"
                obj = New frmAccountsEnt(frmAccountsEnt.VoucherType.Receipt, dispText, ctlId, mnuId, Approval)
            Case "P"
                obj = New frmAccountsEnt(frmAccountsEnt.VoucherType.Payment, dispText, ctlId, mnuId, Approval)
            Case "J"
                obj = New frmAccountsEnt(frmAccountsEnt.VoucherType.Journal, dispText, ctlId, mnuId, Approval)
            Case "D"
                obj = New frmAccountsEnt(frmAccountsEnt.VoucherType.DebitNote, dispText, ctlId, mnuId, Approval)
            Case "C"
                obj = New frmAccountsEnt(frmAccountsEnt.VoucherType.CreditNote, dispText, ctlId, mnuId, Approval)
        End Select
        For Each ctrl As Control In Main.Controls
            If TypeOf ctrl Is LinkLabel Then
                ctrl.Visible = False
            End If
        Next
        Main.frmCount += 1
        objGPack.Validator_Object(obj)
        obj.KeyPreview = True
        obj.MdiParent = Main
        obj.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        obj.Dock = DockStyle.Fill
        obj.WindowState = FormWindowState.Maximized
        AddHandler obj.FormClosing, AddressOf obj_Closing
        obj.Show()
    End Sub
    Public Sub obj_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
        Main.tStripModuleId.Text = ""
        Main.frmCount -= 1
        If Main.frmCount <= 0 Then
            For Each ctrl As Control In Main.Controls
                If TypeOf ctrl Is LinkLabel Then
                    ctrl.Visible = True
                End If
            Next
        End If
    End Sub

    Private Sub AddOtherMasterEntryMenus()
        ''Adding Accounts EntryMenus Based on AccEntryMaster
        strSql = " SELECT * FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY MISCID"
        Dim dtAccMenus As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAccMenus)

        For Each ro As DataRow In dtAccMenus.Rows
            Dim tStrip As New ToolStripMenuItem
            tStrip.Name = "TSTRIPOTHMASTER" & ro!MISCID.ToString
            tStrip.Text = ro!MISCNAME.ToString
            tStrip.AccessibleDescription = ro!MISCID.ToString & "frmOtherMaster"
            tStrip.Size = New System.Drawing.Size(140, 22)
            AddHandler tStrip.Click, AddressOf OtherMasterEntry
            Main.tStripOtherMaster.DropDownItems.Add(tStrip)
        Next
    End Sub

    Private Sub OtherMasterEntry(ByVal sender As Object, ByVal e As EventArgs)
        strSql = " SELECT MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY "
        strSql += " WHERE MISCID = '" & CType(sender, ToolStripMenuItem).Name.Replace("TSTRIPOTHMASTER", "") & "'"
        Dim dispText As String = objGPack.GetSqlValue(strSql)

        Dim mnuId As String = CType(sender, ToolStripMenuItem).Name.Replace("TSTRIPOTHMASTER", "")
        Dim obj As frmOtherMaster = Nothing

        obj = New frmOtherMaster(mnuId, dispText)
        objGPack.Validator_Object(obj)
        obj.KeyPreview = True
        obj.MdiParent = Main
        obj.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedToolWindow
        obj.Dock = DockStyle.None
        obj.WindowState = FormWindowState.Normal
        AddHandler obj.FormClosing, AddressOf obj_Closing
        obj.Show()
    End Sub



    Private Sub lblChangePassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblChangePassword.Click
        If cmbUserName.Text = "" Then
            cmbUserName.Focus()
            Exit Sub
        End If
        Dim MUSERID As Integer = objGPack.GetSqlValue("SELECT userid FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbUserName.Text & "'", , Nothing)
        Dim pwd As String = objGPack.GetSqlValue("SELECT PWD FROM " & cnAdminDb & "..USERMASTER WHERE USERID = " & MUSERID & " AND PWD = '" & BrighttechPack.Methods.Encrypt(txtPassword.Text) & "'", , Nothing)
        If pwd = Nothing Then
            MsgBox("Invalid Password", MsgBoxStyle.Information)
            txtPassword.Focus()
            txtPassword.SelectAll()
            Exit Sub
        End If
        If BrighttechPack.Methods.Encrypt(txtPassword.Text) <> pwd Then
            MsgBox("Invalid Password", MsgBoxStyle.Information)
            txtPassword.Focus()
            txtPassword.SelectAll()
            Exit Sub
        End If
        If MUSERID <> 999 Then
            strSql = "select ISNULL(PWDACCESS,'Y') PWDACCESS FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLEID = (SELECT TOP 1 ROLEID FROM " & cnAdminDb & "..USERROLE WHERE USERID = " & MUSERID & ")"
            If objGPack.GetSqlValue(strSql) = "N" Then MsgBox("Access Denied") : Exit Sub
        End If
        Dim objNewPwd As New frmAdminNewPassword
        If objNewPwd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                Dim updated As Date = GetEntryDate(GetServerDate())
                tran = cn.BeginTransaction
                strSql = " UPDATE " & cnAdminDb & "..USERMASTER SET"
                strSql += " PWD = '" & BrighttechPack.Methods.Encrypt(objNewPwd.txtNewPwd.Text) & "'"
                strSql += ", AUTHPWD = '" & BrighttechPack.Methods.Encrypt(objNewPwd.txtAuthPassword.Text) & "'"
                If _ChangePwd Then
                    strSql += ",PWDUPDATED='" & updated & "' "
                End If
                strSql += " WHERE USERNAME = '" & cmbUserName.Text & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                Dim chitMaindb As String = GetAdmindbSoftValue("CHITDBPREFIX", , tran) + "SAVINGS"
                If objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & chitMaindb & "'", , , tran).Length > 0 Then
                    strSql = " UPDATE " & chitMaindb & "..USERMASTER SET"
                    strSql += " PWD = '" & BrighttechPack.Methods.Encrypt(objNewPwd.txtNewPwd.Text) & "'"
                    strSql += " WHERE USERNAME = '" & cmbUserName.Text & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If
                tran.Commit()
                tran = Nothing
                txtPassword.Clear()
                txtPassword.Focus()
            Catch ex As Exception
                If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                Exit Sub
            End Try
        End If
    End Sub

    Private Sub cmbTransactionYear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbTransactionYear.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            btnOk.Focus()
        End If
    End Sub

    Private Sub cmbLanguage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLanguage.SelectedIndexChanged

    End Sub
End Class