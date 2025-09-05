Imports System.Data.OleDb
Imports System.Net.Mail
Imports System.IO


Public Class rptSendMail
    Dim strSql As String
    Dim cmd As OleDbCommand
    Public rptDate As Date
    Private Sub SendMail(ByVal msgTxt As String, Optional ByVal msgSubject As String = Nothing)
        Dim ToMail As String = ""
        Dim MESSAGE As String = ""
        Dim Attachpath As String = ""
        Dim obj As System.Web.Mail.SmtpMail
        Dim smtpServer As New SmtpClient()
        Dim mail As New MailMessage
        Dim Counter As Integer = 0

        Dim FromId As String = Nothing
        Dim MailServer As String = Nothing
        Dim Password As String = Nothing
        Dim MailTag As String = Nothing
        Try
            FromId = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILSERVER'", "CTLTEXT", , ).ToString.ToLower()
            ToMail = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'REPORTING_MAILID'", "CTLTEXT", , ).ToString.ToLower()
            If FromId.Contains("@") = False Or FromId.Contains(".") = False Then
                FromId = objGPack.GetSqlValue("SELECT CTLNAME FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'FROMMAILSERVER'", "CTLNAME", , ).ToString.ToLower()
            End If
            Password = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILPASSWORD'", "CTLTEXT", , )
            Password = BrighttechPack.Methods.Decrypt(Password)

            If FromId = "" Then MsgBox("From Mail Id is Empty") : Exit Sub
            If Password = "" Then MsgBox("Mail Password is Empty") : Exit Sub
            If ToMail = "" Then MsgBox("To Mail Id is Empty") : Exit Sub
            Dim TomailIds As String()
            TomailIds = Split(ToMail, ",")

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
                ' smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@hotmail.com" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.live.com"
                smtpServer.EnableSsl = True
            Else
                smtpServer.Port = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-PORT'", "CTLTEXT", , ).ToString)
                smtpServer.Host = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-HOSTNAME'", "CTLTEXT", , ).ToString
                smtpServer.EnableSsl = IIf(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-SSL'", "CTLTEXT", , ).ToString.ToUpper() = "Y", True, False)
            End If

            mail.From = New MailAddress(FromId)
            mail.To.Add(New MailAddress(ToMail))
            'If TomailIds.Length > 1 Then mail.CC.Add(New MailAddress(TomailIds(1).ToString))
            mail.Subject = IIf(msgSubject <> "", msgSubject, "Daily Report " & rptDate)
            mail.Body = msgTxt
            mail.IsBodyHtml = True
            'If File.Exists(Attachpath) = True Then mail.Attachments.Add(New Attachment(Attachpath))
            smtpServer.Credentials = New Net.NetworkCredential(FromId.Trim.ToString, Password.Trim.ToString)
            smtpServer.Timeout = 400000
            smtpServer.Send(mail)

        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace, MsgBoxStyle.Information, "Brighttech")
        End Try
    End Sub
    Public Function funcReportDailyTran()
        Dim strNodeId As String
        Dim strCashCounter As String
        Dim strCostName As String

        SenDAutoMail_Rpt()

        strSql = "SELECT DISTINCT SYSTEMID FROM ( "
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ISSUE UNION ALL "
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..RECEIPT  UNION ALL "
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN )X "
        strSql += " ORDER BY SYSTEMID "
        da = New OleDbDataAdapter(strSql, cn)
        Dim dt As DataTable
        dt = New DataTable
        da.Fill(dt)
        strNodeId = ""
        If dt.Rows.Count > 0 Then
            For cnt As Integer = 0 To dt.Rows.Count - 1
                If dt.Rows(cnt).Item(0).ToString <> "" Then strNodeId += "," & dt.Rows(cnt).Item(0).ToString
            Next
        End If
        strSql = "SELECT CASHID, CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME <> '' ORDER BY CASHNAME"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtCashCounter As DataTable
        dtCashCounter = New DataTable
        da.Fill(dtCashCounter)
        strCashCounter = ""
        If dtCashCounter.Rows.Count > 0 Then
            For cnt As Integer = 0 To dtCashCounter.Rows.Count - 1
                If dtCashCounter.Rows(cnt).Item(0).ToString <> "" Then
                    If cnt = 0 Then
                        strCashCounter += dtCashCounter.Rows(cnt).Item(1).ToString
                    ElseIf cnt = dtCashCounter.Rows.Count - 1 Then
                        strCashCounter += dtCashCounter.Rows(cnt).Item(1).ToString & ",ALL"
                    Else
                        strCashCounter += "," & dtCashCounter.Rows(cnt).Item(1).ToString
                    End If
                End If

            Next
        End If

        strSql = " SELECT DISTINCT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE  "
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        strCostName = ""
        If dt.Rows.Count > 0 Then
            For cnt As Integer = 0 To dt.Rows.Count - 1
                If dt.Rows(cnt).Item(0).ToString <> "" Then strCostName += "," & dt.Rows(cnt).Item(0).ToString
            Next
        End If

        strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_TRANSACTIONSUMMARY"
        strSql += vbCrLf + " @FROMDATE = '" & rptDate & "'"
        strSql += vbCrLf + " ,@TODATE = '" & rptDate & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & strCostName & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " ,@SYSTEMID = '" & strNodeId & "'"
        strSql += vbCrLf + " ,@CASHNAME = '" & strCashCounter & "'"
        strSql += vbCrLf + " ,@CASHOPENING = 'Y'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Dim sendTxt As String = ""
            sendTxt = "<html><body bgcolor=#FFFFFF><table align=center width=100% bgcolor=#FFFFFF>"
            sendTxt += "<table border=1 align=center id=tblprint style=width: 100%; height: 100%>"
            Dim html As String = ""
            html += "<tr bgcolor=#AF9B60 style=""font-family:FANTASY; font-size:11pt"">"
            For i As Integer = 0 To dt.Columns.Count - 4
                html += "<td>" + dt.Columns(i).ColumnName + "</td>"
            Next
            html += "</tr>"
            For i As Integer = 0 To dt.Rows.Count - 1
                If dt.Rows(i)("COLHEAD").ToString = "T" Then
                    html += "<tr bgcolor=#F3E5AB style=""font-family:FANTASY; font-size:9pt"">"
                ElseIf dt.Rows(i)("COLHEAD").ToString = "S" Then
                    html += "<tr bgcolor=#F5F5DC style=""font-family:FANTASY; font-size:9pt"">"
                ElseIf dt.Rows(i)("COLHEAD").ToString = "G" Then
                    html += "<tr bgcolor=#AF9B60 style=""font-family:FANTASY; font-size:9pt"">"
                Else
                    html += "<tr bgcolor=#F5F5DC style=""font-family:FANTASY; font-size:9pt"">"
                End If
                For j As Integer = 0 To dt.Columns.Count - 4
                    If Val(dt.Rows(i)(j).ToString()) = 0 Then
                        html += "<td>" + dt.Rows(i)(j).ToString() + "</td>"
                    Else
                        html += "<td align=""right"">" + dt.Rows(i)(j).ToString() + "</td>"
                    End If
                Next
                html += "</tr>"
            Next
            html += "</table>"
            sendTxt += html
            SendMail(sendTxt)
        End If
    End Function
    Private Function ConvertDataTableToHTML(ByVal dt As DataTable)
        Dim html As String = ""
        html += "<tr bgcolor=#AF9B60 style=""font-family:FANTASY; font-size:11pt"">"
        For i As Integer = 0 To dt.Columns.Count - 4
            html += "<td>" + dt.Columns(i).ColumnName + "</td>"
        Next
        html += "</tr>"
        For i As Integer = 0 To dt.Rows.Count - 1
            html += "<tr bgcolor=#F5F5DC style=""font-family:FANTASY; font-size:9pt"">"
            For j As Integer = 0 To dt.Columns.Count - 1
                If Val(dt.Rows(i)(j).ToString()) = 0 Then
                    html += "<td>" + dt.Rows(i)(j).ToString() + "</td>"
                Else
                    html += "<td align=""right"">" + dt.Rows(i)(j).ToString() + "</td>"
                End If
            Next
            html += "</tr>"
        Next
        html += "</table>"
        Return html
    End Function
    Public Function ConvertDataGridViewToHTML(ByVal dgv As DataGridView, Optional ByVal dgvHdr As DataGridView = Nothing)
        Dim html As String = ""
        If dgvHdr.Columns.Count > 0 Then
            html = "<html><body bgcolor=#FFFFFF><table align=center width=100% bgcolor=#FFFFFF>"
            html += "<table border=1 align=center id=tblprint style=width: 100%; height: 100%>"
            html += "<tr bgcolor=#AF9B60 style=""font-family:Baskerville; font-size:11pt"">"
            For i As Integer = 0 To dgv.Columns.Count - 4
                html += "<td>" + dgv.Columns(i).HeaderText + "</td>"
            Next
            html += "</tr>"
            html += "</table>"
        End If
        html += "<html><body bgcolor=#FFFFFF><table align=center width=100% bgcolor=#FFFFFF>"
        html += "<table border=1 align=center id=tblprint style=width: 100%; height: 100%>"
        html += "<tr bgcolor=#AF9B60 style=""font-family:Baskerville; font-size:11pt"">"
        For i As Integer = 0 To dgv.Columns.Count - 4
            html += "<td>" + dgv.Columns(i).HeaderText + "</td>"
        Next
        html += "</tr>"
        For i As Integer = 0 To dgv.Rows.Count - 1
            If dgv.Rows(i).Cells("COLHEAD").ToString = "T" Then
                html += "<tr bgcolor=#F3E5AB style=""font-family:Baskerville; font-size:9pt"">"
            ElseIf dgv.Rows(i).Cells("COLHEAD").ToString = "S" Then
                html += "<tr bgcolor=#F5F5DC style=""font-family:Baskerville; font-size:9pt"">"
            ElseIf dgv.Rows(i).Cells("COLHEAD").ToString = "G" Then
                html += "<tr bgcolor=#AF9B60 style=""font-family:Baskerville; font-size:9pt"">"
            Else
                html += "<tr bgcolor=#F5F5DC style=""font-family:Baskerville; font-size:9pt"">"
            End If
            For j As Integer = 0 To dgv.Columns.Count - 4
                If Val(dgv.Rows(i).Cells(j).Value.ToString()) = 0 Then
                    html += "<td>" + dgv.Rows(i).Cells(j).Value.ToString() + "</td>"
                Else
                    html += "<td align=""right"">" + dgv.Rows(i).Cells(j).Value.ToString() + "</td>"
                End If
            Next
            html += "</tr>"
        Next
        html += "</table>"
        Return html
    End Function
    Public Function SendAutoMail_Rpt(Optional ByVal trptdate As Date = Nothing) As Integer
        Dim obj_DailyAbs As New BrighttechREPORT.frmDailyAbstract()
        Dim HTMLstr As String = obj_DailyAbs.SendMailRpt(trptdate)
        If trptdate = Nothing Then
            If HTMLstr <> "" Then SendMail(HTMLstr, "DAILY ABSTRACT - " & Today.Date.ToString("dd-MM-yyyy"))
        Else
            If HTMLstr <> "" Then SendMail(HTMLstr, "DAILY ABSTRACT - " & Format(trptdate, "dd-MM-yyyy").ToString)
        End If
    End Function
End Class
