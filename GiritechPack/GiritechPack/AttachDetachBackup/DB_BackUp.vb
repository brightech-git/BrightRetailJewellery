Imports System.Data.OleDb
Imports System.IO
Public Class DB_Bakup
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cn As OleDbConnection
    Dim cmd As OleDbCommand
    Dim dtStatus As New DataTable

    Dim cnCompanyId As String = Nothing
    Dim cnCompanyName As String = Nothing
    Dim ServerName As String = Nothing
    Dim passWord As String = Nothing
    Dim Logintype As String = Nothing
    Dim dbPath As String = Nothing
    Dim fileConcat As String
    Dim BackUpType As String = "M"

    Public Enum Type
        DayWise = 0
        TimeWise = 1
    End Enum


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        rbtTimeWise.Checked = True
    End Sub

    Public Sub New(ByVal defaultBackUpPath As String, ByVal fileNameConcatStr As String, Optional ByVal Type As String = "M")
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If Directory.Exists(defaultBackUpPath) Then
            txtBackUpPath.Text = defaultBackUpPath
        End If
        BackUpType = Type
        fileConcat = fileNameConcatStr
        rbtDayWise.Visible = False
        rbtTimeWise.Visible = False
    End Sub

    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Dim dtDbNames As New DataTable
        chkLstDataBases.Items.Clear()
        strSql = " SELECT NAME FROM SYSDATABASES "
        strSql += "  WHERE DBID NOT IN (1,2,3,4)"
        strSql += " ORDER BY NAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDbNames)
        For Each ro As DataRow In dtDbNames.Rows
            chkLstDataBases.Items.Add(UCase(ro!NAME.ToString))
        Next
        Dim SavedDb As Boolean = False
        If Not My.Settings.BackUpDbNames Is Nothing Then
            For cnt As Integer = 0 To chkLstDataBases.Items.Count - 1
                If My.Settings.BackUpDbNames.Contains(chkLstDataBases.Items(cnt).ToString) Then
                    chkLstDataBases.SetItemChecked(cnt, True)
                    SavedDb = True
                End If
            Next
        End If
        Dim i As Integer = 0
        If SavedDb = False Then
            For Each ro As DataRow In dtDbNames.Rows
                If UCase(ro!NAME.ToString).StartsWith(UCase(cnCompanyId)) Then
                    chkLstDataBases.SetItemChecked(i, True)
                    chkLstDataBases.SetSelected(i, True)
                End If
                i += 1
            Next
        End If
        chkLstDataBases.ClearSelected()
        If chkLstDataBases.Items.Count > 0 Then
            btnBackUp.Enabled = True
            ''chkLstDataBases.Focus()
            SendKeys.Send("{TAB}")
        Else
            btnBackUp.Enabled = False
        End If
    End Sub

    Private Sub frmBakup_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'If Not My.Settings.BackUpDbNames Is Nothing Then My.Settings.BackUpDbNames.Clear() Else My.Settings.BackUpDbNames = New ArrayList
        'For i As Integer = 0 To chkLstDataBases.CheckedItems.Count - 1
        '    My.Settings.BackUpDbNames.Add(chkLstDataBases.CheckedItems.Item(i).ToString)
        'Next
        'My.Settings.Save()
        'cn.Close()
    End Sub

    Private Sub frmBakupAndRestore_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnBackUp.Enabled = False
        If IO.File.Exists(Application.StartupPath + "\ConInfo.ini") Then
            Dim file As New FileStream(Application.StartupPath + "\ConInfo.ini", FileMode.Open)
            Dim fstream As New StreamReader(file, System.Text.Encoding.Default)
            fstream.BaseStream.Seek(0, SeekOrigin.Begin)
            cnCompanyId = Mid(fstream.ReadLine, 21)
            cnCompanyName = Mid(fstream.ReadLine, 21)
            ServerName = Mid(fstream.ReadLine, 21)
            dbPath = Mid(fstream.ReadLine, 21)
            passWord = Mid(fstream.ReadLine, 21)
            Logintype = Mid(fstream.ReadLine, 21)
            fstream.Close()
            If passWord <> "" Then passWord = Decrypt(passWord)

            Try
                'cn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=;Data Source={0};uid=sa;pwd=;", ServerName))
                If Logintype.ToUpper() = "W" Then
                    cn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Data Source={0};", ServerName))
                Else
                    cn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=;Data Source={0};uid={1};pwd={2};", ServerName, IIf(Logintype.ToUpper() = "S", "SA", Logintype.ToUpper()), passWord))
                End If
            Catch ex As Exception
                MsgBox(ServerName + " Connection Failed", MsgBoxStyle.Information)
                MsgBox(ex.Message)
                Exit Sub
            End Try
        Else
            cn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=;Data Source=.;uid=sa;pwd=;"))
        End If
        cn.Open()

        dtStatus.Columns.Add("STATUS", GetType(String))
        gridStatus.DataSource = dtStatus
        gridStatus.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridStatus.ColumnHeadersVisible = False
        gridStatus.Columns("STATUS").Width = gridStatus.Width - 20
        btnLoad.Select()
    End Sub

    Private Function GetSQl(ByVal str As String) As Object
        Dim dtVal As New DataTable
        Dim retValue As Object = Nothing
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dtVal)
        If dtVal.Rows.Count > 0 Then
            retValue = dtVal.Rows(0).Item(0)
        End If
        Return retValue
    End Function


    Private Sub btnBackUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackUp.Click
        ''Validation
        If Not chkLstDataBases.CheckedItems.Count > 0 Then
            MsgBox("Please Check Database", MsgBoxStyle.Information)
            chkLstDataBases.Focus()
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
            btnBackUp.Enabled = False
            If rbtDayWise.Visible Then
                Dim dd As Date = GetSQl("SELECT GETDATE()")
                If rbtDayWise.Checked Then
                    fileConcat = "_" + Format(dd.Date.Day, "00") + Format(dd.Date.Month, "00") + Mid(dd.Date.Year, 3, 2)
                Else
                    fileConcat = "_" + Format(dd.Date.Day, "00") + Format(dd.Date.Month, "00") + Mid(dd.Date.Year, 3, 2) _
                    + "_" + Format(dd.TimeOfDay.Hours, "00") + Format(dd.TimeOfDay.Minutes, "00") + Format(dd.TimeOfDay.Seconds, "00")
                End If
            End If

            For i As Integer = 0 To chkLstDataBases.CheckedItems.Count - 1
                Dim dbName As String = chkLstDataBases.CheckedItems.Item(i).ToString
                Dim SrName As String = ServerName
                If SrName.Contains("\") Then
                    SrName = Mid(SrName, 1, SrName.IndexOf("\"))
                End If
                Dim tempPath As String = GetSQl("SELECT FILENAME FROM SYSDATABASES WHERE NAME = '" & dbName & "'").ToString
                tempPath = IIf(SrName <> Nothing And UCase(SrName) <> UCase(My.Computer.Name), "\\" + SrName + "\", "") + tempPath
                tempPath = IIf(SrName <> Nothing And UCase(SrName) <> UCase(My.Computer.Name), tempPath.Replace(":", ""), tempPath)

CheckFilePermission:

                If IO.File.Exists(tempPath) = False Then
PermissionMsg:
                    Dim msgINfo As String = tempPath.Substring(0, tempPath.Length - tempPath.LastIndexOf("\")) + " Write Protected" + vbCrLf + "Please Change that access permission"
                    Select Case MessageBox.Show(msgINfo, "Access Denied", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2)
                        Case Windows.Forms.DialogResult.Abort
                            Exit Sub
                        Case Windows.Forms.DialogResult.Retry
                            GoTo CheckFilePermission
                        Case Windows.Forms.DialogResult.Ignore
                            Continue For
                    End Select
                Else
                    Dim dirInfo As New DirectoryInfo(tempPath.Substring(0, tempPath.Length - tempPath.LastIndexOf("\")))
                    If dirInfo.Attributes = FileAttributes.ReadOnly Then
                        GoTo PermissionMsg
                    End If
                End If
                Dim fil As New FileInfo(tempPath)
                tempPath = fil.DirectoryName & "\" & dbName & fileConcat & ".BAK"
                If IO.File.Exists(tempPath) Then
                    IO.File.Delete(tempPath)
                End If

                Dim dbBackPath As String = txtBackUpPath.Text & "\" & dbName & fileConcat & ".BAK"
                dbBackPath = dbBackPath.Replace("\\", "\")

                LoadStatus(" Set Recovery to " + dbName, True)
                pBarStep()
                _BackUpShrink(dbName)
                strSql = " ALTER DATABASE " & dbName & " SET RECOVERY FULL"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                LoadStatus(dbName + " Set Recovery Completed..")
                pBarStep()


                LoadStatus("Backup " + dbName + "..", True)
                pBarStep()
                strSql = " BACKUP DATABASE [" & dbName & "] TO  DISK = N'" & tempPath & "' WITH  NOINIT ,  NOUNLOAD ,  NAME = N'" & dbName & " BackUp" & "',  NOSKIP ,  STATS = 10,  NOFORMAT "
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                LoadStatus("Backup " + dbName + " Completed..")
                pBarStep()

                If IO.File.Exists(dbBackPath) Then
                    IO.File.Delete(dbBackPath)
                End If
                If BackUpType = "C" Then IO.File.Copy(tempPath, dbBackPath) Else IO.File.Move(tempPath, dbBackPath)
            Next
            pBar.Value = pBar.Maximum
            MsgBox("Task Completed")
            Me.DialogResult = Windows.Forms.DialogResult.OK
            ''Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            btnBackUp.Enabled = True
        End Try
    End Sub
    Private Sub _BackUpShrink(ByVal DbName As String)

        Dim CnShrink As New OleDbConnection(GetConnectionString(DbName, ServerName, passWord, Mid(Logintype, 1, 1)))

        CnShrink.Open()
        strSql = " SELECT NAME FROM " & DbName & "..SYSFILES WHERE FILEID = 2"
        Dim LogFile As String = GetSqlValue(CnShrink, strSql, , , )
        LogFile = LogFile.Replace(Chr(10), "")
        LogFile = LogFile.Replace(Environment.NewLine, "").Trim
        strSql = " ALTER DATABASE " & DbName & " SET RECOVERY SIMPLE"
        cmd = New OleDbCommand(strSql, CnShrink) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        strSql = " ALTER DATABASE " & DbName & " SET RECOVERY FULL"
        cmd = New OleDbCommand(strSql, CnShrink) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        strSql = " DBCC SHRINKFILE('" & LogFile & "',2)"
        cmd = New OleDbCommand(strSql, CnShrink) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        If CnShrink.State = ConnectionState.Open Then
            CnShrink.Close()
        End If
    End Sub
    Private Function GetConnectionString(ByVal DbName As String, ByVal ServerName As String, ByVal Pwd As String, ByVal DbType As String)
        Dim retStr As String = ""
        If DbType.ToUpper = "S" Then
            retStr = "PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & DbName & ";Data Source=" & ServerName & ";uid=" & IIf(DbType.ToUpper() = "S", "SA", DbType.ToUpper()) & ";pwd=" & Pwd & ";"
        Else
            retStr = "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & DbName & ";Data Source=" & ServerName & ""
        End If
        Return retStr
    End Function
    Private Sub LoadStatus(ByVal status As String, Optional ByVal newRow As Boolean = False)
        If newRow Then
            dtStatus.Rows.Add()
            dtStatus.AcceptChanges()
        End If
        dtStatus.Rows(dtStatus.Rows.Count - 1).Item("STATUS") = status
        gridStatus.CurrentCell = gridStatus.Rows(gridStatus.RowCount - 1).Cells(0)
        gridStatus.Refresh()
    End Sub

    Private Sub pBarStep(Optional ByVal stepVal As Integer = 5)
        If pBar.Value + stepVal > pBar.Maximum Then pBar.Value = 0
        pBar.Value += stepVal
        pBar.Refresh()
    End Sub

    Private Sub btnBrowsePath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowsePath.Click
        Dim openFolderDia As New FolderBrowserDialog
        If openFolderDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtBackUpPath.Text = openFolderDia.SelectedPath
        End If
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub chkLstDataBases_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkLstDataBases.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub chkLstDataBases_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstDataBases.LostFocus
        chkLstDataBases.ClearSelected()
    End Sub

    Private Sub rbtDayWise_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rbtDayWise.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub rbtTimeWise_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rbtTimeWise.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtBackUpPath_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBackUpPath.GotFocus
        btnBrowsePath.Focus()
    End Sub

    Private Sub gridStatus_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridStatus.GotFocus
        btnBackUp.Focus()
    End Sub

    Private Sub pBar_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles pBar.GotFocus
        btnBackUp.Focus()
    End Sub

    Private Sub btnBrowsePath_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnBrowsePath.KeyDown
        If e.KeyCode = Keys.Escape Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
End Class

