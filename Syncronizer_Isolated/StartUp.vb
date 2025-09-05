Imports System.IO
Imports System.Data.OleDb
Imports Microsoft.Office.Interop
Imports System.Reflection
Imports System.Threading
Public Class StartUp
    Private WithEvents TxtStatus As New TextBox
    Private _Coninfo As Coninfo
    Private _FromId As String
    Private _Cn As OleDb.OleDbConnection = Nothing
    Private _CnWeb As OleDb.OleDbConnection = Nothing
    Private _CnWebLocal As OleDb.OleDbConnection = Nothing
    Private _Chitcn As OleDb.OleDbConnection = Nothing
    Private StrSql As String
    Private _Da As OleDbDataAdapter
    Private _Cmd As OleDbCommand
    Private _CmdE As OleDbCommand
    Private _Tran As OleDbTransaction
    Private _TranWeb As OleDbTransaction
    Private _CnAdmin As OleDbConnection = Nothing
    Private _ReplaceWords As New List(Of String)
    Private _Admindb As String
    Private _Syncdb As String
    Private _Chitdb As String
    Private _Status_msg As String
    Private _VbDbPrefix As String = ""
    Private _SyncMode As String = ""
    Private _SyncSch As Integer = 30
    Private _SyncErrSendId As New List(Of String)

    Private _WebDbName As String = ""
    Private _WebDbUserName As String = ""
    Private _WebDbPass As String = ""
    Private _WebDbServerName As String = ""
    Private _WebDbTblPrefix As String = ""
    Private _LocalTblPrefix As String = ""
    Private _DetStatus As Boolean = False
    Private _ObjStatus As Status
    Private t As Threading.Thread = Nothing
    Private t1 As Threading.Thread = Nothing
    Private t2 As Threading.Thread = Nothing
    Private t3 As Threading.Thread = Nothing
    Private _AppMode As String = Nothing
    Dim minUid As Long = 0
    Dim maxUid As Long = 0
    Dim PassUids As String = ""
    Dim FailUids As String = ""
    Dim StartTime As Date
    Dim NextTime As Date
    Dim tspan As TimeSpan
    Dim ListTransferTables As New List(Of String)
    Dim SYNCCHK_TRANDB As Boolean = True
    Dim IS_IMAGE_TRF As Boolean = False
    Private _CompanyId As String
    Private _CompanyIds As String = ""
    Dim _Sync_Via_R As Boolean = True
    Dim _Sync_Rec_SyncMast As Boolean = False
    Dim dtReceiveTbl As New DataTable
    Private _SyncDelay As Integer = 30



    Public Sub New()
        If Isrunning() Then End
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Refreshfile()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    
    Function Isrunning() As Boolean
        Dim process As System.Diagnostics.Process()
        process = System.Diagnostics.Process.GetProcessesByName("Syncronizer")
        If process.Length > 1 Then
            ' MsgBox("Synchronizer is running")
            Return True
        End If
        Return False
    End Function

    Private Sub Refreshfile()
        Dim TempFilePath As String = IO.Path.GetTempPath
        Dim xmlFiles = Directory.GetFiles(TempFilePath, "*.xml", SearchOption.TopDirectoryOnly)
        Dim FileDate As Date
        For Each Filename As String In xmlFiles
            FileDate = IO.File.GetLastWriteTime(Filename).Date
            If FileDate < Today Then IO.File.Delete(Filename)
        Next
        TempFilePath = IO.Path.GetTempPath & "back\"
        If Not IO.Directory.Exists(TempFilePath) Then IO.Directory.CreateDirectory(TempFilePath)
        xmlFiles = Directory.GetFiles(TempFilePath, "*.xml", SearchOption.TopDirectoryOnly)
        For Each Filename As String In xmlFiles
            FileDate = IO.File.GetLastWriteTime(Filename).Date
            If FileDate < Today Then IO.File.Delete(Filename)
        Next
    End Sub
    Private Sub SetStatus(Optional ByVal status As String = "")
        Dim dispText As String = status
        If status <> "" Then dispText = IIf(dispText.Length <= 20, LSet(dispText, 20), dispText)
        SetControlPropertyValue(TxtStatus, "Text", dispText)
        If TxtStatus.Text = "" Then Exit Sub
        NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
        NotifyIcon1.BalloonTipText = TxtStatus.Text
        NotifyIcon1.ShowBalloonTip(1000)
    End Sub

    Private Sub Animator()
        'Try
        While 1 = 1
            SetStatus()

            System.Threading.Thread.Sleep(200)
            NotifyIcon1.Icon = My.Resources._1im
            System.Threading.Thread.Sleep(200)
            NotifyIcon1.Icon = My.Resources._2im
            System.Threading.Thread.Sleep(200)
            NotifyIcon1.Icon = My.Resources._3im
            System.Threading.Thread.Sleep(200)
            NotifyIcon1.Icon = My.Resources._4im
            System.Threading.Thread.Sleep(200)
            NotifyIcon1.Icon = My.Resources._5im
            System.Threading.Thread.Sleep(200)
            NotifyIcon1.Icon = My.Resources._6im
            System.Threading.Thread.Sleep(200)
            NotifyIcon1.Icon = My.Resources._7im
            System.Threading.Thread.Sleep(200)
            NotifyIcon1.Icon = My.Resources._8im
        End While
        'Catch ex As Exception
        'End Try
    End Sub

    Private Sub Animatoridle()

        While 1 = 1
            SetStatus()
            System.Threading.Thread.Sleep(50)
            NotifyIcon1.Icon = My.Resources._30_Sync
        End While
        
    End Sub

    Private Sub StartUp_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        KillApp()
    End Sub
    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If t IsNot Nothing Then t.Abort()
        NotifyIcon1.Icon = Nothing
        NotifyIcon1.Visible = False
        KillApp()

    End Sub

    Private Sub initiator()
        'Try
        While 1 = 1
            tspan = DateTime.Now.Subtract(StartTime)
        End While
        'Catch ex As Exception
        'End Try
    End Sub

    Private Sub StartUp_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Control Then
            If e.KeyCode = Keys.D Then
                _DetStatus = True
            End If
        End If

    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
        _Status_msg = "Synchronizer : Ver." & FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion
        funcListTagTables()
        Schedule_sync()

    End Sub
    Private Sub Schedule_sync(Optional ByVal sendreconly As String = Nothing)
        Try
            _ObjStatus = New Status
            _ObjStatus.AddStatus("Getting Confinfo")
            If Not File.Exists(Application.StartupPath & "\Coninfo.ini") Then
                _ObjStatus.AddStatus("Registration info not found")
                _ObjStatus.ShowDialog()
                KillApp()
                Exit Sub
            End If

            Dim ElapsedTime As TimeSpan
            If My.Settings.Er_Created = Nothing Then
                My.Settings.Er_Created = Date.Now
                My.Settings.Er_Text = Nothing
                My.Settings.Er_OldText = Nothing
                My.Settings.Save()
            End If

            ElapsedTime = Now.Subtract(My.Settings.Er_Created)
            If Math.Round(ElapsedTime.TotalDays, 0) <> 0 Then
                My.Settings.Er_Created = Date.Now
                My.Settings.Er_Text = Nothing
                My.Settings.Er_OldText = Nothing
                My.Settings.Save()
                ElapsedTime = Now.Subtract(My.Settings.Er_Created)
            End If
            'MsgBox(My.Settings.Er_Created & vbCrLf & " Diff Days : " & Math.Round(ElapsedTime.TotalDays).ToString + vbCrLf _
            '& " Diff Min  : " & Math.Round(ElapsedTime.TotalMinutes).ToString)

            StartTime = Date.Now
            NextTime = StartTime
            '            If NextTime.ToLongTimeString <> Date.Now.ToLongTimeString Then GoTo Nextschedule
            If t IsNot Nothing Then t.Abort()
            If t1 IsNot Nothing Then t1.Abort()
            If t2 IsNot Nothing Then t2.Abort()
            t = New Threading.Thread(AddressOf Animator)
            t.IsBackground = True
            t.Priority = Threading.ThreadPriority.Lowest
            t.Start()

            _Coninfo = New Coninfo(Application.StartupPath & "\Coninfo.ini")

            _Cn = New OleDbConnection(_Coninfo.lConnectionString)
            _Cn.Open()
            Dim Opt_Sync() As String
            Dim Sync_Comp As String = ""
            Dim passWord As String = _Coninfo.lDbPwd
            'If passWord <> "" Then passWord = GiritechPack.Methods.Decrypt(passWord)
            If passWord <> "" Then passWord = Decrypt(passWord)
            _CompanyId = _Coninfo.lCompanyId
NextIteration:
            _Syncdb = ""
            If _Cn.State = ConnectionState.Closed Then _Cn.Open()
            StrSql = " SELECT NAME FROM SYSDATABASES WHERE NAME = '" & _CompanyId & "ADMINDB'"
            _Admindb = GMethods.GetSqlValue(_Cn, StrSql)
            If _Admindb <> "" Then
                If UCase(_Coninfo.lDbLoginType = "W") Then
                    _CnAdmin = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & _CompanyId & "ADMINDB;Data Source=" & _Coninfo.lServerName & "")
                Else
                    _CnAdmin = New OleDbConnection("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & _CompanyId & "ADMINDB;user id=" & IIf(_Coninfo.lDbUserId <> "", _Coninfo.lDbUserId, "sa") & ";password=" & passWord & ";Data Source=" & _Coninfo.lServerName & ";")
                End If
            End If
            StrSql = " SELECT NAME FROM SYSDATABASES WHERE NAME = '" & _CompanyId & "SH0708'"
            Dim _Chitdb As String = GMethods.GetSqlValue(_Cn, StrSql)
            If _Chitdb <> "" Then
                If UCase(_Coninfo.lDbLoginType = "W") Then
                    _Chitcn = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & _Chitdb & ";Data Source=" & _Coninfo.lServerName & "")
                Else
                    _Chitcn = New OleDbConnection("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & _Chitdb & ";Data Source=" & _Coninfo.lServerName & ";user id=" & IIf(_Coninfo.lDbUserId <> "", _Coninfo.lDbUserId, "sa") & ";password=" & passWord & ";")
                End If
            End If


            If _Admindb = "" Then
                StrSql = " SELECT NAME FROM SYSDATABASES WHERE NAME = '" & _CompanyId & "SAVINGS'"
                _Admindb = GMethods.GetSqlValue(_Cn, StrSql)
                If _Admindb <> "" Then
                    If UCase(_Coninfo.lDbLoginType = "W") Then
                        _CnAdmin = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & _Admindb & ";Data Source=" & _Coninfo.lServerName & "")
                    Else
                        _CnAdmin = New OleDbConnection("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & _Admindb & ";Data Source=" & _Coninfo.lServerName & ";user id=" & IIf(_Coninfo.lDbUserId <> "", _Coninfo.lDbUserId, "sa") & ";password=" & passWord & ";")
                    End If
                End If
            End If
            If _Admindb = "" Then
                _ObjStatus.AddStatus("Admindb could not found")
                _ObjStatus.ShowDialog()
                KillApp()
                Exit Sub
            End If
            If GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-SEPDATADB'") = "Y" Then
                StrSql = " SELECT NAME FROM SYSDATABASES WHERE NAME = '" & _CompanyId & "UTILDB'"
                _Syncdb = GMethods.GetSqlValue(_Cn, StrSql)
            End If
            If _Syncdb = "" Then
                _Syncdb = _Admindb
            Else
                If UCase(_Coninfo.lDbLoginType = "W") Then
                    _CnAdmin = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & _Syncdb & ";Data Source=" & _Coninfo.lServerName & "")
                Else
                    _CnAdmin = New OleDbConnection("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & _Syncdb & ";Data Source=" & _Coninfo.lServerName & ";user id=" & IIf(_Coninfo.lDbUserId <> "", _Coninfo.lDbUserId, "sa") & ";password=" & passWord & ";")
                End If
            End If
            _Cn1 = _CnAdmin
            _CnAdmin.Open()
            Dim SyncErrId As String = GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-ErrRptTo'")
            For Each s As String In SyncErrId.Split(",")
                _SyncErrSendId.Add(s)
            Next
            If Not _SyncErrSendId.Count > 0 Then
                _SyncErrSendId.Add("giritechnologies@gmail.com")
            End If
            IS_IMAGE_TRF = IIf(GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'STKTRANWITHIMAGE'", , "N") = "Y", True, False)
            _SyncMode = GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-MODE'")
            _SyncSch = GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-HOUR'")
            _SyncDelay = GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-DELAY'", , 0)
            _FromId = GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'COSTID'")
            _VbDbPrefix = GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'CHITDBPREFIX'")
            _WebDbName = GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-WEBDB_NAME'")
            _WebDbUserName = GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-WEBDB_USERNAME'")
            _WebDbServerName = GMethods.GetSqlValue(_Cn, "SELECT FTPID FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE FTPID <> '' AND ISNULL(WEBDBNAME,'')=''")
            _WebDbPass = GMethods.GetSqlValue(_Cn, "SELECT PASSWORD FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE FTPID <> '' AND ISNULL(WEBDBNAME,'')=''")
            _WebDbPass = GMethods.Decrypt(_WebDbPass)
            _WebDbTblPrefix = GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-WEBDB_TBLPREFIX'")
            _Sync_Via_R = IIf(GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-VIA'", , "R") = "R", True, False)
            _Sync_Rec_SyncMast = IIf(GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SYNC_REC_SYNCMAST'", , "N") = "Y", True, False)
            If _CompanyIds = "" Then
                Sync_Comp = GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-OPTIONAL'")
                If Sync_Comp <> "" Then Opt_Sync = Sync_Comp.Split(",")
            End If
            If _Sync_Rec_SyncMast Then
                StrSql = "SELECT TABLENAME FROM " & _Admindb & "..SYNCMAST WHERE SYNC='N'"
                dtReceiveTbl = New DataTable
                _Da = New OleDbDataAdapter(StrSql, _CnAdmin)
                _Da.Fill(dtReceiveTbl)
            End If
            If _FromId = "" Then
                _ObjStatus.AddStatus("From costid not set properly")
                _ObjStatus.ShowDialog()
                KillApp()
                Exit Sub
            End If
            Dim _dbmaster As New DataTable
            StrSql = "select dbname from " & _Admindb & "..dbmaster order by dbname "
            _Da = New OleDbDataAdapter(StrSql, _Cn)
            _Da.Fill(_dbmaster)
            _ReplaceWords.Clear()
            _ReplaceWords.Add("ADMINDB..")
            For Each Ro As DataRow In _dbmaster.Rows
                _ReplaceWords.Add(Mid(Ro.Item("dbname").ToString, 4) & "..")
            Next
            ' _ReplaceWords.Add("T0708..")
            '_ReplaceWords.Add("T0809..")
            '_ReplaceWords.Add("T0910..")
            '_ReplaceWords.Add("T1011..")
            '_ReplaceWords.Add("T1112..")
            '_ReplaceWords.Add("T1213..")
            '_ReplaceWords.Add("T1314..")

            _ReplaceWords.Add("SAVINGS..")
            _ReplaceWords.Add("SH0708..")
            _ReplaceWords.Add("UTILDB..")
            Dim CmdParamSend As String = ""
            For Each str As String In My.Application.CommandLineArgs
                _AppMode = str.ToUpper
                CmdParamSend = str
                Exit For
            Next
            SyncPost()
            't = New Threading.Thread(AddressOf Animator)
            't.Start()
            NotifyIcon1.BalloonTipTitle = "Data Synchronizer @ " & _CompanyId & "_" & _FromId.ToUpper
            Select Case _AppMode
                Case "SEND", "S"
                    NotifyIcon1.Text = _Status_msg & vbCrLf & "Data Sending.."
                    NotifyIcon1.BalloonTipText = _Status_msg & vbCrLf & "Data Sending.."
                Case "RECEIVE", "R"
                    NotifyIcon1.Text = _Status_msg & vbCrLf & "Data Receiving.."
                    NotifyIcon1.BalloonTipText = _Status_msg & vbCrLf & "Data Receiving.."
                Case "CHECK", "C"
                    _CnWeb = New OleDbConnection(GetWebConnectionString)
                    _CnWeb.Open()
                    Dim ObjDataTracker As New WebDataTracker(_Cn, _CnWeb, _WebDbTblPrefix, _FromId, _Admindb)
                    ObjDataTracker.ShowDialog()
                    Environment.Exit(0)
                Case Else
                    NotifyIcon1.Text = _Status_msg & vbCrLf & "Synchronizing.. "
                    NotifyIcon1.BalloonTipText = _Status_msg & vbCrLf & "Synchronizing.."
            End Select

            Select Case _SyncMode
                Case "W"
                Case "M"
                Case Else
                    SetStatus("Invalid Sync Mode")
                    GoTo SendAndReceiveComplete
            End Select
            StartSync(sendreconly)
            If Not Opt_Sync Is Nothing Then
                For i As Integer = 0 To Opt_Sync.Length - 1
                    If _CompanyIds.Contains(Opt_Sync(i).ToString) Then Continue For
                    If Opt_Sync(i).ToString.Length = 3 Then
                        _CompanyId = Opt_Sync(i).ToString
                        _CompanyIds += _CompanyId & ","
                        GoTo NextIteration
                    End If
                Next
            End If
SendAndReceiveComplete:
            _CompanyIds = ""
            StartTime = Date.Now
            NextTime = StartTime
            NextTime = NextTime.AddMinutes(_SyncSch)
            NotifyIcon1.Text = _Status_msg & " @ " & _FromId.ToUpper & vbCrLf & "Schedule Time " & NextTime.ToLongTimeString
            NotifyIcon1.BalloonTipText = _Status_msg & vbCrLf & " @ " & _FromId.ToUpper & vbCrLf & "Schedule Time " & NextTime.ToLongTimeString
            SetStatus()
            sendreconly = ""
            If t IsNot Nothing Then t.Abort()
            If t1 IsNot Nothing Then t1.Abort()
            If t2 IsNot Nothing Then t2.Abort()
            't2 = New Threading.Thread(AddressOf Animatoridle)
            't2.IsBackground = True
            't2.Priority = Threading.ThreadPriority.Lowest
            't2.Start()
            NotifyIcon1.Icon = My.Resources._30_Sync
            t2 = New Threading.Thread(AddressOf initiator)
            t2.IsBackground = True
            t2.Priority = Threading.ThreadPriority.Lowest
            t2.Start()

            'GoTo Nextschedule
            'KillApp()
        Catch ex As Exception
            SetStatus(ex.Message)
            _ObjStatus.ShowDialog()
            If t2 IsNot Nothing Then t2.Abort()
            t2 = New Threading.Thread(AddressOf initiator)
            t2.IsBackground = True
            t2.Priority = Threading.ThreadPriority.Lowest
            t2.Start()
            ' KillApp()
        End Try
    End Sub
    Private Sub StartSync(Optional ByVal sendreconly As String = Nothing)

        Select Case _AppMode
            Case "SEND"
                If _SyncMode = "W" Then
                    WebSend()
                    WebSendLocal()
                Else
                    Send()
                End If
                KillApp()
            Case "RECEIVE"
                If _SyncMode = "W" Then
                    WebReceive()
                    WebReceiveLocal()
                Else
                    Receive()
                End If
                KillApp()
            Case Else
                If _SyncMode = "W" Then
                    If sendreconly <> "S" Then WebReceive() : WebReceiveLocal() : _ObjStatus = New Status
                    If sendreconly <> "R" Then WebSend() : WebSendLocal()
                Else
                    If sendreconly <> "S" Then Receive()
                    If sendreconly <> "R" Then Send()
                End If

        End Select
    End Sub

#Region "Send And Receive Using OutLook"

    Private Sub Send()
        SetStatus("Uploading..")
        'TxtStatus.Text = "Uploading.."
        Dim S_RootDir As String = System.IO.Path.GetTempPath & "\UploadXml"
        Dim S_DsTblCollection As DataSet = Nothing
        Dim S_DtToCostCenter As New DataTable
        Dim S_Dt As DataTable = Nothing
        Dim S_Ds As DataSet = Nothing
        Dim S_tToIds As String = ""
        Dim S_FromUserId As String = ""
        Dim S_FromUserPwd As String = ""
        Dim S_FromHostName As String = ""
        Dim S_FromPort As Integer = Nothing
        Dim S_ToUserId As String = ""
        Dim S_FileName As String = ""
        Dim S_FileNameXml As String = ""
        Dim S_FileNameZip As String = ""
        Dim S_ObjZip As Zipper
        Dim S_FileInfo As IO.FileInfo = Nothing
        Dim S_ChitDb As String = Nothing
        Dim S_RowLimit As Integer = 0
        StrSql = "SELECT COSTID FROM " & _Admindb & "..SYNCCOSTCENTRE"
        StrSql += " WHERE COSTID <> '" & _FromId & "'"
        StrSql += " AND ISNULL(MANUAL,'') <> 'Y'"
        _Da = New OleDbDataAdapter(StrSql, _Cn)
        _Da.Fill(S_DtToCostCenter)
        If Not S_DtToCostCenter.Rows.Count > 0 Then
            MsgBox("No receiver info in sync costcentre", MsgBoxStyle.Information)
            Exit Sub
        End If
        For Each Ro As DataRow In S_DtToCostCenter.Rows
            S_tToIds += "'" & Ro.Item("COSTID").ToString & "',"
        Next
        S_tToIds = Mid(S_tToIds, 1, S_tToIds.Length - 1)
        StrSql = " SELECT DISTINCT TOID FROM " & _Syncdb & "..SENDSYNC "
        StrSql += " WHERE FROMID = '" & _FromId & "' AND STATUS = 'N'"
        StrSql += " AND TOID IN (" & S_tToIds & ")"
        S_DtToCostCenter = New DataTable
        _Da = New OleDbDataAdapter(StrSql, _Cn)
        _Da.Fill(S_DtToCostCenter)
        If Not S_DtToCostCenter.Rows.Count > 0 Then
            Exit Sub
        End If
        If Not IO.Directory.Exists(S_RootDir) Then IO.Directory.CreateDirectory(S_RootDir)
        S_FromUserId = GMethods.GetSqlValue(_Cn, "SELECT EMAILID FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & _FromId & "'")
        S_FromUserPwd = GMethods.Decrypt(GetSqlValue(_Cn, " SELECT PASSWORD FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & _FromId & "'"))
        S_FromHostName = GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SMTP-HOSTNAME'")
        S_FromPort = Val(GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SMTP-PORT'"))
        S_RowLimit = Val(GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-ROWLIMIT'", , "0"))
        Dim fIndex As Integer = 0
        Dim tRowLimit As Integer = 0
        ''Transaction Stars here..
        Try
            For Each Ro As DataRow In S_DtToCostCenter.Rows
                'S_FileName = "SYNC-" & _FromId & "-" & Ro.Item("TOID").ToString & "-" & Today.Date.ToString("ddMMyy") & "-" & Date.Now.ToString("HHssmm")
                S_ToUserId = GetSqlValue(_Cn, "SELECT EMAILID FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & Ro.Item("TOID").ToString & "'")
                ''Getting Jewellery Data
                StrSql = "SELECT FROMID,TOID,SQLTEXT,STATUS,TAGIMAGE,TAGIMAGENAME,UID,UPDFILE FROM " & _Syncdb & "..SENDSYNC"
                StrSql += " WHERE FROMID = '" & _FromId & "' AND isnull(STATUS,'N') = 'N'"
                StrSql += " AND TOID = '" & Ro.Item("TOID").ToString & "'"
                StrSql += " ORDER BY UID"
                S_Dt = New DataTable
                _Cmd = New OleDbCommand(StrSql, _Cn)
                _Da = New OleDbDataAdapter(_Cmd)
                _Da.Fill(S_Dt)
                If Not S_Dt.Rows.Count > 0 Then Continue For 'Record not found

                S_DsTblCollection = New DataSet
                S_DsTblCollection.Tables.Add(S_Dt.Clone)
                For Each RoDt As DataRow In S_Dt.Rows
                    If S_DsTblCollection.Tables(S_DsTblCollection.Tables.Count - 1).Rows.Count = 500 Then S_DsTblCollection.Tables.Add(S_Dt.Clone)
                    If Not IsDBNull(RoDt.Item("TAGIMAGE")) Then
                        If S_DsTblCollection.Tables(S_DsTblCollection.Tables.Count - 1).Rows.Count > 0 Then S_DsTblCollection.Tables.Add(S_Dt.Clone)
                        S_DsTblCollection.Tables(S_DsTblCollection.Tables.Count - 1).ImportRow(RoDt)
                        S_DsTblCollection.Tables.Add(S_Dt.Clone)
                    Else
                        S_DsTblCollection.Tables(S_DsTblCollection.Tables.Count - 1).ImportRow(RoDt)
                    End If
                Next
                fIndex = 0
                For Each dt As DataTable In S_DsTblCollection.Tables
                    If Not dt.Rows.Count > 0 Then Continue For
                    fIndex += 1
                    _Tran = Nothing
                    _Tran = _Cn.BeginTransaction
                    S_FileName = "SYNC-" & _FromId & "-" & Ro.Item("TOID").ToString & "-" & Today.Date.ToString("ddMMyy") & "-" & Date.Now.ToString("HHssmm") & "_" & fIndex.ToString
                    S_Ds = New DataSet
                    dt.TableName = S_FileName
                    S_Ds.Tables.Add(dt.Copy)
                    ''Encrypting Qry
                    For Each row As DataRow In S_Ds.Tables(S_FileName).Rows
                        row("SQLTEXT") = GMethods.EncryptXml(row("SQLTEXT").ToString)
                    Next
                    S_Ds.Tables(S_FileName).AcceptChanges()

                    ''Creating xml File
                    S_FileNameXml = S_RootDir & "\" & S_FileName & ".xml"
                    Dim fs As New IO.StreamWriter(S_FileNameXml)
                    S_Ds.WriteXml(fs, XmlWriteMode.WriteSchema)
                    fs.Close()

                    ''Zipping
                    S_FileNameZip = S_RootDir & "\" & S_FileName & ".zip"
                    S_ObjZip = New Zipper
                    If Not S_ObjZip.Zip(S_FileNameXml, S_FileNameZip) Then
                        If IO.File.Exists(S_FileNameXml) Then IO.File.Delete(S_FileNameXml)
                        If IO.File.Exists(S_FileNameZip) Then IO.File.Delete(S_FileNameZip)
                        Continue For
                    Else
                        IO.File.Delete(S_FileNameXml)
                    End If

                    S_FileInfo = New FileInfo(S_FileNameZip)

                    ''Sending Mail
                    If Not SendMail.Send(S_FromHostName, S_FromPort, S_ToUserId, S_FileName, "SEND DATA", S_FileNameZip, S_FromUserId, S_FromUserPwd, S_FromUserId, True) Then Continue For
                    ''Updating sended records
                    StrSql = " UPDATE " & _Syncdb & "..SENDSYNC SET STATUS = 'M',UPDFILE = '" & S_FileName & "'"
                    StrSql += " WHERE FROMID = '" & _FromId & "' AND isnull(STATUS,'N') = 'N'"
                    StrSql += " AND TOID = '" & Ro.Item("TOID").ToString & "'"
                    StrSql += " AND UID BETWEEN " & dt.Rows(0).Item("UID") & " AND " & dt.Rows(dt.Rows.Count - 1).Item("UID") & ""
                    _Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
                    _Cmd.ExecuteNonQuery()
                    _Tran.Commit()
                    _Tran = Nothing
                Next
            Next
        Catch ex As Exception
            If Not _Tran Is Nothing Then _Tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub Receive()
        SetStatus("Downloading..")
        'TxtStatus.Text = "Downloading.."
        Dim R_StrFtr As String = Nothing
        Dim _DtFtr As New DataTable
        StrSql = " SELECT EMAILID FROM " & _Admindb & "..SYNCCOSTCENTRE "
        StrSql += " WHERE COSTID <> '" & _FromId & "'"
        _Da = New OleDbDataAdapter(StrSql, _Cn)
        _Da.Fill(_DtFtr)
        If Not _DtFtr.Rows.Count > 0 Then
            MsgBox("No sender info in sync costcentre", MsgBoxStyle.Information)
            Exit Sub
        End If
        For Each ro As DataRow In _DtFtr.Rows
            R_StrFtr += "[FROM] = '" & ro!EMAILID.ToString & "' OR"
        Next
        If Not OutlookCls.SyncOutlook() Then Exit Sub
        System.Threading.Thread.Sleep(3000)
        Dim R_ObjOL As New Microsoft.Office.Interop.Outlook.Application
        Dim R_RootDir As String = Path.GetTempPath & "\DownloadAttachments"
        If Not IO.Directory.Exists(R_RootDir) Then IO.Directory.CreateDirectory(R_RootDir)
        Dim R_InBox As Outlook.MAPIFolder = R_ObjOL.GetNamespace("MAPI").Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox)
        Dim R_InBoxItems As Outlook.Items = R_InBox.Items
        Dim R_NewEmail As Outlook.MailItem = Nothing
        Dim R_ObjZip As Zipper
        Dim R_FileNameZip As String
        Dim R_FileNameXml As String = ""

        Dim R_Subject() As String = Nothing
        Dim R_SenderId As String = Nothing

        If R_StrFtr <> Nothing Then
            If R_StrFtr.EndsWith("OR") Then R_StrFtr = Trim(R_StrFtr.Remove(R_StrFtr.Length - 2, 2))
            R_InBoxItems = R_InBoxItems.Restrict(R_StrFtr)
        End If

        Try
            For Each collectionItem As Object In R_InBoxItems
                R_NewEmail = TryCast(collectionItem, Outlook.MailItem)
                If R_NewEmail Is Nothing Then Continue For
                ''Validation
                If R_NewEmail.Subject Is Nothing Then Continue For
                If Not R_NewEmail.Subject.StartsWith("SYNC-") Then Continue For
                If Not R_NewEmail.Subject.Contains("-") Then Continue For
                R_Subject = R_NewEmail.Subject.ToUpper.Split("-")
                R_SenderId = R_Subject(1)
                If Not R_Subject.Length > 2 Then Continue For ''Has contain receiver id
                If Not _FromId.ToUpper = R_Subject(2).ToUpper Then Continue For
                If Not R_NewEmail.Attachments.Count > 0 Then Continue For

                ''Attachment Exists
                For i As Integer = 1 To R_NewEmail.Attachments.Count
                    R_FileNameZip = R_RootDir & "\" & (R_NewEmail.Attachments(i).FileName)
                    R_FileNameXml = R_RootDir & "\" & (R_NewEmail.Attachments(i).FileName).Replace(".zip", ".xml")
                    Dim fINfo As New IO.FileInfo(R_FileNameXml)
                    R_NewEmail.Attachments(i).SaveAsFile(R_FileNameZip)
                    R_ObjZip = New Zipper
                    If Not R_ObjZip.UnZip(R_FileNameZip, fINfo.DirectoryName) Then
                        If IO.File.Exists(R_FileNameZip) Then IO.File.Delete(R_FileNameZip)
                        Continue For
                    Else
                        IO.File.Delete(R_FileNameZip)
                    End If
                    If InsertIntoDb(R_SenderId, R_FileNameXml) Then
                        R_NewEmail.Delete()
                    Else
                        Exit For
                    End If
                    If IO.File.Exists(R_FileNameXml) Then IO.File.Delete(R_FileNameXml)
                Next
            Next collectionItem
        Catch ex As Exception
Errorr:
            MsgBox(R_FileNameXml + vbCrLf + ex.Message + vbCrLf + ex.StackTrace)
            Me.Close()
            Application.Exit()
        End Try
    End Sub

    Private Function InsertIntoDb(ByVal SenderId As String, ByVal FileName As String) As Boolean
        Dim I_QryOriginal As String = ""
        Dim OldStr As String = ""
        Dim NewStr As String = ""
        Try
            Dim I_Qry As String = Nothing
            Dim I_Row As DataRow = Nothing
            Dim I_FStream As New IO.FileStream(FileName, IO.FileMode.Open, IO.FileAccess.Read)
            Dim I_Ds As New DataSet
            I_Ds.ReadXml(I_FStream, XmlReadMode.ReadSchema)
            I_FStream.Close()

            Dim I_Finfo As New FileInfo(FileName)
            StrSql = "SELECT 'CHECK' FROM " & _Syncdb & "..RECEIVESYNC WHERE UPDFILE = '" & I_Finfo.Name & "'"
            If GMethods.GetSqlValue(_Cn, StrSql).Length > 0 Then
                MsgBox(I_Finfo.Name & " Already Downloaded", MsgBoxStyle.Information)
                Return False
            End If
            Dim I_PicPath As String = GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")
            Dim I_FromCompId As String = GMethods.GetSqlValue(_Cn, "SELECT COMPID FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & _FromId & "'")
            Dim I_ToCompId As String = GMethods.GetSqlValue(_Cn, "SELECT COMPID FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & SenderId & "'")
            Dim I_VbPrefix As String = GMethods.GetSqlValue(_Cn, "SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'CHITDBPREFIX'")
            Dim I_SenderDb As String = ""
            Dim I_ReplaceDb As String = ""
            Dim I_IndexOfDb As Integer
            _Tran = _Cn.BeginTransaction
            For cnt As Integer = 0 To I_Ds.Tables(0).Rows.Count - 1
                I_Row = I_Ds.Tables(0).Rows(cnt)
                StrSql = GMethods.DecryptXml(I_Row!SQLTEXT.ToString)
                I_QryOriginal = StrSql
                For Each suffix As String In _ReplaceWords
                    If suffix.ToUpper = "SAVINGS" Or suffix.ToUpper = "SH0708" Then I_ReplaceDb = I_VbPrefix Else I_ReplaceDb = I_FromCompId
                    I_IndexOfDb = StrSql.ToUpper.IndexOf(suffix.ToUpper)
                    If I_IndexOfDb > -1 Then
                        I_SenderDb = Mid(StrSql.ToUpper, I_IndexOfDb - 2, 3)
                        I_SenderDb = I_ToCompId
                        For Each suf As String In _ReplaceWords
                            OldStr = I_SenderDb.ToUpper & suf
                            NewStr = I_ReplaceDb.ToUpper & suf
                            'StrSql = StrSql.ToUpper.Replace(OldStr, NewStr)
                            StrSql = StrSql.Replace(OldStr, NewStr.ToUpper)
                        Next
                    End If
                Next
                Dim R_Dbname As String
                R_Dbname = GetSqlValue(_Cn, "SELECT TOP 1 DBNAME FROM " & _Admindb & "..DBMASTER ORDER BY ENDDATE DESC", , , _Tran)
                For Each T As String In ListTransferTables
                    If StrSql.Contains(R_Dbname & ".." & T) Then
                        StrSql = ReplaceQryStrNew(StrSql, R_Dbname & ".." & T, Mid(R_Dbname, 1, 3) & "ADMINDB.." & T)
                    End If
                Next
                I_Qry = " INSERT INTO " & _Syncdb & "..RECEIVESYNC(FROMID,TOID,SQLTEXT"
                If I_Row!TAGIMAGE.ToString <> "" Then I_Qry += ",TAGIMAGE,TAGIMAGENAME"
                I_Qry += " ,UPDFILE)"
                I_Qry += " VALUES"
                I_Qry += " (?,?,?"
                If I_Row!TAGIMAGE.ToString <> "" Then I_Qry += ",?,?"
                I_Qry += " ,?)"
                _Cmd = New OleDbCommand(I_Qry, _Cn, _Tran)
                _Cmd.Parameters.AddWithValue("@FROMID", I_Row!FROMID.ToString)
                _Cmd.Parameters.AddWithValue("@TOID", I_Row!TOID.ToString)
                _Cmd.Parameters.AddWithValue("@SQLTEXT", StrSql)
                If I_Row!TAGIMAGE.ToString <> "" Then
                    _Cmd.Parameters.AddWithValue("@TAGIMAGE", I_Row!TAGIMAGE)
                    _Cmd.Parameters.AddWithValue("@TAGIMAGENAME", I_Row!TAGIMAGENAME)
                End If
                _Cmd.Parameters.AddWithValue("@UPDFILE", Mid(I_Finfo.Name, 1, I_Finfo.Name.Length - 4))
                _Cmd.ExecuteNonQuery()
                If I_Row!TAGIMAGE.ToString <> "" Then
                    If Not IO.Directory.Exists(I_PicPath) Then
                        If _Tran IsNot Nothing Then _Tran.Rollback()
                        MsgBox(I_PicPath & " not found. Please make appropriate path", MsgBoxStyle.Information)
                        Return False
                    End If
                    Dim myByte() As Byte = I_Row!TAGIMAGE
                    Dim stream As System.IO.MemoryStream
                    Dim img As Image
                    stream = New System.IO.MemoryStream()
                    stream.Write(myByte, 0, myByte.Length)
                    img = Image.FromStream(stream, True)
                    img.Save(I_PicPath & "\" & I_Row!TAGIMAGENAME.ToString)
                    stream.Close()
                End If
            Next

            Dim ret As String = Nothing
            ret = "UPDATE " & _Syncdb & "..SENDSYNC SET STATUS = 'Y' "
            ret += " ,UPDFILE = '" & Mid(I_Finfo.Name, 1, I_Finfo.Name.Length - 4) & "'"
            ret += " WHERE FROMID = '" & SenderId & "'"
            ret += " AND TOID = '" & _FromId & "'"
            ret += " AND STATUS = 'M'"
            ret += " AND UID BETWEEN " & Val(I_Ds.Tables(0).Rows(0).Item("UID").ToString) & ""
            ret += " AND " & Val(I_Ds.Tables(0).Rows(I_Ds.Tables(0).Rows.Count - 1).Item("UID").ToString) & ""

            StrSql = " INSERT INTO " & _Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,UPDFILE)"
            StrSql += " VALUES"
            StrSql += " ("
            StrSql += " '" & _FromId & "'"
            StrSql += " ,'" & SenderId & "'"
            StrSql += " ,'" & ret.Replace("'", "''") & "'"
            StrSql += " ,'" & Mid(I_Finfo.Name, 1, I_Finfo.Name.Length - 4) & "'"
            StrSql += " )"
            _Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            _Cmd.ExecuteNonQuery()
            _Tran.Commit()
            Return True
        Catch ex As Exception
            If _Tran IsNot Nothing Then _Tran.Rollback()
            Dim objErr As New ErrorQryDia
            objErr.txtErr.Text = StrSql
            objErr.txtOrginal.Text = I_QryOriginal
            objErr.Text = objErr.Text & "  " & "OLDSTR : " & OldStr & " NEWSTR : " & NewStr
            objErr.ShowDialog()
            MsgBox(FileName + vbCrLf + ex.Message + vbCrLf + ex.StackTrace)
            Me.Close()
            Application.Exit()
            Return False
        End Try
    End Function
#End Region

#Region "Send And Receive Using WebSpace"

    Private Sub WebSend()

        SetStatus("Uploading..")
        _ObjStatus.AddStatus("Uploading data ")
        NotifyIcon1.Text = _Status_msg & vbCrLf & "Uploading Data " & _CompanyId & "_" & _FromId
        NotifyIcon1.BalloonTipText = "Uploading Data @ " & _FromId
        Dim S_DsTblCollection As DataSet = Nothing
        Dim S_DtToCostCenter As New DataTable
        Dim S_Dt As DataTable = Nothing
        Dim S_Ds As DataSet = Nothing
        Dim S_tToIds As String = ""
        Dim exitWhile As Boolean = False
        Dim S_UpdFile As String = ""
        Dim S_ErrorState As String = ""
        Dim S_ImageSync As Boolean = False
        _ObjStatus.AddStatus("Initiating Upload")
        _ObjStatus.AddStatus("Get Sender Info")
        If _Sync_Via_R Then
            StrSql = "SELECT COSTID FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE ISNULL(WEBDBNAME,'')=''"
        Else
            StrSql = "SELECT COSTID FROM " & IIf(_VbDbPrefix = "", _CompanyId, _VbDbPrefix) & "SAVINGS..SYNCCOSTCENTRE  WHERE (ISNULL(WEBDBNAME,'')='' OR ISNULL(WEBDBNAME,'')<>'')"
        End If
        StrSql += " AND COSTID <> '" & _FromId & "'"
        StrSql += " AND ISNULL(MANUAL,'') <> 'Y'"
        _Da = New OleDbDataAdapter(StrSql, _CnAdmin)
        _Da.Fill(S_DtToCostCenter)
        If Not S_DtToCostCenter.Rows.Count > 0 Then
            _ObjStatus.AddStatus("No receiver info in sync costcentre")
            _ObjStatus.ShowDialog()
            Exit Sub
        End If
        For Each Ro As DataRow In S_DtToCostCenter.Rows
            S_tToIds += "'" & Ro.Item("COSTID").ToString & "',"
        Next
        S_tToIds = Mid(S_tToIds, 1, S_tToIds.Length - 1)
        StrSql = " SELECT DISTINCT TOID FROM " & _Syncdb & "..SENDSYNC "
        StrSql += " WHERE FROMID = '" & _FromId & "' AND ISNULL(STATUS,'N') = 'N'"
        StrSql += " AND TOID IN (" & S_tToIds & ")"
        S_DtToCostCenter = New DataTable
        _Da = New OleDbDataAdapter(StrSql, _CnAdmin)
        _Da.Fill(S_DtToCostCenter)
        If Not S_DtToCostCenter.Rows.Count > 0 Then
            Exit Sub
        End If
        ''Transaction Stars here..

CounterStarts:
        Dim Counter As Integer = 0
        Dim LockCounter As Integer = 0
        Dim Row As DataRow = Nothing
        Dim Uids As String = Nothing
        Dim filecount As Integer = 0
        Try
            _ObjStatus.AddStatus("Get Connection from Web")
            _CnWeb = New OleDbConnection(GetWebConnectionString)
            _CnWeb.Open()
            If _CnAdmin.State = ConnectionState.Closed Then _CnAdmin.Open()
           

            For Each Ro As DataRow In S_DtToCostCenter.Rows
                ''Getting Jewellery Data
                filecount = 0
                exitWhile = False
                While 1 = 1
                    Uids = ""
                    S_ImageSync = False
                    Counter += 1
                    filecount += 1
                    LockCounter = 0
                  '  MsgBox(Now.TimeOfDay.ToString)
                    _Tran = Nothing
                    '_Tran = _Cn.BeginTransaction
                    _Tran = _CnAdmin.BeginTransaction
                    _ObjStatus.AddStatus("Getting Send Data")
                    StrSql = "SELECT TOP 200 FROMID,TOID,SQLTEXT,STATUS,TAGIMAGE,TAGIMAGENAME,UID,UPDFILE,IMAGE_CTRLID FROM " & _Syncdb & "..SENDSYNC"
                    StrSql += " WHERE FROMID = '" & _FromId & "' AND ISNULL(STATUS,'N') = 'N'"
                    StrSql += " AND TOID = '" & Ro.Item("TOID").ToString & "'"
                    StrSql += " AND TAGIMAGE IS NULL"
                    'StrSql += " AND ((TAGIMAGE IS NOT NULL AND ISNULL(IMAGE_CTRLID,'') <> '') OR TAGIMAGE IS NULL)"
                    StrSql += " ORDER BY UID"
                    S_Dt = New DataTable : _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran) : _Da = New OleDbDataAdapter(_Cmd)
                    _Da.Fill(S_Dt)
                    S_Dt.AcceptChanges()
                   

                    If Not S_Dt.Rows.Count > 0 Then
                        StrSql = "SELECT TOP 1 FROMID,TOID,SQLTEXT,STATUS,TAGIMAGE,TAGIMAGENAME,UID,UPDFILE,IMAGE_CTRLID FROM " & _Syncdb & "..SENDSYNC"
                        StrSql += " WHERE FROMID = '" & _FromId & "' AND ISNULL(STATUS,'N') = 'N'"
                        StrSql += " AND TOID = '" & Ro.Item("TOID").ToString & "'"
                        StrSql += " AND TAGIMAGE IS NOT NULL "
                        StrSql += " ORDER BY UID"
                        S_Dt = New DataTable : _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran) : _Da = New OleDbDataAdapter(_Cmd)
                        _Da.Fill(S_Dt)
                        S_Dt.AcceptChanges()
                        If S_Dt.Rows.Count > 0 Then
                             S_ImageSync = True
                            GoTo RecordFound

                        End If

                        _ObjStatus.AddStatus("Record not found")
                        
                        exitWhile = True
                        GoTo recordnotfound
                    End If
RecordFound:
                    _TranWeb = Nothing
                    _TranWeb = _CnWeb.BeginTransaction
                    StrSql = "SELECT COUNT(*)CNT FROM " & _WebDbTblPrefix & "_SYNCTABLE WHERE FROMID='" & _FromId & "'"
                    Dim CmdChk As New OleDbCommand(StrSql, _CnWeb, _TranWeb)
                    Dim daChk As New OleDbDataAdapter(CmdChk)
                    Dim dtChk As New DataTable
                    daChk.Fill(dtChk)
                    'If _CnWeb.DataSource <> "108.170.45.170" Then GoTo NoCheck
                    If dtChk.Rows.Count > 0 Then
                        If Val(dtChk.Rows(0).Item("CNT").ToString) >= 20 Then
                            StrSql = "SELECT TOID,COUNT(*)CNT FROM " & _WebDbTblPrefix & "_SYNCTABLE WHERE FROMID='" & _FromId & "' GROUP BY TOID ORDER BY COUNT(*) DESC"
                            Dim CmdChk1 As New OleDbCommand(StrSql, _CnWeb, _TranWeb)
                            Dim daChk1 As New OleDbDataAdapter(CmdChk1)
                            Dim dtChk1 As New DataTable
                            daChk1.Fill(dtChk1)
                            Dim strRowCount As String = "Maximum Size Reached in Cloud for " & _FromId & " Location"
                            For Each dr As DataRow In dtChk1.Rows
                                strRowCount += vbCrLf + dr.Item("TOID").ToString & " : " & dr.Item("CNT").ToString
                            Next
                            SetStatus(strRowCount)
                            exitWhile = True
                            GoTo recordnotfound
                        End If
                    End If
NoCheck:
                    S_UpdFile = "WSYNC-" & _FromId & "-" & Ro.Item("TOID").ToString & "-" & Today.Date.ToString("ddMMyy") & "-" & Date.Now.ToString("HHssmm") & "-" & filecount.ToString
                    Uids = ""
                    Dim maxuid As Long = S_Dt.Compute("max(uid)", Nothing)
                    Dim minuid As Long = S_Dt.Compute("min(uid)", Nothing)
                    Dim sendrec As Long = S_Dt.Rows.Count

                    Dim DsSend As New DataSet
                    Dim XmlFilePath As String = Nothing
                    Dim ZipFilePath As String = Nothing
                    XmlFilePath = IO.Path.GetTempPath & S_UpdFile & ".xml"
                    ZipFilePath = IO.Path.GetTempPath & S_UpdFile & ".zip"
                    DsSend.Tables.Add(S_Dt)
                    DsSend.AcceptChanges()
                    DsSend.WriteXml(XmlFilePath, XmlWriteMode.WriteSchema)

                    Dim dssent As New DataSet
                    dssent.ReadXml(XmlFilePath, XmlReadMode.ReadSchema)
                    Dim smaxuid As Long = dssent.Tables(0).Compute("max(uid)", Nothing)
                    Dim sminuid As Long = dssent.Tables(0).Compute("min(uid)", Nothing)
                    Dim sentrec As Long = dssent.Tables(0).Rows.Count
                    If smaxuid <> maxuid Or sminuid <> minuid Then Throw New Exception("Syncronization not completed.. Sending data max vs min Uids mismatched")
                    If sendrec <> sentrec Then Throw New Exception("Syncronization not completed.. Sending data Records count mismatched")

                    Dim objZip As New Zipper
                    If Not objZip.Zip(XmlFilePath, ZipFilePath) Then
                        If IO.File.Exists(XmlFilePath) Then IO.File.Delete(XmlFilePath)
                        If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
                    Else
                        IO.File.Delete(XmlFilePath)
                    End If

                    Dim fiINfo As New IO.FileInfo(ZipFilePath)
                    StrSql = " INSERT INTO " & _WebDbTblPrefix & "_SYNCTABLE"
                    StrSql += vbCrLf + " (FROMID,TOID"
                    StrSql += vbCrLf + " ,CONTENT,UPDFILE"
                    StrSql += vbCrLf + "  )"
                    StrSql += vbCrLf + "  VALUES"
                    StrSql += vbCrLf + "  (?,?,?,?)"
                    Dim CmdIns As New OleDbCommand(StrSql, _CnWeb, _TranWeb)
                    Dim fileStr As New IO.FileStream(ZipFilePath, IO.FileMode.Open, IO.FileAccess.Read)
                    Dim reader As New IO.BinaryReader(fileStr)
                    Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Long))
                    fileStr.Read(result, 0, result.Length)
                    fileStr.Close()
                    CmdIns.Parameters.AddWithValue("@FROMID", _FromId)
                    CmdIns.Parameters.AddWithValue("@TOID", Ro.Item("TOID").ToString)
                    CmdIns.Parameters.AddWithValue("@CONTENT", result)
                    CmdIns.Parameters.AddWithValue("@UPDFILE", S_UpdFile)
                    CmdIns.ExecuteNonQuery()



                    If Not objZip.UnZip(ZipFilePath, IO.Path.GetTempPath) Then
                        If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
                    Else
                        If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
                    End If
                    Dim dssentf As New DataSet
                    dssentf.ReadXml(XmlFilePath, XmlReadMode.ReadSchema)
                    Dim smaxuidf As Long = dssent.Tables(0).Compute("max(uid)", Nothing)
                    Dim sminuidf As Long = dssent.Tables(0).Compute("min(uid)", Nothing)
                    Dim sentrecf As Long = dssent.Tables(0).Rows.Count
                    If smaxuidf <> maxuid Or sminuidf <> minuid Then Throw New Exception("Syncronization not completed.. Sending data max vs min Uids mismatched")
                    If sentrecf <> sentrec Then Throw New Exception("Syncronization not completed.. Sending data Records count mismatched")


                    'Updating sended records
recordnotfound:
                    If exitWhile Then GoTo ExitWhilef
                    If S_ImageSync Then
                        _ObjStatus.AddStatus("Delete Sqltext")
                        If minuid <> 0 And maxuid <> 0 Then
                            For iiii As Long = minuid To maxuid
                                If dssent.Tables(0).Select("UID =" & iiii, Nothing).Length <> 0 Then
                                    StrSql = " DELETE FROM " & _Syncdb & "..SENDSYNC "
                                    StrSql += " WHERE FROMID = '" & _FromId & "' AND ISNULL(STATUS,'N')  = 'N'"
                                    StrSql += " AND TOID = '" & Ro.Item("TOID").ToString & "'"
                                    StrSql += " AND UID =" & iiii
                                    StrSql += " AND TAGIMAGE IS NOT NULL"
                                    If StrSql <> "" Then _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran) : _Cmd.ExecuteNonQuery()
                                End If
                            Next
                        End If
                    Else
                        'sending data confirmation 
                        If S_UpdFile = "" Then Throw New Exception("Updated file is empty")
                        ' _ObjStatus.AddStatus("Check Data Integrity")
                        'If Not SendDataIntegrity(_FromId, S_UpdFile) Then Throw New Exception("Syncronization not completed.. Sending data mismatched")
                        _ObjStatus.AddStatus("Update Record Status ")
                        StrSql = ""
                        If minuid <> 0 And maxuid <> 0 Then
                            For iiii As Long = minuid To maxuid
                                If dssent.Tables(0).Select("UID =" & iiii, Nothing).Length <> 0 Then
                                    StrSql = " UPDATE " & _Syncdb & "..SENDSYNC SET STATUS = 'M',UPDFILE = '" & S_UpdFile & "'"
                                    StrSql += " WHERE UID =" & iiii
                                    'StrSql += " WHERE FROMID = '" & _FromId & "' AND ISNULL(STATUS,'N') = 'N'"
                                    'StrSql += " AND TOID = '" & Ro.Item("TOID").ToString & "'"
                                    ' 'StrSql += " AND UID BETWEEN " & minuid & " AND " & maxuid & ""
                                    'StrSql += " AND UID =" & iiii
                                    'StrSql += " AND TAGIMAGE IS NULL"
                                    If StrSql <> "" Then _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran) : _Cmd.ExecuteNonQuery()
                                End If
                            Next
                        End If
                    End If
exitWhilef:
                    _Tran.Commit()
                    ' MsgBox(Now.TimeOfDay.ToString)
                    _Tran = Nothing
                    If _TranWeb IsNot Nothing Then _TranWeb.Commit() : _TranWeb = Nothing
                    If Counter >= 5000 Or exitWhile Then
                        exitWhile = False
                        Exit While
                    End If
                    If _SyncDelay > 0 Then Thread.Sleep(_SyncDelay * 1000)
                End While

            Next
            
            StrSql = "UPDATE " & _Admindb & "..SOFTCONTROL SET CTLTEXT = ''"
            StrSql += " WHERE CTLID = 'SYNC-LOCK-SEND'"
            _Cmd = New OleDbCommand(StrSql, _CnAdmin)
            _Cmd.ExecuteNonQuery()

            _CnWeb.Close()
            _CnAdmin.Close()
            _Cn.Close()
        Catch ex As Exception
            If ex.Message.Contains("was deadlocked on lock") = True Then
                'MsgBox("")
            End If
            If _Tran IsNot Nothing Then
                If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
            End If
            If _TranWeb IsNot Nothing Then
                If _TranWeb.Connection IsNot Nothing Then _TranWeb.Rollback()
            End If
            My.Settings.Er_Text = ex.Message
            If My.Settings.Er_OldText <> My.Settings.Er_Text Then
                Try
                    My.Settings.Er_Created = Date.Now
                    My.Settings.Er_OldText = ex.Message
                    _ObjStatus.AddStatus("Sending Error Report")
                    SendMail.Send("smtp.gmail.com", 587, _SyncErrSendId, "Sync Error From_" & _Admindb & "_" & _FromId, ex.Message, "", "akshayagoldhelpline@gmail.com", "giritech@123", "Giritech Support Team", , False)
                    My.Settings.Save()
                Catch ex1 As Exception

                End Try
            End If
            SetStatus(ex.Message)
            _ObjStatus.AddStatus(ex.Message)
            _ObjStatus.ShowDialog()
            'KillApp()
        End Try
        _ObjStatus.AddStatus("Upload Complete")
    End Sub

    Private Sub WebSendLocal()

        _ObjStatus.AddStatus("Uploading data ")
        Dim S_DsTblCollection As DataSet = Nothing
        Dim S_DtToCostCenter As New DataTable
        Dim S_Dt As DataTable = Nothing
        Dim S_Ds As DataSet = Nothing
        Dim S_tToIds As String = ""
        Dim exitWhile As Boolean = False
        Dim S_UpdFile As String = ""
        Dim S_ErrorState As String = ""
        Dim S_ImageSync As Boolean = False
        _ObjStatus.AddStatus("Initiating Upload")
        _ObjStatus.AddStatus("Get Sender Info")
        If _Sync_Via_R Then
            StrSql = "SELECT COSTID FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE ISNULL(WEBDBNAME,'')<>''"
        Else
            StrSql = "SELECT COSTID FROM " & IIf(_VbDbPrefix = "", _CompanyId, _VbDbPrefix) & "SAVINGS..SYNCCOSTCENTRE  WHERE (ISNULL(WEBDBNAME,'')='' OR ISNULL(WEBDBNAME,'')<>'')"
        End If
        StrSql += " AND COSTID <> '" & _FromId & "'"
        StrSql += " AND ISNULL(MANUAL,'') <> 'Y'"
        _Da = New OleDbDataAdapter(StrSql, _CnAdmin)
        _Da.Fill(S_DtToCostCenter)
        If Not S_DtToCostCenter.Rows.Count > 0 Then
            '_ObjStatus.AddStatus("No receiver info in sync costcentre")
            '_ObjStatus.ShowDialog()
            Exit Sub
        End If
        SetStatus("Uploading..")
        NotifyIcon1.Text = _Status_msg & vbCrLf & "Uploading Data " & _CompanyId & "_" & _FromId
        NotifyIcon1.BalloonTipText = "Uploading Data @ " & _FromId
        For Each Ro As DataRow In S_DtToCostCenter.Rows
            S_tToIds += "'" & Ro.Item("COSTID").ToString & "',"
        Next
        S_tToIds = Mid(S_tToIds, 1, S_tToIds.Length - 1)
        StrSql = " SELECT DISTINCT TOID FROM " & _Syncdb & "..SENDSYNC "
        StrSql += " WHERE FROMID = '" & _FromId & "' AND ISNULL(STATUS,'N') = 'N'"
        StrSql += " AND TOID IN (" & S_tToIds & ")"
        S_DtToCostCenter = New DataTable
        _Da = New OleDbDataAdapter(StrSql, _CnAdmin)
        _Da.Fill(S_DtToCostCenter)
        If Not S_DtToCostCenter.Rows.Count > 0 Then
            Exit Sub
        End If
        ''Transaction Stars here..

CounterStarts:
        Dim Counter As Integer = 0
        Dim LockCounter As Integer = 0
        Dim Row As DataRow = Nothing
        Dim Uids As String = Nothing
        Dim filecount As Integer = 0
        Try
            _ObjStatus.AddStatus("Get Connection from Web")
            If _CnAdmin.State = ConnectionState.Closed Then _CnAdmin.Open()


            For Each Ro As DataRow In S_DtToCostCenter.Rows
                _CnWebLocal = New OleDbConnection(GetWebLocalConnectionString(Ro.Item("TOID").ToString))
                _CnWebLocal.Open()
                ''Getting Jewellery Data
                filecount = 0
                exitWhile = False
                While 1 = 1
                    Uids = ""
                    S_ImageSync = False
                    Counter += 1
                    filecount += 1
                    LockCounter = 0
                    '  MsgBox(Now.TimeOfDay.ToString)
                    _Tran = Nothing
                    '_Tran = _Cn.BeginTransaction
                    _Tran = _CnAdmin.BeginTransaction
                    _ObjStatus.AddStatus("Getting Send Data")
                    StrSql = "SELECT TOP 200 FROMID,TOID,SQLTEXT,STATUS,TAGIMAGE,TAGIMAGENAME,UID,UPDFILE,IMAGE_CTRLID FROM " & _Syncdb & "..SENDSYNC"
                    StrSql += " WHERE FROMID = '" & _FromId & "' AND ISNULL(STATUS,'N') = 'N'"
                    StrSql += " AND TOID = '" & Ro.Item("TOID").ToString & "'"
                    StrSql += " AND TAGIMAGE IS NULL"
                    'StrSql += " AND ((TAGIMAGE IS NOT NULL AND ISNULL(IMAGE_CTRLID,'') <> '') OR TAGIMAGE IS NULL)"
                    StrSql += " ORDER BY UID"
                    S_Dt = New DataTable : _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran) : _Da = New OleDbDataAdapter(_Cmd)
                    _Da.Fill(S_Dt)
                    S_Dt.AcceptChanges()


                    If Not S_Dt.Rows.Count > 0 Then
                        StrSql = "SELECT TOP 1 FROMID,TOID,SQLTEXT,STATUS,TAGIMAGE,TAGIMAGENAME,UID,UPDFILE,IMAGE_CTRLID FROM " & _Syncdb & "..SENDSYNC"
                        StrSql += " WHERE FROMID = '" & _FromId & "' AND ISNULL(STATUS,'N') = 'N'"
                        StrSql += " AND TOID = '" & Ro.Item("TOID").ToString & "'"
                        StrSql += " AND TAGIMAGE IS NOT NULL "
                        StrSql += " ORDER BY UID"
                        S_Dt = New DataTable : _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran) : _Da = New OleDbDataAdapter(_Cmd)
                        _Da.Fill(S_Dt)
                        S_Dt.AcceptChanges()
                        If S_Dt.Rows.Count > 0 Then
                            S_ImageSync = True
                            GoTo RecordFound

                        End If

                        _ObjStatus.AddStatus("Record not found")

                        exitWhile = True
                        GoTo recordnotfound
                    End If
RecordFound:
                    _TranWeb = Nothing
                    _TranWeb = _CnWebLocal.BeginTransaction
                    S_UpdFile = "WSYNC-" & _FromId & "-" & Ro.Item("TOID").ToString & "-" & Today.Date.ToString("ddMMyy") & "-" & Date.Now.ToString("HHssmm") & "-" & filecount.ToString
                    Uids = ""
                    Dim maxuid As Long = S_Dt.Compute("max(uid)", Nothing)
                    Dim minuid As Long = S_Dt.Compute("min(uid)", Nothing)
                    Dim sendrec As Long = S_Dt.Rows.Count

                    Dim DsSend As New DataSet
                    Dim XmlFilePath As String = Nothing
                    Dim ZipFilePath As String = Nothing
                    XmlFilePath = IO.Path.GetTempPath & S_UpdFile & ".xml"
                    ZipFilePath = IO.Path.GetTempPath & S_UpdFile & ".zip"
                    DsSend.Tables.Add(S_Dt)
                    DsSend.AcceptChanges()
                    DsSend.WriteXml(XmlFilePath, XmlWriteMode.WriteSchema)

                    Dim dssent As New DataSet
                    dssent.ReadXml(XmlFilePath, XmlReadMode.ReadSchema)
                    Dim smaxuid As Long = dssent.Tables(0).Compute("max(uid)", Nothing)
                    Dim sminuid As Long = dssent.Tables(0).Compute("min(uid)", Nothing)
                    Dim sentrec As Long = dssent.Tables(0).Rows.Count
                    If smaxuid <> maxuid Or sminuid <> minuid Then Throw New Exception("Syncronization not completed.. Sending data max vs min Uids mismatched")
                    If sendrec <> sentrec Then Throw New Exception("Syncronization not completed.. Sending data Records count mismatched")

                    Dim objZip As New Zipper
                    If Not objZip.Zip(XmlFilePath, ZipFilePath) Then
                        If IO.File.Exists(XmlFilePath) Then IO.File.Delete(XmlFilePath)
                        If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
                    Else
                        IO.File.Delete(XmlFilePath)
                    End If

                    Dim fiINfo As New IO.FileInfo(ZipFilePath)
                    StrSql = " INSERT INTO " & _LocalTblPrefix & "_SYNCTABLE"
                    StrSql += vbCrLf + " (FROMID,TOID"
                    StrSql += vbCrLf + " ,CONTENT,UPDFILE"
                    StrSql += vbCrLf + "  )"
                    StrSql += vbCrLf + "  VALUES"
                    StrSql += vbCrLf + "  (?,?,?,?)"
                    Dim CmdIns As New OleDbCommand(StrSql, _CnWebLocal, _TranWeb)
                    Dim fileStr As New IO.FileStream(ZipFilePath, IO.FileMode.Open, IO.FileAccess.Read)
                    Dim reader As New IO.BinaryReader(fileStr)
                    Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Long))
                    fileStr.Read(result, 0, result.Length)
                    fileStr.Close()
                    CmdIns.Parameters.AddWithValue("@FROMID", _FromId)
                    CmdIns.Parameters.AddWithValue("@TOID", Ro.Item("TOID").ToString)
                    CmdIns.Parameters.AddWithValue("@CONTENT", result)
                    CmdIns.Parameters.AddWithValue("@UPDFILE", S_UpdFile)
                    CmdIns.ExecuteNonQuery()



                    If Not objZip.UnZip(ZipFilePath, IO.Path.GetTempPath) Then
                        If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
                    Else
                        If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
                    End If
                    Dim dssentf As New DataSet
                    dssentf.ReadXml(XmlFilePath, XmlReadMode.ReadSchema)
                    Dim smaxuidf As Long = dssent.Tables(0).Compute("max(uid)", Nothing)
                    Dim sminuidf As Long = dssent.Tables(0).Compute("min(uid)", Nothing)
                    Dim sentrecf As Long = dssent.Tables(0).Rows.Count
                    If smaxuidf <> maxuid Or sminuidf <> minuid Then Throw New Exception("Syncronization not completed.. Sending data max vs min Uids mismatched")
                    If sentrecf <> sentrec Then Throw New Exception("Syncronization not completed.. Sending data Records count mismatched")


                    'Updating sended records
recordnotfound:
                    If exitWhile Then GoTo ExitWhilef
                    If S_ImageSync Then
                        _ObjStatus.AddStatus("Delete Sqltext")
                        If minuid <> 0 And maxuid <> 0 Then
                            For iiii As Long = minuid To maxuid
                                If dssent.Tables(0).Select("UID =" & iiii, Nothing).Length <> 0 Then
                                    StrSql = " DELETE FROM " & _Syncdb & "..SENDSYNC "
                                    StrSql += " WHERE FROMID = '" & _FromId & "' AND ISNULL(STATUS,'N')  = 'N'"
                                    StrSql += " AND TOID = '" & Ro.Item("TOID").ToString & "'"
                                    StrSql += " AND UID =" & iiii
                                    StrSql += " AND TAGIMAGE IS NOT NULL"
                                    If StrSql <> "" Then _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran) : _Cmd.ExecuteNonQuery()
                                End If
                            Next
                        End If
                    Else
                        'sending data confirmation 
                        If S_UpdFile = "" Then Throw New Exception("Updated file is empty")
                        ' _ObjStatus.AddStatus("Check Data Integrity")
                        'If Not SendDataIntegrity(_FromId, S_UpdFile) Then Throw New Exception("Syncronization not completed.. Sending data mismatched")
                        _ObjStatus.AddStatus("Update Record Status ")
                        StrSql = ""
                        If minuid <> 0 And maxuid <> 0 Then
                            For iiii As Long = minuid To maxuid
                                If dssent.Tables(0).Select("UID =" & iiii, Nothing).Length <> 0 Then
                                    StrSql = " UPDATE " & _Syncdb & "..SENDSYNC SET STATUS = 'M',UPDFILE = '" & S_UpdFile & "'"
                                    StrSql += " WHERE UID =" & iiii
                                    'StrSql += " WHERE FROMID = '" & _FromId & "' AND ISNULL(STATUS,'N') = 'N'"
                                    'StrSql += " AND TOID = '" & Ro.Item("TOID").ToString & "'"
                                    ' 'StrSql += " AND UID BETWEEN " & minuid & " AND " & maxuid & ""
                                    'StrSql += " AND UID =" & iiii
                                    'StrSql += " AND TAGIMAGE IS NULL"
                                    If StrSql <> "" Then _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran) : _Cmd.ExecuteNonQuery()
                                End If
                            Next
                        End If
                    End If
exitWhilef:
                    _Tran.Commit()
                    ' MsgBox(Now.TimeOfDay.ToString)
                    _Tran = Nothing
                    If _TranWeb IsNot Nothing Then _TranWeb.Commit() : _TranWeb = Nothing
                    If Counter >= 5000 Or exitWhile Then
                        exitWhile = False
                        Exit While
                    End If
                    If _SyncDelay > 0 Then Thread.Sleep(_SyncDelay * 1000)
                End While

            Next

            StrSql = "UPDATE " & _Admindb & "..SOFTCONTROL SET CTLTEXT = ''"
            StrSql += " WHERE CTLID = 'SYNC-LOCK-SEND'"
            _Cmd = New OleDbCommand(StrSql, _CnAdmin)
            _Cmd.ExecuteNonQuery()

            _CnWebLocal.Close()
            _CnAdmin.Close()
            _Cn.Close()
        Catch ex As Exception
            If ex.Message.Contains("was deadlocked on lock") = True Then
                'MsgBox("")
            End If
            If _Tran IsNot Nothing Then
                If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
            End If
            If _TranWeb IsNot Nothing Then
                If _TranWeb.Connection IsNot Nothing Then _TranWeb.Rollback()
            End If
            My.Settings.Er_Text = ex.Message
            If My.Settings.Er_OldText <> My.Settings.Er_Text Then
                Try
                    My.Settings.Er_Created = Date.Now
                    My.Settings.Er_OldText = ex.Message
                    _ObjStatus.AddStatus("Sending Error Report")
                    SendMail.Send("smtp.gmail.com", 587, _SyncErrSendId, "Sync Error From_" & _Admindb & "_" & _FromId, ex.Message, "", "akshayagoldhelpline@gmail.com", "giritech@123", "Giritech Support Team", , False)
                    My.Settings.Save()
                Catch ex1 As Exception

                End Try
            End If
            SetStatus(ex.Message)
            _ObjStatus.AddStatus(ex.Message)
            _ObjStatus.ShowDialog()
            'KillApp()
        End Try
        _ObjStatus.AddStatus("Upload Complete")
    End Sub
    Private Enum InsertMode
        Send = 0
        Receive = 1
    End Enum
    Private Sub funcListTagTables()
        ListTransferTables.Add("ITEMTAG")
        ListTransferTables.Add("TITEMTAG")
        ListTransferTables.Add("PITEMTAG")
        ListTransferTables.Add("CITEMTAG")
        ListTransferTables.Add("CITEMTAGMETAL")
        ListTransferTables.Add("CITEMTAGMISCCHAR")
        ListTransferTables.Add("CITEMTAGSTONE")
        ListTransferTables.Add("PITEMTAGSTONE")
        ListTransferTables.Add("CPURITEMTAG")
        ListTransferTables.Add("CPURITEMTAGMETAL")
        ListTransferTables.Add("CPURITEMTAGMISCCHAR")
        ListTransferTables.Add("CPURITEMTAGSTONE")
        ListTransferTables.Add("ITEMNONTAG")
        ListTransferTables.Add("ITEMNONTAGSTKCHK")
        ListTransferTables.Add("ITEMNONTAGSTONE")
        ListTransferTables.Add("ITEMTAGMETAL")
        ListTransferTables.Add("ITEMTAGMISCCHAR")
        ListTransferTables.Add("ITEMTAGSTONE")
        ListTransferTables.Add("PURITEMTAG")
        ListTransferTables.Add("PURITEMTAGMETAL")
        ListTransferTables.Add("PURITEMTAGMISCCHAR")
        ListTransferTables.Add("PURITEMTAGSTONE")
        ListTransferTables.Add("TITEMNONTAG")
        ListTransferTables.Add("TITEMTAGMETAL")
        ListTransferTables.Add("TITEMNONTAGSTONE")
        ListTransferTables.Add("TITEMTAGMISCCHAR")
        ListTransferTables.Add("TITEMTAGSTONE")
        ListTransferTables.Add("TPURITEMTAG")
        ListTransferTables.Add("TPURITEMTAGMETAL")
        ListTransferTables.Add("TPURITEMTAGMISCCHAR")
        ListTransferTables.Add("TPURITEMTAGSTONE")
        ListTransferTables.Add("CTRANSFER")
        ListTransferTables.Add("ITEMLOT")
        ListTransferTables.Add("OUTSTANDING")
        ListTransferTables.Add("ORMAST")
        ListTransferTables.Add("ORIRDETAIL")
        ListTransferTables.Add("ORSAMPLE")
        ListTransferTables.Add("ORSTONE")
        ListTransferTables.Add("SP_CCTRANSFER")
        ListTransferTables.Add("CUSTOMERINFO")
        ListTransferTables.Add("ITEMDETAIL")
    End Sub
    Private Function WebInsertIntoDb(ByVal InsMode As InsertMode, ByVal Row As DataRow, ByVal InsTableName As String, ByVal Cn As OleDbConnection, ByVal Tran As OleDbTransaction, ByVal ImageDirPath As String, Optional ByVal UpdFile As String = "") As String
        Dim mqry As String = ""
        Dim msqltext As String = ""
        Dim mfromid As String = ""
        Dim mtoid As String = ""
        Dim mupdfile As String = UpdFile
        Dim uid As Long = 0

        Try
            Dim ErrMsg As String = ""
            mfromid = Row!fromid : mtoid = Row!toid : msqltext = Row!sqltext.ToString
            Dim R_Dbname As String
            If SYNCCHK_TRANDB Then
                R_Dbname = GetSqlValue(Cn, "SELECT TOP 1 DBNAME FROM " & _Admindb & "..DBMASTER ORDER BY ENDDATE DESC", , , Tran)
                For Each T As String In ListTransferTables
                    If msqltext.Contains(R_Dbname & ".." & T) Then
                        msqltext = ReplaceQryStrNew(msqltext, R_Dbname & ".." & T, Mid(R_Dbname, 1, 3) & "ADMINDB.." & T)
                    End If
                Next
            End If
            'If maxUid = 0 Or maxUid < Row!uid Then maxUid = Row!Uid
            Dim Qry As String = ""

            Qry = vbCrLf + " INSERT INTO " & InsTableName & ""
            Qry += vbCrLf + " ("
            Qry += vbCrLf + " FROMID"
            Qry += vbCrLf + " ,TOID"
            Qry += vbCrLf + " ,SQLTEXT"
            Qry += vbCrLf + " ,STATUS"
            If Row!TAGIMAGE.ToString <> "" Then Qry += vbCrLf + " ,TAGIMAGE"
            If Row!TAGIMAGE.ToString <> "" Then Qry += vbCrLf + " ,TAGIMAGENAME"
            Qry += vbCrLf + " ,UPDFILE,IMAGE_CTRLID"

            Qry += vbCrLf + " )"
            Qry += " VALUES"
            Qry += " (?,?,?,?"
            If Row!TAGIMAGE.ToString <> "" Then Qry += ",?,?"
            Qry += " ,?,?)"
            _Cmd = New OleDbCommand(Qry, _CnAdmin, Tran)
            _Cmd.Parameters.AddWithValue("@FROMID", Row!FROMID.ToString)
            _Cmd.Parameters.AddWithValue("@TOID", Row!TOID.ToString)
            _Cmd.Parameters.AddWithValue("@SQLTEXT", msqltext)
            _Cmd.Parameters.AddWithValue("@STATUS", "N")
            '_Cmd.Parameters.AddWithValue("@STATUS", IIf(ErrMsg = "", "Y", "N"))
            If Row!TAGIMAGE.ToString <> "" Then
                _Cmd.Parameters.AddWithValue("@TAGIMAGE", Row!TAGIMAGE)
                _Cmd.Parameters.AddWithValue("@TAGIMAGENAME", Row!TAGIMAGENAME.ToString)
            End If
            _Cmd.Parameters.AddWithValue("@UPDFILE", IIf(UpdFile <> "", UpdFile, Row!UPDFILE.ToString))
            _Cmd.Parameters.AddWithValue("@IMAGE_CTRLID", Row!IMAGE_CTRLID.ToString)
            _Cmd.ExecuteNonQuery()
            PassUids += "," & Row!UID
            If InsMode = InsertMode.Receive Then
                If Row!TAGIMAGE.ToString <> "" Then
                    If Not IO.Directory.Exists(ImageDirPath) And Row!IMAGE_CTRLID.ToString.Contains("PICPATH") Then
                        Throw New Exception(ImageDirPath & " not found. Please make appropriate path")
                    ElseIf Not IO.Directory.Exists(ImageDirPath) And Row!IMAGE_CTRLID.ToString.Contains(":") Then
                        Return ""
                    End If
                    Dim myByte() As Byte = Row!TAGIMAGE
                    Dim stream As System.IO.MemoryStream
                    Dim img As Image
                    stream = New System.IO.MemoryStream()
                    stream.Write(myByte, 0, myByte.Length)
                    img = Image.FromStream(stream, True)
                    img.Save(ImageDirPath & Row!TAGIMAGENAME.ToString)
                    stream.Close()
                End If
            End If
            Return ""
        Catch ex As Exception
            FailUids += "," & Row!UID
            Return ex.Message
        End Try
    End Function

    Private Sub WebReceive()
        Dim Iskillapp As Boolean = True

        SetStatus("Downloading..")
        _ObjStatus.AddStatus("Initiating Download")
        Dim R_CostId As String = ""
        Dim R_DtReceiveInfo As New DataTable
        Dim R_OldCompId As String = ""
        Dim R_NewCompId As String = ""
        Dim R_ErrorState As String = ""
        Dim R_Image_CtrlId As String = ""
        Dim R_ImageDir As String = ""
CounterStarts:
        Dim Counter As Integer = 0
        Dim LockCounter As Integer = 0
        Dim Row As DataRow = Nothing
        Dim R_fromid As String
        Dim R_toid As String
        StrSql = vbCrLf + "  IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE XTYPE = 'TR' AND NAME = 'TRIG_RECEIVESYNC')>0"
        StrSql += vbCrLf + "  BEGIN"
        StrSql += vbCrLf + "  DROP TRIGGER TRIG_RECEIVESYNC"
        StrSql += vbCrLf + "  END"
        _Cmd = New OleDbCommand(StrSql, _CnAdmin)
        _Cmd.ExecuteNonQuery()

        ' CHITTRANPOST_TRG(False)

        StrSql = " SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SYNCCHK_TRANDB'"
        If GMethods.GetSqlValue(_CnAdmin, StrSql) = "Y" Then SYNCCHK_TRANDB = True Else SYNCCHK_TRANDB = False
        Dim Uids As Long
        StrSql = " SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'COSTID'"
        R_CostId = GetSqlValue(_CnAdmin, StrSql)
        Try
            If _DetStatus Then SetStatus("Get web Connection..\\" & _WebDbServerName & "\" & _WebDbName)
            _ObjStatus.AddStatus("Get Web Connetion..")

            _ObjStatus.AddStatus(GetWebConnectionString)
            _CnWeb = New OleDbConnection(GetWebConnectionString)
            _CnWeb.Open()

            While 1 = 1
                SetStatus("Downloading..")
                Counter += 1
                _ObjStatus.AddStatus("Get Web Data..")
                StrSql = " SELECT TOP 1 * FROM " & _WebDbTblPrefix & "_SYNCTABLE WHERE TOID = '" & R_CostId & "' ORDER BY FROMID,UID"
                R_DtReceiveInfo = New DataTable
                _Da = New OleDbDataAdapter(StrSql, _CnWeb)
                _Da.Fill(R_DtReceiveInfo)
                If Not R_DtReceiveInfo.Rows.Count > 0 Then
                    Exit While
                End If
                Row = R_DtReceiveInfo.Rows(0)
                Uids = Val(Row.Item("UID").ToString)
                Dim ZipFilePath As String = Nothing
                Dim XmlFilePath As String = Nothing
                Dim myByte() As Byte = Row.Item("CONTENT")
                ZipFilePath = IO.Path.GetTempPath & Row.Item("UPDFILE").ToString & ".ZIP"
                XmlFilePath = IO.Path.GetTempPath & Row.Item("UPDFILE").ToString & ".XML"
                If _DetStatus Then SetStatus("Check & Set file path ..\\" & ZipFilePath)
                If _DetStatus Then _ObjStatus.AddStatus("Checking and setting file path..")
                If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
                If IO.File.Exists(XmlFilePath) Then IO.File.Delete(XmlFilePath)
                If _DetStatus Then SetStatus("Reading file content ..\\" & ZipFilePath & " Size :" & myByte.Length.ToString)
                If _DetStatus Then _ObjStatus.AddStatus("Reading file content..")
                Dim fileStr As New IO.FileStream(ZipFilePath, IO.FileMode.Create, IO.FileAccess.ReadWrite)
                fileStr.Write(myByte, 0, myByte.Length)
                fileStr.Close()
                If _DetStatus Then SetStatus("converting file ..\\" & ZipFilePath & " -> " & XmlFilePath)
                If _DetStatus Then _ObjStatus.AddStatus("Getting & converting file content..")
                Dim objZip As New Zipper
                If Not objZip.UnZip(ZipFilePath, IO.Path.GetTempPath) Then
                    If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
                Else
                    If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
                End If
                If _DetStatus Then SetStatus("Read the xml data.." & XmlFilePath)
                If _DetStatus Then _ObjStatus.AddStatus("Reading data from file..")

                Dim DsReceive As New DataSet
                DsReceive.ReadXml(XmlFilePath, XmlReadMode.ReadSchema)
                DsReceive.AcceptChanges()

                If _Sync_Via_R Then
                    R_NewCompId = GMethods.GetSqlValue(_CnAdmin, "SELECT COMPID FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & R_CostId & "'")
                Else
                    R_NewCompId = GMethods.GetSqlValue(_CnAdmin, "SELECT COMPID FROM " & IIf(_VbDbPrefix = "", _CompanyId, _VbDbPrefix) & "SAVINGS..SYNCCOSTCENTRE WHERE COSTID = '" & R_CostId & "'")
                End If
                If _DetStatus Then SetStatus("Set Replace CompanyId :" & R_NewCompId)
                _ObjStatus.AddStatus("Set Replace CompanyId :" & R_NewCompId)


                R_Image_CtrlId = Nothing
                R_ImageDir = Nothing
                If _Sync_Via_R Then
                    R_OldCompId = GMethods.GetSqlValue(_CnAdmin, "SELECT COMPID FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & Row.Item("FROMID").ToString & "'")
                Else
                    R_OldCompId = GMethods.GetSqlValue(_CnAdmin, "SELECT COMPID FROM " & IIf(_VbDbPrefix = "", _CompanyId, _VbDbPrefix) & "SAVINGS..SYNCCOSTCENTRE WHERE COSTID = '" & Row.Item("FROMID").ToString & "'")
                End If
                If _DetStatus Then SetStatus("Set Old CompanyId :" & R_OldCompId)
                _ObjStatus.AddStatus("Set Old CompanyId :" & R_OldCompId)

                LockCounter = 0


                _Tran = Nothing
                _Tran = _CnAdmin.BeginTransaction(IsolationLevel.Serializable)
                _TranWeb = Nothing
                _TranWeb = _CnWeb.BeginTransaction(IsolationLevel.Serializable)
                PassUids = "" : FailUids = ""
                For Each RowRec As DataRow In DsReceive.Tables(0).Rows
                    _ObjStatus.AddStatus("Web SqlText :" & RowRec.Item("SQLTEXT").ToString)
                    RowRec.Item("SQLTEXT") = ReplaceQryStr(RowRec.Item("SQLTEXT").ToString, R_OldCompId, R_NewCompId)
                    If _DetStatus Then SetStatus("Inserting SqlText ")
                    _ObjStatus.AddStatus("Inserted SqlText :" & RowRec.Item("SQLTEXT").ToString)
                    If RowRec!TAGIMAGE.ToString <> "" Then
                        If RowRec.Item("IMAGE_CTRLID").ToString = "" Then
                            RowRec.Item("IMAGE_CTRLID") = "PICPATH"
                        End If
                    End If
                    If Not IS_IMAGE_TRF Then
                        Dim myByt() As Byte = Nothing
                        RowRec.Item("TAGIMAGE") = myByt
                        RowRec.Item("IMAGE_CTRLID") = ""
                    End If
                    '  remove above lines to enable image downloading
                    If RowRec.Item("IMAGE_CTRLID").ToString <> "" Then
                        StrSql = " SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = '" & RowRec.Item("IMAGE_CTRLID").ToString & "'"
                        R_ImageDir = GMethods.GetSqlValue(_CnAdmin, StrSql, , , _Tran)
                        If Not R_ImageDir.EndsWith("\") And R_ImageDir <> "" Then R_ImageDir += "\"
                    End If
                    R_fromid = RowRec!fromid
                    R_toid = RowRec!toid
                    R_ErrorState = ""

                    R_ErrorState = WebInsertIntoDb(InsertMode.Receive, RowRec, _Syncdb & "..RECEIVESYNC", _CnAdmin, _Tran, R_ImageDir, Row.Item("UPDFILE").ToString)
                    If R_ErrorState <> "" Then
                        If Not R_ErrorState.Contains("Violation of PRIMARY KEY constraint") Then Throw New Exception(R_ErrorState)
                        PassUids += "," & RowRec!UID
                    End If

                Next



                If PassUids.Length > 2 Then PassUids = Mid(PassUids, 2).Trim
                Dim Passfailstr As String = ""
                If PassUids.Length > 1 Then Passfailstr = " UID IN (" & PassUids & ")"
                If PassUids.Length > 2 Then SendAcknowledgePassFail(R_fromid, R_toid, Passfailstr, "Y")
                Passfailstr = ""
                If FailUids.Length > 2 Then FailUids = Mid(FailUids, 2).Trim
                If FailUids.Length > 1 Then Passfailstr = " UID IN (" & FailUids & ")"
                If FailUids.Length > 2 Then SendAcknowledgePassFail(R_fromid, R_toid, Passfailstr, "N")
                StrSql = " DELETE FROM " & _WebDbTblPrefix & "_SYNCTABLE WHERE  UID = " & Val(Row.Item("UID").ToString) & ""
                _Cmd = New OleDbCommand(StrSql, _CnWeb, _TranWeb)
                _Cmd.ExecuteNonQuery()
                _TranWeb.Commit()
                If _Tran.Connection IsNot Nothing Then _Tran.Commit()
                _Tran = Nothing
                _TranWeb = Nothing

                If Counter >= 5000 Then
                    Exit While
                End If

            End While
        Catch ex As Exception
            If _Tran IsNot Nothing Then
                If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
            End If
            If _TranWeb IsNot Nothing Then
                If _TranWeb.Connection IsNot Nothing Then _TranWeb.Rollback()
            End If

            Dim stackTraceW = New System.Diagnostics.StackTrace(ex)
            Dim stackFrameW = stackTraceW.GetFrame(stackTraceW.FrameCount - 1)
            Dim lineNumberW = stackFrameW.GetFileLineNumber() & " Unique Id:" & Uids
            'SetStatus(ex.Message + vbCrLf + " LineNumber : " + lineNumberW.ToString + vbCrLf + ex.StackTrace)
            '_ObjStatus.AddStatus(ex.Message + vbCrLf + " LineNumber :  " + lineNumberW.ToString + " #" + vbCrLf + ex.StackTrace)
            SetStatus("ConStr")
            _ObjStatus.AddStatus("ConStr")
            Thread.Sleep(10000)
        End Try


        Try
            _ObjStatus = New Status
            minUid = 0 : maxUid = 0
            SetStatus("Executing data..")
            _ObjStatus.AddStatus("Executing data in " & _CompanyId & "_" & R_CostId)
            NotifyIcon1.Text = _Status_msg & vbCrLf & "Executing Data @ " & _CompanyId & "_" & R_CostId
            NotifyIcon1.BalloonTipText = "Executing Data @ " & _CompanyId & "_" & R_CostId
            Dim R_Dbname As String
            R_Dbname = GetSqlValue(_CnAdmin, "SELECT TOP 1 DBNAME FROM " & _Admindb & "..DBMASTER ORDER BY ENDDATE DESC")
next1000:
            Dim mUpdfile As String = Nothing

            StrSql = " SELECT top 1000 * FROM " & _Syncdb & "..RECEIVESYNC WHERE TOID = '" & R_CostId & "' AND STATUS = 'N' AND TAGIMAGE IS  NULL ORDER BY UID"
            Dim R_DtReceive As New DataTable
            _Da = New OleDbDataAdapter(StrSql, _CnAdmin)
            _Da.Fill(R_DtReceive)
            If R_DtReceive.Rows.Count > 0 Then

                For II As Integer = 0 To R_DtReceive.Rows.Count - 1
                    Row = R_DtReceive.Rows(II)
                    If Row!TAGIMAGE.ToString <> "" Then Continue For
                    Uids = Row!UID.ToString
                    Try
                        If SYNCCHK_TRANDB Then
                            For Each T As String In ListTransferTables
                                If Row!SQLTEXT.ToString.Contains(R_Dbname & ".." & T) Then
                                    Row!SQLTEXT = ReplaceQryStrNew(Row!SQLTEXT.ToString, R_Dbname & ".." & T, Mid(R_Dbname, 1, 3) & "ADMINDB.." & T)
                                End If
                            Next
                        End If
                        'If Row!sqltext.ToString.Contains("@@@@@(") Then
                        '    Row!SQLTEXT = ReplaceqryPwdstr(Row!SQLTEXT.ToString)
                        'End If
                        Dim _Sync As Boolean = True
                        If _Sync_Rec_SyncMast Then
                            If dtReceiveTbl.Rows.Count > 0 Then
                                For cnt As Integer = 0 To dtReceiveTbl.Rows.Count - 1
                                    Dim Tbl As String = dtReceiveTbl.Rows(cnt).Item("TABLENAME").ToString
                                    If Row!SQLTEXT.ToString.Contains(R_Dbname & ".." & Tbl) Then
                                        _Sync = False
                                        Exit For
                                    End If
                                    If Row!SQLTEXT.ToString.Contains(_Admindb & ".." & Tbl) Then
                                        _Sync = False
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                        _Tran = Nothing
                        _Tran = _CnAdmin.BeginTransaction(IsolationLevel.Serializable)
                        If Row!SQLTEXT.ToString <> "" And _Sync Then
                            _Cmd = New OleDbCommand(Row!SQLTEXT.ToString, _CnAdmin, _Tran)
                            _Cmd.ExecuteNonQuery()
                        End If
                        StrSql = "UPDATE  " & _Syncdb & "..RECEIVESYNC SET STATUS= 'Y' WHERE UID= " & Val(Uids)
                        _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran)
                        _Cmd.ExecuteNonQuery()
                        If _Tran.Connection IsNot Nothing Then _Tran.Commit()
                        R_fromid = Row!fromid
                        R_toid = Row!toid
                        mUpdfile = Row!updfile
                    Catch ex As Exception
                        Dim ErrMsg As String = ex.Message
                        If ErrMsg.Contains("Violation of PRIMARY KEY constraint") Then
                            StrSql = "UPDATE  " & _Syncdb & "..RECEIVESYNC SET STATUS= 'Y' WHERE UID= " & Row!UID
                            _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran)
                            _Cmd.ExecuteNonQuery()
                            If _Tran.Connection IsNot Nothing Then _Tran.Commit()
                        ElseIf ErrMsg.Contains("Violation of UNIQUE KEY constraint") Then
                            StrSql = "UPDATE  " & _Syncdb & "..RECEIVESYNC SET STATUS= 'Y' WHERE UID= " & Row!UID
                            _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran)
                            _Cmd.ExecuteNonQuery()
                            If _Tran.Connection IsNot Nothing Then _Tran.Commit()
                            mUpdfile = ""
                        Else
                            If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
                            mUpdfile = ""
                            Iskillapp = False
                            SetStatus(ex.Message + vbCrLf + ex.StackTrace)
                            _ObjStatus.AddStatus(ex.Message + vbCrLf + ex.StackTrace)
                            Throw New Exception(ErrMsg)
                            Exit Sub
                        End If

                    End Try
                Next
                GoTo next1000
            End If



next10000:
            StrSql = " SELECT top 1000 * FROM " & _Syncdb & "..RECEIVESYNC WHERE TOID = '" & R_CostId & "' AND STATUS = 'N' AND TAGIMAGE IS NOT NULL ORDER BY UID"
            Dim R_DtReceiveimg As New DataTable
            _Da = New OleDbDataAdapter(StrSql, _CnAdmin)
            _Da.Fill(R_DtReceiveimg)
            If R_DtReceiveimg.Rows.Count > 0 Then
                Dim RowImg As DataRow = Nothing
                For II As Integer = 0 To R_DtReceiveimg.Rows.Count - 1
                    RowImg = R_DtReceiveimg.Rows(II)
                    Uids = R_DtReceiveimg.Rows(II).Item("UID").ToString
                    Try
                        If RowImg!IMAGE_CTRLID.ToString <> "PICPATH" Then
                            Dim Image_ctrid As String = RowImg!IMAGE_CTRLID.ToString
                            Dim Img_ctr() As String = Image_ctrid.Split(":")
                            If Img_ctr.Length > 4 Then
                                StrSql = "UPDATE " & Mid(_Syncdb, 1, 3) & Mid(Img_ctr(0), 4, 20) & ".." & Img_ctr(1) & " SET " & Img_ctr(2) & " = ? "
                                StrSql += " WHERE  " & Img_ctr(3) & " = '" & Img_ctr(4) & "'"
                                _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran)
                                Dim Img As String = "@" & Img_ctr(2)
                                _Cmd.Parameters.AddWithValue(Img, RowImg!TAGIMAGE)
                                _Cmd.ExecuteNonQuery()
                            End If
                        End If
                        _Tran = Nothing
                        _Tran = _CnAdmin.BeginTransaction(IsolationLevel.Serializable)
                        StrSql = "DELETE " & _Syncdb & "..RECEIVESYNC WHERE UID= " & Val(Uids)
                        _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran)
                        _Cmd.ExecuteNonQuery()
                        If _Tran.Connection IsNot Nothing Then _Tran.Commit()

                    Catch ex As Exception
                        Dim ErrMsg As String = ex.Message
                        If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
                        mUpdfile = ""
                        Iskillapp = False
                        SetStatus(ex.Message + vbCrLf + ex.StackTrace)
                        _ObjStatus.AddStatus(ex.Message + vbCrLf + ex.StackTrace)
                        Throw New Exception(ErrMsg)
                        Exit Sub
                    End Try
                Next
                GoTo next10000
            End If


            _CnWeb.Close()
            _CnAdmin.Close()
            _Cn.Close()
        Catch ex As Exception
            If _Tran IsNot Nothing Then
                If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
            End If
            If _TranWeb IsNot Nothing Then
                If _TranWeb.Connection IsNot Nothing Then _TranWeb.Rollback()
            End If
            My.Settings.Er_Text = ex.Message
            If My.Settings.Er_OldText <> My.Settings.Er_Text Then
                Try
                    My.Settings.Er_Created = Date.Now
                    My.Settings.Er_OldText = ex.Message
                    My.Settings.Save()
                    _ObjStatus.AddStatus("Sending Error Report")
                    SendMail.Send("smtp.gmail.com", 587, _SyncErrSendId, "Sync Error From_" & _Admindb & "_" & _FromId, ex.Message, "", "akhayagoldhelpline@gmail.com", "giritech@123", "Giritech BugFix Team", , False)
                    My.Settings.Save()
                Catch ex1 As Exception

                End Try
            End If

            Dim stackTrace = New System.Diagnostics.StackTrace(ex)
            Dim stackFrame = stackTrace.GetFrame(stackTrace.FrameCount - 1)
            Dim lineNumber = stackFrame.GetFileLineNumber() & " Unique Id:" & Uids
            'SetStatus(ex.Message + vbCrLf + " LineNumber : " + lineNumber.ToString + vbCrLf + ex.StackTrace)
            '_ObjStatus.AddStatus(ex.Message + vbCrLf + " LineNumber :  " + lineNumber.ToString + " #" + vbCrLf + ex.StackTrace)

            SetStatus("")
            _ObjStatus.AddStatus("")

            _ObjStatus.ShowDialog()
            '        If Iskillapp Then KillApp()
            Exit Sub
        End Try
        _ObjStatus.AddStatus("Download Complete")
        'CHITTRANPOST_TRG(True)
    End Sub

    Private Sub WebReceiveLocal()
        Dim Iskillapp As Boolean = True

        SetStatus("Downloading..")
        _ObjStatus.AddStatus("Initiating Download")
        Dim R_CostId As String = ""
        Dim R_DtReceiveInfo As New DataTable
        Dim R_OldCompId As String = ""
        Dim R_NewCompId As String = ""
        Dim R_ErrorState As String = ""
        Dim R_Image_CtrlId As String = ""
        Dim R_ImageDir As String = ""
CounterStarts:
        Dim Counter As Integer = 0
        Dim LockCounter As Integer = 0
        Dim Row As DataRow = Nothing
        Dim R_fromid As String
        Dim R_toid As String
        If _CnAdmin.State = ConnectionState.Closed Then _CnAdmin.Open()

        StrSql = " SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'SYNCCHK_TRANDB'"
        If GMethods.GetSqlValue(_CnAdmin, StrSql) = "Y" Then SYNCCHK_TRANDB = True Else SYNCCHK_TRANDB = False
        Dim Uids As Long
        StrSql = " SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = 'COSTID'"
        R_CostId = GetSqlValue(_CnAdmin, StrSql)
        Try
            StrSql = "SELECT COSTID FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE (ISNULL(WEBDBNAME,'')='' OR ISNULL(WEBDBNAME,'')<>'')"
            StrSql += " AND COSTID <> '" & R_CostId & "'"
            StrSql += " AND ISNULL(MANUAL,'') <> 'Y'"
            Dim S_DtToCostCenter As New DataTable
            _Da = New OleDbDataAdapter(StrSql, _CnAdmin)
            _Da.Fill(S_DtToCostCenter)
            If S_DtToCostCenter.Rows.Count = 0 Then Exit Sub
            For Each Ro As DataRow In S_DtToCostCenter.Rows
                _ObjStatus.AddStatus("Get Web Connetion..")
                _CnWebLocal = New OleDbConnection(GetWebLocalConnectionString(Ro.Item("COSTID").ToString))
                _CnWebLocal.Open()
                While 1 = 1
                    SetStatus("Downloading..")
                    Counter += 1
                    _ObjStatus.AddStatus("Get Web Data..")
                    StrSql = " SELECT TOP 1 * FROM " & _LocalTblPrefix & "_SYNCTABLE WHERE TOID = '" & R_CostId & "' ORDER BY FROMID,UID"
                    R_DtReceiveInfo = New DataTable
                    _Da = New OleDbDataAdapter(StrSql, _CnWebLocal)
                    _Da.Fill(R_DtReceiveInfo)
                    If Not R_DtReceiveInfo.Rows.Count > 0 Then
                        Exit While
                    End If
                    Row = R_DtReceiveInfo.Rows(0)
                    Uids = Val(Row.Item("UID").ToString)
                    Dim ZipFilePath As String = Nothing
                    Dim XmlFilePath As String = Nothing
                    Dim myByte() As Byte = Row.Item("CONTENT")
                    ZipFilePath = IO.Path.GetTempPath & Row.Item("UPDFILE").ToString & ".ZIP"
                    XmlFilePath = IO.Path.GetTempPath & Row.Item("UPDFILE").ToString & ".XML"
                    If _DetStatus Then SetStatus("Check & Set file path ..\\" & ZipFilePath)
                    If _DetStatus Then _ObjStatus.AddStatus("Checking and setting file path..")
                    If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
                    If IO.File.Exists(XmlFilePath) Then IO.File.Delete(XmlFilePath)
                    If _DetStatus Then SetStatus("Reading file content ..\\" & ZipFilePath & " Size :" & myByte.Length.ToString)
                    If _DetStatus Then _ObjStatus.AddStatus("Reading file content..")
                    Dim fileStr As New IO.FileStream(ZipFilePath, IO.FileMode.Create, IO.FileAccess.ReadWrite)
                    fileStr.Write(myByte, 0, myByte.Length)
                    fileStr.Close()
                    If _DetStatus Then SetStatus("converting file ..\\" & ZipFilePath & " -> " & XmlFilePath)
                    If _DetStatus Then _ObjStatus.AddStatus("Getting & converting file content..")
                    Dim objZip As New Zipper
                    If Not objZip.UnZip(ZipFilePath, IO.Path.GetTempPath) Then
                        If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
                    Else
                        If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
                    End If
                    If _DetStatus Then SetStatus("Read the xml data.." & XmlFilePath)
                    If _DetStatus Then _ObjStatus.AddStatus("Reading data from file..")

                    Dim DsReceive As New DataSet
                    DsReceive.ReadXml(XmlFilePath, XmlReadMode.ReadSchema)
                    DsReceive.AcceptChanges()

                    If _Sync_Via_R Then
                        R_NewCompId = GMethods.GetSqlValue(_CnAdmin, "SELECT COMPID FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & R_CostId & "'")
                    Else
                        R_NewCompId = GMethods.GetSqlValue(_CnAdmin, "SELECT COMPID FROM " & IIf(_VbDbPrefix = "", _CompanyId, _VbDbPrefix) & "SAVINGS..SYNCCOSTCENTRE WHERE COSTID = '" & R_CostId & "'")
                    End If
                    If _DetStatus Then SetStatus("Set Replace CompanyId :" & R_NewCompId)
                    _ObjStatus.AddStatus("Set Replace CompanyId :" & R_NewCompId)


                    R_Image_CtrlId = Nothing
                    R_ImageDir = Nothing
                    If _Sync_Via_R Then
                        R_OldCompId = GMethods.GetSqlValue(_CnAdmin, "SELECT COMPID FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & Row.Item("FROMID").ToString & "'")
                    Else
                        R_OldCompId = GMethods.GetSqlValue(_CnAdmin, "SELECT COMPID FROM " & IIf(_VbDbPrefix = "", _CompanyId, _VbDbPrefix) & "SAVINGS..SYNCCOSTCENTRE WHERE COSTID = '" & Row.Item("FROMID").ToString & "'")
                    End If
                    If _DetStatus Then SetStatus("Set Old CompanyId :" & R_OldCompId)
                    _ObjStatus.AddStatus("Set Old CompanyId :" & R_OldCompId)

                    LockCounter = 0
                    _Tran = Nothing
                    _Tran = _CnAdmin.BeginTransaction
                    _TranWeb = Nothing
                    _TranWeb = _CnWebLocal.BeginTransaction
                    PassUids = "" : FailUids = ""
                    For Each RowRec As DataRow In DsReceive.Tables(0).Rows
                        _ObjStatus.AddStatus("Web SqlText :" & RowRec.Item("SQLTEXT").ToString)
                        RowRec.Item("SQLTEXT") = ReplaceQryStr(RowRec.Item("SQLTEXT").ToString, R_OldCompId, R_NewCompId)
                        If _DetStatus Then SetStatus("Inserting SqlText ")
                        _ObjStatus.AddStatus("Inserted SqlText :" & RowRec.Item("SQLTEXT").ToString)
                        If RowRec!TAGIMAGE.ToString <> "" Then
                            If RowRec.Item("IMAGE_CTRLID").ToString = "" Then
                                RowRec.Item("IMAGE_CTRLID") = "PICPATH"
                            End If
                        End If
                        If Not IS_IMAGE_TRF Then
                            Dim myByt() As Byte = Nothing
                            RowRec.Item("TAGIMAGE") = myByt
                            RowRec.Item("IMAGE_CTRLID") = ""
                        End If
                        '  remove above lines to enable image downloading
                        If RowRec.Item("IMAGE_CTRLID").ToString <> "" Then
                            StrSql = " SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = '" & RowRec.Item("IMAGE_CTRLID").ToString & "'"
                            R_ImageDir = GMethods.GetSqlValue(_CnAdmin, StrSql, , , _Tran)
                            If Not R_ImageDir.EndsWith("\") And R_ImageDir <> "" Then R_ImageDir += "\"
                        End If
                        R_fromid = RowRec!fromid
                        R_toid = RowRec!toid
                        R_ErrorState = ""

                        R_ErrorState = WebInsertIntoDb(InsertMode.Receive, RowRec, _Syncdb & "..RECEIVESYNC", _CnAdmin, _Tran, R_ImageDir, Row.Item("UPDFILE").ToString)
                        If R_ErrorState <> "" Then
                            If Not R_ErrorState.Contains("Violation of PRIMARY KEY constraint") Then Throw New Exception(R_ErrorState)
                            PassUids += "," & RowRec!UID
                        End If

                    Next



                    If PassUids.Length > 2 Then PassUids = Mid(PassUids, 2).Trim
                    Dim Passfailstr As String = ""
                    If PassUids.Length > 1 Then Passfailstr = " UID IN (" & PassUids & ")"
                    If PassUids.Length > 2 Then SendAcknowledgePassFail(R_fromid, R_toid, Passfailstr, "Y")
                    Passfailstr = ""
                    If FailUids.Length > 2 Then FailUids = Mid(FailUids, 2).Trim
                    If FailUids.Length > 1 Then Passfailstr = " UID IN (" & FailUids & ")"
                    If FailUids.Length > 2 Then SendAcknowledgePassFail(R_fromid, R_toid, Passfailstr, "N")
                    StrSql = " DELETE FROM " & _LocalTblPrefix & "_SYNCTABLE WHERE  UID = " & Val(Row.Item("UID").ToString) & ""
                    _Cmd = New OleDbCommand(StrSql, _CnWebLocal, _TranWeb)
                    _Cmd.ExecuteNonQuery()
                    _TranWeb.Commit()
                    If _Tran.Connection IsNot Nothing Then _Tran.Commit()
                    _Tran = Nothing
                    _TranWeb = Nothing

                    If Counter >= 5000 Then
                        Exit While
                    End If

                End While
            Next
        Catch ex As Exception
            If _Tran IsNot Nothing Then
                If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
            End If
            If _TranWeb IsNot Nothing Then
                If _TranWeb.Connection IsNot Nothing Then _TranWeb.Rollback()
            End If

            Dim stackTraceW = New System.Diagnostics.StackTrace(ex)
            Dim stackFrameW = stackTraceW.GetFrame(stackTraceW.FrameCount - 1)
            Dim lineNumberW = stackFrameW.GetFileLineNumber() & " Unique Id:" & Uids
            'SetStatus(ex.Message + vbCrLf + " LineNumber : " + lineNumberW.ToString + vbCrLf + ex.StackTrace)
            '_ObjStatus.AddStatus(ex.Message + vbCrLf + " LineNumber :  " + lineNumberW.ToString + " #" + vbCrLf + ex.StackTrace)
            SetStatus("")
            _ObjStatus.AddStatus("")
            Thread.Sleep(10000)
        End Try


        Try
            _ObjStatus = New Status
            minUid = 0 : maxUid = 0
            SetStatus("Executing data..")
            _ObjStatus.AddStatus("Executing data in " & _CompanyId & "_" & R_CostId)
            NotifyIcon1.Text = _Status_msg & vbCrLf & "Executing Data @ " & _CompanyId & "_" & R_CostId
            NotifyIcon1.BalloonTipText = "Executing Data @ " & _CompanyId & "_" & R_CostId
            Dim R_Dbname As String
            R_Dbname = GetSqlValue(_CnAdmin, "SELECT TOP 1 DBNAME FROM " & _Admindb & "..DBMASTER ORDER BY ENDDATE DESC")
next1000:
            Dim mUpdfile As String = Nothing

            StrSql = " SELECT top 1000 * FROM " & _Syncdb & "..RECEIVESYNC WHERE TOID = '" & R_CostId & "' AND STATUS = 'N' AND TAGIMAGE IS  NULL ORDER BY UID"
            Dim R_DtReceive As New DataTable
            _Da = New OleDbDataAdapter(StrSql, _CnAdmin)
            _Da.Fill(R_DtReceive)
            If R_DtReceive.Rows.Count > 0 Then

                For II As Integer = 0 To R_DtReceive.Rows.Count - 1
                    Row = R_DtReceive.Rows(II)
                    If Row!TAGIMAGE.ToString <> "" Then Continue For
                    Uids = Row!UID.ToString
                    Try
                        If SYNCCHK_TRANDB Then
                            For Each T As String In ListTransferTables
                                If Row!SQLTEXT.ToString.Contains(R_Dbname & ".." & T) Then
                                    Row!SQLTEXT = ReplaceQryStrNew(Row!SQLTEXT.ToString, R_Dbname & ".." & T, Mid(R_Dbname, 1, 3) & "ADMINDB.." & T)
                                End If
                            Next
                        End If
                        'If Row!sqltext.ToString.Contains("@@@@@(") Then
                        '    Row!SQLTEXT = ReplaceqryPwdstr(Row!SQLTEXT.ToString)
                        'End If
                        Dim _Sync As Boolean = True
                        If _Sync_Rec_SyncMast Then
                            If dtReceiveTbl.Rows.Count > 0 Then
                                For cnt As Integer = 0 To dtReceiveTbl.Rows.Count - 1
                                    Dim Tbl As String = dtReceiveTbl.Rows(cnt).Item("TABLENAME").ToString
                                    If Row!SQLTEXT.ToString.Contains(R_Dbname & ".." & Tbl) Then
                                        _Sync = False
                                        Exit For
                                    End If
                                    If Row!SQLTEXT.ToString.Contains(_Admindb & ".." & Tbl) Then
                                        _Sync = False
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                        _Tran = Nothing
                        _Tran = _CnAdmin.BeginTransaction
                        If Row!SQLTEXT.ToString <> "" And _Sync Then
                            _Cmd = New OleDbCommand(Row!SQLTEXT.ToString, _CnAdmin, _Tran)
                            _Cmd.ExecuteNonQuery()
                        End If
                        StrSql = "UPDATE  " & _Syncdb & "..RECEIVESYNC SET STATUS= 'Y' WHERE UID= " & Val(Uids)
                        _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran)
                        _Cmd.ExecuteNonQuery()
                        If _Tran.Connection IsNot Nothing Then _Tran.Commit()
                        R_fromid = Row!fromid
                        R_toid = Row!toid
                        mUpdfile = Row!updfile
                    Catch ex As Exception
                        Dim ErrMsg As String = ex.Message
                        If ErrMsg.Contains("Violation of PRIMARY KEY constraint") Then
                            StrSql = "UPDATE  " & _Syncdb & "..RECEIVESYNC SET STATUS= 'Y' WHERE UID= " & Row!UID
                            _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran)
                            _Cmd.ExecuteNonQuery()
                            If _Tran.Connection IsNot Nothing Then _Tran.Commit()
                        ElseIf ErrMsg.Contains("Violation of UNIQUE KEY constraint") Then
                            StrSql = "UPDATE  " & _Syncdb & "..RECEIVESYNC SET STATUS= 'Y' WHERE UID= " & Row!UID
                            _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran)
                            _Cmd.ExecuteNonQuery()
                            If _Tran.Connection IsNot Nothing Then _Tran.Commit()
                            mUpdfile = ""
                        Else
                            If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
                            mUpdfile = ""
                            Iskillapp = False
                            SetStatus(ex.Message + vbCrLf + ex.StackTrace)
                            _ObjStatus.AddStatus(ex.Message + vbCrLf + ex.StackTrace)
                            Throw New Exception(ErrMsg)
                            Exit Sub
                        End If

                    End Try
                Next
                GoTo next1000
            End If



next10000:
            StrSql = " SELECT top 1000 * FROM " & _Syncdb & "..RECEIVESYNC WHERE TOID = '" & R_CostId & "' AND STATUS = 'N' AND TAGIMAGE IS NOT NULL ORDER BY UID"
            Dim R_DtReceiveimg As New DataTable
            _Da = New OleDbDataAdapter(StrSql, _CnAdmin)
            _Da.Fill(R_DtReceiveimg)
            If R_DtReceiveimg.Rows.Count > 0 Then
                Dim RowImg As DataRow = Nothing
                For II As Integer = 0 To R_DtReceiveimg.Rows.Count - 1
                    RowImg = R_DtReceiveimg.Rows(II)
                    Uids = R_DtReceiveimg.Rows(II).Item("UID").ToString
                    Try
                        If RowImg!IMAGE_CTRLID.ToString <> "PICPATH" Then
                            Dim Image_ctrid As String = RowImg!IMAGE_CTRLID.ToString
                            Dim Img_ctr() As String = Image_ctrid.Split(":")
                            If Img_ctr.Length > 4 Then
                                StrSql = "UPDATE " & Mid(_Syncdb, 1, 3) & Mid(Img_ctr(0), 4, 20) & ".." & Img_ctr(1) & " SET " & Img_ctr(2) & " = ? "
                                StrSql += " WHERE  " & Img_ctr(3) & " = '" & Img_ctr(4) & "'"
                                _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran)
                                Dim Img As String = "@" & Img_ctr(2)
                                _Cmd.Parameters.AddWithValue(Img, RowImg!TAGIMAGE)
                                _Cmd.ExecuteNonQuery()
                            End If
                        End If
                        _Tran = Nothing
                        _Tran = _CnAdmin.BeginTransaction
                        StrSql = "DELETE " & _Syncdb & "..RECEIVESYNC WHERE UID= " & Val(Uids)
                        _Cmd = New OleDbCommand(StrSql, _CnAdmin, _Tran)
                        _Cmd.ExecuteNonQuery()
                        If _Tran.Connection IsNot Nothing Then _Tran.Commit()

                    Catch ex As Exception
                        Dim ErrMsg As String = ex.Message
                        If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
                        mUpdfile = ""
                        Iskillapp = False
                        SetStatus(ex.Message + vbCrLf + ex.StackTrace)
                        _ObjStatus.AddStatus(ex.Message + vbCrLf + ex.StackTrace)
                        Throw New Exception(ErrMsg)
                        Exit Sub
                    End Try
                Next
                GoTo next10000
            End If


            _CnWebLocal.Close()
            _CnAdmin.Close()
            _Cn.Close()
        Catch ex As Exception
            If _Tran IsNot Nothing Then
                If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
            End If
            If _TranWeb IsNot Nothing Then
                If _TranWeb.Connection IsNot Nothing Then _TranWeb.Rollback()
            End If
            My.Settings.Er_Text = ex.Message
            If My.Settings.Er_OldText <> My.Settings.Er_Text Then
                Try
                    My.Settings.Er_Created = Date.Now
                    My.Settings.Er_OldText = ex.Message
                    My.Settings.Save()
                    _ObjStatus.AddStatus("Sending Error Report")
                    SendMail.Send("smtp.gmail.com", 587, _SyncErrSendId, "Sync Error From_" & _Admindb & "_" & _FromId, ex.Message, "", "akhayagoldhelpline@gmail.com", "giritech@123", "Giritech BugFix Team", , False)
                    My.Settings.Save()
                Catch ex1 As Exception

                End Try
            End If

            Dim stackTrace = New System.Diagnostics.StackTrace(ex)
            Dim stackFrame = stackTrace.GetFrame(stackTrace.FrameCount - 1)
            Dim lineNumber = stackFrame.GetFileLineNumber() & " Unique Id:" & Uids
            'SetStatus(ex.Message + vbCrLf + " LineNumber : " + lineNumber.ToString + vbCrLf + ex.StackTrace)
            '_ObjStatus.AddStatus(ex.Message + vbCrLf + " LineNumber :  " + lineNumber.ToString + " #" + vbCrLf + ex.StackTrace)
            SetStatus("")
            _ObjStatus.AddStatus("")
            _ObjStatus.ShowDialog()
            '        If Iskillapp Then KillApp()
            Exit Sub
        End Try
        _ObjStatus.AddStatus("Download Complete")
        'CHITTRANPOST_TRG(True)
    End Sub
    Function SendDataIntegrity(ByVal F_Costid As String, ByVal F_Updfile As String) As Boolean
        Dim S_DtSentInfo As New DataTable
        Dim Row As DataRow
        _ObjStatus = New Status
        minUid = 0 : maxUid = 0
        If Not IO.Directory.Exists(IO.Path.GetTempPath & "BACK\") Then IO.Directory.CreateDirectory(IO.Path.GetTempPath & "BACK\")
        SetStatus("Checking data integrity..")
        _ObjStatus.AddStatus("Checking data integrity in " & F_Costid)
        NotifyIcon1.Text = _Status_msg & vbCrLf & "Checking data integrity @ " & F_Costid
        NotifyIcon1.BalloonTipText = "Checking data integrity @ " & F_Costid

        StrSql = " SELECT * FROM " & _WebDbTblPrefix & "_SYNCTABLE WHERE FROMID = '" & F_Costid & "' AND UPDFILE = '" & F_Updfile & "' ORDER BY TOID,UID"
        S_DtSentInfo = New DataTable
        _Cmd = New OleDbCommand(StrSql, _CnWeb, _TranWeb)
        _Da = New OleDbDataAdapter(_Cmd)
        _Da.Fill(S_DtSentInfo)
        If Not S_DtSentInfo.Rows.Count > 0 Then
            Exit Function
        End If
        Dim ZipFilebackPath As String = IO.Path.GetTempPath & "BACK\"
        If Not IO.Directory.Exists(ZipFilebackPath) Then IO.Directory.CreateDirectory(ZipFilebackPath)
        For ii As Integer = 0 To S_DtSentInfo.Rows.Count - 1
            Row = S_DtSentInfo.Rows(ii)
            Dim ZipFilePath_bak As String = Nothing
            Dim XmlFilePath_bak As String = Nothing
            Dim XmlFilePath As String = Nothing
            Dim ZipFilePath As String = Nothing
            Dim myByte() As Byte = Row.Item("CONTENT")
            ZipFilePath_bak = IO.Path.GetTempPath & "BACK\" & Row.Item("UPDFILE").ToString & ".ZIP"
            XmlFilePath_bak = IO.Path.GetTempPath & "BACK\" & Row.Item("UPDFILE").ToString & ".XML"
            XmlFilePath = IO.Path.GetTempPath & Row.Item("UPDFILE").ToString & ".XML"
            ZipFilePath = IO.Path.GetTempPath & Row.Item("UPDFILE").ToString & ".ZIP"

            If IO.File.Exists(ZipFilePath_bak) Then IO.File.Delete(ZipFilePath_bak)
            If IO.File.Exists(XmlFilePath_bak) Then IO.File.Delete(XmlFilePath_bak)
            Dim fileStr As New IO.FileStream(ZipFilePath_bak, IO.FileMode.Create, IO.FileAccess.ReadWrite)
            fileStr.Write(myByte, 0, myByte.Length)
            fileStr.Close()
            Dim objZip As New Zipper
            If Not objZip.UnZip(ZipFilePath_bak, IO.Path.GetTempPath & "BACK\") Then
                If IO.File.Exists(ZipFilePath_bak) Then IO.File.Delete(ZipFilePath_bak)
            Else
                If IO.File.Exists(ZipFilePath_bak) Then IO.File.Delete(ZipFilePath_bak)
            End If


            If Not objZip.UnZip(ZipFilePath, IO.Path.GetTempPath) Then
                If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
            Else
                If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
            End If

            Dim info_back As New FileInfo(XmlFilePath_bak)
            Dim info_org As New FileInfo(XmlFilePath)
            If info_back.Length <> info_org.Length Then
                SetStatus("Sent file length mismatch :" & vbCrLf & XmlFilePath_bak & "(" & info_back.Length & ") vs " & XmlFilePath & "(" & info_org.Length & ")")
                _ObjStatus.AddStatus("Sent file length mismatch :" & vbCrLf & XmlFilePath_bak & "(" & info_back.Length & ")" & XmlFilePath & "(" & info_org.Length & ")")
                ' _ObjStatus.ShowDialog()
                Return False
            End If
        Next
        Return True
    End Function
    Function SendAcknowledge(ByVal R_fromid As String, ByVal R_toid As String, ByVal mUpdfile As String)
        Dim mqry As String
        '' acknowledgement reply
        If _Tran Is Nothing Then _Tran = _CnAdmin.BeginTransaction
        Dim replsql As String = " UPDATE " & _Syncdb & "..SENDSYNC SET STATUS = 'Y' WHERE STATUS = 'M' AND FROMID = '" & R_fromid & "' AND TOID = '" & R_toid & "' and UPDFILE ='" & mUpdfile & "'"
        mqry = vbCrLf + " INSERT INTO " & _Syncdb & "..SENDSYNC ("
        mqry += vbCrLf + " FROMID"
        mqry += vbCrLf + " ,TOID"
        mqry += vbCrLf + " ,SQLTEXT"
        mqry += vbCrLf + " ,STATUS)"
        mqry += vbCrLf + " VALUES "
        mqry += vbCrLf + " (?,?,?,?)"
        _Cmd = New OleDbCommand(mqry, _CnAdmin, _Tran)
        _Cmd.Parameters.AddWithValue("@FROMID", R_toid.ToString)
        _Cmd.Parameters.AddWithValue("@TOID", R_fromid.ToString)
        _Cmd.Parameters.AddWithValue("@SQLTEXT", replsql.ToString)
        _Cmd.Parameters.AddWithValue("@STATUS", "N")
        _Cmd.ExecuteNonQuery()
        If _Tran.Connection IsNot Nothing Then _Tran.Commit()


    End Function

    Function SendAcknowledgePassFail(ByVal R_fromid As String, ByVal R_toid As String, ByVal Uidstring As String, ByVal status As String)
        Dim mqry As String
        '' acknowledgement reply
        If Uidstring.Length = 0 Then Exit Function
        Dim Tranbegin As Boolean = False
        If _Tran Is Nothing Then _Tran = _CnAdmin.BeginTransaction : Tranbegin = True

        Dim replsql As String = " UPDATE " & _Syncdb & "..SENDSYNC SET STATUS = '" & status & "' WHERE STATUS = 'M' AND FROMID = '" & R_fromid & "' AND TOID = '" & R_toid & "' and " & Uidstring
        mqry = vbCrLf + " INSERT INTO " & _Syncdb & "..SENDSYNC ("
        mqry += vbCrLf + " FROMID"
        mqry += vbCrLf + " ,TOID"
        mqry += vbCrLf + " ,SQLTEXT"
        mqry += vbCrLf + " ,STATUS)"
        mqry += vbCrLf + " VALUES "
        mqry += vbCrLf + " (?,?,?,?)"
        _Cmd = New OleDbCommand(mqry, _CnAdmin, _Tran)
        _Cmd.Parameters.AddWithValue("@FROMID", R_toid.ToString)
        _Cmd.Parameters.AddWithValue("@TOID", R_fromid.ToString)
        _Cmd.Parameters.AddWithValue("@SQLTEXT", replsql.ToString)
        _Cmd.Parameters.AddWithValue("@STATUS", "N")
        _Cmd.ExecuteNonQuery()
        If _Tran.Connection IsNot Nothing And Tranbegin Then _Tran.Commit()

    End Function
    Private Function GetAdmindbSoftValue(ByVal CTLID As String, ByVal DEFVALUE As String) As String
        StrSql = " SELECT CTLTEXT FROM " & _Admindb & "..SOFTCONTROL WHERE CTLID = '" & CTLID & "'"
        Dim NEWVALUE As String = GMethods.GetSqlValue(_CnAdmin, StrSql)
        If NEWVALUE Is Nothing Then Return DEFVALUE Else Return NEWVALUE
    End Function
        

    Public Sub SyncPost()
        Dim mSyncPost() As String = Split(GetAdmindbSoftValue("SYNCPOST", "N,0"), ",")

        If mSyncPost(0).ToString <> "Y" Then Exit Sub
        Dim Todaydate As Date = DateAdd(DateInterval.Day, Val(mSyncPost(1).ToString) * (-1), Now.Date)
        Dim currcostid As String = GetAdmindbSoftValue("COSTID", "")
        StrSql = " SELECT 'EXIST' FROM "
        StrSql += " ("
        StrSql += " SELECT 'EXIST' CHECKS FROM " & _Syncdb & "..SENDSYNC WHERE STATUS ='Y'"
        StrSql += " AND UPDATED < '" & Todaydate.ToString("dd-MMM-yyyy") & "'"
        StrSql += " UNION"
        StrSql += " SELECT 'EXIST' FROM " & _Syncdb & "..RECEIVESYNC WHERE STATUS ='Y'"
        StrSql += " AND UPDATED < '" & Todaydate.ToString("dd-MMM-yyyy") & "'"
        StrSql += " )x"
        If GMethods.GetSqlValue(_CnAdmin, StrSql) = "" Then Exit Sub
        Dim dtstr = Now.Date.ToString("ddMMyy")
        Dim XmlFilePath As String = Nothing
        Dim ZipFilePath As String = Nothing
        ' Dim Bakpath As String = "\\" + _Coninfo.lServerName + "\" + Replace(_Coninfo.lDbPath, ":", "") + "\SYNCBACK"
        'If mSyncPost(0).ToString = "B" Then
        '    Dim Syncbackfiles As String = "SYNC" + currcostid + dtstr + "S"
        '    If Not IO.Directory.Exists(Bakpath) Then IO.Directory.CreateDirectory(Bakpath)
        '    Bakpath = Bakpath + "\"
        '    Dim DsSend As New DataSet
        '    XmlFilePath = Bakpath & Syncbackfiles & ".xml"
        '    ZipFilePath = Bakpath & Syncbackfiles & ".zip"
        '    Dim S_Dt As New DataTable
        '    StrSql = " SELECT top 1000 * FROM " & _Syncdb & "..SENDSYNC WHERE STATUS ='Y'"
        '    StrSql += " AND UPDATED < '" & Todaydate.ToString("dd-MMM-yyyy") & "'"
        '    _Da = New OleDbDataAdapter(StrSql, _CnAdmin)
        '    _Da.Fill(S_Dt)
        '    DsSend.Tables.Add(S_Dt)
        '    DsSend.WriteXml(XmlFilePath, XmlWriteMode.WriteSchema)
        'End If
        Dim DsRecv As New DataSet
        StrSql = " DELETE  FROM " & _Syncdb & "..SENDSYNC WHERE UID IN (SELECT TOP 1000 UID FROM " & _Syncdb & "..SENDSYNC WHERE STATUS ='Y' AND UPDATED < '" & Todaydate.ToString("dd-MMM-yyyy") & "')"
        _Cmd = New OleDbCommand(StrSql, _CnAdmin)
        _Cmd.ExecuteNonQuery()
        'If mSyncPost(0).ToString = "B" Then
        '    Dim Syncbackfiler As String = "SYNC" + currcostid + dtstr + "R"
        '    XmlFilePath = Bakpath & Syncbackfiler & ".xml"
        '    ZipFilePath = Bakpath & Syncbackfiler & ".zip"
        '    StrSql = " SELECT  top 1000 * FROM " & _Syncdb & "..RECEIVESYNC WHERE STATUS ='Y'"
        '    StrSql += " AND UPDATED < '" & Todaydate.ToString("dd-MMM-yyyy") & "'"
        '    Dim R_Dt As New DataTable
        '    _Da = New OleDbDataAdapter(StrSql, _CnAdmin)
        '    _Da.Fill(R_Dt)
        '    DsRecv.Tables.Add(R_Dt)
        '    DsRecv.WriteXml(XmlFilePath, XmlWriteMode.WriteSchema)
        'End If
        StrSql = " DELETE FROM " & _Syncdb & "..RECEIVESYNC WHERE UID IN (SELECT TOP 1000 UID FROM " & _Syncdb & "..RECEIVESYNC WHERE STATUS ='Y' AND UPDATED < '" & Todaydate.ToString("dd-MMM-yyyy") & "')"
        _Cmd = New OleDbCommand(StrSql, _CnAdmin)
        _Cmd.ExecuteNonQuery()
    End Sub

    Function CHITTRANPOST_TRG(ByVal SWITCHON As Boolean)
        If SWITCHON = False Then
            StrSql = vbCrLf + "  IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE XTYPE = 'TR' AND NAME = 'TRIG_SAVINGSTOACCTRAN')>0"
            StrSql += vbCrLf + "  BEGIN"
            StrSql += vbCrLf + "  DROP TRIGGER TRIG_SAVINGSTOACCTRAN"
            StrSql += vbCrLf + "  END"
            _Cmd = New OleDbCommand(StrSql, _Chitcn)
            _Cmd.ExecuteNonQuery()
        Else
            Dim savingdb As String = _Coninfo.lCompanyId + "SAVING"
            StrSql = vbCrLf + "CREATE TRIGGER [dbo].[TRIG_SAVINGSTOACCTRAN] ON [dbo].[SCHEMECOLLECT] FOR INSERT      "
            StrSql += vbCrLf + " AS      "
            StrSql += vbCrLf + "BEGIN /*Trigger Starts */      "
            StrSql += vbCrLf + "SET NOCOUNT ON    "
            StrSql += vbCrLf + " DECLARE @QRY VARCHAR(8000)      "
            StrSql += vbCrLf + " DECLARE @QRY1 VARCHAR(8000)      "
            StrSql += vbCrLf + " DECLARE @TRANDB VARCHAR(8)    "
            StrSql += vbCrLf + " DECLARE @TTRANDATE VARCHAR(12)    "
            StrSql += vbCrLf + " SELECT @TTRANDATE = RDATE FROM INSERTED    "
            StrSql += vbCrLf + " SELECT @QRY = ''      "
            StrSql += vbCrLf + " SELECT * INTO #TINSERTED FROM INSERTED  "
            StrSql += vbCrLf + " IF (SELECT COUNT(*) FROM " & savingdb & "..SOFTCONTROL WHERE CTLID = 'JEWELDBPREFIX' AND CTLTEXT <> '')>0    "
            StrSql += vbCrLf + " BEGIN /** JEWELDB CHECKING STARTS HERE */    "
            StrSql += vbCrLf + "  CREATE TABLE #TTRANDB(DBNAME VARCHAR(15))    "
            StrSql += vbCrLf + "  DECLARE @JEWELDB AS VARCHAR(10)    "
            StrSql += vbCrLf + "  SELECT @JEWELDB = substring(CTLTEXT,1,3) + 'ADMINDB' FROM " & savingdb & "..SOFTCONTROL WHERE CTLID = 'JEWELDBPREFIX'    "
            StrSql += vbCrLf + "  IF (SELECT TOP 1 1 FROM MASTER..SYSDATABASES WHERE NAME = @JEWELDB) > 0  "
            StrSql += vbCrLf + "  BEGIN  "
            StrSql += vbCrLf + "  SELECT @QRY = @QRY + ' INSERT INTO #TTRANDB(DBNAME) SELECT DBNAME FROM '+@JEWELDB+'..DBMASTER WHERE '''+@TTRANDATE+''' BETWEEN STARTDATE AND ENDDATE  '    "
            StrSql += vbCrLf + "  EXEC (@QRY)    "
            StrSql += vbCrLf + "  SELECT @TRANDB = DBNAME FROM #TTRANDB      "
            StrSql += vbCrLf + "  SELECT @QRY = ' /**/ DECLARE @TRANDATE SMALLDATETIME '      "
            StrSql += vbCrLf + "  SELECT @QRY = @QRY + ' /**/ SELECT @TRANDATE = RDATE FROM #TINSERTED '      "
            StrSql += vbCrLf + "  SELECT @QRY = @QRY + ' /**/ EXEC '+@TRANDB+'..SP_CHIT_ACC_POST '      "
            StrSql += vbCrLf + "  SELECT @QRY = @QRY + ' /**/ @FRMDATE=@TRANDATE'"
            StrSql += vbCrLf + "  SELECT @QRY = @QRY + ' /**/ ,@TODATE=@TRANDATE'"
            StrSql += vbCrLf + "  EXEC (@QRY)    "
            StrSql += vbCrLf + " END /* JEWELDB EXISTS CHECKING ENDS HERE */     "
            StrSql += vbCrLf + " END /* JEWELDB CHECKING ENDS HERE */     "
            StrSql += vbCrLf + "END /* Trigger Ends */"
            _Cmd = New OleDbCommand(StrSql, _Chitcn)
        End If
    End Function

    Private Function ReplaceQryStr(ByVal SourceStr As String, ByVal OldDbId As String, ByVal NewDbId As String) As String
        Dim I_SenderDb As String = ""
        Dim I_ReplaceDb As String = ""
        Dim I_IndexOfDb As Integer
        Dim OldStr As String = ""
        Dim NewStr As String = ""
        For Each suffix As String In _ReplaceWords
            '            If (suffix.ToUpper = "SAVINGS.." Or suffix.ToUpper = "SH0708..") And Trim(_VbDbPrefix) <> "" Then I_ReplaceDb = _VbDbPrefix Else I_ReplaceDb = NewDbId
            I_IndexOfDb = SourceStr.ToUpper.IndexOf(suffix.ToUpper)
            If I_IndexOfDb > -1 Then
                I_SenderDb = Mid(SourceStr.ToUpper, I_IndexOfDb - 2, 3)
                ''I_SenderDb = OldDbId
                For Each suf As String In _ReplaceWords
                    If (suf.ToUpper = "SAVINGS.." Or suf.ToUpper = "SH0708..") And Trim(_VbDbPrefix) <> "" Then I_ReplaceDb = _VbDbPrefix Else I_ReplaceDb = NewDbId
                    OldStr = I_SenderDb.ToUpper & suf
                    NewStr = I_ReplaceDb.ToUpper & suf
                    'SourceStr = SourceStr.Replace(OldStr, NewStr)
                    SourceStr = Replace(SourceStr, OldStr, NewStr, , , CompareMethod.Text)
                    'If SourceStr.Contains("@@@@@(") Then SourceStr = SourceStr.Replace(OldStr, NewStr) Else SourceStr = SourceStr.ToUpper.Replace(OldStr, NewStr)
                Next
            End If
        Next
        Return SourceStr
    End Function
    Private Function ReplaceQryStrNew(ByVal SourceStr As String, ByVal OldDbId As String, ByVal NewDbId As String) As String
        'SourceStr = SourceStr.ToUpper.Replace(OldDbId, NewDbId)
        SourceStr = SourceStr.Replace(OldDbId, NewDbId.ToUpper)
        Return SourceStr
    End Function

    Private Function ReplaceqryPwdstr(ByVal SourceStr As String) As String
NEXTONE:
        Dim Startpt As Integer = InStr(SourceStr, "@@@@@(") + 6
        Dim Endpt As Integer = InStr(SourceStr, ")@@@@@")
        Dim DecEncpwd As String = Mid(SourceStr, Startpt, Endpt - Startpt)
        Dim Oldstring As String = "@@@@@(" + DecEncpwd + ")@@@@@"
        Dim Newstring As String = ""
        Dim pwdlen As Integer = Len(DecEncpwd) + 1
        'Dim encdecpwd As String = "@@@@@("
        For iii As Integer = 0 To Len(DecEncpwd) - 1
            Newstring = Newstring & Mid(DecEncpwd, iii + 1, 1)
            iii += 1
        Next
        'Newstring = GiritechPack.Methods.Encrypt(Newstring)
        Newstring = Encrypt(Newstring)
        SourceStr = SourceStr.Replace(Oldstring, Newstring)
        If SourceStr.Contains("@@@@@(") Then GoTo NEXTONE
        Return SourceStr
    End Function

#End Region

    Private Sub NotifyIcon1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.Click
        If TxtStatus.Text = "" Then Exit Sub
        NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
        NotifyIcon1.BalloonTipText = TxtStatus.Text
        NotifyIcon1.ShowBalloonTip(1000)
    End Sub

    Private Sub NotifyIcon1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            NotifyIcon1.Text = ""
            'NotifyIcon1.ShowBalloonTip(0)
        End If
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Me.Show()
        End If

    End Sub

    Private Sub NotifyIcon1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseMove

    End Sub

    Private Function GetWebConnectionString()
        Return "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & _WebDbUserName & ";password=" & _WebDbPass & ";Initial Catalog=" & _WebDbName & ";Data Source=" & _WebDbServerName
        'Return "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & _WebDbUserName & ";password=reset@123;Initial Catalog=" & _WebDbName & ";Data Source=" & _WebDbServerName
    End Function
    Private Function GetWebLocalConnectionString(ByVal CostId As String)
        Dim WebDbUser As String = ""
        Dim WebDbPass As String = ""
        Dim WebDb As String = ""
        Dim WebDbServer As String = ""
        Dim dtWebLocal As New DataTable
        StrSql = "SELECT * FROM " & _Admindb & "..SYNCCOSTCENTRE WHERE COSTID='" & CostId & "'"
        _Da = New OleDbDataAdapter(StrSql, _CnAdmin)
        _Da.Fill(dtWebLocal)
        If dtWebLocal.Rows.Count > 0 Then
            WebDbUser = dtWebLocal.Rows(0).Item("WEBDBUSERNAME").ToString
            WebDbPass = dtWebLocal.Rows(0).Item("PASSWORD").ToString
            'If WebDbPass <> "" Then WebDbPass = GiritechPack.Methods.Decrypt(WebDbPass)
            If WebDbPass <> "" Then WebDbPass = Decrypt(WebDbPass)
            WebDb = dtWebLocal.Rows(0).Item("WEBDBNAME").ToString
            WebDbServer = dtWebLocal.Rows(0).Item("FTPID").ToString
            _LocalTblPrefix = dtWebLocal.Rows(0).Item("WEBDBTBLPREFIX").ToString
        End If
        Return "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & WebDbUser & ";password=" & WebDbPass & ";Initial Catalog=" & WebDb & ";Data Source=" & WebDbServer
    End Function

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnableToolStripMenuItem.Click
        'KillApp()
        If EnableToolStripMenuItem.Text = "Enable" Then
            Timer1.Enabled = True
            EnableToolStripMenuItem.Text = "Disable"
            Schedule_sync()
        ElseIf EnableToolStripMenuItem.Text = "Disable" Then
            NotifyIcon1.Text = _Status_msg & " @ " & _FromId.ToUpper & vbCrLf & "Disabled"
            NotifyIcon1.BalloonTipText = _Status_msg & " @ " & _FromId.ToUpper & vbCrLf & "Disabled"
            EnableToolStripMenuItem.Text = "Enable"
            Timer1.Enabled = False
            If t IsNot Nothing Then t.Abort()
            If t1 IsNot Nothing Then t1.Abort()
            If t2 IsNot Nothing Then t2.Abort()

        End If
    End Sub

    Private Sub KillApp(Optional ByVal appRestart As Boolean = False)
        If t IsNot Nothing Then t.Abort()
        If t1 IsNot Nothing Then t1.Abort()
        If t2 IsNot Nothing Then t2.Abort()
        NotifyIcon1.Icon = Nothing
        NotifyIcon1.Visible = False
        If _ObjStatus IsNot Nothing Then _ObjStatus.Dispose()
        If appRestart Then
            GC.Collect()
            Application.Restart()
        Else
            GC.Collect()
            'Me.Close()
            Environment.Exit(0)
        End If
    End Sub

    Private Sub StatusToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusToolStripMenuItem.Click
        _ObjStatus = New Status
        Pendingstatus()

        _ObjStatus.Show()
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick

    End Sub
    Private Sub Pendingstatus()
        Try


            StrSql = " SELECT 'RECEIVE <<' AS STATUS,FROMID,TOID,COUNT(*) AS RECORDS  FROM " & _Syncdb & "..RECEIVESYNC WHERE STATUS = 'N' GROUP BY FROMID,TOID"
            StrSql += " UNION ALL SELECT 'SEND >>' AS STATUS,FROMID,TOID,COUNT(*) AS RECORDS  FROM " & _Syncdb & "..SENDSYNC WHERE STATUS = 'N' GROUP BY FROMID,TOID"
            Dim DtPend As New DataTable
            _Da = New OleDbDataAdapter(StrSql, _CnAdmin)
            _Da.Fill(DtPend)
            _ObjStatus.AddStatus(DtPend)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If tspan.Minutes = _SyncSch Then
            GC.Collect()
            Schedule_sync()
        End If
    End Sub

    Private Sub ReceiveOnlyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReceiveOnlyToolStripMenuItem.Click
        Call Schedule_sync("R")
    End Sub

    Private Sub SendOnlyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SendOnlyToolStripMenuItem.Click
        Call Schedule_sync("S")
    End Sub

    Private Sub ExitToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        KillApp()
    End Sub
End Class
