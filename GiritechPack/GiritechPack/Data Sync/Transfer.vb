Imports System.IO
Imports System.Net.Mail
Imports System.Data.OleDb
Imports Microsoft.Office.Interop

''SPLIT FILES CODE
'If fInfo.Length > 4194304 Then ''If file length goes to greater than 4MB
'    Dim _spDir As String = _RootDir & "\" & _tblName
'    If Not IO.Directory.Exists(_spDir) Then IO.Directory.CreateDirectory(_spDir)
'    Dim _FileSplitMerge As New SplitMerge()
'    With _FileSplitMerge
'        .ChunkSize = 4194304
'        .FileName = zipFile
'        .DeleteFileAfterSplit = True
'        .OutputPath = _spDir
'        If .SplitFile() Then
'            For Each _filpath As String In IO.Directory.GetFiles(_spDir)
'                _mailSendStatus = SendMail.Send(_toUserId, "SPLIT" & _tblName, "SEND SPLIT DATA", _filpath, _sendUserId, _sendPwd, _tblName, True)
'                If Not _mailSendStatus Then Exit For
'            Next
'            If IO.Directory.Exists(_spDir) Then IO.Directory.Delete(_spDir, True)
'        End If
'    End With
'Else
'    _mailSendStatus = SendMail.Send(_toUserId, _tblName, "SEND DATA", zipFile, _sendUserId, _sendPwd, _tblName, True)
'End If


Public Class Transfer
    Dim _StrSql As String
    Dim _Da As OleDbDataAdapter
    Dim _Cmd As OleDbCommand
    Private WithEvents pBar As ToolStripProgressBar
    Private WithEvents tStripBarStatus As ToolStripStatusLabel
    Private WithEvents frm As Form


    Public Enum Mode
        Mail = 0
        Ftp = 1
    End Enum

    Public Sub New(ByRef progBar As ToolStripProgressBar, ByRef statusLabel As ToolStripStatusLabel, ByRef f As Form)
        frm = f
        pBar = progBar
        tStripBarStatus = statusLabel
    End Sub

    Public Sub ProgressBarShow(Optional ByVal progressStyle As ProgressBarStyle = ProgressBarStyle.Blocks)
        tStripBarStatus.Text = ""
        pBar.Style = progressStyle
        pBar.Value = 0
        pBar.Maximum = 100
        pBar.Step = 5
        pBar.Visible = True
        frm.Refresh()
    End Sub
    Public Sub ProgressBarStep(Optional ByVal statusComment As String = Nothing, Optional ByVal stepValue As Integer = 5)
        If pBar.Value >= pBar.Maximum - stepValue Then
            pBar.Value = 0
        Else
            pBar.Value = pBar.Value + stepValue
        End If
        tStripBarStatus.Text = statusComment + IIf(statusComment <> Nothing, "....     ", "")
        frm.Refresh()
    End Sub
    Public Sub ProgressBarHide()
        pBar.Value = pBar.Maximum
        frm.Refresh()
        'Main.Refresh()
        pBar.Visible = False
        tStripBarStatus.Text = ""
        frm.Refresh()
        'Main.Refresh()
    End Sub

    Public Shared Function GetSqlValue(ByVal qry As String, ByVal cn As OleDbConnection, Optional ByVal defValue As String = "", Optional ByVal tran As OleDbTransaction = Nothing) As String
        Dim da As OleDbDataAdapter
        Dim dtTemp As New DataTable
        Dim cmd As OleDbCommand
        cmd = New OleDbCommand(qry, cn)
        If tran IsNot Nothing Then cmd.Transaction = tran
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            Return dtTemp.Rows(0).Item(0).ToString
        End If
        Return defValue
    End Function
    

    Public Sub Send(ByVal _frmCostId As String, ByVal _toCostId As List(Of String), ByVal _cnAdmindb As String, ByVal _cnStockDb As String, ByVal _lCn As OleDbConnection, Optional ByVal ReplaceDbs As List(Of String) = Nothing)
        Dim strSql As String
        Dim fInfo As IO.FileInfo = Nothing
        Dim _RootDir As String = Application.StartupPath & "\UploadXml"
        Try
            Dim sepDb As String = BrighttechPack.GlobalMethods.GetSqlValue(_lCn, "SELECT CTLTEXT FROM " & _cnAdmindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-SEPDATADB'", , "N")
            Dim Syncdb As String = _cnAdmindb
            Dim uprefix As String = Mid(_cnAdmindb, 1, 3)
            If sepDb = "Y" Then Dim usuffix As String = "UTILDB" : Syncdb = uprefix + usuffix

            ProgressBarShow()
            ProgressBarStep("Receiving..")
            Receive(_frmCostId, _cnAdmindb, _cnStockDb, _lCn, ReplaceDbs)
            strSql = " SELECT DISTINCT TOID FROM " & Syncdb & "..SENDSYNC "
            strSql += " WHERE FROMID = '" & _frmCostId & "' AND STATUS = 'N'"
            strSql += " AND TOID IN ("
            For cnt As Integer = 0 To _toCostId.Count - 1
                strSql += "'" & _toCostId.Item(cnt).ToUpper & "'"
                If cnt <> _toCostId.Count - 1 Then
                    strSql += ","
                End If
            Next
            strSql += ")"
            Dim dtToId As New DataTable
            _Da = New OleDbDataAdapter(strSql, _lCn)
            _Da.Fill(dtToId)
            If Not IO.Directory.Exists(_RootDir) Then IO.Directory.CreateDirectory(_RootDir)
            If Not dtToId.Rows.Count > 0 Then Exit Sub
            Dim objZip As BrighttechPack.Zipper
            Dim _tblName As String = ""
            Dim _sendUserId As String = GetSqlValue(" SELECT EMAILID FROM " & _cnAdmindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & _frmCostId & "'", _lCn)
            Dim _sendPwd As String = Methods.Decrypt(GetSqlValue(" SELECT PASSWORD FROM " & _cnAdmindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & _frmCostId & "'", _lCn))
            Dim _smtpHostName As String = GetSqlValue("SELECT CTLTEXT FROM " & _cnAdmindb & "..SOFTCONTROL WHERE CTLID = 'SMTP-HOSTNAME'", _lCn)
            Dim _smtpPort As Integer = Val(GetSqlValue("SELECT CTLTEXT FROM " & _cnAdmindb & "..SOFTCONTROL WHERE CTLID = 'SMTP-PORT'", _lCn))
            Dim _sendFtpPath As String = GetSqlValue(" SELECT FTPID FROM " & _cnAdmindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & _frmCostId & "'", _lCn)
            Dim _toUserId As String = ""
            Dim _toFtpPath As String = ""
            For Each ro As DataRow In dtToId.Rows
                _tblName = "SYNC-" & _frmCostId & "-" & ro.Item("TOID").ToString & "-" & Today.Date.ToString("ddMMyy") & "-" & Date.Now.ToString("HHssmm")
                _toUserId = GetSqlValue(" SELECT EMAILID FROM " & _cnAdmindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & ro.Item("TOID").ToString & "'", _lCn)
                If _sendPwd = "" Or _smtpHostName = "" Or _smtpPort = 0 Or _toUserId = "" Then Continue For
                strSql = "SELECT * FROM " & Syncdb & "..SENDSYNC"
                strSql += " WHERE FROMID = '" & _frmCostId & "' AND STATUS = 'N'"
                strSql += " AND TOID = '" & ro.Item("TOID").ToString & "'"
                strSql += " ORDER BY UID"
                Dim ds As New DataSet
                _Da = New OleDbDataAdapter(strSql, _lCn)
                _Da.Fill(ds, _tblName)
                If ds.Tables(0).Rows.Count > 0 Then
                    ''Encrypting Qry
                    For Each row As DataRow In ds.Tables(0).Rows
                        row("SQLTEXT") = BrighttechPack.Methods.EncryptXml(row("SQLTEXT").ToString)
                    Next
                    ds.Tables(0).AcceptChanges()

                    ''Creating xml File
                    Dim xmlFile As String = _RootDir & "\" & _tblName & ".xml"
                    Dim fs As New IO.StreamWriter(xmlFile)
                    ds.WriteXml(fs, XmlWriteMode.WriteSchema)
                    fs.Close()

                    ''Zipping
                    Dim zipFile As String = _RootDir & "\" & _tblName & ".zip"
                    objZip = New BrighttechPack.Zipper
                    If Not objZip.Zip(xmlFile, zipFile) Then
                        If IO.File.Exists(xmlFile) Then IO.File.Delete(xmlFile)
                        If IO.File.Exists(zipFile) Then IO.File.Delete(zipFile)
                        Continue For
                    Else
                        IO.File.Delete(xmlFile)
                    End If

                    fInfo = New FileInfo(zipFile)
                    ''Sending Mail
                    Dim _mailSendStatus As Boolean = False
                    _mailSendStatus = SendMail.Send(_smtpHostName, _smtpPort, _toUserId, _tblName, "SEND DATA", zipFile, _sendUserId, _sendPwd, _sendUserId, True)
                    If Not _mailSendStatus Then Continue For
                    ''Updating sended records
                    strSql = " UPDATE " & Syncdb & "..SENDSYNC SET STATUS = 'M',UPDFILE = '" & fInfo.Name & "'"
                    strSql += " WHERE FROMID = '" & _frmCostId & "' AND STATUS = 'N'"
                    strSql += " AND TOID = '" & ro.Item("TOID").ToString & "'"
                    _Cmd = New OleDbCommand(strSql, _lCn)
                    _Cmd.ExecuteNonQuery()
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            ProgressBarHide()
        End Try
    End Sub

    Public Sub Receive(ByVal ReceivedCostId As String, ByVal cnAdmindb As String, ByVal cnStockDb As String, ByVal lCn As OleDbConnection, Optional ByVal ReplaceDbs As List(Of String) = Nothing)
        Dim objOL As New Outlook.Application
        Dim attCount As Integer = 0
        Dim _RootDir As String = Application.StartupPath & "\DownloadAttachments"
        If Not IO.Directory.Exists(_RootDir) Then IO.Directory.CreateDirectory(_RootDir)

        ProgressBarShow()
REFRESH:
        ProgressBarStep("Refreshing Outlook..")

        If OutlookCls.SyncOutlook() Then

        End If
        ProgressBarStep("Intializing..")
        Dim inBox As Outlook.MAPIFolder = objOL.GetNamespace("MAPI").Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox)

        Dim inBoxItems As Outlook.Items = inBox.Items

        Dim newEmail As Outlook.MailItem
        'inBoxItems = inBoxItems.Restrict("[Unread] = true")
        Dim _StrFtr As String = Nothing

        _StrSql = " SELECT EMAILID FROM " & cnAdmindb & "..SYNCCOSTCENTRE "
        _StrSql += " WHERE COSTID <> '" & ReceivedCostId & "'"
        Dim _DtFtr As New DataTable
        _Da = New OleDbDataAdapter(_StrSql, lCn)
        _Da.Fill(_DtFtr)
        If Not _DtFtr.Rows.Count > 0 Then
            ProgressBarStep("No record found in Sync Costcentre..")
            pBar.Visible = False
            Exit Sub
        End If

        For Each ro As DataRow In _DtFtr.Rows
            _StrFtr += "[FROM] = '" & ro!EMAILID.ToString & "' OR"
        Next
        If _StrFtr <> Nothing Then
            If _StrFtr.EndsWith("OR") Then _StrFtr = Trim(_StrFtr.Remove(_StrFtr.Length - 2, 2))
            inBoxItems = inBoxItems.Restrict(_StrFtr)
        End If


        If Not IO.Directory.Exists(_RootDir) Then IO.Directory.CreateDirectory(_RootDir)
        Try
            Dim _SplitFile As String = Nothing
            Dim objZip As BrighttechPack.Zipper
            For Each collectionItem As Object In inBoxItems
                ProgressBarStep("Searching..", 0)
                newEmail = TryCast(collectionItem, Outlook.MailItem)
                If newEmail IsNot Nothing Then
                    ''Validation
                    If newEmail.Subject Is Nothing Then Continue For
                    If Not newEmail.Subject.StartsWith("SYNC-") Then Continue For
                    If Not newEmail.Subject.Contains("-") Then Continue For
                    Dim subject() As String = newEmail.Subject.ToUpper.Split("-")
                    Dim senderId As String = subject(1)
                    Dim ReceivedId As String = ""
                    If subject.Length > 2 Then ReceivedId = subject(2) Else Continue For
                    If Not ReceivedCostId.ToUpper = ReceivedId.ToUpper Then Continue For
                    If Not newEmail.Attachments.Count > 0 Then Continue For

                    ''Attachment Exists
                    For i As Integer = 1 To newEmail.Attachments.Count
                        ProgressBarStep("Downloading..", 0)
                        Dim fileName As String = _RootDir & "\" & (newEmail.Attachments(i).FileName)
                        Dim xmlName As String = _RootDir & "\" & (newEmail.Attachments(i).FileName).Replace(".zip", ".xml")
                        Dim fINfo As New IO.FileInfo(xmlName)
                        newEmail.Attachments(i).SaveAsFile(fileName)
                        objZip = New BrighttechPack.Zipper
                        If Not objZip.UnZip(fileName, fINfo.DirectoryName) Then
                            If IO.File.Exists(fileName) Then IO.File.Delete(fileName)
                            Continue For
                        Else
                            IO.File.Delete(fileName)
                        End If
                        If InsertIntoDb(senderId, ReceivedId, xmlName, cnAdmindb, cnStockDb, lCn, ReplaceDbs) Then
                            newEmail.Delete()
                            attCount += 1
                            GoTo REFRESH
                        End If
                        If IO.File.Exists(xmlName) Then IO.File.Delete(xmlName)
                    Next
                End If
            Next collectionItem
            'MsgBox(attCount & " file(s) Downloaded" & lCn.DataSource)
        Catch ex As Exception
            'MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            ProgressBarStep(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            ProgressBarHide()
        End Try
    End Sub

    Private Function InsertIntoDb(ByVal senderCostId As String, ByVal receiverCostId As String, ByVal fileName As String, ByVal cnADmindb As String, ByVal cnStockDb As String, ByVal lCn As OleDbConnection, Optional ByVal ReplaceDbs As List(Of String) = Nothing) As Boolean
        Dim tran As OleDbTransaction = Nothing
        Dim OrgQry As String = ""
        Dim qry As String = ""
        Try
            Dim sepDb As String = BrighttechPack.GlobalMethods.GetSqlValue(lCn, "SELECT CTLTEXT FROM " & cnADmindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-SEPDATADB'", , "N")
            Dim Syncdb As String = cnADmindb
            Dim uprefix As String = Mid(cnADmindb, 1, 3)
            If sepDb = "Y" Then Dim usuffix As String = "UTILDB" : Syncdb = uprefix + usuffix

            Dim gPack As New Methods(lCn, cnADmindb, "", Color.AliceBlue, Color.AliceBlue, Color.AliceBlue, Color.AliceBlue, Color.AliceBlue)
            Dim fs As New IO.FileStream(fileName, IO.FileMode.Open, IO.FileAccess.Read)
            Dim strSql As String
            Dim ds As New DataSet
            Dim cmd As OleDbCommand
            ds.ReadXml(fs, XmlReadMode.ReadSchema)
            fs.Close()
            Dim Finfo As New FileInfo(fileName)
            strSql = "SELECT 'CHECK' FROM " & Syncdb & "..RECEIVESYNC WHERE UPDFILE = '" & Finfo.Name & "'"
            If gPack.GetSqlValue(strSql).Length > 0 Then
                MsgBox(Finfo.Name & " Already Downloaded", MsgBoxStyle.Information)
                Return False
            End If
            ProgressBarStep("Downloading into Db")
            Dim sendCompId As String = gPack.GetSqlValue("SELECT COMPID FROM " & cnADmindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & senderCostId & "'")
            Dim recvCompId As String = gPack.GetSqlValue("SELECT COMPID FROM " & cnADmindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & receiverCostId & "'")
            tran = lCn.BeginTransaction
            For cnt As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim ro As DataRow = ds.Tables(0).Rows(cnt)
                qry = BrighttechPack.Methods.DecryptXml(ro!SQLTEXT.ToString)
                OrgQry = qry
                Dim tempAdmindb As String = cnADmindb.ToUpper.Replace(recvCompId.ToUpper, sendCompId.ToUpper)
                Dim tempStockdb As String = cnStockDb.ToUpper.Replace(recvCompId.ToUpper, sendCompId.ToUpper)
                qry = qry.ToUpper.Replace(tempAdmindb.ToUpper, cnADmindb.ToUpper)
                qry = qry.ToUpper.Replace(tempStockdb.ToUpper, cnStockDb.ToUpper)
                If Not ReplaceDbs Is Nothing Then
                    Dim vbPrefix As String = gPack.GetSqlValue("SELECT CTLTEXT FROM " & cnADmindb & "..SOFTCONTROL WHERE CTLID = 'VBDBPREFIX'", , , tran)
                    If vbPrefix <> "" Then
                        For Each s As String In ReplaceDbs
                            Dim suffix As String = Mid(s, 4, s.Length)
                            Dim dbIndex As Integer = qry.ToUpper.IndexOf(suffix.ToUpper)
                            If dbIndex > -1 Then
                                Dim replaceDb As String = Mid(qry.ToUpper, dbIndex - 2, suffix.Length + 3)
                                Dim vbDb As String = vbPrefix & suffix
                                qry = qry.ToUpper.Replace(replaceDb.ToUpper, vbDb.ToUpper)
                            End If
                        Next
                    End If
                End If
                ''MsgBox("tempadmindb : " & tempAdmindb + vbCrLf + "tempstockdb : " & tempStockdb + vbCrLf + cnADmindb & " " & cnStockDb)
                strSql = " INSERT INTO " & Syncdb & "..RECEIVESYNC(FROMID,TOID,SQLTEXT"
                If ro!TAGIMAGE.ToString <> "" Then strSql += ",TAGIMAGE,TAGIMAGENAME"
                strSql += " ,UPDFILE)"
                strSql += " VALUES"
                strSql += " (?,?,?"
                If ro!TAGIMAGE.ToString <> "" Then strSql += ",?,?"
                strSql += " ,?)"
                cmd = New OleDbCommand(strSql, lCn, tran)
                cmd.Parameters.AddWithValue("@FROMID", ro!FROMID.ToString)
                cmd.Parameters.AddWithValue("@TOID", ro!TOID.ToString)
                cmd.Parameters.AddWithValue("@SQLTEXT", qry)
                If ro!TAGIMAGE.ToString <> "" Then
                    cmd.Parameters.AddWithValue("@TAGIMAGE", ro!TAGIMAGE)
                    cmd.Parameters.AddWithValue("@TAGIMAGENAME", ro!TAGIMAGENAME)
                End If
                cmd.Parameters.AddWithValue("@UPDFILE", Finfo.Name)
                cmd.ExecuteNonQuery()
                If ro!TAGIMAGE.ToString <> "" Then
                    Dim defDir As String = GetSqlValue("SELECT CTLTEXT FROM " & cnADmindb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'", lCn, , tran)
                    If Not IO.Directory.Exists(defDir) Then
                        If tran.Connection IsNot Nothing Then tran.Rollback()
                        MsgBox(defDir & " not found. Please make appropriate path", MsgBoxStyle.Information)
                        Return False
                    End If
                    Dim myByte() As Byte = ro!TAGIMAGE
                    Dim stream As System.IO.MemoryStream
                    Dim img As Image
                    stream = New System.IO.MemoryStream()
                    stream.Write(myByte, 0, myByte.Length)
                    img = Image.FromStream(stream, True)
                    img.Save(defDir & "\" & ro!TAGIMAGENAME.ToString)
                    stream.Close()
                End If
            Next

            Dim ret As String = Nothing
            ret = "UPDATE " & Syncdb & "..SENDSYNC SET STATUS = 'Y' "
            ret += " ,UPDFILE = '" & Finfo.Name & "'"
            ret += " WHERE FROMID = '" & senderCostId & "'"
            ret += " AND TOID = '" & receiverCostId & "'"
            ret += " AND STATUS = 'M'"
            ret += " AND UID BETWEEN " & Val(ds.Tables(0).Rows(0).Item("UID").ToString) & ""
            ret += " AND " & Val(ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1).Item("UID").ToString) & ""

            qry = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,UPDFILE)"
            qry += " VALUES"
            qry += " ("
            qry += " '" & receiverCostId & "'"
            qry += " ,'" & senderCostId & "'"
            qry += " ,'" & ret.Replace("'", "''") & "'"
            qry += " ,'" & Finfo.Name & "'"
            qry += " )"
            cmd = New OleDbCommand(qry, lCn, tran)
            cmd.ExecuteNonQuery()

            tran.Commit()
            Return True
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            Dim objErr As New ErrorQryDia
            objErr.txtOrginal.Text = OrgQry
            objErr.txtErr.Text = qry
            objErr.ShowDialog()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return False
        End Try
    End Function
End Class
