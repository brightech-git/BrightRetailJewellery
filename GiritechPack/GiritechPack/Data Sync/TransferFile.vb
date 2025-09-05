Imports System.IO
Imports System.Data.OleDb
Public Class TransferFile
    Dim _StrSql As String
    Dim _Da As OleDbDataAdapter
    Dim _Cmd As OleDbCommand
    Dim Syncdb As String

    Public Sub ReceiveFile(ByVal ReceivedCostId As String, ByVal cnAdmindb As String, ByVal cnStockDb As String, ByVal lCn As OleDbConnection, Optional ByVal replaceDbs As List(Of String) = Nothing)
        Try
            Dim sepDb As String = BrighttechPack.GlobalMethods.GetSqlValue(lCn, "SELECT CTLTEXT FROM " & cnAdmindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-SEPDATADB'", , "N")
            Syncdb = cnAdmindb
            Dim uprefix As String = Mid(cnAdmindb, 1, 3)
            If sepDb = "Y" Then Dim usuffix As String = "UTILDB" : Syncdb = uprefix + usuffix
            Dim _StrFtr As String = Nothing
            _StrSql = " SELECT FTPID FROM " & cnAdmindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & ReceivedCostId & "'"
            Dim recFtpId As String = BrighttechPack.GlobalMethods.GetSqlValue(lCn, _StrSql)
            If Not IO.Directory.Exists(recFtpId) Then
                MsgBox("Invalid Receiver FtpId", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim objZip As BrighttechPack.Zipper
            For Each fName As String In Directory.GetFiles(recFtpId, "*.Zip")
                Dim fINfo As New IO.FileInfo(fName)
                If Not fINfo.Name.ToUpper.StartsWith("SYNC-") Then Continue For
                Dim fNameDet() As String = fINfo.Name.ToUpper.Split("-")
                Dim SenderCostId As String = fNameDet(1)
                If Not fNameDet.Length > 2 Then Continue For
                If Not ReceivedCostId.ToUpper = fNameDet(2).ToUpper Then Continue For
                Dim xmlName As String = recFtpId & "\" & fINfo.Name.Replace(".zip", ".xml")
                Dim fINfo1 As New IO.FileInfo(xmlName)
                objZip = New BrighttechPack.Zipper
                If Not objZip.UnZip(fName, fINfo1.DirectoryName) Then
                    If IO.File.Exists(xmlName) Then IO.File.Delete(xmlName)
                    MsgBox("Pbm rising from unzip", MsgBoxStyle.Information)
                    Continue For
                End If
                If InsertIntoDb(SenderCostId, ReceivedCostId, xmlName, cnAdmindb, cnStockDb, lCn, replaceDbs) Then
                    If IO.File.Exists(fName) Then IO.File.Delete(fName)
                Else
                    Exit Sub
                End If
                If IO.File.Exists(xmlName) Then IO.File.Delete(xmlName)
            Next
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Function InsertIntoDb(ByVal senderCostId As String, ByVal receiverCostId As String, ByVal fileName As String, ByVal cnADmindb As String, ByVal cnStockDb As String, ByVal lCn As OleDbConnection, Optional ByVal replaceDbs As List(Of String) = Nothing) As Boolean
        Dim tran As OleDbTransaction = Nothing
        Dim OrgQry As String = ""
        Dim qry As String = ""
        Dim objErr As New ErrorQryDia
        Try
            'Dim gPack As New Methods(lCn, cnADmindb, "", Color.AliceBlue, Color.AliceBlue, Color.AliceBlue, Color.AliceBlue, Color.AliceBlue)
            Dim sepDb As String = BrighttechPack.GlobalMethods.GetSqlValue(lCn, "SELECT CTLTEXT FROM " & cnADmindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-SEPDATADB'", , "N")
            Syncdb = cnADmindb
            Dim uprefix As String = Mid(cnADmindb, 1, 3)
            If sepDb = "Y" Then Dim usuffix As String = "UTILDB" : Syncdb = uprefix + usuffix

            Dim fs As New IO.FileStream(fileName, IO.FileMode.Open, IO.FileAccess.Read)
            Dim strSql As String
            Dim ds As New DataSet
            Dim cmd As OleDbCommand
            ds.ReadXml(fs, XmlReadMode.ReadSchema)
            fs.Close()
            Dim Finfo As New FileInfo(fileName)
            strSql = "SELECT 'CHECK' FROM " & Syncdb & "..RECEIVESYNC WHERE UPDFILE = '" & Finfo.Name & "'"
            If BrighttechPack.GlobalMethods.GetSqlValue(lCn, strSql).Length > 0 Then
                MsgBox(Finfo.Name & " Already Downloaded", MsgBoxStyle.Information)
                Return False
            End If
            Dim sendCompId As String = BrighttechPack.GlobalMethods.GetSqlValue(lCn, "SELECT COMPID FROM " & cnADmindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & senderCostId & "'")
            Dim recvCompId As String = BrighttechPack.GlobalMethods.GetSqlValue(lCn, "SELECT COMPID FROM " & cnADmindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & receiverCostId & "'")
            tran = lCn.BeginTransaction
            For cnt As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim ro As DataRow = ds.Tables(0).Rows(cnt)
                qry = BrighttechPack.Methods.DecryptXml(ro!SQLTEXT.ToString)
                OrgQry = qry
                Dim tempAdmindb As String = cnADmindb.ToUpper.Replace(recvCompId.ToUpper, sendCompId.ToUpper)
                Dim tempStockdb As String = cnStockDb.ToUpper.Replace(recvCompId.ToUpper, sendCompId.ToUpper)
                objErr.ListBox1.Items.Add("Original Qry" & OrgQry)
                qry = qry.ToUpper.Replace(tempAdmindb.ToUpper, cnADmindb.ToUpper)
                objErr.ListBox1.Items.Add("After Admindb Replaced Qry" & qry)
                qry = qry.ToUpper.Replace(tempStockdb.ToUpper, cnStockDb.ToUpper)
                objErr.ListBox1.Items.Add("After TranDb Replaced Qry" & qry)
                If Not replaceDbs Is Nothing Then
                    Dim vbPrefix As String = BrighttechPack.GlobalMethods.GetSqlValue(lCn, "SELECT CTLTEXT FROM " & cnADmindb & "..SOFTCONTROL WHERE CTLID = 'VBDBPREFIX'", , , tran)
                    objErr.ListBox1.Items.Add("Getting vbdbprefix " & vbPrefix)
                    If vbPrefix <> "" Then
                        For Each s As String In replaceDbs
                            objErr.ListBox1.Items.Add("Replace like " & s)
                            Dim suffix As String = Mid(s, 4, s.Length)
                            objErr.ListBox1.Items.Add("Suffix" & suffix)
                            Dim dbIndex As Integer = qry.ToUpper.IndexOf(suffix.ToUpper)
                            objErr.ListBox1.Items.Add("DbIndex " & dbIndex.ToString)
                            If dbIndex > -1 Then
                                Dim replaceDb As String = Mid(qry.ToUpper, dbIndex - 2, suffix.Length + 3)
                                objErr.ListBox1.Items.Add("Replace Db " & dbIndex)
                                Dim vbDb As String = vbPrefix & suffix
                                objErr.ListBox1.Items.Add("Vb Db " & vbDb)
                                qry = qry.ToUpper.Replace(replaceDb.ToUpper, vbDb.ToUpper)
                                objErr.ListBox1.Items.Add("Replaced Qry " & qry)
                            End If
                        Next
                    End If
                End If
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
                    Dim defDir As String = BrighttechPack.GlobalMethods.GetSqlValue(lCn, "SELECT CTLTEXT FROM " & cnADmindb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'", , , tran)
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
            qry = " INSERT INTO " & cnADmindb & "..SENDSYNC(FROMID,TOID,SQLTEXT,UPDFILE)"
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
            objErr.Text = fileName
            objErr.txtOrginal.Text = OrgQry
            objErr.txtErr.Text = qry
            objErr.ShowDialog()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return False
        End Try
    End Function
    Public Sub SendFile(ByVal _frmCostId As String, ByVal _toCostId As List(Of String), ByVal _cnAdmindb As String, ByVal _cnStockDb As String, ByVal _lCn As OleDbConnection, Optional ByVal replaceDbs As List(Of String) = Nothing)
        Dim strSql As String
        Dim fInfo As IO.FileInfo = Nothing
        Dim _RootDir As String = Application.StartupPath & "\UploadXml"
        Try
            Dim sepDb As String = BrighttechPack.GlobalMethods.GetSqlValue(_lCn, "SELECT CTLTEXT FROM " & _cnAdmindb & "..SOFTCONTROL WHERE CTLID = 'SYNC-SEPDATADB'", , "N")
            Syncdb = _cnAdmindb
            Dim uprefix As String = Mid(_cnAdmindb, 1, 3)
            If sepDb = "Y" Then Dim usuffix As String = "UTILDB" : Syncdb = uprefix + usuffix

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
            Dim _sendFtpPath As String = BrighttechPack.GlobalMethods.GetSqlValue(_lCn, " SELECT FTPID FROM " & _cnAdmindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & _frmCostId & "'")
            Dim _toUserId As String = ""
            Dim _toFtpPath As String = ""
            For Each ro As DataRow In dtToId.Rows
                _tblName = "SYNC-" & _frmCostId & "-" & ro.Item("TOID").ToString & "-" & Today.Date.ToString("ddMMyy") & "-" & Date.Now.ToString("HHssmm")
                _toFtpPath = BrighttechPack.GlobalMethods.GetSqlValue(_lCn, " SELECT FTPID FROM " & _cnAdmindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & ro.Item("TOID").ToString & "'")
                If _sendFtpPath = "" Or _toFtpPath = "" Then Continue For
                If Not IO.Directory.Exists(_toFtpPath) Then Continue For
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
                    IO.File.Move(zipFile, _toFtpPath & "\" & _tblName & ".zip")
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
        End Try
    End Sub

    
    
End Class
