Imports System.Data.OleDb
Imports System.IO

Public Class DB_Detach
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim cn As OleDbConnection

    Dim cnCompanyId As String = Nothing
    Dim cnCompanyName As String = Nothing
    Dim ServerName As String = Nothing
    Dim passWord As String = Nothing
    Dim Logintype As String = Nothing

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub Detach_DB_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
                'cn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=;Data Source={0};uid=sa;pwd=;", ServerName))
            Catch ex As Exception
                MsgBox(ServerName + " Connection Failed", MsgBoxStyle.Information)
                MsgBox(ex.Message)
                Exit Sub
            End Try
        Else
            cn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=;Data Source=.;uid=sa;pwd=;"))
            'MsgBox("Connection Info Not Found", MsgBoxStyle.Information)
            'btnDetach.Enabled = False
            'Exit Sub
        End If
        cn.Open()
        Dim dtDb As New DataTable
        strSql = " SELECT NAME FROM SYSDATABASES"
        strSql += " WHERE DBID NOT IN (1,2,3,4)"
        strSql += " ORDER BY NAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDb)
        cmbDbName.Items.Clear()
        For Each ro As DataRow In dtDb.Rows
            cmbDbName.Items.Add(ro!NAME.ToString)
            If UCase(ro!NAME.ToString).StartsWith(UCase(cnCompanyId)) Then
                cmbDbName.Text = ro!NAME.ToString
            End If
        Next
    End Sub

    Private Sub cmbDbName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDbName.SelectedIndexChanged
        If cmbDbName.Text = "" Then Exit Sub
        Dim filPath As String = GetSQl("SELECT FILENAME FROM SYSDATABASES WHERE NAME = '" & cmbDbName.Text & "'")
        Dim fil As FileInfo
        If IO.File.Exists(filPath) Then
            fil = New FileInfo(filPath)
            txtDestPath.Text = fil.DirectoryName
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

    Private Sub btnDetach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDetach.Click

        Dim dbPath As String = UCase(GetSQl("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & cmbDbName.Text & "'"))
        If dbPath = "" Then
            MsgBox("Invalid Database", MsgBoxStyle.Information)
            cmbDbName.Focus()
            Exit Sub
        End If
        If Directory.Exists(txtDestPath.Text) = False Then
            MsgBox("Invalid Destination Path", MsgBoxStyle.Information)
            txtDestPath.Focus()
            Exit Sub
        End If
        Try
            strSql = " SELECT FILENAME FROM " & cmbDbName.Text & "..SYSFILES"
            Dim dtFiles As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtFiles)
            If Not dtFiles.Rows.Count > 0 Then
                MsgBox("SysFiles Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If

            btnDetach.Enabled = False
            strSql = "SP_DETACH_DB " + cmbDbName.Text
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            For Each ro As DataRow In dtFiles.Rows
                Dim fil As FileInfo
                fil = New FileInfo(ro!FILENAME.ToString)
                If ServerName = "." Or ServerName = Nothing Or UCase(ServerName) = UCase(My.Computer.Name) Or ServerName = "" Then
                    If UCase(txtDestPath.Text) <> UCase(fil.DirectoryName) Then
                        Dim destFile As String = txtDestPath.Text + "\" + Trim(fil.Name)
                        File.Move(ro!FILENAME.ToString, destFile)
                    End If
                Else
                    If UCase(txtDestPath.Text) <> UCase("\\" + ServerName + "\" + fil.DirectoryName.Replace(":", "")) Then
                        Dim destFile As String = txtDestPath.Text + "\" + Trim(fil.Name)
                        File.Move(Trim("\\" + ServerName + "\" + ro!FILENAME.ToString.Replace(":", "")), Trim(destFile))
                    End If
                End If
            Next
            MsgBox("Task Completed", MsgBoxStyle.Information)
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            btnDetach.Enabled = True
        End Try
    End Sub

    Private Sub btnBrowseDest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDest.Click
        Dim openFolderDia As New FolderBrowserDialog
        If openFolderDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtDestPath.Text = openFolderDia.SelectedPath
        End If
    End Sub
End Class