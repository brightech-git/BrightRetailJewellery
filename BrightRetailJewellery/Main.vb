Imports System.Data.OleDb
Imports System.Management
Imports System.IO

Public Class Main
    '040113 VASANTH,FOR SHORT CUT KEY
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String = Nothing
    Dim GlobalDate As String = "N"
    Dim GlobalDateweb As String = "N"
    Dim DateReset As String = ""
    Dim Rpt_Send_Time As String = ""
    Dim closealert As Boolean = False
    Dim RATEWARNTIME As String
    Dim Ratewarnupd As Boolean = False
    Public frmCount As Integer = 0
    Private WithEvents taskNotify As TaskBarNotifier


    Public Function funcStatusBar(ByVal uname As String, ByVal shKey As String) As Integer
        pnlUserName.Text = uname
        Return 0
    End Function

    Public Sub menuChoice(ByVal sender As System.Object, ByVal e As System.EventArgs)
        frmCount = 0
        Dim currLinkLabel As LinkLabel = sender
        Dim tempLinkLabel As String = currLinkLabel.Text.ToString
        Dim flagModuleKey As Boolean = True
        Dim row() As DataRow
        If flagModuleKey Then
            row = _DtUserRights.Select("MENUTEXT = '" & currLinkLabel.Text.ToString & "'")
        End If
        If Not row.Length > 0 Then
            MsgBox("Access Denied", MsgBoxStyle.Information)
            Exit Sub
        End If
        'row(0).Item("ACCESSID").ToString CONTRAINS
        '~RPT THEN REPORT
        '~DIA THEN DIALOG
        '~OWN THEN HAVE OWN CONSTRACTOR
        If row(0).Item("ACCESSID").ToString.Contains("OWN") Then
            MsgBox("Cannot Access", MsgBoxStyle.Information)
            Exit Sub
        End If
        '040113
        Dim DllName As String = Nothing
        If row(0).Item("REF_DLL").ToString <> "" Then
            DllName = row(0).Item("REF_DLL").ToString
        End If
        Dim sp() As String = row(0).Item("ACCESSID").ToString.Split("~")
        Dim accessId As String = sp(0)

        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is LinkLabel Then
                ctrl.Visible = False
            End If
        Next
        Dim f As Form
        '040113
        If DllName = Nothing Then
            f = GetForm(accessId)
        Else
            f = GetForm(accessId, "dll", DllName)
        End If
        AddHandler f.FormClosing, AddressOf obj_Closing
        tran = Nothing
        If row(0).Item("ACCESSID").ToString.Contains("~DIA") = False Then
            f.MdiParent = Me
        End If
        f.BackColor = frmBackColor
        f.BackgroundImage = bakImage
        f.BackgroundImageLayout = ImageLayout.Stretch
        f.StartPosition = FormStartPosition.CenterScreen
        If f.WindowState = FormWindowState.Maximized Then
            f.WindowState = FormWindowState.Maximized
            f.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            f.Dock = DockStyle.Fill
        End If
        f.MaximizeBox = False
        BrighttechPack.LanguageChange.Set_Language_Form(f, LangId)
        objGPack.Validator_Object(f)
        f.KeyPreview = True
        Me.TStripLblTitle.Text = f.Text
        'f.Size = New Size(1026, 662)
        f.BringToFront()
        frmCount += 1
        If row(0).Item("ACCESSID").ToString.Contains("~DIA") Then
            f.ShowDialog()
        Else
            f.Show()
        End If
    End Sub

    Public Sub menuChoice_Picture(ByVal sender As System.Object, ByVal e As System.EventArgs)

        frmCount = 0
        Dim currLinkLabelPic As LinkLabel = sender
        Dim tempLinkLabelPic As String = currLinkLabelPic.Name.ToString
        Dim flagModuleKey As Boolean = True
        Dim row() As DataRow
        If flagModuleKey Then
            row = _DtUserRights.Select("MENUTEXT = '" & currLinkLabelPic.Name.ToString & "'")
        End If
        If Not row.Length > 0 Then
            MsgBox("Access Denied", MsgBoxStyle.Information)
            Exit Sub
        End If
        'row(0).Item("ACCESSID").ToString CONTRAINS
        '~RPT THEN REPORT
        '~DIA THEN DIALOG
        '~OWN THEN HAVE OWN CONSTRACTOR
        If row(0).Item("ACCESSID").ToString.Contains("OWN") Then
            MsgBox("Cannot Access", MsgBoxStyle.Information)
            Exit Sub
        End If
        '040113
        Dim DllName As String = Nothing
        If row(0).Item("REF_DLL").ToString <> "" Then
            DllName = row(0).Item("REF_DLL").ToString
        End If
        Dim sp() As String = row(0).Item("ACCESSID").ToString.Split("~")
        Dim accessId As String = sp(0)

        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is LinkLabel Then
                ctrl.Visible = False
            End If
        Next
        Dim f As Form
        '040113
        If DllName = Nothing Then
            f = GetForm(accessId)
        Else
            f = GetForm(accessId, "dll", DllName)
        End If
        AddHandler f.FormClosing, AddressOf obj_Closing
        tran = Nothing
        If row(0).Item("ACCESSID").ToString.Contains("~DIA") = False Then
            f.MdiParent = Me
        End If
        f.BackColor = frmBackColor
        f.BackgroundImage = bakImage
        f.BackgroundImageLayout = ImageLayout.Stretch
        f.StartPosition = FormStartPosition.CenterScreen
        If f.WindowState = FormWindowState.Maximized Then
            f.WindowState = FormWindowState.Maximized
            f.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            f.Dock = DockStyle.Fill
        End If
        f.MaximizeBox = False
        BrighttechPack.LanguageChange.Set_Language_Form(f, LangId)
        objGPack.Validator_Object(f)
        f.KeyPreview = True
        Me.TStripLblTitle.Text = f.Text
        'f.Size = New Size(1026, 662)
        f.BringToFront()
        frmCount += 1
        If row(0).Item("ACCESSID").ToString.Contains("~DIA") Then
            f.ShowDialog()
        Else
            f.Show()
        End If
    End Sub

    Public Sub DesktopShortcut()
        Dim ref_dll As String
        Dim accessname As String
        Dim menutext As String
        Dim px As Integer = 15
        Dim x As Integer = 35
        Dim y As Integer = 100
        Dim dt_menu As New DataTable
        strSql = "SELECT REF_DLL, (SELECT ACCESSID FROM " & cnAdminDb & "..PRJMENUS WHERE MENUID = M.MENUID) ACCESSNAME,MENUTEXT"
        strSql += " ,M.MENUID FROM " & cnAdminDb & "..MENUMASTER AS M WHERE ISNULL(SHORTCUT,'N') = 'Y' "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt_menu)

        If dt_menu.Rows.Count < 40 Then
            For i As Integer = 0 To dt_menu.Rows.Count - 1
                Dim MenuId As String
                Dim a As New LinkLabel
                Dim ap As New LinkLabel
                If y >= 0 Then
                    a.Text = ""
                    MenuId = dt_menu.Rows(i).Item("MENUID").ToString
                    ref_dll = dt_menu.Rows(i).Item("REF_DLL").ToString
                    accessname = dt_menu.Rows(i).Item("ACCESSNAME").ToString
                    If accessname.Contains("~").ToString Then
                        Dim tempIndex As Integer = accessname.IndexOf("~")
                        accessname = accessname.Substring(0, tempIndex)
                    End If
                    Dim ro() As DataRow = _DtUserRights.Select("MENUID = '" & MenuId & "'", "MENUID")
                    If Not ro.Length > 0 Then Continue For
                    menutext = dt_menu.Rows(i).Item("MENUTEXT").ToString
                    '100 Check Vertical Order
                    If y > 550 Then ' Display 10 Records
                        y = 100
                        x += 200
                        px += 200
                    End If
                    ap.Location = New Point(px, y)
                    a.Location = New Point(x, y)

                    ap.MaximumSize = New Size(a.Width, 0)
                    ap.Height = a.PreferredHeight
                    ap.MaximumSize = New Size(0, 0)

                    a.MaximumSize = New Size(ap.Width, 0)
                    a.Height = ap.PreferredHeight
                    a.MaximumSize = New Size(0, 0)
                    'a.Size = New Size(150, 50)
                    ap.AutoSize = False
                    ap.BackColor = Color.White
                    ap.Cursor = Cursors.Hand

                    a.AutoSize = False
                    a.BackColor = Color.White
                    a.Font = New Font("Verdana", 8, FontStyle.Regular)
                    a.Cursor = Cursors.Hand

                    ap.Image = My.Resources.shortcutarrow
                    ap.ImageAlign = ContentAlignment.MiddleLeft
                    a.Text = menutext
                    a.TextAlign = ContentAlignment.MiddleLeft
                    'ap-> MenuText form Name
                    ap.Name = menutext
                    a.Name = accessname
                    AddHandler a.Click, AddressOf menuChoice
                    AddHandler ap.Click, AddressOf menuChoice_Picture
                    Me.Controls.Add(a)
                    Me.Controls.Add(ap)
                    'x += 150
                    y += 50
                End If
            Next
        End If
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is LinkLabel Then
                ctrl.Visible = True
            End If
        Next
    End Sub

    Public Sub obj_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
        Me.TStripLblTitle.Text = ""
        Me.tStripModuleId.Text = ""
        Global_Date_Rate()
        frmCount -= 1
        If frmCount <= 0 Then
            For Each ctrl As Control In Me.Controls
                If TypeOf ctrl Is LinkLabel Then
                    ctrl.Visible = True
                End If
            Next
        End If
    End Sub

    Private Sub formBackApperanceinTableView(ByVal bgcolor As String, ByVal _imageName As String)
        Dim c As Color
        Dim m As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(bgcolor, "A=(?<Alpha>\d+),\s*R=(?<Red>\d+),\s*G=(?<Green>\d+),\s*B=(?<Blue>\d+)")
        If m.Success Then
            Dim alpha As Integer = Integer.Parse(m.Groups("Alpha").Value)
            Dim red As Integer = Integer.Parse(m.Groups("Red").Value)
            Dim green As Integer = Integer.Parse(m.Groups("Green").Value)
            Dim blue As Integer = Integer.Parse(m.Groups("Blue").Value)
            c = Color.FromArgb(alpha, red, green, blue)
        Else
            Dim cName As String = bgcolor.Replace("Color [", "").Replace("]", "")
            c = System.Drawing.Color.FromName(bgcolor.Replace("Color [", "").Replace("]", ""))
        End If
        If _imageName = "FORMCOLOR" Then
            frmBackColor = c
        ElseIf _imageName = "TEXTBOXCOLOR" Then
            lostFocusColor = c
            BrighttechPack.GlobalVariables.G_FocusColor = c
        ElseIf _imageName = "COMBOBOXCOLOR" Then
            focusColor = c
            BrighttechPack.GlobalVariables.G_FocusColor = c
        ElseIf _imageName = "GRIDCOLOR" Then
            grdBackGroundColor = c
            BrighttechPack.GlobalVariables.G_GrdBackGroundColor = c
        ElseIf _imageName = "RADIOCOLOR" Then
            Radio_Check_LostFocusColor = c
            BrighttechPack.GlobalVariables.G_Radio_Check_LostFocusColor = c
        ElseIf _imageName = "BUTTONCOLOR" Then
            Button_LostFocusColor = c
            BrighttechPack.GlobalVariables.G_Button_LostFocusColor = c
        End If
    End Sub
    Public Function formBackApperanceinTable()
        Dim cnt As Integer = 0
        Dim Qry As String = ""
        Qry = "SELECT COUNT(*) CNT FROM " & cnAdminDb & "..STYLE WHERE LOGO = 'N' "
        cnt = Val(GetSqlValue(cn, Qry).ToString)
        If cnt > 0 Then
            Dim dtbackApperance As New DataTable
            Qry = " SELECT * FROM " & cnAdminDb & "..STYLE WHERE LOGO = 'N' "
            dtbackApperance = GetSqlTable(Qry, cn)
            If dtbackApperance.Rows.Count > 0 Then
                For i As Integer = 0 To dtbackApperance.Rows.Count - 1
                    formBackApperanceinTableView(dtbackApperance.Rows(i).Item("BGCOLOR").ToString, dtbackApperance.Rows(i).Item("IMGNAME").ToString)
                Next
            End If
        End If
    End Function
    Public Function funcShow(ByVal f As Form, Optional ByVal frmTitle As String = Nothing, Optional ByVal diaStyle As Boolean = False, Optional ByVal SetBackApperance As Boolean = True) As Integer
        Try
            If f.Name <> "frmSoftControl" And Not cnCostId.Length > 0 And objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'", , "N", tran).ToUpper = "Y" Then
                MsgBox("Default CostId should not empty. Please set COSTID in softcontrol", MsgBoxStyle.Information)
                Exit Function
            End If
            'strSql = " SELECT APPID FROM " & cnAdminDb & "..USERS WHERE APPID = " & _AppId & ""
            'If BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql, , "-1") = "-1" Then
            '    Application.Exit()
            '    Exit Function
            'End If
            If f.Name <> "frmBillInitialize" Then
                For Each ctrl As Control In Me.Controls
                    If TypeOf ctrl Is LinkLabel Then
                        ctrl.Visible = False
                    End If
                Next
                frmCount += 1
            End If
            AddHandler f.FormClosing, AddressOf obj_Closing
            tran = Nothing
            If SetBackApperance Then
                f.BackColor = frmBackColor
                f.BackgroundImage = bakImage
                f.BackgroundImageLayout = ImageLayout.Stretch
            End If
            f.StartPosition = FormStartPosition.CenterScreen
            f.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog
            f.MaximizeBox = False
            BrighttechPack.LanguageChange.Set_Language_Form(f, LangId)
            objGPack.Validator_Object(f)
            f.KeyPreview = True
            f.MaximizeBox = False
            Me.TStripLblTitle.Text = frmTitle
            If diaStyle = False Then
                f.MdiParent = Me
            End If
            If f.WindowState = FormWindowState.Maximized Then
                f.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                f.Dock = DockStyle.Fill
            End If
            formBackApperanceinTable()
            f.BringToFront()
            Try
                If diaStyle Then
                    f.ShowDialog()
                Else
                    f.Show()
                End If
            Catch ex As Exception
                MsgBox(ex.Message + ex.StackTrace)
            End Try
        Catch ex As Exception
            ERRORLOGCREATE(ex.Message)
        End Try
    End Function
    Private Sub EventHandlerSubmenu(ByVal Child As ToolStripMenuItem)
        For Each Child1 As ToolStripMenuItem In Child.DropDownItems
            If Child1.HasDropDownItems Then
                EventHandlerSubmenu(Child1)
            Else
                AddHandler Child1.Click, AddressOf SubMenu_MouseClick
                Child1.ToolTipText = Child1.ShortcutKeyDisplayString
            End If
        Next
    End Sub
    Private Sub EventHandlerSubmenu()
        For Each MainToolStrip As ToolStripMenuItem In MenuStrip1.Items
            If MainToolStrip.HasDropDownItems Then
                EventHandlerSubmenu(MainToolStrip)
            End If
        Next
    End Sub
    Private Sub SubMenu_MouseClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim ShortcutKey As String = ""
        If Not CType(sender, ToolStripMenuItem).ShortcutKeyDisplayString Is Nothing Then
            ShortcutKey = CType(sender, ToolStripMenuItem).ShortcutKeyDisplayString.ToString
        ElseIf Not CType(sender, ToolStripMenuItem).AccessibleDescription Is Nothing Then
            If CType(sender, ToolStripMenuItem).AccessibleDescription.EndsWith("~OWN") Or CType(sender, ToolStripMenuItem).AccessibleDescription.EndsWith("frmOtherMaster") Then
                ShortcutKey = GetSqlValue(cn, "SELECT MODULEID FROM " & cnAdminDb & "..PRJMENUS WHERE ACCESSID='" & CType(sender, ToolStripMenuItem).AccessibleDescription & "'").ToString
            End If
        End If
        tStripModuleId.Text = ShortcutKey
    End Sub
    Private Sub Main_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'GIRI_MESSAGE')>0"
            strSql += " BEGIN"
            strSql += " DELETE FROM GIRI_MESSAGE"
            strSql += " WHERE SYSTEMID = '" & systemId & "'"
            strSql += " END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            'strSql = " DELETE FROM " & cnAdminDb & "..USERS WHERE APPID = " & _AppId & ""
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()

            strSql = " DELETE FROM " & cnAdminDb & "..LOGINDETAIL WHERE SYSTEMIP = '" & IpAddress & "' AND USERID = " & userId & " AND LOGINSTATUS = 'R' "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'EXITWITHLOGOFF'", , "N") = "Y" Then
                Shell("Shutdown -l")
            End If
        Catch ex As Exception
            ERRORLOGCREATE(ex.Message)
        End Try

    End Sub
    Private Sub Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Dim strSql As String = Nothing
        'strSql = " DECLARE @QRY NVARCHAR(4000)"
        'strSql += " DECLARE @TNAME VARCHAR(50)"
        'strSql += " DECLARE CUR CURSOR"
        'strSql += " FOR SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME LIKE 'TEMP%'"
        'strSql += " OPEN CUR"
        'strSql += " WHILE 1=1"
        'strSql += " BEGIN"
        'strSql += " FETCH NEXT FROM CUR INTO @TNAME"
        'strSql += " SET NOCOUNT ON"
        'strSql += " 	IF @@FETCH_STATUS = -1 BREAK"
        'strSql += " SELECT @QRY = 'DROP TABLE ' + @TNAME"
        'strSql += " EXEC SP_EXECUTESQL @QRY"
        'strSql += " END"
        'strSql += " CLOSE CUR"
        'strSql += " DEALLOCATE CUR"
        'Dim cmd As OleDbCommand
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        strSql = " DELETE FROM " & cnAdminDb & "..LOGINDETAIL WHERE SYSTEMIP = '" & IpAddress & "' AND USERID = " & userId & " AND LOGINSTATUS = 'R' "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Application.Exit()
    End Sub
    Private Sub Main_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If AscW(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If

        If AscW(e.KeyChar) = 68 Then
            If Not CheckGoldRate() Then Exit Sub
            frmBillInitialize.BillType = frmBillInitialize.BillTypee.Retail
            funcShow(frmBillInitialize)
        End If

    End Sub
    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        Dim envId As String = Environ("NODE-ID")
        EventHandlerSubmenu()
        'If envId = "" Then
        '    MsgBox("SystemId doesnt Set. Please Contact System Administrator", MsgBoxStyle.Information)
        '    Application.Exit()
        '    Exit Sub
        'ElseIf envId.Length < 3 Then
        '    MsgBox("Incorrect SystemId.SystemId Should Contain minimum 3 characters. Please Contact System Administrator", MsgBoxStyle.Information)
        '    Application.Exit()
        '    Exit Sub
        'End If
        If envId <> "" Then
            If envId.Length > 3 Then
                systemId = Mid(envId, envId.Length - 2, 3)
            ElseIf envId.Length <= 3 Then
                systemId = envId
            End If
        End If
        If systemId Is Nothing Then systemId = ""
        BrighttechPack.G_SearchDialogColAutoFit = IIf(GetAdmindbSoftValue("SEARCHDIA", "Y") = "Y", True, False)

        'strSql = " IF NOT (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'GIRI_USERS')>0"
        'strSql += " CREATE TABLE GIRI_USERS(SYSTEMID VARCHAR(3))"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'strSql = " INSERT INTO GIRI_USERS(SYSTEMID) VALUES('" & systemId & "')"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'MsgBox(getcpumcid())
        funcStatusBar(cnUserName, cnShortCutKey)
        tStripBarStatus.Text = ""
        tStripModuleId.Text = ""
        TStripLblTitle.Text = ""
        pBar.Visible = False
        Me.BackgroundImage = bakImage
        Me.BackgroundImageLayout = ImageLayout.Stretch
        tStripExit.Visible = True
        HideHelpText()
        Dim dblist As String = cnAdminDb & "," & cnStockDb
        Dim dbpbt As New DataTable
        'dbpbt = Dbproperties(dblist)
        Dim pbtdbname As String = ""
        Dim dbpropertiesstr As String = ""
        For ii As Integer = 0 To dbpbt.Rows.Count - 1
            If pbtdbname <> dbpbt.Rows(ii).Item(0) Then pbtdbname = dbpbt.Rows(ii).Item(0) : dbpropertiesstr += pbtdbname
            If dbpbt.Rows(ii).Item(1).ToString.Contains("log") = True Then dbpropertiesstr += " LOG-"
            dbpropertiesstr += " " & dbpbt.Rows(ii).Item(3).ToString & "MB" & IIf(Val(dbpbt.Rows(ii).Item(4).ToString) <> 0, "(" & dbpbt.Rows(ii).Item(4).ToString & "%) ", "")
        Next
        tStripBarStatus.Text = dbpropertiesstr

        dblist = "TEMPTABLEDB"
        dbpbt = New DataTable
        dbpbt = Dbproperties(dblist)
        pbtdbname = ""
        dbpropertiesstr = ""
        If Not dbpbt Is Nothing Then
            For ii As Integer = 0 To dbpbt.Rows.Count - 1
                If pbtdbname <> dbpbt.Rows(ii).Item(0) Then pbtdbname = dbpbt.Rows(ii).Item(0) : dbpropertiesstr += pbtdbname
                If dbpbt.Rows(ii).Item(1).ToString.Contains("log") = True Then dbpropertiesstr += " LOG-"
                dbpropertiesstr += " " & dbpbt.Rows(ii).Item(3).ToString & "MB" & IIf(Val(dbpbt.Rows(ii).Item(4).ToString) <> 0, "(" & dbpbt.Rows(ii).Item(4).ToString & "%) ", "")
            Next
            If dbpbt.Rows.Count = 2 Then
                Dim MaxDbsize As Integer = GetAdmindbSoftValue("TEMPDB_MAXLIMIT", 512)
                If Val(dbpbt.Rows(0).Item(3).ToString) >= MaxDbsize Or Val(dbpbt.Rows(1).Item(3).ToString) >= MaxDbsize Then
                    funcDropTempDbTables()
                    funcShrink("TEMPTABLEDB")
                End If
            End If
        End If

        ''tStripHelp.Text = dbpropertiesstr
        'tStrip = dbpropertiesstr

        ''last date rate updation
        strSql = "  IF NOT (SELECT COUNT(*) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = '" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "')>0"
        strSql += "  BEGIN"
        strSql += "  INSERT INTO " & cnAdminDb & "..RATEMAST(METALID,RDATE,RATEGROUP,PURITY,SRATE,PRATE,USERID,UPDATED,UPTIME"
        If RATE_BRANCHWISE Then
            strSql += "  ,COSTID"
        End If
        strSql += "  )"
        strSql += "  SELECT METALID,'" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "',(SELECT ISNULL(MAX(RATEGROUP),0)+1 FROM " & cnAdminDb & "..RATEMAST) AS RATEGROUP,PURITY,SRATE,PRATE,USERID,UPDATED,UPTIME "
        If RATE_BRANCHWISE Then
            strSql += "  ,COSTID"
        End If
        strSql += "  FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = (sELECT MAX(RDATE) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE <= '" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "' AND  RDATE BETWEEN '" & cnTranFromDate.ToString("yyyy-MM-dd") & "' AND '" & cnTranToDate.ToString("yyyy-MM-dd") & "')"
        strSql += "  AND RATEGROUP = (sELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = (sELECT MAX(RDATE) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE <= '" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "' AND RDATE BETWEEN '" & cnTranFromDate.ToString("yyyy-MM-dd") & "' AND '" & cnTranToDate.ToString("yyyy-MM-dd") & "'))"
        If RATE_BRANCHWISE Then
            strSql += "  AND ISNULL(COSTID,'')<>''"
        End If
        strSql += "  END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Global_Date_Rate()



        reportGSTStyle.ForeColor = Color.White
        reportGSTStyle.BackColor = SystemColors.MenuHighlight
        reportGSTStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        reportGSTStyle.ForeColor = Color.White
        reportGSTStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        reportGSTHeadStyle.ForeColor = Color.Black
        reportGSTHeadStyle.BackColor = Color.SandyBrown
        reportGSTHeadStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        reportGSTHeadStyle.ForeColor = Color.Black
        reportGSTHeadStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        reportHeadStyle.ForeColor = Color.Blue
        reportHeadStyle.BackColor = Color.LightBlue
        reportHeadStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        reportSubTotalStyle.ForeColor = Color.Blue
        reportSubTotalStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        reportHeadStyle1.ForeColor = Color.Red
        reportHeadStyle1.BackColor = Color.Beige
        reportHeadStyle1.Font = New Font("VERDANA", 8, FontStyle.Bold)
        reportSubTotalStyle1.ForeColor = Color.Red
        reportSubTotalStyle1.Font = New Font("VERDANA", 8, FontStyle.Bold)

        reportHeadStyle2.ForeColor = Color.Green
        reportHeadStyle2.BackColor = Color.LightGreen
        reportHeadStyle2.Font = New Font("VERDANA", 8, FontStyle.Bold)
        reportSubTotalStyle2.ForeColor = Color.Green
        reportSubTotalStyle2.Font = New Font("VERDANA", 8, FontStyle.Bold)


        reportTotalStyle.BackColor = Color.LightGoldenrodYellow
        reportTotalStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        reportHigLightStyle.BackColor = Color.LightGreen

        'reportSubTotalStyle.BackColor = Color.LightGray

        reportColumnHeadStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        reportColumnHeadStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        Me.WindowState = FormWindowState.Maximized
        cnCentStock = IIf(GetAdmindbSoftValue("CENT-STOCK", "Y") = "Y", True, False)

        _DefaultPic = GetAdmindbSoftValue("PICPATH")
        If Not _DefaultPic.EndsWith("\") Then _DefaultPic += "\"

        AddHandler CompanySelectionToolStripMenuItem.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler Company0ToolStripMenuItem.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler Company1ToolStripMenuItem.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler Company2ToolStripMenuItem.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler Company3ToolStripMenuItem.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler Company4ToolStripMenuItem.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler Company5ToolStripMenuItem.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler Company6ToolStripMenuItem.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler Company7ToolStripMenuItem.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler Company8ToolStripMenuItem.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler Company9ToolStripMenuItem.Click, AddressOf CompanySelectionMenuItem_Click

        AddHandler ToolStripMenuItem1.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler ToolStripMenuItem2.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler ToolStripMenuItem3.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler ToolStripMenuItem4.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler ToolStripMenuItem5.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler ToolStripMenuItem6.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler ToolStripMenuItem7.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler ToolStripMenuItem8.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler ToolStripMenuItem9.Click, AddressOf CompanySelectionMenuItem_Click
        AddHandler ToolStripMenuItem10.Click, AddressOf CompanySelectionMenuItem_Click


        'GenAppId:
        _AppId = Math.Abs(New Random().Next(1, 10000))
        '        strSql = " SELECT APPID FROM " & cnAdminDb & "..USERS WHERE APPID = " & _AppId & ""
        '        If BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql, , "-1") <> "-1" Then
        '            GoTo GenAppId
        '        End If

        '        strSql = " INSERT INTO " & cnAdminDb & "..USERS"
        '        strSql += " (APPID,USERID,COMPNAME)VALUES"
        '        strSql += " ("
        '        strSql += " " & _AppId & ""
        '        strSql += " ," & userId & ""
        '        strSql += " ,'" & My.Computer.Name & "'"
        '        strSql += " )"
        '        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '        cmd.ExecuteNonQuery()

        If userId <> 999 Then
            If _IsAdmin = False Then tStripBkImage.Visible = False
        End If
        Dim img As Image = Nothing
        strSql = "SELECT COUNT(*) FROM " & cnAdminDb & "..MENUMASTER WHERE ISNULL(SHORTCUT,'N') = 'Y' "
        Dim dbDesktop As Integer = objGPack.GetSqlValue(strSql)

        If _Demo Then
            img = My.Resources.Trial
        Else
            img = My.Resources.Akashaya
            Dim dtImg As New DataTable
            Dim ImgMemoryStream As MemoryStream = New MemoryStream()
            strSql = "SELECT BKIMAGE FROM " & cnAdminDb & "..PRJCTRL WHERE CTLID = 2"
            da = New OleDbDataAdapter(strSql, cn)
            dtImg.Clear()
            da.Fill(dtImg)
            If dtImg.Rows.Count > 0 Then
                If (IsDBNull(dtImg.Rows(0).Item("BKIMAGE")) = True) Then
                    If dbDesktop > 0 Then
                        img = My.Resources.T1
                    Else
                        img = My.Resources.Akashaya
                    End If
                Else
                    Dim tempByte As Byte() = Nothing
                    tempByte = dtImg.Rows(0).Item("BKIMAGE")
                    ImgMemoryStream = New MemoryStream(tempByte)
                    img = Drawing.Image.FromStream(ImgMemoryStream)
                End If
            Else
                img = My.Resources.Akashaya
            End If
        End If

        If img IsNot Nothing Then
            For Each ctrl As Control In Me.Controls
                If TypeOf ctrl Is MdiClient Then
                    ctrl.BackgroundImage = img
                    'ctrl.BackgroundImage = Image.FromFile("c:\trial.jpg")
                    ctrl.BackgroundImageLayout = ImageLayout.Tile
                    Dim styles As ControlStyles = ControlStyles.OptimizedDoubleBuffer
                    Dim flags As Reflection.BindingFlags = Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic
                    Dim method As Reflection.MethodInfo = ctrl.GetType.GetMethod("SetStyle", flags)
                    Dim param As Object() = {styles, True}
                    method.Invoke(ctrl, param)
                    Exit For
                End If
            Next
        End If

        DesktopShortcut()
        RemainInter = GetAdmindbSoftValue("REMAINDERINTERVAL", "0")
        If RemainInter <> 0 Then
            funRemainder()
        End If

        MasterVariables()
        ReportVariables()
        funcGlobalDate()
        funcCheckOTPforUser()


    End Sub

    Public Sub funcDropTempDbTables()
        strSql = " DECLARE @QRY NVARCHAR(4000)"
        strSql += " DECLARE @TNAME VARCHAR(50)"
        strSql += " DECLARE CUR CURSOR"
        strSql += " FOR SELECT NAME FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' "
        strSql += " OPEN CUR"
        strSql += " WHILE 1=1"
        strSql += " BEGIN"
        strSql += " FETCH NEXT FROM CUR INTO @TNAME"
        strSql += " /*SET NOCOUNT ON*/"
        strSql += " 	IF @@FETCH_STATUS = -1 BREAK"
        strSql += " SELECT @QRY = 'DROP TABLE TEMPTABLEDB..[' + @TNAME+ ']'"
        strSql += " EXEC SP_EXECUTESQL @QRY"
        strSql += " END"
        strSql += " CLOSE CUR"
        strSql += " DEALLOCATE CUR"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub funcShrink(ByVal Dbname As String)
        Try
            strSql = " SELECT	SUBSTRING(CONVERT(VARCHAR,SERVERPROPERTY('PRODUCTVERSION')),1,CHARINDEX('.',CONVERT(VARCHAR,SERVERPROPERTY('PRODUCTVERSION')))-1) AS [VERSION]"
            Dim SqlVersion As String = GetSqlValue(cn, strSql)
            strSql = " SELECT NAME FROM " & Dbname & "..SYSFILES WHERE FILEID = 2"
            Dim LogFile As String = GetSqlValue(cn, strSql)
            If LogFile = "" Then
                Exit Sub
            End If
            Dim cnShrink As New OleDbConnection
            Dim passWord As String
            passWord = ConInfo.lDbPwd
            If passWord <> "" Then passWord = BrighttechPack.Methods.Decrypt(passWord)
            If ConInfo.lDbLoginType.ToUpper = "W" Then
                cnShrink = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & Dbname & ";Data Source=" & ConInfo.lServerName & "")
            Else
                cnShrink = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & Dbname & ";Data Source={0};User Id=" & IIf(ConInfo.lDbUserId <> "", ConInfo.lDbUserId, "SA") & ";password=" & passWord & ";", ConInfo.lServerName))
            End If
            cnShrink.Open()
            If SqlVersion = "8" Or SqlVersion = "9" Then
                strSql = " BACKUP LOG " & Dbname & " WITH TRUNCATE_ONLY"
                cmd = New OleDbCommand(strSql, cnShrink)
                cmd.ExecuteNonQuery()
                strSql = " DBCC SHRINKFILE(" & LogFile & ",2)"
                cmd = New OleDbCommand(strSql, cnShrink)
                cmd.ExecuteNonQuery()
            ElseIf SqlVersion = "10" Then
                strSql = "ALTER DATABASE " & Dbname & " SET RECOVERY SIMPLE WITH NO_WAIT"
                cmd = New OleDbCommand(strSql, cnShrink)
                cmd.ExecuteNonQuery()
                strSql = "DBCC SHRINKFILE(" & LogFile & ",2)"
                cmd = New OleDbCommand(strSql, cnShrink)
                cmd.ExecuteNonQuery()
                strSql = "ALTER DATABASE " & Dbname & " SET RECOVERY FULL WITH NO_WAIT"
                cmd = New OleDbCommand(strSql, cnShrink)
                cmd.ExecuteNonQuery()
            End If
            cnShrink.Close()
        Catch ex As Exception
            MsgBox("Auto Shrink " + Dbname + vbCrLf + ex.Message + vbCrLf + ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Sub
    Public Function GetCPUMcid() As String
        Dim cpuID As String = String.Empty
        Dim mc As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
        Dim moc As ManagementObjectCollection = mc.GetInstances()
        For Each mo As ManagementObject In moc
            If (cpuID = String.Empty And CBool(mo.Properties("IPEnabled").Value) = True) Then
                cpuID = mo.Properties("MacAddress").Value.ToString()
                MsgBox(cpuID)
            End If

        Next
        Return cpuID
    End Function

    Public Sub MasterVariables()
        If Not IO.File.Exists(Application.StartupPath & "\BrightMaster.dll") Then
            MsgBox("Master Reference File not Found", MsgBoxStyle.Critical)
        End If
        BrighttechMaster.GlobalVariables.CompanyState = CompanyState
        BrighttechMaster.GlobalVariables.CompanyStateId = CompanyStateId
        BrighttechMaster.GlobalVariables.GST = GST
        BrighttechMaster.GlobalVariables.GstDate = GstDate
        BrighttechMaster.GlobalVariables.cnAdminDb = cnAdminDb
        BrighttechMaster.GlobalVariables.cnStockDb = cnStockDb
        BrighttechMaster.GlobalVariables.cn = cn
        BrighttechMaster.GlobalVariables.cnChitTrandb = cnChitTrandb
        BrighttechMaster.GlobalVariables._DtUserRights = _DtUserRights
        BrighttechMaster.GlobalVariables.userId = userId
        BrighttechMaster.GlobalVariables.cnChitCompanyid = cnChitCompanyid
        BrighttechMaster.GlobalVariables.strCompanyName = strCompanyName
        BrighttechMaster.GlobalVariables.strCompanyId = strCompanyId
        BrighttechMaster.GlobalVariables.reportHeadStyle = reportHeadStyle
        BrighttechMaster.GlobalVariables.reportHeadStyle1 = reportHeadStyle1
        BrighttechMaster.GlobalVariables.reportHeadStyle2 = reportHeadStyle2
        BrighttechMaster.GlobalVariables.reportSubTotalStyle = reportSubTotalStyle
        BrighttechMaster.GlobalVariables.reportSubTotalStyle1 = reportSubTotalStyle1
        BrighttechMaster.GlobalVariables.reportSubTotalStyle2 = reportSubTotalStyle2
        BrighttechMaster.GlobalVariables.reportTotalStyle = reportTotalStyle
        BrighttechMaster.GlobalVariables.reportColumnHeadStyle = reportColumnHeadStyle
        BrighttechMaster.GlobalVariables.cnCostName = cnCostName
        BrighttechMaster.GlobalVariables.cnCompanyId = cnCompanyId
        BrighttechMaster.GlobalVariables.cnDataSource = cnDataSource
        BrighttechMaster.GlobalVariables._MainCompId = _MainCompId
        BrighttechMaster.GlobalVariables.cnTranFromDate = cnTranFromDate
        BrighttechMaster.GlobalVariables.cnTranToDate = cnTranToDate
        BrighttechMaster.GlobalVariables._HideBackOffice = _HideBackOffice
        BrighttechMaster.GlobalVariables._DtTransactionYear = _DtTransactionYear
        BrighttechMaster.GlobalVariables._DefaultPic = _DefaultPic
        BrighttechMaster.GlobalVariables.systemId = systemId

        BrighttechMaster.GlobalVariables.cnCostId = cnCostId
        BrighttechMaster.GlobalVariables.cnHOCostId = cnHOCostId
        BrighttechMaster.GlobalVariables.findStr = findStr
        BrighttechMaster.GlobalVariables.cnCompanyName = cnCompanyName
        BrighttechMaster.GlobalVariables.cnUserName = cnUserName
        BrighttechMaster.GlobalVariables.cnPassword = cnPassword
        BrighttechMaster.GlobalVariables.DiaRnd = DiaRnd
        BrighttechMaster.GlobalVariables.strUserCentrailsed = strUserCentrailsed
        BrighttechMaster.GlobalVariables.cnDefaultCostId = cnDefaultCostId
        BrighttechMaster.GlobalVariables._IsCostCentre = _IsCostCentre
        BrighttechMaster.GlobalVariables.RATE_BRANCHWISE = RATE_BRANCHWISE
        BrighttechMaster.GlobalVariables._SyncTo = _SyncTo
    End Sub

    Public Sub ReportVariables()
        If Not IO.File.Exists(Application.StartupPath & "\BrightReport.dll") Then
            MsgBox("REPORT REFERENCE FILE NOT FOUND", MsgBoxStyle.Critical)
        End If
        BrighttechREPORT.Globalvariables.CompanyState = CompanyState
        BrighttechREPORT.Globalvariables.CompanyStateId = CompanyStateId
        BrighttechREPORT.Globalvariables.GST = GST
        BrighttechREPORT.Globalvariables.GstDate = GstDate
        BrighttechREPORT.Globalvariables.cnAdminDb = cnAdminDb
        BrighttechREPORT.Globalvariables.cnStockDb = cnStockDb
        BrighttechREPORT.Globalvariables.cn = cn
        BrighttechREPORT.Globalvariables.cnChitTrandb = cnChitTrandb
        BrighttechREPORT.Globalvariables._DtUserRights = _DtUserRights
        BrighttechREPORT.Globalvariables.userId = userId
        BrighttechREPORT.Globalvariables.cnChitCompanyid = cnChitCompanyid
        BrighttechREPORT.Globalvariables.strCompanyName = strCompanyName
        BrighttechREPORT.Globalvariables.strCompanyId = strCompanyId
        BrighttechREPORT.Globalvariables.reportGSTStyle = reportGSTStyle
        BrighttechREPORT.Globalvariables.reportGSTHeadStyle = reportGSTHeadStyle
        BrighttechREPORT.Globalvariables.reportHeadStyle = reportHeadStyle
        BrighttechREPORT.Globalvariables.reportHeadStyle1 = reportHeadStyle1
        BrighttechREPORT.Globalvariables.reportHeadStyle2 = reportHeadStyle2
        BrighttechREPORT.Globalvariables.reportSubTotalStyle = reportSubTotalStyle
        BrighttechREPORT.Globalvariables.reportSubTotalStyle1 = reportSubTotalStyle1
        BrighttechREPORT.Globalvariables.reportSubTotalStyle2 = reportSubTotalStyle2
        BrighttechREPORT.Globalvariables.reportTotalStyle = reportTotalStyle
        BrighttechREPORT.Globalvariables.reportHigLightStyle = reportHigLightStyle
        BrighttechREPORT.Globalvariables.reportColumnHeadStyle = reportColumnHeadStyle
        BrighttechREPORT.Globalvariables.cnCostName = cnCostName
        BrighttechREPORT.Globalvariables.cnCompanyId = cnCompanyId
        BrighttechREPORT.Globalvariables.cnDataSource = cnDataSource
        BrighttechREPORT.Globalvariables._MainCompId = _MainCompId
        BrighttechREPORT.Globalvariables.cnTranFromDate = cnTranFromDate
        BrighttechREPORT.Globalvariables.cnTranToDate = cnTranToDate
        BrighttechREPORT.Globalvariables._HideBackOffice = _HideBackOffice
        BrighttechREPORT.Globalvariables._DtTransactionYear = _DtTransactionYear
        BrighttechREPORT.Globalvariables._DefaultPic = _DefaultPic
        BrighttechREPORT.Globalvariables.systemId = systemId

        BrighttechREPORT.Globalvariables.cnCostId = cnCostId
        BrighttechREPORT.Globalvariables.cnHOCostId = cnHOCostId
        BrighttechREPORT.Globalvariables.findStr = findStr
        BrighttechREPORT.Globalvariables.cnCompanyName = cnCompanyName
        BrighttechREPORT.Globalvariables.cnUserName = cnUserName
        BrighttechREPORT.Globalvariables.cnPassword = cnPassword
        BrighttechREPORT.Globalvariables.DiaRnd = DiaRnd
        BrighttechREPORT.Globalvariables.strUserCentrailsed = strUserCentrailsed
        BrighttechREPORT.Globalvariables.cnDefaultCostId = cnDefaultCostId
        BrighttechREPORT.Globalvariables._IsCostCentre = _IsCostCentre
        BrighttechREPORT.Globalvariables.RATE_BRANCHWISE = RATE_BRANCHWISE
        BrighttechREPORT.Globalvariables._SyncTo = _SyncTo
    End Sub

    Public Sub Global_Date_Rate()
        tStripBillDate.Text = GetEntryDate(GetServerDate).ToString("dd/MM/yyyy")
        Dim Rdate As Date = GetEntryDate(GetServerDate)
        Dim mrategroup As Integer
        strSql = "SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST  WHERE RDATE = '" & Rdate.ToString("yyyy-MM-dd") & "'"
        If RATE_BRANCHWISE Then
            strSql += " AND COSTID='" & cnCostId & "'"
        End If
        mrategroup = Val(objGPack.GetSqlValue(strSql).ToString)
        tStripBillDate.Tag = mrategroup

        Dim mgold100rate As String
        Dim mgold75rates As String = ""
        Dim mGold75rate As Decimal = 0
        If GetAdmindbSoftValue("ISDISPLAY_24K_RATE", "N") = "Y" Then
            mgold100rate = "24K-" & Format(Val(GetRate(GetEntryDate(GetServerDate), objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PURITYID IN (SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY=100 AND METALID = 'G' AND METALTYPE = 'M' ORDER BY PURITY DESC)"))), "0.00") & ", 22K-"
            mGold75rate = GetRate_PurityPer(GetEntryDate(GetServerDate), "G", "75")
            If mGold75rate <> 0 Then mgold75rates = ",18K-" & Format(mGold75rate, "0.00")
        End If
        If GetAdmindbSoftValue("GOLD_RATE_PURITY", "") <> "" Then
            tStripGoldRate.Text = mgold100rate & Format(Val(GetRate(GetEntryDate(GetServerDate), objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PURITYID IN (SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN " & GetAdmindbSoftValue("GOLD_RATE_PURITY", "") & "  AND METALID = 'G' AND METALTYPE = 'O' ORDER BY PURITY DESC)"))), "0.00") & mgold75rates
        Else
            tStripGoldRate.Text = mgold100rate & Format(Val(GetRate(GetEntryDate(GetServerDate), objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PURITYID IN (SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN 91.6 AND 92 AND METALID = 'G' AND METALTYPE = 'O' ORDER BY PURITY DESC)"))), "0.00") & mgold75rates
        End If

        tStripSilverRate.Text = Format(Val(GetRate(GetEntryDate(GetServerDate), objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PURITYID IN (SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN 91.6 AND 92 AND METALID = 'S' AND METALTYPE = 'O' ORDER BY PURITY DESC)"))), "0.00")
        Me.Refresh()
    End Sub
    Private Function GetRate_PurityPer(ByVal Ddate As Date, ByVal metalid As String, ByVal purityper As Decimal) As Double
        strSql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST R"
        strSql += " WHERE RDATE = '" & Ddate.ToString("yyyy-MM-dd") & "'"
        strSql += " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST  WHERE RDATE = R.RDATE)"
        strSql += " AND PURITY =" & purityper & " AND METALID ='" & metalid & "'"
        If RATE_BRANCHWISE Then
            strSql += " AND COSTID='" & cnCostId & "'"
        End If
        Return Val(objGPack.GetSqlValue(strSql).ToString)
    End Function
    Private Sub tStripAccGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripAccGroup.Click
        Dim obj As New BrighttechMaster.frmAccountsGroup
        funcShow(obj, "ACCOUNT GROUP")
    End Sub

    Private Sub tStripAccHead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripAccHead.Click
        funcShow(frmAccountHead, "ACCOUNT HEAD")
    End Sub

    Private Sub tStripCostCentre_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCostCentre.Click
        If CENTR_DB_BR Then
            Dim objSecret As New frmGiriPwd()
            If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If
        End If
        Dim obj As New BrighttechMaster.frmCostCentre
        funcShow(obj, "COSTCENTRE")
    End Sub

    Private Sub tStripDesigner_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripDesigner.Click
        Dim obj As New BrighttechMaster.frmDisigner
        funcShow(obj, "DESIGNER")
    End Sub

    Private Sub tStripCreditCard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCreditCard.Click
        Dim obj As New BrighttechMaster.frmCreditCard
        funcShow(obj, "CREDITCARD")
    End Sub

    Private Sub tStripProcessType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripProcessType.Click
        Dim obj As New BrighttechMaster.frmProcessType
        funcShow(obj, "PROCESSTYPE")
    End Sub

    Private Sub tStripNarration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripNarration.Click
        Dim obj As New BrighttechMaster.frmNarration
        funcShow(obj, "NARRATION")
    End Sub

    Private Sub tStripTax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripTax.Click
        Dim obj As New BrighttechMaster.frmTax
        funcShow(obj, "TAX")
    End Sub

    Private Sub tStripMetal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripMetal.Click
        Dim obj As New BrighttechMaster.frmMetal
        funcShow(obj, "METAL")
    End Sub

    Private Sub tStripPurity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripPurity.Click
        Dim obj As New BrighttechMaster.frmPurityMaster
        funcShow(obj, "PURITY")
    End Sub

    Private Sub tStripCategory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCategory.Click
        If GST Then
            Dim obj As New BrighttechMaster.frmCategoryGST
            funcShow(obj, "CATEGORY")
        Else
            Dim obj As New BrighttechMaster.frmCategory
            funcShow(obj, "CATEGORY")
        End If
    End Sub

    Private Sub tStripSubItemGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSubItemGroup.Click
        Dim obj As New BrighttechMaster.frmSubItemGroup
        funcShow(obj, "SUBITEM GROUP")
    End Sub

    Private Sub tStripItemMaster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripItemMaster.Click
        Dim obj As New BrighttechMaster.frmItemMaster
        funcShow(obj, "ITEM MASTER")
    End Sub

    Private Sub tStripSubItemMaster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSubItemMaster.Click
        Dim obj As New BrighttechMaster.frmSubItemMaster
        funcShow(obj, "SUBITEM MASTER")
    End Sub

    Private Sub tStripItemCounter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripItemCounter.Click
        Dim obj As New BrighttechMaster.frmItemCounter
        funcShow(obj, "ITEM COUNTER")
    End Sub

    Private Sub tStripItemSize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripItemSize.Click
        Dim obj As New BrighttechMaster.frmCounterSize
        funcShow(obj, "ITEM COUNTER SIZE")
    End Sub

    Private Sub tStripValueAdded_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripValueAdded.Click
        Dim obj As New BrighttechMaster.frmValueAddedNew
        funcShow(obj, "VALUE ADDED")
    End Sub

    Private Sub tStripOtherCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripOtherCharge.Click

    End Sub

    Private Sub tStripStockReorder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripStockReorder.Click

    End Sub

    Private Sub tStripCashCounter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCashCounter.Click
        Dim obj As New BrighttechMaster.frmCashCounter
        funcShow(obj, "CASH COUNTER")
    End Sub

    Private Sub tStripSoftControl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSoftControl.Click
        Dim obj As New BrighttechMaster.frmSoftControl
        funcShow(obj, "SOFTCONTROL")
    End Sub
    Private Sub tStripEmployeeMaster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripEmployeeMaster.Click
        Dim obj As New BrighttechMaster.frmEmpMaster
        funcShow(obj, "EMPLOYEE MASTER")
    End Sub

    Private Sub tStripDiscAuthorize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripDiscAuthorize.Click
        funcShow(frmDiscAuthorize, "DISCOUNT AUTHORIZE")
    End Sub

    Private Sub tStripDesignation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripDesignation.Click
        Dim obj As New BrighttechMaster.frmDesignation
        funcShow(obj, "DESIGNATION")
    End Sub

    Private Sub tStripRateUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripRateMaster.Click
        funcShow(frmRateMaster, "RATE MASTER")
    End Sub

    Private Function CheckGoldRate() As Boolean
        If Val(tStripGoldRate.Text) = 0 Then
            MsgBox("Please enter valid Gold Rate", MsgBoxStyle.Information)
            Return False
        End If
        If Val(tStripSilverRate.Text) = 0 Then
            MsgBox("Please enter valid Silver Rate", MsgBoxStyle.Information)
            Return False
        End If
        Return True
    End Function

    Private Sub tStripItemTag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripItemTag.Click
        If Not CheckGoldRate() Then Exit Sub
        funcShow(frmItemTag, "ITEM TAG")
    End Sub

    Private Sub tStripIssue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripIssue.Click
        funcShow(frmNonTagIssue, "ITEM NONTAG ISSUE")
    End Sub

    Private Sub tStripReceipt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripReceipt.Click
        If Not CheckGoldRate() Then Exit Sub
        funcShow(frmNonTagReceipt, "ITEM NONTAG RECEIPT")
    End Sub

    Private Sub tStripViewIssueReceipt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripViewIssueReceipt.Click
        funcShow(frmNonTagIssueRecieptView, "NONTAG ISSUE/RECEIPT VIEW")
    End Sub

    Private Sub tStripItemBulkTag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripItemBulkTag.Click
        If Not CheckGoldRate() Then Exit Sub
        funcShow(frmitemBulkTag, "BULK TAG")
    End Sub

    'Private Sub tStripReqTagDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripReqTagDetail.Click
    '    funcShow(frmReqItem, "REQUIREMENT ITEM")
    'End Sub

    Private Sub tStripTagCatalog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripTagCatalog.Click
        funcShow(frmTagImage, "TAG IMAGE VIEWER")
    End Sub

    Private Sub tStripBillwiseRecPay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripBillwiseRecPay.Click
        Dim obj As New BrighttechREPORT.frmBillWiseReceiptPayment()
        funcShow(obj, "BILLWISE RECEIPT AND PAYMENT")

        'funcShow(frmBillWiseReceiptPayment, "BILLWISE RECEIPT AND PAYMENT")

    End Sub

    Private Sub tStripLedgerView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripLedgerView.Click
        funcShow(frmAccountsLedger, "ACCOUNTS LEDGER")
    End Sub

    Private Sub tStripItemWiseDialyRpt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripItemWiseDialyRpt.Click
        funcShow(frmItemWiseSales, "ITEM WISE DAILY REPORT")

    End Sub

    Private Sub tStripTranSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripTranSummary.Click
        Dim obj As New BrighttechREPORT.TransactionSummary()
        funcShow(obj, "TRANSACTION SUMMARY")
        'funcShow(TransactionSummary, "TRANSACTION SUMMARY")
    End Sub

    Private Sub tStripTranDetailed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripTranDetailed.Click
        Dim obj As New BrighttechREPORT.frmTransactionDetailed()
        funcShow(obj, "TRANSACTION DETAILED")
        'funcShow(frmTransactionDetailed, "TRANSACTION DETAILED")
    End Sub

    Private Sub tStripCatStock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCatStock.Click
        If GetAdmindbSoftValue("RPT_CATEGORYSTK_FORMAT", "1") = "1" Then
            Dim obj As New BrighttechREPORT.frmCategoryStock()
            funcShow(obj, "CATEGORY STOCK")
        Else
            Dim obj As New BrighttechREPORT.frmCategoryStockNew()
            funcShow(obj, "CATEGORY STOCK")
        End If
    End Sub

    Private Sub tStripOrderReceipt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripOrderReceipt.Click
        Dim obj As New BrighttechREPORT.frmOrderReceipt()
        funcShow(obj, "ORDER/REPAIR RECEIPT")
        'funcShow(frmOrderReceipt, "ORDER RECEIPT")
    End Sub

    Private Sub tStripBillWiseCollectionDet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripBillWiseCollectionDet.Click
        funcShow(frmBillWiseCollectionDetails, "BILLWISE COLLECTION DETAIL")
    End Sub

    Private Sub tStripAccEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'funcShow(frmAccountsEntry, "ACCOUNTS ENTRY")
    End Sub

    'Private Sub tStripOrnamentMetalStoneRec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If Not CheckGoldRate() Then Exit Sub
    '    funcShow(frmSmithReceipt)
    '    'funcShow(frmMetalOrnamentStoneReceipt, "METAL ORNAMENT STONE RECEIPT")
    'End Sub

    'Private Sub tStripOrnamentMetalStoneIss_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If Not CheckGoldRate() Then Exit Sub
    '    'Dim OBJ As New MaterialIssRecTran(MaterialIssRecTran.MaterialType.Issue)
    '    'funcShow(OBJ)
    '    funcShow(frmSmithIssue)
    '    'funcShow(frmMetalOrnamentStoneIssue, "METAL ORNAMENT STONE ISSUE")
    'End Sub

    Private Sub tStripTrailBalOpening_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripTrailBalOpening.Click
        'funcShow(frmOpeningTrailBal, "OPENING TRAIL BALANCE")
        funcShow(frmOpeningTrailBalance, "OPENING TRAIL BALANCE")
    End Sub

    Private Sub tStripWeightEntOpening_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripWeightEntOpening.Click
        If Not CheckGoldRate() Then Exit Sub
        funcShow(frmOpeningWeightEntry, "OPENING WEIGHT ENTRY")
    End Sub

    Private Sub tStripRateCutEnty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'funcShow(frmRateCutEntry, "RATE CUT ENTRY")
    End Sub

    Private Sub tStripChequeBookEnty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripChequeBookEnty.Click
        funcShow(frmChequeBookEntry, "CHEQUE BOOK ENTRY")
    End Sub

    Private Sub tStripPendingTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripPendingTransfer.Click
        If GetAdmindbSoftValue("PENDTRF2NONTAG", "Y") = "Y" Then
            funcShow(frmReprocess, "PENDING TRANSFER")
        ElseIf GetAdmindbSoftValue("PENDTRF2NONTAG", "Y") = "P" Then
            funcShow(frmPendingTransfer, "PENDING TRANSFER")
        ElseIf GetAdmindbSoftValue("PENDTRF2NONTAG", "Y") = "B" Then
            If cnCostId = cnHOCostId Then
                If GetAdmindbSoftValue("HO_AS_SALES_COSTCENTRE", "Y") = "Y" Then
                    funcShow(frmPendingTransfer, "PENDING TRANSFER")
                Else
                    funcShow(frmPendingTrsCorp, "PENDING TRANSFER")
                End If
            Else
                funcShow(frmPend2Corp, "PENDING TRANSFER")
            End If
        Else
            funcShow(frmPend2Inttrf, "PENDING TRANSFER")
        End If
    End Sub

    Private Sub tStripOrderEnty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripOrderEnty.Click
        'funcShow(frmOrder)
        If Not CheckGoldRate() Then Exit Sub
        If GetAdmindbSoftValue("ORD_" & strCompanyId, "N") = "Y" Then Exit Sub
        frmOrderInitialize.isRepair = False
        frmOrderInitialize.isEstimateOrder = False
        frmOrderInitialize.Text = "Order Information"
        funcShow(frmOrderInitialize)
    End Sub

    Private Sub tStripTranfer2IssRec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripTranfer2IssRec.Click
        funcShow(frmOrderIssueReceipt, "ISSUE & RECEIPT TRANSFER")
    End Sub

    Private Sub tStripCatTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCatTransfer.Click
        funcShow(frmCategoryTransfer, "CATEGORY TRANSFER", True)
    End Sub

    Private Sub tStripEstimationEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripEstimationEntry.Click
        Dim CostId As String = Nothing
        Dim CashId As String = Nothing

        Dim objEst As New frmEstimation1
        objEst.Hide()
        objEst.BillDate = GetEntryDate(GetServerDate)
        objEst.lblUserName.Text = cnUserName
        objEst.lblSystemId.Text = systemId
        objEst.lblBillDate.Text = GetEntryDate(GetServerDate).ToString("dd/MM/yyyy")
        objEst.Set916Rate(objEst.BillDate)
        objEst.WindowState = FormWindowState.Minimized
        BrighttechPack.LanguageChange.Set_Language_Form(objEst, LangId)
        objGPack.Validator_Object(objEst)

        objEst.Size = New Size(1032, 745)
        objEst.MaximumSize = New Size(1032, 745)
        objEst.StartPosition = FormStartPosition.Manual
        objEst.Location = New Point((ScreenWid - objEst.Width) / 2, ((ScreenHit - 25) - objEst.Height) / 2)

        objEst.KeyPreview = True
        objEst.MaximizeBox = False
        objEst.StartPosition = FormStartPosition.CenterScreen
        'objEst.Location = New Point(-3, -3)
        objEst.Show()
        objEst.WindowState = FormWindowState.Normal
    End Sub
    Private Sub EstimatToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripEstimationRevise.Click
        Dim CostId As String = Nothing
        Dim CashId As String = Nothing

        Dim objEst As New frmEstimation_2
        objEst.Hide()
        objEst.BillDate = GetEntryDate(GetServerDate)
        objEst.lblUserName.Text = cnUserName
        objEst.lblSystemId.Text = systemId
        objEst.lblBillDate.Text = GetEntryDate(GetServerDate).ToString("dd/MM/yyyy")
        objEst.Set916Rate(objEst.BillDate)
        objEst.WindowState = FormWindowState.Minimized
        BrighttechPack.LanguageChange.Set_Language_Form(objEst, LangId)
        objGPack.Validator_Object(objEst)

        objEst.Size = New Size(1032, 745)
        objEst.MaximumSize = New Size(1032, 745)
        objEst.StartPosition = FormStartPosition.Manual
        objEst.Location = New Point((ScreenWid - objEst.Width) / 2, ((ScreenHit - 25) - objEst.Height) / 2)

        objEst.KeyPreview = True
        objEst.MaximizeBox = False
        objEst.StartPosition = FormStartPosition.CenterScreen
        'objEst.Location = New Point(-3, -3)
        objEst.Show()
        objEst.WindowState = FormWindowState.Normal
    End Sub

    Private Sub tStripLot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripLot.Click
        funcShow(frmLotEntry, "LOT ENTRY")
    End Sub


    Private Sub tStripItemWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripItemWise.Click

        Dim obj As New BrighttechREPORT.frmItemWiseStock()
        funcShow(obj, "ITEMWISE STOCK")
        'funcShow(frmItemWiseStock, "ITEMWISE STOCK")
    End Sub

    Private Sub tStripManualStockCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripManualStockCheck.Click
        funcShow(frmStockCheckManual, "MANUAL STOCK CHECKING")
    End Sub

    Private Sub tStripAutomaticStockCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripAutomaticStockCheck.Click
        If GetAdmindbSoftValue("RPT_STOCKCHECK_RFID", "N") = "Y" Then
            Dim obj As New BrighttechREPORT.frmStockCheckWithRFID()
            funcShow(obj, "AUTOMATIC STOCK CHECKING")
        Else
            Dim obj As New BrighttechREPORT.frmStockCheckLoading()
            funcShow(obj, "AUTOMATIC STOCK CHECKING")
        End If

        'funcShow(frmStockCheckLoading, "AUTOMATIC STOCK CHECKING")
    End Sub

    Private Sub tStripAgeWiseAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripAgeWiseAnalysis.Click
        If GetAdmindbSoftValue("REP_NEWSTOCKAGE", "N") = "Y" Then
            Dim obj As New BrighttechREPORT.frmNewAgeWiseAnalysis()
            funcShow(obj, "AGEWISE ANALYSIS")
        Else
            Dim obj As New frmAgeWiseAnalysis()
            funcShow(obj, "AGEWISE ANALYSIS")
        End If
    End Sub

    Private Sub tStripLanguageChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripLanguageChange.Click
        Dim f As Form = Me.ActiveMdiChild
        If f Is Nothing Then
            Exit Sub
        End If
        Dim obj As New BrighttechPack.frmLanguageSetDialg(f)
        obj.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog
        obj.MaximizeBox = False
        obj.ShowDialog()
    End Sub

    Private Sub tStripLogOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripLogOut.Click
        Application.Restart()
    End Sub

    Private Sub tStripCalculator_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCalculator.Click
        Shell("CALC.EXE")
    End Sub

    Private Sub tStripStockView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripStockView.Click
        funcShow(frmTagedItemsStockView, "TAGED ITEMS STOCK VIEW")
    End Sub

    Private Sub tStripReceiptView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripReceiptView.Click
        funcShow(frmTagedItemsStockViewReceiptView, "TAGED ITEMS RECEIPT VIEW")
    End Sub

    Private Sub tStripView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripView.Click
        funcShow(frmTagedItemsStockViewIssueView, "TAGED ITEMS ISSUE VIEW")
    End Sub


    Private Sub tStripApprovalIssRec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripApprovalIssRec.Click
        Dim obj As New BrighttechREPORT.frmApprovalIssRecPen()
        funcShow(obj, "APPROVAL ISSUE/RECEIPT")
        'funcShow(frmApprovalIssRecPen, "APPROVAL ISSUE/RECEIPT")
    End Sub


    Private Sub tStripAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim objAttach As New BrighttechPack.DB_Attach
        objAttach.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog
        objAttach.MaximizeBox = False
        objAttach.ShowDialog()
    End Sub

    Private Sub tStripDetach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim objDetach As New BrighttechPack.DB_Detach
        objDetach.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog
        objDetach.MaximizeBox = False
        objDetach.ShowDialog()
    End Sub

    Private Sub tStripBackUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripBackUp.Click
        Dim db_BackupPath As String = UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DB_BACKUP_PATH'"))
        Dim db_FileType As String = UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DB_FILETYPE'"))
        Dim db_BackupType As String = UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DB_BACKUP_TYPE'", , "M"))
        Dim dd As Date = CType(objGPack.GetSqlValue("SELECT GETDATE()"), Date)
        Dim fileConcatStr As String = Nothing
        If db_FileType = "D" Then
            fileConcatStr = "_" + Format(dd.Date.Day, "00") + Format(dd.Date.Month, "00") + Mid(dd.Date.Year, 3, 2)
        Else
            fileConcatStr = "_" + Format(dd.Date.Day, "00") + Format(dd.Date.Month, "00") + Mid(dd.Date.Year, 3, 2) _
            + "_" + Format(dd.TimeOfDay.Hours, "00") + Format(dd.TimeOfDay.Minutes, "00") + Format(dd.TimeOfDay.Seconds, "00")
        End If
        Dim objBAckup As New BrighttechPack.DB_Bakup(db_BackupPath, fileConcatStr, db_BackupType)
        objBAckup.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog
        objBAckup.MaximizeBox = False
        If objBAckup.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If UCase(objBAckup.txtBackUpPath.Text) <> db_BackupPath Then
                If MessageBox.Show("Would you like to set you DefaultPath to " + objBAckup.txtBackUpPath.Text, "Update Alert?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                    Dim str As String
                    str = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & objBAckup.txtBackUpPath.Text & "' WHERE CTLID = 'DB_BACKUP_PATH'"
                    cmd = New OleDbCommand(str, cn)
                    cmd.ExecuteNonQuery()
                End If
            End If
        End If
    End Sub

    Private Sub tStripExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripExit.Click
        If MessageBox.Show("Do you want to close", "Close Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
            closealert = True
            Me.Close()
        End If
    End Sub

    Public Sub ShowHelpText(ByVal helpText As String)
        tStripHelp.AutoSize = True
        tStripHelp.Text = helpText
    End Sub
    Public Sub HideHelpText()
        tStripHelp.Text = ""
    End Sub

    Private Sub tStripBillControl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripBillControl.Click
        funcShow(frmBillControl, "BILL CONTROL", False)
    End Sub

    Private Sub tStripSalesEstimationRpt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSalesEstimationRpt.Click
        funcShow(frmEstSales)
    End Sub

    Private Sub tStripPurchaseEstimationRpt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripPurchaseEstimationRpt.Click
        funcShow(frmEstPurchase)
    End Sub

    Private Sub tStripCardDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCardDetails.Click
        funcShow(frmCardEst)
    End Sub

    Private Sub tStripGuaranteeCardPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripGuaranteeCardPrint.Click
        funcShow(frmCardDetails)
    End Sub

    Private Sub tStripDailyAbstract_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripDailyAbstract.Click
        'Dim obj As New BrighttechREPORT.frmDailyAbstract()
        'funcShow(obj, "DAILY ABSTRACT")
        ''funcShow(frmDailyAbstract, "DAILY ABSTRACT")
        If GetAdmindbSoftValue("DAILYABS_FORMAT", "N") = "F1" Then
            Dim obj As New BrighttechREPORT.frmDailyAbstractTT()
            funcShow(obj, "DAILY ABSTRACT")
        Else
            Dim obj As New BrighttechREPORT.frmDailyAbstract()
            funcShow(obj, "DAILY ABSTRACT")
        End If
    End Sub

    Private Sub tStripSalesAbstract_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSalesAbstract.Click
        If GetAdmindbSoftValue("SALESABS_FORMAT", "N") = "F1" Then
            Dim obj As New BrighttechREPORT.frmSalesAbsNew()
            funcShow(obj, "SALES ABSTRACT")
        Else
            Dim obj As New BrighttechREPORT.frmSalesAbstract()
            funcShow(obj, "SALES ABSTRACT")
            'funcShow(frmSalesAbstract, "SALES ABSTRACT")
        End If

    End Sub

    Private Sub tStripPurchaseAbstract_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripPurchaseAbstract.Click
        Dim obj As New BrighttechREPORT.frmPurchaseAbstract()
        funcShow(obj, "PURCHASE ABSTRACT")
        'funcShow(frmPurchaseAbstract, "PURCHASE ABSTRACT")
    End Sub

    Private Sub tStripSalesReturnAbstract_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSalesReturnAbstract.Click
        Dim obj As New BrighttechREPORT.frmSalesReturnAbstract()
        funcShow(obj, "SALES RETURN ABSTRACT")
        'funcShow(frmSalesReturnAbstract, "SALES RETURN ABSTRACT")
    End Sub

    Private Sub tStripRepairEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripRepairEntry.Click
        If Not CheckGoldRate() Then Exit Sub
        frmOrderInitialize.isRepair = True
        frmOrderInitialize.Text = "Repair Information"
        funcShow(frmOrderInitialize)
    End Sub

    'Private Sub tStripOrderRepairStatusView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripOrderRepairStatusView.Click
    '    funcShow(frmOrderRepairStatus, "ORDER REPAIR STATUS VIEW")
    'End Sub

    Private Sub tStripcompany_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripcompany.Click
        Dim obj As New BrighttechMaster.frmCompanyMast
        funcShow(obj, "COMPANY CREATION")
    End Sub

    Private Sub tStripMessenger_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripMessenger.Click
        frmChat.ShowDialog()
    End Sub

    Public Sub tStripCompanySelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCompanySelection.Click
        frmCompany.ShowDialog()
    End Sub

    Private Sub tStripIssueVsAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripIssueVsAccount.Click
        funcShow(New frmDataChecking(frmDataChecking.Type.Issue), "ISSUE VS TRANSATION DATA CHECKING")
    End Sub

    Private Sub tStripReceiptVsAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripReceiptVsAccount.Click
        funcShow(New frmDataChecking(frmDataChecking.Type.Receipt), "RECEIPT VS TRANSATION DATA CHECKING")
    End Sub

    Private Sub tStripGlobalDateChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripGlobalDateChange.Click
        If GetAdmindbSoftValue("GLOBALDATE").ToUpper = "Y" Then
            frmGlobalEntryDate.ShowDialog()
        End If
    End Sub

    Private Sub tStripUtility_DropDownOpened(ByVal sender As Object, ByVal e As System.EventArgs) Handles tStripUtility.DropDownOpened
        If GetAdmindbSoftValue("GLOBALDATE").ToUpper = "Y" Then
            If Not tStripUtility.DropDownItems.Contains(tStripGlobalDateChange) Then tStripUtility.DropDownItems.Add(tStripGlobalDateChange)
        Else
            If tStripUtility.DropDownItems.Contains(tStripGlobalDateChange) Then tStripUtility.DropDownItems.Remove(tStripGlobalDateChange)
        End If
    End Sub

    Private Sub DebToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DebToolStripMenuItem.Click
        If Not CheckGoldRate() Then Exit Sub
        funcShow(frmOpenDebtos)
    End Sub

    Public Sub tStripShortcut_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tStripShortcut.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim key As String = Nothing
            Dim flagModuleKey As Boolean = False
            If TypeOf sender Is Control Then
                key = CType(sender, Control).Text
                CType(sender, Control).Text = ""
            ElseIf TypeOf sender Is System.Windows.Forms.ToolStripTextBox Then
                key = CType(sender, System.Windows.Forms.ToolStripTextBox).Text
                CType(sender, System.Windows.Forms.ToolStripTextBox).Text = ""
            End If

            If key = "b" Or key = "B" Then
                If Not CheckGoldRate() Then Exit Sub
                frmBillInitialize.BillType = frmBillInitialize.BillTypee.Retail
                funcShow(frmBillInitialize)
            End If

            If key = "e" Or key = "E" Then
                Dim CostId As String = Nothing
                Dim CashId As String = Nothing

                Dim objEst As New frmEstimation1
                objEst.Hide()
                objEst.BillDate = GetEntryDate(GetServerDate)
                objEst.lblUserName.Text = cnUserName
                objEst.lblSystemId.Text = systemId
                objEst.lblBillDate.Text = GetEntryDate(GetServerDate).ToString("dd/MM/yyyy")
                objEst.Set916Rate(objEst.BillDate)
                objEst.WindowState = FormWindowState.Minimized
                BrighttechPack.LanguageChange.Set_Language_Form(objEst, LangId)
                objGPack.Validator_Object(objEst)

                objEst.Size = New Size(1032, 745)
                objEst.MaximumSize = New Size(1032, 745)
                objEst.StartPosition = FormStartPosition.Manual
                objEst.Location = New Point((ScreenWid - objEst.Width) / 2, ((ScreenHit - 25) - objEst.Height) / 2)

                objEst.KeyPreview = True
                objEst.MaximizeBox = False
                objEst.StartPosition = FormStartPosition.CenterScreen
                'objEst.Location = New Point(-3, -3)
                objEst.Show()
                objEst.WindowState = FormWindowState.Normal
            End If

            If key = "" Then
                'MsgBox("Invalid Id")
                Exit Sub
            End If


            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..MENUMASTER WHERE ISNULL(ACCESSKEY,'') = '" & key & "'"
            If Not objGPack.GetSqlValue(strSql).Length > 0 Then
                strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..MENUMASTER WHERE ISNULL(ACCESSKEY1,'') = '" & key & "'"
                If Not objGPack.GetSqlValue(strSql).Length > 0 Then
                    '  MsgBox("Invalid Id")
                    Exit Sub
                End If
                flagModuleKey = True
            End If
            Dim row() As DataRow
            If flagModuleKey Then
                row = _DtUserRights.Select("ACCESSKEY1 = '" & key & "'")
            Else
                row = _DtUserRights.Select("ACCESSKEY = '" & key & "'")
            End If
            If Not row.Length > 0 Then
                MsgBox("Access Denied", MsgBoxStyle.Information)
                Exit Sub
            End If
            'row(0).Item("ACCESSID").ToString CONTRAINS
            '~RPT THEN REPORT
            '~DIA THEN DIALOG
            '~OWN THEN HAVE OWN CONSTRACTOR
            If row(0).Item("ACCESSID").ToString.Contains("~OWN") Then
                MsgBox("Cannot Access", MsgBoxStyle.Information)
                Exit Sub
            End If
            '040113
            Dim DllName As String = Nothing
            If row(0).Item("REF_DLL").ToString <> "" Then
                DllName = row(0).Item("REF_DLL").ToString
            End If
            Dim sp() As String = row(0).Item("ACCESSID").ToString.Split("~")
            Dim accessId As String = sp(0)
            For Each ctrl As Control In Me.Controls
                If TypeOf ctrl Is LinkLabel Then
                    ctrl.Visible = False
                End If
            Next
            Dim f As Form
            '040113
            If DllName = Nothing Then
                f = GetForm(accessId)
            Else
                f = GetForm(accessId, "dll", DllName)
            End If
            AddHandler f.FormClosing, AddressOf obj_Closing
            tran = Nothing
            If row(0).Item("ACCESSID").ToString.Contains("~DIA") = False Then
                f.MdiParent = Me
            End If
            f.BackColor = frmBackColor
            f.BackgroundImage = bakImage
            f.BackgroundImageLayout = ImageLayout.Stretch
            f.StartPosition = FormStartPosition.CenterScreen
            If f.WindowState = FormWindowState.Maximized Then
                f.WindowState = FormWindowState.Maximized
                f.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                f.Dock = DockStyle.Fill
            End If
            f.MaximizeBox = False
            BrighttechPack.LanguageChange.Set_Language_Form(f, LangId)
            objGPack.Validator_Object(f)
            f.KeyPreview = True
            Me.TStripLblTitle.Text = f.Text
            'f.Size = New Size(1026, 662)
            f.BringToFront()
            If row(0).Item("ACCESSID").ToString.Contains("~DIA") Then
                f.ShowDialog()
            Else
                f.Show()
            End If
        End If
    End Sub
    Private Sub MultiDiscountToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MultiDiscountToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmMultiDiscount
        funcShow(obj, "MULTI DISCOUNT", False)
    End Sub

    Private Sub tStripTagTransfer2CTag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripTagTransfer2CTag.Click
        funcShow(frmTagTransfer2CTag, "SALES TAG TRANSFER", False)

    End Sub

    Private Sub tStripSyncCostcentre_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSyncCostcentre.Click
        If CENTR_DB_BR Then
            Dim objSecret As New frmGiriPwd()
            If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If
        End If
        Dim obj As New BrighttechMaster.frmSyncCostcentre
        funcShow(obj, "SYNC COSTCENTRE MASTER")
    End Sub

    Private Sub tStripSendReceiveView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSendReceiveView.Click
        Dim obj As New BrighttechMaster.frmSendReceiveView
        funcShow(obj, "SEND RECEIVE VIEW")
    End Sub

    Private Sub Send()
        Dim frmCostId As String = objGPack.GetSqlValue(" SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTID'").ToUpper
        Dim mainCostId As String = objGPack.GetSqlValue(" SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'").ToUpper
        If Not frmCostId.Length > 0 Then Exit Sub
        If Not mainCostId.Length > 0 Then Exit Sub
        Dim str As String = Nothing
        If frmCostId = mainCostId Then
            str = "SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE"
            str += " WHERE COSTID <> '" & frmCostId & "'"
            str += " AND MAIN <> 'Y' AND ISNULL(MANUAL,'') <> 'Y'"
        Else
            str = "SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE"
            str += " WHERE MAIN = 'Y'  AND ISNULL(MANUAL,'') <> 'Y'"
        End If
        Dim dt As New DataTable
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dt)
        Dim toCostId As New List(Of String)
        For Each ro As DataRow In dt.Rows
            toCostId.Add(ro!COSTID.ToString)
        Next
        If Not toCostId.Count > 0 Then Exit Sub
        Try
            If GetAdmindbSoftValue("SYNC-MODE", "M") = "M" Then
                Dim obj As New BrighttechPack.Transfer(Me.pBar, Me.tStripBarStatus, Me)
                obj.Send(frmCostId, toCostId, cnAdminDb, cnStockDb, cn)
            Else
                Dim obj As New BrighttechPack.TransferFile()
                obj.SendFile(frmCostId, toCostId, cnAdminDb, cnStockDb, cn)
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub receiveThread_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles receiveThread.DoWork
        'Dim obj As New BrighttechPack.Transfer
        'Dim frmCostId As String = objGPack.GetSqlValue(" SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTID'").ToUpper
        'obj.Receive(frmCostId, cnAdminDb, cnStockDb, cn)
    End Sub

    Private Sub sendThread_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles sendThread.DoWork
        'Dim obj As New BrighttechPack.Transfer
        'Dim frmCostId As String = objGPack.GetSqlValue(" SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTID'").ToUpper
        'Dim mainCostId As String = objGPack.GetSqlValue(" SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'").ToUpper
        'If Not frmCostId.Length > 0 Then Exit Sub
        'If Not mainCostId.Length > 0 Then Exit Sub
        'Dim str As String = Nothing
        'If frmCostId = mainCostId Then
        '    str = "SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE"
        '    str += " WHERE COSTID <> '" & frmCostId & "'"
        '    str += " AND MAIN <> 'Y'"
        'Else
        '    str = "SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE"
        '    str += " WHERE MAIN = 'Y'"
        'End If
        'Dim dt As New DataTable
        'da = New OleDbDataAdapter(str, cn)
        'da.Fill(dt)
        'Dim toCostId As New List(Of String)
        'For Each ro As DataRow In dt.Rows
        '    toCostId.Add(ro!COSTID.ToString)
        'Next
        'If Not toCostId.Count > 0 Then Exit Sub
        'Try
        '    obj.Send(frmCostId, toCostId, cnAdminDb, cnStockDb, cn)
        'Catch ex As Exception
        '    MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        'End Try
    End Sub

    Private Sub tStripSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSend.Click
        If GetAdmindbSoftValue("SYNC-MODE") <> "W" Then
            'Me.Cursor = Cursors.WaitCursor
            Me.Refresh()
            Send()
            Me.Cursor = Cursors.Default
            If Not sendThread.IsBusy Then sendThread.RunWorkerAsync()
        Else
            If IO.File.Exists(Application.StartupPath & "\Syncronizer.EXE") Then
                System.Diagnostics.Process.Start(Application.StartupPath & "\Syncronizer.EXE", "SEND")
                Exit Sub
            End If
        End If

    End Sub

    Private Sub Receive()
        Dim frmCostId As String = objGPack.GetSqlValue(" SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTID'").ToUpper
        If GetAdmindbSoftValue("SYNC-MODE", "M") = "M" Then
            Dim obj As New BrighttechPack.Transfer(Me.pBar, Me.tStripBarStatus, Me)
            obj.Receive(frmCostId, cnAdminDb, cnStockDb, cn)
        Else
            Dim obj As New BrighttechPack.TransferFile
            obj.ReceiveFile(frmCostId, cnAdminDb, cnStockDb, cn)
        End If
    End Sub

    Private Sub tStripReceive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripReceive.Click
        If GetAdmindbSoftValue("SYNC-MODE") <> "W" Then
            'Me.Cursor = Cursors.WaitCursor
            Me.Refresh()
            Receive()
            Me.Cursor = Cursors.Default
            If Not receiveThread.IsBusy Then receiveThread.RunWorkerAsync()
        Else
            If IO.File.Exists(Application.StartupPath & "\Syncronizer.EXE") Then
                System.Diagnostics.Process.Start(Application.StartupPath & "\Syncronizer.EXE", "RECEIVE")
                Exit Sub
            End If
        End If
    End Sub

    Private Sub tSripSyncMaster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tSripSyncMaster.Click
        Dim obj As New BrighttechMaster.frmSyncMast
        funcShow(obj, "SYNC MASTER", False)
    End Sub

    Private Sub tStripStateCategory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripStateCategory.Click
        Dim obj As New BrighttechMaster.frmStateCategory
        funcShow(obj, "STATE WISE CATEGORY MASTER")
    End Sub

    Private Sub StateWiseItemMastToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StateWiseItemMastToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmStateItemMast
        funcShow(obj, "STATE WISE ITEM MASTER")
    End Sub

    Private Sub MenuMasterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripMenuMaster.Click
        funcShow(frmMenuMaster, "MENU MASTER", True)
    End Sub

    Private Sub tStripUserMaster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripUserMaster.Click
        Dim obj As New BrighttechMaster.frmUserMaster
        funcShow(obj, "USER MASTER", False)
    End Sub

    Private Sub tStripRoleMaster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripRoleMaster.Click
        Dim obj As New BrighttechMaster.frmRoleMaster
        funcShow(obj, "ROLE MASTER", False)
    End Sub

    Private Sub tStripRoleTran_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripRoleTran.Click
        funcShow(frmRoleTran, "ROLE TRAN", False)
    End Sub

    Private Sub tStripUserRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripUserRole.Click
        Dim obj As New BrighttechMaster.frmUserRole
        funcShow(obj, "USER ROLE", False)
    End Sub


    Private Sub tStripTagTransit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripTagTransit.Click
        funcShow(frmTagTransit, "TAG TRANSIT")
    End Sub

    Private Sub tStripAccEntryMaster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripAccEntryMaster.Click
        Dim obj As New BrighttechMaster.frmAccEntryMaster
        funcShow(obj, "ACCOUNTS ENTRY MASTER")
    End Sub

    Private Sub tStripSavingsScheme_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSavingsScheme.Click
        If IO.File.Exists(Application.StartupPath & "\SAVINGS\CHIT.EXE") Then
            System.Diagnostics.Process.Start(Application.StartupPath & "\SAVINGS\CHIT.EXE", "" & cnUserName & " " & cnPassword & "")
        Else
            MsgBox("Savings exe not found", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub tStripBagnoGeneration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripBagnoGeneration.Click
        funcShow(frmBagNoGeneration, "BAGNO GENERATION")
    End Sub

    Private Sub tStripBagnoWiseSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripBagnoWiseSummary.Click
        funcShow(frmBagNoWiseSummary, "BAGNO WISE SUMMARY")
    End Sub

    Private Sub tStripMeltingIssue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripMeltingIssue.Click
        funcShow(frmMeltingIssue, "MELTING ISSUE")
    End Sub

    Private Sub tStripMeltingReceipt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripMeltingReceipt.Click
        funcShow(frmMeltingReceipt, "MELTING RECEIPT")
    End Sub


    Private Sub tStripBagnoWiseProfitLoss_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripBagnoWiseProfitLoss.Click
        funcShow(frmBagNoWiseProfitLossSummary, "BAGNO WISE PROFIT & LOSS")
    End Sub

    Private Sub tStripCounterChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripTagCounterChange.Click
        funcShow(frmCounterChange, "TAG COUNTER CHANGE")
    End Sub

    Private Sub tStripNonTagCounterChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripNonTagCounterChange.Click
        funcShow(frmNonTagCounterChange, "NON TAG COUNTER CHANGE")
    End Sub

    Private Sub tStripLotDetailedView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripLotDetailedView.Click
        funcShow(frmLotViewDetailed, "LOT VIEW")
    End Sub

    Private Sub PendingLotViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PendingLotViewToolStripMenuItem.Click
        funcShow(frmPendingLotView, "PENDING LOTVIEW")
    End Sub

    Private Sub tStripBillView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripBillView.Click
        frmBillView.type = frmBillView.BillViewType.BillView
        funcShow(frmBillView, "BILLVIEW")
    End Sub

    Private Sub tStripDuplicateBillPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripDuplicateBillPrint.Click
        frmBillView.type = frmBillView.BillViewType.DuplicateBillPrint
        funcShow(frmBillView, "DUPLICATE BILLPRINT")
    End Sub


    Private Sub tStripTagTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripTagTransfer.Click
        funcShow(frmTagTransfer, "TAG TRANSFER")
        'funcShow(frmStkTransfer, "STOCK TRANSFER")
    End Sub

    Private Sub TagCheckTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagCheckTStrip.Click
        funcShow(frmTagCheck, "TAG CHECK", False)
    End Sub

    Private Sub SmithBalanceSummaryTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SmithBalanceSummaryTStrip.Click
        If GetAdmindbSoftValue("RPT_SMITHBALSUM_FORMAT", "1") = "1" Then
            Dim obj As New BrighttechREPORT.frmSmithBalanceSummaryReport()
            funcShow(obj, "SMITH BALANCE SUMMARY")
        ElseIf GetAdmindbSoftValue("RPT_SMITHBALSUM_FORMAT", "1") = "2" Then
            Dim obj As New BrighttechREPORT.frmSmithBalanceSummaryReportNew()
            funcShow(obj, "SMITH BALANCE SUMMARY")
        Else
            funcShow(frmSmithBalanceSummaryFormat3, "SMITH BALANCE SUMMARY")
        End If
    End Sub

    Private Sub SmithIssueReciptViewTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SmithIssueReciptViewTStrip.Click
        If GetAdmindbSoftValue("SMITHISSREC_FORMAT", "1") = "2" Then
            funcShow(frmSmithRecIssView_New, "SMITH ISSUE RECEIPT VIEW")
        Else
            funcShow(frmSmithRecIssView, "SMITH ISSUE RECEIPT VIEW")
        End If
    End Sub

    Private Sub SmithBalanceDetailedTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SmithBalanceDetailedTStrip.Click
        If GetAdmindbSoftValue("RPT_SMITHBALDET_FORMAT", "1") = "3" Then
            Dim obj As New BrighttechREPORT.frmNewSmithBalanceDetailedReport()
            funcShow(obj, "SMITH BALANCE DETAILED")
        ElseIf GetAdmindbSoftValue("RPT_SMITHBALDET_FORMAT", "1") = "4" Then
            Dim obj As New BrighttechREPORT.frmNewSmithBalanceSummary()
            funcShow(obj, "SMITH BALANCE DETAILED")
        Else
            Dim obj As New BrighttechREPORT.frmSmithBalanceDetailedReport()
            funcShow(obj, "SMITH BALANCE DETAILED")
        End If
    End Sub

    Private Sub SaleItemWastageMcAnalysisTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaleItemWastageMcAnalysisTStrip.Click
        Dim obj As New BrighttechREPORT.frmSaleReportWastMc()
        funcShow(obj, "SALES ITEM WASTAGE & MC ANALYSIS REPORT")
        'funcShow(frmSaleReportWastMc, "SALES ITEM WASTAGE & MC ANALYSIS REPORT")
    End Sub

    Private Sub ApprovalSummaryTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApprovalSummaryTStrip.Click
        Dim obj As New BrighttechREPORT.frmApprovalSummary()
        funcShow(obj, "APPROVAL SUMMARY")
        'funcShow(frmApprovalSummary, "APPROVAL SUMMARY")
    End Sub

    Private Sub MetalOrnamentStockViewTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MetalOrnamentStockViewTStrip.Click
        Dim obj As New BrighttechREPORT.FRM_STOCKVIEW_SUMMARY()
        funcShow(obj, "METAL ORNAMENT STOCK VIEW (SUMMARY)")
        'funcShow(FRM_STOCKVIEW_SUMMARY, "METAL ORNAMENT STOCK VIEW (SUMMARY)")
    End Sub

    Private Sub MetalOrnamentStockDetailedTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MetalOrnamentStockDetailedTStrip.Click
        Dim obj As New BrighttechREPORT.frmMetalOrnamentDetailedStockView()
        funcShow(obj, "METAL ORNAMENT STOCK VIEW (DETAILED)")
        'funcShow(frmMetalOrnamentDetailedStockView, "METAL ORNAMENT STOCK VIEW (DETAILED)")
    End Sub

    Private Sub OrderStatusTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderStatusTStrip.Click
        Dim obj As New BrighttechREPORT.frmOrderStatusReport()
        funcShow(obj, "ORDER/REPAIR STATUS REPORT")
        'funcShow(frmOrderStatusReport, "ORDER STATUS REPORT")
    End Sub

    Private Sub CentRateUpdateTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CentRateUpdateTStrip.Click
        Dim obj As New BrighttechMaster.frmCentRateUpdate()
        funcShow(obj, "CENT RATE UPDATOR")
    End Sub

    Private Sub tStripCounterSales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripHomeSales.Click
        funcShow(frmHomeSalesReport, "HOME SALES REPORT")
    End Sub

    Private Sub tStripPartlySales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripPartlySales.Click
        Dim obj As New BrighttechREPORT.frmPartlySalesReport()
        funcShow(obj, "PARTLY SALES REPORT")
        'funcShow(frmPartlySalesReport, "PARTLY SALES REPORT")
    End Sub

    Private Sub tStripMiscellaneous_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripMiscellaneous.Click
        Dim obj As New BrighttechREPORT.frmMiscellaneousReport()
        funcShow(obj, "MISCELLANEOUS REPORT")
        'funcShow(frmMiscellaneousReport, "MISCELLANEOUS REPORT")
    End Sub

    Private Sub tStripBillwiseTransaction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripBillwiseTransaction.Click
        Dim obj As New BrighttechREPORT.frmBillWiseTransaction()
        funcShow(obj, "BILL WISE TRANSACTION REPORT")
        'funcShow(frmBillWiseTransaction, "BILL WISE TRANSACTION REPORT")
    End Sub

    Private Sub tStripCardCollection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCardCollection.Click
        Dim obj As New BrighttechREPORT.frmCardCollectionsReport()
        funcShow(obj, "CARD COLLECTION REPORT")
        'funcShow(frmCardCollectionsReport, "CARD COLLECTION REPORT")
    End Sub

    Private Sub tStripCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCancel.Click
        Dim obj As New BrighttechREPORT.frmCancelReport()
        funcShow(obj, "CANCEL REPORT")
        'funcShow(frmCancelReport, "CANCEL REPORT")
    End Sub

    Private Sub tStripCashTransactionRpt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCashTransactionRpt.Click
        Dim obj As New BrighttechREPORT.frmCashTransactionReport()
        funcShow(obj, "CASH TRANSACTION REPORT")
        'funcShow(frmCashTransactionReport, "CASH TRANSACTION REPORT")
    End Sub

    Private Sub tStripCustomerOutstanding_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCustomerOutstanding.Click

    End Sub

    Private Sub tStripPurchase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripPurchase.Click
        Dim obj As New BrighttechREPORT.frmPurchaseReport()
        funcShow(obj, "PURCHASE REPORT")
        'funcShow(frmPurchaseReport, "PURCHASE REPORT")
    End Sub

    Private Sub tStripSalesPerPerformance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSalesPerPerformance.Click
        Dim obj As New BrighttechREPORT.frmSalesPersonPerformance()
        funcShow(obj, "SALES PERFORMANCE REPORT")
        'funcShow(frmSalesPersonPerformance, "SALES PERFORMANCE REPORT")
    End Sub

    Private Sub StockSaleValueCheckToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StockSaleValueCheckToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.FrmStockSaleValueCheckRpt()
        funcShow(obj, "STOCK VALUE CHECK REPORT")
        'funcShow(FrmStockSaleValueCheckRpt, "STOCK VALUE CHECK REPORT")
    End Sub
    Private Sub tStripCounterWiseSales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCounterWiseSales.Click
        Dim obj As New BrighttechREPORT.frmCounterwiseSales()
        funcShow(obj, "COUNTER WISE SALES")
        'funcShow(frmCounterwiseSales, "COUNTER WISE SALES")
    End Sub
    Private Sub tStripRateView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripRateView.Click
        Dim obj As New BrighttechREPORT.frmRateView()
        funcShow(obj, "RATE VIEW")
        'funcShow(frmRateView, "RATE VIEW")
    End Sub
    Private Sub PurchaseReportForTagedItemsTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagedItemsPurchaseReportTStrip.Click

    End Sub
    Private Sub TagedItemsSalesReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagedItemsSalesReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmTagItemsSalesReport()
        funcShow(obj, "TAGED ITEMS SALES REPORT")
        'funcShow(frmTagItemsSalesReport, "TAGED ITEMS SALES REPORT")
    End Sub
    Private Sub DayBookTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DayBookTStrip.Click

    End Sub

    Private Sub EstimatePostTrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EstimatePostTrip.Click
        funcShow(EstimatePostingFrm, "ESTIMATE POST", False)
    End Sub

    Private Sub DuplicateTagTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DuplicateTagTStrip.Click
        funcShow(TagDuplicatePrint, "DUPLICATE TAG", False)
    End Sub

    Private Sub YearEndProcessTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YearEndProcessTStrip.Click
        Dim objSecret As New frmAdminPassword()
        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If
        Dim dd As Date = objGPack.GetSqlValue("SELECT CONVERT(VARCHAR,STARTDATE) FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME = '" & cnStockDb & "'")
        'GetServerDate(tran)
        Dim objYEnd As New DatabaseCreator(dd, dd.AddYears(1), False)
        objYEnd.ShowDialog()
    End Sub

    Private Sub YearSelectionTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YearSelectionTStrip.Click
        YearSelection.ShowDialog()
    End Sub

    Private Sub CashAbstractToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CashAbstractToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.CashAbstract()
        funcShow(obj, "CASH ABSTRACT")
        'funcShow(CashAbstract, "CASH ABSTRACT")
    End Sub


    Private Sub ReceiptAndPaymentToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReceiptAndPaymentToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmReceiptPaymentDetailss()
        funcShow(obj, "RECEIPT & PAYMENT DETAIL")
        'funcShow(frmReceiptPaymentDetailss, "RECEIPT & PAYMENT DETAIL")
    End Sub

    Private Sub EditChequeAccountToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditChequeAccountToolStripMenuItem.Click
        funcShow(EditChequeAccount, "EDIT CHEQUE ACCOUNT NAME")
    End Sub

    Private Sub ChitAccountPostToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChitAccountPostToolStripMenuItem.Click
        funcShow(ChitAccountPost, "SCHEME ACCOUNT POST", False)
    End Sub

    Private Sub TagWiseProfitLossToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagWiseProfitLossToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.TagWiseProfitLoss()
        funcShow(obj, "TAG WISE PROFIT & LOSS")
        'funcShow(TagWiseProfitLoss, "TAG WISE PROFIT & LOSS")
    End Sub

    Private Sub AccountRunningBalanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AccountRunningBalanceToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.AccRunningBalance()
        funcShow(obj, "ACCOUNT RUNNING BALANCE")
        'funcShow(AccRunningBalance, "ACCOUNT RUNNING BALANCE")
    End Sub

    Private Sub ImportExportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim objExpImp As New Export
        'objExpImp.ShowDialog()
    End Sub

    Private Sub TagDetailToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagDetailToolStripMenuItem.Click
        funcShow(frmTagEditView, "TAGEDIT VIEW")
    End Sub

    Private Sub tStripTagBulkDetailUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripTagBulkDetailUpdate.Click
        funcShow(frmTagDetailUpdator, "TAG DETAIL BULK UPDATOR")
    End Sub

    Private Sub tStripItemWiseStockSummaryView_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripItemWiseStockSummaryView.Click
        Dim obj As New BrighttechREPORT.frmItemWiseStockSummary()
        funcShow(obj, "ITEM WISE STOCK SUMMARY")
        'funcShow(frmItemWiseStockSummary, "ITEM WISE STOCK SUMMARY")
    End Sub

    Private Sub TagImageUpdatorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagImageUpdatorToolStripMenuItem.Click
        funcShow(frmPicManualUpdate, "TAG IMAGE UPDATE (MANUAL)")
    End Sub

    Private Sub CatalogEntryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CatalogEntryToolStripMenuItem.Click
        Dim catalogPath As String = GetAdmindbSoftValue("TAGCATALOGPATH")
        If catalogPath = "" Then
            MsgBox("Please set tag catalog path at softcontrol", MsgBoxStyle.Information)
            Exit Sub
        Else
            If Not IO.Directory.Exists(catalogPath) Then
                MsgBox("Tagcatalog path not found", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        funcShow(TagCatalog, "TAG CATALOG")
    End Sub

    Private Sub CatalogViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CatalogViewToolStripMenuItem.Click
        funcShow(CatalogView, "TAG CATALOG VIEW")
    End Sub

    Private Sub CounterWiseStockToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CounterWiseStockToolStripMenuItem.Click
        If GetAdmindbSoftValue("CTRSTK_REP_OLD", "N") = "Y" Then
            Dim obj As New BrighttechREPORT.CounterWiseStock_old()
            funcShow(obj, "COUNTER WISE STOCK")
            'funcShow(CounterWiseStock_old, "COUNTER WISE STOCK")
        Else
            Dim obj As New BrighttechREPORT.CounterWiseStock()
            funcShow(obj, "COUNTER WISE STOCK")
            'funcShow(CounterWiseStock, "COUNTER WISE STOCK")
        End If
    End Sub

    Private Sub CustomerBalanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomerBalanceToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.WCustomerOutstanding()
        funcShow(obj, "CUSTOMER OUTSTANDING")
        'funcShow(WCustomerOutstanding, "CUSTOMER OUTSTANDING")
    End Sub

    Private Sub CentRateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CentRateToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmCentRate
        funcShow(obj, "CENT RATE")
    End Sub

    Private Sub CentSizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CentSizeToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmCentSize
        funcShow(obj, "CENT SIZE")
    End Sub

    Private Sub StockViewDetailedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StockViewDetailedToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.TagedItemsStockViewDetailed()
        funcShow(obj, "TAGED ITEMS STOCK VIEW DETAILED")
        'funcShow(TagedItemsStockViewDetailed, "TAGED ITEMS STOCK VIEW DETAILED")
    End Sub

    Private Sub RetailToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RetailToolStripMenuItem.Click
        If Not CheckGoldRate() Then Exit Sub
        frmBillInitialize.BillType = frmBillInitialize.BillTypee.Retail
        funcShow(frmBillInitialize)
    End Sub

    Private Sub WholeSaleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WholeSaleToolStripMenuItem.Click
        If Not CheckGoldRate() Then Exit Sub
        frmBillInitialize.BillType = frmBillInitialize.BillTypee.WholeSale
        funcShow(frmBillInitialize)
    End Sub

    Private Sub ManualSendToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManualSendToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.SendManual
        funcShow(obj, "SEND MANUAL [FILE GENERATOR]", False)
    End Sub

    Private Sub ManualReceiveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManualReceiveToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.ReceiveManual
        funcShow(obj, "RECEIVE MANUAL [FILE RECEIVER]", False)
    End Sub

    Private Sub PacketWiseStockToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PacketWiseStockToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.PacketWiseStockView()
        funcShow(obj, "PACKET WISE STOCK")
        'funcShow(PacketWiseStockView, "PACKET WISE STOCK")
    End Sub

    Private Sub OrderTagToRegularTagToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderTagToRegularTagToolStripMenuItem.Click
        funcShow(OrderTagtoRegular, "Order Tag to Regular Tag")
    End Sub

    Private Sub RunningBalanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RunningBalanceToolStripMenuItem.Click
        funcShow(W_RunningBalance, "RUNNING BALANCE")
    End Sub

    Private Sub OrderEditToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderEditToolStripMenuItem.Click
        funcShow(OrderEditView, "ORDER EDIT VIEW")
    End Sub

    Private Sub CounterTransferInfoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CounterTransferInfoToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.CounterTransferInfo()
        funcShow(obj, "COUNTER TRANSFER INFO")
        'funcShow(CounterTransferInfo, "COUNTER TRANSFER INFO")
    End Sub

    Private Sub VaMarginToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VaMarginToolStripMenuItem1.Click
        Dim obj As New BrighttechMaster.VAMARGIN
        funcShow(obj, "VALUE ADDED MARGIN SETTINGS")
    End Sub

    Private Sub ItemWiseMarginLockToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemWiseMarginLockToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.ItemwiseMarginDiscLock
        funcShow(obj, "ITEM/TABLE WISE MARGIN/DISC. LOCK")
    End Sub

    Private Sub TagStockChangeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagStockChangeToolStripMenuItem.Click
        funcShow(StockTransfer, "STOCK COMPANY CHANGE")
    End Sub

    Private Sub DailyTransactionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DailyTransactionToolStripMenuItem.Click
        funcShow(WTransactionReport, "DAILY TRANSACTION")
    End Sub

    Private Sub JewelNotDeliveredToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles JewelNotDeliveredToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.JewelNotDelivered()
        funcShow(obj, "JEWEL NOT DELIVERED")
        'funcShow(JewelNotDelivered, "JEWEL NOT DELIVERED")
    End Sub

    Private Sub RateCutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RateCutToolStripMenuItem.Click
        funcShow(WRateCut, "RATE CUT")
    End Sub

    Private Sub TagedItemsPurchaseReportDetailedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub WholeSaleSmithIssRecToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WholeSaleSmithIssRecToolStripMenuItem.Click
        funcShow(WSmithIssRec, "SMITH & DEALER ISSUE RECEIPT", , False)
    End Sub

    Private Sub AllLedgerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllLedgerToolStripMenuItem.Click
        funcShow(WLedger_All, "ALL LEDGER")
    End Sub

    Private Sub GroupLedgerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupLedgerToolStripMenuItem.Click
        Dim obj As New GroupLedger(GroupLedger.GRType.GroupLedger)
        funcShow(obj, "GROUP LEDGER")
    End Sub


    Private Sub TagCertificateNoUpdationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagCertificateNoUpdationToolStripMenuItem.Click
        funcShow(CertificateNoUpdation, "TAG CERTIFICATENO UPDATOR")
    End Sub

    Private Sub CounterStockCheckToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CounterStockCheckToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.CounterWiseStock(True)
        funcShow(obj, "COUNTER WISE STOCK")
        'Dim obj As New CounterWiseStock(True)
        'funcShow(obj, "COUNTER WISE STOCK")
    End Sub

    Private Sub ItemWiseProfitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemWiseProfitToolStripMenuItem.Click
        If GetAdmindbSoftValue("RPT_ITEMPROFIT_FORMAT", "N") = "2" Then
            Dim obj As New BrighttechREPORT.frmItemWiseProfit()
            funcShow(obj, "ITEM WISE PROFIT")
        Else
            Dim obj As New BrighttechREPORT.ItemWiseProfit()
            funcShow(obj, "ITEM WISE PROFIT")
        End If
    End Sub

    Private Sub CounterTransferAuthorizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CounterTransferAuthorizeToolStripMenuItem.Click
        funcShow(CounterInwardAuthorize, "AUTHORIZE COUNTER TRANSFER INFO")
    End Sub

    Private Sub CashPointToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CashPointToolStripMenuItem.Click
        If Not CheckGoldRate() Then Exit Sub
        frmBillInitialize.BillType = frmBillInitialize.BillTypee.CashPoint

        funcShow(frmBillInitialize)
    End Sub

    Private Sub XmlGenerationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XmlGenerationToolStripMenuItem.Click
        If IO.File.Exists(Application.StartupPath & "\XmlGenerator.exe") Then
            Dim cmdArgs As String = ""
            cmdArgs += cnAdminDb
            cmdArgs += " " & cnStockDb
            cmdArgs += " " & cnTranFromDate.ToString("yyyy-MM-dd")
            cmdArgs += " " & cnTranToDate.ToString("yyyy-MM-dd")
            System.Diagnostics.Process.Start(Application.StartupPath & "\XmlGenerator.exe", cmdArgs)
        End If
    End Sub

    Private Sub SalesVatToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesVatToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.Sales_Purchase_Vat()
        funcShow(obj, "SALES AND PURCHASE VAT")
        'funcShow(Sales_Purchase_Vat, "SALES AND PURCHASE VAT")
    End Sub

    Private Sub AddressInfoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddressInfoToolStripMenuItem.Click
        funcShow(AddressInfo, "ADDRESS INFO")
    End Sub

    Private Sub MetalStockToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MetalStockToolStripMenuItem.Click
        funcShow(WMetalBalance, "METAL STOCK")
    End Sub

    Private Sub DailyAverageRateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DailyAverageRateToolStripMenuItem.Click
        funcShow(W_DailyRateReport, "DAILY AVERAGE RATE")
    End Sub

    Private Sub TrailSummaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrailSummaryToolStripMenuItem.Click
        funcShow(WTrialSummary, "TRIAL SUMMARY")
    End Sub

    Private Sub InterestCalculationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InterestCalculationToolStripMenuItem.Click
        funcShow(W_InterestCalculation, "INTEREST CALCULATION")
    End Sub

    Private Sub TagNoWiseProfitLossToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagNoWiseProfitLossToolStripMenuItem.Click
        funcShow(W_TagWiseProfitAndLoss, "TAGNO WISE PROFIT & LOSS")
    End Sub

    Private Sub ChangePartyCodeOutstandingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangePartyCodeOutstandingToolStripMenuItem.Click
        funcShow(EditPartyCode, "CHANGE CUSTOMER INFO")
    End Sub

    Private Sub LotBulkIssueToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        funcShow(W_LotBulkIssue, "LOT BULK ISSUE")
    End Sub

    Private Sub AgeWiseOutStandingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AgeWiseOutStandingToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.AgeWiseOutstanding()
        funcShow(obj, "AGE WISE OUTSTANDING")
        'funcShow(AgeWiseOutstanding, "AGE WISE OUTSTANDING")
    End Sub

    Private Sub DenominationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DenominationToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.DenominationRpt()
        funcShow(obj, "DENOMINATION REPORT")
        'funcShow(DenominationRpt, "DENOMINATION REPORT")
    End Sub

    Private Sub ItemWiseSalesReviewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemWiseSalesReviewToolStripMenuItem.Click
        funcShow(ItemWiseSales_Review, "ITEM WISE SALES REVIEW")
    End Sub

    Private Sub OtherReportsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OtherReportsToolStripMenuItem.Click
        If IO.File.Exists(Application.StartupPath & "\AGReports\AGOLDREPORT.exe") Then
            System.Diagnostics.Process.Start(Application.StartupPath & "\AGReports\AGOLDREPORT.exe")
        End If
    End Sub

    Private Sub TagWiseTransferIssReceiptToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagWiseTransferIssReceiptToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.TagIssRecSyncRpt()
        funcShow(obj, "TAG WISE TRANSFER ISSUE/RECEIPT")
        'funcShow(TagIssRecSyncRpt, "TAG WISE TRANSFER ISSUE/RECEIPT")
    End Sub

    Private Sub DueDateWiseOutstandingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DueDateWiseOutstandingToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.DueDateWiseOutstandingRpt()
        funcShow(obj, "DUE DATE WISE OUTSTANDING")
        'funcShow(DueDateWiseOutstandingRpt, "DUE DATE WISE OUTSTANDING")
    End Sub

    Private Sub ItemWiseStockFlowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemWiseStockFlowToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.ItemWiseStockFlow()
        funcShow(obj, "ITEM WISE STOCK FLOW")
        'funcShow(ItemWiseStockFlow, "ITEM WISE STOCK FLOW")
    End Sub

    Private Sub TagHallmarkInfoUpdationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagHallmarkInfoUpdationToolStripMenuItem.Click
        funcShow(HallmarkInfo, "HALLMARK INFO")
    End Sub

    Private Sub ApprovalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApprovalToolStripMenuItem.Click
        funcShow(AccApproval, "APPROVAL")
    End Sub

    Private Sub ExpenseAnalysisToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExpenseAnalysisToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.ExpenseAnalysis()
        funcShow(obj, "EXPENSE ANALYSIS")
        'funcShow(ExpenseAnalysis, "EXPENSE ANALYSIS")
    End Sub

    Private Sub AccountsConfirmationLetterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AccountsConfirmationLetterToolStripMenuItem.Click
        If GetAdmindbSoftValue("ACC_CONFIRMATION_FORMAT", "1") = "1" Then
            Dim obj As New GroupLedger(GroupLedger.GRType.ConfirmationLetter)
            funcShow(obj, "ACCOUNTS CONFIRMATION LETTER")
        Else
            Dim obj As New GroupLedgerNew(GroupLedgerNew.GRType.ConfirmationLetter)
            funcShow(obj, "ACCOUNTS CONFIRMATION LETTER")
        End If
    End Sub

    Private Sub AdvanceClosedAgingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdvanceClosedAgingToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.AdvanceClosedAging()
        funcShow(obj, "ADVANCE CLOSED AGING")
        'funcShow(AdvanceClosedAging, "ADVANCE CLOSED AGING")
    End Sub

    Private Sub TDSCategoryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TDSCategoryToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmTDScategory
        funcShow(obj, "TDS CATEGORY")
    End Sub

    Private Sub AdditionalChargesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdditionalChargesToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmAddChargeMaster
        funcShow(obj, "ADDITIONAL CHARGES")
    End Sub

    Private Sub BulkIssueToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BulkIssueToolStripMenuItem.Click
        funcShow(LotBulkIssue, "BULK LOT ISSUE")
    End Sub

    Private Sub ManualIssueToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManualIssueToolStripMenuItem.Click
        funcShow(LotManualIssue, "MANUAL LOT ISSUE")
    End Sub

    Private Sub MaterialIssueToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MaterialIssueToolStripMenuItem.Click
        If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MIMR_CASHCTRPRMT'", , "N", tran).ToUpper = "Y" Then
            frmMIMRInitialize.isReceipt = False
            frmMIMRInitialize.Text = "MATERIAL ISSUE"
            funcShow(frmMIMRInitialize)
            Exit Sub
        End If
        Dim obj As New MaterialIssRecTran(MaterialIssRecTran.MaterialType.Issue)
        obj.lblTitle.Text = "MATERIAL ISSUE"
        funcShow(obj, "MATERIAL ISSUE")
    End Sub

    Private Sub MaterialReceiptToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MaterialReceiptToolStripMenuItem.Click
        If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MIMR_CASHCTRPRMT'", , "N", tran).ToUpper = "Y" Then
            frmMIMRInitialize.isReceipt = True
            frmMIMRInitialize.Text = "MATERIAL RECEIPT"
            funcShow(frmMIMRInitialize)
            Exit Sub
        End If
        Dim obj As New MaterialIssRecTran(MaterialIssRecTran.MaterialType.Receipt)
        obj.lblTitle.Text = "MATERIAL RECEIPT"
        funcShow(obj, "MATERIAL RECEIPT")
    End Sub

    Private Sub NonTagCostcentreTransferToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        funcShow(frmNonTagCostcentreChange, "NONTAG COSTCENTRE TRANSFER")
    End Sub

    Private Sub WeightDayBookToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WeightDayBookToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmWeightDayBook()
        funcShow(obj, "WEIGHT DAY BOOK")
        'funcShow(frmWeightDayBook, "WEIGHT DAY BOOK")
    End Sub

    Private Sub AccountsTransferToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AccountsTransferToolStripMenuItem.Click
        Dim objSecret As New frmAdminPassword()
        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If
        funcShow(frmAccountsTransfer, "ACCOUNTS TRANSFER")
    End Sub

    Private Sub ReorderStockCounterWiseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub TrandingProfitAndLossToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrandingProfitAndLossToolStripMenuItem.Click
        'Dim obj As New BrighttechREPORT.frmTrading()
        'funcShow(obj, "TRADING AND PROFIT & LOSS")

        funcShow(frmTrading, "TRADING AND PROFIT & LOSS")

        'If GetAdmindbSoftValue("TRADING_PROFITLOSS_FORMAT1", "N") = "Y" Then
        '    funcShow(frmTradingNew, "TRADING AND PROFIT & LOSS")
        'Else
        '    funcShow(frmTrading, "TRADING AND PROFIT & LOSS")
        'End If

    End Sub

    Private Sub BalanceSheetToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BalanceSheetToolStripMenuItem.Click
        If GetAdmindbSoftValue("RPT_BALANCESHEET_FORMAT1", "N") = "Y" Then
            Dim obj As New BrighttechREPORT.frmBalanceSheet_Format1()
            funcShow(obj, "BALANCE SHEET")
        Else
            Dim obj As New BrighttechREPORT.frmBalanceSheet()
            funcShow(obj, "BALANCE SHEET")
        End If
    End Sub

    Private Sub BookerItemMarkToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BookerItemMarkToolStripMenuItem.Click
        funcShow(BOOKED_ITEM_MARK, "MARK BOOKED ITEM")
    End Sub

    Private Sub BookerItemUnMarkToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BookerItemUnMarkToolStripMenuItem.Click
        funcShow(BOOKED_ITEM_UNMARK, "UNMARK BOOKED ITEM")
    End Sub

    Private Sub OrderStatusToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderStatusToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmOrderStaus
        funcShow(obj, "ORDER STATUS")
    End Sub

    Private Sub OrderStatusToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderStatusToolStripMenuItem1.Click
        Dim obj As New BrighttechREPORT.FRM_ORDERREPAIRSTATUS()
        funcShow(obj, "ORDER/REPAIR STATUS")
        'funcShow(FRM_ORDERREPAIRSTATUS, "ORDER REPAIR STATUS")
    End Sub

    Private Sub BillNoRegeneratorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BillNoRegeneratorToolStripMenuItem.Click
        BillNoRegenerator()
    End Sub

    Private Sub TagRateUpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagRateUpdateToolStripMenuItem.Click
        funcShow(frmTagRateUpdate, "TAG RATE UPDATE")
    End Sub

    Private Sub SalesPersonCommisionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSalesPersonCommision.Click
        If GetAdmindbSoftValue("REP_NEWSALESCOMM", "N") = "Y" Then
            Dim obj As New BrighttechREPORT.frmSalesCommission()
            funcShow(obj, "SALES PERSON COMMISION")
        Else
            Dim obj As New BrighttechREPORT.RPT_SALES_COMMISION()
            funcShow(obj, "SALES PERSON COMMISION")
        End If
    End Sub

    Private Sub SalesCommisionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesCommisionToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.FRM_SALES_COMMISION
        funcShow(obj, "SALES COMMISION")
    End Sub

    Private Sub BillnoWiseTransactionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BillnoWiseTransactionToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.RPT_BILLNOWISETRANSACTION
        funcShow(obj, "BILLNO WISE TRANSACTION")
    End Sub

    Private Sub SalesPersonCommisionComToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesPersonCommisionComToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.FRM_SALES_COMMISION_COM()
        funcShow(obj, "SALES PERSON COMMISION")
        'funcShow(FRM_SALES_COMMISION_COM, "SALES PERSON COMMISION")
    End Sub

    Private Sub CardCollectionDetailedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CardCollectionDetailedToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCardCollectionsReport_New()
        funcShow(obj, "CARD COLLECTION DETAILED")
        'funcShow(frmCardCollectionsReport_New, "CARD COLLECTION DETAILED")
    End Sub

    Private Sub OrderAdvanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderAdvanceToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.FRM_ORDERADVANCE()
        funcShow(obj, "ORDER/REPAIR ADVANCE")
        'funcShow(FRM_ORDERADVANCE, "ORDER ADVANCE")
    End Sub

    Private Sub SalesRegisterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesRegisterToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.FRM_SALESREGISTER()
        funcShow(obj, "SALES REGISTER")
        'funcShow(FRM_SALESREGISTER, "SALES REGISTER")
    End Sub

    Private Sub AccOutstandingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim obj As New BrighttechREPORT.FRM_ACC_OUTSTANDING()
        funcShow(obj, "ACCOUNT OUTSTANDING")
        'funcShow(FRM_ACC_OUTSTANDING, "ACCOUNT OUTSTANDING")
    End Sub

    Private Sub TagWiseProfitAnalysisToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagWiseProfitAnalysisToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.FRM_TAGWISEPROFIT()
        funcShow(obj, "TAG WISE PROFIT ANALYSIS")
        'funcShow(FRM_TAGWISEPROFIT, "TAG WISE PROFIT ANALYSIS")
    End Sub

    Private Sub TrayWiseStockCheckReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrayWiseStockCheckReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.FRM_STOCKCHECK_REPORT()
        funcShow(obj, "TRAY WISE STOCK CHECK")
        'funcShow(FRM_STOCKCHECK_REPORT, "TRAY WISE STOCK CHECK")
    End Sub

    Private Sub ItemWiseStockWithApprovalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemWiseStockWithApprovalToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmItemWiseStock_NEW()
        funcShow(obj, "ITEM WISE STOCK")
        'funcShow(frmItemWiseStock_NEW, "ITEM WISE STOCK")
    End Sub

    Private Sub AlloyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlloyToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmAlloyReport()
        funcShow(obj, "ALLOY REPORT")
        'funcShow(frmAlloyReport, "ALLOY REPORT")
    End Sub

    Private Sub WastageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WastageToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmwastagereport()
        funcShow(obj, "WASTAGE REPORT")
        'funcShow(frmwastagereport, "WASTAGE REPORT")
    End Sub

    Private Sub SalesWithDifferenceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesWithDifferenceToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalesDifferenceReport()
        funcShow(obj, "SALES WITH DIFFERENCE")
        'funcShow(frmSalesDifferenceReport, "SALES WITH DIFFERENCE")
    End Sub

    Private Sub DesignerWiseStockToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DesignerWiseStockToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmDesignerWiseStock()
        funcShow(obj, "DESIGNER WISE STOCK")

        'funcShow(frmDesignerWiseStock, "DESIGNER WISE STOCK")
    End Sub

    Private Sub USRateUpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles USRateUpdateToolStripMenuItem.Click
        funcShow(FRM_USRATEUPDATE_BULK, "USRATE BULK UPDATE")
    End Sub

    Private Sub ItemWisePersormanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemWisePersormanceToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmItemSalesPerformance()
        funcShow(obj, "ITEM WISE SALES PERFORMANCE")
        'funcShow(frmItemSalesPerformance, "ITEM WISE SALES PERFORMANCE")
    End Sub

    'Private Sub DD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DD.Click
    '    If MessageBox.Show("Sure, You want to remove?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
    '        Exit Sub
    '    End If
    '    Dim objSecret As New frmAdminPassword()
    '    If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
    '        Exit Sub
    '    End If
    '    Dim rCompanyId As String = objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'EXPORTCOMPANY'") & "'")
    '    Dim OBJ As New BrighttechPack.COMPANY(rCompanyId, cnStockDb, cnAdminDb, cn.ConnectionString, userId)
    '    OBJ.RemoveInfo()
    'End Sub

    Private Sub PriviledgeReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PriviledgeReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmPrevilegetran()
        funcShow(obj, "PREVILEGE REPORT", False)
        'funcShow(frmPrevilegetran, "PREVILEGE REPORT", False)
    End Sub

    Private Sub DateWiseStockToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateWiseStockToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmDateWiseStock()
        funcShow(obj, "DATE WISE STOCK REPORT", False)
        'funcShow(frmDateWiseStock, "DATE WISE STOCK REPORT", False)
    End Sub

    Private Sub PriviledToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PriviledToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmprivilige
        funcShow(obj, "PRIVILEDGE", False)
    End Sub

    Private Sub DealerWastageAddedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DealerWastageAddedToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmWastageAdded
        funcShow(obj, "DEALER WASTAGE ADDED", False)
    End Sub

    Private Sub BillWiseAgeOutstandingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BillWiseAgeOutstandingToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmduetran()
        funcShow(obj, "Bill Wise Age Outstanding", False)
        'funcShow(frmduetran, "Bill Wise Age Outstanding", False)
    End Sub

    Private Sub ToolStripMenuItem11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem11.Click
        Dim obj As New BrighttechREPORT.frmPrevilegeSummary()
        funcShow(obj, "PREVILEGE SUMMARY", False)
        'funcShow(frmPrevilegeSummary, "PRIVILEDGE SUMMARY", False)
    End Sub

    Private Sub SummaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SummaryToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCustomerOutstanding()
        funcShow(obj, "CUSTOMER OUTSTANDING SUMMARY REPORT")
        'funcShow(frmCustomerOutstanding, "CUSTOMER OUTSTANDING SUMMARY REPORT")
    End Sub

    Private Sub DetailedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DetailedToolStripMenuItem.Click
        funcShow(frmNewCustomerOutstanding, "CUSTOMER OUTSTANDING DETAILED REPORT")
    End Sub

    Private Sub DealerVAToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DealerVAToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmDealerWmc
        funcShow(obj, "DEALER VA", False)
    End Sub

    Private Sub ItemNonTagStockCheckToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemNonTagStockCheckToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmItemNonTagCheck()
        funcShow(obj, "ITEM NON TAG STOCK CHECK")
        'funcShow(frmItemNonTagCheck, "ITEM NON TAG STOCK CHECK")
    End Sub

    Private Sub DayBookWithJournalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DayBookWithJournalToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.DayBookWithJournalfrm()
        funcShow(obj, "DAY BOOK WITH JOURNAL")
        'funcShow(DayBookWithJournalfrm, "DAY BOOK WITH JOURNAL")
    End Sub

    Private Sub DayBookToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DayBookToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.DayBookfrm()
        funcShow(obj, "DAY BOOK")
        'funcShow(DayBookfrm, "DAY BOOK")
    End Sub

    Private Sub SalesPurchaseEstimationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesPurchaseEstimationToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalPurchNew()
        funcShow(obj, "SALES & PURCHASE ESTIMATION")
        'funcShow(frmSalPurchNew, "SALES & PURCHASE ESTIMATION")
    End Sub

    Private Sub QualityCheckToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles QualityCheckToolStripMenuItem.Click
    End Sub

    Private Sub TagReservedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagReservedToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.TagReserved()
        funcShow(obj, "TAG RESERVED")
        'funcShow(TagReserved, "TAG RESERVED")
    End Sub

    Private Sub ClosingDenominationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClosingDenominationToolStripMenuItem.Click
        If GetAdmindbSoftValue("ADDL_INFO_DENOM", "N") = "Y" Then
            Dim obj As New BrighttechREPORT.ClosingDenominationAddInfo()
            funcShow(obj, "CLOSING DENOMINATION")
        Else
            Dim obj As New BrighttechREPORT.ClosingDenomination()
            funcShow(obj, "CLOSING DENOMINATION")
        End If
    End Sub

    Private Sub AlertGroupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlertGroupToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmAlertGrpMaster
        funcShow(obj, "ALERT GROUP")
    End Sub

    Private Sub AlertTransactionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlertTransactionToolStripMenuItem.Click
        funcShow(frmAlertTran, "ALERT TRANSACTION")
    End Sub

    Private Sub TriggerCreationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TriggerCreationToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.SMSTRIGGER
        funcShow(obj, "ALERT TRIGGER CREATION")
    End Sub

    Private Sub ReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmTagItemsPurchaseReport()
        funcShow(obj, "TAGED ITEMS PURCHASE REPORT")
    End Sub

    Private Sub DetailedToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DetailedToolStripMenuItem1.Click
        funcShow(frmTagItemsPurchaseReport_Old, "TAGED ITEMS DETAILED PURCHASE REPORT")
    End Sub

    Private Sub MetalWiseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MetalWiseToolStripMenuItem.Click
        funcShow(frmTagItemsPurchaseReport_Detail, "TAGED ITEMS METALWISE PURCHASE REPORT")
    End Sub

    Private Sub WastageWiseSalesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WastageWiseSalesToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmWastagewiseSales()
        funcShow(obj, "WASTAGE WISE SALES REPORT")
        'funcShow(frmWastagewiseSales, "WASTAGE WISE SALES REPORT")
    End Sub

    Private Sub AlertTimerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlertTimerToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmAlertTimerr
        funcShow(obj, "ALERT TIMER")
    End Sub

    Private Sub TargetCounterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TargetCounterToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmTargetCounter
        funcShow(obj, "TARGET COUNTER")
    End Sub

    Private Sub ToolStripMenuItem13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem13.Click
        funcShow(frmTargetCounterReport, "TARGET COUNTERWISE SALES")
    End Sub

    Private Sub ToolStripMenuItem14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem14.Click
        Dim obj As New BrighttechREPORT.DealerSmithLedger()
        funcShow(obj, "DEALER/SMITH LEDGER REPORT")
        'funcShow(DealerSmithLedger, "DEALER/SMITH LEDGER REPORT")
    End Sub

    Private Sub SubItemUpdationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        funcShow(frmSubItemUpdation, "SUBITEM UPDATION")
    End Sub

    Private Sub TagSubItemChangeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagSubItemChangeToolStripMenuItem.Click
        funcShow(frmSubItemUpdation, "TAG SUBITEM CHANGE")
    End Sub

    Private Sub ToolStripMenuItem15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub AcCodewiseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AcCodewiseToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmAdvanceOutstandingReport()
        funcShow(obj, " A/C CODEWISE OUTSTANDING REPORT")
    End Sub



    Private Sub CashCounterCollectionSummaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CashCounterCollectionSummaryToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCashCounterCollection()
        funcShow(obj, " Cash Counter Collection Summary")
        'funcShow(frmCashCounterCollection, "Cash Counter Collection Summary")
    End Sub

    Private Sub CashCounterStockSummaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CashCounterStockSummaryToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCashCounterStock()
        funcShow(obj, " Cash Counter Stock Summary")
        'funcShow(frmCashCounterStock, "Cash Counter Stock Summary")
    End Sub

    Private Sub CashPointReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CashPointReportToolStripMenuItem.Click
        funcShow(frmCashPointRpt, "Cash Point Report")
    End Sub

    Private Sub DiscountReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiscountReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmDiscountReport()
        funcShow(obj, " DISCOUNT REPORT")
        'funcShow(frmDiscountReport, "DISCOUNT REPORT")
    End Sub

    Private Sub SmithBalanceReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim obj As New BrighttechREPORT.frmSmithBalanceRpt()
        funcShow(obj, " SMITH ABSTRACT REPORT")
        'funcShow(frmSmithBalanceRpt, "SMITH ABSTRACT REPORT")
    End Sub

    Private Sub SubsidryLedgerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SubsidryLedgerTStrip.Click
        Dim obj As New BrighttechREPORT.frmSubsidLed()
        funcShow(obj, " SUBSIDY LEDGER")
        'funcShow(frmSubsidLed, "SUBSIDY LEDGER")
    End Sub

    Private Sub TrialRemovalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub


    Private Sub BudgetControlToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BudgetControlToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmBudGetControl
        funcShow(obj, "Budget Control")
    End Sub

    Private Sub ClosingStockCashReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClosingStockCashReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmClosingStock_Category()
        funcShow(obj, "Closing Stock Cash Report")
        'funcShow(frmClosingStock_Category, " Closing Stock Cash Report")
    End Sub

    Private Sub OfferDiscountUpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OfferDiscountUpdateToolStripMenuItem.Click
        funcShow(frmDiscountUpdator, "Offer Discount Update")
    End Sub

    Private Sub DetailRegisterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DetailRegisterToolStripMenuItem.Click
        funcShow(frmTagDetailedRegister, "Tag Detailed Register")
    End Sub
    Private Sub ChequePrintFormatToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChequePrintFormatToolStripMenuItem.Click
        funcShow(frmChqPrintFormat, "Cheque Print Format")
    End Sub

    Private Sub tStripItemGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripItemGroup.Click
        Dim obj As New BrighttechMaster.frmItemGroup
        funcShow(obj, "Item Group ")
    End Sub

    Private Sub SizeWiseStockReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SizeWiseStockReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSizewiseStockReport()
        funcShow(obj, "Size Wise Stock Report ")
        'funcShow(frmSizewiseStockReport, "Size Wise Stock Report ")
    End Sub

    Private Sub BrsOpeningToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BrsOpeningToolStripMenuItem.Click
        funcShow(frmOpenBrs, "BrsOpening")
    End Sub

    Private Sub TagToNontagItemsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagToNontagItemsToolStripMenuItem.Click
        funcShow(frmTagtoNontag, "Tag To Nontag items")
    End Sub

    Private Sub ToolStripMenuItem16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub TableBasedWastageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TableBasedWastageToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmTableBaseWastage()
        funcShow(obj, "Table Based Wastage")
        'funcShow(frmTableBaseWastage, "Table Based Wastage")
    End Sub

    Private Sub ToolStripMenuItem16_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem16.Click
        Dim obj As New BrighttechREPORT.frmSmithBalanceDetailedCategoryWise()
        funcShow(obj, "Smith Balance Detailed Category Wise")
        'funcShow(frmSmithBalanceDetailedCategoryWise, "Smith Balance Detailed Category Wise")
    End Sub

    Private Sub ToolStripMenuItem17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem17.Click
        Dim obj As New BrighttechMaster.frmStockReorder
        funcShow(obj, "STOCK REORDER")
    End Sub

    Private Sub PieceWiseStockReorderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PieceWiseStockReorderToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmPieceWiseStockReOrder
        funcShow(obj, "PIECE WISE STOCK REORDER")
    End Sub

    Private Sub ReorderStockReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReorderStockReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmReorderStock()
        funcShow(obj, "REORDER STOCK")
        'funcShow(frmReorderStock, "REORDER STOCK")
    End Sub

    Private Sub StockReorderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StockReorderToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmReorderStockPlan()
        funcShow(obj, "STOCK REORDER PLAN")
        'funcShow(frmReorderStockPlan, "STOCK REORDER PLAN")
    End Sub

    Private Sub ReorderStockCounterWiseToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReorderStockCounterWiseToolStripMenuItem1.Click
        Dim obj As New BrighttechREPORT.frmCounterWiseStockReorderLevel()
        funcShow(obj, "COUNTER WISE STOCK REORDER")
        'funcShow(frmCounterWiseStockReorderLevel, "COUNTER WISE STOCK REORDER")
    End Sub

    Private Sub ReorderStockRangeWiseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReorderStockRangeWiseToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmReorderRangeWiseStock()
        funcShow(obj, "REORDER RANGE WISE STOCK")
        'funcShow(frmReorderRangeWiseStock, "REORDER RANGE WISE STOCK")
    End Sub

    Private Sub ReorderStockPieceWiseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReorderStockPieceWiseToolStripMenuItem.Click
        funcShow(frmReorderStockPiecewise, "Piece Wise Reorder Stock")
    End Sub

    Private Sub PasswordOptionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasswordOptionToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmPasswordOption()
        funcShow(obj, "Password Options")
    End Sub

    Private Sub PasswordMasterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasswordMasterToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmPasswordMaster
        funcShow(obj, "Password Master")
    End Sub

    Private Sub TagPurchaseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagPurchaseToolStripMenuItem.Click
        funcShow(TagwisePurchaseUpdate, "Purchase Tag Save")
    End Sub

    Private Sub TagSplitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ReportsRoleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReportsRoleToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmReportsrole
        funcShow(obj, "Role - Report")
    End Sub

    Private Sub CToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCreditBalanceReport()
        funcShow(obj, "Credit/Outstanding Summary")
        'funcShow(frmCreditBalanceReport, "Credit/Outstanding Summary")
    End Sub

    Private Sub ToolStripMenuItem15_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem15.Click
        Dim obj As New BrighttechMaster.frmItemWiseMarginLink
        funcShow(obj, "Item Wise Margin Link")
    End Sub

    Private Sub RoleWiseMarginLinkToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RoleWiseMarginLinkToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmRoleWiseMargin
        funcShow(obj, "Role Wise Margin Link")
    End Sub

    Private Sub SubItemWiseSmithLinkToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SubItemWiseSmithLinkToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmSubItemSmithLink
        funcShow(obj, "SubItem Wise Smith Link")
    End Sub

    Private Sub ToolStripMenuItem18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripBillToJnt.Click
        funcShow(frmBillToJnd, "BILLTOJND")
    End Sub

    Private Sub ToolStripMenuItem18_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WS_DealerSmithLedgerToolStripMenuItem.Click
        funcShow(WS_DealerSmithLedger, "Whole Sale Dealer Smith Ledger Report")
    End Sub

    Private Sub HalmarkIssueReceiptToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HalmarkIssueReceiptToolStripMenuItem.Click
        funcShow(frmHallMarkMaterialIssueReceipt, "Hallmark Issue/Receipt")
    End Sub

    Private Sub SalesRegistoryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesRegistoryToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalesRegister()
        funcShow(obj, "Sales Register")
        'funcShow(frmSalesRegister, "Sales Register")
    End Sub

    Private Sub PurchaseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripfrmPurchaseMakeWise.Click
        Dim obj As New BrighttechREPORT.frmPurchaseMakeWise()
        funcShow(obj, "Purchase MakeWise")
        'funcShow(frmPurchaseMakeWise, "Purchase MakeWise")
    End Sub


    Private Sub OrderEstimateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripOrderEstimate.Click
        If Not CheckGoldRate() Then Exit Sub
        funcShow(frmOrderEstimate, "Order Estimate")
    End Sub

    Private Sub VouGenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VouGenToolStripMenuItem.Click
        funcShow(frmGvgenerate, "Gift Voucher Generation")
    End Sub

    Private Sub VoucherDistributionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VouDistrToolStripMenuItem.Click
        Dim frmgvissue As New frmAddressDia("Y")
        funcShow(frmgvissue, "Gift Voucher Issue")
    End Sub

    Private Sub GiftTransferToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GiftTransferToolStripMenuItem.Click
        funcShow(frmGiftTransfer, "Gift Transfer")
    End Sub

    Private Sub CounterwiseStockDetailedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CounterwiseStockDetailedToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCounterwiseStockDetail()
        funcShow(obj, "Counter Wise Stock Detail")
        'funcShow(frmCounterwiseStockDetail, "Counter Wise Stock Detail")
    End Sub

    Private Sub tStripGiftTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub OrderAdvPurityConversionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderAdvPurityConversionToolStripMenuItem.Click
        funcShow(frmorderpurityconversion, "Order Adv. Weight Purity Conversion")
    End Sub

    Private Sub RangeMasterToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RangeMasterToolStripMenuItem1.Click
        Dim obj As New BrighttechMaster.frmRangeMaster
        funcShow(obj, "RANGE MASTER")
    End Sub

    Private Sub AgewiseRangeMasterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AgewiseRangeMasterToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmAgewisestockmaster
        funcShow(obj, "AGEWISE RANGE MASTER")
    End Sub

    Private Sub ToolStripMenuItem18_Click_2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem18.Click

    End Sub

    Private Sub ItemWiseDiscountLockToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'funcShow(ItemwiseDiscountLock, "ITEM WISE DISCOUNT LOCK")
    End Sub

    Private Sub LotRegenerateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LotRegenerateToolStripMenuItem.Click
        funcShow(LotMerge, "LotMerge")
    End Sub

    Private Sub ValueAddedUpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ValueAddedUpdateToolStripMenuItem.Click
        funcShow(frmTagUpdator, "Value AddedUpdate")
    End Sub

    Private Sub OrderMergeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderMergeToolStripMenuItem.Click
        funcShow(frmOrderMerge, "Order Merge")
    End Sub

    Private Sub ToolStripMenuItem19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem19.Click
        funcShow(frmMiscChargeUpdate, "Misc Charge Update")
    End Sub

    Private Sub TagSplitToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagSplitToolStripMenuItem.Click
        funcShow(TagSplit, "Tag Split")
    End Sub

    Private Sub TagMergeInterchangeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagMergeInterchangeToolStripMenuItem.Click
        funcShow(TagMerge, "TAG MERGE / INTERCHANGE")
    End Sub

    Private Sub tStripBankconciliation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripBankconciliation.Click
        funcShow(frmBankReconciliation, "BANK RECONCILIATION")
    End Sub

    Private Sub BrsExcelDownloadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BrsExcelDownloadToolStripMenuItem.Click
        funcShow(frmBrsExcelDownload, "BRS EXCELDOWNLOAD")
    End Sub

    Private Sub ReceiptTagPostingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReceiptTagPostingToolStripMenuItem.Click
        funcShow(ReceiptTagPosting, "RECEIPT TAG POSTING")
    End Sub

    Private Sub ReSendDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReSendDataToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.ReSendTransaction
        funcShow(obj, " GENERATE DATA TO RE SEND")
    End Sub

    Private Sub ReSendDataAdvanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReSendDataAdvanceToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.ReSendMaster
        funcShow(obj, "RE SEND DATA-ADVANCED")
    End Sub

    Private Sub MaintenanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MaintenanceToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.Maintenance
        funcShow(obj, "MAINTANANCE")
    End Sub

    Private Sub ToolStripMenuItem20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem20.Click
        Dim obj As New BrighttechMaster.frmSubItemDesignerLink
        funcShow(obj, "DESIGNER WISE STUDDED STONE")
    End Sub

    Private Sub AdvanceRateFixingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdvanceRateFixingToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmAdvanceRateFixing
        funcShow(obj, "ADVANCE RATE FIXING")
    End Sub

    Private Sub TagTypeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagTypeToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmTagType
        funcShow(obj, "ITEM TYPE")
    End Sub

    Private Sub PurchaseItemTypeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PurchaseItemTypeToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmPurItemType
        funcShow(obj, "PURCHASE ITEM TYPE")
    End Sub

    Private Sub TcsCollectToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TcsCollectToolStripMenuItem1.Click
        Dim obj As New BrighttechREPORT.frmTcsReport
        funcShow(obj, "TCS COLLECT")
    End Sub

    Private Sub SchemeOfferToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SchemeOfferToolStripMenuItem.Click
        'funcShow(frmSchemeOffer, "SCHEME OFFER")
        Dim obj As New BrighttechMaster.frmSchemeOfferNew
        funcShow(obj, "SCHEME OFFER")
    End Sub


    Private Sub tStripDiscount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripDiscount.Click
        Dim obj As New BrighttechMaster.frmDiscount
        funcShow(obj, "DISCOUNT")
    End Sub

    Private Sub CategoryWisePurchaseDiscountToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CategoryWisePurchaseDiscountToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmCategoryWiseDiscount
        funcShow(obj, "CategoryWisePurchaseDiscount")
    End Sub

    Private Sub SchemeOfferRangeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSchemeOfferRange.Click
        Dim obj As New BrighttechMaster.frmSchemeOfferRange
        funcShow(obj, "SCHEME OFFER RANGE")
    End Sub

    Private Sub InterestAnalysisToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InterestAnalysisToolStripMenuItem.Click
        Dim obj As New InterestAnalysys
        funcShow(obj, "INTEREST ANALYSIS")
    End Sub

    Private Sub GiftVoucherDetailToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GiftVoucherDetailToolStripMenuItem.Click
        funcShow(frmGiftReport, "Gift Voucher Detail")
    End Sub

    Private Sub SalesPurchaseAbstractToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesPurchaseAbstractToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalesAbstractNew()
        funcShow(obj, "Tally Export")
    End Sub

    Private Sub CategorywisePurchaseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CategorywisePurchaseTStrip.Click
        Dim obj As New BrighttechREPORT.frmCategoryWisePurchase
        funcShow(obj, "Purchase Categorywise")
    End Sub

    Private Sub JobNowiToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles JobNowiTStrip.Click
        Dim obj As New BrighttechREPORT.frmJobnoWiseReport
        funcShow(obj, "JobNoWise report")
    End Sub

    Private Sub SalesPerformanceAnalysisToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesPerformanceAnalysisTStrip.Click
        Dim obj As New BrighttechREPORT.frmSalesPerformanceAnalysis
        funcShow(obj, "Sales Performance Analysis report")
    End Sub

    Private Sub GiftVoucherAgainstSalesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GiftVoucherAgainstSalesToolStripMenuItem.Click
        funcShow(frmGiftGenerate, "Gift Voucher Against Sales")
    End Sub

    Private Sub ReorderStockPlanNewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReorderStockPlanNewToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmReorderStockPlan_NEW
        funcShow(obj, "Stock Reorder Plan New ")
    End Sub

    Private Sub tStripPurTaxPost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripPurTaxPost.Click
        funcShow(frmPutaxpost, "PURCHASE TAX POSTING")

    End Sub


    Private Sub CtrReturnToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '        funcShow(frmReprocess, "COUNTER RETURN PROCESS")
    End Sub

    Private Sub DailyActivityReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DailyActivityReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.DailyActivityReport
        funcShow(obj, "DAILY ACTIVITY REPORT")
    End Sub

    'Private Sub MetalOrnamentStockViewDetailedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MetalOrnamentStockViewDetailedTStrip.Click
    '    Dim obj As New BrighttechREPORT.frmMetalOrnamentDetailedStockView_New
    '    funcShow(obj, "METAL ORNAMENT STOCK VIEW (DETAILED)")
    'End Sub

    Private Sub TagTypeReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagTypeReportTStrip.Click
        Dim obj As New BrighttechREPORT.frmTagEntrytypeReport
        funcShow(obj, "Tag Entry Typewise Breakup")
    End Sub

    Private Sub SmithStatusReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SmithStatusReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSmithStatus
        funcShow(obj, "MISC ISSUE REASONWISE REPORT")
    End Sub

    Private Sub TagItemUpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagItemUpdateToolStripMenuItem.Click
        funcShow(frmItemUpdation, "TAG ITEM CHANGE")
    End Sub

    Private Sub TablewiseMarginLockTlStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'funcShow(TablewiseMarginLock, "TABLEWISE MARGIN")
    End Sub

    Private Sub AdvCrtTrferTStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdvCrtTrferTStrip.Click
        funcShow(frmOutstanding_Transfer, "ADVANCE/CREDIT TRANSFER TO COSTCENTRE")
    End Sub

    Private Sub LedgerWithWeightToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripLedgerWithWeight.Click
        funcShow(frmAccountsLedger_Weight, "LEDGER WITH WEIGHT")
    End Sub

    Private Sub BudgetAnalysisToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BudgetAnalysisToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmBudgetAnalysis
        funcShow(obj, "BUDGET ANALYSIS")
    End Sub

    Private Sub ReceiptLotTagToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReceiptLotTagToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmReceiptLotView
        funcShow(obj, "MATERIAL RECEIPT STATUS VIEW")
    End Sub

    Private Sub BillnowiseSalesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BillnowiseSalesToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalesBillnowise
        funcShow(obj, "Customerwise Sales")
    End Sub

    Private Sub Company0ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Company0ToolStripMenuItem.Click

    End Sub

    Private Sub CompanystatusToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompanystatusToolStripMenuItem.Click
        frmCompany.Statuschange = True

        frmCompany.ShowDialog()
    End Sub

    Private Sub ReSendMasterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReSendMasterToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.ReSend_Master
        funcShow(obj, "Re Send Master")
    End Sub
    Private Sub funcGlobalDate()
        GlobalDate = GetAdmindbSoftValue("GLOBALDATE").ToUpper
        GlobalDateweb = GetAdmindbSoftValue("GLOBALDATE_WEB", "N").ToUpper
        DateReset = GetAdmindbSoftValue("GLOBALDATECHANGETIME").ToUpper()
        Rpt_Send_Time = GetAdmindbSoftValue("RPT_MAIL_TIME").ToUpper()
        RATEWARNTIME = GetAdmindbSoftValue("RATEWARNTIME").ToUpper()
        If DateReset <> "" Then GlobalDateTimer.Enabled = True
        If Rpt_Send_Time <> "" Then GlobalDateTimer.Enabled = True
    End Sub

    Private Sub GlobalDateTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GlobalDateTimer.Tick
        Try
            If GlobalDate = "Y" Then
                If DateReset <> "" Then
                    If GetServerTime.Hour & ":" & GetServerTime.Minute = DateReset Then
                        Dim Updated As Date
                        Dim Getdatevalue As Date = GetEntryDate(GetServerDate)
                        Dim obj_SendMail As New rptSendMail
                        tran = Nothing
                        tran = cn.BeginTransaction()
                        If GlobalDateweb <> "Y" Then
                            Dim NewGlobaldate As Date
                            Updated = objGPack.GetSqlValue("SELECT UPDATED FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'GLOBALDATEVAL'", , , tran)
                            If GetServerDate(tran) = Format(Updated, "yyyy-MM-dd") Then Exit Sub
                            NewGlobaldate = GetAdmindbSoftValue("GLOBALDATEVAL", , tran)
                            NewGlobaldate = NewGlobaldate.AddDays(1)
                            strSql = "UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & Format(NewGlobaldate, "yyyy-MM-dd") & "',UPDATED='" & GetServerDate(tran) & "'"
                            strSql += " WHERE CTLID = 'GLOBALDATEVAL'"
                            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()
                        End If
                        strSql = "  IF NOT (SELECT COUNT(*) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = '" & Getdatevalue.ToString("yyyy-MM-dd") & "')>0"
                        strSql += "  BEGIN"
                        strSql += "  INSERT INTO " & cnAdminDb & "..RATEMAST(METALID,RDATE,RATEGROUP,PURITY,SRATE,PRATE,USERID,UPDATED,UPTIME)"
                        strSql += "  SELECT METALID,'" & Getdatevalue.ToString("yyyy-MM-dd") & "',(SELECT ISNULL(MAX(RATEGROUP),0)+1 FROM " & cnAdminDb & "..RATEMAST) AS RATEGROUP,PURITY,SRATE,PRATE,USERID,UPDATED,UPTIME "
                        strSql += "  FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = (sELECT MAX(RDATE) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE <= '" & Getdatevalue.ToString("yyyy-MM-dd") & "' AND  RDATE BETWEEN '" & cnTranFromDate.ToString("yyyy-MM-dd") & "' AND '" & cnTranToDate.ToString("yyyy-MM-dd") & "')"
                        strSql += "  AND RATEGROUP = (sELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = (sELECT MAX(RDATE) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE <= '" & Getdatevalue.ToString("yyyy-MM-dd") & "' AND RDATE BETWEEN '" & cnTranFromDate.ToString("yyyy-MM-dd") & "' AND '" & cnTranToDate.ToString("yyyy-MM-dd") & "'))"
                        strSql += "  END"
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()
                        tran.Commit()
                        tran = Nothing
                        Global_Date_Rate()
                        Dim MAILRPT_DAILY As Boolean = IIf(GetAdmindbSoftValue("MAILRPT_DAILYTRAN", "N") = "Y", True, False)
                        Dim NewMaildate As Date
                        NewMaildate = GetAdmindbSoftValue("GLOBALDATEVAL")
                        Dim rptMailDate As String = ""
                        rptMailDate = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'RPT_MAIL_DATE'", , , tran)
                        If MAILRPT_DAILY And rptMailDate <> Format(NewMaildate, "yyyy-MM-dd") Then
                            strSql = "UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & Format(NewMaildate, "yyyy-MM-dd") & "'"
                            strSql += " WHERE CTLID = 'RPT_MAIL_DATE'"
                            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()
                            obj_SendMail.SendAutoMail_Rpt()
                        End If
                        'MsgBox("Global Bill date has been changed..." & vbCrLf & "Please Re-Loading the application to refresh", MsgBoxStyle.Information)
                    End If
                End If
                If Rpt_Send_Time <> "" Then
                    If GetServerTime.Hour & ":" & GetServerTime.Minute = Rpt_Send_Time Then
                        Dim Updated As Date
                        Dim obj_SendMail As New rptSendMail
                        Dim MAILRPT_DAILY As Boolean = IIf(GetAdmindbSoftValue("MAILRPT_DAILYTRAN", "N") = "Y", True, False)
                        Dim rptMailDate As String = ""
                        rptMailDate = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'RPT_MAIL_DATE'", , , tran)
                        Dim NewMaildate As Date
                        NewMaildate = GetAdmindbSoftValue("GLOBALDATEVAL")
                        If MAILRPT_DAILY And rptMailDate <> NewMaildate And rptMailDate.ToString <> "" And NewMaildate.ToString <> "" Then
                            Dim TrptDate As Date = Convert.ToDateTime(rptMailDate)
                            obj_SendMail.SendAutoMail_Rpt(TrptDate)
                            strSql = "UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & Format(NewMaildate, "yyyy-MM-dd") & "'"
                            strSql += " WHERE CTLID = 'RPT_MAIL_DATE'"
                            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()

                        End If
                    End If
                End If
                If RATEWARNTIME <> "" Then
                    If Ratewarnupd = False Then
                        Dim RATEWARNTIMES() As String = RATEWARNTIME.Split(",")
                        For WTIME As Integer = 0 To RATEWARNTIMES.Length - 1
                            If GetServerTime.Hour & ":" & GetServerTime.Minute = RATEWARNTIMES(WTIME) Then
                                If BrighttechPack.Methods.GetRights(_DtUserRights, frmRateMaster.Name, BrighttechPack.Methods.RightMode.Save) Then
                                    Ratewarnupd = True
                                    MsgBox("Please enter current Rate.", MsgBoxStyle.Information)
                                    Exit For
                                End If
                            Else
                                Ratewarnupd = False
                            End If

                        Next
                    End If
                End If
                If tStripBillDate.Tag Is Nothing Then Exit Sub
                Dim Rdate As Date = GetEntryDate(GetServerDate)
                Dim mrategroup As Integer = Val(objGPack.GetSqlValue("SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST  WHERE RDATE = '" & Rdate.ToString("yyyy-MM-dd") & "'").ToString)
                If tStripBillDate.Tag <> mrategroup Then Global_Date_Rate()
            End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub

    Private Sub ChargesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChargesToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmOtherCharges
        funcShow(obj, "OTHER CHARGES")
    End Sub

    Private Sub AcctwiseChargesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AcctwiseChargesToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmAcOthCharge
        funcShow(obj, "OTHER CHARGES RANGEWISE")
    End Sub

    Private Sub TagSaleModeUpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagSaleModeUpdateToolStripMenuItem.Click
        funcShow(FrmTagDetailSaleModeUpdate, "SALE MODE UPDATE")
    End Sub

    Private Sub AuditReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AuditReportToolStripMenuItem.Click
        Dim AuditLog As Boolean = IIf(GetAdmindbSoftValue("AUDITLOG", "N") = "Y", True, False)
        If Not AuditLog Then MsgBox("Application setup does not support to audit log", MsgBoxStyle.Exclamation) : Exit Sub
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " select name from master..sysdatabases where name = '" & cnCompanyId & "AUDITLOG'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        da.Fill(ds, "sysDatabases")
        If ds.Tables("sysDatabases").Rows.Count = 0 Then MsgBox("AUDITLOG DATABASE NOT FOUND" & vbCrLf & "PLEASE RUN DBCREATOR ", MsgBoxStyle.Information) : Exit Sub
        Dim obj As New BrighttechREPORT.FrmAuditReport
        funcShow(obj, "Audit Log")
    End Sub

    Private Sub ClosingStockToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClosingStockToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmClosingStock
        funcShow(obj, "Closing Stock")
    End Sub

    Private Sub OrderDeliveryReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderDeliveryReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmOrderDeliveredReport
        funcShow(obj, "Order/Repair Delivery Report ")
    End Sub

    Private Sub AdvanceLockUnlockToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdvanceLockUnlockToolStripMenuItem.Click
        funcShow(frmAdvanceLock, "Advance Lock/Unlock")
    End Sub

    Private Sub StnCutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StnCutToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmStnCut
        funcShow(obj, "Stone Cut")
    End Sub

    Private Sub StoneColorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StoneColorToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmStnColor
        funcShow(obj, "Stone Color")
    End Sub

    Private Sub StoneClarityToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StoneClarityToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmStnClarity
        funcShow(obj, "Stone Clarity")
    End Sub

    Private Sub ApprovalInterchangeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApprovalInterchangeToolStripMenuItem.Click
        funcShow(frmApprovalInterChange, "Approval InterChange")
    End Sub

    Private Sub ChequeReturnToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChequeReturnToolStripMenuItem.Click
        funcShow(frmChequeReturns, "Cheque Return")
    End Sub

    Private Sub CounterStockDiaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CounterStockDiaryToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCounterStockDiary
        funcShow(obj, "Counter Stock Diary")
    End Sub

    Private Sub PurchaseUploadExcelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PurchaseUploadExcelToolStripMenuItem.Click
        Dim SoftCtrl As String = GetAdmindbSoftValue("TAGXLUPLOAD_MAN_AUTO", "A")
        If SoftCtrl = "A" Then  'KHAWASISH DIAMONDS
            funcShow(frmPurchaseExcel, "Purchase Upload")
        ElseIf SoftCtrl = "J" Then 'J.J.DIAMONDS
            funcShow(frmPurchaseExcel, "Purchase Upload")
        ElseIf SoftCtrl = "P" Then 'PAYAL JEWELLERS
            funcShow(frmPurchaseExcel, "Purchase Upload")
        ElseIf SoftCtrl = "M" Then 'DAR 
            funcShow(frmPurchaseExcell, "Manual Tag Upload")
        ElseIf SoftCtrl = "N" Then 'SPENCER
            funcShow(frmTagExcelDown, "Manual Tag Upload")
        End If
    End Sub

    Private Sub StoneSettingTypeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StoneSettingTypeToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmStnSettingType
        funcShow(obj, "Stone Setting Type")
    End Sub

    Private Sub DiaStyleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiaStyleToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmDiaStyle
        funcShow(obj, "Dia Style")
    End Sub

    Private Sub StoneShapeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StoneShapeToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmStnShape
        funcShow(obj, "Stone Shape")
    End Sub

    Private Sub NonTagTransferToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NonTagTransferToolStripMenuItem.Click
        funcShow(frmNonTagTransfer, "NonTag Transfer")
    End Sub

    Private Sub StockRegisterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StockRegisterToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmStockRegister
        funcShow(obj, "Stock Register")
    End Sub

    Private Sub SyncStatusCheckingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SyncStatusCheckingToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSyncStatusChecking
        funcShow(obj, "Sync Status Checking")
    End Sub

    Private Sub CategoryGroupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CategoryGroupToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmCategoryGroup
        funcShow(obj, "Category Group")
    End Sub

    Private Sub CollectVsTransactionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CollectVsTransactionToolStripMenuItem.Click
        funcShow(New frmDataChecking(frmDataChecking.Type.Collect), "SAVINGS VS ACCOUNTS DATA RECONZILATION")
    End Sub

    Private Sub BackendIncentiveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackendIncentiveToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.MonthwiseIncentiveMaster("B")
        funcShow(obj, "Backend Incentive")
    End Sub

    Private Sub MonthwiseIncentiveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MonthwiseIncentiveToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.MonthwiseIncentiveMaster("M")
        funcShow(obj, "Monthwise Incentive")
    End Sub

    Private Sub BillnoWiseTransactioSummaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BillnoWiseTransactioSummaryToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmBillnowiseTransactionSummary
        funcShow(obj, "Billno wise transaction Summary")
    End Sub

    Private Sub UserCashCounterLinkToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UserCashCounterLinkToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmUserCash
        funcShow(obj, "User Cashcounter Link")
    End Sub

    Private Sub RapaportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RapaportToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmRapaport
        funcShow(obj, "Rapaport")
    End Sub

    Private Sub SalesAbstractDetailedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesAbstractDetailedToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalesAbsWithReturn
        funcShow(obj, "Sales Abstract Detailed")
    End Sub

    Private Sub OutstandigEntryFromLedgerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutstandigEntryFromLedgerToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmAcctoOutstanding
        funcShow(obj, "Outstanding Entry from Ledger")
    End Sub

    'Private Sub TStripMarkUnmark_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TStripMarkUnmark.Click
    '    Dim obj As New BrighttechREPORT.frmMarkUnmark
    '    funcShow(obj, "Mark/Unmark Booked Item Report")
    'End Sub

    Private Sub TDSReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TDSReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmStaxTdsReport
        funcShow(obj, "TDS/SERVICE TAX Report")
        'funcShow(frmTdsReport, "Tds Report")
    End Sub

    Private Sub tStripTrailBalance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tStripTrailBalance.Click
        funcShow(frmTrailBal, "Trail Balance")
    End Sub

    Private Sub TStripMaterialIntransist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TStripMaterialIntransist.Click
        If cnCostId = cnHOCostId Or GetAdmindbSoftValue("MFG_RECEIPT", "N") = "Y" Then
            Dim obj As New BrighttechMaster.frmPendingMR
            funcShow(obj, "MATERIAL IN TRANSIT")
        End If
    End Sub

    Private Sub ToolStripMenuItem21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem21.Click
        funcShow(FrmDailyAuditReport, "Daily Audit Report")
    End Sub

    Private Sub ExemptionReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExemptionReportToolStripMenuItem.Click
        Dim Obj As New BrighttechREPORT.frmExemptionReport
        funcShow(Obj, "Exemption Report")
    End Sub

    Private Sub TStripMenuItemPurifyIssue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TStripMenuItemPurifyIssue.Click
        funcShow(frmPurifyIssue, "PURIFICATION ISSUE")
    End Sub

    Private Sub TStripMenuItemPurifyRec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TStripMenuItemPurifyRec.Click
        funcShow(frmPurifyrec, "PURIFICATION RECEIPT")
    End Sub

    Private Sub MaterialReceiptVsStockToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MaterialReceiptVsStockToolStripMenuItem.Click
        Dim Obj As New BrighttechREPORT.frmMaterialReceiptVsStock
        funcShow(Obj, "Material Receipt Vs Stock")
    End Sub

    Private Sub CustomerBillSummaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomerBillSummaryToolStripMenuItem.Click
        Dim Obj As New BrighttechREPORT.frmCustomerBills
        funcShow(Obj, "Customer Bill Summary")
    End Sub

    Private Sub CreditCardSettlementToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreditCardSettlementToolStripMenuItem.Click
        funcShow(frmCreditcardSettlement, "Credit Card Settlement")
    End Sub

    Private Sub NontagConstcentreTransferToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NontagConstcentreTransferToolStripMenuItem.Click
        funcShow(frmNonTagCostcentreChange, "NONTAG COSTCENTRE TRANSFER")
    End Sub

    Private Sub AmountWiseSalesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AmountWiseSalesToolStripMenuItem.Click
        Dim Obj As New BrighttechREPORT.frmAmountWiseBillDetails
        funcShow(Obj, "AMOUNT WISE SALES")
    End Sub

    Private Sub OtherMasterEntryToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OtherMasterEntryToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmOtherMasterEntry
        funcShow(obj, "ADDITIONAL MASTER GROUP")
    End Sub

    Private Sub LOTVSTAGToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LOTVSTAGToolStripMenuItem.Click
        Dim Obj As New BrighttechREPORT.frmTaggingReport
        funcShow(Obj, "LOT VS TAG SUMMARY")
    End Sub

    Private Sub SalesSummaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesSummaryToolStripMenuItem.Click
        Dim Obj As New BrighttechREPORT.frmAvgSalesReport
        funcShow(Obj, "SALES SUMMARY REPORT")
    End Sub

    Private Sub CounterWiseSalesSummaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CounterWiseSalesSummaryToolStripMenuItem.Click
        Dim Obj As New BrighttechREPORT.frmCounterWiseSalesReport
        funcShow(Obj, "COUNTERWISE SALES SUMMARY REPORT")
    End Sub
    Private Sub tStripPurchaseVatRpt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripPurchaseVatRpt.Click
        Dim Obj As New BrighttechREPORT.PurchaseVatReportNew
        funcShow(Obj, "Purchase VAT Report")
    End Sub

    Private Sub PurchaseVatAnnexureIToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PurchaseVatAnnexureIToolStripMenuItem.Click
        Dim Obj As New BrighttechREPORT.frmpurchasevatannexture1
        funcShow(Obj, "Purchase VAT Annexure I")
    End Sub

    Private Sub MRMIAccountsRegenerateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MRMIAccountsRegenerateToolStripMenuItem.Click
        funcShow(frmRearrangeTranNo, "MR,MI,ACCOUNTS REGENERATE TRANNO")
    End Sub

    Private Sub RequestionSlipToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RequestionSlipToolStripMenuItem.Click
        funcShow(frmRequestionSlip, "REQUESTION SLIP")
    End Sub

    Private Sub DocumentSendToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DocumentSendToolStripMenuItem.Click
        Dim obj As New frmdocumentupdation("S")
        funcShow(obj, "Document Send")
    End Sub

    Private Sub DocumentReceiveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DocumentReceiveToolStripMenuItem.Click
        Dim obj As New frmdocumentupdation("R")
        funcShow(obj, "Document Receipt")
    End Sub

    Private Sub CashBookToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CashBookToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCashBook
        funcShow(obj, "CASH BOOK")
    End Sub

    Private Sub InternalTransferReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InternalTransferReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmInternalTransferRpt
        funcShow(obj, "INTERNAL TRANSFER REPORT")
    End Sub

    Private Sub StockCheckingReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StockCheckingReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmStockcheckreport
        funcShow(obj, "STOCK CHECKING REPORT")
    End Sub

    Private Sub OrderDetailedReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderDetailedReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmOrderDetailedReport
        funcShow(obj, "ORDER/REPAIR DETAILED REPORT")
    End Sub

    Private Sub ToolStripMenuItem22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem22.Click
        funcShow(RegularToOrderTag, "REGULAR TAG TO ORDER TAG")
    End Sub

    Private Sub RecallTransferedTagsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecallTransferedTagsToolStripMenuItem.Click
        funcShow(frmRecallTransferTag, "RECALL TRANSFER TAG")
    End Sub

    Private Sub DrsMarkingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DrsMarkingToolStripMenuItem.Click
        funcShow(frmDrsMarking, "DRS MARKING")
    End Sub

    Private Sub tStripDataCheckToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tStripDataCheckToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmDataCheck
        funcShow(obj, "DATA CONSISTENT CHECKING")
    End Sub

    Private Sub OtherBranchAdvSchemeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OtherBranchAdvSchemeToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmOtherbranchadvanceadjustment
        funcShow(obj, "OTHER BRANCH ADV/SCHEME RECONCILIATION")
    End Sub

    Private Sub AutoBrsMasterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoBrsMasterToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmAutoBrsMast
        funcShow(obj, "AUTO BRS MASTER")
    End Sub

    Private Sub TagnoRegenerateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagnoRegenerateToolStripMenuItem.Click
        funcShow(frmRegenerateTagno, "REGENERATE TAGNO")
    End Sub

    Private Sub VATFormIToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VATFormIToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmVatFormI
        funcShow(obj, "VAT FORM I ")
    End Sub

    Private Sub UserDiscCrAuthorizeTStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UserDiscCrAuthorizeTStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmUserAuthorize
        funcShow(obj, "USERWISE DISCOUNT/DUE AUTHORIZE")
    End Sub

    Private Sub tStripAcctSchedule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripAcctSchedule.Click
        Dim obj As New BrighttechMaster.frmAccgroupschedule
        funcShow(obj, "Account Schedule No Update")
    End Sub

    Private Sub AnnexurePartBToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AnnexurePartBToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmAnnexure
        funcShow(obj, "ANNEXURE (PART B)")
    End Sub

    Private Sub TradingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TradingToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCatTrading
        funcShow(obj, "CATEGORY WISE TRADING ACCOUNT")
    End Sub

    Private Sub tStripPMGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripPMGroup.Click
        Dim obj As New BrighttechMaster.frmPMGroup
        funcShow(obj, "Compliment Group")
    End Sub

    Private Sub tstripPMItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tstripPMItem.Click
        Dim obj As New BrighttechMaster.frmPMMast
        funcShow(obj, "Compliment Items")
    End Sub

    Private Sub tstripPMSubItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tstripPMSubItem.Click
        Dim obj As New BrighttechMaster.frmPMSubMast
        funcShow(obj, "Compliment SubItems")
    End Sub

    Private Sub tstripPMRangeControl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tstripPMRangeControl.Click
        Dim obj As New BrighttechMaster.frmPMValidation
        funcShow(obj, "Compliment validation")
    End Sub

    Private Sub tStripCompliments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCompliments.Click
        If GetAdmindbSoftValue("COMPLIMENT_NONTAG", "Y") = "Y" Then
            funcShow(frmCompIssue, "Compliment/Packing material Issue")
        Else
            funcShow(frmPMGenerateIssue, "Compliment/Packing material Issue")
        End If
    End Sub

    Private Sub JournalSingleEntryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SingleJournalEntryToolStripMenuItem.Click
        funcShow(frmSingleAccountsEnttry, "Single Journal Entry")
    End Sub

    Private Sub tStripSendSmsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSendSmsToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSendSms
        funcShow(obj, "SEND SMS")
    End Sub

    Private Sub SmsTemplateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SmsTemplateToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSmsTemplate
        funcShow(obj, "SMS TEMPLATE")
    End Sub

    Private Sub ReorderStockReportNewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReorderStockReportNewToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmReorder_new
        funcShow(obj, "REORDER STOCK REPORT")
    End Sub

    Private Sub tStripTagEntry_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tStripTagEntry.Click
        funcShow(frmWebTag, "Web tag")
    End Sub

    Private Sub tStripTagView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripTagView.Click
        funcShow(WTagedView, "Web tag View")
    End Sub

    Private Sub SyncReceivedStatusToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SyncReceivedStatusToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSyncReceivedStatus
        funcShow(obj, "SYNC RECEIVED STATUS")
    End Sub

    Private Sub EChellanToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EChellanToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmEchallan
        funcShow(obj, "E-CHELLAN")
    End Sub

    Private Sub ItemwiseStockMRMIToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemwiseStockMRMIToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSmithStockSummaryReport()
        funcShow(obj, "ITEMWISE STOCK REPORT(MR/MI)")
    End Sub

    Private Sub MeltingDetailReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MeltingDetailReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmMeltingDetailReport()
        funcShow(obj, "MELTING DETAIL REPORT")
    End Sub
    Private Sub tStripCustomerQueryMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCustomerQueryMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCustomerFeedback()
        funcShow(obj, "CUSTOMER QUERY/FEEDBACK")
    End Sub

    Private Sub tStripCustomerTransactionMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripCustomerTransactionMenuItem.Click
        Dim obj As New BrighttechREPORT.FRM_CUSTOMER_RECORD
        funcShow(obj, "CUSTOMER TRANSACTION FLOW")
    End Sub

    Private Sub TagSplitCancelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagSplitCancelToolStripMenuItem.Click
        funcShow(frmTagSplitCancel, "CANCEL SPLITTED TAG")
    End Sub

    Private Sub ItemPostToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemPostToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmItemMastePosting
        funcShow(obj, "ITEM MASTER EXPORT")
    End Sub

    Private Sub BookVsCounterToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BookVsCounterToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmBookVsCounterStockReport()
        funcShow(obj, "BOOK STOCK VS COUNTER STOCK")
    End Sub

    Private Sub EditViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditViewToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmrptTagEdit()
        funcShow(obj, "TAGGED ITEMS EDIT VIEW")
    End Sub

    Private Sub TagPacketNoGenerationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagPacketNoGenerationToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmPktNoGen()
        funcShow(obj, "PACKET NUMBER GENERATION")
    End Sub

    Private Sub DiamondSalesReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiamondSalesReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmDiaSalesRpt()
        funcShow(obj, "DIAMOND SALES REPORT")
    End Sub

    Private Sub TagNoChangeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagNoChangeToolStripMenuItem.Click
        funcShow(frmTagnoUpdation, "TAGNO CHANGE")
    End Sub

    Private Sub VaultReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VaultReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmVaultReport()
        funcShow(obj, "VAULT REPORT")
    End Sub

    Private Sub ComplementsToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComplementsToolStripMenuItem1.Click
        Dim obj As New BrighttechREPORT.frmcomplements
        funcShow(obj, "COMPLEMENTS REPORT")
    End Sub

    Private Sub OldGoldReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OldGoldReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmOGRpt
        funcShow(obj, "OLD GOLD REPORT")
    End Sub
    Private Sub AnnexurePartAToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AnnexurePartAToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmAnnexureFormA
        funcShow(obj, "ANNEXURE (PART A)")
    End Sub

    Private Sub CertificateChargesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CertificateChargesToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmCertificationCharges
        funcShow(obj, "CERTIFICATION CHARGES")
    End Sub

    Private Sub OpenPrevilegeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenPrevilegeToolStripMenuItem.Click
        funcShow(frmPrevilege, "OPEN PREVILEGE")
    End Sub

    Private Sub MonthWiseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MonthWiseToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCustomerOutstandingMonth
        funcShow(obj, "Customer Outstanding Month Wise")
    End Sub

    Private Sub CReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frm4CReport
        funcShow(obj, "4CReport")
    End Sub
    Private Sub TagTransferStockReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagTransferStockReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmTagTransferStockReport
        funcShow(obj, "TAG TRANSFER STOCK REPORT")
    End Sub
    Private Sub DesignerVAToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DesignerVAToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmDesignerVA
        funcShow(obj, "Designer Value Added")
    End Sub

    Private Sub AddressToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddressToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmAddress
        funcShow(obj, "ADDRESS MASTER")
    End Sub

    Private Sub WholesaleEstimateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WholesaleEstimateToolStripMenuItem.Click
        Dim CostId As String = Nothing
        Dim CashId As String = Nothing

        Dim objEst As New frmEstimation3
        objEst.Hide()
        objEst.BillDate = GetEntryDate(GetServerDate)
        objEst.lblUserName.Text = cnUserName
        objEst.lblSystemId.Text = systemId
        objEst.lblBillDate.Text = GetEntryDate(GetServerDate).ToString("dd/MM/yyyy")
        objEst.Set916Rate(objEst.BillDate)
        objEst.WindowState = FormWindowState.Minimized
        BrighttechPack.LanguageChange.Set_Language_Form(objEst, LangId)
        objGPack.Validator_Object(objEst)

        objEst.Size = New Size(1032, 745)
        objEst.MaximumSize = New Size(1032, 745)
        objEst.StartPosition = FormStartPosition.Manual
        objEst.Location = New Point((ScreenWid - objEst.Width) / 2, ((ScreenHit - 25) - objEst.Height) / 2)

        objEst.KeyPreview = True
        objEst.MaximizeBox = False
        objEst.StartPosition = FormStartPosition.CenterScreen
        'objEst.Location = New Point(-3, -3)
        objEst.Show()
        objEst.WindowState = FormWindowState.Normal
    End Sub

    Private Sub tStripBkImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripBkImage.Click
        Try
            Dim openDia As New OpenFileDialog
            Dim str As String
            str = "JPEG(*.jpg)|*.jpg"
            str += "|Bitmap(*.bmp)|*.bmp"
            str += "|GIF(*.gif)|*.gif"
            str += "|All Files(*.*)|*.*"
            openDia.Filter = str
            If openDia.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim ImageTemp As Byte() = ConvertImageFiletoBytes(openDia.FileName.ToString)
                strSql = "UPDATE " & cnAdminDb & "..PRJCTRL SET BKIMAGE = ? "
                strSql += " WHERE  CTLID = 2"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.Parameters.AddWithValue("@BKIMAGE", ImageTemp)
                cmd.ExecuteNonQuery()
                MsgBox("Application Need to Restart.", MsgBoxStyle.Information)
                Application.Restart()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information)
        End Try
    End Sub
#Region "USER Define Function"
    Public Function ConvertImageFiletoBytes(ByVal ImageFilePath As String) As Byte()
        Dim _tempByte() As Byte = Nothing
        If String.IsNullOrEmpty(ImageFilePath) = True Then
            Throw New ArgumentNullException("Image File Name Cannot be Null or Empty", "ImageFilePath")
            Return Nothing
        End If
        Try
            Dim _fileInfo As New IO.FileInfo(ImageFilePath)
            Dim _NumBytes As Long = _fileInfo.Length
            Dim _FStream As New IO.FileStream(ImageFilePath, IO.FileMode.Open, IO.FileAccess.Read)
            Dim _BinaryReader As New IO.BinaryReader(_FStream)
            _tempByte = _BinaryReader.ReadBytes(Convert.ToInt32(_NumBytes))
            _fileInfo = Nothing
            _NumBytes = 0
            _FStream.Close()
            _FStream.Dispose()
            _BinaryReader.Close()
            Return _tempByte
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
#End Region


    Private Sub OrderRemainderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderRemainderToolStripMenuItem.Click
        For Each ps As System.Diagnostics.Process In System.Diagnostics.Process.GetProcessesByName("AKSMESSENGER")
            ps.Kill()
        Next
        Dim ro() As DataRow = _DtUserRights.Select("MENUID = '" & OrderStatusTStrip.Name & "'", "MENUID")
        If ro.Length > 0 Then
            If IO.File.Exists(Application.StartupPath + "\AskRemainder.EXE") Then
                System.Diagnostics.Process.Start(Application.StartupPath + "\AskRemainder.EXE", "ORDER")
            End If
        End If
    End Sub

    Private Sub OrderRemainderTodayToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderRemainderTodayToolStripMenuItem.Click
        For Each ps As System.Diagnostics.Process In System.Diagnostics.Process.GetProcessesByName("AKSMESSENGER")
            ps.Kill()
        Next
        Dim ro() As DataRow = _DtUserRights.Select("MENUID = '" & OrderStatusTStrip.Name & "'", "MENUID")
        If ro.Length > 0 Then
            If IO.File.Exists(Application.StartupPath + "\AskRemainder.EXE") Then
                System.Diagnostics.Process.Start(Application.StartupPath + "\AskRemainder.EXE", "ORDER_TODAY")
            End If
        End If
    End Sub

    Private Sub tStripMRTransfertoIss_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripMRTransfertoIss.Click
        Dim obj As New BrighttechMaster.frmMRTransfertoIss
        funcShow(obj, "MATERIAL RECEIPT TRANSFER TO SMITH")
    End Sub

    Private Sub StoneSizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StoneSizeToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmStnSize
        funcShow(obj, "Stone Size")
    End Sub

    Private Sub frmChqDesign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        funcShow(frmChequePrint, "CHEQUE DESIGN")
    End Sub

    Private Sub RangeWiseStockToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RangeWiseStockToolStripMenuItem1.Click
        Dim obj As New BrighttechREPORT.frmrangewisestock
        funcShow(obj, "RANGEWISE STOCK REPORT")
    End Sub

    Private Sub RangeWiseStockItemToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RangeWiseStockItemToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmItemRangeWiseStock
        funcShow(obj, "RANGEWISE STOCK REPORT (ITEM)")
    End Sub

    Private Sub ITEMWISESMITHREPORTToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ITEMWISESMITHREPORTToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmItemWiseSmithReport
        funcShow(obj, "ITEM WISE SMITH REPORT")
    End Sub

    Private Sub CustomerTransactionReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomerTransactionReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCustomerTransactionDetail_New
        funcShow(obj, "CUSTOMER TRANSACTION DETAILS")
    End Sub

    Private Sub LotDesignerChangeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LotDesignerChangeToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmLotnoUpdation
        funcShow(obj, "LOT DESIGNER CHANGE")
    End Sub

    Private Sub SetItemsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetItemsToolStripMenuItem.Click
        Dim obj As New frmSetItemRpt
        funcShow(obj, "SET ITEMS REPORT")
    End Sub

    Private Sub AccountTypeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AccountTypeToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmAccountType
        funcShow(obj, "ACCOUNT TYPE")
    End Sub
    Dim HourTimer As DateTime
    Dim lbl1 As String
    Dim RemainInter As Double = 0
    Dim objTimer As New Timer

    'Function Call Load Method
    Private Sub funRemainder()
        HourTimer = DateTime.Now
        lbl1 = HourTimer.ToString
        AddHandler objTimer.Tick, AddressOf RemainderTimer_Tick_Runtime
        objTimer.Interval = 10
        objTimer.Enabled = True
        objTimer.Start()
    End Sub

    Private Sub RemainderTimer_Tick_Runtime(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim oneHours As DateTime = DateTime.Now
        lbl1 = oneHours.ToString
        Dim result As TimeSpan = oneHours - HourTimer
        RemainInter = GetAdmindbSoftValue("REMAINDERINTERVAL", "0")
        If result.Minutes >= RemainInter And RemainInter <> 0 Then
            objTimer.Stop()
            If IO.File.Exists(Application.StartupPath + "\AskRemainder.EXE") Then
                System.Diagnostics.Process.Start(Application.StartupPath + "\AskRemainder.EXE", "STOCK")
            End If
            HourTimer = DateTime.Now
            lbl1 = HourTimer.ToString
        End If
        objTimer.Start()
    End Sub
    Private Sub OutstandingRemainderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutstandingRemainderToolStripMenuItem.Click
        Dim ro() As DataRow = _DtUserRights.Select("MENUID = '" & SummaryToolStripMenuItem.Name & "'", "MENUID")
        If ro.Length > 0 Then
            If IO.File.Exists(Application.StartupPath + "\AskRemainder.EXE") Then
                System.Diagnostics.Process.Start(Application.StartupPath + "\AskRemainder.EXE", "OUTSTANDING_TODAY")
            End If
        End If
    End Sub
    Private Sub CustomerSummaryRemainderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomerSummaryRemainderToolStripMenuItem.Click
        Dim ro() As DataRow = _DtUserRights.Select("MENUID = '" & SummaryToolStripMenuItem.Name & "'", "MENUID")
        If ro.Length > 0 Then
            If IO.File.Exists(Application.StartupPath + "\AskRemainder.EXE") Then
                System.Diagnostics.Process.Start(Application.StartupPath + "\AskRemainder.EXE", "OUTSTANDING")
            End If
        End If
    End Sub

    Private Sub StockRemainderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StockRemainderToolStripMenuItem.Click
        Dim ro() As DataRow = _DtUserRights.Select("MENUID = '" & tStripItemWise.Name & "'", "MENUID")
        If ro.Length > 0 Then
            If IO.File.Exists(Application.StartupPath + "\AskRemainder.EXE") Then
                System.Diagnostics.Process.Start(Application.StartupPath + "\AskRemainder.EXE", "STOCK")
            End If
        End If
    End Sub

    Private Sub frmJJFormRptGen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles frmJJFormRptGen.Click
        Dim obj As New frmIssueReceiptView
        funcShow(obj, "JJ Form Report Generate")
    End Sub

    Private Sub ReorderStockSizeWiseReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReorderStockSizeWiseReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSizeWiseReorderStock()
        funcShow(obj, "REORDER STOCK SIZE WISE")
    End Sub

    Private Sub JEInTransitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles JEInTransitToolStripMenuItem.Click
        Dim obj As New frmAmountTransferCostCentre
        funcShow(obj, "JE IN-TRANSIT")
    End Sub

    Private Sub PendingStockViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PendingStockViewToolStripMenuItem.Click
        funcShow(frmTagedItemsPendingStockView, "PENDING STOCK VIEW")
    End Sub

    Private Sub TagSplitViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagSplitViewToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmTagSplitView
        funcShow(obj, "TAG SPLIT VIEW")
    End Sub

    Private Sub TagPurchaseDetailToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagPurchaseDetailToolStripMenuItem.Click
        Dim obj As New frmPurTagEditView
        funcShow(obj, "PURCHASE TAG VIEW")
    End Sub

    Private Sub GiftVoucherDenominationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GiftVoucherDenominationToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmGiftDenom
        funcShow(obj, "GIFT VOUCHER DENOMINATION")
    End Sub

    Private Sub GiftVoucherIssueToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GiftVoucherIssueToolStripMenuItem.Click
        Dim obj As New frmGiftDist
        funcShow(obj, "GIFT VOUCHER ISSUE")
    End Sub

    Private Sub MetalWiseTradingProfitLossToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MetalWiseTradingProfitLossToolStripMenuItem.Click
        Dim obj As New frmTradingAccount
        funcShow(obj, "METAL WISE TRADING PROFIT AND LOSS ")
    End Sub

    Private Sub BillnoWiseTransactionDetailedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BillnoWiseTransactionDetailedToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalPurWiseSummary
        funcShow(obj, "BILLNO WISE TRANSACTION DETAILED")
    End Sub

    Private Sub GiftVoucherReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GiftVoucherReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmGiftVoucherReport
        funcShow(obj, "GIFT VOUCHER REPORT")
    End Sub

    Private Sub GiftVoucherCheckingToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GiftVoucherCheckingToolStripMenuItem.Click
        Dim obj As New frmGiftChecking
        funcShow(obj, "GIFT VOUCHER CHECKING")
    End Sub

    Private Sub SalesCommisionToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesCommisionToolStripMenuItem1.Click
        Dim obj As New BrighttechREPORT.CTR_SALES_COMMISION
        funcShow(obj, "SALES COMMISION")
    End Sub

    Private Sub ToolStripMenuItem23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem23.Click
        Dim obj As New BrighttechREPORT.frmSalesPersonPerformanceNew
        funcShow(obj, "SALES PERSON PERFORMANCE NEW ")
    End Sub

    Private Sub TagDetailBulkUpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagDetailBulkUpdateToolStripMenuItem.Click
        Dim obj As New BrighttechRetailJewellery.frmWTagDetailUpdator
        funcShow(obj, "TAG DETAIL BULK UPDATOR ")
    End Sub

    Private Sub tstripTransactionReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tstripTransactionReport.Click
        Dim obj As New BrighttechREPORT.frmDaywiseIOSummary
        funcShow(obj, "TRANSACTION REPORT ")
    End Sub

    Private Sub AgeWiseSalesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AgeWiseSalesToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmAgeWiseSalesAnalysis
        funcShow(obj, "SALES AGE WISE ANALYSIS")
    End Sub

    Private Sub BillAdjustmentAuthorizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BillAdjustmentAuthorizeToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmBillAuthorize
        funcShow(obj, "BILL ADJUSTMENT AUTHORIZE")
    End Sub

    Private Sub OrderVAAnalysisToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderVAAnalysisToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmOrderVaAnalysis
        funcShow(obj, "ORDER VA ANALYSIS")
    End Sub

    Private Sub CostCenterWiseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CostCenterWiseToolStripMenuItem.Click
        funcShow(frmTagedITemCostCenterWise, "COST CENTER WISE TAGED ITEM")
    End Sub

    Private Sub BillAuthorizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BillAuthorizeToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmbillauthorize
        funcShow(obj, "BILL ADJUSTMENT AUTHORIZE")
    End Sub

    Private Sub SalesBillDetailsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesBillDetailsToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.FrmSalesBillWise
        funcShow(obj, "SALES BILL WISE DETAILS ")
    End Sub

    Private Sub SalesPurchaseDetailedReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesPurchaseDetailedReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalPurWiseSummary
        funcShow(obj, "SALES PURCHASE DETAILED REPORT")
    End Sub

    Private Sub SalesWiseSummaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesWiseSummaryToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalesCatSummary()
        funcShow(obj, "SALES WISE SUMMARY REPORT")
    End Sub

    Private Sub SalesWiseDetailToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesWiseDetailToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalesWiseDetail
        funcShow(obj, "SALES WISE DETAIL REPORT")
    End Sub

    Private Sub SalesReturnReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesReturnReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalesReturn_new
        funcShow(obj, "SALES RETURN REPORT")
    End Sub

    Private Sub ItemCounterGrp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemCounterGrp.Click
        Dim obj As New BrighttechMaster.frmCounterItemGrp
        funcShow(obj, "ITEM COUNTER GROUP")
    End Sub

    Private Sub AddressBookToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddressBookToolStripMenuItem.Click
        If IO.File.Exists(Application.StartupPath & "\ADDRESSBOOK\ADDRESSBOOK.EXE") Then
            System.Diagnostics.Process.Start(Application.StartupPath & "\ADDRESSBOOK\ADDRESSBOOK.EXE", "" & cnUserName & " " & cnPassword & "")
        Else
            MsgBox("AddressBook exe not found...", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub tStripStockUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tStripStockUpload.Click
        Dim obj As New BrighttechREPORT.frmStockUpload
        funcShow(obj, "STOCK UPLOAD")
    End Sub

    Private Sub tStripTranSumNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripTranSumNew.Click
        Dim obj As New BrighttechREPORT.TransactionSummarynew
        funcShow(obj, "TRANSACTION SUMMARY NEW")
    End Sub

    Private Sub tStripItemWiseNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripItemWiseNew.Click
        Dim obj As New BrighttechREPORT.frmItemWiseReport
        funcShow(obj, "ITEMWISE SALES")
    End Sub

    Private Sub tStripOnlProduct_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripOnlProduct.Click
        Dim obj As New BrighttechREPORT.frmTagedItemsStockViewOnlProduct
        funcShow(obj, "ONLINE PRODUCT STOCK")
    End Sub

    Private Sub tStripOnlStone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripOnlStone.Click
        Dim obj As New BrighttechREPORT.FRMTAGEDITEMSSTOCKVIEWONLSTONE
        funcShow(obj, "ONLINE STONE STOCK")
    End Sub

    Private Sub TagTypeChangeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TagTypeChangeToolStripMenuItem.Click
        funcShow(frmTagTypeChange, "TAG TYPE CHANGE")
    End Sub

    Private Sub SmithBalanceReportToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SmithBalanceReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmNewSmithBalance
        funcShow(obj, "SMITH BALANCE REPORT")
    End Sub

    Private Sub CategoryStockEDToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CategoryStockEDToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCatStockED
        funcShow(obj, "CATEGORY STOCK ED")
    End Sub

    Private Sub DiscountAuthenticationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiscountAuthenticationToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmDiscAuth
        funcShow(obj, "DISCOUNT AUTHENTICATION")
    End Sub

    Private Sub BillWiseTransactionDetailedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BillWiseTransactionDetailedToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalPurWiseSummaryNew
        funcShow(obj, "BILL WISE TRANSACTION DETAILED")
    End Sub

    Private Sub PurchaseOrderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PurchaseOrderToolStripMenuItem.Click
        If True Then
            funcShow(frmPurchaseOrderReceiptReject, "PURCHASE ORDER/RECEIPT/REJECT")
        Else
            funcShow(frmPurchaseOrder, "PURCHASE ORDER")
        End If
    End Sub

    Private Sub ItemWiseEDReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemWiseEDReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmItemWiseSalesED
        funcShow(obj, "ITEM WISE ED REPORT")
    End Sub

    Private Sub tStripSalesPurchaseAbs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripSalesPurchaseAbs.Click
        Dim obj As New BrighttechREPORT.frmSalesPurAbs
        funcShow(obj, "SALES AND PURCHASE REPORT")
    End Sub

    Private Sub tStripBranchVa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripBranchVa.Click
        Dim obj As New BrighttechMaster.frmBranchVA
        funcShow(obj, "VALUE ADDED")
    End Sub
    Private Sub NotifyIcon1_BalloonTipClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.BalloonTipClicked
        strSql = "SELECT "
        strSql += " (SELECT OPTIONNAME FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONID=P.PWDOPTIONID)OPTIONAME "
        'strSql += " ,(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=P.PWDUSERID)NAME"
        strSql += " ,COUNT(*)NOOFOTP"
        strSql += " ,(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=P.CRUSERID)GIVENBY"
        strSql += " ,REMARKS"
        strSql += " FROM " & cnAdminDb & "..PWDMASTER AS P"
        strSql += " WHERE PWDDATE='" & GetEntryDate(GetServerDate()) & "' AND PWDSTATUS NOT IN('C','E') AND PWDUSERID=" & userId
        If cnCostId <> "" Then
            strSql += " AND COSTID='" & cnCostId & "'"
        End If
        strSql += " GROUP BY PWDOPTIONID,PWDUSERID,REMARKS,CRUSERID"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            taskNotify = New TaskBarNotifier()
            taskNotify.SetBackgroundBitmap(New Bitmap(My.Resources.skin1), Color.FromArgb(255, 0, 255))
            taskNotify.SetCloseBitmap(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(465, 17))
            taskNotify.AddControlToGridView1(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(18, 141), dt)
            taskNotify.TitleRectangle = New Rectangle(150, 57, 400, 28)
            taskNotify.TextRectangle = New Rectangle(75, 92, 400, 55)

            With taskNotify
                .NormalTitleColor = Color.Black
                .HoverTitleColor = Color.Blue
                .NormalContentColor = Color.Yellow
                .HoverContentColor = Color.White
                .CloseButtonClickEnabled = True
                .TitleClickEnabled = False
                .TextClickEnabled = True
                .DrawTextFocusRect = True
                .KeepVisibleOnMouseOver = True
                .ReShowOnMouseOver = False
                .Show("OTP Pending for User " & cnUserName, "", Integer.Parse("1500"), Integer.Parse("43200000"), Integer.Parse("1500"))
            End With
        End If
        NotifyIcon1.Visible = False
    End Sub
    Private Sub funcCheckOTPforUser()
        strSql = " SELECT COUNT(*) CNT FROM " & cnAdminDb & "..PWDMASTER "
        strSql += " WHERE PWDDATE='" & GetEntryDate(GetServerDate()) & "' "
        strSql += " AND PWDSTATUS NOT IN('C','E') AND PWDUSERID=" & userId
        If cnCostId <> "" Then
            strSql += " AND COSTID='" & cnCostId & "'"
        End If
        If Val(objGPack.GetSqlValue(strSql, "CNT", 0).ToString) > 0 Then
            NotifyIcon1.Visible = True
            NotifyIcon1.Text = "You have an OTP."
            NotifyIcon1.BalloonTipText = "You have an OTP."
            NotifyIcon1.ShowBalloonTip(5000, "Information", "You have an OTP.", ToolTipIcon.Info)
        Else
            NotifyIcon1.Visible = False
        End If
    End Sub

    Private Sub tStripOtpDiscountReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripOtpDiscountReport.Click
        Dim obj As New BrighttechREPORT.frmOtpDiscReport
        funcShow(obj, "OTP DISCOUNT REPORT")
    End Sub

    Private Sub GiftVoucherSchemeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GiftVoucherSchemeToolStripMenuItem.Click
        Dim obj As New frmSchemeGiftDist
        funcShow(obj, "GIFT VOUCHER (SCHEME)")
    End Sub

    Private Sub CardComissionReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CardComissionReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCardTransaction
        funcShow(obj, "CARD COMISSION REPORT")
    End Sub

    Private Sub ValueAddedAnalysisEmpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ValueAddedAnalysisEmpToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmEmployeeWiseValueAdded
        funcShow(obj, "VALUE ADDED ANALYSIS (EMPLOYEE)")
    End Sub

    Private Sub AccountHeadAttachmentsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AccountHeadAttachmentsToolStripMenuItem.Click
        funcShow(frmAcheadAttachments, "ACCOUNT HEAD ATTACHMENTS")
    End Sub

    Private Sub SalesAbstractGSTToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesAbstractGSTToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalesAbstractGST
        funcShow(obj, "SALES ABSTRACT [GST]")
    End Sub

    Private Sub SalesReturnAbstractGSTToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesReturnAbstractGSTToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalesReturnAbstractGST
        funcShow(obj, "SALES RETURN ABSTRACT [GST]")
    End Sub

    Private Sub tStripPurchaseAbsGST_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripPurchaseAbsGST.Click
        Dim obj As New BrighttechREPORT.frmPurchaseAbstractGST
        funcShow(obj, "PURCHASE ABSTRACT [GST]")
    End Sub

    Private Sub GSTRegisterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GSTRegisterToolStripMenuItem.Click
        If GetAdmindbSoftValue("GSTENTRYNEWFORMAT", "N") = "Y" Then
            funcShow(frmGstRegister_New, "GST REGISTER")
        Else
            funcShow(frmGstRegister, "GST REGISTER")
        End If
    End Sub

    Private Sub tStripGstUpdates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tStripGstUpdates.Click
        Dim obj As New BrighttechMaster.frmGstUpdates
        funcShow(obj, "GST UPDATES")
    End Sub

    Private Sub GSTPlusGSTR1ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GSTPlusGSTR1ToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmGSTR1
        funcShow(obj, "GST REPORT 1")
    End Sub

    Private Sub GSTR2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GSTR2ToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmGSTR2
        funcShow(obj, "GST REPORT 2")
    End Sub

    Private Sub ReconcileMisMatchToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReconcileMisMatchToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmGSTReConcile
        funcShow(obj, "GST RECONCILE")
    End Sub

    Private Sub GSTR3BToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GSTR3BToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmGSTR3
        funcShow(obj, "GST REPORT 3")
    End Sub

    Private Sub GSTR1SummaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GSTR1SummaryToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmGSTR1A
        funcShow(obj, "GST REPORT 1")
    End Sub

    Private Sub GSTReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GSTReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmGSTR3B
        funcShow(obj, "GST REPORT")
    End Sub

    Private Sub GSTCategoryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GSTCategoryToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmGSTcategory
        funcShow(obj, "GST CATEGORY")
    End Sub

    Private Sub JobNoWiseReporToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles JobNoWiseReporToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmJobnoRpt
        funcShow(obj, "JOBNO WISE REPORT")
    End Sub

    Private Sub tStripTagedItemsReceiptView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripTagedItemsReceiptView.Click
        Dim obj As New BrighttechREPORT.frmTagReceiptView
        funcShow(obj, "TAGED ITEMS RECEIPT VIEW")
    End Sub

    Private Sub TagedItemsStockViewWithStoneToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TagedItemsStockViewWithStoneToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmTagStockView
        funcShow(obj, "TAGED ITEMS STOCK VIEW")
    End Sub

    Private Sub CostcentreWiseSalesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CostcentreWiseSalesToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCostCentreWiseSales
        funcShow(obj, "COST CENTRE WISE SALES")
    End Sub

    Private Sub CustomerDateWiseReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomerDateWiseReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCustomerDateWise
        funcShow(obj, "CUSTOMER DATE WISE REPORT")
    End Sub

    Private Sub CategoryWiseStockDetailToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CategoryWiseStockDetailToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCategoryStock_NewFormat
        funcShow(obj, "CATEGORY WISE STOCK DETAIL REPORT")
    End Sub

    Private Sub SalesBillWiseRegisterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesBillWiseRegisterToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmSalesRegisterBillWise
        funcShow(obj, "SALES BILLWISE REGISTER REPORT")
    End Sub

    Private Sub PortalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PortalToolStripMenuItem.Click
        Dim obj As New frmPortal
        funcShow(obj, "")
    End Sub

    Private Sub CustomerDataUpLoadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomerDataUpLoadToolStripMenuItem.Click
        Dim obj As New frmCallUrl
        funcShow(obj, "")
    End Sub

    Private Sub CreditCardSlabToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreditCardSlabToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmCardComm
        funcShow(obj, "")
    End Sub

    Private Sub CallURLToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CallURLToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmCallUrl
        funcShow(obj, "")
    End Sub

    Private Sub OrderRepairCollectionDetailsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderRepairCollectionDetailsToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmOrderCollectionDetails
        funcShow(obj, "ORDER\REPAIR COLLECTION DETAILS")
    End Sub

    Private Sub PcsAndWeightWiseStockReorderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PcsAndWeightWiseStockReorderToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmWeightPieceWiseStockReOrder
        funcShow(obj, "PIECES AND WEIGHT REORDER MASTER")
    End Sub

    Private Sub ReorderStockMinAndMaxWiseReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReorderStockMinAndMaxWiseReportToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmReorderStockPieceandWeightwise
        funcShow(obj, "REORDER STOCK SIZE WISE")
    End Sub

    Private Sub DailyAbstractOnlyPaymentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DailyAbstractOnlyPaymentToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmDailyAbstractOnlyPayment
        funcShow(obj, "DAILYABSTRACT ONLY PAYMENT")
    End Sub

    Private Sub MetalvsCostcentre_Click(sender As Object, e As EventArgs) Handles MetalvsCostcentre.Click
        Dim obj As New BrighttechREPORT.frmMetalVscostcentre
        funcShow(obj, "METAL VS COSTCENTRE WISE STOCK")
    End Sub

    Private Sub OnlineToolStripMenuItem24_Click(sender As Object, e As EventArgs) Handles OnlineToolStripMenuItem24.Click
        Dim obj As New BrighttechREPORT.frmOnlineImportandExport
        funcShow(obj, "EXPORT AND IMPORT")
    End Sub

    Private Sub TradingStockPurchase_Click(sender As Object, e As EventArgs) Handles TradingStockPurchase.Click
        Dim obj As New BrighttechREPORT.frmTradingStock
        funcShow(obj, "TRADING STOCK PURCHASE")
    End Sub

    Private Sub TallyExportXmlVoucherToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TallyExportXmlVoucherToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmTallyExportxmlVoucher
        funcShow(obj, "EXPORT XML VOUCHER TO TALLY")
    End Sub

    Private Sub TrackingCustomerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TrackingCustomerToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmTrackingCustomer
        funcShow(obj, "TRACKING CUSTOMER")
    End Sub

    Private Sub tStripOrderAdditionMaster_Click(sender As Object, e As EventArgs) Handles tStripOrderAdditionMaster.Click
        Dim obj As New BrighttechMaster.frmOrAdditionalMaster
        funcShow(obj, "ORDER ADDTIONAL MASTER")
    End Sub

    Private Sub ToolStripMenuItem24_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem24.Click
        Dim obj As New BrighttechMaster.frmOrAddValueMaster
        funcShow(obj, "ORDER ADDITIONAL VALUE MASTER")
    End Sub

    Private Sub tStripSmsWishes_Click(sender As Object, e As EventArgs) Handles tStripSmsWishes.Click
        Dim obj As New BrighttechMaster.Wishes
        funcShow(obj, "BIRTHDAY / ANNIVERSARY WISHES ")
    End Sub

    Private Sub tStripStuddedAmtRate_Click(sender As Object, e As EventArgs)
        Dim obj As New BrighttechRetailJewellery.frmStuddedAmtRateUpdate
        funcShow(obj, "Studded Amount Rate Update")
    End Sub

    Private Sub StockReorderBulkToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StockReorderBulkToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmStockReorderBulk
        funcShow(obj, "STOCK REORDER BULK")
    End Sub

    Private Sub StockReorderToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StockReorderToolStripMenuItem1.Click
        Dim ro() As DataRow = _DtUserRights.Select("MENUID = '" & SummaryToolStripMenuItem.Name & "'", "MENUID")
        If ro.Length > 0 Then
            If IO.File.Exists(Application.StartupPath + "\AskRemainder.EXE") Then
                System.Diagnostics.Process.Start(Application.StartupPath + "\AskRemainder.EXE", "REORDER")
            End If
        End If
    End Sub

    Private Sub CashReceivedTodayToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CashReceivedTodayToolStripMenuItem.Click
        Dim ro() As DataRow = _DtUserRights.Select("MENUID = '" & SummaryToolStripMenuItem.Name & "'", "MENUID")
        If ro.Length > 0 Then
            If IO.File.Exists(Application.StartupPath + "\AskRemainder.EXE") Then
                System.Diagnostics.Process.Start(Application.StartupPath + "\AskRemainder.EXE", "INSTANTCASHDETAIL")
            End If
        End If
    End Sub

    Private Sub CGroupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CGroupToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frm4cgroup
        funcShow(obj, "Stone Group")
    End Sub
    Private Sub StudRateUpdate_Click(sender As Object, e As EventArgs) Handles StudRateUpdate.Click
        Dim obj As New BrighttechRetailJewellery.frmStuddedAmtRateUpdate
        funcShow(obj, "Studded Amount Rate Update")
    End Sub

    Private Sub ManufacturingIssueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManufacturingIssueToolStripMenuItem.Click
        Dim ORDER_MULTI_MIMR As Boolean = IIf(GetAdmindbSoftValue("ORDER_MULTI_MIMR", "N") = "Y", True, False)
        If ORDER_MULTI_MIMR Then
            Dim obj As New ManuFacturingIssRecTran(ManuFacturingIssRecTran.MaterialType.Issue)
            obj.lblTitle.Text = "MANUFACTURING ISSUE"
            funcShow(obj, "MANUFACTURING ISSUE")
        Else
            MsgBox("Check Soft Control ORDER_MULTI_MIMR")
        End If
    End Sub

    Private Sub ManufacturingReceiptToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManufacturingReceiptToolStripMenuItem.Click
        Dim ORDER_MULTI_MIMR As Boolean = IIf(GetAdmindbSoftValue("ORDER_MULTI_MIMR", "N") = "Y", True, False)
        If ORDER_MULTI_MIMR Then
            Dim obj As New ManuFacturingIssRecTran(ManuFacturingIssRecTran.MaterialType.Receipt)
            obj.lblTitle.Text = "MANUFACTURING RECEIPT"
            funcShow(obj, "MANUFACTURING RECEIPT")
        Else
            MsgBox("Check Soft Control ORDER_MULTI_MIMR")
        End If
    End Sub

    Private Sub tStripItemWiseStockFormat1MenuItem25_Click(sender As Object, e As EventArgs) Handles tStripItemWiseStockFormat1MenuItem25.Click
        If GetAdmindbSoftValue("RPT_ITEMWISESTK_FORMAT1", "Y") = "Y" Then
            Dim obj As New BrighttechREPORT.frmItemWiseStock_Format1()
            funcShow(obj, "ITEMWISE STOCK")
            'frmItemWiseStock~RPT~AUT
            'funcShow(frmItemWiseStock, "ITEMWISE STOCK")
        Else
            MsgBox("Check Soft Control RPT_ITEMWISESTK_FORMAT1")
        End If
    End Sub

    Private Sub TallyExportXmlVoucher2ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TallyExportXmlVoucher2ToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmTallyExportxmlVoucher2
        funcShow(obj, "EXPORT XML VOUCHER TO TALLY")
    End Sub

    Private Sub tcatLogItemDetail_Click(sender As Object, e As EventArgs) Handles tcatLogItemDetail.Click
        Dim obj As New frmCatLogItemDetail
        funcShow(obj, "CATLOG ITEM DETAILS TRANSFER")
    End Sub

    Private Sub TallyExportXmlVoucherToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles TallyExportXmlVoucherToolStripMenuItem1.Click
        Dim obj As New BrighttechREPORT.frmTallyExportxmlVoucher3
        funcShow(obj, "EXPORT XML VOUCHER TO TALLY")
    End Sub

    Private Sub tStripAccountsEnt_Click(sender As Object, e As EventArgs) Handles tStripAccountsEnt.Click

    End Sub

    Private Sub ToolStripMenuItemStoneGroupWiseStock_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemStoneGroupWiseStock.Click
        Dim obj As New BrighttechREPORT.frmStoneGroupReport()
        funcShow(obj, "Stone Group Wise Stock")
    End Sub
    Private Sub GSTRITC04ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GSTRITC04ToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmGSTITC04()
        funcShow(obj, "GSTR ITC-04 REPORT")
    End Sub

    Private Sub Birthday_AnniversaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Birthday_AnniversaryToolStripMenuItem.Click
        Dim ro() As DataRow = _DtUserRights.Select("MENUID = '" & SummaryToolStripMenuItem.Name & "'", "MENUID")
        If ro.Length > 0 Then
            If IO.File.Exists(Application.StartupPath + "\AskRemainder.EXE") Then
                System.Diagnostics.Process.Start(Application.StartupPath + "\AskRemainder.EXE", "BIRTHREMAINDER")
            End If
        End If
    End Sub

    Private Sub ProcessToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProcessToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmProcessWiseReport()
        funcShow(obj, "ProcessWise Report")
    End Sub

    Private Sub ProcessWiseReportNewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProcessWiseReportNewToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmProcessWiseReportNew()
        funcShow(obj, "ProcessWise Report")
    End Sub

    Private Sub tstripClosingStockMenuItem_Click(sender As Object, e As EventArgs) Handles tstripClosingStockMenuItem.Click
        Dim obj As New BrighttechREPORT.frmClosingStockReport()
        funcShow(obj, "Closing Stock")
    End Sub

    Private Sub CatStockItemWise_Click(sender As Object, e As EventArgs) Handles CatStockItemWise.Click
        Dim obj As New BrighttechREPORT.frmItemWiseStocknew()
        funcShow(obj, "Item Wise Category Stock")
    End Sub

    Private Sub tAcheadApproval_Click(sender As Object, e As EventArgs) Handles tAcheadApproval.Click
        Dim obj As New BrighttechREPORT.frmAcheadApproval
        funcShow(obj, "Achead Approval")
    End Sub

    Private Sub tstripItemVsMetal_Click(sender As Object, e As EventArgs) Handles tstripItemVsMetal.Click
        Dim obj As New BrighttechREPORT.frmItemVsMetalStockSummary
        funcShow(obj, "ITEM vs METAL SUMMARY ")
    End Sub

    Private Sub CashCounterConsolidatedSummaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CashCounterConsolidatedSummaryToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmConsolidatedCashCounterRpt
        funcShow(obj, " CONSOLIDATED CASH COUNTER SUMMARY")
    End Sub

    Private Sub PrivilegeTypeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrivilegeTypeToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmPrivilegeType
        funcShow(obj, "Privilege Type Master")
    End Sub

    Private Sub PaymodeTypeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PaymodeTypeToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmPaymodeType
        funcShow(obj, "Paymode Type Master")
    End Sub

    Private Sub tStripFontBackgroundColor_Click(sender As Object, e As EventArgs) Handles tStripFontBackgroundColor.Click
        Dim obj As New frmBkfonColor
        funcShow(obj, "FONT BACKGROUND COLOR")
    End Sub

    Private Sub StickerPrintToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StickerPrintToolStripMenuItem.Click
        Dim obj As New frmStickerPrint
        funcShow(obj, "Sticker Print")
    End Sub

    Private Sub EstimateWeightToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EstimateWeightToolStripMenuItem.Click
        Dim obj As New frmEstimateWeight
        funcShow(obj, "Estimate Weight Entry")
    End Sub

    Private Sub PurchaseEstimationWeightToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PurchaseEstimationWeightToolStripMenuItem.Click
        Dim obj As New frmEstPurWeight
        funcShow(obj, "Purchase Weight Report")
    End Sub

    Private Sub tEinvoiceExcelImport_Click(sender As Object, e As EventArgs) Handles tEinvoiceExcelImport.Click
        Dim obj As New frmEinvoiceImportExcel
        funcShow(obj, "Einvoice Upload")
    End Sub

    Private Sub TStripcatWeightTransfer_Click(sender As Object, e As EventArgs) Handles TStripcatWeightTransfer.Click
        Dim obj As New frmCategoryWeightTransfer
        funcShow(obj, "Category Weight Transfer")
    End Sub

    Private Sub MaterialIssueReceiptToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MaterialIssueReceiptToolStripMenuItem.Click
        Dim obj As New frmMIMR
        obj.Hide()
        obj.WindowState = FormWindowState.Minimized
        BrighttechPack.LanguageChange.Set_Language_Form(obj, LangId)
        objGPack.Validator_Object(obj)
        obj.Size = New Size(1032, 745)
        obj.MaximumSize = New Size(1032, 745)
        obj.StartPosition = FormStartPosition.Manual
        obj.Location = New Point((ScreenWid - obj.Width) / 2, ((ScreenHit - 25) - obj.Height) / 2)
        obj.KeyPreview = True
        obj.MaximizeBox = False
        obj.StartPosition = FormStartPosition.CenterScreen
        obj.Show()
        obj.WindowState = FormWindowState.Normal
    End Sub

    Private Sub ReorderStockWeightBasedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReorderStockWeightBasedToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmReorderWeightBased
        funcShow(obj, "REORDER STOCK WEIGHT BASED REPORT")
    End Sub

    Private Sub tstripHallmarknoImport_Click(sender As Object, e As EventArgs) Handles tstripHallmarknoImport.Click
        Dim obj As New frmHallmarkNoUpload
        funcShow(obj, "HallmarkNo Import")
    End Sub

    Private Sub TStripBounzProcess_Click(sender As Object, e As EventArgs) Handles TStripBounzProcess.Click
        Dim obj As New frmBounzUpload
        funcShow(obj, "Bounz Upload Process")
    End Sub

    Private Sub TStripBillprintDesign_Click(sender As Object, e As EventArgs) Handles TStripBillprintDesign.Click
        Dim obj As New BrighttechREPORT.frmBillPrintCustDesign
        funcShow(obj, "Billprint Designs")
    End Sub

    Private Sub TstripDashBoard_Click(sender As Object, e As EventArgs) Handles TstripDashBoard.Click
        Dim obj As New BrighttechREPORT.frmCustomerDashboardDetail
        funcShow(obj, "DashBoard")
    End Sub

    Private Sub CategoryWiseTransactionTStripMenuItem_Click(sender As Object, e As EventArgs) Handles CategoryWiseTransactionTStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmCategoryWiseTransaction
        funcShow(obj, "CategoryWise Transaction Report")
    End Sub

    Private Sub SchemeNoOfferToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SchemeNoOfferToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmSchemeNoOfferMast
        funcShow(obj, "Scheme NoOffer Master")
    End Sub

    Private Sub AccountsEntryImportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AccountsEntryImportToolStripMenuItem.Click
        Dim obj As New frmAccountsEntImport
        funcShow(obj, "Account Entry Import")
    End Sub

    Private Sub TStripTransferVoucherGeneration_Click(sender As Object, e As EventArgs) Handles TStripTransferVoucherGeneration.Click
        Dim obj As New frmTransferVoucherGeneration
        funcShow(obj, "Transfer Voucher Generation")
    End Sub

    Private Sub GSTReportNewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GSTReportNewToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmGSTR3BNEW
        funcShow(obj, "GST REPORT NEW")
    End Sub

    Private Sub TStripMIVoucherGeneration_Click(sender As Object, e As EventArgs) Handles TStripMIVoucherGeneration.Click
        Dim obj As New frmMIVoucherGeneration
        funcShow(obj, "MiscIssue Voucher Generation")
    End Sub

    Private Sub BagNoWiseReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BagNoWiseReportToolStripMenuItem.Click
        Dim obj As New frmBagNoWiseReport
        funcShow(obj, "BagNo Wise Report")
    End Sub

    Private Sub DealerStuddedDetail_Click(sender As Object, e As EventArgs) Handles DealerStuddedDetail.Click
        Dim obj As New BrighttechMaster.frmDealerstudded
        funcShow(obj, "Dealer Studded")
    End Sub

    Private Sub tstripDailyCollectionReport_Click(sender As Object, e As EventArgs) Handles tstripDailyCollectionReport.Click
        Dim obj As New BrighttechREPORT.frmDailyTransactionCollection
        funcShow(obj, "Daily Collection Report")
    End Sub

    Private Sub FinalEstimationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FinalEstimationToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmFinalEstSalesPurRetRpt
        funcShow(obj, "Final Estimation Report")
    End Sub

    Private Sub NonTagTransferIssueReceiptToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NonTagTransferIssueReceiptToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.NonTagIssRecSyncRpt()
        funcShow(obj, "NONTAG WISE TRANSFER ISSUE/RECEIPT")
    End Sub

    Private Sub CostcentreWiseTransferSummaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CostcentreWiseTransferSummaryToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmTransferSumCostCentreWise()
        funcShow(obj, "CostcentreWise Transfer Summary")
    End Sub

    Private Sub tStripMaster_Click(sender As Object, e As EventArgs) Handles tStripMaster.Click

    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    Private Sub StatusStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles StatusStrip1.ItemClicked

    End Sub

    Private Sub tStripShortcut_Click(sender As Object, e As EventArgs) Handles tStripShortcut.Click


    End Sub

    Private Sub tStripTitle_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles tStripTitle.ItemClicked

    End Sub
    Private Sub Main_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.B Then
            'ent += 1
            MsgBox("HI")
        End If
    End Sub

    Private Sub RangeWiseStockIssueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RangeWiseStockIssueToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmItemRangeWiseStockIssue
        funcShow(obj, "RANGEWISE STOCK REPORT (ISSUE)")
    End Sub

    Private Sub RangeWiseStockToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles RangeWiseStockToolStripMenuItemCostCentre.Click
        Dim obj As New BrighttechREPORT.frmItemRangeWiseStockCostCentre
        funcShow(obj, "RANGEWISE STOCK REPORT (COST CENTRE)")
    End Sub

    Private Sub BillingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BillingToolStripMenuItem.Click
        If Not CheckGoldRate() Then Exit Sub
        frmBillInitialize.BillType = frmBillInitialize.BillTypee.WholeSale_New
        funcShow(frmBillInitialize)
    End Sub

    Private Sub HallMarkCenterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HallMarkCenterToolStripMenuItem.Click
        Dim obj As New BrighttechMaster.frmHMCenter
        funcShow(obj, "Hall Mark Center")
    End Sub

    Private Sub ReportsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ReportsToolStripMenuItem1.Click
        funcShow(frmQCHMReport, "QC / HallMark")
    End Sub

    Private Sub EntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EntryToolStripMenuItem.Click
        funcShow(frmMR_QA, "QC / HallMark")
    End Sub

    Private Sub PurchaseOrderToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PurchaseOrderToolStripMenuItem1.Click
        Dim obj As New BrighttechREPORT.frmPurchaseOrder
        funcShow(obj, "PURCHASE ORDER REPORT")
    End Sub

    Private Sub DesignerLotToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DesignerLotToolStripMenuItem.Click
        funcShow(frmDesignerLotEntry, "DESIGNER LOT ENTRY")
    End Sub

    Private Sub PurchaseOrderTrackingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PurchaseOrderTrackingToolStripMenuItem.Click
        Dim obj As New BrighttechREPORT.frmPurchaseOrderTracking
        funcShow(obj, "PURCHASE ORDER TRACKING")
    End Sub
End Class