Imports System.Data.Sql
Imports System.Data.OleDb
Imports System.IO
Imports System.Xml.Serialization
'CALNO 01114 SATHYA
Public Class SqlUtility
    Dim Da As OleDbDataAdapter
    Dim StrSql As String
    Dim Cn As OleDbConnection
    Dim _Con As OleDbConnection
    Dim Cmd As OleDbCommand
    Dim FileConcat As String
    Dim DtStatus As New DataTable
    Public _Admindb As String
    Public _DataSource As String
    Public _DropDb As String
    Dim _Tran As OleDbTransaction
    Dim Constraint_name, TableName, ColumnName As String
    Dim ExternalCalling As Boolean = False
    Dim DtCommandLineArguments As New DataTable
    Dim dtAttach As New DataTable
    Dim dtAttachAll As New DataTable
    Dim StrDbServerName As String, StrDbPath As String, StrDbUser As String, StrDbPwd As String, StrDbLoginType As String
    Dim StrDbMdfPath(1000) As String
    Dim StrDbLdfPath(1000) As String
    Dim StrAttachDbName(1000) As String, StrDetachDbName(1000) As String
    ''' <summary>
    ''' COMMAND LINE ARGUMENTS
    ''' 0 Datasource (Datasource=server)
    ''' 1 loginMode authentication (loginType=server/windows)
    ''' 2 username (userid=sa)
    ''' 3 password (pwd="")
    ''' 4 action mode (action=(A)ttach/(D)etach/(B)ackup/(R)estore/(S)hrink)
    ''' 5 DetachDbName (DetachDbName="")
    ''' 6 AttachMdfPath (AttachMdfFilePath=D:\data\cccadmindb.mdf)
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Enum Action
        Attach = 0
        Detach = 1
        BackUp = 2
        Restore = 3
        Shrink = 4
    End Enum
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Initializer()

    End Sub
    Public Sub New(ByVal DataSourceName As String _
, ByVal LoginType As String _
, ByVal UserId As String _
, ByVal Pwd As String _
, ByVal ActionType As Action _
, ByVal AttachMdfFilePath As String _
)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Initializer()
        SetCommandLineValues("DATASOURCE", DataSourceName)
        SetCommandLineValues("LOGINTYPE", LoginType)
        SetCommandLineValues("USERID", UserId)
        SetCommandLineValues("PWD", Pwd)
        SetCommandLineValues("ACTION", "A")
        SetCommandLineValues("ATTACHMDFFILEPATH", AttachMdfFilePath)
    End Sub
    Public Sub New(ByVal DataSourceName As String _
    , ByVal LoginType As String _
    , ByVal UserId As String _
    , ByVal Pwd As String _
    , ByVal Action As String _
    , ByVal DetachDbName As String _
    , ByVal AttachMdfFilePath As String _
    )

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Initializer()
        SetCommandLineValues("DATASOURCE", DataSourceName)
        SetCommandLineValues("LOGINTYPE", LoginType)
        SetCommandLineValues("USERID", UserId)
        SetCommandLineValues("PWD", Pwd)
        SetCommandLineValues("ACTION", Action)
        SetCommandLineValues("DETACHDBNAME", DetachDbName)
        SetCommandLineValues("ATTACHMDFFILEPATH", AttachMdfFilePath)

    End Sub
#Region "General Methods & Events"

    Private Sub Initializer()
        cmbLoginType.Text = "SERVER"
        DtCommandLineArguments.Columns.Add("NAME", GetType(String))
        DtCommandLineArguments.Columns.Add("VALUE", GetType(String))
        DtStatus.Columns.Add("STATUS", GetType(String))
        gridBackUpStatus.DataSource = DtStatus
        gridBackUpStatus.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridBackUpStatus.ColumnHeadersVisible = False
        gridBackUpStatus.Columns("STATUS").Width = gridBackUpStatus.Width - 20
        gridRestoreFiles.DataSource = clsProperties.pRestoreDtFileNames
        gridRestoreFiles.Columns("FILENAME").Width = 120
        gridRestoreFiles.Columns("FILEPATH").Visible = False
        gridRestoreFiles.Columns("DBNAME").Width = 80
        gridRestoreFiles.Columns("FILEPATH").ReadOnly = True
        gridRestoreFiles.Columns("DBNAME").ReadOnly = False
        gridRestoreFiles.Columns("DBNAME").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        gridRestoreFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dtAttach = New DataTable()
        dtAttach.Columns.Add(New DataColumn("Dbname", Type.GetType("System.String")))
        dtAttach.Columns.Add(New DataColumn("Mdf", Type.GetType("System.String")))
        dtAttach.Columns.Add(New DataColumn("Ldf", Type.GetType("System.String")))
        dtAttach.Columns.Add(New DataColumn("Status", Type.GetType("System.String")))
    End Sub

    Private Function GetSqlServers() As ArrayList
        Dim listOfServers As New ArrayList
        Dim sqlEnumerator As SqlDataSourceEnumerator = SqlDataSourceEnumerator.Instance
        Dim sqlServersTable As DataTable = sqlEnumerator.GetDataSources()
        For Each rowOfData As DataRow In sqlServersTable.Rows
            'get the server name 
            Dim serverName As String = rowOfData("ServerName").ToString() '+ "\" + rowOfData("INSTANCENAME").ToString()
            If listOfServers.Contains(serverName) = False Then listOfServers.Add(serverName)
        Next
        listOfServers.Sort()
        Return listOfServers
    End Function

    Private Sub SqlUtility_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If ExternalCalling Then Exit Sub
        If MessageBox.Show("Do you want to save this setting", "Save Setting?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            My.Settings.DataSource = cmbServers.Text
            My.Settings.DbType = cmbLoginType.Text
            My.Settings.LoginName = txtLoginName.Text
            My.Settings.Password = txtPassword.Text
            My.Settings.BackUpPath = txtBackUpPath.Text
            My.Settings.RbtBackUp = IIf(rdbBackUp.Checked, True, False)
            If Not My.Settings.ServerNames Is Nothing Then My.Settings.ServerNames.Clear() Else My.Settings.ServerNames = New ArrayList
            For Each s As String In cmbServers.Items
                My.Settings.ServerNames.Add(s)
            Next
            _ShrinkSetSettings()
            _BackUpSetSettings()
            My.Settings.Save()
        End If
    End Sub

    Private Sub SetCommandLineValues(ByVal Name As String, ByVal Value As String)
        Dim ro As DataRow = DtCommandLineArguments.NewRow
        ro!NAME = Name
        ro!VALUE = Value
        DtCommandLineArguments.Rows.Add(ro)
    End Sub

    Private Function GetCommandLineValues(ByVal Name As String) As String
        Dim retStr As String = ""
        Dim ro() As DataRow = DtCommandLineArguments.Select("NAME = '" & Name & "'")
        For Each r As DataRow In ro
            retStr = r.Item("VALUE").ToString
        Next
        Return retStr
    End Function

    Private Sub SqlUtility_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For Each s As String In My.Application.CommandLineArgs
            Dim sp() As String = s.Split("=")
            If sp.Length = 2 Then
                SetCommandLineValues(sp(0), sp(1))
            End If
        Next
        If DtCommandLineArguments.Rows.Count > 0 Then
            ExternalCalling = True
            cmbServers.Text = GetCommandLineValues("DATASOURCE")
            If GetCommandLineValues("LOGINTYPE").ToUpper = "S" Or GetCommandLineValues("LOGINTYPE").ToUpper = "SA" Then
                cmbLoginType.Text = "Server"
            Else
                cmbLoginType.Text = "Windows"
            End If
            If cmbLoginType.Text = "Server" Then
                txtLoginName.Text = "Sa"
            Else
                txtLoginName.Text = GetCommandLineValues("USERID")
            End If
            txtPassword.Text = BrighttechPack.Methods.Decrypt(GetCommandLineValues("PWD"))
            Dim Action As String = GetCommandLineValues("ACTION")
            tabMain.TabPages.Clear()
            tabMain.TabPages.Add(tabGeneral)
            ''Action Mode
            Select Case GetCommandLineValues("ACTION")
                Case "A"
                    rdbAttach.Checked = True
                    btnNext_Click(Me, New EventArgs)
                Case "D"
                    rdbDetach.Checked = True
                    btnNext_Click(Me, New EventArgs)
                Case "R"
                    rdbRestore.Checked = True
                    btnNext_Click(Me, New EventArgs)
                Case "B"
                    rdbBackUp.Checked = True
                    btnNext_Click(Me, New EventArgs)
                Case "S"
                    rdbShrinkLogfile.Checked = True
                    btnNext_Click(Me, New EventArgs)
            End Select
            ''Detach Values
            cmbDetachDatabase.Text = GetCommandLineValues("DETACHDBNAME")
            ''Attach Values
            txtAttachSourcePath.Text = GetCommandLineValues("AttachMdfFilePath")
            _AttachGetFileLocations()
        End If

        If Not ExternalCalling Then
            tabMain.TabPages.Clear()
            tabMain.TabPages.Add(tabGeneral)
            tabMain_SelectedIndexChanged(Me, New EventArgs)
            If IO.File.Exists(Application.StartupPath & "\" & "CONINFO.INI") Then
                Dim fil As New FileStream(Application.StartupPath & "\" & "CONINFO.INI", FileMode.Open, FileAccess.Read)
                Dim fs As New StreamReader(fil, System.Text.Encoding.Default)

                fs.BaseStream.Seek(0, SeekOrigin.Begin)
                fs.ReadLine() 'companyid
                fs.ReadLine() 'companyname
                cmbServers.Text = Mid(fs.ReadLine, 21)
                StrDbPath = Mid(fs.ReadLine, 21).ToUpper 'DbPath
                Dim pwd As String
                pwd = Mid(fs.ReadLine, 21) '5
                If pwd <> "" Then pwd = BrighttechPack.Methods.Decrypt(pwd)
                txtPassword.Text = pwd
                Dim logintype As String
                logintype = Mid(fs.ReadLine, 21).ToUpper
                If logintype.ToUpper <> "W" Then 'If Mid(fs.ReadLine, 21).ToUpper = "S" Then
                    cmbLoginType.Text = "Server"
                    If logintype.ToUpper <> "S" Then txtLoginName.Text = logintype.ToUpper
                Else
                    cmbLoginType.Text = "Windows"
                End If
                fs.Close()
            End If
        End If
        txtLoginName.Text = "sa"
        If Not My.Settings.BackUpPath Is Nothing Then txtBackUpPath.Text = My.Settings.BackUpPath
        If Not My.Settings.DataSource Is Nothing Then cmbServers.Text = My.Settings.DataSource
        If Not My.Settings.DbType Is Nothing Then cmbLoginType.Text = My.Settings.DbType
        If Not My.Settings.LoginName Is Nothing Then txtLoginName.Text = My.Settings.LoginName
        If Not My.Settings.Password Is Nothing Then txtPassword.Text = My.Settings.Password
        rdbBackUp.Checked = My.Settings.RbtBackUp
    End Sub

    Private Sub tabMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabMain.SelectedIndexChanged
        If Not tabMain.TabPages.Count > 0 Then Exit Sub
        If tabMain.SelectedTab.Name = tabGeneral.Name Then
            btnBack.Enabled = False
            btnNext.Text = "Next >"
        Else
            btnBack.Enabled = True
            btnNext.Text = "Finish"
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Function GetSqlValue(ByVal Qry As String, Optional ByVal DefValue As String = "", Optional ByVal tCn As OleDbConnection = Nothing) As Object
        If tCn Is Nothing Then
            tCn = Cn
        End If
        Dim retStr As String = ""
        Dim dtSqlVal As New DataTable
        Da = New OleDbDataAdapter(Qry, tCn)
        Da.Fill(dtSqlVal)
        If dtSqlVal.Rows.Count > 0 Then retStr = dtSqlVal.Rows(0).Item(0).ToString
        Return retStr
    End Function
    Private Function GetConnectionString(ByVal DbName As String, ByVal ServerName As String, ByVal LoginName As String, ByVal Pwd As String, ByVal DbType As String)
        Dim retStr As String = ""
        If DbType.ToUpper = "S" Then
            retStr = "PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & DbName & ";Data Source=" & ServerName & ";uid=" & LoginName & ";pwd=" & Pwd & ";"
        Else
            retStr = "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & DbName & ";Data Source=" & ServerName & ""
        End If
        Return retStr
    End Function
    Private Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If btnNext.Text = "Finish" Then
            If rdbShrinkLogfile.Checked Then
                _Shrink()
            ElseIf rdbBackUp.Checked Then
                _BackUp()
            ElseIf rdbAttach.Checked Then
                If tabctrlAttachDetach.SelectedIndex = 0 Then
                    If Not DtCommandLineArguments.Rows.Count > 0 Then
                        _AttachNew()
                    Else
                        _Attach()
                    End If
                Else
                    _AttachNewAll()
                End If
            ElseIf rdbDetach.Checked Then
                If tabctrlDetach.SelectedIndex = 0 Then
                    _Detach()
                Else
                    _DetachAll()
                End If

            ElseIf rdbRestore.Checked Then
                _Restore()
            ElseIf RbtnRecover.Checked Then
                '_Recover()
            ElseIf RbtnRelease.Checked Then
                _Release()
            Else
                MsgBox("Finished")
            End If
        Else
            If txtLoginName.Text = String.Empty Then MsgBox("LoginName Empty", MsgBoxStyle.Information) : Exit Sub
            If Mid(cmbLoginType.Text, 1, 1) = "S" Then
                Cn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=;Data Source={0};uid=" & txtLoginName.Text & ";pwd=" & txtPassword.Text & ";", cmbServers.Text))
            Else
                Cn = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=;Data Source=" & cmbServers.Text & "")
            End If
            Try
                Cn.Open()
                _ShrinkLoadDatabase()
                _BackUpLoadDatabase()
                _DetachLoadDatabase()
                _AlterCollationLoad()
                _RecoverLoadDatabase()
                _ReleaseLoadDatabase()
            Catch ex As Exception
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                Exit Sub
            End Try
            tabMain.TabPages.Clear()
            If rdbBackUp.Checked Then
                tabMain.TabPages.Add(tabBackUp)
            ElseIf rdbRestore.Checked Then
                tabMain.TabPages.Add(tabRestore)
            ElseIf rdbAttach.Checked Then
                tabMain.TabPages.Add(tabAttach)
            ElseIf rdbDetach.Checked Then
                tabMain.TabPages.Add(tabDetach)
            ElseIf rdbShrinkLogfile.Checked Then
                tabMain.TabPages.Add(tabShrink)
            ElseIf RbtnCollation.Checked Then
                tabMain.TabPages.Add(Collation)
            ElseIf RbtnRecover.Checked Then
                tabMain.TabPages.Add(tabRecover)
            ElseIf RbtnRelease.Checked Then
                tabMain.TabPages.Add(tabReleaseMemory)
            End If
        End If
        tabMain_SelectedIndexChanged(Me, New EventArgs)
    End Sub
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.TabPages.Clear()
        tabMain.TabPages.Add(tabGeneral)
        tabMain_SelectedIndexChanged(Me, New EventArgs)
    End Sub
    Private Sub btnVBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVBack.Click
        tabMain.TabPages.Clear()
        tabMain.TabPages.Add(tabReleaseMemory)
        tabMain_SelectedIndexChanged(Me, New EventArgs)
        btnBack.Visible = True : btnCancel.Visible = True : btnNext.Visible = True
    End Sub
    Private Sub btnRefreshServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshServer.Click
        btnRefreshServer.Enabled = False
        cmbServers.Focus()
        cmbServers.Items.Clear()
        For Each s As String In GetSqlServers()
            cmbServers.Items.Add(s)
        Next
        btnRefreshServer.Enabled = True
    End Sub

#End Region
#Region "Shrink Methods & Events"
    Private Sub _ShrinkLoadDatabase()
        ChkLstbShrinkDbName.Items.Clear()
        Dim DtDb As New DataTable
        StrSql = " SELECT NAME FROM MASTER..SYSDATABASES ORDER BY NAME"
        Da = New OleDbDataAdapter(StrSql, Cn)
        Da.Fill(DtDb)
        For Each ro As DataRow In DtDb.Rows
            'cmbShrinkDbName.Items.Add(ro("NAME").ToString)
            ChkLstbShrinkDbName.Items.Add(ro("NAME").ToString)
        Next
        If Not My.Settings.ShrinkDbNames Is Nothing Then
            For cnt As Integer = 0 To ChkLstbShrinkDbName.Items.Count - 1
                If My.Settings.ShrinkDbNames.Contains(ChkLstbShrinkDbName.Items(cnt).ToString) Then
                    ChkLstbShrinkDbName.SetItemChecked(cnt, True)
                End If
            Next
        End If
    End Sub
    Private Sub _Shrink()
        If Not ChkLstbShrinkDbName.CheckedItems.Count > 0 Then
            MsgBox("Please select databasename", MsgBoxStyle.Information)
            ChkLstbShrinkDbName.Focus()
            Exit Sub
        End If
        Try
            For i As Integer = 0 To ChkLstbShrinkDbName.CheckedItems.Count - 1
                Dim StrShrinkDbName As String = ChkLstbShrinkDbName.CheckedItems(i).ToString
                'Change connection
                Cn = New OleDbConnection(GetConnectionString(StrShrinkDbName, cmbServers.Text, txtLoginName.Text, txtPassword.Text, Mid(cmbLoginType.Text, 1, 1)))
                Cn.Open()
                StrSql = " SELECT	SUBSTRING(CONVERT(VARCHAR,SERVERPROPERTY('PRODUCTVERSION')),1,CHARINDEX('.',CONVERT(VARCHAR,SERVERPROPERTY('PRODUCTVERSION')))-1) AS [VERSION]"
                Dim sqlVERSION As String = GetSqlValue(StrSql)
                StrSql = " SELECT NAME FROM " & StrShrinkDbName & "..SYSFILES WHERE FILEID = 2"
                Dim LogFile As String = GetSqlValue(StrSql)
                If LogFile = "" Then
                    MsgBox("Logfile not found", MsgBoxStyle.Information)
                    Exit Sub
                End If
                If sqlVERSION = "8" Or sqlVERSION = "9" Then
                    StrSql = " BACKUP LOG " & StrShrinkDbName & " WITH TRUNCATE_ONLY"
                    Cmd = New OleDbCommand(StrSql, Cn)
                    Cmd.ExecuteNonQuery()
                    StrSql = " DBCC SHRINKFILE(" & LogFile & "," & dudShrink.Text & ")"
                    Cmd = New OleDbCommand(StrSql, Cn)
                    Cmd.ExecuteNonQuery()
                ElseIf Val(sqlVERSION) >= 10 Then
                    StrSql = "ALTER DATABASE " & StrShrinkDbName & " SET RECOVERY SIMPLE WITH NO_WAIT"
                    Cmd = New OleDbCommand(StrSql, Cn)
                    Cmd.ExecuteNonQuery()
                    StrSql = "DBCC SHRINKFILE(" & LogFile & "," & dudShrink.Text & ")"
                    Cmd = New OleDbCommand(StrSql, Cn)
                    Cmd.ExecuteNonQuery()
                    StrSql = "ALTER DATABASE " & StrShrinkDbName & " SET RECOVERY FULL WITH NO_WAIT"
                    Cmd = New OleDbCommand(StrSql, Cn)
                    Cmd.ExecuteNonQuery()
                End If
            Next
            MsgBox("Shrink Completed", MsgBoxStyle.Information)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub _ShrinkGetSettings()
        ''Load Previous Shrink Settngs
        If Not My.Settings.ShrinkDbNames Is Nothing Then
            For cnt As Integer = 0 To ChkLstbShrinkDbName.Items.Count - 1
                If My.Settings.ShrinkDbNames.Contains(ChkLstbShrinkDbName.Items(cnt).ToString) Then
                    ChkLstbShrinkDbName.SetItemChecked(cnt, True)
                End If
            Next
        End If
    End Sub

    Private Sub _ShrinkSetSettings()
        'My.Settings.ShrinkDbName = cmbShrinkDbName.Text
        If Not My.Settings.ShrinkDbNames Is Nothing Then My.Settings.ShrinkDbNames.Clear() Else My.Settings.ShrinkDbNames = New ArrayList
        For cnt As Integer = 0 To ChkLstbShrinkDbName.CheckedItems.Count - 1
            My.Settings.ShrinkDbNames.Add(ChkLstbShrinkDbName.CheckedItems.Item(cnt).ToString)
        Next
    End Sub

#End Region
#Region "BackUp Methods & Events"
    Private Sub _BackUpGetSettings()
        ''Load Previous BackUp Settngs
        If Not My.Settings.BackUpPath Is Nothing Then
            txtBackUpPath.Text = My.Settings.BackUpPath
        End If
        If My.Settings.BackUpTimeWise = False Then
            rbtBackUpTimeWise.Checked = False
            rbtBackUpDayWise.Checked = True
        Else
            rbtBackUpTimeWise.Checked = True
            rbtBackUpDayWise.Checked = False
        End If
    End Sub
    Private Sub _BackUpSetSettings()
        If Not My.Settings.BackUpDbNames Is Nothing Then My.Settings.BackUpDbNames.Clear() Else My.Settings.BackUpDbNames = New ArrayList
        For cnt As Integer = 0 To chkLstBackUpDataBases.CheckedItems.Count - 1
            My.Settings.BackUpDbNames.Add(chkLstBackUpDataBases.CheckedItems.Item(cnt).ToString)
        Next
        My.Settings.BackUpPath = txtBackUpPath.Text
        My.Settings.BackUpTimeWise = IIf(rbtBackUpTimeWise.Checked, True, False)
    End Sub
    Private Sub _BackUpOld()
        '        ''Validation
        '        If Not chkLstBackUpDataBases.CheckedItems.Count > 0 Then
        '            MsgBox("Please Check Database", MsgBoxStyle.Information)
        '            chkLstBackUpDataBases.Focus()
        '            Exit Sub
        '        End If
        '        If txtBackUpPath.Text = "" Then
        '            MsgBox("Backup Path Should Not Empty", MsgBoxStyle.Information)
        '            txtBackUpPath.Focus()
        '            Exit Sub
        '        ElseIf Directory.Exists(txtBackUpPath.Text) = False Then
        '            MsgBox("Invalid Backup Path", MsgBoxStyle.Information)
        '            txtBackUpPath.Focus()
        '            Exit Sub
        '        End If
        '        Try
        '            If rbtBackUpDayWise.Visible Then
        '                Dim dd As Date = GetSqlValue("SELECT GETDATE()")
        '                If rbtBackUpDayWise.Checked Then
        '                    FileConcat = "_" + Format(dd.Date.Day, "00") + Format(dd.Date.Month, "00") + Mid(dd.Date.Year, 3, 2)
        '                Else
        '                    FileConcat = "_" + Format(dd.Date.Day, "00") + Format(dd.Date.Month, "00") + Mid(dd.Date.Year, 3, 2) _
        '                    + "_" + Format(dd.TimeOfDay.Hours, "00") + Format(dd.TimeOfDay.Minutes, "00") + Format(dd.TimeOfDay.Seconds, "00")
        '                End If
        '            End If

        '            For i As Integer = 0 To chkLstBackUpDataBases.CheckedItems.Count - 1
        '                Dim dbBackPath As String = ""
        '                Dim dbName As String = chkLstBackUpDataBases.CheckedItems.Item(i).ToString
        '                Dim tempPath As String = GetSqlValue("SELECT FILENAME FROM MASTER..SYSDATABASES WHERE NAME = '" & dbName & "'").ToString
        '                Dim fil As New FileInfo(tempPath)
        '                If Not ChkBackUpNetWork.Checked Then
        '                    tempPath = txtBackUpPath.Text & "\" & dbName & FileConcat & ".BAK"
        '                    _BackUpLoadStatus(" Set Recovery to " + dbName, True)
        '                    _BackUpBarStep()
        '                    StrSql = " ALTER DATABASE " & dbName & " SET RECOVERY FULL"
        '                    Cmd = New OleDbCommand(StrSql, Cn)
        '                    Cmd.ExecuteNonQuery()
        '                    _BackUpLoadStatus(dbName + " Set Recovery Completed..")
        '                    _BackUpBarStep()

        '                    _BackUpLoadStatus("Backup " + dbName + "..", True)
        '                    _BackUpBarStep()
        '                    StrSql = " BACKUP DATABASE [" & dbName & "] TO  DISK = N'" & tempPath & "' WITH  NOINIT ,  NOUNLOAD ,  NAME = N'" & dbName & " BackUp" & "',  NOSKIP ,  STATS = 10,  NOFORMAT "
        '                    Cmd = New OleDbCommand(StrSql, Cn)
        '                    Cmd.ExecuteNonQuery()
        '                    _BackUpLoadStatus("Backup Databases" + dbName + " Completed..")
        '                    _BackUpBarStep()
        '                ElseIf ChkBackUpNetWork.Checked Then
        '                    If txtBackUpPath.Text.EndsWith("\") Then
        '                        dbBackPath = txtBackUpPath.Text & dbName & FileConcat & ".BAK"
        '                    Else
        '                        dbBackPath = txtBackUpPath.Text & "\" & dbName & FileConcat & ".BAK"
        '                    End If
        '                    fil = New FileInfo(tempPath)
        '                    tempPath = fil.DirectoryName & "\" & dbName & FileConcat & ".BAK"
        'CheckFilePermission:
        '                    If IO.Directory.Exists(fil.DirectoryName) = False Then
        'PermissionMsg:
        '                        Dim msgINfo As String = dbBackPath + " Write Protected" + vbCrLf + "Please Change that access permission"
        '                        Select Case MessageBox.Show(msgINfo, "Access Denied", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2)
        '                            Case Windows.Forms.DialogResult.Abort
        '                                Exit Sub
        '                            Case Windows.Forms.DialogResult.Retry
        '                                GoTo CheckFilePermission
        '                            Case Windows.Forms.DialogResult.Ignore
        '                                Continue For
        '                        End Select
        '                    End If
        '                    _BackUpLoadStatus(" Set Recovery to " + dbName, True)
        '                    _BackUpBarStep()
        '                    StrSql = " ALTER DATABASE " & dbName & " SET RECOVERY FULL"
        '                    Cmd = New OleDbCommand(StrSql, Cn)
        '                    Cmd.ExecuteNonQuery()
        '                    _BackUpLoadStatus(dbName + " Set Recovery Completed..")
        '                    _BackUpBarStep()

        '                    _BackUpLoadStatus("Backup " + dbName + "..", True)
        '                    _BackUpBarStep()
        '                    StrSql = " BACKUP DATABASE [" & dbName & "] TO  DISK = N'" & tempPath & "' WITH  NOINIT ,  NOUNLOAD ,  NAME = N'" & dbName & " BackUp" & "',  NOSKIP ,  STATS = 10,  NOFORMAT "
        '                    Cmd = New OleDbCommand(StrSql, Cn)
        '                    Cmd.ExecuteNonQuery()
        '                    _BackUpLoadStatus("Backup Databases" + dbName + " Completed..")
        '                    _BackUpBarStep()
        '                    If IO.File.Exists(dbBackPath) Then
        '                        IO.File.Delete(dbBackPath)
        '                    End If
        '                    If Not tempPath.StartsWith("\\") Then
        '                        Dim ser() As String = cmbServers.Text.Split("\")
        '                        Dim servName As String = cmbServers.Text
        '                        If ser.Length > 0 Then servName = ser(0)
        '                        tempPath = "\\" & servName & "\" & tempPath.Replace(":", "")
        '                    End If
        '                    IO.File.Move(tempPath, dbBackPath)
        '                End If
        '            Next
        '            pBackUpBar.Value = pBackUpBar.Maximum
        '            MsgBox("Backup Completed")
        '            Me.DialogResult = Windows.Forms.DialogResult.OK
        '            Me.Close()
        '        Catch ex As Exception
        '            MsgBox(ex.Message)
        '            MsgBox(ex.StackTrace)
        '        End Try
    End Sub
    Private Sub _BackUpShrink(ByVal DbName As String)
        Dim CnShrink As New OleDbConnection(GetConnectionString(DbName, cmbServers.Text, txtLoginName.Text, txtPassword.Text, Mid(cmbLoginType.Text, 1, 1)))
        CnShrink.Open()
        StrSql = " SELECT NAME FROM " & DbName & "..SYSFILES WHERE FILEID = 2"
        Dim LogFile As String = GetSqlValue(StrSql, , CnShrink)
        LogFile = LogFile.Replace(Chr(10), "")
        LogFile = LogFile.Replace(Environment.NewLine, "").Trim
        _BackUpLoadStatus(DbName + " Shrink Started..", True)
        StrSql = " ALTER DATABASE " & DbName & " SET RECOVERY SIMPLE"
        _BackUpLoadStatus(DbName + " Shrinking.. SET RECOVERY SIMPLE", False)
        Cmd = New OleDbCommand(StrSql, CnShrink) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        StrSql = " ALTER DATABASE " & DbName & " SET RECOVERY FULL"
        _BackUpLoadStatus(DbName + " Shrinking.. SET RECOVERY FULL", False)
        Cmd = New OleDbCommand(StrSql, CnShrink) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        StrSql = " DBCC SHRINKFILE('" & LogFile & "',2)"
        _BackUpLoadStatus(DbName + " Shrinking..", False)
        Cmd = New OleDbCommand(StrSql, CnShrink) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        _BackUpBarStep()
        If CnShrink.State = ConnectionState.Open Then
            CnShrink.Close()
        End If
    End Sub
    Private Sub _BackUp(ByVal DbName As String, ByVal BackUpPath As String)
        _BackUpShrink(DbName)
        StrSql = " ALTER DATABASE " & DbName & " SET RECOVERY FULL"
        Cmd = New OleDbCommand(StrSql, Cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        _BackUpLoadStatus(DbName + " Set Recovery Completed..", False)
        _BackUpBarStep()

        _BackUpLoadStatus("Backup " + DbName + "..", False)
        _BackUpBarStep()
        StrSql = " BACKUP DATABASE [" & DbName & "] TO  DISK = N'" & BackUpPath & "' WITH  INIT ,  NOUNLOAD ,  NAME = N'" & DbName & " BackUp" & "',  SKIP ,  STATS = 10,  NOFORMAT "
        Cmd = New OleDbCommand(StrSql, Cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        _BackUpLoadStatus("Backup " + DbName + " Completed..")
        _BackUpBarStep()
    End Sub
    Private Function isLocalPath(ByVal path As String) As Boolean
        If path.StartsWith("\\") Then
            Dim sp() As String = path.Replace("\\", "").Split("\")
            If sp(0).ToUpper = My.Computer.Name.ToUpper Then
                Return True
            Else
                Return False
            End If
        Else
            Return True
        End If
    End Function
    Private Function _BackUpGetNetWorkAddress(ByVal Path As String, ByVal ServerName As String) As String
        Dim retPath As String = Nothing
        If Path.StartsWith("\\") Then
            retPath = Path
        Else
            retPath = Path
            retPath = "\\" & ServerName & "\" & retPath.Replace(":", "")
        End If
        Return retPath
    End Function
    Private Sub _BackUp()
        ''Validation
        If Not chkLstBackUpDataBases.CheckedItems.Count > 0 Then
            MsgBox("Please Check Database", MsgBoxStyle.Information)
            chkLstBackUpDataBases.Focus()
            Exit Sub
        End If
        If txtBackUpPath.Text = "" Then
            MsgBox("Backup Path Should Not Empty", MsgBoxStyle.Information)
            txtBackUpPath.Focus()
            Exit Sub
        ElseIf Directory.Exists(txtBackUpPath.Text) = False Then
            MsgBox("Invalid Backup Path", MsgBoxStyle.Information)
            txtBackUpPath.Focus()
            Exit Sub
        End If
        Try
            If rbtBackUpDayWise.Visible Then
                Dim dd As Date = GetSqlValue("SELECT GETDATE()")
                If rbtBackUpDayWise.Checked Then
                    FileConcat = "_" + Format(dd.Date.Day, "00") + Format(dd.Date.Month, "00") + Mid(dd.Date.Year, 3, 2)
                    '01114
                ElseIf rbtdayname.Checked Then
                    FileConcat = "_" + dd.DayOfWeek.ToString
                    '01114
                Else
                    FileConcat = "_" + Format(dd.Date.Day, "00") + Format(dd.Date.Month, "00") + Mid(dd.Date.Year, 3, 2) _
                    + "_" + Format(dd.TimeOfDay.Hours, "00") + Format(dd.TimeOfDay.Minutes, "00") + Format(dd.TimeOfDay.Seconds, "00")
                End If
            End If
            Dim ServerName As String = UCase(cmbServers.Text)
            Dim ser() As String = ServerName.Split("\")
            If ser.Length > 0 Then ServerName = ser(0)
            Dim SysFilePath As String = Nothing
            Dim SelectedDbName As String = Nothing
            Dim BackUpPath As String = Nothing
            Dim DbPath As String = Nothing
            Dim FilINfo As FileInfo
            For i As Integer = 0 To chkLstBackUpDataBases.CheckedItems.Count - 1
                SelectedDbName = chkLstBackUpDataBases.CheckedItems.Item(i).ToString
                SysFilePath = GetSqlValue("SELECT FILENAME FROM MASTER..SYSDATABASES WHERE NAME = '" & SelectedDbName & "'").ToString
                If txtBackUpPath.Text.EndsWith("\") Then
                    BackUpPath = txtBackUpPath.Text & SelectedDbName & FileConcat & ".BAK"
                Else
                    BackUpPath = txtBackUpPath.Text & "\" & SelectedDbName & FileConcat & ".BAK"
                End If
                FilINfo = New FileInfo(SysFilePath)
                DbPath = FilINfo.DirectoryName & "\" & SelectedDbName & FileConcat & ".BAK"
                If chkBackUpIsDBSystemPath.Checked Then
                    _BackUp(SelectedDbName, BackUpPath)
                Else
                    _BackUp(SelectedDbName, DbPath)
                    If IO.File.Exists(BackUpPath) Then IO.File.Delete(BackUpPath)
                    FilINfo = New FileInfo(_BackUpGetNetWorkAddress(DbPath, ServerName))
                    IO.File.Move(FilINfo.FullName, BackUpPath)
                End If
                'If My.Computer.Name.ToUpper = ServerName Then
                '    ''Selected Db is Local Db
                '    If isLocalPath(txtBackUpPath.Text) Then
                '        _BackUp(SelectedDbName, BackUpPath)
                '    Else
                '        _BackUp(SelectedDbName, DbPath)
                '        If IO.File.Exists(BackUpPath) Then IO.File.Delete(BackUpPath)
                '        IO.File.Move(DbPath, BackUpPath)
                '    End If
                'Else
                '    If txtBackUpPath.Text.StartsWith("\\" & ServerName) Then
                '        _BackUp(SelectedDbName, BackUpPath)
                '    Else
                '        _BackUp(SelectedDbName, DbPath)
                '        If IO.File.Exists(BackUpPath) Then IO.File.Delete(BackUpPath)
                '        FilINfo = New FileInfo(_BackUpGetNetWorkAddress(DbPath, ServerName))
                '        IO.File.Move(FilINfo.FullName, BackUpPath)
                '    End If
                'End If
            Next
            pBackUpBar.Value = pBackUpBar.Maximum
            MsgBox("Backup Completed")
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Sub
    Private Sub _Restore(ByVal DbName As String, ByVal Path As String)
        Dim DtRestoreFileList As DataTable
        Dim RowMdf() As DataRow = Nothing
        Dim RowLdf() As DataRow = Nothing
        StrSql = " RESTORE FILELISTONLY FROM DISK = '" & Path & "'"
        DtRestoreFileList = New DataTable
        Da = New OleDbDataAdapter(StrSql, Cn)
        Da.Fill(DtRestoreFileList)
        If DtRestoreFileList.Rows.Count > 0 Then
            RowMdf = DtRestoreFileList.Select("TYPE = 'D'")
            RowLdf = DtRestoreFileList.Select("TYPE = 'L'")
            StrSql = vbCrLf + " RESTORE DATABASE " & DbName & " FROM DISK = '" & Path & "'"
            StrSql += vbCrLf + " WITH REPLACE,MOVE '" & RowMdf(0).Item("LOGICALNAME").ToString & "' TO '" & txtRestorePath.Text & "\" & DbName & ".MDF'"
            StrSql += vbCrLf + " ,MOVE '" & RowLdf(0).Item("LOGICALNAME").ToString & "' TO '" & txtRestorePath.Text & "\" & DbName & "_LOG.LDF'"
            Cmd = New OleDbCommand(StrSql, Cn)
            Cmd.ExecuteNonQuery()
        End If
    End Sub
    Private Sub _Restore()
        ''Validation
        If Not gridRestoreFiles.RowCount > 0 Then
            MsgBox("Please Add BackUp files", MsgBoxStyle.Information)
            Exit Sub
        End If
        For Each DgvRow As DataGridViewRow In gridRestoreFiles.Rows
            If DgvRow.Cells("DBNAME").Value.ToString = "" Then
                gridRestoreFiles.CurrentCell = DgvRow.Cells("DBNAME")
                MsgBox("DbName should not empty", MsgBoxStyle.Information)
                gridRestoreFiles.Select()
                Exit Sub
            End If
        Next
        If txtRestorePath.Text.StartsWith("\\") Then
            MsgBox("Network path not support", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim ServerName As String = UCase(cmbServers.Text)
        Dim ser() As String = ServerName.Split("\")
        If ser.Length > 0 Then ServerName = ser(0)
        Dim SysFilePath As String = Nothing
        Dim SelectedDbName As String = Nothing
        Dim DbPath As String = Nothing
        Dim FilINfo As FileInfo = Nothing
        Dim TPath As String = Nothing
        Dim TPath1 As String = Nothing
        Try
            btnNext.Enabled = False
            For Each DgvRow As DataGridViewRow In gridRestoreFiles.Rows
                SelectedDbName = DgvRow.Cells("DBNAME").Value.ToString
                DbPath = DgvRow.Cells("FILEPATH").Value.ToString
                If My.Computer.Name.ToUpper = ServerName Then
                    If isLocalPath(DbPath) Then
                        _Restore(SelectedDbName, DbPath)
                    Else
                        FilINfo = New FileInfo(DbPath)
                        IO.File.Copy(DbPath, IO.Path.GetTempPath & FilINfo.Name, True)
                        _Restore(SelectedDbName, IO.Path.GetTempPath & FilINfo.Name)
                        IO.File.Delete(IO.Path.GetTempPath & FilINfo.Name)
                    End If
                Else
                    FilINfo = New FileInfo(DbPath)
                    If DbPath.StartsWith("\\" & ServerName) Then
                        _Restore(SelectedDbName, DbPath)
                    Else
                        TPath = "\\" & ServerName & "\" & txtRestorePath.Text.Replace(":", "") & IIf(txtRestorePath.Text.EndsWith("\"), "", "\") & FilINfo.Name
                        IO.File.Copy(DbPath, TPath, True)
                        _Restore(SelectedDbName, txtRestorePath.Text & IIf(txtRestorePath.Text.EndsWith("\"), "", "\") & FilINfo.Name)
                        IO.File.Delete(TPath)
                    End If
                End If
            Next
            MsgBox("Restore Completed")
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            btnNext.Enabled = True
        End Try
    End Sub
    Private Sub _AttachNewAll()
        Try
            Dim Flag As Boolean = True
            Dim Chk As Boolean = True
            For j As Integer = 0 To dgvAttach.RowCount - 1
                Dim Dbname, Mdf, Ldf As String
                Dim fil As FileInfo
                Chk = dgvAttach.Rows(j).Cells("CHK").Value
                If Chk = False Then Continue For
                Dbname = dgvAttach.Rows(j).Cells("Dbname").Value.ToString
                If Trim(Dbname.ToString) = "" Then Continue For
                Mdf = dgvAttach.Rows(j).Cells("Mdf").Value.ToString
                Ldf = dgvAttach.Rows(j).Cells("Ldf").Value.ToString
                fil = New FileInfo(Mdf)
                If Not IO.Directory.Exists(fil.Directory.FullName) Then
                    dgvAttach.Rows(j).Cells("Status").Value = "Invalid Mdf Location"
                    Flag = False
                    Continue For
                End If
                If Ldf <> "" Then
                    fil = New FileInfo(Ldf)
                    If Not IO.Directory.Exists(fil.Directory.FullName) Then
                        dgvAttach.Rows(j).Cells("Status").Value = "Invalid Ldf Location"
                        Flag = False
                        Continue For
                    End If
                End If
                If UCase(GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & Dbname & "'")).ToString <> "" Then
                    dgvAttach.Rows(j).Cells("Status").Value = "Already Attached"
                    Flag = False
                    Continue For
                End If
                Dim aPath As New FileInfo(Mdf)
                fil = New FileInfo(Mdf)
                If UCase(fil.Directory.FullName) <> UCase(aPath.Directory.FullName) Then
                    File.Copy(fil.FullName, Mdf)
                End If
                If Ldf <> "" Then
                    fil = New FileInfo(Ldf)
                    If UCase(fil.Directory.FullName) <> UCase(aPath.Directory.FullName) Then
                        File.Copy(fil.FullName, Ldf)
                    End If
                End If
                StrSql = " SP_ATTACH_DB " + Dbname
                StrSql += " ,@FILENAME1 = '" & Mdf & "'"
                If Ldf <> "" Then
                    StrSql += " ,@FILENAME2 = '" & Ldf & "'"
                End If
                Cmd = New OleDbCommand(StrSql, Cn)
                Cmd.ExecuteNonQuery()
                dgvAttach.Rows(j).Cells("Status").Value = "Attached"
            Next
            dgvAttach.Columns("Status").DefaultCellStyle.ForeColor = Color.Blue
            If Flag = False Then
                MsgBox("Attach Failed,See Status for More details", MsgBoxStyle.Information)
            Else
                MsgBox("Attach Completed", MsgBoxStyle.Information)
                'FuncNew()
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub _DetachAll()
        Try
            Dim Chk As Boolean = True
            For i As Integer = 0 To dgvDetach.RowCount - 1
                Dim Dbname As String
                Chk = dgvDetach.Rows(i).Cells("CHK").Value
                If Chk = False Then Continue For
                Dbname = dgvDetach.Rows(i).Cells("Dbname").Value.ToString
                If Trim(Dbname.ToString) = "" Then Continue For
                If UCase(GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & Dbname & "'")).ToString = "" Then
                    dgvDetach.Rows(i).Cells("Status").Value = "Invalid Database"
                    Continue For
                End If
                StrSql = "SP_DETACH_DB " + Dbname.ToString
                Cmd = New OleDbCommand(StrSql, Cn)
                Cmd.ExecuteNonQuery()
                dgvDetach.Rows(i).Cells("Status").Value = "Detached"
                dgvDetach.Columns("Status").DefaultCellStyle.ForeColor = Color.Red
            Next

            MsgBox("Detach Competed")
            'Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        End Try
    End Sub
    Public Sub FuncAddDtAttach()

        dtAttachAll = New DataTable()
        dtAttachAll.Columns.Add("CHK", GetType(Boolean))
        dtAttachAll.Columns("CHK").DefaultValue = chkSelectAll.Checked
        dtAttachAll.Columns.Add(New DataColumn("Dbname", Type.GetType("System.String")))
        dtAttachAll.Columns.Add(New DataColumn("Status", Type.GetType("System.String")))
        dtAttachAll.Columns.Add(New DataColumn("Mdf", Type.GetType("System.String")))
        dtAttachAll.Columns.Add(New DataColumn("Ldf", Type.GetType("System.String")))

    End Sub
    Private Sub _BackUpLoadDatabase()
        chkLstBackUpDataBases.Items.Clear()
        Dim DtDb As New DataTable
        StrSql = " SELECT NAME FROM MASTER..SYSDATABASES  "
        StrSql += " WHERE NAME NOT IN ('MASTER','TEMPDB','MODEL','MSDB')"
        StrSql += " ORDER BY NAME"
        Da = New OleDbDataAdapter(StrSql, Cn)
        Da.Fill(DtDb)
        For Each ro As DataRow In DtDb.Rows
            chkLstBackUpDataBases.Items.Add(ro("NAME").ToString)
        Next
        If Not My.Settings.BackUpDbNames Is Nothing Then
            For cnt As Integer = 0 To chkLstBackUpDataBases.Items.Count - 1
                If My.Settings.BackUpDbNames.Contains(chkLstBackUpDataBases.Items(cnt).ToString) Then
                    chkLstBackUpDataBases.SetItemChecked(cnt, True)
                End If
            Next
        End If
    End Sub
    Private Sub _BackUpBarStep(Optional ByVal stepVal As Integer = 5)
        If pBackUpBar.Value + stepVal > pBackUpBar.Maximum Then pBackUpBar.Value = 0
        pBackUpBar.Value += stepVal
        pBackUpBar.Refresh()
    End Sub
    Private Sub _BackUpLoadStatus(ByVal status As String, Optional ByVal newRow As Boolean = False)
        If newRow Then
            DtStatus.Rows.Add()
            DtStatus.AcceptChanges()
        End If
        DtStatus.Rows(DtStatus.Rows.Count - 1).Item("STATUS") = status
        gridBackUpStatus.CurrentCell = gridBackUpStatus.Rows(gridBackUpStatus.RowCount - 1).Cells(0)
        gridBackUpStatus.Refresh()
    End Sub

    ''start
    Private Sub _AttachGetFileLocationsAll()
        Dim FInfo As FileInfo = Nothing
        Dim file1 As String = Nothing
        Dim file1Path As String = Nothing
        Dim file2 As String = Nothing
        Dim file2Path As String = Nothing
        Dim i As Integer
        Try

            FuncAddDtAttach()
            If StrDbPath = "" Then Exit Sub
            'If Not IO.File.Exists(txtAttachFilePAth.Text) Then Exit Sub
            FInfo = New FileInfo(StrDbPath & "\")
            file1 = Trim(FInfo.Name)
            file1Path = Trim(FInfo.FullName)
            'StrDbMdfPath = file1Path
            'StrAttachDbName = file1.Replace(".mdf", "").Replace(".MDF", "")
            i = 0
            For Each file As String In Directory.GetFiles(FInfo.DirectoryName, "*.mdf")
                FInfo = New FileInfo(file)
                file2 = FInfo.Name
                file2Path = FInfo.FullName
                StrDbMdfPath(i) = file2Path
                StrAttachDbName(i) = file2.Replace(".mdf", "").Replace(".MDF", "")
                i = i + 1
                If i > 10 Then
                    'Exit For
                End If
            Next
            i = 0
            For Each file As String In Directory.GetFiles(FInfo.DirectoryName, "*.ldf")
                FInfo = New FileInfo(file)
                file2 = FInfo.Name
                file2Path = FInfo.FullName
                StrDbLdfPath(i) = file2Path
                i = i + 1
                If i > 10 Then
                    'Exit For
                End If
            Next
            Dim Flag As Boolean = True
            For j As Integer = 0 To StrAttachDbName.Length - 1
                Dim dr As DataRow
                Dim StrDbName As String = StrAttachDbName(j)
                If Trim(StrDbName) = "" Then Exit For
                dr = dtAttachAll.NewRow()
                dr("Dbname") = StrAttachDbName(j)
                dr("Mdf") = StrDbMdfPath(j)
                dr("Ldf") = StrDbLdfPath(j)
                If UCase(GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE FILENAME = '" & StrDbMdfPath(j) & "'")).ToString <> "" Then
                    dr("Status") = "Attached"
                    Flag = False
                Else
                    dr("Status") = ""
                End If
                dtAttachAll.Rows.Add(dr)
                dtAttachAll.AcceptChanges()
                If j > 9 Then
                    'Exit For
                End If
            Next

            dgvAttach.DataSource = Nothing
            dgvAttach.DataSource = dtAttachAll
            dgvAttach.ColumnHeadersDefaultCellStyle.Font = New Font("Verdana", 8, FontStyle.Bold)
            dgvAttach.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvAttach.Columns("CHK").Width = 30
            Panel2.Visible = True
            If Flag = False Then
                MsgBox("Database Already Attached,See Status for More details", MsgBoxStyle.Information)
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub _DetachGetFileLocationsAll()
        Dim FInfo As FileInfo = Nothing
        Dim file1 As String = Nothing
        Dim file1Path As String = Nothing
        Dim file2 As String = Nothing
        Dim file2Path As String = Nothing
        Dim i As Integer
        Try

            FuncAddDtAttach()
            If StrDbPath = "" Then Exit Sub
            'If Not IO.File.Exists(txtAttachFilePAth.Text) Then Exit Sub
            FInfo = New FileInfo(StrDbPath & "\")
            file1 = Trim(FInfo.Name)
            file1Path = Trim(FInfo.FullName)
            'StrDbMdfPath = file1Path
            'StrAttachDbName = file1.Replace(".mdf", "").Replace(".MDF", "")
            i = 0
            For Each file As String In Directory.GetFiles(FInfo.DirectoryName, "*.mdf")
                FInfo = New FileInfo(file)
                file2 = FInfo.Name
                file2Path = FInfo.FullName
                StrDbMdfPath(i) = file2Path
                StrAttachDbName(i) = file2.Replace(".mdf", "").Replace(".MDF", "")
                i = i + 1
                If i > 10 Then
                    Exit For
                End If
            Next
            i = 0
            For Each file As String In Directory.GetFiles(FInfo.DirectoryName, "*.ldf")
                FInfo = New FileInfo(file)
                file2 = FInfo.Name
                file2Path = FInfo.FullName
                StrDbLdfPath(i) = file2Path
                i = i + 1
                If i > 10 Then
                    Exit For
                End If
            Next
            Dim Flag As Boolean = True
            For j As Integer = 0 To StrAttachDbName.Length - 1
                Dim dr As DataRow
                Dim StrDbName As String = StrAttachDbName(j)
                If Trim(StrDbName) = "" Then Exit For
                dr = dtAttachAll.NewRow()
                dr("Dbname") = StrAttachDbName(j)
                dr("Mdf") = StrDbMdfPath(j)
                dr("Ldf") = StrDbLdfPath(j)
                If UCase(GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE FILENAME = '" & StrDbMdfPath(j) & "'")).ToString <> "" Then
                    dr("Status") = "Attached"
                    Flag = False
                Else
                    dr("Status") = ""
                End If
                dtAttachAll.Rows.Add(dr)
                dtAttachAll.AcceptChanges()
                If j > 9 Then
                    Exit For
                End If
            Next

            dgvDetach.DataSource = Nothing
            dgvDetach.DataSource = dtAttachAll
            dgvDetach.ColumnHeadersDefaultCellStyle.Font = New Font("Verdana", 8, FontStyle.Bold)
            dgvDetach.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvDetach.Columns("CHK").Width = 30
            Panel2.Visible = True
            If Flag = False Then
                'MsgBox("Database Already Attached,See Status for More details", MsgBoxStyle.Information)
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub btnBackUpBrowsePath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackUpBrowsePath.Click
        Dim openFolderDia As New FolderBrowserDialog
        If openFolderDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtBackUpPath.Text = openFolderDia.SelectedPath
            SendKeys.Send("{TAB}")
        End If
    End Sub
#End Region
#Region "Attach Methods & Events"
    Private Sub _AttachGetFileLocations()
        Dim FInfo As FileInfo = Nothing
        Dim file1 As String = Nothing
        Dim file1Path As String = Nothing
        Dim file2 As String = Nothing
        Dim file2Path As String = Nothing
        If txtAttachSourcePath.Text = "" Then Exit Sub
        If Not IO.File.Exists(txtAttachSourcePath.Text) Then Exit Sub

        FInfo = New FileInfo(txtAttachSourcePath.Text)
        file1 = Trim(FInfo.Name)
        file1Path = Trim(FInfo.FullName)
        txtAttachMdfLocation.Text = file1Path
        txtAttachDatabaseName.Text = file1.Replace(".mdf", "").Replace(".MDF", "")
        For Each file As String In Directory.GetFiles(FInfo.DirectoryName, "*.ldf")
            FInfo = New FileInfo(file)
            Dim tFile1 As String = UCase(file1.Replace(".mdf", "")).Replace(".MDF", "")
            If UCase(FInfo.Name).StartsWith(tFile1) Then
                file2 = FInfo.Name
                file2Path = FInfo.FullName
                txtAttachLdfLocation.Text = file2Path
                Exit For
            End If
        Next
        If Not DtCommandLineArguments.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dtAttach.NewRow()
            dr("Dbname") = txtAttachDatabaseName.Text
            dr("Mdf") = txtAttachMdfLocation.Text
            dr("Ldf") = txtAttachLdfLocation.Text
            dr("Status") = ""
            dtAttach.Rows.Add(dr)
            dtAttach.AcceptChanges()
            GridViewAttach.DataSource = Nothing
            GridViewAttach.DataSource = dtAttach
            GridViewAttach.ColumnHeadersDefaultCellStyle.Font = New Font("Verdana", 8, FontStyle.Bold)
            GridViewAttach.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            txtAttachMdfLocation.Clear()
            txtAttachLdfLocation.Clear()
            txtAttachDatabaseName.Clear()
            txtAttachSourcePath.Clear()
            btnAttachBrowseSource.Focus()
            PnlAttachGrid.Visible = True
        End If
    End Sub

    Private Sub btnAttachBrowseSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttachBrowseSource.Click
        Dim fileOpenDia As New OpenFileDialog
        fileOpenDia.Filter = "Master Data Files (*.mdf)|*.mdf"
        Dim FInfo As FileInfo = Nothing
        Dim file1 As String = Nothing
        Dim file1Path As String = Nothing
        Dim file2 As String = Nothing
        Dim file2Path As String = Nothing
        If fileOpenDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.Refresh()
            If UCase(GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE FILENAME = '" & fileOpenDia.FileName & "'")).ToString <> "" Then
                MsgBox("Already Attached..", MsgBoxStyle.Information)
                Exit Sub
            End If
            txtAttachSourcePath.Text = fileOpenDia.FileName
            _AttachGetFileLocations()
            Me.Refresh()
        End If
    End Sub
    Private Sub _AttachNew()
        Try
            Dim Flag As Boolean = True
            For j As Integer = 0 To GridViewAttach.RowCount - 1
                Dim Dbname, Mdf, Ldf As String
                Dim fil As FileInfo
                Dbname = GridViewAttach.Rows(j).Cells("Dbname").Value.ToString
                Mdf = GridViewAttach.Rows(j).Cells("Mdf").Value.ToString
                Ldf = GridViewAttach.Rows(j).Cells("Ldf").Value.ToString
                fil = New FileInfo(Mdf)
                If Not IO.Directory.Exists(fil.Directory.FullName) Then
                    GridViewAttach.Rows(j).Cells("Status").Value = "Invalid Mdf Location"
                    Flag = False
                    Continue For
                End If
                If Ldf <> "" Then
                    fil = New FileInfo(Ldf)
                    If Not IO.Directory.Exists(fil.Directory.FullName) Then
                        GridViewAttach.Rows(j).Cells("Status").Value = "Invalid Ldf Location"
                        Flag = False
                        Continue For
                    End If
                End If
                If UCase(GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & Dbname & "'")).ToString <> "" Then
                    GridViewAttach.Rows(j).Cells("Status").Value = "Already Attached"
                    Flag = False
                    Continue For
                End If
                Dim aPath As New FileInfo(Mdf)
                fil = New FileInfo(Mdf)
                If UCase(fil.Directory.FullName) <> UCase(aPath.Directory.FullName) Then
                    File.Copy(fil.FullName, Mdf)
                End If
                If Ldf <> "" Then
                    fil = New FileInfo(Ldf)
                    If UCase(fil.Directory.FullName) <> UCase(aPath.Directory.FullName) Then
                        File.Copy(fil.FullName, Ldf)
                    End If
                End If
                StrSql = " SP_ATTACH_DB " + Dbname
                StrSql += " ,@FILENAME1 = '" & Mdf & "'"
                If Ldf <> "" Then
                    StrSql += " ,@FILENAME2 = '" & Ldf & "'"
                End If
                Cmd = New OleDbCommand(StrSql, Cn)
                Cmd.ExecuteNonQuery()
                GridViewAttach.Rows(j).Cells("Status").Value = "Attached"
            Next
            GridViewAttach.Columns("Status").DefaultCellStyle.ForeColor = Color.Blue
            If Flag = False Then
                MsgBox("Attach Failed,See Status for More details", MsgBoxStyle.Information)
            Else
                MsgBox("Attach Completed", MsgBoxStyle.Information)
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub _Attach()
        Try
            Dim fil As FileInfo
            fil = New FileInfo(txtAttachMdfLocation.Text)
            If Not IO.Directory.Exists(fil.Directory.FullName) Then
                MsgBox("Invalid Mdf Location", MsgBoxStyle.Information)
                txtAttachMdfLocation.Select()
                Exit Sub
            End If
            If txtAttachLdfLocation.Text <> "" Then
                fil = New FileInfo(txtAttachLdfLocation.Text)
                If Not IO.Directory.Exists(fil.Directory.FullName) Then
                    MsgBox("Invalid Ldf Location", MsgBoxStyle.Information)
                    txtAttachLdfLocation.Select()
                    Exit Sub
                End If
            End If
            If UCase(GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & txtAttachDatabaseName.Text & "'")).ToString <> "" Then
                MsgBox("Already Attached..", MsgBoxStyle.Information)
                Exit Sub
            End If


            Dim aPath As New FileInfo(txtAttachSourcePath.Text)
            fil = New FileInfo(txtAttachMdfLocation.Text)
            If UCase(fil.Directory.FullName) <> UCase(aPath.Directory.FullName) Then
                File.Copy(fil.FullName, txtAttachSourcePath.Text)
            End If
            If txtAttachLdfLocation.Text <> "" Then
                fil = New FileInfo(txtAttachLdfLocation.Text)
                If UCase(fil.Directory.FullName) <> UCase(aPath.Directory.FullName) Then
                    File.Copy(fil.FullName, txtAttachSourcePath.Text)
                End If
            End If

            StrSql = " SP_ATTACH_DB " + Trim(txtAttachDatabaseName.Text)
            StrSql += " ,@FILENAME1 = '" & txtAttachMdfLocation.Text & "'"
            If txtAttachLdfLocation.Text <> "" Then ''Single File Attach
                StrSql += " ,@FILENAME2 = '" & txtAttachLdfLocation.Text & "'"
            End If
            Cmd = New OleDbCommand(StrSql, Cn)
            Cmd.ExecuteNonQuery()

            MsgBox("Attach Completed")
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
#End Region
#Region "Detach Methods & Events"

    Private Sub _DetachLoadDatabase()
        cmbDetachDatabase.Items.Clear()
        chkcmbDetachDatabase.Items.Clear()
        Dim DtDb As New DataTable
        StrSql = " SELECT NAME FROM MASTER..SYSDATABASES "
        StrSql += " WHERE NAME NOT IN ('MASTER','TEMPDB','MODEL','MSDB')"
        StrSql += " ORDER BY NAME"
        Da = New OleDbDataAdapter(StrSql, Cn)
        Da.Fill(DtDb)
        For Each ro As DataRow In DtDb.Rows
            cmbDetachDatabase.Items.Add(ro!NAME.ToString)
            chkcmbDetachDatabase.Items.Add(ro!NAME.ToString)
        Next
    End Sub

    Private Sub _Detach()
        Try
            'If cmbDetachDatabase.Text = "" Or Not cmbDetachDatabase.Items.Contains(cmbDetachDatabase.Text) Then
            '    MsgBox("Invalid Database", MsgBoxStyle.Information)
            '    cmbDetachDatabase.Focus()
            '    Exit Sub
            'End If
            If chkcmbDetachDatabase.Text = "" Then
                MsgBox("Invalid Database", MsgBoxStyle.Information)
                cmbDetachDatabase.Focus()
                Exit Sub
            End If
            For i As Integer = 0 To chkcmbDetachDatabase.CheckedItems.Count - 1
                StrSql = "SP_DETACH_DB " + chkcmbDetachDatabase.CheckedItems.Item(i).ToString()
                Cmd = New OleDbCommand(StrSql, Cn)
                Cmd.ExecuteNonQuery()
            Next
            'StrSql = "SP_DETACH_DB " + cmbDetachDatabase.Text
            'Cmd = New OleDbCommand(StrSql, Cn)
            'Cmd.ExecuteNonQuery(l)
            MsgBox("Detach Competed")
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        End Try
    End Sub
#End Region
#Region "Release Methods & Events"
    Private Sub _Release()
        If CmbRDataBase.Text = String.Empty And chkDroptempdb.Checked = False Then MsgBox("Select Database", MsgBoxStyle.Information) : Exit Sub
        Me.Cursor = Cursors.WaitCursor
        If chkMemory.Checked Then
            FuncRealseMemory(CmbRDataBase.Text)
        ElseIf chkDroptempdb.Checked Then
            DropTempTableDb()
        ElseIf chkIndex.Checked Then
            'FuncRebuildIndex(CmbRDataBase.Text)
            FuncRebuildIndexNew(CmbRDataBase.Text)
        ElseIf CkhRebuidDatabase.Checked Then
            FuncRebuildDatabase(CmbRDataBase.Text)
        ElseIf ChkSyncData.Checked Then
            FuncSyncDataDelete(dtpSyncDate.Value, CmbRDataBase.Text)
        End If
        Me.Cursor = Cursors.Arrow
        funcUncheck()
    End Sub
    Function FuncSyncDataDelete(ByVal SyncDate As Date, ByVal dbname As String)
        Try
            Dim muid As Integer = 0
            StrSql = "SELECT 1 FROM " & dbname & "..SYSOBJECTS WHERE NAME ='SENDSYNC'"
            If GetSqlValue(StrSql, , Cn) = "" Then MsgBox("Sync Table Not Exists", MsgBoxStyle.Information) : Exit Function
            StrSql = "SELECT MAX(UID) UID FROM " & dbname & "..SENDSYNC WHERE STATUS='M' AND SUBSTRING(UPDFILE,15,6)='" & Replace(Format(SyncDate, "dd-MM-yy"), "-", "") & "'"
            muid = Val(GetSqlValue(StrSql, , Cn))
            StrSql = "DELETE FROM " & dbname & "..SENDSYNC WHERE STATUS <> 'N' AND UID <" & muid
            Cmd = New OleDbCommand(StrSql, Cn)
            Cmd.ExecuteNonQuery()
            muid = Val(GetSqlValue("SELECT MAX(UID) FROM " & dbname & "..RECEIVESYNC WHERE STATUS<>'N' AND SUBSTRING(UPDFILE,15,6)='" & Replace(Format(SyncDate, "dd-MM-yy"), "-", "") & "'"))
            StrSql = "DELETE FROM " & dbname & "..RECEIVESYNC WHERE STATUS='Y' AND UID < " & muid
            Cmd = New OleDbCommand(StrSql, Cn)
            Cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message + ex.StackTrace, MsgBoxStyle.Information)
            Me.Cursor = Cursors.Arrow
            Return False
        End Try
        MsgBox("Completed", MsgBoxStyle.Information)
        Return True
    End Function
    Function FuncRebuildDatabase(ByVal dbname As String) As Boolean
        StrSql = "USE " & dbname
        StrSql += vbCrLf + "ALTER DATABASE " & dbname & " SET SINGLE_USER WITH ROLLBACK IMMEDIATE"
        StrSql += vbCrLf + "DBCC CHECKDB('" & dbname & "', REPAIR_REBUILD)"
        StrSql += vbCrLf + "ALTER DATABASE " & dbname & " SET MULTI_USER"
        Try
            Cmd = New OleDbCommand(StrSql, Cn)
            Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message + ex.StackTrace, MsgBoxStyle.Information)
            Me.Cursor = Cursors.Arrow
            Return False
        End Try
        MsgBox("Completed", MsgBoxStyle.Information)
        Return True
    End Function
    Function FuncRebuildIndexNew(ByVal dbname As String) As Boolean
        Try
            StrSql = "USE " & dbname
            StrSql += vbCrLf + " IF OBJECT_ID('TEMPINDEX','U') IS NOT NULL DROP TABLE TEMPINDEX"
            StrSql += vbCrLf + " SELECT "
            StrSql += vbCrLf + " I.NAME AS INDEXNAME, "
            StrSql += vbCrLf + " O.NAME AS TABLENAME, "
            StrSql += vbCrLf + " IC.KEY_ORDINAL AS COLUMNORDER,"
            StrSql += vbCrLf + " IC.IS_INCLUDED_COLUMN AS ISINCLUDED, "
            StrSql += vbCrLf + " CO.[NAME] AS COLUMNNAME"
            StrSql += vbCrLf + " INTO TEMPINDEX FROM SYS.INDEXES I "
            StrSql += vbCrLf + " JOIN SYS.OBJECTS O ON I.OBJECT_ID = O.OBJECT_ID"
            StrSql += vbCrLf + " JOIN SYS.INDEX_COLUMNS IC ON IC.OBJECT_ID = I.OBJECT_ID "
            StrSql += vbCrLf + " AND IC.INDEX_ID = I.INDEX_ID"
            StrSql += vbCrLf + " JOIN SYS.COLUMNS CO ON CO.OBJECT_ID = I.OBJECT_ID "
            StrSql += vbCrLf + " AND CO.COLUMN_ID = IC.COLUMN_ID"
            StrSql += vbCrLf + " WHERE I.[TYPE] = 2 "
            StrSql += vbCrLf + " AND I.IS_UNIQUE = 0 "
            StrSql += vbCrLf + " AND I.IS_PRIMARY_KEY = 0"
            StrSql += vbCrLf + " AND O.[TYPE] = 'U'"
            StrSql += vbCrLf + " ORDER BY O.[NAME], I.[NAME], IC.IS_INCLUDED_COLUMN, IC.KEY_ORDINAL"
            Cmd = New OleDbCommand(StrSql, Cn)
            Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = vbCrLf + " DECLARE @TBLNAME VARCHAR(50)"
            StrSql += vbCrLf + " DECLARE @INDEXNAME VARCHAR(50)"
            StrSql += vbCrLf + " DECLARE @COLNAME VARCHAR(50)"
            StrSql += vbCrLf + " DECLARE @QRY NVARCHAR(4000)"
            StrSql += vbCrLf + " DECLARE CUR CURSOR FOR SELECT DISTINCT TABLENAME,INDEXNAME FROM TEMPINDEX "
            StrSql += vbCrLf + " OPEN CUR"
            StrSql += vbCrLf + " FETCH NEXT FROM CUR INTO @TBLNAME,@INDEXNAME"
            StrSql += vbCrLf + " WHILE @@FETCH_STATUS<>-1"
            StrSql += vbCrLf + " BEGIN"
            StrSql += vbCrLf + " 	SELECT @QRY='DROP INDEX ' + @TBLNAME + '.'+ @INDEXNAME"
            StrSql += vbCrLf + " 	EXEC (@QRY)"
            StrSql += vbCrLf + " 	SELECT @QRY='CREATE NONCLUSTERED INDEX ' +@INDEXNAME + ' ON ' + @TBLNAME + '('"
            StrSql += vbCrLf + " 	DECLARE INDCUR CURSOR FOR SELECT COLUMNNAME FROM TEMPINDEX WHERE INDEXNAME=@INDEXNAME ORDER BY COLUMNORDER"
            StrSql += vbCrLf + " 	OPEN INDCUR"
            StrSql += vbCrLf + " 	FETCH NEXT FROM INDCUR INTO @COLNAME"
            StrSql += vbCrLf + " 	WHILE @@FETCH_STATUS<>-1"
            StrSql += vbCrLf + " 	BEGIN"
            StrSql += vbCrLf + " 		SELECT @QRY=@QRY+ @COLNAME + ','"
            StrSql += vbCrLf + " 		FETCH NEXT FROM INDCUR INTO @COLNAME"
            StrSql += vbCrLf + " 	END"
            StrSql += vbCrLf + " 	SELECT @QRY=SUBSTRING(@QRY,1,LEN(@QRY)-1)"
            StrSql += vbCrLf + " 	SELECT @QRY=@QRY+')'"
            StrSql += vbCrLf + " 	EXEC(@QRY)"
            StrSql += vbCrLf + " 	CLOSE INDCUR"
            StrSql += vbCrLf + " 	DEALLOCATE INDCUR"
            StrSql += vbCrLf + " 	FETCH NEXT FROM CUR INTO @TBLNAME,@INDEXNAME"
            StrSql += vbCrLf + " END"
            StrSql += vbCrLf + " CLOSE CUR"
            StrSql += vbCrLf + " DEALLOCATE CUR"
            Cmd = New OleDbCommand(StrSql, Cn)
            Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message + ex.StackTrace, MsgBoxStyle.Information)
            Me.Cursor = Cursors.Arrow
            Return False
        End Try
        MsgBox("Completed", MsgBoxStyle.Information)
        Return True
    End Function
    Function FuncRebuildIndex(ByVal dbname As String) As Boolean
        Try
            StrSql = vbCrLf + "DECLARE @DATABASE VARCHAR(255)  "
            StrSql += vbCrLf + "DECLARE @TABLE VARCHAR(255)  "
            StrSql += vbCrLf + "DECLARE @CMD NVARCHAR(500)  "
            StrSql += vbCrLf + "DECLARE @FILLFACTOR INT"
            StrSql += vbCrLf + "SET @FILLFACTOR = 90"
            StrSql += vbCrLf + "SET @DATABASE='" & dbname & "'"
            StrSql += vbCrLf + "SET @CMD = 'DECLARE TABLECURSOR CURSOR FOR SELECT ''['' + TABLE_CATALOG + ''].['' + TABLE_SCHEMA + ''].['' +"
            StrSql += vbCrLf + "TABLE_NAME + '']'' AS TABLENAME FROM [' + @DATABASE + '].INFORMATION_SCHEMA.TABLES"
            StrSql += vbCrLf + "WHERE TABLE_TYPE = ''BASE TABLE'''  "
            StrSql += vbCrLf + "-- CREATE TABLE CURSOR  "
            StrSql += vbCrLf + "EXEC (@CMD)  "
            StrSql += vbCrLf + "OPEN TABLECURSOR  "
            StrSql += vbCrLf + "FETCH NEXT FROM TABLECURSOR INTO @TABLE  "
            StrSql += vbCrLf + "WHILE @@FETCH_STATUS = 0  "
            StrSql += vbCrLf + "BEGIN  "
            StrSql += vbCrLf + "    IF (@@MICROSOFTVERSION / POWER(2, 24) >= 9)"
            StrSql += vbCrLf + "    BEGIN"
            StrSql += vbCrLf + "        -- SQL 2005 OR HIGHER COMMAND"
            StrSql += vbCrLf + "        SET @CMD = 'ALTER INDEX ALL ON ' + @TABLE + ' REBUILD WITH (FILLFACTOR = ' + CONVERT(VARCHAR(3),@FILLFACTOR) + ')'"
            StrSql += vbCrLf + "        EXEC (@CMD)"
            StrSql += vbCrLf + "    END"
            StrSql += vbCrLf + "    ELSE"
            StrSql += vbCrLf + "    BEGIN"
            StrSql += vbCrLf + "	   -- SQL 2000 COMMAND"
            StrSql += vbCrLf + "       DBCC DBREINDEX(@TABLE,' ',@FILLFACTOR)  "
            StrSql += vbCrLf + "    END"
            StrSql += vbCrLf + "    FETCH NEXT FROM TABLECURSOR INTO @TABLE  "
            StrSql += vbCrLf + "END  "
            StrSql += vbCrLf + "CLOSE TABLECURSOR  "
            StrSql += vbCrLf + "DEALLOCATE TABLECURSOR  "
            Cmd = New OleDbCommand(StrSql, Cn)
            Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message + ex.StackTrace, MsgBoxStyle.Information)
            Me.Cursor = Cursors.Arrow
            Return False
        End Try
        MsgBox("Completed", MsgBoxStyle.Information)
        Return True
    End Function
    Private Sub DropTempTableDb_OLD()
        ''StrSql = "IF EXISTS( SELECT * FROM MASTER..SYSDATABASES WHERE NAME='TEMPTABLEDB') DROP DATABASE TEMPTABLEDB"
        ''Cmd = New OleDbCommand(StrSql, Cn)
        ''Cmd.ExecuteNonQuery()
        ''StrSql = "CREATE DATABASE TEMPTABLEDB"
        ''Cmd = New OleDbCommand(StrSql, Cn)
        ''Cmd.ExecuteNonQuery()
        ''MsgBox("Completed", MsgBoxStyle.Information)
    End Sub

    Private Sub DropTempTableDb()
        Dim dtdb As New DataTable
        StrSql = " IF EXISTS( SELECT * FROM MASTER..SYSDATABASES WHERE NAME='TEMPTABLEDB') "
        StrSql += " BEGIN "
        StrSql += "  SELECT * FROM TEMPTABLEDB.SYS.DATABASE_FILES "
        StrSql += " END"
        Da = New OleDbDataAdapter(StrSql, Cn)
        Da.Fill(dtdb)
        StrSql = "If EXISTS( Select * FROM MASTER..SYSDATABASES WHERE NAME='TEMPTABLEDB') DROP DATABASE TEMPTABLEDB"
        Cmd = New OleDbCommand(StrSql, Cn)
        Cmd.ExecuteNonQuery()

        StrSql = " CREATE DATABASE [TEMPTABLEDB] "
        If dtdb.Rows.Count > 0 And dtdb.Rows.Count = 2 Then
            Dim MdfFileName As String = ""
            Dim LdfFileName As String = ""
            MdfFileName = dtdb.Rows(0).Item("physical_name").ToString
            LdfFileName = dtdb.Rows(1).Item("physical_name").ToString
            If MdfFileName.ToString <> "" And LdfFileName.ToString <> "" Then
                StrSql += " ON  PRIMARY "
                StrSql += " ( NAME = N'TEMPTABLEDB_dat', FILENAME = N'" & MdfFileName.ToString & "' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )"
                StrSql += "  LOG ON "
                StrSql += " ( NAME = N'TEMPTABLEDB_log', FILENAME = N'" & LdfFileName.ToString & "' , SIZE = 512KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)"
            End If
        End If
        Cmd = New OleDbCommand(StrSql, Cn)
        Cmd.ExecuteNonQuery()
        MsgBox("Completed", MsgBoxStyle.Information)
    End Sub

    Function FuncRealseMemory(ByVal dbname As String) As Boolean
        Dim dt As New DataTable
        _DropDb = dbname
        Try
            StrSql = "SELECT NAME FROM " & dbname & "..SYSOBJECTS WHERE XTYPE='U' AND NAME LIKE 'TEMP%'"
            Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
            dtCol.DefaultValue = True
            dt.Columns.Add(dtCol)
            Da = New OleDbDataAdapter(StrSql, Cn)
            Da.Fill(dt)
            GridView.DataSource = Nothing
            If dt.Rows.Count > 0 Then
                GridView.DataSource = dt
                tabMain.TabPages.Clear()
                tabMain.TabPages.Add(tabView)
                btnBack.Visible = False : btnCancel.Visible = False : btnNext.Visible = False
                With GridView
                    .Columns("NAME").Width = 250
                    .Columns("NAME").ReadOnly = True
                End With
            Else
                MsgBox("TempTable Not Found", MsgBoxStyle.Information)
                Exit Function
            End If
        Catch ex As Exception
            MsgBox(ex.Message + ex.StackTrace, MsgBoxStyle.Information)
            Me.Cursor = Cursors.Arrow
            Return False
        End Try
        Return True
    End Function
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim dr() As DataRow
        dr = CType(GridView.DataSource, DataTable).Select("CHECK = TRUE")
        If Not dr.Length > 0 Then
            MsgBox("There is no selected record", MsgBoxStyle.Information)
            Exit Sub
        End If
        For Each ro As DataRow In dr
            StrSql = "DROP TABLE " & _DropDb & ".." & ro.Item("NAME").ToString
            Cmd = New OleDbCommand(StrSql, Cn)
            Cmd.ExecuteNonQuery()
        Next
        MsgBox("Completed", MsgBoxStyle.Information)
        btnVBack_Click(Me, New EventArgs)
    End Sub
    Private Sub _ReleaseLoadDatabase()
        LoadComboDatabaseName(CmbRDataBase)
    End Sub
    Private Sub LoadComboDatabaseName(ByVal Cmb As ComboBox)
        Dim dtcol As New DataTable
        Dim dt As New DataTable
        StrSql = "SELECT NAME FROM MASTER..SYSDATABASES "
        StrSql += vbCrLf + " WHERE NAME NOT IN ('MASTER','TEMPDB','MODEL','MSDB') ORDER BY NAME"
        Da = New OleDbDataAdapter(StrSql, Cn)
        Da.Fill(dtcol)
        For Each ro As DataRow In dtcol.Rows
            Cmb.Items.Add(ro("NAME").ToString)
        Next
    End Sub
    Private Sub funcUncheck()
        chkIndex.Checked = False
        chkMemory.Checked = True
        CkhRebuidDatabase.Checked = False
        ChkSyncData.Checked = False
        LblDelete.Visible = False
        dtpSyncDate.Visible = False
    End Sub
    Private Sub ChkSyncData_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkSyncData.CheckedChanged
        If ChkSyncData.Checked = True Then
            LblDelete.Visible = True
            dtpSyncDate.Visible = True
        Else
            LblDelete.Visible = False
            dtpSyncDate.Visible = False
        End If
    End Sub
#End Region
#Region "Recover Methods & Events"
    Private Sub LoadComboDatabaseName()
        Dim dtcol As New DataTable
        StrSql = vbCrLf + " SELECT NAME AS SUSPECTDB,"
        StrSql += vbCrLf + " DATABASEPROPERTY(NAME, N'ISSUSPECT') AS SUSPECT"
        StrSql += vbCrLf + " FROM SYSDATABASES"
        StrSql += vbCrLf + " WHERE (DATABASEPROPERTY(NAME, N'ISSUSPECT') = 1)"
        Da = New OleDbDataAdapter(StrSql, Cn)
        Da.Fill(dtcol)
        For Each ro As DataRow In dtcol.Rows
            CmbRcDatabase.Items.Add(ro("SUSPECTDB").ToString)
        Next
    End Sub
    Private Sub _RecoverLoadDatabase()
        LoadComboDatabaseName()
    End Sub
    Private Sub _recover()
        If CmbRcDatabase.Text = String.Empty Then MsgBox("Select Database", MsgBoxStyle.Information) : Exit Sub
        Dim dt As New DataTable
        Dim LogPath As String
        Try
            StrSql = vbCrLf + " SP_CONFIGURE 'ALLOW UPDATES', 1"
            StrSql = vbCrLf + " GO"
            StrSql = vbCrLf + " RECONFIGURE WITH OVERRIDE"
            StrSql = vbCrLf + " GO"
            StrSql = vbCrLf + " UPDATE MASTER..SYSDATABASES SET STATUS = 32768 WHERE NAME = '" & CmbRcDatabase.Text & "'"
            StrSql = vbCrLf + " GO"
            StrSql = vbCrLf + " SP_CONFIGURE 'ALLOW UPDATES', 0"
            StrSql = vbCrLf + " GO"
            StrSql = vbCrLf + " RECONFIGURE WITH OVERRIDE"
            Cmd = New OleDbCommand(StrSql, Cn)
            Cmd.ExecuteNonQuery()
            StrSql = " IF EXISTS(SELECT * FROM MASTER..SYSDATABASES WHERE NAME='RECOVERDB') DROP DATABASE RECOVERDB"
            Cmd = New OleDbCommand(StrSql, Cn)
            Cmd.ExecuteNonQuery()
            StrSql = " CREATE DATABASE RECOVERDB"
            Cmd = New OleDbCommand(StrSql, Cn)
            Cmd.ExecuteNonQuery()
            StrSql = "SELECT 'SELECT * INTO RECOVERDB..'+NAME+' FROM " & CmbRcDatabase.Text & "..'+NAME "
            StrSql += vbCrLf + " AS QUERY FROM " & CmbRcDatabase.Text & "..SYSOBJECTS WHERE XTYPE='U'"
            Da = New OleDbDataAdapter(StrSql, Cn)
            Da.Fill(dt)
            For Each ro As DataRow In dt.Rows
                Cmd = New OleDbCommand(ro("QUERY").ToString, Cn)
                Cmd.ExecuteNonQuery()
            Next
        Catch ex As Exception
            MsgBox(ex.Message + ex.StackTrace, MsgBoxStyle.Information)
            Exit Sub
        End Try
        StrSql = " DROP DATABASE " & CmbRcDatabase.Text
        Cmd = New OleDbCommand(StrSql, Cn)
        Cmd.ExecuteNonQuery()
        LogPath = GetSqlValue("SELECT FILENAME FROM " & CmbRcDatabase.Text & "..SYSFILES WHERE FILEID =2")
        If IO.File.Exists(LogPath) Then IO.File.Delete(LogPath)
        MsgBox("Recover Completed", MsgBoxStyle.Information)
        MsgBox("Backup the Database Named 'RECOVERDB' and Restore with Name '" & CmbRcDatabase.Text & "'", MsgBoxStyle.Information)
        CmbRcDatabase.Text = ""
    End Sub
#End Region
    Private Sub SaveProperties()
        ''serialize
        Dim objStreamWriter As New StreamWriter(Application.StartupPath & "\" & "Properties.xml", True)
        Dim x As New XmlSerializer(clsProperties.GetType)
        x.Serialize(objStreamWriter, clsProperties)
        objStreamWriter.Close()
    End Sub
    Private Sub GetProperties()

    End Sub
    Private Sub btnAddBackUpFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddBackUpFiles.Click
        Dim objFileDia As New OpenFileDialog
        objFileDia.Filter = "Back Up Files (*.Bak)|*.BAK"
        objFileDia.Multiselect = True
        If objFileDia.ShowDialog Then
            Dim Row As DataRow = Nothing
            Dim fINfo As FileInfo
            Dim dbName As String = Nothing
            Dim sp() As String = Nothing
            Dim paramChar() As Char = "_."
            clsProperties.pRestoreDtFileNames.Rows.Clear()
            For Each fName As String In objFileDia.FileNames
                fINfo = New FileInfo(fName)
                Row = clsProperties.pRestoreDtFileNames.NewRow
                Row!FILENAME = fINfo.Name
                Row!FILEPATH = fINfo.FullName
                sp = fINfo.Name.Split(paramChar)
                Row!DBNAME = sp(0)
                clsProperties.pRestoreDtFileNames.Rows.Add(Row)
            Next
        End If
    End Sub
    Private Sub btnRestorePathBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestorePathBrowse.Click
        Dim openFolderDia As New FolderBrowserDialog
        If openFolderDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            If openFolderDia.SelectedPath.StartsWith("\\") Then
                MsgBox("Network path not support", MsgBoxStyle.Information)
                Exit Sub
            End If
            txtRestorePath.Text = openFolderDia.SelectedPath
            SendKeys.Send("{TAB}")
        End If
    End Sub
#Region " ALTER COLLATION "
    Private Sub _AlterCollationLoad()
        Dim i As Integer
        Dim dtcol As New DataTable
        Dim dt As New DataTable
        StrSql = "SELECT NAME FROM MASTER..SYSDATABASES ORDER BY NAME"
        Da = New OleDbDataAdapter(StrSql, Cn)
        Da.Fill(dtcol)
        For Each ro As DataRow In dtcol.Rows
            CmbDataBaseName.Items.Add(ro("NAME").ToString)
        Next
    End Sub
    Private Sub LoadCollation()
        Dim i As Integer
        Dim dtcol As New DataTable
        Dim dt As New DataTable
        StrSql = "select name  from ::fn_helpcollations()"
        Da = New OleDbDataAdapter(StrSql, Cn)
        dt = New DataTable
        Da.Fill(dt)
        For i = 0 To dt.Rows.Count - 1
            CmbCollationName.Items.Add(dt.Rows(i).Item("name"))
        Next
    End Sub
    Private Sub CmbDataBaseName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbDataBaseName.SelectedIndexChanged
        Try
            Cmbcurcollation.Items.Clear()
            Dim dtcn As New DataTable
            StrSql = "SELECT COLLATION_NAME FROM " & CmbDataBaseName.Text & ".INFORMATION_SCHEMA.COLUMNS WHERE COLLATION_NAME NOT IN('NULL') GROUP BY COLLATION_NAME "
            Da = New OleDbDataAdapter(StrSql, Cn)
            Da.Fill(dtcn)
            For Each ro As DataRow In dtcn.Rows
                If (dtcn.Rows.Count > 0) Then
                    Cmbcurcollation.Items.Add(ro.Item(0))
                End If
            Next
            Cmbcurcollation.SelectedIndex = 0
            LoadCollation()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub BtnUpdateCollaton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnUpdateCollaton.Click
        If (CmbDataBaseName.Text = "") Then
            MessageBox.Show("Must Select Database Name")
            CmbDataBaseName.Focus()
            Exit Sub
        End If
        If (CmbCollationName.Text = "") Then
            MessageBox.Show("Must Select Collation Name")
            Exit Sub
            CmbCollationName.Focus()
        End If
        CONN()
        DataBaseSetSingleUser()
        DropIndex()
        DropCheckConstraints()
        AlterDataBaseCollat()
        Try
            _Tran = _Con.BeginTransaction
            TempTableCreation()
            LoadConstraints()
            LoadValuesPrimary()
            SelectDefaultConst()
            DropDefaultConst()
            DropConstraints()
            UpdateCollationName()
            AlterAddPrimaryNotNull()
            AddConstraints()
            AdddefaultConst()
            DropTable()
            _Tran.Commit()
            _Tran = Nothing
        Catch ex As Exception
            If _Tran IsNot Nothing Then _Tran.Rollback()
            _Tran = Nothing
            MessageBox.Show(ex.Message)
        Finally
            DataBaseSetMultiUser()
            MessageBox.Show("Update SuccessFully..")
            funClear()
        End Try
    End Sub
    Private Sub FunChk()
        If (CmbDataBaseName.Text = "") Then
            MessageBox.Show("Must Select Database Name")
            CmbDataBaseName.Focus()
            Exit Sub
        End If
        If (CmbCollationName.Text = "") Then
            MessageBox.Show("Must Select Collation Name")
            Exit Sub
            CmbCollationName.Focus()
        End If
    End Sub
    Private Sub DataBaseSetSingleUser()
        StrSql = "ALTER DATABASE  " & CmbDataBaseName.Text & " SET SINGLE_USER"
        Cmd = New OleDbCommand(StrSql, _Con)
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub AlterDataBaseCollat()
        StrSql = "ALTER DATABASE " & CmbDataBaseName.Text & " COLLATE " & CmbCollationName.Text & ""
        Cmd = New OleDbCommand(StrSql, _Con)
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub DropIndex()
        Dim dt As New DataTable
        Dim i As Integer
        StrSql = vbCrLf + " SELECT 'DROP INDEX ' + T.name + '.'+I.name"
        StrSql += vbCrLf + " ,I.name AS INDNAME,T.name AS TBLNAME"
        StrSql += vbCrLf + " FROM sysindexes AS I"
        StrSql += vbCrLf + " INNER JOIN sysobjects AS T ON T.id = I.id AND T.xtype = 'U'"
        StrSql += vbCrLf + " WHERE I.NAME LIKE 'IND_%' "
        Cmd = New OleDbCommand(StrSql, _Con)
        Da = New OleDbDataAdapter(Cmd)
        dt = New DataTable
        Da.Fill(dt)
        For i = 0 To dt.Rows.Count - 1
            StrSql = dt.Rows(i).Item(0)
            Cmd = New OleDbCommand(StrSql, _Con)
            Cmd.ExecuteNonQuery()
        Next
    End Sub
    Private Sub DropCheckConstraints()
        Dim dt As New DataTable
        StrSql = vbCrLf + "SELECT DISTINCT(C.TABLE_NAME),C.CONSTRAINT_NAME "
        StrSql += vbCrLf + "FROM  " & CmbDataBaseName.Text & ".INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C"
        StrSql += vbCrLf + "," & CmbDataBaseName.Text & ".INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS T  "
        StrSql += vbCrLf + "WHERE C.TABLE_NAME=T.TABLE_NAME AND C.CONSTRAINT_NAME=T.CONSTRAINT_NAME   "
        StrSql += vbCrLf + "AND CONSTRAINT_TYPE='CHECK'"
        StrSql += vbCrLf + "ORDER BY C.TABLE_NAME"
        Da = New OleDbDataAdapter(StrSql, _Con)
        dt = New DataTable
        Da.Fill(dt)
        For i As Integer = 0 To dt.Rows.Count - 1
            TableName = dt.Rows(i).Item(0).ToString()
            Constraint_name = dt.Rows(i).Item(1).ToString()
            StrSql = " ALTER TABLE " & TableName & " DROP constraint " & Constraint_name & ""
            Cmd = New OleDbCommand(StrSql, _Con)
            Cmd.ExecuteNonQuery()
        Next
    End Sub
    Private Sub TempTableCreation()
        StrSql = " IF OBJECT_ID('TEMPUPLOAD') IS NOT NULL DROP TABLE TEMPUPLOAD"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        cmd.ExecuteNonQuery()
        StrSql = " CREATE TABLE tempupload(TABLE_NAME VARCHAR(50),CONSTRAINT_TYPE VARCHAR(70),column_name VARCHAR(50),CONSTRAINT_NAME VARCHAR (70))"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        cmd.ExecuteNonQuery()
        StrSql = " IF OBJECT_ID('tempPrimaryval') IS NOT NULL DROP TABLE tempPrimaryval"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        cmd.ExecuteNonQuery()
        StrSql = "create table tempPrimaryval (TABLE_NAME VARCHAR(50),CONSTRAINT_TYPE VARCHAR(50),COLUMN_NAME VARCHAR (50),CONSTRAINT_NAME VARCHAR(50),DATA_TYPE VARCHAR(20),CHARACTER_LENGTH VARCHAR(20))"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        cmd.ExecuteNonQuery()
        StrSql = " IF OBJECT_ID('TEMPDEFAULTCONST') IS NOT NULL DROP TABLE TEMPDEFAULTCONST"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        cmd.ExecuteNonQuery()
        StrSql = "CREATE TABLE TEMPDEFAULTCONST(TABLE_NAME VARCHAR(50),CONSTRAINT_NAME VARCHAR(70),column_name VARCHAR(50),DEFAULT_CLAUSE VARCHAR (70))"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub LoadConstraints()
        StrSql = "insert into tempupload "
        StrSql += " SELECT DISTINCT(c.TABLE_NAME),c.CONSTRAINT_TYPE,t.column_name,c.CONSTRAINT_NAME FROM "
        StrSql += " " & CmbDataBaseName.Text & ".INFORMATION_SCHEMA.TABLE_CONSTRAINTS as c, "
        StrSql += " " & CmbDataBaseName.Text & ".INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as t "
        StrSql += " where c.table_name=t.table_name and c.CONSTRAINT_name=t.CONSTRAINT_name  "
        StrSql += " order by c.table_name"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub LoadValuesPrimary()
        StrSql = vbCrLf + "INSERT INTO tempPrimaryval "
        StrSql += vbCrLf + " SELECT c.TABLE_NAME,c.CONSTRAINT_TYPE,t.column_name,c.CONSTRAINT_NAME,D.DATA_TYPE,D.CHARACTER_MAXIMUM_LENGTH FROM "
        StrSql += vbCrLf + "  INFORMATION_SCHEMA.TABLE_CONSTRAINTS as c, "
        StrSql += vbCrLf + "  INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as t ,"
        StrSql += vbCrLf + " INFORMATION_SCHEMA.COLUMNS AS D"
        StrSql += vbCrLf + "  where c.table_name=t.table_name AND t.table_name=D.TABLE_NAME"
        StrSql += vbCrLf + "  and c.CONSTRAINT_name=t.CONSTRAINT_name"
        StrSql += vbCrLf + "  AND t.COLUMN_NAME=D.COLUMN_NAME "
        StrSql += vbCrLf + "  AND c.CONSTRAINT_TYPE='PRIMARY KEY' AND (D.DATA_TYPE='varchar' OR D.DATA_TYPE='char') "
        StrSql += vbCrLf + "   Order by c.table_name"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub SelectDefaultConst()
        StrSql = " INSERT INTO TEMPDEFAULTCONST"
        StrSql += " select	"
        StrSql += " 	t_obj.name 				as TABLE_NAME"
        StrSql += " 	,c_obj.name				as CONSTRAINT_NAME"
        StrSql += " 	,col.name				as COLUMN_NAME"
        StrSql += " 	,com.text				as DEFAULT_CLAUSE "
        StrSql += " from	sysobjects	c_obj"
        StrSql += " join 	syscomments	com on 	c_obj.id = com.id"
        StrSql += " join 	sysobjects	t_obj on c_obj.parent_obj = t_obj.id  "
        StrSql += " join    sysconstraints con on c_obj.id	= con.constid"
        StrSql += " join 	syscolumns	col on t_obj.id = col.id"
        StrSql += " 			and con.colid = col.colid"
        StrSql += " where"
        StrSql += " 	c_obj.uid	= user_id()"
        StrSql += " 	and c_obj.xtype	= 'D'"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub DropDefaultConst()
        Dim dt As New DataTable
        Dim i As Integer
        StrSql = "SELECT * FROM  TEMPDEFAULTCONST"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        Da = New OleDbDataAdapter(Cmd)
        dt = New DataTable
        Da.Fill(dt)
        For i = 0 To dt.Rows.Count - 1
            Constraint_name = dt.Rows(i).Item(1).ToString()
            TableName = dt.Rows(i).Item(0).ToString()
            StrSql = " ALTER TABLE " & TableName & " DROP constraint " & Constraint_name & ""
            Cmd = New OleDbCommand(StrSql, _Con, _Tran)
            Cmd.ExecuteNonQuery()
        Next
    End Sub
    Private Sub DropConstraints()
        Dim dt As New DataTable
        Dim i As Integer
        StrSql = "SELECT CONSTRAINT_NAME,TABLE_NAME from  tempupload "
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        Da = New OleDbDataAdapter(Cmd)
        dt = New DataTable
        Da.Fill(dt)
        For i = 0 To dt.Rows.Count - 1
            Constraint_name = dt.Rows(i).Item(0).ToString()
            TableName = dt.Rows(i).Item(1).ToString()
            StrSql = "IF (select COUNT(*) from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where constraint_name='" & Constraint_name & "') >0"
            StrSql += " BEGIN "
            StrSql += " ALTER TABLE " & TableName & " DROP constraint " & Constraint_name & ""
            StrSql += " END "
            Cmd = New OleDbCommand(StrSql, _Con, _Tran)
            Cmd.ExecuteNonQuery()
        Next
    End Sub
    Private Sub UpdateCollationName()
        Dim dt As DataTable
        Dim i As Integer
        Dim Type, COLLATIONNAME, LENGTH As String
        StrSql = "SELECT * FROM " & CmbDataBaseName.Text & ".INFORMATION_SCHEMA.COLUMNS where  collation_name  not in ('NULL')  "
        StrSql += "  and table_NAME in (select name from sysobjects where xtype='U')"
        StrSql += " ORDER BY TABLE_NAME"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        Da = New OleDbDataAdapter(Cmd)
        dt = New DataTable
        Da.Fill(dt)
        For i = 0 To dt.Rows.Count - 1
            TableName = dt.Rows(i).Item(2).ToString()
            ColumnName = dt.Rows(i).Item(3).ToString()
            Type = dt.Rows(i).Item(7).ToString()
            COLLATIONNAME = dt.Rows(i).Item(19).ToString()
            LENGTH = Convert.ToUInt16(dt.Rows(i).Item(8))
            If (IsDBNull(Type) = False) Then
                StrSql = "ALTER TABLE " & CmbDataBaseName.Text & ".." & TableName & " alter column  " & ColumnName & "  "
                StrSql += "  " & Type & " ( " & LENGTH & " ) COLLATE  " & CmbCollationName.Text & "  "
                Cmd = New OleDbCommand(StrSql, _Con, _Tran)
                Cmd.ExecuteNonQuery()
            End If
        Next
    End Sub
    Private Sub AlterAddPrimaryNotNull()
        Dim dt As New DataTable
        Dim i As Integer
        Dim Type, LENGTH, ConstrainType, Column_Name As String
        StrSql = "SELECT * FROM  tempPrimaryval"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        Da = New OleDbDataAdapter(Cmd)
        dt = New DataTable
        Da.Fill(dt)
        For i = 0 To dt.Rows.Count - 1
            TableName = dt.Rows(i).Item(0).ToString()
            ConstrainType = dt.Rows(i).Item(1).ToString()
            Type = dt.Rows(i).Item(4).ToString()
            Column_Name = dt.Rows(i).Item(2).ToString()
            LENGTH = dt.Rows(i).Item(5).ToString()
            StrSql = "ALTER TABLE  " & TableName & " ALTER COLUMN  " & Column_Name & "  " & Type & "(" & LENGTH & ") NOT NULL"
            Cmd = New OleDbCommand(StrSql, _Con, _Tran)
            Cmd.ExecuteNonQuery()
        Next
    End Sub
    Private Sub AddConstraints()
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dtc As New DataTable
        Dim i, k, j As Integer
        Dim ConstrainType, Column_Name As String
        Dim colname As String = Nothing
        StrSql = "select  TABLE_NAME from tempupload GROUP BY TABLE_NAME "
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        da = New OleDbDataAdapter(cmd)
        dt = New DataTable
        da.Fill(dt)
        For i = 0 To dt.Rows.Count - 1
            TableName = dt.Rows(i).Item("TABLE_NAME").ToString()
            StrSql = "select CONSTRAINT_TYPE from tempupload where TABLE_NAME='" & TableName & "' GROUP BY CONSTRAINT_TYPE"
            Cmd = New OleDbCommand(StrSql, _Con, _Tran)
            da = New OleDbDataAdapter(cmd)
            dtc.Clear()
            da.Fill(dtc)
            For k = 0 To dtc.Rows.Count - 1
                If dtc.Rows(k).Item("CONSTRAINT_TYPE").ToString = "PRIMARY KEY" Then
                    StrSql = "select column_name,CONSTRAINT_TYPE from tempupload where TABLE_NAME='" & TableName & "' AND CONSTRAINT_TYPE='" & dtc.Rows(k).Item("CONSTRAINT_TYPE").ToString & "'"
                    Cmd = New OleDbCommand(StrSql, _Con, _Tran)
                    da = New OleDbDataAdapter(cmd)
                    dt1 = New DataTable
                    da.Fill(dt1)
                    ConstrainType = dt1.Rows(0).Item("CONSTRAINT_TYPE").ToString()
                    colname = Nothing
                    For j = 0 To dt1.Rows.Count - 1
                        colname += dt1.Rows(j).Item("column_name").ToString() + ","
                    Next
                    colname = Mid(colname, 1, Len(colname) - 1)
                    StrSql = "ALTER TABLE " & TableName & " ADD " & ConstrainType & " (" & colname & ")"
                    Cmd = New OleDbCommand(StrSql, _Con, _Tran)
                    cmd.ExecuteNonQuery()
                Else
                    StrSql = "select column_name,CONSTRAINT_TYPE from tempupload where TABLE_NAME='" & TableName & "' AND CONSTRAINT_TYPE='" & dtc.Rows(k).Item("CONSTRAINT_TYPE").ToString & "'"
                    Cmd = New OleDbCommand(StrSql, _Con, _Tran)
                    da = New OleDbDataAdapter(cmd)
                    dt1 = New DataTable
                    da.Fill(dt1)
                    ConstrainType = dt1.Rows(0).Item("CONSTRAINT_TYPE").ToString()
                    Column_Name = dt1.Rows(0).Item("column_name").ToString()
                    StrSql = "ALTER TABLE  " & TableName & " ADD " & ConstrainType & " (" & Column_Name & ")"
                    Cmd = New OleDbCommand(StrSql, _Con, _Tran)
                    cmd.ExecuteNonQuery()
                End If
            Next
        Next
    End Sub
    Private Sub AdddefaultConst()
        Dim i, k, j As Integer
        Dim dt As New DataTable
        Dim ConstrainType, Type, Column_Name As String
        StrSql = "SELECT * FROM  TEMPDEFAULTCONST"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        Da = New OleDbDataAdapter(Cmd)
        dt = New DataTable
        Da.Fill(dt)
        For i = 0 To dt.Rows.Count - 1
            TableName = dt.Rows(i).Item(0).ToString()
            ConstrainType = dt.Rows(i).Item(1).ToString()
            Type = dt.Rows(i).Item(3).ToString()
            Column_Name = dt.Rows(i).Item(2).ToString()
            StrSql = "ALTER TABLE " & TableName & " ADD CONSTRAINT " & ConstrainType & " DEFAULT  " & Type & " FOR " & Column_Name & ""
            Cmd = New OleDbCommand(StrSql, _Con, _Tran)
            Cmd.ExecuteNonQuery()
        Next
    End Sub
    Private Sub DropTable()
        StrSql = " DROP TABLE  tempupload"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        cmd.ExecuteNonQuery()
        StrSql = " DROP TABLE  tempPrimaryval"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        cmd.ExecuteNonQuery()
        StrSql = " DROP TABLE  TEMPDEFAULTCONST"
        Cmd = New OleDbCommand(StrSql, _Con, _Tran)
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub DataBaseSetMultiUser()
        StrSql = "ALTER DATABASE  " & CmbDataBaseName.Text & " SET MULTI_USER"
        Cmd = New OleDbCommand(StrSql, _Con)
        cmd.ExecuteNonQuery()
    End Sub
    Public Sub CONN()
        _Admindb = CmbDataBaseName.Text
        _DataSource = cmbServers.Text
        '_Con = New OleDbConnection("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & _Admindb & "; Data Source=" & _DataSource & ";User Id=sa;password=;")
        If Mid(cmbLoginType.Text, 1, 1) = "S" Then
            _Con = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & _Admindb & ";Data Source={0};uid=" & txtLoginName.Text & ";pwd=" & txtPassword.Text & ";", _DataSource))
        Else
            _Con = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & _Admindb & ";Data Source=" & _DataSource & "")
        End If
        _Con.Open()
    End Sub
    Private Sub funClear()
        CmbDataBaseName.Text = ""
        CmbRDataBase.Text = ""
        CmbCollationName.Text = ""
        Cmbcurcollation.Items.Clear()
    End Sub
#End Region

    Private Sub ChkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkAll.CheckedChanged
        Dim dr() As DataRow
        If GridView.DataSource Is Nothing Then Exit Sub
        dr = CType(GridView.DataSource, DataTable).Select
        For Each ro As DataRow In dr
            ro.Item("CHECK") = ChkAll.Checked
        Next
    End Sub

    Private Sub btnAttachBrowseSource_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'btnAttachBrowseSource_Click(Me, New EventArgs)
    End Sub

    Private Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        If Not dgvAttach.RowCount > 0 Then Exit Sub
        chkSelectAll.Enabled = False
        For Each dgvRow As DataGridViewRow In dgvAttach.Rows
            dgvRow.Cells("CHK").Value = chkSelectAll.Checked
        Next
        chkSelectAll.Enabled = True
    End Sub

    Private Sub tabpgAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabpgAll.Click
        _AttachGetFileLocationsAll()
    End Sub

    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub tabctrlAttachDetach_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabctrlAttachDetach.SelectedIndexChanged
        If tabctrlAttachDetach.SelectedIndex = 1 Then
            _AttachGetFileLocationsAll()
        End If
    End Sub

    Private Sub chkdSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkdSelectAll.CheckedChanged
        If Not dgvDetach.RowCount > 0 Then Exit Sub
        chkdSelectAll.Enabled = False
        For Each dgvRow As DataGridViewRow In dgvDetach.Rows
            dgvRow.Cells("CHK").Value = chkdSelectAll.Checked
        Next
        chkdSelectAll.Enabled = True
    End Sub

    Private Sub tabctrlDetach_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabctrlDetach.SelectedIndexChanged
        If tabctrlDetach.SelectedIndex = 1 Then
            _DetachGetFileLocationsAll()
        End If
    End Sub

    Private Sub tabShrink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabShrink.Click

    End Sub
End Class
