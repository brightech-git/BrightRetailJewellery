Imports System.IO
Imports System.Data.OleDb
Public Class frmSign
    Dim DbType As String
    Dim ServerName As String
    Dim Pwd As String
    Dim Pwd1 As String
    Dim DbPath As String
    Dim cn As New OleDbConnection
    Dim Cmd As New OleDbCommand
    Dim da As New OleDbDataAdapter
    Dim strSql As String
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Public Sub New(ByVal ServerName As String, ByVal Dbpath As String _
    , ByVal Pwd As String, ByVal dbtype As String, ByVal SmsUrl As String _
    , ByVal MailId As String, ByVal MailPwd As String, ByVal SmtpHost As String _
    , ByVal SmtpPort As Integer, ByVal SmtpSsl As Boolean, ByVal OptinUrl As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        txtDbserver.Text = ServerName
        txtDbPath.Text = Dbpath
        txtSqlPwd.Text = Pwd
        cmbLogintype.Text = dbtype
        txtSqluser.Text = IIf(dbtype = "SQL MODE", "sa", "")
        txtSmsUrl.Text = SmsUrl
        txtMailId.Text = MailId
        txtMailPwd.Text = MailPwd
        txtHostName.Text = SmtpHost
        txtSmtpPort.Text = SmtpPort
        ChkSSL.Checked = SmtpSsl
        txtSmsOptinUrl.Text = OptinUrl
    End Sub
    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If txtDbserver.Text = "" Then txtDbserver.Focus() : Exit Sub
        ServerName = txtDbserver.Text
        DbPath = txtDbPath.Text
        Pwd1 = txtSqlPwd.Text.Trim
        Pwd = IIf(txtSqlPwd.Text.Trim <> "", Encrypt(txtSqlPwd.Text.Trim), "")
        Dim Url As String = txtSmsUrl.Text
        Dim OptinUrl As String = txtSmsOptinUrl.Text
        Dim FromMail As String = txtMailId.Text
        Dim MailPwd As String = IIf(txtMailPwd.Text.Trim <> "", Encrypt(txtMailPwd.Text.Trim), "")
        If cmbLogintype.Text = "SQL MODE" Then
            DbType = "S"
        Else
            DbType = "W"
        End If
        Dim filePath As String = Application.StartupPath + "\SmsServer.ini"
        If IO.File.Exists(filePath) = True Then
            IO.File.Delete(filePath)
        End If
        Dim write As IO.StreamWriter
        write = IO.File.CreateText(filePath)
        write.WriteLine(LSet("Database Server    :", 20) & ServerName)
        write.WriteLine(LSet("DB Login Type      :", 20) & DbType)
        write.WriteLine(LSet("Password           :", 20) & Pwd)
        write.WriteLine(LSet("DbPath             :", 20) & DbPath)
        write.WriteLine(LSet("SMS Mode(W/M)      :", 20) & "W")
        write.WriteLine(LSet("SMS Web URL        :", 20) & Url)
        write.WriteLine(LSet("From Mail Id       :", 20) & FromMail)
        write.WriteLine(LSet("From Mail Pwd      :", 20) & MailPwd)
        write.WriteLine(LSet("Smtp HostName      :", 20) & txtHostName.Text)
        write.WriteLine(LSet("Smtp Port No       :", 20) & Val(txtSmtpPort.Text))
        write.WriteLine(LSet("Enable SSL         :", 20) & IIf(ChkSSL.Checked, "Y", "N"))
        write.WriteLine(LSet("SMS Optin URL      :", 20) & OptinUrl)
        write.Flush()
        write.Close()
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
        Try
            GetConnString()
        Catch ex As Exception
            MsgBox(ex.Message + ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Sub
    Function GetConnString()
        If DbType = "W" Then
            cn = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=;Data Source=" & serverName & "")
        Else
            cn = New OleDbConnection("Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & IIf(DbType.ToUpper = "S", "SA", DbType.ToUpper) & ";Initial Catalog=;Data Source=" & ServerName & ";password=" & Pwd1)
        End If
        cn.Open()
        funcCreateDb()
    End Function
    Function funcCreateDb()
        If DbChecker("AKSHAYASMSDB") = 0 Then
            CreateDb("AKSHAYASMSDB")
        End If
        If TblChecker("SMSDATA") = 0 Then funcCreateTableSMSDATA("AKSHAYASMSDB")
        If TblChecker("MAILDATA") = 0 Then funcCreateTableMAILDATA("AKSHAYASMSDB")
    End Function
    Function funcCreateTableSMSDATA(ByVal dbname As String)
        strSql = vbCrLf + " CREATE TABLE " & dbname & "..SMSDATA"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "  SERNO INT IDENTITY(1,1)"
        strSql += vbCrLf + "  ,MOBILENO VARCHAR(50)"
        strSql += vbCrLf + "  ,MESSAGES VARCHAR(250)"
        strSql += vbCrLf + "  ,STATUS VARCHAR(1)"
        strSql += vbCrLf + "  ,UPDATED SMALLDATETIME"
        strSql += vbCrLf + "  ,EXPIRYDATE SMALLDATETIME"
        strSql += vbCrLf + "  ,SENTUPDATED SMALLDATETIME"
        strSql += vbCrLf + "  ,REMARKS VARCHAR(50)"
        strSql += vbCrLf + "  ,BATCHNO VARCHAR(15)"
        strSql += vbCrLf + "  )"
        Cmd = New OleDbCommand(strSql, cn)
        Cmd.ExecuteNonQuery()
    End Function
    Function funcCreateTableMAILDATA(ByVal dbname As String)
        strSql = vbCrLf + " CREATE TABLE " & dbname & "..MAILDATA"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "  SERNO INT IDENTITY(1,1)"
        strSql += vbCrLf + "  ,MAILID VARCHAR(50)"
        strSql += vbCrLf + "  ,SUBJECT VARCHAR(100)"
        strSql += vbCrLf + "  ,MESSAGES VARCHAR(250)"
        strSql += vbCrLf + "  ,STATUS VARCHAR(1)"
        strSql += vbCrLf + "  ,UPDATED SMALLDATETIME"
        strSql += vbCrLf + "  ,EXPIRYDATE SMALLDATETIME"
        strSql += vbCrLf + "  ,SENTUPDATED SMALLDATETIME"
        strSql += vbCrLf + "  ,REMARKS VARCHAR(50)"
        strSql += vbCrLf + "  ,BATCHNO VARCHAR(15)"
        strSql += vbCrLf + "  ,FILEPATH VARCHAR(100)"
        strSql += vbCrLf + "  )"
        Cmd = New OleDbCommand(strSql, cn)
        Cmd.ExecuteNonQuery()
    End Function
    Function DbChecker(ByVal dbname As String) As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " SELECT NAME FROM MASTER..SYSDATABASES WHERE NAME = '" & dbname & "'"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(Cmd)
        da.Fill(ds, "SYSDATABASES")
        If ds.Tables("SYSDATABASES").Rows.Count > 0 Then
            Return 1
        End If
        Return 0
    End Function
    Function TblChecker(ByVal TblName As String) As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " SELECT NAME FROM AKSHAYASMSDB..SYSOBJECTS WHERE NAME = '" & TblName & "'"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(Cmd)
        da.Fill(ds, "SYSOBJECTS")
        If ds.Tables("SYSOBJECTS").Rows.Count > 0 Then
            Return 1
        End If
        Return 0
    End Function
    Function CreateDb(ByVal dbName As String) As Integer
        strSql = " CREATE DATABASE " & dbName & ""
        If dbPath <> "" Then
            strSql += vbCrLf + "  On (Name = " & dbName & "_dat,FILENAME = '" & dbPath & "\" & dbName & ".mdf')"
            strSql += vbCrLf + "  Log On (Name = '" & dbName & "_log',FILENAME = '" & dbPath & "\" & dbName & "_log.ldf')"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        Return 1
    End Function
    Public Shared Function Encrypt(ByVal Pwd As String) As String
        Dim strEncryptPwd As String = Nothing
        Try
            For cnt As Integer = 1 To Len(Pwd)
                Dim IntAscii As Integer = 0
                IntAscii = (Asc(Strings.Mid(Pwd, cnt, 1)) + (cnt * 2) + 14)
                strEncryptPwd = strEncryptPwd & Chr(IntAscii)
            Next
        Catch ex As Exception
            MsgBox("ERROR :" & ex.Message & vbCrLf & vbCrLf & "POSITION :" & ex.StackTrace)
        End Try
        Return strEncryptPwd

    End Function
    Private Sub frmSign_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmSign_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtSqlPwd.CharacterCasing = CharacterCasing.Normal
    End Sub
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class