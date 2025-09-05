Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Imports System.Net.Mail
Public Class FrmDailyAuditReport
    Dim dtuser As New DataTable
    Dim strsql As String = ""
    Dim title As String = ""

    Private Sub NewToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem1.Click
        FuncNew()
    End Sub

    Private Sub ExitToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem2.Click
        Me.Close()
    End Sub

    Private Sub FrmDailyAuditReport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub FrmDailyAuditReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FuncNew()
    End Sub

    Private Sub btnview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnview.Click
        FuncView()
    End Sub

    Private Sub btnnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew.Click
        FuncNew()
    End Sub

    Private Sub btnsendmail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsendmail.Click
        Try
            Dim msg As Boolean = True
            If GridView.Rows.Count > 0 Then
                If MsgBox("Are you sure want to send mail.", MsgBoxStyle.YesNo, "Brighttech asking") = MsgBoxResult.No Then Exit Sub
                Dim mailid As String = GetAdmindbSoftValue("REPORTING_MAILID", "")
                mailid = mailid.ToLower()
                Dim ToMail() As String
                ToMail = mailid.Split(",")
                If ToMail.Length < 0 Then MsgBox("Pls add mail Id..") : Exit Sub
                For i As Integer = 0 To ToMail.Length - 1
                    If ((ToMail(i).Contains("@") And ToMail(i).Contains("com")) Or (ToMail(i).Contains("@") And (ToMail(i).Contains("net") Or ToMail(i).Contains("NET")))) = False Then
                        MsgBox(ToMail(i) & " is not valid mail Id..")
                        GoTo exitt
                    End If
                    If NEWMAILSEND(ToMail(i), GenerateHTMLreport(), Application.StartupPath & "\DAILYREPORT.PDF") = 0 Then
                        msg = False
                    End If
                Next
                If msg = True Then MsgBox("Mail Send successfully..")
            Else
                MsgBox("No detail is there to send")
            End If
exitt:
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnexit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Me.Close()
    End Sub

    Private Sub FuncNew()
        dtpEditDate.Value = GetServerDate()
        strSql = " SELECT 'ALL' USERNAME,'ALL' USERID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT USERNAME,CONVERT(vARCHAR,USERID),2 RESULT FROM " & cnAdminDb & "..USERMASTER"
        strSql += " ORDER BY RESULT,USERNAME"
        dtuser = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtuser)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbUserName_own, dtuser, "USERNAME", , "ALL")
        GridView.DataSource = Nothing
        dtpEditDate.Focus()
    End Sub

    Private Sub FuncView()
        Dim checkDatabase As Integer
        strsql = " select count(*) cnt from sys.databases where name = '" & strCompanyId & "AUDITLOG' "
        checkDatabase = Val(GetSqlValue(cn, strsql).ToString)
        If checkDatabase = 0 Then
            MsgBox("This Report cannot View Required database " & vbCrLf & "" & strCompanyId & "AUDITLOG")
            dtpEditDate.SelectAll()
            dtpEditDate.Focus()
            Exit Sub
        End If

        Dim userid As String
        title = dtpEditDate.Value.ToString("dd/MM/yyyy")
        If chkCmbUserName_own.Text <> "ALL" Then
            userid = GetSelectedUserId(chkCmbUserName_own, True)
        Else
            userid = "'ALL'"
        End If
        strsql = "EXEC " & cnAdminDb & "..COMPARE_AUDIT_VS_MASTER "
        strsql += vbCrLf + "@AUDITDB='" & Replace(cnAdminDb, "ADMINDB", "AUDITLOG") & "'"
        strsql += vbCrLf + ",@DBNAME='" & cnStockDb & "'"
        strsql += vbCrLf + ",@EDITDATE='" & dtpEditDate.Value.ToString("yyyy/MM/dd") & "'"
        strsql += vbCrLf + ",@USER=" & userid & ""
        strsql += vbCrLf + ",@WITHVALUE='" & IIf(chkwithvalue.Checked, "Y", "N") & "'"
        Dim ds As New DataSet
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(ds)
        If ds.Tables(0).Rows.Count > 0 Then
            GridView.DataSource = ds.Tables(0)
            If chkwithvalue.Checked = False Then
                GridView.Columns("USERNAME").Width = 120
                GridView.Columns("TABLENAME").Width = 120
                GridView.Columns("DESCRIPTION").Width = 773
            Else
                GridView.Columns("USERNAME").Width = 120
                GridView.Columns("TABLENAME").Width = 120
                GridView.Columns("COLUMNNAME").Width = 120
                GridView.Columns("OLDVALUE").Width = 325
                GridView.Columns("NEWVALUE").Width = 325

            End If
        Else
            GridView.DataSource = Nothing
            MsgBox("No Records found.")
        End If
    End Sub

    Function GenerateHTMLreport() As String
        Dim hdt As New DataTable
        hdt = CType(GridView.DataSource, DataTable)
        If hdt.Rows.Count > 0 Then
            Dim HM As String
            'BORDER=3 WIDTH=100%  CELLPADDING=1 CELLSPACING=1
            HM = " <TABLE cellspacing=""0"" cellpadding=""4"" border=""1"" align=""center"" WIDTH=100% border-style:""thin solid"">"
            HM += "<TR>"
            HM += "<TH FONT SIZE=""2"" BGCOLOR=""#C2C67E"" ><FONT COLOR=""660000""><H3>DAILY AUDIT REPORT " & vbCrLf & dtpEditDate.Value.ToString("dd/MM/yyyy") & "</H3>"
            HM += "</TH>"
            HM += "</TR>"
            HM += "</TABLE>"
            ' BORDER=5 WIDTH=100% CELLPADDING=4 CELLSPACING=3
            HM += "<TABLE cellspacing=""0"" cellpadding=""4"" border=""1"" align=""center"" WIDTH=100% border-style:""thin solid"">"
            HM += "<TR>"
            HM += "<TH FONT SIZE=""2"" BGCOLOR=""#C2C67E""><FONT COLOR=""660000""><H5>USERNAME</H5>"
            HM += "<TH FONT SIZE=""2"" BGCOLOR=""#C2C67E""><FONT COLOR=""660000""><H5>TABLENAME</H5>" '--
            HM += "<TH FONT SIZE=""2"" BGCOLOR=""#C2C67E""><FONT COLOR=""660000""><H5>DESCRIPTION</H5>" '--
            HM += "</TH>"
            HM += "</TR>"
            Dim _particular As String = ""
            Dim _wieght As String = ""
            Dim _amount As String = ""
            Dim I As Integer = 0
            While I <> hdt.Rows.Count
                _particular = hdt.Rows(I)("USERNAME").ToString
                _wieght = hdt.Rows(I)("TABLENAME").ToString
                _amount = hdt.Rows(I)("DESCRIPTION").ToString

                HM += "<TR >"
                'HM += "<TD " & IIf((hdt.Rows(I)("COLHEAD").ToString = "TT" Or hdt.Rows(I)("COLHEAD").ToString = "S1"), "BGCOLOR=""#C1CFDC"" ALIGN= LEFT> <FONT size=2> <FONT COLOR=""#3300FF"">", IIf(hdt.Rows(I)("COLHEAD").ToString = "S", "><b>", ">")) + _particular + "</TD>"
                'HM += "<TD ALIGN=" & IIf((hdt.Rows(I)("COLHEAD").ToString = "TT" Or hdt.Rows(I)("COLHEAD").ToString = "S1"), "CENTRE", "RIGHT") & ">" & _wieght & "</TD>"
                'HM += "<TD ALIGN=" & IIf((hdt.Rows(I)("COLHEAD").ToString = "TT" Or hdt.Rows(I)("COLHEAD").ToString = "S1"), "CENTRE", "RIGHT") & ">" & _amount & "</TD>"


                HM += "<TD> " + _particular + "</TD>"
                HM += "<TD> " & _wieght & "</TD>"
                HM += "<TD> " & _amount & "</TD>"

                HM += "</TR>" + vbCrLf
                I = I + 1
            End While
            HM += "</TABLE>"
            Return HM
        End If
    End Function

    Function NEWMAILSEND(ByVal ToMail As String, ByVal MESSAGE As String, Optional ByVal Attachpath As String = "")
        If ToMail.Trim = "" Then Return 0 : Exit Function
        If MESSAGE.Trim = "" Then Return 0 : Exit Function
        Dim smtpServer As New SmtpClient()
        Dim mail As New MailMessage
        Dim Counter As Integer = 0

        Dim FromId As String = Nothing
        Dim MailServer As String = Nothing
        Dim Password As String = Nothing
        Try
            FromId = GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILSERVER'")
            Password = GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILPASSWORD'")
            Password = BrighttechPack.Methods.Decrypt(Password)
            Dim MailServer1 As String = Nothing
            Dim MailServer2 As String = Nothing
            If FromId.Contains("@") = True Then
                Dim SplitMailServer() As String = Split(FromId, "@")
                If Not SplitMailServer Is Nothing Then
                    MailServer1 = SplitMailServer(0)
                    MailServer2 = Trim(SplitMailServer(1))
                    MailServer2 = "@" & MailServer2
                End If
            End If
            If Trim(MailServer2) = "@gmail.com" Then
                smtpServer.Host = "smtp.gmail.com"
                smtpServer.Port = 587
                smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@yahoo.com" Then
                smtpServer.Port = 465
                smtpServer.Host = "smtp.mail.yahoo.com"
                smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@yahoo.co.in" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.mail.yahoo.com"
            ElseIf Trim(MailServer2) = "@hotmail.com" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.live.com"
                smtpServer.EnableSsl = True
            Else
                smtpServer.Port = GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-PORT'")
                smtpServer.Host = GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-HOSTNAME'")
                smtpServer.EnableSsl = IIf(GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-SSL'").ToString.ToUpper() = "Y", True, False)
            End If
            If FromId = "" Then MsgBox("Sender Id is Empty", MsgBoxStyle.Information) : Return 0
            mail.From = New MailAddress(FromId)
            mail.To.Add(New MailAddress(ToMail))
            mail.Subject = "DAILY AUDIT REPORT "
            mail.Body = MESSAGE
            mail.IsBodyHtml = True

            'If File.Exists(Attachpath) = True Then mail.Attachments.Add(New Attachment(Attachpath))
            smtpServer.Credentials = New Net.NetworkCredential(FromId.Trim.ToString, Password.Trim.ToString)
            smtpServer.Timeout = 400000
            smtpServer.Send(mail)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return 0
        End Try
        Return 1
    End Function

    Private Sub chkwithvalue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkwithvalue.CheckedChanged
        If chkwithvalue.Checked Then
            btnsendmail.Enabled = False
            GridView.DataSource = Nothing
        Else
            btnsendmail.Enabled = True
            GridView.DataSource = Nothing
        End If
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        If GridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "DAILY AUDIT REPORT-" & title, GridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
End Class