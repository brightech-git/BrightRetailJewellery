Imports System.IO
Imports System.Data.OleDb
Imports System.Text.RegularExpressions

Public Class frmSendSms
    Dim Strsql As String
    Dim strConn As String
    Dim da As New OleDbDataAdapter
    Dim dt As New DataTable
    Dim Cmd As New OleDbCommand
    Dim SmsVia As String = "Web"
    Dim SmsFlag As Boolean = False
    Dim SMSURL As String
    Public SMSDATEFORMAT As String
    Dim FTPID As String = GetAdmindbSoftValue("FTPID")
    Dim FTPUSER As String = GetAdmindbSoftValue("FTPUSER")
    Dim FTPPWD As String = GetAdmindbSoftValue("FTPPWD")
    Dim SMS_MSG_RATE As String = objGPack.GetSqlValue("SELECT ISNULL(TEMPLATE_DESC,'') AS TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='SMS_MSG_RATE' AND ISNULL(ACTIVE,'Y')<>'N'", "TEMPLATE_DESC").ToString

    Private Sub funcLoadTempalteDesc()
        If SMS_MSG_RATE <> "" Then
            txtMsg_OWN.Text = SMS_MSG_RATE
        End If
    End Sub

    Private Sub LinkClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkClear.Click
        lstToMobileNo_OWN.Items.Clear()
        txtMsg_OWN.Clear()
        lblMobileCnt.Text = "Total No's:0"
        SmsFlag = False
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        My.Settings.WebBased = rbtWebBased.Checked
        'My.Settings.SenderId = txtSenderId_OWN.Text
        Me.Close()
    End Sub
    Function funcDbChecker(ByVal dbname As String) As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        Strsql = " SELECT NAME FROM MASTER..SYSDATABASES WHERE NAME = '" & dbname & "'"
        da = New OleDbDataAdapter(Strsql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return 1
        End If
        Return 0
    End Function
    Function funcTableChecker(ByVal dbname As String, ByVal TblName As String) As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        Strsql = " SELECT NAME FROM " & dbname & "..SYSOBJECTS WHERE NAME = '" & TblName & "' AND XTYPE='U'"
        da = New OleDbDataAdapter(Strsql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return 1
        End If
        Return 0
    End Function
    Private Sub funcInsertAkshayaSmsTable()
        For I As Integer = 0 To lstToMobileNo_OWN.Items.Count - 1
            Dim str(2) As String
            str = lstToMobileNo_OWN.Items(I).ToString().Split(":")
            If str.Length > 1 Then
                Strsql = "INSERT INTO AKSHAYASMSDB..SMSDATA(MOBILENO,MESSAGES,STATUS,UPDATED)VALUES('" & str(0) & "',N'" & IIf(str(1) = "", "", str(1) & " ") & txtMsg_OWN.Text & "','','" & Date.Now & "') "
            Else
                Strsql = "INSERT INTO AKSHAYASMSDB..SMSDATA(MOBILENO,MESSAGES,STATUS,UPDATED)VALUES('" & str(0) & "',N'" & txtMsg_OWN.Text & "','','" & Date.Now & "') "
            End If
            Cmd = New OleDbCommand(Strsql, cn)
            Cmd.ExecuteNonQuery()
            SmsFlag = True
        Next
        SmsVia = "AkshayaSms"
    End Sub
    Private Sub funcSendSmsViaWeb()
        Dim Status As String
        For I As Integer = 0 To lstToMobileNo_OWN.Items.Count - 1
            Dim str(2) As String
            str = lstToMobileNo_OWN.Items(I).ToString().Split(":")
            If str.Length > 1 Then
                Status = SENDURLSMS(str(0), IIf(str(1) = "", "", str(1) & "  ") & txtMsg_OWN.Text)
            Else
                Status = SENDURLSMS(str(0), txtMsg_OWN.Text)
            End If
            If Status = "Invalid Sender ID" Then
                MsgBox("Invalid Sender ID", MsgBoxStyle.Information, "Sms") : Exit Sub
            End If
            SmsFlag = True
        Next
        SmsVia = "Web"
    End Sub
    Public Function SENDURLSMS(ByVal mobno As String, ByVal msg As String) As String
        Dim address As String = SMSURL
        address = address.Replace("<SMSDATEFORMAT>", DateTime.Now.ToString(SMSDATEFORMAT))
        address = address.Replace("<SMSTO>", mobno).Replace("<SMSMSG>", msg)
        If address.Contains("<") = False And address.Contains(">") = False Then
            Return CALLURL(address)
        Else
            Return "Fail"
        End If
    End Function

    Public Function CALLURL(ByVal address As String) As String
        Try
            Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(address)
            Dim resp As System.Net.WebResponse = req.GetResponse()
            Dim s As System.IO.Stream = resp.GetResponseStream()
            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(s, System.Text.Encoding.ASCII)
            Dim info As String = sr.ReadToEnd()
            Return info
        Catch exc As Exception
            Return "Fail"
        End Try
    End Function
    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        If lstToMobileNo_OWN.Items.Count = 0 Then MsgBox("To Mobile No's Empty.", MsgBoxStyle.Information) : Exit Sub
        If (txtMsg_OWN.Text).Length = 0 Then MsgBox("Message Empty.", MsgBoxStyle.Information) : txtMsg_OWN.Focus() : Exit Sub
        SMSURL = GETSMSURL(IIf(rbtGeneral.Checked, True, False), True)
        If SMSURL = "Not Register" Then Exit Sub
        If rbtSmsTable.Checked Then
            If funcDbChecker("AKSHAYASMSDB") = 0 Then MsgBox("BrighttechSms Database Not found for Send Sms.", MsgBoxStyle.Information) : Exit Sub
            If funcTableChecker("AKSHAYASMSDB", "SMSDATA") = 0 Then MsgBox("SmsData Table Not found for Send Sms.", MsgBoxStyle.Information) : Exit Sub
            funcInsertAkshayaSmsTable()
        Else
            If SMSURL Is Nothing Or SMSURL = "" Then MsgBox("SMSURL Not Set in Softcontrol.", MsgBoxStyle.Information) : Exit Sub
            funcSendSmsViaWeb()
        End If
        If SmsVia = "Web" And SmsFlag Then
            MsgBox("Sms Sent", MsgBoxStyle.Information)
        ElseIf SmsVia = "AkshayaSms" And SmsFlag Then
            MsgBox("Sms Generated,Please run Alert Sms Application!", MsgBoxStyle.Information)
        End If
    End Sub
    Public Function GETSMSURL(ByVal Gen As Boolean, Optional ByVal OnlineChk As Boolean = False) As String
        Try
            Dim address As String = Nothing
            Dim temp, tosplit, rep As String
            Dim param As String()
            Dim i As Integer = 0
            If Gen Then
                Strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='SMSURL'"
            Else
                Strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='SMSURLPROMO'"
            End If
            address = objGPack.GetSqlValue(Strsql, "CTLTEXT", "")
            If address <> "" Then
                Dim SenderId As String = ""
                If Gen Then
                    SenderId = Mid(address, InStr(address.ToUpper, "SENDER=", CompareMethod.Text) + 7, 6)
                Else
                    SenderId = Mid(address, InStr(address.ToUpper, "SENDER=", CompareMethod.Text) + 7, 7)
                    SenderId = SenderId.Replace("&", "").Trim
                End If
                txtSenderId_OWN.Text = SenderId.ToUpper
                txtSenderId_OWN.Enabled = False
                If OnlineChk And rbtWebBased.Checked Then
                    Dim OnCon As New OleDbConnection
                    OnCon = New OleDbConnection("Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & FTPUSER & ";Initial Catalog=giritech1;Data Source=" & FTPID & ";password=" & FTPPWD)
                    Try
                        If OnCon.State = ConnectionState.Closed Then OnCon.Open()
                        Strsql = "SELECT COUNT(*)CNT FROM CLIENTSMS WHERE SENDERID='" & SenderId & "' AND ISNULL(" & IIf(Gen, "TRAN", "PROMO") & "SMS,'Y')<>'N'"
                        Dim da As New OleDbDataAdapter(Strsql, OnCon)
                        Dim dt As New DataTable
                        da.Fill(dt)
                        If OnCon.State = ConnectionState.Open Then OnCon.Close()
                        If Val(dt.Rows(0).Item("CNT").ToString) = 0 Then
                            MsgBox("Sender Id " & SenderId & " Not Registered " & vbCrLf & vbCrLf & "Contact Administrator", MsgBoxStyle.Information, "Giritech Server Says!!")
                            Return "Not Register"
                            Exit Function
                        End If
                    Catch ex As Exception
                        MsgBox("Server Not Connected " & vbCrLf & vbCrLf & "Contact Administrator", MsgBoxStyle.Information, "Giritech Server Says!!")
                        Return "Not Register"
                        Exit Function
                    End Try
                End If
                If Not Gen Then
                    address = address.Replace(SenderId, "BULKSMS")
                End If
                temp = address
                tosplit = Nothing
                While temp.Contains("<") = True
                    tosplit += temp.Substring(temp.IndexOf("<") + 1, temp.IndexOf(">") - temp.IndexOf("<") - 1) + ","
                    If temp.Length > temp.IndexOf(">") + 1 Then
                        temp = temp.Substring(temp.IndexOf(">") + 1)
                    Else
                        temp = ""
                    End If
                End While
                tosplit = tosplit.Substring(0, tosplit.Length - 1)
                param = tosplit.Split(",")
                For Each str As String In param
                    If str <> "" Then
                        Strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='" + str.ToUpper + "'"
                        rep = objGPack.GetSqlValue(Strsql, "CTLTEXT", "")
                        If rep <> "" And str.ToUpper <> "SMSDATEFORMAT" Then
                            address = address.Replace("<" + str + ">", rep)
                        ElseIf rep <> "" And str.ToUpper = "SMSDATEFORMAT" Then
                            SMSDATEFORMAT = rep
                        End If
                    End If
                Next
                Return (address)
            Else
                Return ""
            End If
        Catch exc As Exception
            ' MessageBox.Show("Incorrect SMSURL Format" + vbCrLf + exc.ToString(), "Exception Caught")
        End Try
    End Function

    Private Sub txtMsg_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMsg_OWN.TextChanged
        lblSmsCount.Text = Len(txtMsg_OWN.Text) & " Character(s)," & IIf(Convert.ToInt32(Len(txtMsg_OWN.Text) / 160) = 0, 1, Math.Ceiling(Len(txtMsg_OWN.Text) / 160)) & " SMS"
    End Sub

    Private Sub LinkImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkImport.Click
        lstToMobileNo_OWN.MultiColumn = False
        Dim openDia As New OpenFileDialog
        Dim dsExceelDatas As New DataTable
        Strsql = "All Excel Files|*.xls;*.xlsx|Excel 97-2003 WorkBook|*.xls|Excel WorkBook 2007|*.xlsx|All Files|*.*"
        openDia.Filter = Strsql
        If openDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.Refresh()
            Dim myExcel As Excel.Application
            Dim myWorkBook As Excel.Workbook
            Dim tworksheet As Excel.Worksheet
            myExcel = CreateObject("Excel.Application")
            myWorkBook = myExcel.Workbooks.Open(openDia.FileName)
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & openDia.FileName & ";Extended Properties=""Excel 8.0;"""
            Strsql = "SELECT * FROM [SHEET1$]"
            da = New OleDbDataAdapter(Strsql, strConn)
            da.Fill(dsExceelDatas)
            For I As Integer = 0 To dsExceelDatas.Rows.Count - 1
                If lstToMobileNo_OWN.Items.Contains(dsExceelDatas.Rows(I).Item(0)) Then Continue For
                lstToMobileNo_OWN.Items.Add(dsExceelDatas.Rows(I).Item(0))
            Next
            lstToMobileNo_TextChanged(Me, New EventArgs)
            CmbTemplate.Focus()
            myWorkBook.Close()
            myExcel.Quit()
            releaseObject(tworksheet)
            releaseObject(myWorkBook)
            releaseObject(myExcel)
        End If
    End Sub

    Private Sub LinkCustInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkCustInfo.Click
        Dim chitdblink As String = GetAdmindbSoftValue("CHITDB", "N").ToString
        Dim chitdbprefix As String = GetAdmindbSoftValue("CHITDBPREFIX", "").ToString
        Dim chitMaindb As String = ""
        If chitdbprefix <> "" Then
            chitMaindb = chitdbprefix + "SAVINGS"
        End If
        Strsql = "SELECT DISTINCT MOBILE FROM " & cnAdminDb & "..PERSONALINFO "
        Strsql += " UNION "
        Strsql += " SELECT DISTINCT MOBILE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE='C' "
        If chitMaindb <> "" Then
            Strsql += " UNION "
            Strsql += " SELECT DISTINCT MOBILE FROM " & chitMaindb & "..PERSONALINFO "
        End If
        da = New OleDbDataAdapter(Strsql, cn)
        Dim dtCustInfo As New DataTable
        da.Fill(dtCustInfo)
        If dtCustInfo.Rows.Count > 0 Then
            For I As Integer = 0 To dtCustInfo.Rows.Count - 1
                If lstToMobileNo_OWN.Items.Contains(dtCustInfo.Rows(I).Item("MOBILE")) Then Continue For
                lstToMobileNo_OWN.Items.Add(dtCustInfo.Rows(I).Item("MOBILE"))
            Next
            lstToMobileNo_TextChanged(Me, New EventArgs)
            CmbTemplate.Focus()
        End If
    End Sub

    Private Sub LinkImportName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkImportName.Click
        lstToMobileNo_OWN.MultiColumn = True
        Dim openDia As New OpenFileDialog
        Dim dsExceelDatas As New DataTable
        Strsql = "All Excel Files|*.xls;*.xlsx|Excel 97-2003 WorkBook|*.xls|Excel WorkBook 2007|*.xlsx|All Files|*.*"
        openDia.Filter = Strsql
        If openDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.Refresh()
            Dim myExcel As Excel.Application
            Dim myWorkBook As Excel.Workbook
            Dim tworksheet As Excel.Worksheet
            myExcel = CreateObject("Excel.Application")
            myWorkBook = myExcel.Workbooks.Open(openDia.FileName)
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & openDia.FileName & ";Extended Properties=""Excel 8.0;"""
            Strsql = "SELECT * FROM [SHEET1$]"
            da = New OleDbDataAdapter(Strsql, strConn)
            da.Fill(dsExceelDatas)
            For I As Integer = 0 To dsExceelDatas.Rows.Count - 1
                If lstToMobileNo_OWN.Items.Contains(dsExceelDatas.Rows(I).Item(0) & ":" & dsExceelDatas.Rows(I).Item(1)) Then Continue For
                lstToMobileNo_OWN.Items.Add(dsExceelDatas.Rows(I).Item(0) & ":" & dsExceelDatas.Rows(I).Item(1))
            Next
            lstToMobileNo_TextChanged(Me, New EventArgs)
            CmbTemplate.Focus()
            myWorkBook.Close()
            myExcel.Quit()
            releaseObject(tworksheet)
            releaseObject(myWorkBook)
            releaseObject(myExcel)
        End If
    End Sub

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    Private Sub lstToMobileNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstToMobileNo_OWN.TextChanged, lstToMobileNo_OWN.Leave
        lblMobileCnt.Text = "Total No's:" & lstToMobileNo_OWN.Items.Count
    End Sub

    Private Sub frmSendSms_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SMSURL = GETSMSURL(True)
        rbtWebBased.Checked = My.Settings.WebBased
        rbtSmsTable.Checked = Not My.Settings.WebBased
        If FTPID = "" Then FTPID = "108.170.45.170"
        If FTPUSER = "" Then FTPUSER = "w{†I"
        If FTPPWD = "" Then FTPPWD = "qtwz}€"
        If FTPUSER <> "" Then FTPUSER = BrighttechPack.Methods.Decrypt(FTPUSER)
        If FTPPWD <> "" Then FTPPWD = BrighttechPack.Methods.Decrypt(FTPPWD)
        If FTPID <> "" Then FTPID = BrighttechPack.Methods.Decrypt(FTPID)
        'If Not My.Settings.SenderId Is Nothing Then txtSenderId_OWN.Text = My.Settings.SenderId
    End Sub

    Private Sub rbtPromo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtPromo.CheckedChanged
        If rbtPromo.Checked Then
            rbtWebBased.Checked = True
        End If
    End Sub

    Private Sub CmbTemplate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbTemplate.KeyDown
        If e.KeyCode = Keys.Enter And CmbTemplate.Text <> "" Then
            funcLoadTempalteDesc()
        End If
    End Sub

    Private Sub CmbTemplate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbTemplate.SelectedIndexChanged
        funcLoadTempalteDesc()
    End Sub
End Class