Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Imports System.Net.Mail
Public Class frmExemptionReport
    Dim strsql As String = String.Empty
    Dim title As String = String.Empty
    Dim dtOptions As New DataTable
    Dim dtCostCentre As New DataTable
    Dim til As String = String.Empty
    Private Sub frmExceptionReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            FuncNew()
            strsql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strsql += " UNION ALL"
            strsql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strsql += " ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
            If strUserCentrailsed <> "Y" And cnDefaultCostId Then chkCmbCostCentre.Enabled = False
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
    Private Sub FuncNew()
        Me.lblTitle.Text = ""
        dtpEditDate.Value = GetServerDate()
        strsql = " SELECT 'ALL' OPTIONNAME,'ALL' OPTIONID,1 RESULT"
        strsql += " UNION ALL"
        strsql += " SELECT OPTIONNAME,CONVERT(VARCHAR,OPTIONID),2 RESULT FROM " & cnAdminDb & "..PRJPWDOPTION"
        strsql += " ORDER BY RESULT,OPTIONNAME"
        dtOptions = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtOptions)
        BrighttechPack.GlobalMethods.FillCombo(cmbOptions, dtOptions, "OPTIONNAME", )
        GridView.DataSource = Nothing
        dtpEditDate.Focus()
        Prop_Gets()
    End Sub

    Private Sub FuncView()
        Dim Optionid As String = ""
        AutoResizeToolStripMenuItem.Checked = False
        title = dtpEditDate.Value.ToString("MM/dd/yyyy")
        Dim CHKCOSTID As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", False)
        If cmbOptions.Text <> "ALL" Then
            Optionid = objGPack.GetSqlValue("SELECT OPTIONID FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME = '" & cmbOptions.Text & "'")
        End If


        strsql = "EXEC " & cnAdminDb & "..SP_RPT_EXEMPTION "
        If chkDate.Checked = True Then
            strsql += vbCrLf + " @FROMDATE='" & cnTranFromDate.ToString("MM/dd/yyyy") & "'"
            strsql += vbCrLf + " ,@TODATE='" & dtpEditDate.Value.ToString("MM/dd/yyyy") & "'"
        Else
            strsql += vbCrLf + " @FROMDATE='" & dtpEditDate.Value.ToString("MM/dd/yyyy") & "'"
            strsql += vbCrLf + " ,@TODATE='" & dtpTo.Value.ToString("MM/dd/yyyy") & "'"
        End If
        strsql += vbCrLf + ",@COSTID='" & CHKCOSTID & "'"
        strsql += vbCrLf + ",@OPTIONID='" & Optionid & "'"
        strsql += vbCrLf + ",@FORMAT='" & IIf(rbtFormat1.Checked, 1, 2) & "'"
        strsql += vbCrLf + ",@DBNAME='" & cnStockDb & "'"
        strsql += vbCrLf + ",@WITHDISC='" & IIf(ChkwithDisc.Checked, "Y", "N") & "'"
        Dim ds As New DataSet
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(ds)
        dt = ds.Tables(0)
        If ds.Tables(0).Rows.Count > 0 Then
            'GridView.DataSource = ds.Tables(0)
            GridView.DataSource = dt
            'GridColor()
            GridStyle()
            Heading()
            GridViewHeaderStyle()
        Else
            GridView.DataSource = Nothing
            Me.lblTitle.Text = ""
            MsgBox("No Records Found.")
            btnsendmail.Enabled = False
        End If
        Prop_Sets()
    End Sub
    Private Sub GridViewHeaderStyle()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        If rbtFormat1.Checked Then
        Else
            With dtMergeHeader
                If ChkwithDisc.Checked Then
                    .Columns.Add("BILLNO~DATE~TIME~USERNAME~TYPEOFOTP~DISCALLOWAMT~DISCOUNT", GetType(String))
                Else
                    .Columns.Add("BILLNO~DATE~TIME~USERNAME~TYPEOFOTP", GetType(String))
                End If
                .Columns.Add("TOCOSTID~BUSERNAME~REMARKS~CLOSEOTPDATEANDTIME~STATUS", GetType(String))
                If ChkwithDisc.Checked Then
                    .Columns("BILLNO~DATE~TIME~USERNAME~TYPEOFOTP~DISCALLOWAMT~DISCOUNT").Caption = "CORPUSERDETAILS"
                Else
                    .Columns("BILLNO~DATE~TIME~USERNAME~TYPEOFOTP").Caption = "CORPUSERDETAILS"
                End If
                .Columns("TOCOSTID~BUSERNAME~REMARKS~CLOSEOTPDATEANDTIME~STATUS").Caption = "BRANCHUSERDETAILS"
                .Columns.Add("SCROLL", GetType(String))
                .Columns("SCROLL").Caption = ""

            End With
            With GridHead
                .DataSource = dtMergeHeader
                If ChkwithDisc.Checked Then
                    .Columns("BILLNO~DATE~TIME~USERNAME~TYPEOFOTP~DISCALLOWAMT~DISCOUNT").HeaderText = "CORPUSERDETAILS"
                Else
                    .Columns("BILLNO~DATE~TIME~USERNAME~TYPEOFOTP").HeaderText = "CORPUSERDETAILS"
                End If
                .Columns("TOCOSTID~BUSERNAME~REMARKS~CLOSEOTPDATEANDTIME~STATUS").HeaderText = "BRANCHUSERDETAILS"
                .Columns("SCROLL").HeaderText = ""
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                funcColWidth()
                GridView.Focus()
                Dim colWid As Integer = 0
                For cnt As Integer = 0 To GridView.ColumnCount - 1
                    If GridView.Columns(cnt).Visible Then colWid += GridView.Columns(cnt).Width
                Next
                If colWid >= GridView.Width Then
                    .Columns("SCROLL").Visible = CType(GridView.Controls(0), HScrollBar).Visible
                    .Columns("SCROLL").Width = CType(GridView.Controls(1), VScrollBar).Width
                Else
                    .Columns("SCROLL").Visible = False
                End If
            End With
        End If

    End Sub
    Private Sub GridStyle()
        If rbtFormat1.Checked Then
            With GridView
                GridHead.Visible = False
                .Columns("DESCRIPTION").Width = 250
                .Columns("DESCRIPTION").ReadOnly = True
                .Columns("OPTIONNAME").Width = 150
                .Columns("OPTIONNAME").ReadOnly = True
                .Columns("USERNAME").Width = 150
                .Columns("USERNAME").ReadOnly = True
                .Columns("EXEMPDATE").Width = 100
                .Columns("EXEMPDATE").ReadOnly = True
                .Columns("EXEMPTIME").Width = 100
                .Columns("EXEMPTIME").ReadOnly = True
                .Columns("COSTNAME").Visible = False
                .Columns("RESULT").Visible = False
                If .Columns.Contains("DISCOUNT") Then
                    .Columns("DISCOUNT").Visible = False
                End If
                If .Columns.Contains("DISCALLOWAMT") Then
                    .Columns("DISCALLOWAMT").Visible = False
                End If
            End With
            If GridView.Columns.Contains("EXEMPDATE") Then GridView.Columns("EXEMPDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            If GridView.Columns.Contains("EXEMPTIME") Then GridView.Columns("EXEMPTIME").DefaultCellStyle.Format = "hh:mm:ss tt"
            btnsendmail.Enabled = True
        Else
            GridHead.Visible = True
            FormatGridColumns(GridView, False, False, False, False)
            GridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            GridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            GridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            With GridView
                If ChkwithDisc.Checked = False Then
                    If .Columns.Contains("DISCOUNT") Then
                        .Columns("DISCOUNT").Visible = False
                    End If
                    If .Columns.Contains("DISCALLOWAMT") Then
                        .Columns("DISCALLOWAMT").Visible = False
                    End If
                Else
                    If .Columns.Contains("DISCOUNT") Then
                        .Columns("DISCOUNT").Visible = True
                    End If
                    If .Columns.Contains("DISCALLOWAMT") Then
                        .Columns("DISCALLOWAMT").Visible = True
                        .Columns("DISCALLOWAMT").HeaderText = "DISCALLOW"
                    End If
                End If
                .Columns("BILLNO").Width = 75
                .Columns("BILLNO").ReadOnly = True
                .Columns("DATE").Width = 100
                .Columns("DATE").ReadOnly = True
                .Columns("TIME").Width = 100
                .Columns("TIME").ReadOnly = True
                .Columns("USERNAME").Width = 150
                .Columns("USERNAME").ReadOnly = True
                .Columns("TYPEOFOTP").Width = 100
                .Columns("TYPEOFOTP").ReadOnly = True
                .Columns("TOCOSTID").Width = 100
                .Columns("TOCOSTID").ReadOnly = True
                .Columns("REMARKS").Width = 250
                .Columns("REMARKS").ReadOnly = True
                .Columns("CLOSEOTPDATEANDTIME").Width = 200
                .Columns("CLOSEOTPDATEANDTIME").ReadOnly = True
                .Columns("STATUS").Width = 100
                .Columns("STATUS").ReadOnly = True
                .Columns("ID").Visible = False
                If GridView.Columns.Contains("DATE") Then GridView.Columns("DATE").DefaultCellStyle.Format = "dd-MM-yyyy"
                If GridView.Columns.Contains("TIME") Then GridView.Columns("TIME").DefaultCellStyle.Format = "hh:mm:ss tt"
                If GridView.Columns.Contains("CLOSEOTPDATEANDTIME") Then GridView.Columns("CLOSEOTPDATEANDTIME").DefaultCellStyle.Format = "dd-MM-yyyy"
                btnsendmail.Enabled = True
            End With

            FormatGridColumns(GridView, False, False, False, False)
            GridView.Focus()
        End If
    End Sub

    Function funcColWidth() As Integer
        With GridHead

            '.Columns("DATE~TIME~USERNAME~TYPEOFOTP").Visible = GridHead.Columns("CORPUSERDETAILS").Visible = True
            If ChkwithDisc.Checked Then
                .Columns("BILLNO~DATE~TIME~USERNAME~TYPEOFOTP~DISCALLOWAMT~DISCOUNT").Width = _
                IIf(GridView.Columns("BILLNO").Visible, GridView.Columns("BILLNO").Width, 0) _
                + IIf(GridView.Columns("DATE").Visible, GridView.Columns("DATE").Width, 0) _
                + IIf(GridView.Columns("TIME").Visible, GridView.Columns("TIME").Width, 0) _
                + IIf(GridView.Columns("USERNAME").Visible, GridView.Columns("USERNAME").Width, 0) _
                + IIf(GridView.Columns("TYPEOFOTP").Visible, GridView.Columns("TYPEOFOTP").Width, 0) _
                + IIf(GridView.Columns("DISCOUNT").Visible, GridView.Columns("DISCOUNT").Width, 0) _
                + IIf(GridView.Columns("DISCALLOWAMT").Visible, GridView.Columns("DISCALLOWAMT").Width, 0)
                .Columns("BILLNO~DATE~TIME~USERNAME~TYPEOFOTP~DISCALLOWAMT~DISCOUNT").HeaderText = "CORPUSERDETAILS"
            Else
                .Columns("BILLNO~DATE~TIME~USERNAME~TYPEOFOTP").Width = _
                IIf(GridView.Columns("BILLNO").Visible, GridView.Columns("BILLNO").Width, 0) _
                + IIf(GridView.Columns("DATE").Visible, GridView.Columns("DATE").Width, 0) _
                + IIf(GridView.Columns("TIME").Visible, GridView.Columns("TIME").Width, 0) _
                + IIf(GridView.Columns("USERNAME").Visible, GridView.Columns("USERNAME").Width, 0) _
                + IIf(GridView.Columns("TYPEOFOTP").Visible, GridView.Columns("TYPEOFOTP").Width, 0)
                .Columns("BILLNO~DATE~TIME~USERNAME~TYPEOFOTP").HeaderText = "CORPUSERDETAILS"
            End If

            '.Columns("TOCOSTID~BUSERNAME~REMARKS~CLOSEOTPDATEANDTIME~STATUS").Visible = GridHead.Columns("BRANCHUSERDETAILS").Visible = True
            .Columns("TOCOSTID~BUSERNAME~REMARKS~CLOSEOTPDATEANDTIME~STATUS").Width = _
            IIf(GridView.Columns("TOCOSTID").Visible, GridView.Columns("TOCOSTID").Width, 0) _
            + IIf(GridView.Columns("BUSERNAME").Visible, GridView.Columns("BUSERNAME").Width, 0) _
            + IIf(GridView.Columns("REMARKS").Visible, GridView.Columns("REMARKS").Width, 0) _
            + IIf(GridView.Columns("CLOSEOTPDATEANDTIME").Visible, GridView.Columns("CLOSEOTPDATEANDTIME").Width, 0) _
             + IIf(GridView.Columns("STATUS").Visible, GridView.Columns("STATUS").Width, 0)
            .Columns("TOCOSTID~BUSERNAME~REMARKS~CLOSEOTPDATEANDTIME~STATUS").HeaderText = "BRANCHUSERDETAILS"
        End With
    End Function

    'Function HeadGrid() As String
    '    Dim dtHead As DataTable
    '    dtHead = GET_HEADER()
    '    With GridHead

    '        .DataSource = dtHead

    '        With .Columns("CORPUSERDETAILS")
    '            .Width = OptionsGrid.Columns("DATE").Width + OptionsGrid.Columns("TIME").Width + OptionsGrid.Columns("USERNAME").Width + OptionsGrid.Columns("TYPEOFOTP").Width
    '            .HeaderText = "CORPUSERDETAILS"
    '            GridHead.Columns("CORPUSERDETAILS").Width = 450
    '            GridHead.Columns("CORPUSERDETAILS").ReadOnly = True

    '        End With

    '        With .Columns("BRANCHUSERDETAILS")
    '            .Width = OptionsGrid.Columns("TOCOSTID").Width + OptionsGrid.Columns("BUSERNAME").Width + OptionsGrid.Columns("REMARKS").Width + OptionsGrid.Columns("CLOSEOTPDATEANDTIME").Width + OptionsGrid.Columns("STATUS").Width
    '            .HeaderText = "BRANCHUSERDETAILS"
    '            GridHead.Columns("BRANCHUSERDETAILS").Width = 650
    '            GridHead.Columns("BRANCHUSERDETAILS").ReadOnly = True
    '        End With

    '    End With

    'End Function
    'Public Function GET_HEADER()

    '    Dim strsql As String

    '    strsql = "select 'DATE-TIME-USERNAME-TYPEOFOTP'CORPUSERDETAILS,'TOCOSTID-BUSERNAME-REMARKS-CLOSEOTPDATEANDTIME-STATUS 'BRANCHUSERDETAILS"
    '    Dim dthead As New DataTable
    '    da = New OleDbDataAdapter(strsql, cn)
    '    da.Fill(dthead)
    '    Return dthead

    'End Function
    Private Sub Heading()
        If chkDate.Checked = True Then
            til = Me.Text & "  As OnDate :" & Me.dtpEditDate.Value.ToString("dd-MM-yyyy")
        Else
            til = Me.Text & " Date : " & Me.dtpEditDate.Value.ToString("dd-MM-yyyy")
            til += " To " & Me.dtpTo.Value.ToString("dd-MM-yyyy")
        End If
        til += " At :" & chkCmbCostCentre.Text
        Me.lblTitle.Font = New System.Drawing.Font("VERDANA", 9, FontStyle.Bold)
        Me.lblTitle.Text = til
    End Sub
    Private Sub btnview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnview.Click
        FuncView()
    End Sub
    Private Sub btnnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew.Click
        FuncNew()
    End Sub
    Private Sub GridColor()
        For Each dgvRow As DataGridViewRow In GridView.Rows
            Select Case dgvRow.Cells("RESULT").Value.ToString
                Case 1
                    dgvRow.DefaultCellStyle.ForeColor = Color.Blue
                    dgvRow.DefaultCellStyle.Font = reportHeadStyle1.Font
            End Select
        Next
    End Sub
    Private Sub frmExceptionReport_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.F3
                btnnew_Click(btnnew, Nothing)
                Exit Select
            Case Keys.F12
                btnexit_Click(btnexit, Nothing)
                Exit Select
            Case Keys.V
                btnview_Click(btnview, Nothing)
                Exit Select
            Case Keys.X
                btnExcel_Click(btnExport, Nothing)
                Exit Select
            Case Keys.P
                btnPrint_Click(btnPrint, Nothing)
                Exit Select
            Case Keys.M
                btnsendmail_Click(btnsendmail, Nothing)
                Exit Select
            Case e.KeyCode = Keys.Enter
                SendKeys.Send("{TAB}")
                Exit Select
        End Select
    End Sub
    Private Sub btnexit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Me.Close()
    End Sub
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If GridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Exception Report-" & title, GridView, BrightPosting.GExport.GExportType.Export, GridHead)
        End If
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
                    If NEWMAILSEND(ToMail(i), GenerateHTMLreport(), Application.StartupPath & "\EXEMPTIONREPORT.PDF") = 0 Then
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
            mail.Subject = "Exception Report "
            mail.Body = MESSAGE
            mail.IsBodyHtml = True
            smtpServer.Credentials = New Net.NetworkCredential(FromId.Trim.ToString, Password.Trim.ToString)
            smtpServer.Timeout = 400000
            smtpServer.Send(mail)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return 0
        End Try
        Return 1
    End Function

    Function GenerateHTMLreport() As String
        Dim hdt As New DataTable
        hdt = CType(GridView.DataSource, DataTable)
        If hdt.Rows.Count > 0 Then
            Dim HM As String
            HM = " <TABLE cellspacing=""0"" cellpadding=""4"" border=""1"" align=""center"" WIDTH=100% border-style:""thin solid"">"
            HM += "<TR>"
            HM += "<TH FONT SIZE=""2"" BGCOLOR=""#C2C67E"" ><FONT COLOR=""660000""><H3>EXEMPTION REPORT " & vbCrLf & dtpEditDate.Value.ToString("dd/MM/yyyy") & "</H3>"
            HM += "</TH>"
            HM += "</TR>"
            HM += "</TABLE>"
            HM += "<TABLE cellspacing=""0"" cellpadding=""4"" border=""1"" align=""center"" WIDTH=100% border-style:""thin solid"">"
            HM += "<TR>"
            HM += "<TH FONT SIZE=""2"" BGCOLOR=""#C2C67E""><FONT COLOR=""660000""><H5>DESCRIPTION</H5>"
            HM += "<TH FONT SIZE=""2"" BGCOLOR=""#C2C67E""><FONT COLOR=""660000""><H5>OPTIONNAME</H5>" '--
            HM += "<TH FONT SIZE=""2"" BGCOLOR=""#C2C67E""><FONT COLOR=""660000""><H5>USERNAME</H5>" '--
            HM += "<TH FONT SIZE=""2"" BGCOLOR=""#C2C67E""><FONT COLOR=""660000""><H5>EXEMPDATE</H5>"
            HM += "<TH FONT SIZE=""2"" BGCOLOR=""#C2C67E""><FONT COLOR=""660000""><H5>EXEMPTIME</H5>" '--
            HM += "</TH>"
            HM += "</TR>"
            Dim _OPTIONNAME As String = ""
            Dim _DESCRIPTION As String = ""
            Dim _USERNAME As String = ""
            Dim _EXEMPDATE As String = ""
            Dim _EXEMPTIME As String = ""
            Dim I As Integer = 0
            While I <> hdt.Rows.Count
                _DESCRIPTION = hdt.Rows(I)("DESCRIPTION").ToString
                _OPTIONNAME = hdt.Rows(I)("OPTIONNAME").ToString
                _USERNAME = hdt.Rows(I)("USERNAME").ToString
                _EXEMPDATE = hdt.Rows(I)("EXEMPDATE").ToString
                _EXEMPTIME = hdt.Rows(I)("EXEMPTIME").ToString

                HM += "<TR >"
                HM += "<TD> " + _DESCRIPTION + "</TD>"
                HM += "<TD> " & _OPTIONNAME & "</TD>"
                HM += "<TD> " & _USERNAME & "</TD>"
                HM += "<TD> " & _EXEMPDATE & "</TD>"
                HM += "<TD> " & _EXEMPTIME & "</TD>"
                HM += "</TR>" + vbCrLf
                I = I + 1
            End While
            HM += "</TABLE>"
            Return HM
        End If
    End Function

    Private Sub chkDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        With Me
            If chkDate.Checked = True Then
                .chkDate.Text = "&As OnDate"
                .lblEditDate.Visible = False
                .dtpTo.Visible = False
            Else
                .chkDate.Text = "&Date From"
                .lblEditDate.Visible = True
                .dtpTo.Visible = True
            End If
        End With
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If GridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, GridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        'If GridHead.RowCount > 0 Then
        '    If AutoResizeToolStripMenuItem.Checked Then
        '        GridHead.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        '        GridHead.Invalidate()
        '        For Each dgvCol As DataGridViewColumn In GridHead.Columns
        '            dgvCol.Width = dgvCol.Width
        '        Next
        '        GridHead.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        '    Else
        '        For Each dgvCol As DataGridViewColumn In GridHead.Columns
        '            dgvCol.Width = dgvCol.Width
        '        Next
        '        GridHead.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        '    End If
        'End If
        If GridView.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                GridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In GridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In GridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
        GridViewHeaderStyle()
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmExemptionReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmExemptionReport_Properties))
        chkDate.Checked = obj.p_chkDate
        rbtFormat2.Checked = obj.p_RbtFormat2
        rbtFormat1.Checked = obj.p_RbtFormat1
        cmbOptions.Text = obj.p_cmbOptions
        ChkwithDisc.Checked = obj.p_chkWithDisc
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmExemptionReport_Properties
        obj.p_chkDate = chkDate.Checked
        obj.p_RbtFormat2 = rbtFormat2.Checked
        obj.p_RbtFormat1 = rbtFormat1.Checked
        obj.p_cmbOptions = cmbOptions.Text
        obj.p_chkWithDisc = ChkwithDisc.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmExemptionReport_Properties))
    End Sub

    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles GridView.KeyPress
        If UCase(e.KeyChar) = Chr(Keys.Escape) Then
            dtpEditDate.Focus()
        End If
    End Sub
End Class

Public Class frmExemptionReport_Properties
    Private chkDate As Boolean = True
    Public Property p_chkDate() As Boolean
        Get
            Return chkDate
        End Get
        Set(ByVal value As Boolean)
            chkDate = value
        End Set
    End Property
    Private RbtFormat2 As Boolean = True
    Public Property p_RbtFormat2() As Boolean
        Get
            Return RbtFormat2
        End Get
        Set(ByVal value As Boolean)
            RbtFormat2 = value
        End Set
    End Property

    Private RbtFormat1 As Boolean = True
    Public Property p_RbtFormat1() As Boolean
        Get
            Return RbtFormat1
        End Get
        Set(ByVal value As Boolean)
            RbtFormat1 = value
        End Set
    End Property
    Private cmbOptions As String = "ALL"
    Public Property p_cmbOptions() As String
        Get
            Return cmbOptions
        End Get
        Set(ByVal value As String)
            cmbOptions = value
        End Set
    End Property
    Private chkWithDisc As Boolean = False
    Public Property p_chkWithDisc() As Boolean
        Get
            Return chkWithDisc
        End Get
        Set(ByVal value As Boolean)
            chkWithDisc = value
        End Set
    End Property

End Class