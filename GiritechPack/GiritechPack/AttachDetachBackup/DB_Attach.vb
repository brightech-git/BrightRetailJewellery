Imports System.Data.OleDb
Imports System.IO
Public Class DB_Attach
    Dim strSql As String
    Dim cn As OleDbConnection
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtFileView As New DataTable
    Dim defDestPath As String

    Dim cnCompanyId As String = Nothing
    Dim cnCompanyName As String = Nothing
    Dim ServerName As String = Nothing
    Dim passWord As String = Nothing
    Dim Logintype As String = Nothing

    Private Sub frmAttach_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        cn.Close()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub New(ByVal defaultDestinationPath As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        defDestPath = defaultDestinationPath
    End Sub

    Private Sub frmAttach_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnAttach.Enabled = False
        If IO.File.Exists(Application.StartupPath + "\ConInfo.ini") Then
            Dim file As New FileStream(Application.StartupPath + "\ConInfo.ini", FileMode.Open)
            Dim fstream As New StreamReader(file, System.Text.Encoding.Default)
            fstream.BaseStream.Seek(0, SeekOrigin.Begin)
            cnCompanyId = Mid(fstream.ReadLine, 21)
            cnCompanyName = Mid(fstream.ReadLine, 21)
            ServerName = Mid(fstream.ReadLine, 21)
            passWord = Mid(fstream.ReadLine, 21)
            Logintype = Mid(fstream.ReadLine, 21)
            fstream.Close()
            If passWord <> "" Then passWord = Decrypt(passWord)
            Try
                If Logintype.ToUpper() = "W" Then
                    cn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Data Source={0};", ServerName))
                Else
                    cn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=;Data Source={0};uid={1};pwd=;", ServerName, IIf(Logintype.ToUpper() = "S", "SA", Logintype.ToUpper())))
                End If
            Catch ex As Exception
                MsgBox(ServerName + " Connection Failed", MsgBoxStyle.Information)
                MsgBox(ex.Message)
                Exit Sub
            End Try
        Else
            cn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=;Data Source=.;uid=sa;pwd=;"))
            ServerName = "."
            'MsgBox("Connection Info Not Found", MsgBoxStyle.Information)
            'btnAttach.Enabled = False
            'Exit Sub
        End If
        cn.Open()
        dtFileView.Columns.Add("FileName(s)", GetType(String))
        dtFileView.Columns.Add("FileLocation(s)", GetType(String))
        dtFileView.Rows.Add()
        dtFileView.Rows.Add()
        gridFileView.DataSource = dtFileView
        gridFileView.Columns("FILENAME(S)").Width = 160
        gridFileView.Columns("FILENAME(S)").SortMode = DataGridViewColumnSortMode.NotSortable
        gridFileView.Columns("FILELOCATION(S)").Width = 232
        gridFileView.Columns("FILELOCATION(S)").SortMode = DataGridViewColumnSortMode.NotSortable
        txtDestinationPath.Text = defDestPath
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnBrowseSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseSource.Click
        Dim fileOpenDia As New OpenFileDialog
        fileOpenDia.Filter = "Master Data Files (*.mdf)|*.mdf"
        Dim FInfo As FileInfo = Nothing
        Dim file1 As String = Nothing
        Dim file1Path As String = Nothing
        Dim file2 As String = Nothing
        Dim file2Path As String = Nothing
        If fileOpenDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.Refresh()
            If UCase(GetSQl("SELECT NAME FROM SYSDATABASES WHERE FILENAME = '" & fileOpenDia.FileName & "'")).ToString <> "" Then
                btnAttach.Enabled = False
                MsgBox("Already Attached..", MsgBoxStyle.Information)
                Exit Sub
            End If

            'If Not (UCase(ServerName) = UCase(My.Computer.Name) Or ServerName = ".") Then
            '    If UCase(txtDestinationPath.Text).StartsWith(UCase("\\" + ServerName)) = False Then
            '        MsgBox("Select \\" + ServerName + " Path Only", MsgBoxStyle.Exclamation)
            '        Exit Sub
            '    End If
            'Else
            '    If UCase(txtDestinationPath.Text).StartsWith(UCase("\\" + ServerName)) Then
            '        MsgBox("Local Path Only", MsgBoxStyle.Exclamation)
            '        Exit Sub
            '    End If
            'End If
            txtSourcePath.Text = fileOpenDia.FileName
            FInfo = New FileInfo(txtSourcePath.Text)
            file1 = Trim(FInfo.Name)
            file1Path = Trim(FInfo.FullName)

            dtFileView.Rows(0).Item("FILENAME(S)") = file1
            dtFileView.Rows(0).Item("FILELOCATION(S)") = file1Path
            For Each file As String In Directory.GetFiles(FInfo.DirectoryName, "*.ldf")
                FInfo = New FileInfo(file)
                If UCase(FInfo.Name).StartsWith(UCase(file1.Replace(".mdf", ""))) Then
                    file2 = FInfo.Name
                    file2Path = FInfo.FullName
                    dtFileView.Rows(1).Item("FILENAME(S)") = Trim(file2)
                    dtFileView.Rows(1).Item("FILELOCATION(S)") = file2Path
                    ''Exit For
                End If

            Next
            btnAttach.Enabled = True

            'strSql = " DBCC CHECKPRIMARYFILE (N'" & txtSourcePath.Text & "', 3)"
            'Dim dtFileDetails As New DataTable
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dtFileDetails)
            'If dtFileDetails.Rows.Count > 0 Then
            '    Dim fil As FileInfo = Nothing
            '    'Dim file1 As String = Nothing
            '    'Dim file1Path As String = Nothing
            '    'Dim file2 As String = Nothing
            '    'Dim file2Path As String = Nothing
            '    fil = New FileInfo(dtFileDetails.Rows(0).Item("filename").ToString)
            '    file1 = fil.Name
            '    file1Path = fil.FullName
            '    dtFileView.Rows(0).Item("FILENAME(S)") = Trim(file1)
            '    dtFileView.Rows(0).Item("FILELOCATION(S)") = file1Path
            '    If dtFileDetails.Rows.Count >= 2 Then
            '        fil = New FileInfo(dtFileDetails.Rows(1).Item("filename").ToString)
            '        file2 = fil.Name
            '        file2Path = fil.FullName
            '    End If
            '    dtFileView.Rows(1).Item("FILENAME(S)") = Trim(file2)
            '    dtFileView.Rows(1).Item("FILELOCATION(S)") = file2Path
            '    btnAttach.Enabled = True
            'End If
            Me.Refresh()
        End If
    End Sub

    Private Function GetSQl(ByVal str As String) As String
        Dim dtVal As New DataTable
        Dim retValue As String = Nothing
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dtVal)
        If dtVal.Rows.Count > 0 Then
            retValue = dtVal.Rows(0).Item(0).ToString
        End If
        Return retValue
    End Function

    Private Sub btnBrowseDest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDest.Click
        Dim openFolderDia As New FolderBrowserDialog
        If openFolderDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtDestinationPath.Text = openFolderDia.SelectedPath
            If UCase(ServerName) = UCase(My.Computer.Name) Or ServerName = "." Or ServerName = Nothing Then
                If txtDestinationPath.Text.Contains("\\") Then
                    MsgBox("Select Local Path", MsgBoxStyle.Exclamation)
                    txtDestinationPath.Clear()
                    Exit Sub
                End If
            Else
                If txtDestinationPath.Text.Contains("\\") = False Then
                    MsgBox("Select \\" + ServerName + " Path Only", MsgBoxStyle.Exclamation)
                    txtDestinationPath.Clear()
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub btnAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAttach.Click
        Try
            If txtDestinationPath.Text = "" Then
                MsgBox("Destination Path Should Not Empty", MsgBoxStyle.Information)
                txtDestinationPath.Focus()
                Exit Sub
            ElseIf Directory.Exists(txtDestinationPath.Text) = False Then
                MsgBox("Invalid Destination Path", MsgBoxStyle.Information)
                txtDestinationPath.Focus()
                Exit Sub
            End If
            If UCase(ServerName) = UCase(My.Computer.Name) Or ServerName = "." Or ServerName = Nothing Then
                If txtDestinationPath.Text.Contains("\\") Then
                    MsgBox("Select Local Path", MsgBoxStyle.Exclamation)
                    txtDestinationPath.Clear()
                    Exit Sub
                End If
            Else
                If txtDestinationPath.Text.Contains("\\") = False Then
                    MsgBox("Select \\" + ServerName + " Path Only", MsgBoxStyle.Exclamation)
                    txtDestinationPath.Clear()
                    Exit Sub
                End If
            End If
            btnAttach.Enabled = False
            Dim fil As FileInfo
            Dim file1Path As String
            Dim file2Path As String
            fil = New FileInfo(dtFileView.Rows(0).Item("FILELOCATION(S)").ToString)
            If UCase(fil.DirectoryName) <> UCase(txtDestinationPath.Text) Then
                If File.Exists(txtDestinationPath.Text + "\" + fil.Name) Then
                    File.Delete(txtDestinationPath.Text + "\" + fil.Name)
                End If
                File.Move(fil.FullName, txtDestinationPath.Text + "\" + fil.Name)
            End If
            fil = New FileInfo(dtFileView.Rows(1).Item("FILELOCATION(S)").ToString)
            If UCase(fil.DirectoryName) <> UCase(txtDestinationPath.Text) Then
                If File.Exists(txtDestinationPath.Text + "\" + fil.Name) Then
                    File.Delete(txtDestinationPath.Text + "\" + fil.Name)
                End If
                File.Move(fil.FullName, txtDestinationPath.Text + "\" + fil.Name)
            End If

            file1Path = txtDestinationPath.Text + "\" + dtFileView.Rows(0).Item("FILENAME(S)").ToString
            If file1Path.Contains("\\") Then
                file1Path = file1Path.Replace("\\", "")
                file1Path = file1Path.Substring(file1Path.IndexOf("\") + 1)
                file1Path = file1Path.Insert(1, ":")
                If file1Path.Contains("\") = False Then file1Path += "\"
            End If


            strSql = " SP_ATTACH_DB " + Trim(dtFileView.Rows(0).Item("FILENAME(S)").ToString.Replace(".mdf", ""))
            strSql += " ,@FILENAME1 = '" & file1Path & "'"
            If dtFileView.Rows(0).Item("FILELOCATION(S)").ToString <> "" Then
                file2Path = txtDestinationPath.Text + "\" + dtFileView.Rows(1).Item("FILENAME(S)").ToString
                If file2Path.Contains("\\") Then
                    file2Path = file2Path.Replace("\\", "")
                    file2Path = file2Path.Substring(file2Path.IndexOf("\") + 1)
                    file2Path = file2Path.Insert(1, ":")
                    If file2Path.Contains("\") = False Then file2Path += "\"
                End If
                strSql += " ,@FILENAME2 = '" & file2Path & "'"
            End If
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            MsgBox("Task Completed")
            Me.Close()

            'strSql = " dbcc checkprimaryfile (N'" & txtSourcePath.Text & "', 2)"
            'Dim dtDBdetail As New DataTable
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dtDBdetail)
            'If Not dtDBdetail.Rows.Count > 0 Then
            '    MsgBox("Invalid Database Detail", MsgBoxStyle.Information)
            '    Exit Sub
            'End If
            'file1Path = dtFileView.Rows(0).Item("FILELOCATION(S)").ToString
            'file2Path = dtFileView.Rows(1).Item("FILELOCATION(S)").ToString
            'If File.Exists(file1Path) Then
            '    fil = New FileInfo(file1Path)
            '    If UCase(fil.DirectoryName) <> UCase(txtDestinationPath.Text) Then
            '        File.Move(file1Path, txtDestinationPath.Text + "\" + fil.Name)
            '        file1Path = txtDestinationPath.Text + "\" + fil.Name
            '    End If
            'End If

            'If File.Exists(file2Path) Then
            '    fil = New FileInfo(file2Path)
            '    If UCase(fil.DirectoryName) <> UCase(txtDestinationPath.Text) Then
            '        File.Move(file2Path, txtDestinationPath.Text + "\" + fil.Name)
            '        file2Path = txtDestinationPath.Text + "\" + fil.Name
            '    End If
            'End If

            'strSql = " SP_ATTACH_DB " + dtDBdetail.Rows(0).Item("value").ToString
            'strSql += " ,@FILENAME1 = '" & file1Path & "'"
            'If dtFileView.Rows(0).Item("FILELOCATION(S)").ToString <> "" Then
            '    strSql += " ,@FILENAME2 = '" & file2Path & "'"
            'End If
            'cmd = New OleDbCommand(strSql, cn)
            'cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            btnAttach.Enabled = True
        End Try
    End Sub


End Class