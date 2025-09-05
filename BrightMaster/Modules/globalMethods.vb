Imports System.Data.OleDb
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Security.Permissions
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient

Module globalMethods
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String = Nothing
    Dim dt As New DataTable

    <DllImport("advapi32.DLL", SetLastError:=True)> _
    Public Function LogonUser(ByVal lpszUsername As String, ByVal lpszDomain As String, _
        ByVal lpszPassword As String, ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, _
        ByRef phToken As IntPtr) As Integer
    End Function
    
    Public Function WTCEILINGCALC(ByVal OrgValue As Decimal, ByVal RounDeD As Integer) As Decimal
        If OrgValue = 0 Then Return 0 : Exit Function
        Dim returnvalue As Decimal = 0
        Dim abc As New String("0"c, RounDeD)
        abc = "1" + abc.Trim
        Dim abcd As Integer = Val(abc)
        returnvalue = Math.Ceiling((OrgValue * abcd)) / abcd
        Return returnvalue
    End Function



    Public Function NEWMAILSEND(ByVal ToMail As String, ByVal MESSAGE As String, Optional ByVal Success As Boolean = False)
        Dim smtpServer As New SmtpClient()
        Dim mail As New MailMessage
        Dim Counter As Integer = 0
        Dim FromId As String = Nothing
        Dim MailServer As String = Nothing
        Dim Password As String = Nothing
        Try
            FromId = "akshayagoldhelpline@gmail.com"
            Password = "akshaya@123"
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
                smtpServer.Port = 587
                smtpServer.Host = "smtp.gmail.com"
                smtpServer.EnableSsl = False
            End If
            mail.From = New MailAddress(FromId)
            mail.To.Add(New MailAddress(ToMail))
            mail.Subject = "Installation Confirmation "
            mail.Body = MESSAGE
            mail.IsBodyHtml = True
            'If File.Exists(Attachpath) = True Then mail.Attachments.Add(New Attachment(Attachpath))
            smtpServer.Credentials = New Net.NetworkCredential(FromId.Trim.ToString, Password.Trim.ToString)
            smtpServer.Timeout = 400000
            smtpServer.Send(mail)
        Catch ex As Exception
            'MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return 0
        End Try
        Return 1
    End Function
    Public Function SendmailToInternalTrfAccount(ByVal trandate As String, ByVal BatchNo As String) As Integer
        Dim Message As String = ""
        Dim smtpServer As New SmtpClient()
        Dim mail As New MailMessage
        Dim FromId As String = Nothing
        Dim MailServer As String = Nothing
        Dim Password As String = Nothing
        Dim costcentre As String
        Try
            strSql = "SELECT AH.ACCODE,AC.COSTID,AH.EMAILID,convert(varchar,AC.TRANDATE,105)TRANDATE,AC.TRANNO,CASE WHEN AC.TRANMODE='C'THEN 'Cr.'ELSE 'Dr.' END TRANMODE,AC.AMOUNT,AC.CANCEL FROM " & cnStockDb & "..ACCTRAN AC "
            strSql += vbCrLf + " INNER JOIN  " & cnAdminDb & "..ACHEAD AS AH ON AC.ACCODE=AH.ACCODE AND AH.ACTYPE='I' "
            strSql += vbCrLf + " WHERE AC.TRANDATE='" & trandate & "' AND AC.BATCHNO='" & BatchNo & "' AND ISNULL(AH.EMAILID,'')<>''"
            Dim dtsms As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtsms)
            If dtsms.Rows.Count > 0 Then
                For Each ro As DataRow In dtsms.Rows
                    costcentre = objGPack.GetSqlValue("SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & ro!COSTID.ToString & "'")
                    If ro!CANCEL.ToString = "" Then
                        Message = "The Internal Transfer Entry Made from " & costcentre & "." & vbCrLf & "Billno : " & ro!TRANNO.ToString & vbCrLf & "BillDate : " & ro!TRANDATE.ToString & vbCrLf & "and Amount : " & ro!AMOUNT.ToString & " " & ro!TRANMODE.ToString
                    Else
                        Message = "Internal Transfer Entry Cancelled from " & costcentre & "." & vbCrLf & "Billno : " & ro!TRANNO.ToString & vbCrLf & "BillDate : " & ro!TRANDATE.ToString & vbCrLf & "and Amount : " & ro!AMOUNT.ToString & " " & ro!TRANMODE.ToString
                    End If
                    FromId = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILSERVER'")
                    Password = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILPASSWORD'")
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
                        smtpServer.Port = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-PORT'")
                        smtpServer.Host = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-HOSTNAME'")
                        smtpServer.EnableSsl = IIf(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-SSL'").ToString.ToUpper() = "Y", True, False)
                    End If
                    Dim mailid() As String
                    mailid = ro!emailid.ToString.Split(",")
                    For i As Integer = 0 To mailid.Length - 1
                        If i = 0 Then
                            mail.To.Add(New MailAddress(mailid(i).ToString))
                        Else
                            mail.CC.Add(New MailAddress(mailid(i).ToString))
                        End If
                    Next
                    mail.From = New MailAddress(FromId)
                    mail.Subject = "Internal Transfer Entry "
                    mail.Body = Message
                    mail.IsBodyHtml = True
                    'If File.Exists(Attachpath) = True Then mail.Attachments.Add(New Attachment(Attachpath))
                    smtpServer.Credentials = New Net.NetworkCredential(FromId.Trim.ToString, Password.Trim.ToString)
                    smtpServer.Timeout = 400000
                    smtpServer.Send(mail)

                Next
            End If
        Catch ex As Exception
            Return 0
        End Try
        Return 1

    End Function

    Public Function SendmailToInternalTrfAccountold(ByVal billno As String, ByVal billdate As String, ByVal accode As String, ByVal amount As Double, ByVal costid As String, ByVal tranmode As String, ByVal MAILID As String) As Integer
        Dim Message As String = ""
        Dim smtpServer As New SmtpClient()
        Dim mail As New MailMessage
        Dim Counter As Integer = 0
        Dim FromId As String = Nothing
        Dim MailServer As String = Nothing
        Dim Password As String = Nothing
        Dim costcentre As String = objGPack.GetSqlValue("SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & costid & "'")
        Try
            Message = "The Internal Transfer Entry Made from " & costcentre & "." & vbCrLf & "BillDate : " & billdate & vbCrLf & "Billno : " & billno & vbCrLf & "and Amount : " & amount & " " & tranmode

            FromId = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILSERVER'")
            Password = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILPASSWORD'")
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
                smtpServer.Port = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-PORT'")
                smtpServer.Host = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-HOSTNAME'")
                smtpServer.EnableSsl = IIf(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-SSL'").ToString.ToUpper() = "Y", True, False)
            End If
            mail.From = New MailAddress(FromId)
            mail.To.Add(New MailAddress(MAILID))
            mail.Subject = "Internal Transfer Entry "
            mail.Body = Message
            mail.IsBodyHtml = True
            'If File.Exists(Attachpath) = True Then mail.Attachments.Add(New Attachment(Attachpath))
            smtpServer.Credentials = New Net.NetworkCredential(FromId.Trim.ToString, Password.Trim.ToString)
            smtpServer.Timeout = 400000
            smtpServer.Send(mail)
        Catch ex As Exception
            Return 0
        End Try
        Return 1

    End Function

    Public Sub PrintStockTransferOLD(ByVal RefNo As String, ByVal RefDate As Date)
        If RefNo = "" Then Exit Sub
        strSql = " SELECT TOP 1 (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.TCOSTID)aS COSTNAME"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.TCOSTID)aS COSTNAME"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG AS T"
        strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.TCOSTID)aS COSTNAME"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS T"
        strSql += vbCrLf + "  WHERE T.RECISS ='I' AND T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"

        Dim tCostName As String = objGPack.GetSqlValue(strSql)
        If tCostName = "" Then
            Exit Sub
        End If
        Dim UserName As String = Nothing
        Dim UpTime As String = Nothing
        strSql = " SELECT TOP 1 CASE WHEN UPTIME IS NOT NULL THEN SUBSTRING(CONVERT(VARCHAR,UPTIME,113),13,5) ELSE '' END AS UPTIME"
        strSql += vbCrLf + " ,(SELECT TOP 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID)AS USERNAME"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE REFNO = '" & RefNo & "' AND REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " UNION"
        strSql += vbCrLf + " SELECT TOP 1 CASE WHEN UPTIME IS NOT NULL THEN SUBSTRING(CONVERT(VARCHAR,UPTIME,113),13,5) ELSE '' END AS UPTIME"
        strSql += vbCrLf + " ,(SELECT TOP 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID)AS USERNAME"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE REFNO = '" & RefNo & "' AND REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"

        Dim DtTemp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtTemp)
        If DtTemp.Rows.Count > 0 Then
            UserName = DtTemp.Rows(0).Item("USERNAME").ToString
            UpTime = DtTemp.Rows(0).Item("UPTIME").ToString
            If UpTime = "00:00" Then UpTime = ""
        End If


        Dim Detail As Boolean = False
        If MessageBox.Show("Do you want Detailed Stock Transfer Print?", "Stock Transfer Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Detail = True
        End If
        Dim vDtTranTitle1 As New BrightPosting.GDatatable
        vDtTranTitle1.pTableContentAlignment = StringAlignment.Center
        vDtTranTitle1.Columns.Add("DES1", GetType(String))
        Dim Ro As BrightPosting.GDataRow = Nothing
        Ro = vDtTranTitle1.NewRow
        Ro.Item("DES1") = cnCompanyName
        vDtTranTitle1.Rows.Add(Ro)
        vDtTranTitle1.pRowHeight = 21
        vDtTranTitle1.pColHeaderVisible = False
        vDtTranTitle1.pCellBorder = False
        vDtTranTitle1.pContentFont = New Font("VERDANA", 12, FontStyle.Bold)

        Dim vDtTranTitle2 As New BrightPosting.GDatatable
        vDtTranTitle2.pTableContentAlignment = StringAlignment.Center
        vDtTranTitle2.Columns.Add("DES1", GetType(String))


        Ro = vDtTranTitle2.NewRow
        Ro.Item("DES1") = "STOCK TRANSFER FROM " & cnCostName & " TO " & tCostName
        vDtTranTitle2.Rows.Add(Ro)
        vDtTranTitle2.pRowHeight = 21
        vDtTranTitle2.pColHeaderVisible = False
        vDtTranTitle2.pCellBorder = False
        vDtTranTitle2.pContentFont = New Font("verdana", 10, FontStyle.Bold)

        Dim vDtTranTitle3 As New BrightPosting.GDatatable
        vDtTranTitle3.pTableContentAlignment = StringAlignment.Near
        vDtTranTitle3.Columns.Add("DES1", GetType(String))
        Ro = vDtTranTitle3.NewRow
        Ro.Item("DES1") = "USER NAME : " & UserName
        vDtTranTitle3.Rows.Add(Ro)
        Ro = vDtTranTitle3.NewRow
        Ro.Item("DES1") = "TRASFER NO : " & RefNo & " DATE : " & RefDate.ToString("dd/MM/yyyy") & IIf(UpTime <> "", " " & UpTime, "")
        vDtTranTitle3.Rows.Add(Ro)
        vDtTranTitle3.pRowHeight = 21
        vDtTranTitle3.pColHeaderVisible = False
        vDtTranTitle3.pCellBorder = False
        vDtTranTitle3.pContentFont = New Font("verdana", 10, FontStyle.Bold)

        Dim vDtTranInfo1 As New BrightPosting.GDatatable
        Dim vDtTranINfo2 As New BrightPosting.GDatatable
        If Detail Then
            strSql = vbCrLf + "  SELECT IM.ITEMNAME AS ITEM,TAGNO,T.PCS,T.GRSWT,T.NETWT"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN RATE <> 0 THEN T.RATE ELSE NULL END) AS RATE"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN T.SALEMODE IN ('R','F') AND T.SALVALUE <> 0 THEN T.SALVALUE ELSE NULL END) AS SALVALUE"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + "  SELECT IM.ITEMNAME AS ITEM,TAGNO,T.PCS,T.GRSWT,T.NETWT"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN RATE <> 0 THEN T.RATE ELSE NULL END) AS RATE"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN T.SALEMODE IN ('R','F') AND T.SALVALUE <> 0 THEN T.SALVALUE ELSE NULL END) AS SALVALUE"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..CITEMTAG AS T"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + "  SELECT IM.ITEMNAME AS ITEM,'' TAGNO,T.PCS,T.GRSWT,T.NETWT"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN RATE <> 0 THEN T.RATE ELSE NULL END) AS RATE"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),0) AS SALVALUE"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG AS T"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + "  WHERE T.RECISS = 'I' AND T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"

            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(vDtTranInfo1)
            vDtTranInfo1.Columns("ITEM").Caption = 150
            vDtTranInfo1.Columns("TAGNO").Caption = 70
            vDtTranInfo1.Columns("PCS").Caption = 60
            vDtTranInfo1.Columns("GRSWT").Caption = 70
            vDtTranInfo1.Columns("NETWT").Caption = 70
            vDtTranInfo1.Columns("DIAPCS").Caption = 70
            vDtTranInfo1.Columns("DIAWT").Caption = 80
            vDtTranInfo1.Columns("RATE").Caption = 70
            vDtTranInfo1.Columns("SALVALUE").Caption = 90
            vDtTranInfo1.pRowHeight = 21
            vDtTranInfo1.pColHeaderVisible = True
            vDtTranInfo1.pCellBorder = True
            vDtTranInfo1.pCellBorderColor = Pens.LightGray
            vDtTranInfo1.pContentFont = New Font("verdana", 8, FontStyle.Regular)

            vDtTranINfo2 = vDtTranInfo1.Clone
            Ro = vDtTranINfo2.NewRow
            Ro.Item("ITEM") = "TOTAL"
            Ro.Item("PCS") = Val(vDtTranInfo1.Compute("SUM(PCS)", String.Empty).ToString)
            Ro.Item("GRSWT") = Val(vDtTranInfo1.Compute("SUM(GRSWT)", String.Empty).ToString)
            Ro.Item("NETWT") = Val(vDtTranInfo1.Compute("SUM(NETWT)", String.Empty).ToString)
            Ro.Item("DIAPCS") = Val(vDtTranInfo1.Compute("SUM(DIAPCS)", String.Empty).ToString)
            Ro.Item("DIAWT") = Val(vDtTranInfo1.Compute("SUM(DIAWT)", String.Empty).ToString)
            Ro.Item("SALVALUE") = Val(vDtTranInfo1.Compute("SUM(SALVALUE)", String.Empty).ToString)
            vDtTranINfo2.pTableBackColor = Color.LightGoldenrodYellow
            vDtTranINfo2.Rows.Add(Ro)
            vDtTranINfo2.pRowHeight = 21
            vDtTranINfo2.pColHeaderVisible = False
            vDtTranINfo2.pCellBorder = True
            vDtTranINfo2.pCellBorderColor = Pens.LightGray
            vDtTranINfo2.pContentFont = New Font("verdana", 8, FontStyle.Bold)
        Else
            strSql = vbCrLf + "  SELECT ITEM,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT"
            strSql += vbCrLf + "  FROM"
            strSql += vbCrLf + "  ("
            strSql += vbCrLf + "  SELECT IM.ITEMNAME AS ITEM,T.PCS,T.GRSWT,T.NETWT"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT IM.ITEMNAME AS ITEM,T.PCS,T.GRSWT,T.NETWT"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..CITEMTAG AS T"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  )X "
            strSql += vbCrLf + "  GROUP BY ITEM"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(vDtTranInfo1)
            vDtTranInfo1.Columns("ITEM").Caption = 200
            vDtTranInfo1.Columns("PCS").Caption = 60
            vDtTranInfo1.Columns("GRSWT").Caption = 80
            vDtTranInfo1.Columns("NETWT").Caption = 80
            vDtTranInfo1.Columns("DIAPCS").Caption = 60
            vDtTranInfo1.Columns("DIAWT").Caption = 80
            vDtTranInfo1.pRowHeight = 21
            vDtTranInfo1.pColHeaderVisible = True
            vDtTranInfo1.pCellBorder = True
            vDtTranInfo1.pCellBorderColor = Pens.LightGray
            vDtTranInfo1.pContentFont = New Font("verdana", 8, FontStyle.Regular)

            vDtTranINfo2 = vDtTranInfo1.Clone
            Ro = vDtTranINfo2.NewRow
            Ro.Item("ITEM") = "TOTAL"
            Ro.Item("PCS") = Val(vDtTranInfo1.Compute("SUM(PCS)", String.Empty).ToString)
            Ro.Item("GRSWT") = Val(vDtTranInfo1.Compute("SUM(GRSWT)", String.Empty).ToString)
            Ro.Item("NETWT") = Val(vDtTranInfo1.Compute("SUM(NETWT)", String.Empty).ToString)
            Ro.Item("DIAPCS") = Val(vDtTranInfo1.Compute("SUM(DIAPCS)", String.Empty).ToString)
            Ro.Item("DIAWT") = Val(vDtTranInfo1.Compute("SUM(DIAWT)", String.Empty).ToString)
            vDtTranINfo2.pTableBackColor = Color.LightGoldenrodYellow
            vDtTranINfo2.Rows.Add(Ro)
            vDtTranINfo2.pRowHeight = 21
            vDtTranINfo2.pColHeaderVisible = False
            vDtTranINfo2.pCellBorder = True
            vDtTranINfo2.pCellBorderColor = Pens.LightGray
            vDtTranINfo2.pContentFont = New Font("verdana", 8, FontStyle.Bold)
        End If

        Dim vDtTranFooter1 As New BrightPosting.GDatatable
        vDtTranFooter1.pTableContentAlignment = StringAlignment.Near
        vDtTranFooter1.Columns.Add("DES1", GetType(String))
        vDtTranFooter1.Columns.Add("DES2", GetType(String))
        vDtTranFooter1.Columns.Add("DES3", GetType(String))
        vDtTranFooter1.Rows.Add()

        Ro = vDtTranFooter1.NewRow
        Ro.Item("DES1") = "TRANSFERED BY"
        Ro.Item("DES2") = "TRANSIT BY"
        Ro.Item("DES3") = "RECEIVED BY"
        'vDtTranFooter1.Columns("DES1").Caption = 400
        'vDtTranFooter1.Columns("DES2").Caption = 100
        vDtTranFooter1.Rows.Add(Ro)
        vDtTranFooter1.pRowHeight = 21
        vDtTranFooter1.pColHeaderVisible = False
        vDtTranFooter1.pCellBorder = False
        vDtTranFooter1.pContentFont = New Font("verdana", 10, FontStyle.Bold)
        vDtTranFooter1.pTableContentAlignment = StringAlignment.Center

        Dim lstSource As New List(Of Object)
        lstSource.Clear()
        lstSource.Add(vDtTranTitle1)
        lstSource.Add(vDtTranTitle2)
        lstSource.Add(vDtTranTitle3)
        lstSource.Add(vDtTranInfo1)
        lstSource.Add(vDtTranINfo2)
        lstSource.Add(vDtTranFooter1)
        Dim obj As New BrightPosting.GListPrinter(lstSource)
        obj.Print()
    End Sub

    Public Sub PrintStockTransfer(ByVal RefNo As String, ByVal RefDate As Date)
        If RefNo = "" Then Exit Sub
        Dim countergroup As Boolean = IIf(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PRN_STKTRANSFER_CTR'").ToString = "Y", True, False)
        strSql = " SELECT TOP 1 (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.TCOSTID)aS COSTNAME"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.TCOSTID)aS COSTNAME"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG AS T"
        strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.TCOSTID)aS COSTNAME"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS T"
        strSql += vbCrLf + "  WHERE T.RECISS ='I' AND T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"

        Dim tCostName As String = objGPack.GetSqlValue(strSql)
        If tCostName = "" Then
            Exit Sub
        End If
        Dim UserName As String = Nothing
        Dim UpTime As String = Nothing
        Dim Remark1 As String = ""
        Dim Remark2 As String = ""
        strSql = " SELECT TOP 1 CASE WHEN UPTIME IS NOT NULL THEN SUBSTRING(CONVERT(VARCHAR,UPTIME,113),13,5) ELSE '' END AS UPTIME"
        strSql += vbCrLf + " ,(SELECT TOP 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID)AS USERNAME"
        strSql += vbCrLf + "  ,REMARK1,REMARK2"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE REFNO = '" & RefNo & "' AND REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " UNION"
        strSql += vbCrLf + " SELECT TOP 1 CASE WHEN UPTIME IS NOT NULL THEN SUBSTRING(CONVERT(VARCHAR,UPTIME,113),13,5) ELSE '' END AS UPTIME"
        strSql += vbCrLf + " ,(SELECT TOP 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID)AS USERNAME"
        strSql += vbCrLf + "  ,REMARK1,REMARK2"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE REFNO = '" & RefNo & "' AND REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"

        Dim DtTemp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtTemp)
        If DtTemp.Rows.Count > 0 Then
            UserName = DtTemp.Rows(0).Item("USERNAME").ToString
            UpTime = DtTemp.Rows(0).Item("UPTIME").ToString
            Remark1 = DtTemp.Rows(0).Item("REMARK1").ToString
            Remark2 = DtTemp.Rows(0).Item("REMARK2").ToString
            If UpTime = "00:00" Then UpTime = ""
        End If
        Dim strCtrName As String = ""
        Dim DtTemp1 As DataTable
        strSql = vbCrLf + " SELECT C.ITEMCTRNAME FROM ( "
        strSql += vbCrLf + " SELECT DISTINCT ITEMCTRID FROM " & cnAdminDb & "..ITEMTAG WHERE REFNO = '" & RefNo & "' AND REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT DISTINCT ITEMCTRID FROM " & cnAdminDb & "..CITEMTAG WHERE REFNO = '" & RefNo & "' AND REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT DISTINCT ITEMCTRID FROM " & cnAdminDb & "..TITEMTAG WHERE REFNO = '" & RefNo & "' AND REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " )X INNER JOIN " & cnAdminDb & "..ITEMCOUNTER AS C ON X.ITEMCTRID = C.ITEMCTRID "
        da = New OleDbDataAdapter(strSql, cn)
        DtTemp1 = New DataTable
        da.Fill(DtTemp1)
        If DtTemp1.Rows.Count > 0 Then
            For kk As Integer = 0 To DtTemp1.Rows.Count - 1
                strCtrName += DtTemp1.Rows(kk)("ITEMCTRNAME").ToString
                If DtTemp1.Rows(kk)("ITEMCTRNAME").ToString = "" Then Continue For
                If kk < DtTemp1.Rows.Count - 1 Then strCtrName += ","
            Next
        End If


        Dim Detail As Boolean = False
        If MessageBox.Show("Do you want Detailed Stock Transfer Print?", "Stock Transfer Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Detail = True
        End If
        Dim vDtTranTitle1 As New BrightPosting.GDatatable
        vDtTranTitle1.pTableContentAlignment = StringAlignment.Center
        vDtTranTitle1.Columns.Add("DES1", GetType(String))
        Dim Ro As BrightPosting.GDataRow = Nothing
        Ro = vDtTranTitle1.NewRow
        Ro.Item("DES1") = cnCompanyName
        vDtTranTitle1.Rows.Add(Ro)
        vDtTranTitle1.pRowHeight = 21
        vDtTranTitle1.pColHeaderVisible = False
        vDtTranTitle1.pCellBorder = False
        vDtTranTitle1.pContentFont = New Font("VERDANA", 12, FontStyle.Bold)

        Dim vDtTranTitle2 As New BrightPosting.GDatatable
        vDtTranTitle2.pTableContentAlignment = StringAlignment.Center
        vDtTranTitle2.Columns.Add("DES1", GetType(String))


        Ro = vDtTranTitle2.NewRow
        Ro.Item("DES1") = "STOCK TRANSFER FROM " & cnCostName & " TO " & tCostName
        vDtTranTitle2.Rows.Add(Ro)
        vDtTranTitle2.pRowHeight = 21
        vDtTranTitle2.pColHeaderVisible = False
        vDtTranTitle2.pCellBorder = False
        vDtTranTitle2.pContentFont = New Font("verdana", 10, FontStyle.Bold)

        Dim vDtTranTitle3 As New BrightPosting.GDatatable
        vDtTranTitle3.pTableContentAlignment = StringAlignment.Near
        vDtTranTitle3.Columns.Add("DES1", GetType(String))
        Ro = vDtTranTitle3.NewRow
        Ro.Item("DES1") = "USER NAME : " & UserName
        vDtTranTitle3.Rows.Add(Ro)
        Ro = vDtTranTitle3.NewRow
        Ro.Item("DES1") = "TRASFER NO : " & Mid(RefNo, 1, 5) & "-" & Mid(RefNo, 6) & " DATE : " & RefDate.ToString("dd/MM/yyyy") & IIf(UpTime <> "", " " & UpTime, "")
        vDtTranTitle3.Rows.Add(Ro)
        If countergroup And strCtrName <> "" Then
            Ro = vDtTranTitle3.NewRow
            Ro.Item("DES1") = "COUNTER NAME : " & strCtrName
            vDtTranTitle3.Rows.Add(Ro)
        End If
        vDtTranTitle3.pRowHeight = 21
        vDtTranTitle3.pColHeaderVisible = False
        vDtTranTitle3.pCellBorder = False
        vDtTranTitle3.pContentFont = New Font("verdana", 10, FontStyle.Bold)

        '''  Sno| Subitem Name | Tagno | No.of Tag | Pcs | Grs Wt | Stn Wt grm | Stn Wt Crt | Dia Crt | Net wt  | Rate | SaleValue
        Dim TempFile As String = "TEMP" & systemId & "TRFPRINT"
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = '" & TempFile & "')> 0"
        strSql += " DROP TABLE " & TempFile
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        Dim vDtTranInfo1 As New BrightPosting.GDatatable
        'VDTTRANINFO1
        Dim vDtTranINfo2 As New BrightPosting.GDatatable
        Dim vDtTranINfo3 As New BrightPosting.GDatatable
        Dim vDtTranFoot As New BrightPosting.GDatatable
        Dim vDtTranFoot2 As New BrightPosting.GDatatable
        Dim vDtTranFoot3 As New BrightPosting.GDatatable
        If Detail Then
            If countergroup Then
                strSql = vbCrLf + "  SELECT CTR.ITEMCTRNAME,IM.ITEMNAME,/*IM.ITEMNAME+' '+*/SIM.SUBITEMNAME  AS ITEM,TAGNO"
                strSql += vbCrLf + "  ,CASE WHEN T.PCS<>0 THEN T.PCS/T.PCS END NOTAG"
                strSql += vbCrLf + "  ,T.PCS,T.GRSWT,T.NETWT"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE STONEUNIT = 'G' AND TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE <> 'D'))AS STNWT_G"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE STONEUNIT = 'C' AND TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE <> 'D'))AS STNWT_C"
                strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN RATE <> 0 THEN T.RATE ELSE NULL END) AS RATE"
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN T.SALEMODE IN ('R','F') AND T.SALVALUE <> 0 THEN T.SALVALUE ELSE NULL END) AS SALVALUE,1 AS DISPORDER"
                strSql += vbCrLf + "  INTO " & TempFile
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMCOUNTER AS CTR ON CTR.ITEMCTRID = T.ITEMCTRID"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..SUBITEMMAST AS SIM ON SIM.ITEMID = T.ITEMID AND SIM.SUBITEMID = T.SUBITEMID"
                strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + "  SELECT CTR.ITEMCTRNAME,IM.ITEMNAME,/*IM.ITEMNAME+' '+*/SIM.SUBITEMNAME  AS ITEM,TAGNO"
                strSql += vbCrLf + "  ,CASE WHEN T.PCS<>0 THEN T.PCS/T.PCS END NOTAG"
                strSql += vbCrLf + "  ,T.PCS,T.GRSWT,T.NETWT"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE STONEUNIT = 'G' AND TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE <> 'D'))AS STNWT_G"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE STONEUNIT = 'C' AND TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE <> 'D'))AS STNWT_C"
                strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN RATE <> 0 THEN T.RATE ELSE NULL END) AS RATE"
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN T.SALEMODE IN ('R','F') AND T.SALVALUE <> 0 THEN T.SALVALUE ELSE NULL END) AS SALVALUE,1 AS DISPORDER"
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..CITEMTAG AS T"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMCOUNTER AS CTR ON CTR.ITEMCTRID = T.ITEMCTRID"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..SUBITEMMAST AS SIM ON SIM.ITEMID = T.ITEMID AND SIM.SUBITEMID = T.SUBITEMID"
                strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + "  SELECT CTR.ITEMCTRNAME,IM.ITEMNAME,/*IM.ITEMNAME+' '+*/SIM.SUBITEMNAME  AS ITEM,'' TAGNO,0 NONTAG,T.PCS,T.GRSWT,T.NETWT"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT ='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE <> 'D'))AS STNWT_G"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT ='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE <> 'D'))AS STNWT_C"
                strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN RATE <> 0 THEN T.RATE ELSE NULL END) AS RATE"
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),0) AS SALVALUE,1 AS DISPORDER"
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG AS T"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMCOUNTER AS CTR ON CTR.ITEMCTRID = T.ITEMCTRID"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..SUBITEMMAST AS SIM ON SIM.ITEMID = T.ITEMID AND SIM.SUBITEMID = T.SUBITEMID"
                strSql += vbCrLf + "  WHERE T.RECISS = 'I' AND T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
                cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()

                strSql = "INSERT INTO " & TempFile & "(ITEMCTRNAME,ITEM,DISPORDER) SELECT DISTINCT A.ITEMCTRNAME,A.ITEMCTRNAME AS ITEM,0 AS DISPORDER FROM " & TempFile & " A ORDER BY A.ITEMCTRNAME"
                cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
                strSql = "INSERT INTO " & TempFile & "(ITEMCTRNAME,ITEM,TAGNO, DISPORDER,NOTAG,PCS,GRSWT"
                strSql += vbCrLf + "  ,STNWT_G,STNWT_C,DIAWT,NETWT,SALVALUE) SELECT A.ITEMCTRNAME"
                strSql += vbCrLf + "  ,A.ITEMCTRNAME AS ITEM,'' TAGNO,3 AS DISPORDER"
                strSql += vbCrLf + "  ,SUM(NOTAG),SUM(PCS),SUM(GRSWT),SUM(STNWT_G),SUM(STNWT_C)"
                strSql += vbCrLf + "  ,SUM(DIAWT),SUM(NETWT),SUM(SALVALUE) "
                strSql += vbCrLf + "  FROM " & TempFile & " A WHERE A.DISPORDER = 1 "
                strSql += vbCrLf + "  GROUP BY A.ITEMCTRNAME"
                cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()


                'strSql = vbCrLf + "    INSERT INTO " & TempFile & "(ITEMCTRNAME,ITEM,DISPORDER,NOTAG,PCS,GRSWT"
                'strSql += vbCrLf + "    ,STNWT_G,STNWT_C,DIAWT,NETWT,SALVALUE) "
                'strSql += vbCrLf + "    SELECT 'ZZZZZZZ',A.ITEM,3,count(*),SUM(PCS),SUM(GRSWT),SUM(STNWT_G),SUM(STNWT_C)"
                'strSql += vbCrLf + "    ,SUM(DIAWT),SUM(NETWT),SUM(SALVALUE) "
                'strSql += vbCrLf + "    FROM " & TempFile & " A WHERE A.DISPORDER = 1 GROUP BY A.ITEM ORDER BY A.ITEM "
                'cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()

                strSql = " ALTER TABLE " & TempFile & " ADD SNO INT "
                cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()

                strSql = "  DECLARE CUR CURSOR FOR "
                strSql += vbCrLf + "  SELECT TAGNO FROM " & TempFile & " WHERE ISNULL(TAGNO,'')<>'' AND ISNULL(TAGNO,'')<>' TOTAL' ORDER BY ITEMCTRNAME,DISPORDER"
                strSql += vbCrLf + "  OPEN CUR"
                strSql += vbCrLf + "  DECLARE @ID VARCHAR(15)"
                strSql += vbCrLf + "  DECLARE @I INT "
                strSql += vbCrLf + "  SET @I=0"
                strSql += vbCrLf + "  FETCH NEXT FROM CUR INTO @ID"
                strSql += vbCrLf + "  WHILE (@@FETCH_STATUS = 0)"
                strSql += vbCrLf + "  BEGIN"
                strSql += vbCrLf + "      PRINT(@ID)"
                strSql += vbCrLf + "      SET @I=@I+1"
                strSql += vbCrLf + "      PRINT @I"
                strSql += vbCrLf + "      UPDATE " & TempFile & " SET SNO=CONVERT(VARCHAR(12),@I) WHERE TAGNO=@ID"
                strSql += vbCrLf + "      FETCH NEXT FROM CUR INTO @ID"
                strSql += vbCrLf + "  END"
                strSql += vbCrLf + "  CLOSE CUR"
                strSql += vbCrLf + "  DEALLOCATE CUR"
                cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()

                strSql = "SELECT SNO,ITEM,TAGNO TAGNO,NOTAG TAG,PCS PCS"
                strSql += vbCrLf + "  ,GRSWT ,STNWT_G AS STN_G"
                strSql += vbCrLf + "  ,STNWT_C AS STN_C,DIAWT,NETWT "
                strSql += vbCrLf + "  ,RATE,SALVALUE AS SALVAL,DISPORDER "
                strSql += vbCrLf + "  FROM " & TempFile & " ORDER BY ITEMCTRNAME,DISPORDER"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(vDtTranInfo1)
                vDtTranInfo1.Columns("SNO").Caption = 60
                vDtTranInfo1.Columns("ITEM").Caption = 200
                vDtTranInfo1.Columns("TAG").Caption = 55
                vDtTranInfo1.Columns("TAGNO").Caption = 90
                vDtTranInfo1.Columns("PCS").Caption = 55
                vDtTranInfo1.Columns("GRSWT").Caption = 90
                vDtTranInfo1.Columns("NETWT").Caption = 90
                vDtTranInfo1.Columns("STN_G").Caption = 90
                vDtTranInfo1.Columns("STN_C").Caption = 90
                vDtTranInfo1.Columns("DIAWT").Caption = 90
                vDtTranInfo1.Columns("RATE").Caption = 70
                vDtTranInfo1.Columns("SALVAL").Caption = 90
                vDtTranInfo1.pRowHeight = 21
                vDtTranInfo1.pColHeaderVisible = True
                vDtTranInfo1.pCellBorder = True
                vDtTranInfo1.pCellBorderColor = Pens.LightGray
                vDtTranInfo1.pContentFont = New Font("verdana", 7, FontStyle.Regular)


                vDtTranINfo2 = vDtTranInfo1.Clone
                Ro = vDtTranINfo2.NewRow
                Ro.Item("ITEM") = "TOTAL"
                Dim Totstring As String = "DISPORDER=1"
                Ro.Item("TAG") = IIf(Val(vDtTranInfo1.Compute("SUM(TAG)", Totstring).ToString) = 0, DBNull.Value, Val(vDtTranInfo1.Compute("SUM(TAG)", Totstring).ToString))
                Ro.Item("PCS") = IIf(Val(vDtTranInfo1.Compute("SUM(PCS)", Totstring).ToString) = 0, DBNull.Value, Val(vDtTranInfo1.Compute("SUM(PCS)", Totstring).ToString))
                Ro.Item("GRSWT") = IIf(Val(vDtTranInfo1.Compute("SUM(GRSWT)", Totstring).ToString) = 0, DBNull.Value, Val(vDtTranInfo1.Compute("SUM(GRSWT)", Totstring).ToString))
                Ro.Item("NETWT") = IIf(Val(vDtTranInfo1.Compute("SUM(NETWT)", Totstring).ToString) = 0, DBNull.Value, Val(vDtTranInfo1.Compute("SUM(NETWT)", Totstring).ToString))
                Ro.Item("STN_G") = IIf(Val(vDtTranInfo1.Compute("SUM(STN_G)", Totstring).ToString) = 0, DBNull.Value, Val(vDtTranInfo1.Compute("SUM(STN_G)", Totstring).ToString))
                Ro.Item("STN_C") = IIf(Val(vDtTranInfo1.Compute("SUM(STN_C)", Totstring).ToString) = 0, DBNull.Value, Val(vDtTranInfo1.Compute("SUM(STN_C)", Totstring).ToString))
                Ro.Item("DIAWT") = IIf(Val(vDtTranInfo1.Compute("SUM(DIAWT)", Totstring).ToString) = 0, DBNull.Value, Val(vDtTranInfo1.Compute("SUM(DIAWT)", Totstring).ToString))
                Ro.Item("SALVAL") = IIf(Val(vDtTranInfo1.Compute("SUM(SALVAL)", Totstring).ToString) = 0, DBNull.Value, Val(vDtTranInfo1.Compute("SUM(SALVAL)", Totstring).ToString))
                vDtTranINfo2.pTableBackColor = Color.LightGoldenrodYellow
                vDtTranINfo2.Rows.Add(Ro)
                vDtTranINfo2.pRowHeight = 21
                vDtTranINfo2.pColHeaderVisible = False
                vDtTranINfo2.pCellBorder = True
                vDtTranINfo2.pCellBorderColor = Pens.LightGray
                vDtTranINfo2.pContentFont = New Font("verdana", 8, FontStyle.Bold)

                vDtTranINfo3 = vDtTranInfo1.Clone
                strSql = vbCrLf + "    SELECT A.ITEMNAME AS ITEM,count(*) AS TAG,SUM(PCS) AS PCS,SUM(GRSWT) AS GRSWT,SUM(STNWT_G) AS STN_G,SUM(STNWT_C) AS STN_C"
                strSql += vbCrLf + "    ,SUM(DIAWT) AS DIAWT,SUM(NETWT) AS NETWT,SUM(SALVALUE) AS SALVALUE "
                strSql += vbCrLf + "    FROM " & TempFile & " A WHERE A.DISPORDER = 1 GROUP BY A.ITEMNAME ORDER BY A.ITEMNAME "
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(vDtTranFoot3)
                'vDtTranFoot3 = vDtTranInfo1.Clone
                If vDtTranFoot3.Rows.Count > 0 Then
                    Ro = vDtTranINfo2.NewRow
                    Ro.Item("ITEM") = "ITEM WISE"
                    vDtTranINfo2.pTableBackColor = Color.LightGoldenrodYellow
                    vDtTranINfo2.Rows.Add(Ro)
                    vDtTranINfo2.pRowHeight = 21
                    vDtTranINfo2.pColHeaderVisible = False
                    vDtTranINfo2.pCellBorder = True
                    vDtTranINfo2.pCellBorderColor = Pens.LightGray
                    vDtTranINfo2.pContentFont = New Font("verdana", 8, FontStyle.Bold)
                End If
                For kk As Integer = 0 To vDtTranFoot3.Rows.Count - 1
                    Ro = vDtTranINfo3.NewRow
                    Ro.Item("ITEM") = vDtTranFoot3.Rows(kk)("ITEM").ToString
                    Ro.Item("TAG") = vDtTranFoot3.Rows(kk)("TAG").ToString
                    Ro.Item("PCS") = vDtTranFoot3.Rows(kk)("PCS").ToString
                    Ro.Item("GRSWT") = vDtTranFoot3.Rows(kk)("GRSWT").ToString
                    'Ro.Item("STN_G") = vDtTranFoot3.Rows(kk)("STN_G").ToString
                    'Ro.Item("STN_C") = iff(vDtTranFoot3.Rows(kk)("STN_C").ToString
                    'Ro.Item("DIAWT") = vDtTranFoot3.Rows(kk)("DIAWT").ToString
                    Ro.Item("NETWT") = vDtTranFoot3.Rows(kk)("NETWT").ToString
                    vDtTranINfo3.pTableBackColor = Color.LightGoldenrodYellow
                    vDtTranINfo3.Rows.Add(Ro)
                    vDtTranINfo3.pRowHeight = 21
                    vDtTranINfo3.pColHeaderVisible = False
                    vDtTranINfo3.pCellBorder = True
                    vDtTranINfo3.pCellBorderColor = Pens.LightGray
                    vDtTranINfo3.pContentFont = New Font("verdana", 7, FontStyle.Regular)

                Next


                vDtTranFoot.pTableContentAlignment = StringAlignment.Near
                vDtTranFoot.Columns.Add("DES1", GetType(String))
                vDtTranFoot.Rows.Add()
                Ro = vDtTranFoot.NewRow
                If Remark1 <> "" Then
                    Ro.Item("DES1") = "REMARK:" & Remark1.Trim & " " & Remark2.Trim
                End If
                vDtTranFoot.Rows.Add(Ro)
                vDtTranFoot.pRowHeight = 21
                vDtTranFoot.pColHeaderVisible = False
                vDtTranFoot.pCellBorder = False
                vDtTranFoot.pContentFont = New Font("verdana", 7, FontStyle.Bold)
                vDtTranFoot.pTableContentAlignment = StringAlignment.Near


                'vDtTranFoot2 = vDtTranFoot.Clone
                'Ro = vDtTranFoot2.NewRow
                'If Remark2 <> "" Then
                '    Ro.Item("DES1") = "REMARK2:" & Remark2
                'End If
                'vDtTranFoot2.Rows.Add(Ro)
                'vDtTranFoot2.pRowHeight = 21
                'vDtTranFoot2.pColHeaderVisible = False
                'vDtTranFoot2.pCellBorder = False
                'vDtTranFoot2.pContentFont = New Font("verdana", 8, FontStyle.Bold)
                'vDtTranFoot2.pTableContentAlignment = StringAlignment.Near

                'strSql = vbCrLf + "  SELECT ITEM DES1,SUM(PCS)PCS,SUM(GRSWT)GRSWT"
                'strSql += vbCrLf + "  FROM " & TempFile & " WHERE DISPORDER = '1' GROUP BY ITEM ORDER BY ITEM"
                'da = New OleDbDataAdapter(strSql, cn)
                'da.Fill(vDtTranFoot3)
                'Ro = vDtTranFoot3.NewRow
                'vDtTranFoot3.Rows.Add(Ro)
                'vDtTranFoot3.pRowHeight = 21
                'vDtTranFoot3.pColHeaderVisible = False
                'vDtTranFoot3.pCellBorder = False
                'vDtTranFoot3.pContentFont = New Font("verdana", 8, FontStyle.Bold)
                'vDtTranFoot3.pTableContentAlignment = StringAlignment.Near
            Else
                strSql = vbCrLf + "  SELECT IM.ITEMNAME AS ITEM,TAGNO,T.PCS,T.GRSWT,T.NETWT"
                strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN RATE <> 0 THEN T.RATE ELSE NULL END) AS RATE"
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN T.SALEMODE IN ('R','F') AND T.SALVALUE <> 0 THEN T.SALVALUE ELSE NULL END) AS SALVALUE"
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + "  SELECT IM.ITEMNAME AS ITEM,TAGNO,T.PCS,T.GRSWT,T.NETWT"
                strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN RATE <> 0 THEN T.RATE ELSE NULL END) AS RATE"
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN T.SALEMODE IN ('R','F') AND T.SALVALUE <> 0 THEN T.SALVALUE ELSE NULL END) AS SALVALUE"
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..CITEMTAG AS T"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + "  SELECT IM.ITEMNAME AS ITEM,'' TAGNO,T.PCS,T.GRSWT,T.NETWT"
                strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN RATE <> 0 THEN T.RATE ELSE NULL END) AS RATE"
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),0) AS SALVALUE"
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG AS T"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + "  WHERE T.RECISS = 'I' AND T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(vDtTranInfo1)
                vDtTranInfo1.Columns("ITEM").Caption = 150
                vDtTranInfo1.Columns("TAGNO").Caption = 70
                vDtTranInfo1.Columns("PCS").Caption = 60
                vDtTranInfo1.Columns("GRSWT").Caption = 70
                vDtTranInfo1.Columns("NETWT").Caption = 70
                vDtTranInfo1.Columns("DIAPCS").Caption = 70
                vDtTranInfo1.Columns("DIAWT").Caption = 80
                vDtTranInfo1.Columns("RATE").Caption = 70
                vDtTranInfo1.Columns("SALVALUE").Caption = 90
                vDtTranInfo1.pRowHeight = 21
                vDtTranInfo1.pColHeaderVisible = True
                vDtTranInfo1.pCellBorder = True
                vDtTranInfo1.pCellBorderColor = Pens.LightGray
                vDtTranInfo1.pContentFont = New Font("verdana", 8, FontStyle.Regular)

                vDtTranINfo2 = vDtTranInfo1.Clone
                Ro = vDtTranINfo2.NewRow
                Ro.Item("ITEM") = "TOTAL"
                Ro.Item("PCS") = Val(vDtTranInfo1.Compute("SUM(PCS)", String.Empty).ToString)
                Ro.Item("GRSWT") = Val(vDtTranInfo1.Compute("SUM(GRSWT)", String.Empty).ToString)
                Ro.Item("NETWT") = Val(vDtTranInfo1.Compute("SUM(NETWT)", String.Empty).ToString)
                Ro.Item("DIAPCS") = Val(vDtTranInfo1.Compute("SUM(DIAPCS)", String.Empty).ToString)
                Ro.Item("DIAWT") = Val(vDtTranInfo1.Compute("SUM(DIAWT)", String.Empty).ToString)
                Ro.Item("SALVALUE") = Val(vDtTranInfo1.Compute("SUM(SALVALUE)", String.Empty).ToString)
                vDtTranINfo2.pTableBackColor = Color.LightGoldenrodYellow
                vDtTranINfo2.Rows.Add(Ro)
                vDtTranINfo2.pRowHeight = 21
                vDtTranINfo2.pColHeaderVisible = False
                vDtTranINfo2.pCellBorder = True
                vDtTranINfo2.pCellBorderColor = Pens.LightGray
                vDtTranINfo2.pContentFont = New Font("verdana", 8, FontStyle.Bold)
            End If
        Else
            If countergroup Then
                strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = '" & TempFile & "')> 0"
                strSql += " DROP TABLE " & TempFile
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                strSql = vbCrLf + "  SELECT ITEMCTRNAME,ITEM,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT,1 AS RESULT "
                strSql += vbCrLf + "  INTO " & TempFile & " FROM"
                strSql += vbCrLf + "  ("
                strSql += vbCrLf + "  SELECT IC.ITEMCTRNAME,IM.ITEMNAME AS ITEM,T.PCS,T.GRSWT,T.NETWT"
                strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMCOUNTER AS IC ON IC.ITEMCTRID = T.ITEMCTRID"
                strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT IC.ITEMCTRNAME,IM.ITEMNAME AS ITEM,T.PCS,T.GRSWT,T.NETWT"
                strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..CITEMTAG AS T"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMCOUNTER AS IC ON IC.ITEMCTRID = T.ITEMCTRID"
                strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + "  )X "
                strSql += vbCrLf + "  GROUP BY ITEMCTRNAME,ITEM"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO " & TempFile & "(ITEMCTRNAME,ITEM,RESULT) SELECT DISTINCT ITEMCTRNAME,ITEMCTRNAME,0 FROM " & TempFile & " A "
                cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
                strSql = "INSERT INTO " & TempFile & "(ITEMCTRNAME,ITEM,PCS,GRSWT,NETWT,DIAPCS,DIAWT,RESULT)"
                strSql += vbCrLf + "  SELECT ITEMCTRNAME,ITEMCTRNAME,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT,2 "
                strSql += vbCrLf + "  FROM " & TempFile & " A WHERE RESULT=1 GROUP BY ITEMCTRNAME  "
                cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()

                strSql = " ALTER TABLE " & TempFile & " ADD SNO INT "
                cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()

                strSql = "  DECLARE CUR CURSOR FOR "
                strSql += vbCrLf + "  SELECT ITEMCTRNAME,ITEM FROM " & TempFile & " WHERE RESULT=1 ORDER BY ITEMCTRNAME,ITEM"
                strSql += vbCrLf + "  OPEN CUR"
                strSql += vbCrLf + "  DECLARE @ITEMCTRNAME VARCHAR(150)"
                strSql += vbCrLf + "  DECLARE @ITEM VARCHAR(150)"
                strSql += vbCrLf + "  DECLARE @I INT "
                strSql += vbCrLf + "  SET @I=0"
                strSql += vbCrLf + "  FETCH NEXT FROM CUR INTO @ITEMCTRNAME,@ITEM"
                strSql += vbCrLf + "  WHILE (@@FETCH_STATUS = 0)"
                strSql += vbCrLf + "  BEGIN"
                strSql += vbCrLf + "      PRINT(@ITEMCTRNAME + @ITEM)"
                strSql += vbCrLf + "      SET @I=@I+1"
                strSql += vbCrLf + "      PRINT @I"
                strSql += vbCrLf + "      UPDATE " & TempFile & " SET SNO=CONVERT(VARCHAR(12),@I) WHERE ITEMCTRNAME=@ITEMCTRNAME AND ITEM=@ITEM "
                strSql += vbCrLf + "      FETCH NEXT FROM CUR INTO @ITEMCTRNAME,@ITEM"
                strSql += vbCrLf + "  END"
                strSql += vbCrLf + "  CLOSE CUR"
                strSql += vbCrLf + "  DEALLOCATE CUR"
                cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()


                strSql = "  SELECT SNO,ITEM,PCS,GRSWT,NETWT,DIAPCS,DIAWT FROM " & TempFile & " ORDER BY ITEMCTRNAME,RESULT "
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(vDtTranInfo1)
            Else
                strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = '" & TempFile & "')> 0"
                strSql += " DROP TABLE " & TempFile
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                strSql = vbCrLf + "  SELECT ITEM,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT"
                strSql += vbCrLf + "  INTO " & TempFile & " FROM"
                strSql += vbCrLf + "  ("
                strSql += vbCrLf + "  SELECT IM.ITEMNAME AS ITEM,T.PCS,T.GRSWT,T.NETWT"
                strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT IM.ITEMNAME AS ITEM,T.PCS,T.GRSWT,T.NETWT"
                strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
                strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..CITEMTAG AS T"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + "  )X "
                strSql += vbCrLf + "  GROUP BY ITEM"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                strSql = " ALTER TABLE " & TempFile & " ADD SNO INT IDENTITY NOT NULL "
                cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
                strSql = "  SELECT SNO,ITEM,PCS,GRSWT,NETWT,DIAPCS,DIAWT FROM " & TempFile
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(vDtTranInfo1)
            End If
            vDtTranInfo1.Columns("SNO").Caption = 60
            vDtTranInfo1.Columns("ITEM").Caption = 200
            vDtTranInfo1.Columns("PCS").Caption = 60
            vDtTranInfo1.Columns("GRSWT").Caption = 80
            vDtTranInfo1.Columns("NETWT").Caption = 80
            vDtTranInfo1.Columns("DIAPCS").Caption = 60
            vDtTranInfo1.Columns("DIAWT").Caption = 80
            vDtTranInfo1.pRowHeight = 21
            vDtTranInfo1.pColHeaderVisible = True
            vDtTranInfo1.pCellBorder = True
            vDtTranInfo1.pCellBorderColor = Pens.LightGray
            vDtTranInfo1.pContentFont = New Font("verdana", 8, FontStyle.Regular)

            vDtTranINfo2 = vDtTranInfo1.Clone
            Ro = vDtTranINfo2.NewRow
            Ro.Item("ITEM") = "TOTAL"
            Ro.Item("PCS") = Val(vDtTranInfo1.Compute("SUM(PCS)", String.Empty).ToString)
            Ro.Item("GRSWT") = Val(vDtTranInfo1.Compute("SUM(GRSWT)", String.Empty).ToString)
            Ro.Item("NETWT") = Val(vDtTranInfo1.Compute("SUM(NETWT)", String.Empty).ToString)
            Ro.Item("DIAPCS") = Val(vDtTranInfo1.Compute("SUM(DIAPCS)", String.Empty).ToString)
            Ro.Item("DIAWT") = Val(vDtTranInfo1.Compute("SUM(DIAWT)", String.Empty).ToString)
            vDtTranINfo2.pTableBackColor = Color.LightGoldenrodYellow
            vDtTranINfo2.Rows.Add(Ro)
            vDtTranINfo2.pRowHeight = 21
            vDtTranINfo2.pColHeaderVisible = False
            vDtTranINfo2.pCellBorder = True
            vDtTranINfo2.pCellBorderColor = Pens.LightGray
            vDtTranINfo2.pContentFont = New Font("verdana", 8, FontStyle.Bold)
        End If

        Dim vDtTranFooter1 As New BrightPosting.GDatatable
        vDtTranFooter1.pTableContentAlignment = StringAlignment.Near
        vDtTranFooter1.Columns.Add("DES1", GetType(String))
        vDtTranFooter1.Columns.Add("DES2", GetType(String))
        vDtTranFooter1.Columns.Add("DES3", GetType(String))
        vDtTranFooter1.Rows.Add()

        Ro = vDtTranFooter1.NewRow
        Ro.Item("DES1") = "TRANSFERED BY"
        Ro.Item("DES2") = "TRANSIT BY"
        Ro.Item("DES3") = "RECEIVED BY"
        'vDtTranFooter1.Columns("DES1").Caption = 400
        'vDtTranFooter1.Columns("DES2").Caption = 100
        vDtTranFooter1.Rows.Add(Ro)
        vDtTranFooter1.pRowHeight = 21
        vDtTranFooter1.pColHeaderVisible = False
        vDtTranFooter1.pCellBorder = False
        vDtTranFooter1.pContentFont = New Font("verdana", 10, FontStyle.Bold)
        vDtTranFooter1.pTableContentAlignment = StringAlignment.Center

        Dim lstSource As New List(Of Object)
        lstSource.Clear()
        lstSource.Add(vDtTranTitle1)
        lstSource.Add(vDtTranTitle2)
        lstSource.Add(vDtTranTitle3)
        lstSource.Add(vDtTranInfo1)
        lstSource.Add(vDtTranINfo2)
        lstSource.Add(vDtTranINfo3)
        If countergroup Then lstSource.Add(vDtTranFoot) : lstSource.Add(vDtTranFoot2)
        lstSource.Add(vDtTranFooter1)
        Dim obj As New BrightPosting.GListPrinter(lstSource)
        obj.Print()
    End Sub

    Public Function Budgetcheck(ByVal accode As String, ByVal trandate As DateTime, Optional ByVal tranmode As String = "", Optional ByVal Issubledger As Boolean = False, Optional ByVal Costid As String = Nothing) As String
        Dim bal As Double
        Dim CREDIT As Double
        Dim retval As String
        Dim validby As Integer = 1
        retval = ""
        strSql = "DECLARE @GIVENDATE SMALLDATETIME"
        strSql += vbCrLf + "SELECT @GIVENDATE='" & Format(trandate, "yyyy-MM-dd") & "'"
        strSql += "SELECT  BUDVALUE,TRANMODE,BUDMONTH,ISNULL(BUDCALMODE,'Y') AS BUDCAL ,BUDEFFFROM,BUDEFFTO FROM " & cnStockDb & "..BUDGETCONTROL WHERE ACCODE='" & accode & "'"
        If Costid <> Nothing Then strSql += vbCrLf + " AND COSTID = '" & Costid & "'"
        strSql += vbCrLf + " AND @GIVENDATE>=BUDEFFFROM AND @GIVENDATE<=BUDEFFTO"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            If dt.Rows(0).Item("BUDCAL").ToString = "D" Then validby = 365
            If dt.Rows(0).Item("BUDCAL").ToString = "M" Then validby = 12
            strSql = " SELECT isnull(sum(CASE WHEN TRANMODE='D'THEN isnull(AMOUNT,0) ELSE -1* isnull(AMOUNT,0)  END),0) FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(CANCEL,'') <> 'Y' "
            If Issubledger Then
                strSql += "AND SACCODE='" & accode & "'"
            Else
                strSql += "AND ACCODE='" & accode & "'"
            End If
            If Costid <> Nothing Then strSql += vbCrLf + " AND COSTID = '" & Costid & "'"
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dt.Rows(0).Item("BUDEFFFROM"), "yyyy-MM-dd") & "' AND '" & Format(dt.Rows(0).Item("BUDEFFTO"), "yyyy-MM-dd") & "'"
            bal = Val(GetSqlValue(cn, strSql).ToString)
            If bal <= 0 And dt.Rows(0).Item("TRANMODE") = "C" Then
                retval = ((Val(dt.Rows(0).Item("BUDVALUE").ToString) / validby) + bal).ToString + " Cr"
            ElseIf bal >= 0 And dt.Rows(0).Item("TRANMODE") = "D" Then
                retval = ((Val(dt.Rows(0).Item("BUDVALUE").ToString) / validby) - bal).ToString + " Dr"
            End If
        End If
        Return retval
    End Function
    Public Function OTPCHECK(ByVal Optionname As String, ByVal Costid As String, ByVal userid As Integer) As Boolean
        Dim IS_USERLEVELPWD As Boolean = IIf(GetAdmindbSoftValue("USERLEVELPWD", "N") = "Y", True, False)
        If userid = 999 Then Return True
        If IS_USERLEVELPWD Then
            Dim pwdid As Integer = 0 : Dim pwdpass As Boolean = False
            Dim Sqlqry As String = "Select Optionid from " & cnAdminDb & "..PRJPWDOPTION where OPTIONNAME ='" & Optionname & "' AND active = 'Y'"
            Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(Sqlqry, , , tran))
            If Optionid = 0 Then Return False
            If Optionid <> 0 Then
                pwdid = GetuserPwd(Optionid, Costid, userid)
                If pwdid <> 0 Then
                    Dim objUpwd As New frmUserPassword(pwdid)
                    If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then Return False Else Return True
                Else
                    Return False
                End If
            End If
        Else
            Return False
        End If

    End Function
    Public Sub ERRORLOGCREATE(ByVal errmsg As String)
        HostName = System.Net.Dns.GetHostName()
        IpAddress = System.Net.Dns.GetHostByName(HostName).AddressList(0).ToString()
        strSql = IpAddress & ":" & HostName & ":" & userId & ":" & Now & ":E:" & errmsg
        Dim memfile As String = Application.StartupPath & "\" & IpAddress & userId.ToString & "Errorlog.err"
        If IO.File.Exists(memfile) Then
            Dim write As IO.StreamWriter
            write = IO.File.CreateText(memfile)
            write.WriteLine(strSql)
            write.Flush()
            write.Close()
        End If
    End Sub

    'Public Function GlbCalcMaxMinValues(ByVal Type As String, ByVal wt As Decimal, ByVal CostCentre As String, _
    '    Optional ByVal TableCode As String = "", _
    '    Optional ByVal Item As String = "", Optional ByVal SubItem As String = "", _
    '    Optional ByVal Designer As String = "", _
    '    Optional ByVal ItemType As String = "" _
    '    ) As String
    '    Select Case Type
    '        Case "T"
    '            strSql = " DECLARE @WT FLOAT"
    '            strSql += vbCrLf + " SET @WT = " & wt & ""
    '            strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
    '            strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
    '            strSql += vbCrLf + " WHERE TABLECODE = '" & TableCode & "'"
    '            strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
    '            strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
    '            strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & CostCentre & "'),'')"
    '        Case "I"
    '            strSql = " DECLARE @WT FLOAT"
    '            strSql += vbCrLf + " SET @WT = " & wt & ""
    '            strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
    '            strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
    '            strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Item & "')"
    '            strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & SubItem & "' AND ITEMID = ( SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Item & "')),0)"
    '            strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
    '            strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
    '            strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
    '            strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & CostCentre & "'),'')"
    '        Case "D"
    '            strSql = " DECLARE @WT FLOAT"
    '            strSql += vbCrLf + " SET @WT = " & wt & ""
    '            strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
    '            strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
    '            strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Item & "')"
    '            strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & SubItem & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Item & "')),0)"
    '            strSql += vbCrLf + " AND DESIGNERID = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & Designer & "'),0)"
    '            strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
    '            strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
    '            strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
    '            strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & CostCentre & "'),'')"
    '        Case "P"
    '            strSql = " DECLARE @WT FLOAT"
    '            strSql += vbCrLf + " SET @WT = " & wt & ""
    '            strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
    '            strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
    '            strSql += vbCrLf + " WHERE ITEMTYPE = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & ItemType & "')"
    '            strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
    '            strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
    '            strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & CostCentre & "'),'')"
    '    End Select
    'End Function

    Public Function GetuserPwd(ByVal Optid As Integer, ByVal xCostId As String, Optional ByVal xUserid As Integer = Nothing, Optional ByVal xCtrid As Integer = 0) As Integer

        Dim Sqlqry As String = "Select Pwdid from " & cnAdminDb & "..PWDMASTER where PWDOPTIONID =" & Optid & " AND PWDSTATUS <> 'C'"
        If xCostId <> "" Then Sqlqry += " AND COSTID = '" & xCostId & "'"
        If xUserid <> 0 Then Sqlqry += " AND PWDUSERID = " & xUserid
        Sqlqry += " AND PWDDATE='" & GetEntryDate(GetServerDate) & "'"
        Sqlqry += " AND PWDEXPIRY<>1"
        'If xCtrid <> 0 Then Sqlqry += " AND PWDCOUNTERID= " & xCtrid
        Return Val(objGPack.GetSqlValue(Sqlqry, , , tran))
    End Function

    Public Function GetuserPwdwithuser(ByVal Optid As Integer, ByVal xCostId As String, Optional ByVal xUserid As Integer = Nothing, Optional ByVal xCtrid As Integer = 0) As String

        Dim Sqlqry As String = "Select Pwdid,Cruserid from " & cnAdminDb & "..PWDMASTER where PWDOPTIONID =" & Optid & " AND PWDSTATUS <> 'C'"
        If xCostId <> "" Then Sqlqry += " AND COSTID = '" & xCostId & "'"
        If xUserid <> 0 Then Sqlqry += " AND PWDUSERID = " & xUserid
        Sqlqry += " order by pwdid"
        Dim Datar As DataRow
        Datar = GetSqlRow(Sqlqry, cn, tran)
        Dim datastring As String = Nothing
        If Not Datar Is Nothing Then
            datastring = Datar(0).ToString & "," & Datar(1).ToString

        End If
        Return datastring
    End Function

    Public Function Check_Is_Authrorize(ByVal Totalamt As Double, ByVal Curramt As Double, ByVal Currper As Double, ByVal LimitPer As Double, ByVal LimitAmt As Double, Optional ByVal Confirm2OTP As Boolean = False) As Boolean
        Dim discPer As Double = Math.Round((LimitAmt * 100) / Totalamt, 2)
        If LimitPer <> 0 And LimitAmt <> 0 Then
            If Curramt > LimitAmt Then
                MsgBox("This User Authorize Rs " & LimitAmt & " Only")
                Return False
            End If
            If Currper > LimitPer Then
                MsgBox("This User Authorize Rs " & Math.Round(Totalamt * (LimitPer / 100), 2) & " Only")
                Return False
            End If
        ElseIf LimitPer = 0 And LimitAmt <> 0 Then
            If Curramt > LimitAmt Then
                MsgBox("This User Authorize Rs " & LimitAmt & " Only")
                Return False
            End If
        ElseIf LimitPer <> 0 And LimitAmt = 0 Then
            If Currper > LimitPer Then
                MsgBox("This User Authorize Rs " & Math.Round(Totalamt * (LimitPer / 100), 2) & " Only")
                Return False
            End If
        End If
        Return True
    End Function

    Public Sub LogonServer()
        Dim ServerSystemName As String = GetAdmindbSoftValue("SERVER_NAME")
        Dim ServerAdminName As String = GetAdmindbSoftValue("SERVER_ACCNAME")
        Dim ServerPwd As String = GetAdmindbSoftValue("SERVER_ACCPWD")
        If ServerSystemName = "" Or ServerAdminName = "" Or ServerPwd = "" Then
            Exit Sub
        End If
        ServerPwd = BrighttechPack.Methods.Decrypt(ServerPwd)
        Dim admin_token As IntPtr
        Dim wid_current As WindowsIdentity = WindowsIdentity.GetCurrent()
        Dim wid_admin As WindowsIdentity = Nothing
        Dim wic As WindowsImpersonationContext = Nothing
        If LogonUser(ServerAdminName, ServerSystemName, ServerPwd, IIf(My.Computer.Name.ToUpper = ServerSystemName, 3, 9), 0, admin_token) <> 0 Then '3
            wid_admin = New WindowsIdentity(admin_token)
            wic = wid_admin.Impersonate()
        End If
    End Sub

    Public Enum Modules
        Stock = 0
        Bill = 1
        Estimation = 2
        Accounts = 3
        OrderRepair = 4
        SavingsScheme = 5
        StoreManagement = 6
    End Enum

    Public Enum SyncMode
        Master = 0
        Stock = 1
        Transaction = 2
    End Enum

    Public Function checkEmailId(ByVal txt As TextBox) As Boolean
        Dim pattern As String = "^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$"
        Dim match As System.Text.RegularExpressions.Match
        match = System.Text.RegularExpressions.Regex.Match(txt.Text.Trim(), pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        If Not match.Success Then
            MsgBox("Enter valid Email address")
            txt.Focus()
            Return False
        End If
        Return True
    End Function

    Public Enum TranSnoType
        ISSUECODE = 0
        ISSSTONECODE = 1
        ISSMISCCODE = 2
        OUTSTANDINGCODE = 3
        ACCTRANCODE = 4
        RECEIPTCODE = 5
        RECEIPTSTONECODE = 6
        RECEIPTMISCCODE = 7
        PERSONALINFOCODE = 8
        REQITEMCODE = 9
        OPENWEIGHTCODE = 10
        OPENITEMCODE = 11
        CHEQUEBOOKCODE = 12
        ESTISSUECODE = 13
        ESTISSSTONECODE = 14
        ESTISSMISCCODE = 15
        ESTRECEIPTCODE = 16
        ESTRECEIPTSTONECODE = 17
        ESTPERSONALINFOCODE = 18
        ISSMETALCODE = 19
        RECEIPTMETALCODE = 20
        ESTISSMETALCODE = 21
        ESTRECEIPTMISCCODE = 22
        ESTRECEIPTMETALCODE = 23
        ITEMLOTCODE = 24
        ITEMDETAILCODE = 25
        CTRANSFERCODE = 26
        ORMASTCODE = 27
        ORSTONECODE = 28
        ORSAMPLECODE = 29
        ORIRDETAILCODE = 30
        ITEMTAGCODE = 31
        ITEMTAGSTONECODE = 32
        ITEMTAGMISCCHARCODE = 33
        ITEMTAGMETALCODE = 34
        ITEMNONTAGCODE = 35
        ITEMNONTAGSTONECODE = 36
        TISSUECODE = 37
        TRECEIPTCODE = 38
        TACCTRANCODE = 39
        TOUTSTANDINGCODE = 40
        TISSSTONECODE = 41
        TISSMISCCODE = 42
        TRECEIPTSTONECODE = 43
        TRECEIPTMISCCODE = 44
        BRSINFOCODE = 45
        CHQPRINT_FORMAT = 46
        STKREORDERCODE = 47
        BRS_ACCTRANCODE = 48
        GVTRANCODE = 49
        DESIGNERSTONECODE = 50
        ESTWT2WTAMTCALCODE = 51
        ALLOYDETAILSCODE = 52
        VACONTROLCODE = 53
        ESTACCTRANCODE = 54
        ADDINFOITEMTAGCODE = 55
        DOC_STATUS = 56
        STOCK_CHKREPORT = 57
        PMTRANCODE = 58
        WITEMTAGCODE = 59
        WITEMTAGSTONECODE = 60
        MIMRRFIDCODE = 61
        PRIVILEGETRANCODE = 62
        MR_ORDERCODE = 63
        MR_ORDERSTONECODE = 64
        ADJTRANCODE = 65
        TAXTRANCODE = 66
        ESTTAXTRANCODE = 67
        GSTREGISTERCODE = 68
        PURCHASEORDER = 69
        PURCHASERECEIPT = 70
        PURCHASEREJECT = 71
        ORIRDETAILSTONECODE = 72
        ORADTRAN = 73
    End Enum

    Public Enum ListControl
        Combo = 0
        ListBox = 1
    End Enum
    Public Sub ColorChange(ByVal ctrl As Object, ByVal fClr As Color, ByVal frmColor As Color)
        For Each c As Object In ctrl.Controls
            If TypeOf c Is CodeVendor.Controls.Grouper Then
                CType(c, CodeVendor.Controls.Grouper).BackgroundColor = fClr
                CType(c, CodeVendor.Controls.Grouper).BackgroundGradientColor = fClr
                CType(c, CodeVendor.Controls.Grouper).BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
                ColorChange(c, fClr, frmColor)
            ElseIf CType(c, Control).Controls.Count > 0 Then
                CType(c, Control).BackColor = frmColor
                ColorChange(c, fClr, frmColor)
            End If
        Next
    End Sub
    Public Sub FormatGridColumns(ByVal grid As DataGridView, Optional ByVal colHeadVisibleSetFalse As Boolean = True, Optional ByVal colFormat As Boolean = True, Optional ByVal reeadOnly As Boolean = True, Optional ByVal sortColumns As Boolean = True)
        With grid
            If colHeadVisibleSetFalse Then .ColumnHeadersVisible = False
            For i As Integer = 0 To .ColumnCount - 1
                .Columns(i).ReadOnly = reeadOnly
                If .Columns(i).ValueType.Name Is GetType(Decimal).Name Then
                    If colFormat Then .Columns(i).DefaultCellStyle.Format = "0.000"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Double).Name Then
                    If colFormat Then .Columns(i).DefaultCellStyle.Format = "0.00"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Integer).Name Then
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Date).Name Then
                    If colFormat Then .Columns(i).DefaultCellStyle.Format = "dd/MM/yyyy"
                End If
                If Not sortColumns Then .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
                '.Columns(i).Resizable = DataGridViewTriState.False 
            Next
        End With
    End Sub
    Function GetRate_Purity(ByVal DDate As Date, ByVal PURITYID As String, Optional ByVal tran As OleDbTransaction = Nothing) As String
        Dim rate As Double = Nothing
        Dim sql As String = Nothing
        sql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
        sql += vbCrLf + "  WHERE RDATE = '" & DDate.ToString("yyyy-MM-dd") & "'"
        sql += vbCrLf + "  AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
        sql += vbCrLf + "  AND METALID = "
        sql += vbCrLf + "  (SELECT METALID FROM " & cnAdminDb & "..PURITYMAST "
        sql += vbCrLf + "      WHERE PURITYID = '" & PURITYID & "')"
        sql += vbCrLf + "  AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST "
        sql += vbCrLf + "      WHERE PURITYID = '" & PURITYID & "')"
        sql += vbCrLf + "  ORDER BY SNO DESC"
        rate = Val(objGPack.GetSqlValue(sql, , , tran))
        Return rate.ToString
    End Function

    Function GetRate(ByVal DDate As Date, ByVal CatCode As String, Optional ByVal tran As OleDbTransaction = Nothing, Optional ByVal Isdaterate As Boolean = False) As String
        Dim rate As Double = Nothing
        Dim sql As String = Nothing
        sql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
        sql += vbCrLf + "  WHERE RDATE = '" & DDate.ToString("yyyy-MM-dd") & "'"
        sql += vbCrLf + "  AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
        sql += vbCrLf + " AND METALID = "
        sql += vbCrLf + " (SELECT METALID FROM " & cnAdminDb & "..CATEGORY "
        sql += vbCrLf + "     WHERE CATCODE = '" & CatCode & "')"
        sql += vbCrLf + " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST "
        sql += vbCrLf + "     WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY "
        sql += vbCrLf + "        WHERE CATCODE = '" & CatCode & "'))"
        sql += vbCrLf + " ORDER BY SNO DESC"
        rate = Val(objGPack.GetSqlValue(sql, , , tran))
        Return rate.ToString
    End Function

    Function CheckRate_Today(ByVal DDate As Date, ByVal ITEMID As Integer, ByVal ITEMTYPENAME As String, Optional ByVal EnterRate As Decimal = Nothing) As String
        Dim Rate As Double = Nothing
        Dim sql As String = Nothing
nextiter:
        If ITEMTYPENAME <> "" Then
            Dim PURITYID As String = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & ITEMTYPENAME & "' AND RATEGET = 'Y'", , )
            Dim Purityrow As DataRow = GetSqlRow("SELECT METALID,PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = '" & PURITYID & "'", cn)
            If Purityrow Is Nothing Then ITEMTYPENAME = "" : GoTo nextiter
            Dim METALID As String = Purityrow(0).ToString
            Dim PURITY As Double = Val(Purityrow(1).ToString)
            sql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
            sql += vbCrLf + "  WHERE RDATE = '" & DDate.ToString("yyyy-MM-dd") & "'"
            sql += vbCrLf + "  AND SRATE = " & EnterRate
            ' sql += vbCrLf + "  AND RATEGROUP between (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE) and (SELECT MIN(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
            sql += vbCrLf + "  AND METALID = '" & METALID & "'"
            sql += vbCrLf + "  AND PURITY = " & PURITY
            sql += vbCrLf + "  ORDER BY SNO DESC"
            rate = Val(objGPack.GetSqlValue(sql, , , tran))
            If rate <> 0 Then Return True Else Return False

        Else
            Dim CATCODE As String
            Dim METALID As String
            Dim PURITYID As String
            Dim itemrow As DataRow = GetSqlRow("SELECT A.CATCODE,B.METALID,B.PURITYID FROM " & cnAdminDb & "..ITEMMAST A LEFT JOIN " & cnAdminDb & "..CATEGORY B ON A.CATCODE = B.CATCODE WHERE A.ITEMID = " & ITEMID & "", cn)
            CATCODE = itemrow(0).ToString
            METALID = itemrow(1).ToString
            PURITYID = itemrow(2).ToString
            sql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
            sql += vbCrLf + "  WHERE RDATE = '" & DDate.ToString("yyyy-MM-dd") & "'"
            sql += vbCrLf + "  AND SRATE = " & EnterRate
            'sql += vbCrLf + "  AND RATEGROUP between (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE) and (SELECT MIN(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
            sql += vbCrLf + " AND METALID = '" & METALID & "'"
            sql += vbCrLf + " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST "
            sql += vbCrLf + "     WHERE PURITYID = '" & PURITYID & "')"
            sql += vbCrLf + " ORDER BY SNO DESC"
            Rate = Val(objGPack.GetSqlValue(sql, , , tran))
            If Rate <> 0 Then Return True Else Return False
        End If
    End Function

    'Function funcGetAdminDbTableId(ByVal ctlId As String, ByRef tran As OleDbTransaction) As String
    '    Dim Sql As String = Nothing
    '    Dim dtCode As New DataTable
    '    Dim cmd As OleDbCommand
    '    Dim code As String = Nothing
    '    Sql = "SELECT ISNULL(MAX(CTLTEXT),0)+1 AS CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL "
    '    Sql += " WHERE CTLID = '" & ctlId & "'"
    '    cmd = New OleDbCommand(Sql, cn, tran)
    '    da = New OleDbDataAdapter(cmd)
    '    da.Fill(dtCode)
    '    If dtCode.Rows.Count > 0 Then
    '        code = Val(dtCode.Rows(0).Item("CTLTEXT").ToString)
    '        Sql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & code & "' WHERE CTLID = '" & ctlId & "'"
    '        cmd = New OleDbCommand(Sql, cn, tran)
    '        cmd.ExecuteNonQuery()
    '    End If
    '    Return code
    'End Function
    'Function funcGetTranDbName(ByVal cmpId As String, ByVal strcat As String, ByVal tranDate As Date) As String
    '    Dim dbName As String = Nothing
    '    Dim frmYear As String
    '    Dim toYear As String
    '    Dim month As Integer = tranDate.Month
    '    If month > 3 Then
    '        frmYear = Mid(tranDate.Year.ToString, 3, 2)
    '    Else
    '        frmYear = Mid((tranDate.Year - 1).ToString, 3, 2)
    '    End If
    '    toYear = Val(frmYear) + 1
    '    If toYear < 10 Then
    '        toYear = "0" + toYear
    '    End If
    '    dbName = cmpId + strcat + frmYear + toYear
    '    Return dbName
    'End Function

    Function funcGridStyle(ByVal obj As DataGridView) As Integer
        obj.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Window
        obj.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText
        obj.RowTemplate.Resizable = DataGridViewTriState.False
        obj.RowHeadersVisible = False
        obj.BackgroundColor = grdBackGroundColor
        obj.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        obj.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        obj.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        obj.RowTemplate.Height = 18
        obj.Font = New Font("VERDANA", 8, FontStyle.Regular)
    End Function

    'Function funcGetBoardRate(ByVal Billdate As Date, Optional ByVal tran As OleDbTransaction = Nothing) As String
    '    Dim strsql As String
    '    Dim cmd As OleDbCommand
    '    Dim da As OleDbDataAdapter
    '    Dim rate As Double = Nothing
    '    strsql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST"
    '    strsql += " WHERE RDATE = '" & Billdate.Date.ToString("yyyy-MM-dd") & "'"
    '    strsql += " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = '" & Billdate.Date.ToString("yyyy-MM-dd") & "')"
    '    strsql += " ORDER BY PURITY DESC"
    '    If tran Is Nothing Then
    '        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    Else
    '        cmd = New OleDbCommand(strsql, cn, tran)
    '    End If
    '    da = New OleDbDataAdapter(cmd)
    '    Dim dt As New DataTable
    '    da.Fill(dt)
    '    If dt.Rows.Count > 0 Then
    '        rate = Val(dt.Rows(0).Item("SRATE").ToString)
    '    End If
    '    Return IIf(rate > 0, Format(rate, "0.00"), "")
    'End Function
    'Function objGPack.DupCheck(ByVal qry As String, Optional ByVal TRAN As OleDbTransaction = Nothing) As Boolean
    '    Dim dt As New DataTable
    '    Dim cmd As OleDbCommand = Nothing
    '    If TRAN Is Nothing Then
    '        da = New OleDbDataAdapter(qry, cn)
    '    Else
    '        cmd = New OleDbCommand(qry, cn, TRAN)
    '        da = New OleDbDataAdapter(cmd)
    '    End If
    '    da.Fill(dt)
    '    If dt.Rows.Count > 0 Then
    '        Return True
    '    End If
    '    Return False
    'End Function
    Function funcCheckCostCentreStatusFalse() As Boolean
        Dim strSql As String = Nothing
        Dim dt As New DataTable
        strSql = " SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' AND CTLTEXT = 'Y'"
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return True
        End If
    End Function
    Function funcFiltVatExm(ByVal FiltVatExm As String) As String
        Dim str As String = Nothing
        If FiltVatExm = "F1" Then
            str += " "
        ElseIf FiltVatExm = "F2" Then
            str += " AND VATEXM='Y'"
        Else
            str += " AND VATEXM='N'"
        End If
        Return str
    End Function
    Function funcSampleActivate() As String
        Dim strSampleActivate As String = Nothing
        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='SAMPLEACTIVATE'", "CTLTEXT", "N")) = "Y" Then
            strSampleActivate = "Y"
        Else
            strSampleActivate = "N"
        End If
        Return strSampleActivate
    End Function
    Function funcOpenGrid(ByVal str As String, ByVal grid As DataGridView) As Integer
        Dim da As New OleDbDataAdapter
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dt)
        grid.DataSource = dt
    End Function
    Function funcSetNumberStyle(ByVal num As String, ByVal maxDigit As Integer) As String
        Dim temp As String = Nothing
        For cnt As Integer = 1 To maxDigit - num.Length
            temp += "0"
        Next
        num = temp + num
        Return num
    End Function
    'Function funcGetTrandbTableId(ByVal ctlId As String, ByRef tran As OleDbTransaction) As Integer
    '    Dim Sql As String = Nothing
    '    Dim dtCode As New DataTable
    '    Dim cmd As OleDbCommand
    '    Dim code As Integer = Nothing
    '    Sql = "SELECT ISNULL(MAX(CTLTEXT),0)+1 AS CTLTEXT FROM " & cnStockDb & "..SOFTCONTROLTRAN "
    '    Sql += " WHERE CTLID = '" & ctlId & "'"
    '    cmd = New OleDbCommand(Sql, cn, tran)
    '    da = New OleDbDataAdapter(cmd)
    '    da.Fill(dtCode)
    '    If dtCode.Rows.Count > 0 Then
    '        code = Val(dtCode.Rows(0).Item("CTLTEXT").ToString)
    '        Sql = " UPDATE " & cnStockDb & "..SOFTCONTROLTRAN SET CTLTEXT = '" & code & "' WHERE CTLID = '" & ctlId & "' AND CTLMODULE = 'X'"
    '        cmd = New OleDbCommand(Sql, cn, tran)
    '        cmd.ExecuteNonQuery()
    '    End If
    '    Return code
    'End Function

    Private Function GetListStr(ByVal list As List(Of String)) As String
        Dim _RetStr As String = Nothing
        For cnt As Integer = 0 To list.Count - 1
            _RetStr += "'" & list(cnt).ToUpper & "'"
            If cnt <> list.Count - 1 Then _RetStr += ","
        Next
        Return _RetStr
    End Function

    Public Function DeleteItem(ByVal mode As SyncMode, ByVal colName As List(Of String) _
    , ByVal delQry As String, ByVal checkStr As String _
    , ByVal neglectTable As String) As Boolean
        strSql = " SELECT CNAME "
        strSql += " ,'" & cnAdminDb & "..' AS DBNAME,(SELECT NAME FROM " & cnAdminDb & "..SYSOBJECTS WHERE ID = T.ID)AS TNAME"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT NAME CNAME,ID FROM " & cnAdminDb & "..SYSCOLUMNS WHERE NAME IN ("
        strSql += GetListStr(colName)
        strSql += " )"
        strSql += " )T"
        strSql += " UNION ALL"
        strSql += " SELECT CNAME "
        strSql += " ,'" & cnStockDb & "..' AS DBNAME,(SELECT NAME FROM " & cnStockDb & "..SYSOBJECTS WHERE ID = T.ID)AS TNAME"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT NAME CNAME,ID FROM " & cnStockDb & "..SYSCOLUMNS WHERE NAME IN ("
        strSql += GetListStr(colName)
        strSql += " )"
        strSql += " )T"
        Dim _DtTableColl As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(_DtTableColl)
        Dim _chkQry As String = Nothing
        For cnt As Integer = 0 To _DtTableColl.Rows.Count - 1
            If _DtTableColl.Rows(cnt).Item("TNAME").ToString.ToUpper = neglectTable.ToUpper Then Continue For
            _chkQry += " SELECT DISTINCT " & _DtTableColl.Rows(cnt).Item("CNAME").ToString
            _chkQry += " FROM " & _DtTableColl.Rows(cnt).Item("DBNAME").ToString _
                        & _DtTableColl.Rows(cnt).Item("TNAME").ToString + vbCrLf
            _chkQry += " WHERE " & _DtTableColl.Rows(cnt).Item("CNAME").ToString & " = "
            _chkQry += " '" & checkStr & "'"
            If cnt <> _DtTableColl.Rows.Count - 1 Then _chkQry += " UNION ALL"
        Next
        If _chkQry.EndsWith("UNION ALL") Then
            _chkQry = Left(_chkQry, _chkQry.Length - 9)
        End If
        If _chkQry <> Nothing Then Return DeleteItem(mode, _chkQry, delQry)
    End Function

    Public Function DeleteItem(ByVal type As SyncMode, ByVal checkQry As String, ByVal delQry As String) As Boolean
        Try
            'Jaganathan
            ''ProgressBarShow()
            ''ProgressBarStep("Availability Checking..", 20)
            If objGPack.DupCheck(checkQry) Then
                ''ProgressBarHide()
                MsgBox("This Item Already Used. So Cannot Delete this Item")
            Else
                ''ProgressBarHide()
                If MessageBox.Show("Do you want Delete this Item", "Delete Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.OK Then
                    'JAGANATHAN
                    ''ProgressBarShow()
                    ''ProgressBarStep("Deleting..", 20)
                    Try
                        tran = Nothing
                        tran = cn.BeginTransaction
                        'ExecQuery(type, delQry, cn, tran, , True)
                        ExecQuery(type, delQry, cn, tran, , , , , True)
                        tran.Commit()
                        tran = Nothing
                        MsgBox("Successfully Deleted..")
                        Return True
                    Catch ex As Exception
                        If tran IsNot Nothing Then tran.Rollback()
                        MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                    End Try
                End If
            End If
        Catch ex As Exception
            MsgBox("Message : " + ex.Message + vbCrLf + "Stack Trace    : " + ex.StackTrace)
        Finally
            ''ProgressBarHide()
        End Try
    End Function

    Public Function DeleteItem_chkgrid(ByVal type As SyncMode, ByVal checkQry As String, ByVal delQry As String) As Boolean
        Try
            If objGPack.DupCheck(checkQry) Then
                MsgBox("This Item Already Used. So Cannot Delete this Item")
            Else
                Try
                    tran = Nothing
                    tran = cn.BeginTransaction
                    ExecQuery(type, delQry, cn, tran, , , , , True)
                    tran.Commit()
                    tran = Nothing
                    Return True
                Catch ex As Exception
                    If tran IsNot Nothing Then tran.Rollback()
                    MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                End Try
            End If
        Catch ex As Exception
            MsgBox("Message : " + ex.Message + vbCrLf + "Stack Trace    : " + ex.StackTrace)
        Finally
        End Try
    End Function

    Public Sub ProgressBarShow(Optional ByVal progressStyle As ProgressBarStyle = ProgressBarStyle.Blocks)
        'JAGANATHAN
        'Main.tStripBarStatus.Text = ""
        'Main.pBar.Style = progressStyle
        'Main.pBar.Value = 0
        'Main.pBar.Maximum = 100
        'Main.pBar.Step = 5
        'Main.pBar.Visible = True
        'Main.Refresh()
    End Sub
    Public Sub ProgressBarStep(Optional ByVal statusComment As String = Nothing, Optional ByVal stepValue As Integer = 5)
        'JAGANATHAN
        ''If Main.pBar.Value >= Main.pBar.Maximum - stepValue Then
        ''    Main.pBar.Value = 0
        ''Else
        ''    Main.pBar.Value = Main.pBar.Value + stepValue
        ''End If
        ''Main.tStripBarStatus.Text = statusComment + IIf(statusComment <> Nothing, "....     ", "")
        ''Main.Refresh()
    End Sub
    Public Sub ProgressBarHide()
        'JAGANATHAN
        ''Main.pBar.Value = Main.pBar.Maximum
        ''Main.Refresh()
        ''Main.pBar.Visible = False
        ''Main.tStripBarStatus.Text = ""
        ''Main.Refresh()
    End Sub

    Public Sub MsgBoxx(ByVal Prompt1 As String, Optional ByVal Title As String = "MessageBox")
        ''Dim obj As New frmMsgDiag(Prompt)
    End Sub

    Public Function IsActive(ByVal ctrl As Control) As Boolean
        If TypeOf (ctrl) Is DataGridView Then
            If ctrl.Focused Then Return True
        Else
            For cnt As Integer = 0 To ctrl.Controls.Count - 1
                If TypeOf (ctrl.Controls(cnt)) Is GroupBox Then
                    Return IsActive(ctrl.Controls(cnt))
                ElseIf TypeOf (ctrl.Controls(cnt)) Is Panel Then
                    Return IsActive(ctrl.Controls(cnt))
                ElseIf TypeOf (ctrl.Controls(cnt)) Is SplitContainer Then
                    Return IsActive(ctrl.Controls(cnt))
                ElseIf TypeOf (ctrl.Controls(cnt)) Is TabControl Then
                    Return IsActive(ctrl.Controls(cnt))
                ElseIf TypeOf (ctrl.Controls(cnt)) Is TabPage Then
                    Return IsActive(ctrl.Controls(cnt))
                Else
                    If ctrl.Controls(cnt).Focused Then Return True
                    'Else
                    '    If ctrl.Focused Then Return True
                End If
            Next
        End If
    End Function

    Public Sub ComboScript(ByRef cmb As ComboBox, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim sComboText As String = ""
        Dim iLoop As Integer
        Dim sTempString As String
        If e.KeyCode >= 65 And e.KeyCode <= 90 Then
            'only look at letters A-Z
            sTempString = cmb.Text
            If Len(sTempString) = 1 Then sComboText = sTempString
            For iLoop = 0 To (cmb.Items.Count - 1)
                If UCase((sTempString & Mid$(cmb.Items.Item(iLoop), _
                  Len(sTempString) + 1))) = UCase(cmb.Items.Item(iLoop)) Then
                    cmb.SelectedIndex = iLoop
                    cmb.Text = cmb.Items.Item(iLoop)
                    cmb.SelectionStart = Len(sTempString)
                    cmb.SelectionLength = Len(cmb.Text) - (Len(sTempString))
                    sComboText = sComboText & Mid$(sTempString, Len(sComboText) + 1)
                    Exit For
                Else
                    If InStr(UCase(sTempString), UCase(sComboText)) Then
                        sComboText = sComboText & Mid$(sTempString, Len(sComboText) _
                        + 1)
                        cmb.Text = sComboText
                        cmb.SelectionStart = Len(cmb.Text)
                    Else
                        sComboText = sTempString
                    End If
                End If
            Next iLoop
        End If
    End Sub

    Public Function GetEstNoValue(ByVal BillControlId As String, ByVal tran As OleDbTransaction) As Integer
GetNewBillNo:
        Dim NewBillNo As Integer = Nothing
        strSql = " SELECT 'CHECK' FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ESTBILLNO'"
        If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then 'TEMPBILLNO EXIST
            strSql = " SELECT 'CHECK' FROM TEMP" & systemId & "ESTBILLNO WHERE CTLID = '" & BillControlId & "'"
            If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then 'BILLNO ALREADY GENERATED
                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM TEMP" & systemId & "ESTBILLNO WHERE CTLID = '" & BillControlId & "'"
                NewBillNo = Val(objGPack.GetSqlValue(strSql, , , tran))
            Else 'NEWBILLNO GENERATING HERE
                GoTo GenerateNewBillNo
            End If
        Else 'TEMPBILLNO NOT EXIST
GenerateNewBillNo:
            strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & BillControlId & "' AND COMPANYID = '" & _MainCompId & "'"
            If strbCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strbCostid & "'"
            NewBillNo = Val(objGPack.GetSqlValue(strSql, , , tran))

            strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & (NewBillNo + 1).ToString & "'"
            strSql += " WHERE CTLID ='" & BillControlId & "' AND COMPANYID = '" & _MainCompId & "'"
            If strbCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strbCostid & "'"
            strSql += " AND CONVERT(INT,CTLTEXT) = '" & NewBillNo & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            If cmd.ExecuteNonQuery() = 0 Then
                If strBCostid <> Nothing Then Return 0
                GoTo GetNewBillNo
            End If
            strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ESTNO')>0"
            strSql += " 	BEGIN"
            strSql += " 	INSERT INTO TEMP" & systemId & "ESTNO(CTLID,CTLTEXT)SELECT '" & BillControlId & "' CTLID,'" & NewBillNo + 1 & "'CTLTEXT"
            strSql += " 	END"
            strSql += " ELSE"
            strSql += " 	BEGIN "
            strSql += " 	CREATE TABLE TEMP" & systemId & "ESTNO(SNO INT IDENTITY(1,1),CTLID VARCHAR(50),CTLTEXT VARCHAR(200))"
            strSql += " 	INSERT INTO TEMP" & systemId & "ESTNO(CTLID,CTLTEXT)SELECT '" & BillControlId & "' CTLID,'" & NewBillNo + 1 & "'CTLTEXT"
            strSql += " 	END"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
        End If
        Return NewBillNo + 1
    End Function

    Public Sub InsertBillNo(ByVal BillControlId As String, ByVal BillNo As Integer, ByVal tran As OleDbTransaction)
        strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')>0"
        strSql += " 	BEGIN"
        strSql += " 	INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "BILLNO(CTLID,CTLTEXT)SELECT '" & BillControlId & "' CTLID,'" & BillNo & "'CTLTEXT"
        strSql += " 	END"
        strSql += " ELSE"
        strSql += " 	BEGIN "
        strSql += " 	CREATE TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO(SNO INT IDENTITY(1,1),CTLID VARCHAR(50),CTLTEXT VARCHAR(200))"
        strSql += " 	INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "BILLNO(CTLID,CTLTEXT)SELECT '" & BillControlId & "' CTLID,'" & BillNo & "'CTLTEXT"
        strSql += " 	END"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
    End Sub


    Public Function GetBillNoValue(ByVal BillControlId As String, ByVal tran As OleDbTransaction) As Integer
        Dim isfirst As Boolean = True
GetNewBillNo:
        Dim NewBillNo As Integer = Nothing
        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO'"
        If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then 'TEMPBILLNO EXIST
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..TEMP" & systemId & "BILLNO WHERE CTLID = '" & BillControlId & "'"

            If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then 'BILLNO ALREADY GENERATED
                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnAdminDb & "..TEMP" & systemId & "BILLNO WHERE CTLID = '" & BillControlId & "'"

                NewBillNo = Val(objGPack.GetSqlValue(strSql, , , tran))
                Return NewBillNo
            Else 'NEWBILLNO GENERATING HERE
                isfirst = False
                GoTo GenerateNewBillNo
            End If
        Else 'TEMPBILLNO NOT EXIST

GenerateNewBillNo:
            strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & BillControlId & "' AND COMPANYID = '" & strCompanyId & "'"
            If strBCostid <> Nothing And isfirst Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
            NewBillNo = Val(objGPack.GetSqlValue(strSql, , , tran))

            strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & (NewBillNo + 1).ToString & "'"
            strSql += " WHERE CTLID ='" & BillControlId & "' AND COMPANYID = '" & strCompanyId & "'"
            strSql += " AND CONVERT(INT,CTLTEXT) = '" & NewBillNo & "'"
            If strBCostid <> Nothing And isfirst Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            If cmd.ExecuteNonQuery() = 0 Then
                If strBCostid <> Nothing Then Return 0
                isfirst = False
                GoTo GetNewBillNo
            End If
            InsertBillNo(BillControlId, NewBillNo + 1, tran)
            Return NewBillNo + 1
        End If
    End Function

    Public Function GetBillControlValue(ByVal BillControlId As String, ByVal tran As OleDbTransaction, Optional ByVal Nextiter As Boolean = False) As String
        If strBCostid <> Nothing And Not Nextiter Then
            Return UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & BillControlId & "' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(COSTID,'') ='" & strBCostid & "'", , , tran))
        Else
            Return UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & BillControlId & "' AND COMPANYID = '" & strCompanyId & "'", , , tran))
        End If

    End Function

    Function GetGvNo(ByVal tran As OleDbTransaction, Optional ByVal withupdated As Boolean = False) As String
        Dim Gvnumber As String = Nothing
        strSql = " SELECT CTLTEXT AS GVNOS FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'GVNUMBER'"
        If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
        Gvnumber = objGPack.GetSqlValue(strSql, , "1", tran)

        Dim str As String = Nothing
        If IsNumeric(Gvnumber) Then
            Gvnumber = Val(Gvnumber) + 1
        Else
            Dim fPart As String = Nothing
            Dim sPart As String = Nothing
            For Each c As Char In Gvnumber
                If IsNumeric(c) Then
                    sPart += c
                Else
                    fPart += c
                End If
            Next
            Gvnumber = fPart + (Val(sPart) + 1).ToString
        End If
        Dim Gvnos As String
        Gvnos = Getprefix(Gvnumber, False, tran)
        Gvnos = Gvnos + Gvnumber
        If withupdated Then UpdateGvNo(Gvnos, cnCostId, tran)
        Return Gvnos
    End Function


    Function Getprefix(ByRef Gvno As String, ByVal upd As Boolean, ByVal tran As OleDbTransaction) As String
        ' strSql = " SELECT CTLTEXT AS GVPREFIX FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'GVPREFIX'"
        Dim tagPrefix As String = GetAdmindbSoftValue("GVPREFIX", "GV,8", tran)
        Dim prefixarray() As String = Split(tagPrefix, ",")
        Dim prefixstr As String = prefixarray(0)
        Dim prefixlen As Integer
        If prefixarray.Length > 1 Then prefixlen = Val("" & prefixarray(1)) Else prefixlen = 1
        Dim Gvnos As String = ""
        Dim gcostid As String = GetCostId(cnCostId) & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString
        If upd = False Then
            If Len(prefixstr & gcostid & Gvno.Trim) < prefixlen Then
                For cnt As Integer = 1 To prefixlen - Len(prefixstr & gcostid & Gvno)
                    Gvnos += "0"
                Next
                Gvnos = prefixstr & gcostid & Gvnos
            Else
                Gvnos = prefixstr & gcostid
            End If
        Else
            Gvnos = prefixstr & gcostid
        End If
        Return Gvnos

    End Function
    Public Sub UpdateGvNo(ByVal Gvno As String, ByVal COSTID As String, ByVal tran As OleDbTransaction)

        Dim GvPrefix As String '= GetAdmindbSoftValue("GVPREFIX", "GV,8", tran)
        GvPrefix = Getprefix(Gvno, True, tran)

        Dim updGvNo As String = Nothing
        If GvPrefix.Length > 0 Then
            updGvNo = Gvno.Replace(GvPrefix, "")
        Else
            updGvNo = GvPrefix
        End If
        strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT  = '" & Val(updGvNo) & "' WHERE CTLID = 'GVNUMBER'"
        If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
        ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)
    End Sub

    Public Sub InsertIntoBillControl( _
      ByVal ctlId As String, ByVal ctlName As String, ByVal ctlType As String _
      , ByVal ctlMode As String, ByVal ctlText As String, ByVal ctlModule As String, Optional ByVal TRAN As OleDbTransaction = Nothing)
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT 1 FROM " & cnStockDb & "..TBILLCONTROL WHERE CTLID = '" & ctlId & "' AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
        If TRAN Is Nothing Then
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Else
            cmd = New OleDbCommand(strSql, cn, TRAN)
        End If
        da = New OleDbDataAdapter(cmd)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Exit Sub
        End If
        strSql = " INSERT INTO " & cnStockDb & "..TBILLCONTROL"
        strSql += " (CTLID,CTLNAME,CTLTYPE,CTLTEXT,CTLMODE,CTLMODULE,COMPANYID,COSTID)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & ctlId & "','" & ctlName & "','" & ctlType & "'"
        strSql += " ,'" & ctlText & "','" & ctlMode & "','" & ctlModule & "'"
        strSql += " ,'" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += " ,'" & strBCostid.ToString & "'" Else strSql += " ,''"
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        If TRAN IsNot Nothing Then cmd.Transaction = TRAN
        cmd.ExecuteNonQuery()
    End Sub

    'Public Sub TextScript(ByRef txt As TextBox, ByRef lstSearch As ListBox, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Dim sComboText As String = ""
    '    Dim iLoop As Integer
    '    Dim sTempString As String
    '    If e.KeyCode >= 65 And e.KeyCode <= 90 Then
    '        'only look at letters A-Z
    '        sTempString = txt.Text
    '        If Len(sTempString) = 1 Then sComboText = sTempString
    '        For iLoop = 0 To (lstSearch.Items.Count - 1)
    '            If UCase((sTempString & Mid$(lstSearch.Items.Item(iLoop), _
    '              Len(sTempString) + 1))) = UCase(lstSearch.Items.Item(iLoop)) Then
    '                lstSearch.SelectedIndex = iLoop
    '                txt.Text = lstSearch.Items.Item(iLoop)
    '                txt.SelectionStart = Len(sTempString)
    '                txt.SelectionLength = Len(txt.Text) - (Len(sTempString))
    '                sComboText = sComboText & Mid$(sTempString, Len(sComboText) + 1)
    '                Exit For
    '            Else
    '                If InStr(UCase(sTempString), UCase(sComboText)) Then
    '                    sComboText = sComboText & Mid$(sTempString, Len(sComboText) _
    '                    + 1)
    '                    txt.Text = sComboText
    '                    txt.SelectionStart = Len(txt.Text)
    '                Else
    '                    sComboText = sTempString
    '                End If
    '            End If
    '        Next iLoop
    '    End If
    'End Sub

    Public Sub TextScript(ByRef txt As TextBox, ByRef lstSearch As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim sComboText As String = ""
        Dim iLoop As Integer
        Dim sTempString As String
        Dim cmblst As Object = Nothing
        If TypeOf lstSearch Is ComboBox Then
            cmblst = New ComboBox
            cmblst = lstSearch
        ElseIf TypeOf lstSearch Is ListBox Then
            cmblst = New ListBox
            cmblst = lstSearch
        End If
        'CType(lstSearch.items.item(0), DataRowView).Item(0).ToString
        If e.KeyCode >= 65 And e.KeyCode <= 90 Then
            'only look at letters A-Z
            sTempString = txt.Text
            If Len(sTempString) = 1 Then sComboText = sTempString
            For iLoop = 0 To (cmblst.Items.Count - 1)
                'If UCase((sTempString & Mid$(CType(lstSearch.items.item(0), DataRowView).Item(0).ToString, _
                'Len(sTempString) + 1))) = UCase(CType(lstSearch.items.item(0), DataRowView).Item(0).ToString) Then
                '    lstSearch.SelectedIndex = iLoop
                '    txt.Text = lstSearch.Items.Item(iLoop)
                '    txt.SelectionStart = Len(sTempString)
                '    txt.SelectionLength = Len(txt.Text) - (Len(sTempString))
                '    sComboText = sComboText & Mid$(sTempString, Len(sComboText) + 1)
                '    Exit For
                'Else
                '    If InStr(UCase(sTempString), UCase(sComboText)) Then
                '        sComboText = sComboText & Mid$(sTempString, Len(sComboText) _
                '        + 1)
                '        txt.Text = sComboText
                '        txt.SelectionStart = Len(txt.Text)
                '    Else
                '        sComboText = sTempString
                '    End If
                'End If

                If UCase((sTempString & Mid$(cmblst.Items.Item(iLoop), _
                  Len(sTempString) + 1))) = UCase(cmblst.Items.Item(iLoop)) Then
                    cmblst.SelectedIndex = iLoop
                    txt.Text = cmblst.Items.Item(iLoop)
                    txt.SelectionStart = Len(sTempString)
                    txt.SelectionLength = Len(txt.Text) - (Len(sTempString))
                    sComboText = sComboText & Mid$(sTempString, Len(sComboText) + 1)
                    Exit For
                Else
                    If InStr(UCase(sTempString), UCase(sComboText)) Then
                        sComboText = sComboText & Mid$(sTempString, Len(sComboText) _
                        + 1)
                        txt.Text = sComboText
                        txt.SelectionStart = Len(txt.Text)
                    Else
                        sComboText = sTempString
                    End If
                End If
            Next iLoop
        End If
    End Sub

    Function funcGridViewGrouping(ByVal dt As DataTable, Optional ByVal SourceCol As Integer = 0, Optional ByVal DestCol As Integer = 1, Optional ByVal WithStoneField As Boolean = False, Optional ByVal sortFieldAlter As Boolean = False, Optional ByVal secondDestCol As Integer = -1, Optional ByVal RESULT As Boolean = False) As DataSet
        Dim column1ToString As String = "#@$^&*@#!@#$#@!"
        Dim column2ToString As String = "##%&%@#!!@%#$#!"
        Dim ds As New DataSet
        ds.Clear()
        Dim subTotalDt As New DataTable("SubTotal")
        Dim titleDt As New DataTable("Title")
        subTotalDt.Clear()
        titleDt.Clear()
        titleDt.Columns.Add("Title")
        subTotalDt.Columns.Add("SubTotal")

        Dim tempDt As New DataTable("Result")
        tempDt.Clear()
        tempDt.Columns.Add("PARTICULAR", GetType(String))
        For cnt As Integer = 0 To dt.Columns.Count - 1
            tempDt.Columns.Add(dt.Columns(cnt).ColumnName, GetType(String))
        Next
        Dim ro As DataRow = Nothing
        Dim roSubTotal As DataRow = Nothing
        Dim roTitle As DataRow = Nothing
        Select Case secondDestCol
            Case -1

            Case Is <> -1

        End Select
        For rowIndex As Integer = 0 To dt.Rows.Count - 1
            ro = tempDt.NewRow
            With dt.Rows(rowIndex)
                If RESULT = True Then
                    If .Item("RESULT").ToString = "2" Then
                        .Item(SourceCol) = "SUB TOTAL"
                    End If
                    If .Item("RESULT").ToString = "3" Then
                        .Item(SourceCol) = "GRAND TOTAL"
                    End If
                End If
                If .Item(SourceCol).ToString <> "SUB TOTAL" And .Item(SourceCol).ToString <> "GRAND TOTAL" And .Item(DestCol).ToString <> "SUB TOTAL" And .Item(DestCol).ToString <> "GRAND TOTAL" Then
                    If column1ToString <> .Item(SourceCol).ToString Then
                        If WithStoneField = True Then
                            If .Item("Stone").ToString <> "2" Then
                                ro(0) = .Item(SourceCol).ToString
                                For cnt As Integer = 1 To dt.Columns.Count - 1
                                    ro(cnt) = ""
                                Next
                                column1ToString = .Item(SourceCol).ToString
                                tempDt.Rows.Add(ro)
                                ''Adding Title Index
                                roTitle = titleDt.NewRow
                                roTitle("Title") = rowIndex + titleDt.Rows.Count
                                titleDt.Rows.Add(roTitle)
                            End If
                        Else
                            ro(0) = .Item(SourceCol).ToString
                            For cnt As Integer = 1 To dt.Columns.Count - 1
                                ro(cnt) = ""
                            Next
                            column1ToString = .Item(SourceCol).ToString
                            tempDt.Rows.Add(ro)
                            ''Adding Title Index
                            roTitle = titleDt.NewRow
                            roTitle("Title") = rowIndex + titleDt.Rows.Count
                            titleDt.Rows.Add(roTitle)
                        End If
                    End If

                End If
                If .Item(SourceCol).ToString = "SUB TOTAL" Or .Item(DestCol).ToString = "SUB TOTAL" Then
                    ''Adding Group SubTotal Index into SubTotal Table
                    roSubTotal = subTotalDt.NewRow
                    roSubTotal("SubTotal") = rowIndex + titleDt.Rows.Count
                    subTotalDt.Rows.Add(roSubTotal)
                End If
                ro = tempDt.NewRow
                If sortFieldAlter = False Then
                    ro(0) = .Item(SourceCol).ToString
                    If Trim(.Item(DestCol).ToString) <> "" Then
                        ro(0) = .Item(DestCol).ToString
                    End If
                    For cnt As Integer = 0 To dt.Columns.Count - 1
                        ro(cnt + 1) = .Item(cnt).ToString
                    Next
                Else
                    ro(0) = .Item(DestCol).ToString
                    For cnt As Integer = 0 To dt.Columns.Count - 1
                        ro(cnt + 1) = .Item(cnt).ToString
                    Next
                End If
                tempDt.Rows.Add(ro)
            End With
        Next
        ds.Tables.Add(tempDt)
        ds.Tables.Add(subTotalDt)
        ds.Tables.Add(titleDt)
        Return ds
    End Function
    Public Sub AmountValidation(ByVal sender As TextBox, ByVal e As KeyPressEventArgs, Optional ByVal amtRnd As Integer = 2)
        If e.KeyChar = "." And sender.Text.Contains(".") Then
            e.Handled = True
            Exit Sub
        End If
        Select Case e.KeyChar
            Case "+", "-", "0" To "9", "."
            Case ChrW(Keys.Enter), ChrW(Keys.Escape), ChrW(Keys.Space), Chr(Keys.Back)
                Exit Sub
            Case Else
                e.Handled = True
                sender.Focus()
        End Select
        If sender.Text.Contains(".") Then
            Dim dotPos As Integer = InStr(sender.Text, ".", CompareMethod.Text)
            Dim sp() As String = sender.Text.Split(".")
            Dim curPos As Integer = sender.SelectionStart
            If sp.Length >= 2 Then
                If curPos >= dotPos Then
                    If sp(1).Length > amtRnd - 1 Then
                        e.Handled = True
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub WeightValidation(ByVal sender As TextBox, ByVal e As KeyPressEventArgs, Optional ByVal wtRnd As Integer = 3)
        If e.KeyChar = "." And sender.Text.Contains(".") Then
            e.Handled = True
            Exit Sub
        End If
        Select Case e.KeyChar
            Case "+", "-", "0" To "9", "."
            Case ChrW(Keys.Enter), ChrW(Keys.Escape), ChrW(Keys.Space), Chr(Keys.Back)
                Exit Sub
            Case Else
                e.Handled = True
                sender.Focus()
        End Select
        If sender.Text.Contains(".") Then
            Dim dotPos As Integer = InStr(sender.Text, ".", CompareMethod.Text)
            Dim sp() As String = sender.Text.Split(".")
            Dim curPos As Integer = sender.SelectionStart
            If sp.Length >= 2 Then
                If curPos >= dotPos Then
                    If sp(1).Length > wtRnd - 1 Then
                        e.Handled = True
                    End If
                End If
            End If
        End If
    End Sub
    Public Function GetServerDate(Optional ByVal tran As OleDbTransaction = Nothing) As String
        If GetAdmindbSoftValue("ENTRYDATE", "Y", tran) = "Y" Then
            Dim dd As Date = CType(objGPack.GetSqlValue("SELECT CONVERT(SMALLDATETIME,CONVERT(VARCHAR(12),GETDATE(),101))", , , tran), Date)
            Return Format(dd, "yyyy-MM-dd")
        Else
            Return Format(Today.Date, "yyyy-MM-dd")
        End If
    End Function

    Public Function GetServerTime(Optional ByVal tran As OleDbTransaction = Nothing) As Date
        If GetAdmindbSoftValue("ENTRYDATE", "Y", tran) = "Y" Then
            Return CType(objGPack.GetSqlValue("SELECT CONVERT(SMALLDATETIME,CONVERT(VARCHAR(12),GETDATE(),108))", , , tran), Date)
        Else
            Return Date.Now.ToLongTimeString
        End If
    End Function

    ''JAGANATHAN -- 2015-10-15
    ''Public Sub CompanySelectionMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    ''    'If Not Main.tStripCompanySelection.Visible Then Exit Sub
    ''    Select Case UCase(CType(sender, ToolStripMenuItem).Tag)
    ''        Case "MAIN"
    ''            Main.tStripCompanySelection_Click(Main, New EventArgs)
    ''        Case ""
    ''        Case Else
    ''            strSql = " SELECT COMPANYID,COMPANYNAME FROM " & cnAdminDb & "..COMPANY"
    ''            strSql += " WHERE ISNULL(SHORTKEY,'') = '" & CType(sender, ToolStripMenuItem).Tag & "'"
    ''            Dim dtCom As New DataTable
    ''            da = New OleDbDataAdapter(strSql, cn)
    ''            da.Fill(dtCom)
    ''            If dtCom.Rows.Count > 0 Then
    ''                strCompanyId = ""
    ''                strCompanyId = dtCom.Rows(0).Item("COMPANYID").ToString
    ''                strCompanyName = dtCom.Rows(0).Item("COMPANYNAME").ToString
    ''                BrighttechREPORT.Globalvariables.strCompanyName = strCompanyName
    ''                BrighttechREPORT.Globalvariables.strCompanyId = strCompanyId
    ''                Main.Text = dtCom.Rows(0).Item("COMPANYNAME").ToString + IIf(_Demo, " " & _DemoLImitDays & " Days Trial", "") + " Version : " + VERSION
    ''            End If
    ''    End Select
    ''End Sub

    Public Function GetQryStringForSp(ByVal Source As String, ByVal ChkTable As String, ByVal SelectCol As String, ByVal ChkCol As String, Optional ByVal WithQuotes As Boolean = False, Optional ByVal sep As String = ",") As String
        Dim ret As String = ""
        If Source = "" Or Source.ToUpper = "ALL" Then Return Source
        Dim Qry As String = ""
        Dim sp() As String = Source.ToString.Split(sep)
        For Each s As String In sp
            Qry = " SELECT " & SelectCol & " FROM " & ChkTable & " WHERE " & ChkCol & " = '" & Trim(s) & "'"
            If WithQuotes Then ret += "'"
            ret += objGPack.GetSqlValue(Qry)
            If WithQuotes Then ret += "'"
            ret += ","
        Next
        If ret <> "" Then
            ret = Mid(ret, 1, ret.Length - 1)
        End If
        Return ret
    End Function
    Public Function GetQryString(ByVal lst As List(Of String), Optional ByVal withSingleQt As Boolean = True) As String
        Dim retStr As String = ""
        For cnt As Integer = 0 To lst.Count - 1
            If withSingleQt Then retStr += "'"
            retStr += lst.Item(cnt).ToString
            If withSingleQt Then retStr += "'"
            retStr += ","
        Next
        If retStr <> "" Then retStr = Mid(retStr, 1, retStr.Length - 1)
        Return retStr
    End Function

    Public Function GetQryString(ByVal Source As String, Optional ByVal sep As String = ",") As String
        Dim ret As String = ""
        Dim sp() As String = Source.ToString.Split(sep)
        For Each s As String In sp
            ret += "'" & Trim(s) & "',"
        Next
        If ret <> "" Then
            ret = Mid(ret, 1, ret.Length - 1)
        End If
        Return ret
    End Function



#Region "Get Batchno & BillNo & Sno"

    Public Function GetTranDbSoftControlValue(ByVal ctlId As String, Optional ByVal upDate As Boolean = True, Optional ByVal trn As OleDbTransaction = Nothing) As String
        Dim ctlValue As Integer
GETCTLVALUE:
        strSql = " SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..SOFTCONTROLTRAN WHERE CTLID = '" & ctlId & "'"
        If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
        ctlValue = Val(objGPack.GetSqlValue(strSql, , , trn))
        If upDate Then
            strSql = "UPDATE " & cnStockDb & "..SOFTCONTROLTRAN SET CTLTEXT = " & ctlValue + 1 & " "
            strSql += " WHERE CTLID = '" & ctlId & "' AND CONVERT(INT,CTLTEXT) = " & ctlValue & ""
            If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
            cmd = New OleDbCommand(strSql, cn, trn)
            If cmd.ExecuteNonQuery() = 0 Then

                GoTo GETCTLVALUE
            End If
        End If
        Return (ctlValue + 1).ToString
    End Function


    Public Function GetAdmindbSoftValue(ByVal ctlId As String, Optional ByVal defValue As String = "", Optional ByVal ttran As OleDbTransaction = Nothing) As String
        Dim strSql As String = Nothing
        Dim Globvalue As Boolean = False
        Dim CtlText As String = ""
NextIteration:
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & ctlId & "'"
        If strBCostid <> Nothing And Not Globvalue Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
        If strBCostid <> Nothing Then
            CtlText = objGPack.GetSqlValue(strSql, , , ttran)
            If CtlText.ToString.Trim.Length = 0 And Globvalue Then CtlText = defValue
        Else
            CtlText = objGPack.GetSqlValue(strSql, , defValue, ttran)
        End If
        If CtlText.ToString.Trim.Length >= 1 Then
            Return CtlText.ToString.Trim.ToUpper
        Else
            If strBCostid <> Nothing And Globvalue = False Then Globvalue = True : GoTo NextIteration Else CtlText = defValue
            Return CtlText.ToString.Trim
        End If
    End Function
    Public Function GetAdmindbSoftValueNew(ByVal ctlId As String, ByVal CCn As OleDbConnection, Optional ByVal defValue As String = "", Optional ByVal ttran As OleDbTransaction = Nothing) As String
        Dim strSql As String = Nothing
        Dim CtlText As String = ""
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & ctlId & "'"
        If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
        CtlText = BrighttechPack.GlobalMethods.GetSqlValue(CCn, strSql, , defValue, ttran)
        If CtlText.ToString.Trim.Length >= 1 Then
            Return CtlText.ToString.Trim.ToUpper
        Else
            Return CtlText.ToString.Trim
        End If
    End Function



    Public Function GetAdmindbSoftValueAll(Optional ByVal ctrlstring As String = "") As DataTable
        ctrlstring = Replace(ctrlstring, "Where ctlid in (", "")
        ctrlstring = Replace(ctrlstring, ")", "")
        If Not ctrlstring Is Nothing Then ctrlstring = """" & ctrlstring & """" Else ctrlstring = "''"
        Dim strsql As String
        strsql = " EXEC " & cnAdminDb & "..SP_GETCONTROLS @DBNAME='" & cnAdminDb & "',@IDS=" & ctrlstring
        Dim cmd As OleDbCommand = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
        Dim da As OleDbDataAdapter = New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        Dim dtsoft As New DataTable
        dtsoft = dss.Tables(0)

        'Dim strSql As String = Nothing
        'Dim dtsoft1 As New DataTable
        'strsql = " SELECT CTLID,CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL "
        'If ctrlstring <> "" Then strsql = strsql & ctrlstring
        'da = New OleDbDataAdapter(strsql, cn)

        'da.Fill(dtsoft1)

        Return dtsoft
    End Function

    Public Function GetAdmindbSoftValuefromDt(ByVal dt As DataTable, ByVal softid As String, ByVal softval As String) As String
        If Not dt Is Nothing Then
            Dim mQrystr As String = "CTLID='" & softid & "'"
            Dim drow() As DataRow = Nothing
            drow = dt.Select(mQrystr)
            If drow.Length > 0 And Not (drow Is Nothing) Then Return drow(0).Item("CTLTEXT").ToString Else Return softval
        End If
    End Function
    Public Function formatchk(ByVal formatstr As String, ByVal inputvalue As String) As Boolean
        Dim formatlen As Integer = Len(formatstr)
        Dim inputlen As Integer = Len(inputvalue)
        If formatlen <> inputlen Then Return False
        For I As Integer = 1 To formatlen
            If Char.IsLetter(Mid(formatstr, I, 1)) <> Char.IsLetter(Mid(inputvalue, I, 1)) Then Return False
            If Char.IsDigit(Mid(formatstr, I, 1)) <> Char.IsDigit(Mid(inputvalue, I, 1)) Then Return False
        Next
        Return True
    End Function
    Public Function formatchkGST(ByVal formatstr As String, ByVal inputvalue As String) As Boolean
        Dim formatlen As Integer = Len(formatstr)
        Dim inputlen As Integer = Len(inputvalue)
        If formatlen <> inputlen Then Return False
        For I As Integer = 1 To formatlen - 3
            If Char.IsLetter(Mid(formatstr, I, 1)) <> Char.IsLetter(Mid(inputvalue, I, 1)) Then Return False
            If Char.IsDigit(Mid(formatstr, I, 1)) <> Char.IsDigit(Mid(inputvalue, I, 1)) Then Return False
        Next
        Return True
    End Function

    Public Function GetActualEntryDate() As Date
        strSql = " SELECT MAX(TRANDATE) FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT MAX(TRANDATE)TRANDATE FROM " & cnStockDb & "..ISSUE WHERE ISNULL(CANCEL,'') = '' AND LEN(TRANTYPE) = 2"
        strSql += vbCrLf + " UNION SELECT MAX(TRANDATE)TRANDATE FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'') = ''  AND LEN(TRANTYPE) = 2"
        strSql += vbCrLf + " UNION SELECT MAX(TRANDATE)TRANDATE FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(CANCEL,'') = '' AND FROMFLAG NOT IN ('A','S','C','')"
        strSql += vbCrLf + " UNION SELECT MAX(TRANDATE)TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE ISNULL(CANCEL,'') = '' AND FROMFLAG NOT IN ('A','S')"
        strSql += vbCrLf + " )X"
        Dim EntryDAte As Date = GetEntryDate(GetServerDate)
        Dim Dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If Dt.Rows(0).Item(0).ToString <> "" Then
                EntryDAte = CType(Dt.Rows(0).Item(0), Date)
            End If
        End If
        Return EntryDAte
    End Function

    Public Function GetEntryDate(ByRef defDate As Date, Optional ByVal ttran As OleDbTransaction = Nothing, Optional ByVal Serverdate As Boolean = False) As Date
        If Serverdate = True Then
            defDate = GetServerDate(ttran)
        ElseIf GetAdmindbSoftValue("GLOBALDATE", , ttran).ToUpper = "Y" Then
            Try
                defDate = GetAdmindbSoftValue("GLOBALDATEVAL", GetServerDate(ttran), ttran)
            Catch ex As Exception
                defDate = GetServerDate(ttran)
            End Try
        End If
        Return defDate
    End Function
    'Public Sub SetEntryDate(ByRef dtp As DateTimePicker)
    '    If GetAdmindbSoftValue("GLOBALDATE").ToUpper = "Y" Then
    '        dtp.Value = GetAdmindbSoftValue("GLOBALDATEVAL", GetServerDate())
    '    End If
    'End Sub

    Public Function CheckEntryDate(ByVal actualDate As Date) As Boolean
        If CheckTrialDate(GetEntryDate(actualDate)) = False Then
            Return True
        End If
        If actualDate <> GetEntryDate(actualDate) Then
            If MessageBox.Show("Entry date changed" & vbCrLf & "This entry should affect on " & GetEntryDate(actualDate).ToString("dd/MM/yyyy") + vbCrLf + "Do you wish to Continue?", "Date Mismatch", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Cancel Then
                Return True
            End If
        End If
    End Function

    Public Function GetForm(ByVal frmName As String, Optional ByVal assemblyType As String = "exe", Optional ByVal dllname As String = Nothing) As Form
        Dim sp() As String = System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(",")
        Dim appName As String = sp(0)

        Dim obj_handle As System.Runtime.Remoting.ObjectHandle
        If dllname Is Nothing Then
            obj_handle = Activator.CreateInstanceFrom(appName & "." & assemblyType, appName & "." & frmName.Replace(".vb", "").Trim)
        Else
            obj_handle = Activator.CreateInstanceFrom(dllname & "." & assemblyType, dllname & "." & frmName.Replace(".vb", "").Trim)
        End If
        Dim frm As New Form
        frm = CType(CType(obj_handle.Unwrap, Object), System.Windows.Forms.Form)
        Return frm

    End Function

    Private Function getTableName(ByVal qry As String) As String
        Dim tableName As String = Nothing
        Dim ch(2) As Char
        ch(0) = " "
        ch(1) = "("
        Dim str() As String = qry.ToUpper.Split(ch)
        For Each st As String In str
            If st.Contains(".DBO.") Then
                tableName = Mid(st, InStr(st, ".DBO.") + 5)
                Exit For
            ElseIf st.Contains("..") Then
                tableName = Mid(st, InStr(st, "..") + 2)
                Exit For
            End If
        Next
        Return tableName
    End Function
    Function DbChecker(ByVal dbname As String, Optional ByVal withTran As Boolean = False, Optional ByRef tran As OleDbTransaction = Nothing) As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " select name from master..sysdatabases where name = '" & dbname & "'"
        If withTran Then
            cmd = New OleDbCommand(strSql, cn, tran)
        Else
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        End If
        da = New OleDbDataAdapter(cmd)
        da.Fill(ds, "sysDatabases")
        If ds.Tables("sysDatabases").Rows.Count > 0 Then
            Return 1
        End If
        Return 0
    End Function
    Function DbCheckernew(ByVal dbname As String, ByVal CCn As OleDbConnection, ByVal tran As OleDbTransaction) As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " select name from master..sysdatabases where name = '" & dbname & "'"
        cmd = New OleDbCommand(strSql, CCn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(ds, "sysDatabases")
        If ds.Tables("sysDatabases").Rows.Count > 0 Then
            Return 1
        End If
        Return 0
    End Function


    Public Function Exec(ByVal qry As String _
  , ByVal cn As OleDbConnection _
  , ByVal toId As String _
  , ByVal imagePath As String _
  , Optional ByRef tran As OleDbTransaction = Nothing _
  , Optional ByVal imagePathCtrlId As String = Nothing _
  , Optional ByVal _SyncDb As String = Nothing _
  , Optional ByVal ImgByte() As Byte = Nothing) As Boolean
        Dim syncdb As String = cnAdminDb
        Dim uprefix As String = Left(cnAdminDb, 3)
        If GetAdmindbSoftValueNew("SYNC-SEPDATADB", cn, "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbCheckernew(uprefix + usuffix, cn, tran) <> 0 Then syncdb = uprefix + usuffix
        End If
        If _SyncDb <> Nothing Then syncdb = _SyncDb
        strSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ('" & cnCostId & "','" & toId & "','" & qry & "'"
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        If imagePath <> Nothing Then
            If IO.File.Exists(imagePath) Then
                strSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID"
                strSql += ",TAGIMAGE,TAGIMAGENAME,IMAGE_CTRLID"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ('" & cnCostId & "','" & toId & "',?,?,'" & imagePathCtrlId & "')"
                cmd = New OleDbCommand(strSql, cn, tran)
                Dim fileStr As New IO.FileStream(imagePath, IO.FileMode.Open, IO.FileAccess.Read)
                Dim reader As New IO.BinaryReader(fileStr)
                Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                fileStr.Read(result, 0, result.Length)
                fileStr.Close()
                cmd.Parameters.AddWithValue("@TAGIMAGE", result)
                Dim fInfo As New IO.FileInfo(imagePath)
                cmd.Parameters.AddWithValue("@TAGIMAGENAME", fInfo.Name)
                cmd.ExecuteNonQuery()
            End If
        End If
        If Not ImgByte Is Nothing Then
            If ImgByte.Length > 0 Then
                strSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID"
                strSql += ",TAGIMAGE,TAGIMAGENAME,IMAGE_CTRLID"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ('" & cnCostId & "','" & toId & "',?,?,'" & imagePathCtrlId & "')"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.Parameters.AddWithValue("@TAGIMAGE", ImgByte)
                cmd.Parameters.AddWithValue("@TAGIMAGENAME", "")
                cmd.ExecuteNonQuery()
            End If
        End If
    End Function

    Public Function Dbproperties(Optional ByVal dblist As String = Nothing) As DataTable
        If dblist <> "" Then dblist = Replace(dblist, ",", "','")
        'Dim sqlstr As String = "SELECT Distinct DB.name As DbName,mf.name as logical_name,"
        'sqlstr += " mf.physical_name Filelocation,(size*8)/1024 SizeMB,"
        'sqlstr += " cntr_value as [LogFull%]"
        'sqlstr += " FROM sys.master_files mf"
        'sqlstr += " join sys.databases DB"
        'sqlstr += " on mf.database_id = DB.database_id"
        'sqlstr += " left join sys.dm_os_performance_counters opc "
        'sqlstr += " on opc.instance_name = db.name and mf.name like '%log'"
        'sqlstr += " WHERE mf.database_id >4  "
        'sqlstr += " and counter_name LIKE 'Percent Log Used%'"
        'sqlstr += " AND instance_name not in ('_Total', 'mssqlsystemresource')"
        'If dblist <> Nothing Then sqlstr += " and db.name in ('" & dblist & "')"
        'sqlstr += " union all"
        'sqlstr += " SELECT Distinct DB.name As DbName,"
        'sqlstr += " mf.name as logical_name,"
        'sqlstr += " mf.physical_name Filelocation,"
        'sqlstr += " (size*8)/1024 SizeMB,"
        'sqlstr += " 0 as [LogFull%]"
        'sqlstr += " FROM sys.master_files mf"
        'sqlstr += " join sys.databases DB"
        'sqlstr += " on mf.database_id = DB.database_id"
        'sqlstr += " WHERE mf.database_id >4 and mf.name not like '%log'"
        'If dblist <> Nothing Then sqlstr += " and db.name in ('" & dblist & "')"
        'sqlstr += " order by dbname"
        Dim sqlstr As String = "SELECT Distinct DB.name As DbName,mf.name as logical_name,"
        sqlstr += " mf.filename Filelocation,(size*8)/1024 SizeMB,"
        sqlstr += " cntr_value as [LogFull%]"
        sqlstr += " FROM sysaltfiles mf"
        sqlstr += " join sysdatabases DB"
        sqlstr += " on mf.dbid = DB.dbid"
        sqlstr += " left join sysperfinfo opc "
        sqlstr += " on opc.instance_name = db.name and mf.name like '%log'"
        sqlstr += " WHERE mf.dbid >4  "
        sqlstr += " and counter_name LIKE 'Percent Log Used%'"
        sqlstr += " AND instance_name not in ('_Total', 'mssqlsystemresource')"
        If dblist <> Nothing Then sqlstr += " and db.name in ('" & dblist & "')"
        sqlstr += " union all"
        sqlstr += " SELECT Distinct DB.name As DbName,"
        sqlstr += " mf.name as logical_name,"
        sqlstr += " mf.filename Filelocation,"
        sqlstr += " (size*8)/1024 SizeMB,"
        sqlstr += " 0 as [LogFull%]"
        sqlstr += " FROM sysaltfiles mf"
        sqlstr += " join sysdatabases DB"
        sqlstr += " on mf.dbid = DB.dbid"
        sqlstr += " WHERE mf.dbid >4 and mf.name not like '%log'"
        If dblist <> Nothing Then sqlstr += " and db.name in ('" & dblist & "')"
        sqlstr += " order by dbname"

        Dim dbdt As New DataTable
        da = New OleDbDataAdapter(sqlstr, cn)
        da.Fill(dbdt)
        Return dbdt
    End Function

    Public Sub DbTabledrop(ByVal dbname As String, Optional ByVal tblmatch As String = Nothing)
        Dim sqlstr As String = " select name from " & dbname & "..sysobjects where xtype = 'u'"
        If tblmatch <> Nothing Then sqlstr += " and name like '" & tblmatch & "%'"
        Dim dropdt As New DataTable
        da = New OleDbDataAdapter(sqlstr, cn)
        da.Fill(dropdt)
        If dropdt.Rows.Count > 0 Then
            For ii As Integer = 0 To dropdt.Rows.Count - 1
                sqlstr = "drop table " & dbname & ".." & dropdt.Rows(ii).Item(0)
                Try
                    cmd = New OleDbCommand(sqlstr, cn)
                    If tran IsNot Nothing Then cmd.Transaction = tran
                    cmd.ExecuteNonQuery()
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub
    Public Function Exec_Safi(ByVal qry As String _
    , ByVal cn As OleDbConnection _
    , ByVal toId As String _
    , ByVal imagePath As String _
    , Optional ByRef tran As OleDbTransaction = Nothing) As Boolean
        strSql = " INSERT INTO " & cnAdminDb & "..SENDSYNC(FROMID,TOID,SQLTEXT"
        If imagePath <> Nothing Then
            If IO.File.Exists(imagePath) Then
                strSql += ",TAGIMAGE,TAGIMAGENAME"
            Else
                imagePath = Nothing
            End If
        End If
        strSql += " )"
        strSql += " VALUES"
        strSql += " (?,?,?"
        If imagePath <> Nothing Then strSql += ",?,?"
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.Parameters.AddWithValue("@FROMID", cnCostId)
        cmd.Parameters.AddWithValue("@TOID", toId)
        cmd.Parameters.AddWithValue("@SQLTEXT", qry)
        If imagePath <> Nothing Then
            Dim fileStr As New IO.FileStream(imagePath, IO.FileMode.Open, IO.FileAccess.Read)
            Dim reader As New IO.BinaryReader(fileStr)
            Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
            fileStr.Read(result, 0, result.Length)
            fileStr.Close()
            cmd.Parameters.AddWithValue("@TAGIMAGE", result)

            Dim fInfo As New IO.FileInfo(imagePath)
            cmd.Parameters.AddWithValue("@TAGIMAGENAME", fInfo.Name)
        End If
        cmd.ExecuteNonQuery()
    End Function

    Public Function SetCenterLocation(ByRef f As Control) As Point
        f.Location = New Point((ScreenWid - f.Width) / 2, ((ScreenHit - 128) - f.Height) / 2)
    End Function
    Public Function ExecQuery(ByVal mode As SyncMode _
           , ByVal qry As String _
           , ByVal cn As OleDbConnection _
           , Optional ByRef tran As OleDbTransaction = Nothing _
           , Optional ByVal toId As String = Nothing _
           , Optional ByVal stateId As String = Nothing _
           , Optional ByVal imagePath As String = Nothing _
           , Optional ByVal replaceTableName As String = Nothing _
           , Optional ByVal LocalExecution As Boolean = True _
           , Optional ByVal Sync As Boolean = True _
           , Optional ByVal ImagePathCtrlId As String = Nothing _
           , Optional ByVal Mastercheck As Boolean = True _
           , Optional ByVal toAdvId As String = Nothing _
           , Optional ByVal SyncDb As String = Nothing _
           , Optional ByVal ImgByte() As Byte = Nothing) As Boolean
        If Not cnCostId.Length > 0 And _HasCostcentre Then
            If tran IsNot Nothing Then tran.Rollback()
            tran = Nothing
            MsgBox("Default CostId doesnt set. Please Set COSTID in Softcontrol and You must Restart your current application", MsgBoxStyle.Information)
            Application.Restart()
        End If
        Dim  tableName As String = UCase(getTableName(qry))
        If Mastercheck Then
            If mode = SyncMode.Master And _SyncTo <> "" Then
                If BrighttechPack.GlobalMethods.GetSqlValue(cn, "SELECT TABLENAME FROM " & cnAdminDb & "..SYNCMAST WHERE TABLENAME = '" & tableName & "' AND SYNC = 'Y'", , , tran).Length > 0 Then
                    If Not tran Is Nothing Then tran.Rollback()
                    tran = Nothing
                    MsgBox("Master entry cannot allow at location", MsgBoxStyle.Information)
                    Return False
                End If
            End If
        End If
        Dim OriginalQry As String = qry
        Dim cmd As OleDbCommand
        If LocalExecution Then
            cmd = New OleDbCommand(qry, cn)
            If Not tran Is Nothing Then cmd.Transaction = tran
            cmd.ExecuteNonQuery()
        End If
        If strBCostid <> Nothing And BrighttechPack.GlobalMethods.GetSqlValue(cn, "SELECT ISNULL(CTLTEXT,'') FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CENTR_DB_ALLCOSTID' AND CTLTEXT = 'Y'", , , tran).Length > 0 Then Sync = False
        If Sync = False Then Return True
        If Not BrighttechPack.GlobalMethods.GetSqlValue(cn, "SELECT ISNULL(CTLTEXT,'') FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & strCompanyId & "_SYNC' AND CTLTEXT = 'Y'", , , tran).Length > 0 Then Return True
        Dim strSql As String = Nothing
        If BrighttechPack.GlobalMethods.GetSqlValue(cn, "SELECT TABLENAME FROM " & cnAdminDb & "..SYNCMAST WHERE TABLENAME = '" & tableName & "' AND SYNC = 'N'", , , tran).Length > 0 Then
            Return True
        End If
        If replaceTableName <> Nothing Then qry = qry.Replace(tableName, replaceTableName)
        If mode = SyncMode.Master Then
            Dim dtCostId As New DataTable
            strSql = "SELECT COSTID,STATEID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID <> '" & cnCostId & "' AND ISNULL(ACTIVE,'Y')<>'N'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            If Not tran Is Nothing Then cmd.Transaction = tran
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtCostId)
            For Each ro As DataRow In dtCostId.Rows
                If stateId <> Nothing And ro!STATEID.ToString <> stateId Then Continue For
                Exec(qry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId, SyncDb, ImgByte)
            Next
        ElseIf mode = SyncMode.Stock Then
            If GetAdmindbSoftValue("SYNC-STOCK", "N", tran) = "Y" Then
                Dim dtCostId As New DataTable
                strSql = "SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE "
                strSql += " WHERE COSTID <> '" & cnCostId & "'  AND ISNULL(ACTIVE,'Y')<>'N'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                If Not tran Is Nothing Then cmd.Transaction = tran
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtCostId)
                For Each ro As DataRow In dtCostId.Rows
                    If ro!COSTID.ToString = cnCostId Then Continue For
                    If ro!COSTID.ToString = toId Then
                        Exec(qry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId, SyncDb, ImgByte)
                    Else
                        Exec(OriginalQry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId, SyncDb, ImgByte)
                    End If
                Next
            Else
                If toId <> Nothing Then
                    If toId = cnCostId Then Return True
                    Exec(qry.Replace("'", "''"), cn, toId, imagePath, tran, ImagePathCtrlId, SyncDb, ImgByte)
                End If
            End If
        ElseIf mode = SyncMode.Transaction Then
            If toId <> Nothing And toAdvId Is Nothing Then
                Dim syncTo As String = _SyncTo
                If syncTo = "" Then syncTo = toId
                If syncTo = cnCostId Then Return True
                Exec(qry.Replace("'", "''"), cn, syncTo, imagePath, tran, ImagePathCtrlId, SyncDb, ImgByte)
            Else
                If toAdvId <> Nothing Then
                    Exec(qry.Replace("'", "''"), cn, toAdvId, imagePath, tran, ImagePathCtrlId, SyncDb, ImgByte)
                End If
            End If
        End If
        Return True
    End Function
    Public Function ExecQuery_DbCreator(ByVal mode As SyncMode _
          , ByVal qry As String _
          , ByVal cn As OleDbConnection _
          , Optional ByRef tran As OleDbTransaction = Nothing _
          , Optional ByVal toId As String = Nothing _
          , Optional ByVal stateId As String = Nothing _
          , Optional ByVal imagePath As String = Nothing _
          , Optional ByVal replaceTableName As String = Nothing _
          , Optional ByVal LocalExecution As Boolean = True _
          , Optional ByVal Sync As Boolean = True _
          , Optional ByVal ImagePathCtrlId As String = Nothing _
          , Optional ByVal Mastercheck As Boolean = True _
              ) As Boolean
        If Not cnCostId.Length > 0 And _HasCostcentre Then
            If tran IsNot Nothing Then tran.Rollback()
            tran = Nothing
            MsgBox("Default CostId doesnt set. Please Set COSTID in Softcontrol and You must Restart your current application", MsgBoxStyle.Information)
            Application.Restart()
        End If

        Dim OriginalQry As String = qry
        Dim cmd As OleDbCommand
        If LocalExecution Then
            cmd = New OleDbCommand(qry, cn)
            If Not tran Is Nothing Then cmd.Transaction = tran
            cmd.ExecuteNonQuery()
        End If
        If Sync = False Then Return True

        If Not BrighttechPack.GlobalMethods.GetSqlValue(cn, "SELECT ISNULL(CTLTEXT,'') FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & strCompanyId & "_SYNC' AND CTLTEXT = 'Y'", , , tran).Length > 0 Then Return True
        Dim strSql As String = Nothing

        If mode = SyncMode.Master Then
            Dim dtCostId As New DataTable
            strSql = "SELECT COSTID,STATEID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID <> '" & cnCostId & "'  AND ISNULL(ACTIVE,'Y')<>'N'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            If Not tran Is Nothing Then cmd.Transaction = tran
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtCostId)
            For Each ro As DataRow In dtCostId.Rows
                If stateId <> Nothing And ro!STATEID.ToString <> stateId Then Continue For
                Exec(qry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId)
            Next
        ElseIf mode = SyncMode.Stock Then
            If GetAdmindbSoftValue("SYNC-STOCK", "N", tran) = "Y" Then
                Dim dtCostId As New DataTable
                strSql = "SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE "
                strSql += " WHERE COSTID <> '" & cnCostId & "'  AND ISNULL(ACTIVE,'Y')<>'N' "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                If Not tran Is Nothing Then cmd.Transaction = tran
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtCostId)
                For Each ro As DataRow In dtCostId.Rows
                    If ro!COSTID.ToString = cnCostId Then Continue For
                    If ro!COSTID.ToString = toId Then
                        Exec(qry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId)
                    Else
                        Exec(OriginalQry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId)
                    End If
                Next
            Else
                If toId <> Nothing Then
                    If toId = cnCostId Then Return True
                    Exec(qry.Replace("'", "''"), cn, toId, imagePath, tran, ImagePathCtrlId)
                End If
            End If
        ElseIf mode = SyncMode.Transaction Then
            If toId <> Nothing Then
                Dim syncTo As String = _SyncTo
                If syncTo = "" Then syncTo = toId
                If syncTo = cnCostId Then Return True
                Exec(qry.Replace("'", "''"), cn, syncTo, imagePath, tran, ImagePathCtrlId)
            End If
        End If
        Return True
    End Function
    Public Function ExecQuery_Safi(ByVal mode As SyncMode _
       , ByVal qry As String _
       , ByVal cn As OleDbConnection _
       , Optional ByRef tran As OleDbTransaction = Nothing _
       , Optional ByVal toId As String = Nothing _
       , Optional ByVal stateId As String = Nothing _
       , Optional ByVal imagePath As String = Nothing _
       , Optional ByVal replaceTableName As String = Nothing _
       , Optional ByVal LocalExecution As Boolean = True _
       , Optional ByVal ImagePathCtrlId As String = Nothing _
       ) As Boolean
        If Not cnCostId.Length > 0 And objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'", , "N", tran).ToUpper = "Y" Then
            If tran IsNot Nothing Then tran.Rollback()
            tran = Nothing
            MsgBox("Default CostId doesnt set. Please Set COSTID in Softcontrol and You must Restart your current application", MsgBoxStyle.Information)
            Application.Restart()
        End If
        Dim tableName As String = UCase(getTableName(qry))
        If mode = SyncMode.Master And GetAdmindbSoftValue("SYNC-TO", , tran) <> "" Then
            If objGPack.GetSqlValue("SELECT TABLENAME FROM " & cnAdminDb & "..SYNCMAST WHERE TABLENAME = '" & tableName & "' AND SYNC = 'Y'", , , tran).Length > 0 Then
                If Not tran Is Nothing Then tran.Rollback()
                tran = Nothing
                MsgBox("Master entry cannot allow at location", MsgBoxStyle.Information)
                Return False
            End If
        End If
        Dim OriginalQry As String = qry
        Dim cmd As OleDbCommand
        If LocalExecution Then
            cmd = New OleDbCommand(qry, cn)
            If Not tran Is Nothing Then cmd.Transaction = tran
            cmd.ExecuteNonQuery()
        End If
        Dim strSql As String = Nothing
        If objGPack.GetSqlValue("SELECT TABLENAME FROM " & cnAdminDb & "..SYNCMAST WHERE TABLENAME = '" & tableName & "' AND SYNC = 'N'", , , tran).Length > 0 Then
            Return True
        End If
        If replaceTableName <> Nothing Then qry = qry.Replace(tableName, replaceTableName)

        If mode = SyncMode.Master Then
            Dim dtCostId As New DataTable
            strSql = "SELECT COSTID,STATEID FROM " & cnAdminDb & "..SYNCCOSTCENTRE where COSTID <> '" & cnCostId & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            If Not tran Is Nothing Then cmd.Transaction = tran
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtCostId)
            For Each ro As DataRow In dtCostId.Rows
                If stateId <> Nothing And ro!STATEID.ToString <> stateId Then Continue For
                Exec(qry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId)

            Next
        ElseIf mode = SyncMode.Stock Then
            If GetAdmindbSoftValue("SYNC-STOCK", "N", tran) = "Y" Then
                Dim dtCostId As New DataTable
                strSql = "SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE "
                strSql += " WHERE COSTID <> '" & cnCostId & "'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                If Not tran Is Nothing Then cmd.Transaction = tran
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtCostId)
                For Each ro As DataRow In dtCostId.Rows
                    If ro!COSTID.ToString = cnCostId Then Continue For
                    If ro!COSTID.ToString = toId Then
                        Exec(qry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId)
                    Else
                        Exec(OriginalQry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId)
                    End If
                Next
            Else
                If toId <> Nothing Then
                    If toId = cnCostId Then Return True
                    Exec(qry.Replace("'", "''"), cn, toId, imagePath, tran, ImagePathCtrlId)
                End If
            End If
        ElseIf mode = SyncMode.Transaction Then
            If toId <> Nothing Then
                Dim syncTo As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SYNC-TO' AND CTLTEXT <> ''", , toId, tran)
                If syncTo = cnCostId Then Return True
                Exec(qry.Replace("'", "''"), cn, syncTo, imagePath, tran, ImagePathCtrlId)
            End If
        End If
        Return True
    End Function

    Public Sub FillCheckedListBox(ByVal qry As String, ByVal chkLst As CheckedListBox, Optional ByVal clear As Boolean = True, Optional ByVal Check As Boolean = False)
        chkLst.Items.Clear()
        Dim dt As New DataTable
        da = New OleDbDataAdapter(qry, cn)
        da.Fill(dt)
        For Each ro As DataRow In dt.Rows
            chkLst.Items.Add(ro(0).ToString)
            chkLst.SetItemChecked(chkLst.Items.Count - 1, Check)
        Next
    End Sub


    Public Function GetChecked_CheckedList(ByVal chkLst As CheckedListBox, Optional ByVal withSingleQt As Boolean = True) As String
        Dim retStr As String = ""
        For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
            If withSingleQt Then retStr += "'"
            retStr += chkLst.CheckedItems.Item(cnt).ToString
            If withSingleQt Then retStr += "'"
            retStr += ","
        Next
        If retStr <> "" Then retStr = Mid(retStr, 1, retStr.Length - 1)
        Return retStr
    End Function
    Public Function GetChecked_CheckedList(ByVal chkLst As BrighttechPack.CheckedComboBox, Optional ByVal withSingleQt As Boolean = True) As String
        Dim retStr As String = ""
        For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
            If withSingleQt Then retStr += "'"
            retStr += chkLst.CheckedItems.Item(cnt).ToString
            If withSingleQt Then retStr += "'"
            retStr += ","
        Next
        If retStr <> "" Then retStr = Mid(retStr, 1, retStr.Length - 1)
        Return retStr
    End Function

    Public Sub FillGridGroupStyle(ByVal gridview As DataGridView, Optional ByVal ParticularColName As String = "")
        Dim ind As Integer = 0
        If ParticularColName <> "" Then
            If gridview.Columns.Contains(ParticularColName) Then
                ind = gridview.Columns(ParticularColName).Index
            End If
        End If
        For Each dgvRow As DataGridViewRow In gridview.Rows
            If dgvRow.Cells("COLHEAD").Value.ToString = "T" Then
                dgvRow.Cells(ind).Style.BackColor = Color.LightBlue
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "T1" Then
                dgvRow.Cells(ind).Style.BackColor = Color.Beige
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "T2" Then
                dgvRow.Cells(ind).Style.BackColor = Color.LightGreen
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "T3" Then
                dgvRow.Cells(ind).Style.BackColor = Color.MistyRose
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S1" Then
                dgvRow.DefaultCellStyle.BackColor = Color.Beige
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S2" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightGreen
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S3" Then
                dgvRow.DefaultCellStyle.BackColor = Color.MistyRose
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "G" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
    End Sub


    Public Sub FillGridGroupStyle_KeyNoWise(ByVal gridView As DataGridView, Optional ByVal FirstColumnName As String = Nothing)
        If gridView.Columns.Contains("KEYNO") = False Then Exit Sub
        If gridView.Columns.Contains("COLHEAD") = False Then Exit Sub
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable).Copy
        ''title
        Dim rowTitle() As DataRow = Nothing
        rowTitle = dt.Select("COLHEAD = 'T'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("PARTICULAR").Style = reportHeadStyle
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle
            End If
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Next
        rowTitle = dt.Select("COLHEAD = 'T1'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("PARTICULAR").Style = reportHeadStyle1
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle1
            End If
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Next
        rowTitle = dt.Select("COLHEAD = 'T2'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("PARTICULAR").Style = reportHeadStyle2
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle2
            End If
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Next
        rowTitle = dt.Select("COLHEAD = 'N'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("PARTICULAR").Style = reportHeadStyle2
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle2
            End If
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.ForeColor = Color.OrangeRed
        Next
        rowTitle = dt.Select("COLHEAD = 'S'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle
        Next
        rowTitle = dt.Select("COLHEAD = 'S1'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle1
        Next
        rowTitle = dt.Select("COLHEAD = 'S2'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle2
        Next
        rowTitle = dt.Select("COLHEAD = 'G'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportTotalStyle
        Next
        'ROWTITLE = DT.Select("COLHEAD = 'S'"
    End Sub
    Public Function CalcRoundoffAmt(ByVal amt As Double, ByVal type As String) As Double
        Select Case type
            Case "L"
                Return Math.Floor(amt)
            Case "F"
                If Math.Abs(amt - Math.Floor(amt)) >= 0.5 Then
                    Return Math.Ceiling(amt)
                Else
                    Return Math.Floor(amt)
                End If
            Case "H"
                Return Math.Ceiling(amt)
            Case Else
                Return amt
        End Select
        Return amt
    End Function
    Public Function InsertData( _
    ByVal Mode As SyncMode _
    , ByVal Dt As DataTable _
    , ByVal Con As OleDbConnection _
    , Optional ByVal Tran As OleDbTransaction = Nothing _
    , Optional ByVal toId As String = Nothing _
    , Optional ByVal stateId As String = Nothing _
    , Optional ByVal imagePath As String = Nothing _
    , Optional ByVal replaceTableName As String = Nothing _
    , Optional ByVal LocalExecution As Boolean = True _
    , Optional ByVal Sync As Boolean = True _
    ) As Boolean
        Try
            Dim Qry As String = ""
            Dim _Column As String = ""
            Dim _Values As String = ""
            For Each Row As DataRow In Dt.Rows
                Qry = "INSERT INTO " & Row.Table.TableName & " ( "
                _Column = ""
                _Values = ""
                For Each dCol As DataColumn In Row.Table.Columns
                    _Column = _Column & "," + dCol.ColumnName
                    If dCol.DataType.Name = "String" Then
                        _Values = _Values & ",'" & Row.Item(dCol.ColumnName) & "'"
                    ElseIf dCol.DataType.Name = "DateTime" Then
                        If IsDBNull(Row.Item(dCol.ColumnName)) Then
                            _Values = _Values & "," + "NULL" + ""
                        ElseIf Row.Item(dCol.ColumnName).ToString = "01/01/0001 12:00:00 AM" Then
                            _Values = _Values & "," + "NULL" + ""
                        ElseIf Row.Item(dCol.ColumnName).ToString = "1/1/0001 12:00:00 AM" Then
                            _Values = _Values & "," + "NULL" + ""
                        ElseIf Row.Item(dCol.ColumnName).ToString = "01/01/0001" Then
                            _Values = _Values & "," + "NULL" + ""
                        ElseIf Row.Item(dCol.ColumnName).ToString = "1/1/0001" Then
                            _Values = _Values & "," + "NULL" + ""

                        Else
                            _Values = _Values & ",'" & Microsoft.VisualBasic.Format(Row.Item(dCol.ColumnName), "MM/dd/yyyy") & "'"
                        End If
                    ElseIf dCol.DataType.Name = GetType(Double).Name Or dCol.DataType.Name = GetType(Decimal).Name _
                    Or dCol.DataType.Name = GetType(Int16).Name Or dCol.DataType.Name = GetType(Int32).Name Or dCol.DataType.Name = GetType(Int64).Name Or dCol.DataType.Name = GetType(Integer).Name Then
                        _Values = _Values & "," & Val(Row.Item(dCol.ColumnName).ToString)
                    ElseIf dCol.DataType.Name = "Boolean" Then
                        _Values = _Values & "," + "NULL" + ""
                    Else
                        _Values = _Values & "," & Row.Item(dCol.ColumnName)
                    End If
                Next
                _Column = _Column & ")"
                _Column = _Column.Substring(1)
                _Values = _Values.Substring(1)
                Qry = (Qry + _Column & " VALUES (") + _Values & ")"
                ExecQuery(Mode, Qry, Con, Tran, toId, stateId, imagePath, replaceTableName, LocalExecution, Sync)
            Next
            Return True
        Catch ex As Exception
            If Not Tran Is Nothing Then Tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return False
        End Try
    End Function
    Public Function InsertQry(ByVal Row As DataRow, Optional ByVal DB As String = Nothing) As String
        Dim qry As String = ""
        qry = "INSERT INTO " & IIf(Not DB Is Nothing, DB & "..", "") & Row.Table.TableName & " ( "
        Dim _Column As String = ""
        Dim _Values As String = ""
        For Each dCol As DataColumn In Row.Table.Columns
            _Column = _Column & "," + dCol.ColumnName
            If dCol.DataType.Name = "String" Then
                _Values = _Values & ",'" & Row.Item(dCol.ColumnName) & "'"
            ElseIf dCol.DataType.Name = "DateTime" Then
                If IsDBNull(Row.Item(dCol.ColumnName)) Then
                    _Values = _Values & "," + "NULL" + ""
                Else
                    _Values = _Values & ",'" & Microsoft.VisualBasic.Format(Row.Item(dCol.ColumnName), "MM/dd/yyyy") & "'"
                End If
            ElseIf dCol.DataType.Name = GetType(Double).Name Or dCol.DataType.Name = GetType(Decimal).Name _
            Or dCol.DataType.Name = GetType(Int16).Name Or dCol.DataType.Name = GetType(Int32).Name Or dCol.DataType.Name = GetType(Int64).Name Or dCol.DataType.Name = GetType(Integer).Name Then
                _Values = _Values & "," & Val(Row.Item(dCol.ColumnName).ToString)
            Else
                _Values = _Values & "," & Row.Item(dCol.ColumnName)
            End If
        Next
        _Column = _Column & ")"
        _Column = _Column.Substring(1)
        _Values = _Values.Substring(1)
        qry = (qry + _Column & " VALUES (") + _Values & ")"
        Return qry
    End Function
    Public Function GetStockCompId() As String
        If cnCentStock Then
            Return _MainCompId
        Else
            Return strCompanyId
        End If
    End Function
    Public Function CheckTrialDate(ByVal dat As Date) As Boolean
        If _Demo Then
            If dat > _DemoExpiredDate Then
                MsgBox("Your 30 days trail period expired. Please try evaluate version", MsgBoxStyle.Information)
                Return False
            End If
        End If
        Return True
    End Function

    Public Function CheckBckDays(ByVal Userid As Integer, ByVal Menuname As String, ByVal dtdate As Date) As Boolean
        Dim mserverdate As Date = GetServerDate()
        If dtdate < mserverdate Then
            If Userid = 999 Then Return True
            Dim mdifdays As Integer = Math.Abs(DateDiff(DateInterval.Day, mserverdate, dtdate))
            If Userid = 0 Or Menuname = "" Then Return True
            Dim ro() As DataRow = _DtUserRights.Select("ACCESSID LIKE '" & Menuname & "%'")
            If ro.Length = 0 Then Return True
            Dim menuid As String = ro(0).Item(0).ToString
            strSql = "select isnull(RT.VIEWDAYS,0) from " & cnAdminDb & "..ROLETRAN RT INNER JOIN " & cnAdminDb & "..USERROLE UR ON RT.ROLEID =UR.ROLEID WHERE UR.USERID = " & Userid & " AND MENUID ='" & menuid & "'"
            Dim mdays As Decimal = Val(objGPack.GetSqlValue(strSql))
            If mdays > 0 Then
                If mdifdays >= mdays Then MsgBox("(Days Exceed)Please check given date") : Return False Else Return True
            Else
                Return True
            End If
        Else
            'If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'GLOBALDATE'") = "Y" Then
            '    If dtdate > mserverdate Then MsgBox("Date should not be more than global date", MsgBoxStyle.Information) : Return False : Exit Function
            'End If
            Return True
        End If
    End Function

    Public Function CheckDayEnd(ByVal dat As Date) As Boolean
        If GetAdmindbSoftValue("ENTRYLOCK", "Y") = "Y" Then
            strSql = "SELECT COUNT(*)CNT FROM " & cnStockDb & "..SYSOBJECTS WHERE NAME='DAILYREPORT' AND XTYPE='U'"
            If objGPack.GetSqlValue(strSql, "CNT", 0) > 0 Then
                strSql = "SELECT COUNT(*)CNT FROM " & cnStockDb & "..DAILYREPORT WHERE TRANDATE='" & dat.ToString("yyyy-MM-dd") & "'"
                If objGPack.GetSqlValue(strSql, "CNT", 0) > 0 Then
                    MsgBox("DSR Generated not able to make Entry", MsgBoxStyle.Information)
                    Return False
                End If
            End If
        End If
        Return True
    End Function

    Public Function CheckDate(ByVal dat As Date) As Boolean
        If Not (dat >= cnTranFromDate And dat <= cnTranToDate) Then
            MsgBox("Invalid Entry Date", MsgBoxStyle.Information)
            Return False
        End If
        Return True
    End Function

    Public Function GetColumnNames(ByVal dbName As String, ByVal tblName As String, Optional ByVal ttran As OleDbTransaction = Nothing) As String
        Dim retStr As String = Nothing
        strSql = " SELECT NAME FROM " & dbName & "..SYSCOLUMNS WHERE ID = "
        strSql += " (SELECT ID FROM " & dbName & "..SYSOBJECTS WHERE NAME = '" & tblName & "')"
        Dim dtTEmp As New DataTable
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        If Not ttran Is Nothing Then cmd.Transaction = ttran
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtTEmp)
        For cnt As Integer = 0 To dtTEmp.Rows.Count - 1
            retStr += dtTEmp.Rows(cnt).Item("NAME").ToString
            If cnt <> dtTEmp.Rows.Count - 1 Then
                retStr += ","
            End If
        Next
        Return retStr
    End Function


    Public Sub LoadCompany(ByRef chkLstBox As CheckedListBox)
        chkLstBox.Items.Clear()
        strSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY DISPLAYORDER,COMPANYNAME"
        Dim dtCompany As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        For Each ro As DataRow In dtCompany.Rows
            chkLstBox.Items.Add(ro("COMPANYNAME").ToString)
            If ro("COMPANYNAME").ToString = strCompanyName Then chkLstBox.SetItemChecked(chkLstBox.Items.Count - 1, True)
        Next
    End Sub

    Public Sub CheckedListCompany_Lostfocus(ByVal sender As Object, ByVal e As EventArgs)
        Dim chklst As CheckedListBox = CType(sender, CheckedListBox)
        If chklst.Items.Count > 0 Then
            If Not chklst.CheckedItems.Count > 0 Then
                For cnt As Integer = 0 To chklst.Items.Count - 1
                    If chklst.Items.Item(cnt).ToString = strCompanyName Then
                        chklst.SetItemChecked(cnt, True)
                        Exit Sub
                    End If
                Next
            End If
        End If
    End Sub
    Public Function GetSelectedCompanyId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "" & strCompanyId & ""
        End If
        Return retStr
    End Function
    Public Function GetSelectedCompanyId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "" & strCompanyId & ""
        End If
        Return retStr
    End Function

    Public Function GetSelectedUserId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT USERID FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function

    Public Function GetSelectedCostId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
    Public Function GetSelectedCostId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
    Public Function GetSelecteditemid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
    Public Function GetSelectedSubitemid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean, Optional ByVal chkitems As Boolean = True, Optional ByVal itemid As String = Nothing) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            If chkitems Then
                For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                    If WithQuotes Then retStr += "'"
                    retStr += objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'" & IIf(itemid <> Nothing, " AND ITEMID='" & itemid & "'", "") & "")
                    If WithQuotes Then retStr += "'"
                    If cnt <> chkLst.CheckedItems.Count - 1 Then
                        retStr += ","
                    End If
                Next
            Else

                Dim dt As New DataTable()
                strSql = "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE  ITEMID='" & itemid & "' AND ACTIVE='Y'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    For i As Integer = 0 To dt.Rows.Count - 1
                        If WithQuotes Then retStr += "'"
                        retStr += dt.Rows(i).Item("SUBITEMID").ToString
                        If WithQuotes Then retStr += "'"
                        If i <> dt.Rows.Count - 1 Then
                            retStr += ","
                        End If
                    Next
                End If
            End If
        Else
            retStr = "''"
        End If
        Return retStr
    End Function


    Public Sub LoadCostName(ByVal chkLstCostCentre As BrighttechPack.CheckedComboBox, Optional ByVal withAll As Boolean = True)
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' "
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = "SELECT DISTINCT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                chkLstCostCentre.Enabled = True
                chkLstCostCentre.Items.Clear()
                If withAll Then
                    chkLstCostCentre.Items.Add("ALL", True)
                End If
                For cnt As Integer = 0 To dt.Rows.Count - 1
                    chkLstCostCentre.Items.Add(dt.Rows(cnt).Item(0).ToString)
                Next
            End If
        Else
            chkLstCostCentre.Items.Clear()
            chkLstCostCentre.Enabled = False
        End If
    End Sub


    Public Sub LoadCostName(ByVal chkLstCostCentre As CheckedListBox, Optional ByVal withAll As Boolean = True)
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' "
        If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = "SELECT DISTINCT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                chkLstCostCentre.Enabled = True
                chkLstCostCentre.Items.Clear()
                If withAll Then
                    chkLstCostCentre.Items.Add("ALL", True)
                End If
                For cnt As Integer = 0 To dt.Rows.Count - 1
                    chkLstCostCentre.Items.Add(dt.Rows(cnt).Item(0).ToString)
                Next
            End If
        Else
            chkLstCostCentre.Items.Clear()
            chkLstCostCentre.Enabled = False
        End If
    End Sub
    Public Function FormatNumberStyle(ByVal noOfDecimal As Integer) As String
        Dim retStr As String = "0"
        If noOfDecimal > 0 Then retStr += "."
        For cnt As Integer = 1 To noOfDecimal
            retStr += "0"
        Next
        Return retStr
    End Function
    Public Function FormatStringCustom(ByVal orgStr As String, ByVal spaceChar As Char, ByVal leng As Integer) As String
        Dim retStr As String = ""
        For cnt As Integer = 1 To leng - orgStr.Length
            retStr += spaceChar
        Next
        Return retStr & orgStr
    End Function

    Public Sub AutoImageSizer(ByVal bmp As Bitmap, ByVal picBox As PictureBox, Optional ByVal pSizeMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage)
        Try
            picBox.Image = bmp.Clone
            AutoImageSizer(picBox, pSizeMode)
        Catch ex As Exception
            picBox.Image = My.Resources.no_photo
        End Try
    End Sub

    Public Sub AutoImageSizer(ByVal ImagePath As String, ByVal picBox As PictureBox, Optional ByVal pSizeMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage)
        Try
            picBox.Image = Nothing
            picBox.SizeMode = pSizeMode
            If System.IO.File.Exists(ImagePath) Then
                Dim fStream As IO.FileStream
                fStream = New IO.FileStream(ImagePath, IO.FileMode.Open, IO.FileAccess.Read)
                picBox.Image = Image.FromStream(fStream)
                fStream.Close()
            Else
                picBox.Image = My.Resources.no_photo
            End If
            AutoImageSizer(picBox, pSizeMode)
        Catch ex As Exception
            picBox.Image = My.Resources.no_photo
        End Try
    End Sub

    Public Sub AutoImageSizer(ByVal picBox As PictureBox, Optional ByVal pSizeMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage)
        Try
            picBox.SizeMode = pSizeMode
            Dim imgOrg As Bitmap
            Dim imgShow As Bitmap
            Dim g As Graphics
            Dim divideBy, divideByH, divideByW As Double
            imgOrg = DirectCast(picBox.Image, Bitmap)

            divideByW = imgOrg.Width / picBox.Width
            divideByH = imgOrg.Height / picBox.Height
            If divideByW > 1 Or divideByH > 1 Then
                If divideByW > divideByH Then
                    divideBy = divideByW
                Else
                    divideBy = divideByH
                End If

                imgShow = New Bitmap(CInt(CDbl(imgOrg.Width) / divideBy), CInt(CDbl(imgOrg.Height) / divideBy))
                imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
                g = Graphics.FromImage(imgShow)
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.DrawImage(imgOrg, New Rectangle(0, 0, CInt(CDbl(imgOrg.Width) / divideBy), CInt(CDbl(imgOrg.Height) / divideBy)), 0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel)
                g.Dispose()
            Else
                imgShow = New Bitmap(imgOrg.Width, imgOrg.Height)
                imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
                g = Graphics.FromImage(imgShow)
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.DrawImage(imgOrg, New Rectangle(0, 0, imgOrg.Width, imgOrg.Height), 0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel)
                g.Dispose()
            End If
            imgOrg.Dispose()
            picBox.Image = imgShow
        Catch ex As Exception
            picBox.Image = My.Resources.no_photo
            'MsgBox(ex.ToString)
        End Try
    End Sub

    'Public Sub AutosizeImage(ByVal ImagePath As String, ByVal picBox As PictureBox, Optional ByVal pSizeMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage)
    '    Try
    '        picBox.Image = Nothing
    '        picBox.SizeMode = pSizeMode
    '        If System.IO.File.Exists(ImagePath) Then
    '            Dim imgOrg As Bitmap
    '            Dim imgShow As Bitmap
    '            Dim g As Graphics
    '            Dim divideBy, divideByH, divideByW As Double
    '            imgOrg = DirectCast(Bitmap.FromFile(ImagePath), Bitmap)

    '            divideByW = imgOrg.Width / picBox.Width
    '            divideByH = imgOrg.Height / picBox.Height
    '            If divideByW > 1 Or divideByH > 1 Then
    '                If divideByW > divideByH Then
    '                    divideBy = divideByW
    '                Else
    '                    divideBy = divideByH
    '                End If

    '                imgShow = New Bitmap(CInt(CDbl(imgOrg.Width) / divideBy), CInt(CDbl(imgOrg.Height) / divideBy))
    '                imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
    '                g = Graphics.FromImage(imgShow)
    '                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
    '                g.DrawImage(imgOrg, New Rectangle(0, 0, CInt(CDbl(imgOrg.Width) / divideBy), CInt(CDbl(imgOrg.Height) / divideBy)), 0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel)
    '                g.Dispose()
    '            Else
    '                imgShow = New Bitmap(imgOrg.Width, imgOrg.Height)
    '                imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
    '                g = Graphics.FromImage(imgShow)
    '                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
    '                g.DrawImage(imgOrg, New Rectangle(0, 0, imgOrg.Width, imgOrg.Height), 0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel)
    '                g.Dispose()
    '            End If
    '            imgOrg.Dispose()

    '            picBox.Image = imgShow
    '        Else
    '            picBox.Image = My.Resources.no_photo
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.ToString)
    '    End Try
    'End Sub

    Public Function GetNewBatchno(ByVal CostId As String, ByVal BillDate As Date, ByVal Transact As OleDbTransaction, Optional ByVal Isestimate As Boolean = False) As String
        Dim StrSql As String = Nothing

        StrSql = " DECLARE @RETBATCHVALUE VARCHAR(15)"
        If Isestimate Then StrSql += " EXEC " & cnStockDb & "..GET_ESTBATCHNO" Else StrSql += " EXEC " & cnStockDb & "..GET_BATCHNO"
        StrSql += " @COSTID = '" & CostId & "'"
        StrSql += " ,@BILLDATE = '" & BillDate.ToString("yyyy-MM-dd") & "'"
        StrSql += " ,@COMPANYID = '" & strCompanyId & "'"
        StrSql += " ,@RETVALUE = @RETBATCHVALUE OUTPUT"
        StrSql += " SELECT @RETBATCHVALUE"
        Return objGPack.GetSqlValue(StrSql, , , Transact)
    End Function

    Public Function GetNewBatchnoNew(ByVal CostId As String, ByVal BillDate As Date, ByVal CCn As OleDbConnection, ByVal Transact As OleDbTransaction, Optional ByVal Isestimate As Boolean = False) As String
        Dim StrSql As String = Nothing

        StrSql = " DECLARE @RETBATCHVALUE VARCHAR(15)"
        If Isestimate Then StrSql += " EXEC " & cnStockDb & "..GET_ESTBATCHNO" Else StrSql += " EXEC " & cnStockDb & "..GET_BATCHNO"
        StrSql += " @COSTID = '" & CostId & "'"
        StrSql += " ,@BILLDATE = '" & BillDate.ToString("yyyy-MM-dd") & "'"
        StrSql += " ,@COMPANYID = '" & strCompanyId & "'"
        StrSql += " ,@RETVALUE = @RETBATCHVALUE OUTPUT"
        StrSql += " SELECT @RETBATCHVALUE"
        Return BrighttechPack.GlobalMethods.GetSqlValue(CCn, StrSql, , , Transact)
    End Function



    Public Function GetNewSno(ByVal CtlId As TranSnoType, ByVal Transact As OleDbTransaction, Optional ByVal ProcName As String = "GET_TRANSNO_TRAN", Optional ByVal CompId As String = Nothing, Optional ByVal ProcDbName As String = Nothing) As String
        If ProcDbName = Nothing Then ProcDbName = cnStockDb
        Dim StrSql As String = Nothing
        StrSql = " DECLARE @RETSNOVALUE VARCHAR(15)"
        StrSql += " EXEC " & ProcDbName & ".." & ProcName
        StrSql += " @COSTID = '" & cnCostId & "'"
        StrSql += " ,@CTLID = '" & CtlId.ToString & "'"
        StrSql += " ,@CHECKTABLENAME = '" & CtlId.ToString.ToUpper.Replace("CODE", "") & "'"
        StrSql += " ,@COMPANYID = '" & IIf(CompId = Nothing, strCompanyId, CompId) & "'"
        StrSql += " ,@RETVALUE = @RETSNOVALUE OUTPUT"
        StrSql += " SELECT @RETSNOVALUE"
        Return objGPack.GetSqlValue(StrSql, , , Transact)
    End Function

    Public Function GetNewSnoNew(ByVal CtlId As TranSnoType, ByVal CCn As OleDbConnection, ByVal Transact As OleDbTransaction, Optional ByVal ProcName As String = "GET_TRANSNO_TRAN", Optional ByVal CompId As String = Nothing, Optional ByVal ProcDbName As String = Nothing) As String
        If ProcDbName = Nothing Then ProcDbName = cnStockDb
        Dim StrSql As String = Nothing
        StrSql = " DECLARE @RETSNOVALUE VARCHAR(15)"
        StrSql += " EXEC " & ProcDbName & ".." & ProcName
        StrSql += " @COSTID = '" & cnCostId & "'"
        StrSql += " ,@CTLID = '" & CtlId.ToString & "'"
        StrSql += " ,@CHECKTABLENAME = '" & CtlId.ToString.ToUpper.Replace("CODE", "") & "'"
        StrSql += " ,@COMPANYID = '" & IIf(CompId = Nothing, strCompanyId, CompId) & "'"
        StrSql += " ,@RETVALUE = @RETSNOVALUE OUTPUT"
        StrSql += " SELECT @RETSNOVALUE"
        Return BrighttechPack.GlobalMethods.GetSqlValue(CCn, StrSql, , , Transact)
    End Function


    Public Function GetMaxTranNo(ByVal BillDate As Date, ByVal Transact As OleDbTransaction) As Integer
        Dim Strsql As String
        Strsql = " DECLARE @RE VARCHAR(50)"
        Strsql += " EXEC " & cnStockDb & "..GET_TRANNO_MAX"
        Strsql += " @BILLDATE = '" & BillDate.ToString("yyyy-MM-dd") & "'"
        Strsql += " ,@RETVALUE = @RE OUTPUT"
        Strsql += " SELECT @RE AS TRANNO"
        Return Val(objGPack.GetSqlValue(Strsql, , , Transact))
    End Function
#End Region

#Region "GridDispDiaMethods"
    Public Function SetGroupHeadColWid(ByVal HeadCol As DataGridViewColumn, ByVal Grid As DataGridView) As Integer
        Dim wid As Integer = 0
        Dim vis As Boolean = False
        Dim cNames() As String = HeadCol.Name.Split("~")
        For Each s As String In cNames
            If Grid.Columns.Contains(s) Then
                wid += IIf(Grid.Columns(s).Visible, Grid.Columns(s).Width, 0)
            Else
                wid = HeadCol.Width
            End If
        Next
        If Not HeadCol.Name.Contains("~") Then
            HeadCol.HeaderText = ""
        End If
        Return wid
    End Function

    Public Function GetTagSaleValue(ByVal ItemId As Integer, ByVal TagNo As String, ByVal ItemRate As Decimal, Optional ByVal chkwt As Decimal = 0) As Decimal
        Dim dtTagInfo As New DataTable
        Dim TagSalValue As Decimal = Nothing
        strSql = vbCrLf + " SELECT T.GRSWT,T.NETWT,T.GRSNET,P.PURGRSNET,P.PURNETWT,P.PURWASTAGE,P.PURTOUCH,P.PURMC"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(PURAMT) FROM " & cnAdminDb & "..PURITEMTAGSTONE WHERE TAGSNO = P.TAGSNO),0)PURSTNAMT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(PURAMOUNT) FROM " & cnAdminDb & "..PURITEMTAGMISCCHAR WHERE TAGSNO = P.TAGSNO),0)PURMISCAMT"
        strSql += vbCrLf + " ,T.SALVALUE,P.PURVALUE,T.ADD_VA_PER"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
        strSql += vbCrLf + " WHERE T.ITEMID =  " & ItemId & "  AND T.TAGNO = '" & TagNo & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTagInfo)
        If Not dtTagInfo.Rows.Count > 0 Then Return 0
        Dim RoPur As DataRow = dtTagInfo.Rows(0)
        TagSalValue = Val(RoPur!SALVALUE.ToString)
        Dim tagwt As Decimal = IIf(RoPur!GRSNET.ToString = "N" And Val(RoPur!NETWT.ToString) <> 0, Val(RoPur!NETWT.ToString), Val(RoPur!GRSWT.ToString))

        If chkwt <> 0 Then TagSalValue = (TagSalValue / tagwt) * chkwt
                If Val(RoPur!PURVALUE.ToString) <> 0 And Val(RoPur!ADD_VA_PER.ToString) <> 0 Then
            Dim purValue As Double = Nothing
            If chkwt <> 0 Then
                    purValue = (chkwt + Val(RoPur!PURWASTAGE.ToString)) _
            * IIf(Val(RoPur!PURTOUCH.ToString) > 0, (Val(RoPur!PURTOUCH.ToString) / 100), 1)

            Else
                purValue = (IIf(RoPur!PURGRSNET.ToString = "G", Val(RoPur!GRSWT.ToString), Val(RoPur!PURNETWT.ToString)) + Val(RoPur!PURWASTAGE.ToString)) _
                            * IIf(Val(RoPur!PURTOUCH.ToString) > 0, (Val(RoPur!PURTOUCH.ToString) / 100), 1)
            End If
            
            purValue = purValue * ItemRate
            purValue += Val(RoPur!PURSTNAMT.ToString) + Val(RoPur!PURMISCAMT.ToString) + Val(RoPur!PURMC.ToString)

            Dim AddVaAmt = purValue * (Val(RoPur!ADD_VA_PER.ToString) / 100)
            purValue = purValue + AddVaAmt
            If TagSalValue < purValue Then
                TagSalValue = purValue
            End If
        End If
        Return TagSalValue
    End Function

    Public Sub SetGridHeadColWidth(ByVal gridViewHeader As DataGridView)
        Dim f As frmGridDispDia
        f = objGPack.GetParentControl(gridViewHeader)
        If Not f.gridViewHeader.Visible Then Exit Sub
        If f.gridViewHeader Is Nothing Then Exit Sub
        If Not f.gridView.ColumnCount > 0 Then Exit Sub
        If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With f.gridViewHeader
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).Width = SetGroupHeadColWid(.Columns(cnt), f.gridView)
            Next
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To f.gridView.ColumnCount - 1
                If f.gridView.Columns(cnt).Visible Then colWid += f.gridView.Columns(cnt).Width
            Next
            If colWid >= f.gridView.Width Then
                f.gridViewHeader.Columns("SCROLL").Visible = CType(f.gridView.Controls(1), VScrollBar).Visible
                f.gridViewHeader.Columns("SCROLL").Width = CType(f.gridView.Controls(1), VScrollBar).Width
                f.gridViewHeader.Columns("SCROLL").HeaderText = ""
            Else
                f.gridViewHeader.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub

    'Public Sub SetGridHeadColWidth(ByVal gridViewHeader As DataGridView)
    '    Dim f As frmGridDispDia
    '    f = objGPack.GetParentControl(gridViewHeader)
    '    If Not f.gridViewHeader.Visible Then Exit Sub
    '    If f.gridViewHeader Is Nothing Then Exit Sub
    '    If Not f.gridView.ColumnCount > 0 Then Exit Sub
    '    If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
    '    With f.gridViewHeader
    '        For cnt As Integer = 0 To .ColumnCount - 1
    '            .Columns(cnt).Width = SetGroupHeadColWid(.Columns(cnt), f.gridView)
    '        Next
    '        Dim colWid As Integer = 0
    '        For cnt As Integer = 0 To f.gridView.ColumnCount - 1
    '            If f.gridView.Columns(cnt).Visible Then colWid += f.gridView.Columns(cnt).Width
    '        Next
    '        If colWid >= f.gridView.Width Then
    '            f.gridViewHeader.Columns("SCROLL").Visible = CType(f.gridView.Controls(1), VScrollBar).Visible
    '            f.gridViewHeader.Columns("SCROLL").Width = CType(f.gridView.Controls(1), VScrollBar).Width
    '            f.gridViewHeader.Columns("SCROLL").HeaderText = ""
    '        Else
    '            f.gridViewHeader.Columns("SCROLL").Visible = False
    '        End If
    '    End With
    'End Sub
#End Region

    Public Function GetSubItemQry(ByVal SelectColumns() As String, Optional ByVal ItemId As Integer = 0) As String
        Dim SelCol As String = Nothing
        For Each s As String In SelectColumns
            SelCol += s & ","
        Next
        SelCol = Mid(SelCol, 1, SelCol.Length - 1)
        Dim Str As String
        Str = "SELECT " & SelCol & " FROM " & cnAdminDb & "..SUBITEMMAST"
        Str += " WHERE ITEMID = " & ItemId
        Str += " AND ACTIVE = 'Y'"
        Str += " ORDER BY "
        If _SubItemOrderByName Then
            Str += " SUBITEMNAME"
        Else
            Str += " DISPLAYORDER,SUBITEMNAME"
        End If
        Return Str
    End Function

    Public Function GetAcNameQryFilteration(Optional ByVal AliasName As String = Nothing) As String
        Dim Str As String = Nothing
        Str += " AND ISNULL(" & IIf(AliasName <> "", AliasName & ".", "") & "ACTIVE,'Y') NOT IN ('N','H')"
        Str += " AND (ISNULL(" & IIf(AliasName <> "", AliasName & ".", "") & "COMPANYID,'') = ''"
        Str += " OR ISNULL(" & IIf(AliasName <> "", AliasName & ".", "") & "COMPANYID,'') LIKE '%" & strCompanyId & "%')"
        Return Str
    End Function


    Public Function GetItemQryFilteration(Optional ByVal ModuleId As String = "A", Optional ByVal AliasName As String = Nothing) As String
        ModuleId = ModuleId.ToUpper
        Dim Str As String = Nothing
        Str = " AND ISNULL(" & IIf(AliasName <> "", AliasName & ".", "") & "ACTIVE,'Y') = 'Y'"
        Str += " AND (ISNULL(" & IIf(AliasName <> "", AliasName & ".", "") & "COMPANYID,'') = ''"
        Str += " OR ISNULL(" & IIf(AliasName <> "", AliasName & ".", "") & "COMPANYID,'') LIKE '%" & strCompanyId & "%')"
        If ModuleId = "S" And cnCentStock Then
            Str = " AND ISNULL(" & IIf(AliasName <> "", AliasName & ".", "") & "ACTIVE,'Y') = 'Y'"
        End If
        Return Str
    End Function

    Public Sub BillNoRegenerator()
        If objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'REGENBILLNO' AND CTLTEXT = 'Y'" & IIf(strBCostid <> Nothing, " AND COSTID ='" & strBCostid & "'", "")) = "" Then Exit Sub
        If MessageBox.Show("Sure, You want to reset billno?", "BillNo Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> DialogResult.Yes Then
            Exit Sub
        End If
        'Dim objSecret As New frmAdminPassword()
        'If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
        '    Exit Sub
        'End If
        strSql = vbCrLf + " UPDATE " & cnStockDb & "..BILLCONTROL "
        strSql += vbCrLf + " SET CTLTEXT = ''"
        strSql += vbCrLf + " WHERE CTLTYPE = 'N' "
        strSql += vbCrLf + " AND COMPANYID IN (SELECT COMPANYID FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'REGENBILLNO' AND CTLTEXT = 'Y')"
        If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
        strSql += vbCrLf + " AND CTLID NOT IN"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " 'GEN-CRSALESBILLNO'"
        strSql += vbCrLf + " ,'GEN-APPISSBILLNO'"
        strSql += vbCrLf + " ,'GEN-APPRECBILLNO'"
        strSql += vbCrLf + " ,'GEN-ADVANCEBILLNO'"
        strSql += vbCrLf + " ,'ORDERNO','REPAIRNO'"
        strSql += vbCrLf + " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'Alter By vasanth On -17/10/2013 
        'strSql = vbCrLf + " UPDATE " & cnStockDb & "..BILLCONTROL "
        'strSql += vbCrLf + " SET CTLTEXT = '" + Mid(Format(GetServerDate()), 6, 2) + Mid(Format(GetServerDate()), 9, 2) + "0'"
        'strSql += vbCrLf + " WHERE CTLTYPE = 'N' "
        'strSql += vbCrLf + " AND COMPANYID IN (SELECT COMPANYID FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'REGENBILLNO' AND CTLTEXT = 'Y')"
        'strSql += vbCrLf + " AND CTLID IN"
        'strSql += vbCrLf + " ('ORDERNO','REPAIRNO')"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()


        MsgBox("Bill information reset successfully", MsgBoxStyle.Information)
    End Sub


    Public Sub SetGlobalVariables()
        strSql = " EXEC " & cnAdminDb & "..SP_GETCONTROLS @DBNAME='" & cnAdminDb & "',@IDS=''"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        Dim dtSoftKeys As DataTable
        dtSoftKeys = dss.Tables(0)

        'Dim Is_Productdisc As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "POS_DISC_OPT", "N")
        _IsWholeSaleType = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "ISWHOLESALE", "N") = "Y", True, False)
        _HideBackOffice = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "HIDEBACKOFFICE", "N") = "Y", True, False)
        _CounterWiseCashMaintanance = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "COUNT-CASH", "N") = "Y", True, False)
        _SubItemOrderByName = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "ORDER_SUBITEM", "Y") = "Y", True, False)
        cnCentStock = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "CENT-STOCK", "Y") = "Y", True, False)
        cnChitTrandb = GetAdmindbSoftValuefromDt(dtSoftKeys, "CHITDBPREFIX", "") & "SH0708"
        cnChitCompanyid = GetAdmindbSoftValuefromDt(dtSoftKeys, "CHITDBPREFIX", "")
        _MainCompId = GetAdmindbSoftValuefromDt(dtSoftKeys, "COMPANYID", "")
        If GetAdmindbSoftValuefromDt(dtSoftKeys, "CENTR_DB_ALLCOSTID", "N") = "N" And cnCostId = "" Then
            cnCostId = GetAdmindbSoftValuefromDt(dtSoftKeys, "COSTID", "")
        End If
        cnHOCostId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'")
        cnDefaultCostId = IIf(cnCostId = cnHOCostId, True, False)
        _CashOpening = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "RPTWITH_CASH", "N") = "Y", True, False)
        DiaRnd = Val(GetAdmindbSoftValuefromDt(dtSoftKeys, "ROUNDOFF-DIA", 4))
        EXE_WITH_PARAM = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "EXE_WITH_PARAM", "N") = "Y", True, False)
        _SyncTo = GetAdmindbSoftValuefromDt(dtSoftKeys, "SYNC-TO", "")
        _HasCostcentre = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "COSTCENTRE", "N") = "Y", True, False)
        COSTCENTRE_SINGLE = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "COSTCENTRE_SINGLE", "N") = "Y", True, False)
        PIC_ITEMWISE = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "PIC_ITEMWISE", "N") = "Y", True, False)
        PICPATH = GetAdmindbSoftValuefromDt(dtSoftKeys, "PICPATH", "") : PICPATH = PICPATH & IIf(PICPATH.EndsWith("\") = False, "\", "")
        WTAMTOPT = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "WTAMTOPT", "N") = "Y", True, False)
    End Sub
    Public Function GetSqlValue(ByVal Cn As OleDb.OleDbConnection, ByVal Qry As String) As Object
        Dim Obj As Object = Nothing
        Dim Da As OleDb.OleDbDataAdapter
        Dim DtTemp As New DataTable
        Da = New OleDb.OleDbDataAdapter(Qry, Cn)
        Da.Fill(DtTemp)
        If DtTemp.Rows.Count > 0 Then
            Obj = DtTemp.Rows(0).Item(0)
        End If
        Return Obj
    End Function

    Public Sub SetSettingsObj(ByVal Obj As Object, ByVal frmName As String, ByVal ObjType As Type)
        Dim SettingsFileName As String = IO.Path.GetTempPath & "\RPT_" & frmName & ".xml"
        Dim objStreamWriter As New IO.StreamWriter(SettingsFileName)
        Dim x As New System.Xml.Serialization.XmlSerializer(ObjType)
        x.Serialize(objStreamWriter, Obj)
        objStreamWriter.Close()

        Dim fileStr As New IO.FileStream(SettingsFileName, IO.FileMode.Open, IO.FileAccess.Read)
        Dim reader As New IO.BinaryReader(fileStr)
        Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
        fileStr.Read(result, 0, result.Length)
        fileStr.Close()

        strSql = " DELETE FROM " & cnAdminDb & "..APPDATA WHERE FILENAMES = 'RPT_" & frmName & ".xml'"
        strSql += " AND USERID = " & userId & ""
        cmd = New OleDb.OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        Dim fiINfo As New IO.FileInfo(SettingsFileName)
        strSql = " INSERT INTO " & cnAdminDb & "..APPDATA"
        strSql += vbCrLf + " (FILENAMES"
        strSql += vbCrLf + " ,CONTENT,USERID"
        strSql += vbCrLf + "  )"
        strSql += vbCrLf + "  VALUES"
        strSql += vbCrLf + "  (?,?,?)"
        cmd = New OleDb.OleDbCommand(strSql, cn)
        cmd.Parameters.AddWithValue("@FILENAMES", fiINfo.Name)
        cmd.Parameters.AddWithValue("@CONTENT", result)
        cmd.Parameters.AddWithValue("@USERID", userId)
        cmd.ExecuteNonQuery()
    End Sub

    Public Sub GetSettingsObj(ByRef Obj As Object, ByVal frmName As String, ByVal ObjType As Type)
        Dim SettingsFileName As String = IO.Path.GetTempPath & "\RPT_" & frmName & ".xml"
        strSql = " SELECT CONTENT FROM " & cnAdminDb & "..APPDATA WHERE FILENAMES = 'RPT_" & frmName & ".xml'"
        strSql += " AND USERID = " & userId & ""
        Dim filContent As Object = GetSqlValue(cn, strSql)
        If filContent IsNot Nothing Then
            Dim myByte() As Byte = filContent
            Dim fileStr As New IO.FileStream(SettingsFileName, IO.FileMode.Create, IO.FileAccess.ReadWrite)
            fileStr.Write(myByte, 0, myByte.Length)
            fileStr.Close()

            Dim fInfo As New IO.FileInfo(SettingsFileName)
            Dim objStreamReader As New IO.StreamReader(SettingsFileName)
            Dim x As New System.Xml.Serialization.XmlSerializer(ObjType)
            Obj = x.Deserialize(objStreamReader)
            objStreamReader.Close()
            Exit Sub
        End If

        strSql = " SELECT CONTENT FROM " & cnAdminDb & "..APPDATA WHERE FILENAMES = 'RPT_" & frmName & ".xml'"
        strSql += " AND ISNULL(USERID,0) = 0"
        filContent = GetSqlValue(cn, strSql)
        If filContent IsNot Nothing Then
            Dim myByte() As Byte = filContent
            Dim fileStr As New IO.FileStream(SettingsFileName, IO.FileMode.Create, IO.FileAccess.ReadWrite)
            fileStr.Write(myByte, 0, myByte.Length)
            fileStr.Close()

            Dim fInfo As New IO.FileInfo(SettingsFileName)
            Dim objStreamReader As New IO.StreamReader(SettingsFileName)
            Dim x As New System.Xml.Serialization.XmlSerializer(ObjType)
            Obj = x.Deserialize(objStreamReader)
            objStreamReader.Close()
        End If
    End Sub
    Public Sub GetChecked_CheckedList(ByVal chkLst As CheckedListBox, ByVal CheckedItems As List(Of String))
        CheckedItems.Clear()
        For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
            CheckedItems.Add(chkLst.CheckedItems.Item(cnt).ToString)
        Next
    End Sub
    Public Sub GetChecked_CheckedList(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal CheckedItems As List(Of String))
        CheckedItems.Clear()
        For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
            CheckedItems.Add(chkLst.CheckedItems.Item(cnt).ToString)
        Next
    End Sub
    Public Sub SetChecked_CheckedList(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal CheckedItems As List(Of String), ByVal DefCheckVal As String)
        If CheckedItems.Count > 0 Then DefCheckVal = Nothing
        For cnt As Integer = 0 To chkLst.Items.Count - 1
            If CheckedItems.Contains(chkLst.Items.Item(cnt).ToString) Or (chkLst.Items.Item(cnt).ToString = DefCheckVal And DefCheckVal <> Nothing) Then
                chkLst.SetItemChecked(cnt, True)
            Else
                chkLst.SetItemChecked(cnt, False)
            End If
        Next
    End Sub
    Public Sub SetChecked_CheckedList(ByVal chkLst As CheckedListBox, ByVal CheckedItems As List(Of String), ByVal DefCheckVal As String)
        If CheckedItems.Count > 0 Then DefCheckVal = Nothing
        For cnt As Integer = 0 To chkLst.Items.Count - 1
            If CheckedItems.Contains(chkLst.Items.Item(cnt).ToString) Or (chkLst.Items.Item(cnt).ToString = DefCheckVal And DefCheckVal <> Nothing) Then
                chkLst.SetItemChecked(cnt, True)
            Else
                chkLst.SetItemChecked(cnt, False)
            End If
        Next
    End Sub

    Public Sub SetChecked_CheckedList(ByVal chkLst As CheckedListBox, ByVal status As Boolean)
        For cnt As Integer = 0 To chkLst.Items.Count - 1
            chkLst.SetItemChecked(cnt, status)
        Next
    End Sub

    Public Function GetCostId(ByVal CCostId As String) As String
        If CCostId = Nothing Then CCostId = ""
        Dim RetStr As String = ""
        For cnt As Integer = 1 To 2 - CCostId.ToString.Length
            RetStr += "0"
        Next
        RetStr += CCostId
        Return RetStr
    End Function
    Public Function GetCompanyId(ByVal CompanyId As String) As String
        Dim RetStr As String = ""
        For cnt As Integer = 1 To 3 - CompanyId.Length
            RetStr += "0"
        Next
        RetStr += CompanyId
        Return RetStr
    End Function
    Public Function GetBillDate(ByRef BillDate As Date) As Boolean
        If GetAdmindbSoftValue("GLOBALDATE", "N").ToUpper = "Y" Then
            BillDate = GetAdmindbSoftValue("GLOBALDATEVAL", BillDate)
            Return True
        Else
            Dim RetBilldate As Date
            RetBilldate = GetServerDate()
            If BillDate <> RetBilldate Then
                Select Case MessageBox.Show("Billdate and Entrydate both are different, Do you want to continue with " & BillDate.ToString("dd/MM/yyyy") & "?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                    Case DialogResult.Yes
                        Return True
                    Case DialogResult.No
                        BillDate = RetBilldate
                        Return True
                End Select
            Else
                Return True
            End If
        End If
        Return False
    End Function
    Public Function GetSqlTable(ByVal StrSql As String, ByVal lCn As OleDbConnection, Optional ByVal Tran As OleDbTransaction = Nothing) As DataTable
        Dim Cmd As New OleDbCommand(StrSql, lCn)
        If Tran IsNot Nothing Then Cmd.Transaction = Tran
        Dim Da As New OleDbDataAdapter(Cmd)
        Dim DtTemp As New DataTable
        Da.Fill(DtTemp)
        Return DtTemp
    End Function
    Public Function GetSqlRow(ByVal StrSql As String, ByVal lCn As OleDbConnection, Optional ByVal Tran As OleDbTransaction = Nothing) As DataRow
        Dim Dr As DataRow = Nothing
        Dim Cmd As New OleDbCommand(StrSql, lCn)
        If Tran IsNot Nothing Then Cmd.Transaction = Tran
        Dim Da As New OleDbDataAdapter(Cmd)
        Dim DtTemp As New DataTable
        Da.Fill(DtTemp)
        If DtTemp.Rows.Count > 0 Then
            Dr = DtTemp.Rows(0)
        End If
        Return Dr
    End Function

    Function funcComboChecker(ByVal field As String, ByVal cmb As ComboBox, Optional ByVal emptyCheck As Boolean = True) As Boolean
        If cmb.Text.Contains("'") Then
            MsgBox("Single Quote Not Allowed", MsgBoxStyle.Information)
            Return True
        End If
        If emptyCheck Then
            If cmb.Text = "" Then
                MsgBox(field + " Should Not Empty", MsgBoxStyle.Information)
                cmb.Focus()
                Return True
            End If
        End If
        If cmb.Items.Count > 0 Then
            If cmb.Text <> "" Then
                If cmb.Items.Contains(cmb.Text) = False Then
                    MsgBox("Invalid " + field, MsgBoxStyle.Information)
                    cmb.Focus()
                    Return True
                End If
            End If
        End If
    End Function

    Public Function usrpwdokonly(ByVal PWDCTRL As String, ByVal IS_USERLEVELPWD As Boolean) As Boolean

        If IS_USERLEVELPWD Then
            Dim pwdid As Integer = 0 : Dim pwdpass As Boolean = False
            Dim Sqlqry As String = "Select Optionid from " & cnAdminDb & "..PRJPWDOPTION where OPTIONNAME ='" & PWDCTRL & "' AND active = 'Y'"
            Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(Sqlqry, , , tran))
            If Optionid <> 0 Then
                pwdid = GetuserPwd(Optionid, cnCostId, userId)
                If pwdid <> 0 Then
                    Dim objUpwd As New frmUserPassword(pwdid)
                    If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then pwdpass = False Else pwdpass = True
                End If
            Else
                pwdpass = False
            End If
            Return pwdpass
        End If
        Return False
    End Function
    Public Function usrpwdok(ByVal PWDCTRL As String, ByVal IS_USERLEVELPWD As Boolean) As Boolean

        If IS_USERLEVELPWD Then
            Dim pwdid As Integer = 0 : Dim pwdpass As Boolean = False
            Dim Sqlqry As String = "Select Optionid from " & cnAdminDb & "..PRJPWDOPTION where OPTIONNAME ='" & PWDCTRL & "' AND active = 'Y'"
            Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(Sqlqry, , , tran))
            If Optionid = 0 Then pwdpass = True
            If userId <> 999 And Optionid <> 0 And Not _IsAdmin Then
                pwdid = GetuserPwd(Optionid, cnCostId, userId)
                If pwdid <> 0 Then
                    Dim objUpwd As New frmUserPassword(pwdid)
                    If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then pwdpass = False Else pwdpass = True
                End If
            Else
                pwdpass = True
            End If
            Return pwdpass
        Else
            Return True
        End If
        Return False
    End Function

    Public Function usrpwdid(ByVal PWDCTRL As String, ByVal IS_USERLEVELPWD As Boolean) As Integer
        If IS_USERLEVELPWD Then
            Dim pwdid As Integer = 0 : Dim pwdpass As Boolean = False
            Dim Sqlqry As String = "Select Optionid from " & cnAdminDb & "..PRJPWDOPTION where OPTIONNAME ='" & PWDCTRL & "' AND active = 'Y'"
            Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(Sqlqry, , , tran))
            If Optionid = 0 Then pwdid = 1
            If userId <> 999 And Optionid <> 0 And Not _IsAdmin Then
                pwdid = GetuserPwd(Optionid, cnCostId, userId)
                If pwdid <> 0 Then
                    Dim objUpwd As New frmUserPassword(pwdid)
                    If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then pwdid = 0 Else Return pwdid
                End If
            Else
                pwdpass = True : pwdid = 1
            End If
            Return pwdid
        Else
            Return 1
        End If
        Return 0
    End Function

    Public Function ChkDbClosed(Optional ByVal dbname As String = "") As Boolean
        If dbname = "" Then dbname = cnStockDb
        strSql = " SELECT CLOSED FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME='" & dbname & "'"
        If objGPack.GetSqlValue(strSql, , "N", tran) = "Y" Then
            Return True
        End If
        Return False
    End Function

    Public Function SmsSend(ByVal Msg As String, ByVal Mobile As String) As Boolean
        If Msg <> "" And Mobile.Length = 10 Then
            strSql = "SELECT COUNT(*)CNT FROM SYSDATABASES WHERE NAME='AKSHAYASMSDB'"
            If objGPack.GetSqlValue(strSql, "CNT", 0) > 0 Then
                strSql = "INSERT INTO AKSHAYASMSDB..SMSDATA(MOBILENO,MESSAGES,STATUS,EXPIRYDATE,UPDATED)"
                strSql += " VALUES"
                strSql += " ("
                strSql += " '" & Mobile.Trim & "','" & Msg & "','N'"
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Today.Date.ToString("yyyy-MM-dd") & "'"
                strSql += " ) "
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                Return True
            Else
                MsgBox("AkhayaSmsDb Not Found", MsgBoxStyle.Information)
                Return False
            End If
        End If
        Return True
    End Function
End Module
