Imports System.IO
Imports System.Data.OleDb
Public Class ReceiveManual
    Dim Da As OleDbCommand
    Dim Cmd As OleDbCommand
    Private _Da As OleDbDataAdapter
    Dim Tran As OleDbTransaction = Nothing
    Dim frmCostId As String = objGPack.GetSqlValue(" SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTID'").ToUpper
    Dim SenderId As String
    Dim ReceiverId As String
    Dim _ReplaceWords As New List(Of String)
    Dim vbPrefix As String = ""
    Dim Syncdb As String
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()

    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Dim filDia As New OpenFileDialog
        Dim Str As String = "Zip(*.Zip)|*.Zip"
        Dim FInfo As FileInfo
        filDia.Filter = Str
        If filDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            FInfo = New FileInfo(filDia.FileName)
            If Not FInfo.Name.ToUpper.StartsWith("SYNC-") Then
                MsgBox("Invalid File", MsgBoxStyle.Information, "File Selection Error 0xc123")
                Exit Sub
            End If
            If Not FInfo.Name.ToUpper.Contains("-") Then
                MsgBox("Invalid File", MsgBoxStyle.Information, "File Selection Error 0xc124")
                Exit Sub
            End If
            Dim subject() As String = FInfo.Name.ToUpper.Split("-")
            SenderId = subject(1)
            ReceiverId = ""
            If subject.Length > 2 Then
                ReceiverId = subject(2)
            Else
                MsgBox("Invalid File", MsgBoxStyle.Information, "File Selection Error 0xc125")
                Exit Sub
            End If
            If Not frmCostId.ToUpper = ReceiverId.ToUpper Then
                MsgBox("Invalid File", MsgBoxStyle.Information, "File Selection Error 0xc126")
                Exit Sub
            End If
            txtFilePath.Text = filDia.FileName
        End If
    End Sub

    Private Sub txtFilePath_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFilePath.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub btnReceive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReceive.Click
        If Not IO.File.Exists(txtFilePath.Text) Then
            MsgBox("Invalid File", MsgBoxStyle.Information)
            Exit Sub
        End If
        If SenderId = "" Then
            MsgBox("Invalid SenderId ", MsgBoxStyle.Information)
            Exit Sub
        End If
        If ReceiverId = "" Then
            MsgBox("Invalid ReceiverId", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim objZip As BrighttechPack.Zipper
        Dim fileName As String = txtFilePath.Text
        Dim fINfo As New IO.FileInfo(fileName)
        Dim xmlName As String = System.IO.Path.GetTempPath & "\" & fINfo.Name.ToUpper.Replace("ZIP", "XML")
        objZip = New BrighttechPack.Zipper
        objZip.UnZip(fileName, System.IO.Path.GetTempPath)
        If InsertIntoDb(SenderId, ReceiverId, xmlName, cnAdminDb, cnStockDb, cn, _ReplaceWords) Then
            IO.File.Delete(txtFilePath.Text)
            If IO.File.Exists(xmlName) Then IO.File.Delete(xmlName)
        End If
        Me.Close()
    End Sub
    Private Function ReplaceQryStr(ByVal SourceStr As String, ByVal OldDbId As String, ByVal NewDbId As String) As String
        Dim I_SenderDb As String = ""
        Dim I_ReplaceDb As String = ""
        Dim I_IndexOfDb As Integer
        Dim OldStr As String = ""
        Dim NewStr As String = ""
        For Each suffix As String In _ReplaceWords
            If suffix.ToUpper = "SAVINGS.." Or suffix.ToUpper = "SH0708.." Then I_ReplaceDb = vbPrefix Else I_ReplaceDb = NewDbId
            I_IndexOfDb = SourceStr.ToUpper.IndexOf(suffix.ToUpper)
            If I_IndexOfDb > -1 Then
                I_SenderDb = Mid(SourceStr.ToUpper, I_IndexOfDb - 2, 3)
                OldStr = I_SenderDb.ToUpper & suffix
                NewStr = I_ReplaceDb.ToUpper & suffix
                SourceStr = SourceStr.ToUpper.Replace(OldStr, NewStr)
            End If
        Next
        Return SourceStr
    End Function

    Private Function InsertIntoDb(ByVal senderCostId As String, ByVal receiverCostId As String, ByVal fileName As String, ByVal cnADmindb As String, ByVal cnStockDb As String, ByVal lCn As OleDbConnection, Optional ByVal ReplaceDbs As List(Of String) = Nothing) As Boolean
        'Dim tran As OleDbTransaction = Nothing
        Dim OrgQry As String = ""
        Dim qry As String = ""
        Try
            Dim fs As New IO.FileStream(fileName, IO.FileMode.Open, IO.FileAccess.Read)
            Dim strSql As String
            Dim ds As New DataSet
            Dim cmd As OleDbCommand
            ds.ReadXml(fs, XmlReadMode.ReadSchema)
            fs.Close()
            Dim Finfo As New FileInfo(fileName)
            strSql = "SELECT 'CHECK' FROM " & Syncdb & "..RECEIVESYNC WHERE UPDFILE = '" & Finfo.Name & "'"
            If objGPack.GetSqlValue(strSql).Length > 0 Then
                MsgBox(Finfo.Name & " Already Downloaded", MsgBoxStyle.Information)
                Return False
            End If
            ProgressBarStep("Downloading into Db")
            Dim sendCompId As String = objGPack.GetSqlValue("SELECT COMPID FROM " & cnADmindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & senderCostId & "'")
            Dim recvCompId As String = objGPack.GetSqlValue("SELECT COMPID FROM " & cnADmindb & "..SYNCCOSTCENTRE WHERE COSTID = '" & receiverCostId & "'")
            vbPrefix = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnADmindb & "..SOFTCONTROL WHERE CTLID = 'CHITDBPREFIX'")
            Dim mupdfile As String = Finfo.Name
            tran = lCn.BeginTransaction
            For cnt As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim ro As DataRow = ds.Tables(0).Rows(cnt)
                qry = BrighttechPack.Methods.DecryptXml(ro!SQLTEXT.ToString)
                OrgQry = qry
                If Not qry Is Nothing Then qry = ReplaceQryStr(qry, sendCompId, recvCompId)
                
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


            Dim Row As DataRow = Nothing
            Dim Uids As Long
            Dim R_fromid As String, R_toid As String
            strSql = " SELECT * FROM " & Syncdb & "..RECEIVESYNC WHERE TOID = '" & receiverCostId & "' AND STATUS = 'N' ORDER BY UID"
            Dim R_DtReceive As New DataTable
            Dim _cmde As New OleDbCommand
            _cmde = New OleDbCommand(strSql, lCn, tran)
            _Da = New OleDbDataAdapter(_cmde)
            _Da.Fill(R_DtReceive)
            If R_DtReceive.Rows.Count > 0 Then
                For II As Integer = 0 To R_DtReceive.Rows.Count - 1

                    Row = R_DtReceive.Rows(II)
                    If Row!TAGIMAGE.ToString <> "" Then Continue For
                    Uids = Row!UID.ToString

                    Try
                        If Row!SQLTEXT.ToString <> "" Then
                            'tran = Nothing
                            'tran = _CnAdmin.BeginTransaction
                            cmd = New OleDbCommand(Row!SQLTEXT.ToString, lCn, tran)
                            cmd.ExecuteNonQuery()
                            strSql = "UPDATE  " & Syncdb & "..RECEIVESYNC SET STATUS= 'Y' WHERE UID= " & Val(Uids)
                            cmd = New OleDbCommand(strSql, lCn, tran)
                            cmd.ExecuteNonQuery()
                            'If tran.Connection IsNot Nothing Then tran.Commit()
                            R_fromid = Row!fromid
                            R_toid = Row!toid
                            If mupdfile <> "" And Row!updfile <> mupdfile Then SendAcknowledge(R_fromid, R_toid, mupdfile, lCn)
                            mUpdfile = Row!updfile
                        End If
                    Catch ex As Exception
                        Dim ErrMsg As String = ex.Message
                        If ErrMsg.Contains("Violation of PRIMARY KEY constraint") Then
                            strSql = "UPDATE  " & Syncdb & "..RECEIVESYNC SET STATUS= 'Y' WHERE UID= " & Row!UID
                            cmd = New OleDbCommand(strSql, lCn, tran)
                            cmd.ExecuteNonQuery()
                            'If tran.Connection IsNot Nothing Then tran.Commit()
                        ElseIf ErrMsg.Contains("Violation of UNIQUE KEY constraint") Then
                            'If tran.Connection IsNot Nothing Then tran.Rollback()
                            If Not tran Is Nothing Then tran.Rollback()
                            mUpdfile = ""
                        Else
                            'If tran.Connection IsNot Nothing Then tran.Rollback()
                            If Not tran Is Nothing Then tran.Rollback()
                            mUpdfile = ""

                            Throw New Exception(ErrMsg)
                            Exit Function
                        End If

                    End Try


                Next
                If mupdfile <> "" Then SendAcknowledge(R_fromid, R_toid, mupdfile, lCn)
                

            End If




            'Dim ret As String = Nothing
            'ret = "UPDATE " & Syncdb & "..SENDSYNC SET STATUS = 'Y' "
            'ret += " WHERE FROMID = '" & senderCostId & "'"
            'ret += " AND TOID = '" & receiverCostId & "'"
            'ret += " AND STATUS = 'M'"
            'ret += " and UPDFILE = '" & Finfo.Name & "'"
            'ret += " AND UID BETWEEN " & Val(ds.Tables(0).Rows(0).Item("UID").ToString) & ""
            'ret += " AND " & Val(ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1).Item("UID").ToString) & ""

            'qry = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,UPDFILE)"
            'qry += " VALUES"
            'qry += " ("
            'qry += " '" & receiverCostId & "'"
            'qry += " ,'" & senderCostId & "'"
            'qry += " ,'" & ret.Replace("'", "''") & "'"
            'qry += " ,'" & Finfo.Name & "'"
            'qry += " )"
            'cmd = New OleDbCommand(qry, lCn, tran)
            'cmd.ExecuteNonQuery()
            ProgressBarStep("")
            ProgressBarHide()
            tran.Commit()
            tran = Nothing
            Return True
        Catch ex As Exception
            'If tran IsNot Nothing Then tran.Rollback()
            If Not tran Is Nothing Then tran.Rollback()
            Dim objErr As New ErrorQryDia
            objErr.txtOrginal.Text = OrgQry
            objErr.txtErr.Text = qry
            objErr.ShowDialog()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return False
        End Try
    End Function

    Function SendAcknowledge(ByVal R_fromid As String, ByVal R_toid As String, ByVal mUpdfile As String, ByVal lCn As OleDbConnection)
        Dim mqry As String
        '' acknowledgement reply
        'If Tran Is Nothing Then Tran = lCn.BeginTransaction
        Dim replsql As String = " UPDATE " & Syncdb & "..SENDSYNC SET STATUS = 'Y' WHERE STATUS = 'M' AND FROMID = '" & R_fromid & "' AND TOID = '" & R_toid & "' and UPDFILE ='" & mUpdfile & "'"
        mqry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC ("
        mqry += vbCrLf + " FROMID"
        mqry += vbCrLf + " ,TOID"
        mqry += vbCrLf + " ,SQLTEXT"
        mqry += vbCrLf + " ,STATUS)"
        mqry += vbCrLf + " VALUES "
        mqry += vbCrLf + " (?,?,?,?)"
        Cmd = New OleDbCommand(mqry, lCn, Tran)
        Cmd.Parameters.AddWithValue("@FROMID", R_toid.ToString)
        Cmd.Parameters.AddWithValue("@TOID", R_fromid.ToString)
        Cmd.Parameters.AddWithValue("@SQLTEXT", replsql.ToString)
        Cmd.Parameters.AddWithValue("@STATUS", "N")
        Cmd.ExecuteNonQuery()
        'If tran.Connection IsNot Nothing Then tran.Commit()

    End Function

    Private Sub ReceiveManual_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        syncdb = cnAdminDb
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", Tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix, True, Tran) <> 0 Then syncdb = uprefix + usuffix
        End If
        _ReplaceWords.Add("ADMINDB..")
        _ReplaceWords.Add("T0708..")
        _ReplaceWords.Add("T0809..")
        _ReplaceWords.Add("T0910..")
        _ReplaceWords.Add("T1011..")
        _ReplaceWords.Add("T1112..")
        _ReplaceWords.Add("T1213..")
        _ReplaceWords.Add("T1314..")
        _ReplaceWords.Add("T1415..")
        _ReplaceWords.Add("T1516..")
        _ReplaceWords.Add("T1617..")
        _ReplaceWords.Add("T1718..")
        _ReplaceWords.Add("T1819..")
        _ReplaceWords.Add("SAVINGS..")
        _ReplaceWords.Add("SH0708..")
        _ReplaceWords.Add("UTILDB..")
    End Sub
End Class

