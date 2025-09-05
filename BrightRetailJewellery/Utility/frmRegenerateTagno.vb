Imports System.Data.OleDb
Imports System.IO
Public Class frmRegenerateTagno
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim Syncdb As String = cnAdminDb
    Dim objTag As New TagGeneration
    Dim DefaultPctFile As String = GetAdmindbSoftValue("DEFAULT_PCTFILE", "")
    Dim defalutDestination As String = ""

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub frmRegenerateTagno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmRegenerateTagno_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        'If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
        '    Dim usuffix As String = "UTILDB"
        '    If DbChecker(uprefix + usuffix) <> 0 Then Syncdb = uprefix + usuffix
        'End If
        funcLoadItemName()
    End Sub
    Function funcLoadItemName() As Integer
        StrSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        StrSql += " WHERE ACTIVE = 'Y'"
        StrSql += " ORDER BY RESULT,ITEMNAME"
        Dim dtItem As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", True, "ALL")
    End Function

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        Dim Otagno As String
        Dim Itemid As Integer
        Dim Ntagno As String
        Dim NtagVal As Integer
        Dim Ntagkey As String
        Dim Itemname As String
        Dim PicPath As String = ""
        Dim LOTNO As String = ""
        Dim dttag As New DataTable
        StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
        defalutDestination = UCase(objGPack.GetSqlValue(StrSql, "CTLTEXT", , tran))
        If Not defalutDestination.EndsWith("\") And defalutDestination <> "" Then defalutDestination += "\"
        Try
            If chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                StrSql = "SELECT SNO,RECDATE,T.ITEMID,I.ITEMNAME,TAGNO,LOTSNO,ISNULL(T.PCTFILE,'') PCTFILE "
                StrSql += " ,(SELECT LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = T.LOTSNO)AS LOTNO"
                StrSql += " FROM " & cnAdminDb & "..ITEMTAG T "
                StrSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID=T.ITEMID "
                StrSql += " WHERE T.ITEMID IN(" & GetSelecteditemid(chkCmbItem, False) & ") And T.ISSDATE Is NULL"
                dttag = New DataTable
                Da = New OleDbDataAdapter(StrSql, cn)
                Da.Fill(dttag)
            Else
                StrSql = "SELECT SNO,RECDATE,T.ITEMID,I.ITEMNAME,TAGNO,LOTSNO,ISNULL(T.PCTFILE,'') PCTFILE "
                StrSql += " ,(SELECT LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = T.LOTSNO)AS LOTNO"
                StrSql += " FROM " & cnAdminDb & "..ITEMTAG T "
                StrSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID=T.ITEMID And ISNULL(I.ACTIVE,'')<>'N' "
                StrSql += " WHERE T.ISSDATE IS NULL"
                dttag = New DataTable
                Da = New OleDbDataAdapter(StrSql, cn)
                Da.Fill(dttag)
            End If
            tran = Nothing
            tran = cn.BeginTransaction
            For v As Integer = 0 To dttag.Rows.Count - 1
                Otagno = dttag.Rows(v).Item("TAGNO").ToString
                Itemid = Val(dttag.Rows(v).Item("ITEMID").ToString)
                Itemname = dttag.Rows(v).Item("ITEMNAME").ToString
                LOTNO = dttag.Rows(v).Item("LOTNO").ToString
                Ntagno = objTag.GetTagNo(Format(dttag.Rows(v).Item("RECDATE"), "yyyy-MM-dd"), Itemname, dttag.Rows(v).Item("LOTSNO").ToString, tran)
                NtagVal = objTag.GetTagVal(Ntagno)
                Ntagkey = Itemid.ToString & Ntagno
                PicPath = IIf(dttag.Rows(v).Item("PCTFILE").ToString <> "", dttag.Rows(v).Item("PCTFILE").ToString, "")

                ''StrSql = "UPDATE " & cnAdminDb & "..ITEMTAG SET OLDTAGNO='" & Otagno & "' "
                ''StrSql += " WHERE SNO='" & dttag.Rows(v).Item("SNO").ToString & "'"
                ''Cmd = New OleDbCommand(StrSql, cn, tran)
                ''Cmd.ExecuteNonQuery()

                StrSql = "UPDATE " & cnAdminDb & "..ITEMTAG SET TAGNO='" & Ntagno & "'"
                StrSql += " ,TAGKEY='" & Ntagkey & "' ,TAGVAL='" & NtagVal & "'"
                StrSql += " ,OLDTAGNO='" & Otagno & "'"

                '' added on 14-June-2022 for pctfile update
                If PicPath.ToString <> "" And defalutDestination.ToString <> "" Then
                    Dim picExtension As String = ""
                    Dim fileDestPath As String = defalutDestination & PicPath.ToString
                    If File.Exists(fileDestPath.ToString) Then
                        Dim Finfo As FileInfo
                        Finfo = New FileInfo(fileDestPath)
                        picExtension = IIf(Finfo.Extension.ToString <> "", Finfo.Extension, ".jpg")
                        If DefaultPctFile.ToString <> "" Then
                            Dim _tempdefpctfile As String = ""
                            _tempdefpctfile = DefaultPctFile.ToString
                            _tempdefpctfile = _tempdefpctfile.Replace("<ITEMID>", Itemid.ToString)
                            _tempdefpctfile = _tempdefpctfile.Replace("<TAGNO>", Ntagno.Replace(":", "").ToString)
                            StrSql += " ,PCTFILE ='" & _tempdefpctfile.ToString & "'" 'pctfile
                            If File.Exists(defalutDestination.ToString & _tempdefpctfile.ToString) Then
                                File.Delete(defalutDestination.ToString & _tempdefpctfile.ToString)
                            End If
                            My.Computer.FileSystem.RenameFile(defalutDestination.ToString & PicPath.ToString, _tempdefpctfile.ToString)
                        Else
                            Dim _temppctpath As String = IIf(PicPath <> "", GetStockCompId() & "L" + LOTNO.ToString + "I" + Itemid.ToString + "T" + Ntagno.Replace(":", "") + IIf(picExtension.ToString.StartsWith("."), picExtension.ToString, "." + picExtension.ToString), PicPath)
                            StrSql += " ,PCTFILE = '" & _temppctpath.ToString & "'"
                            If File.Exists(defalutDestination.ToString & _temppctpath.ToString) Then
                                File.Delete(defalutDestination.ToString & _temppctpath.ToString)
                            End If
                            My.Computer.FileSystem.RenameFile(defalutDestination.ToString & PicPath.ToString, _temppctpath.ToString)
                        End If
                    End If
                End If
                ''End added on 14-June-2022 for pctfile update

                StrSql += " WHERE SNO='" & dttag.Rows(v).Item("SNO").ToString & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()

                ''StrSql = "UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET OLDTAGNO='" & Otagno & "'"
                ''StrSql += " WHERE TAGSNO='" & dttag.Rows(v).Item("SNO").ToString & "'"
                ''Cmd = New OleDbCommand(StrSql, cn, tran)
                ''Cmd.ExecuteNonQuery()

                StrSql = "UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET TAGNO='" & Ntagno & "'"
                StrSql += " ,OLDTAGNO='" & Otagno & "'"
                StrSql += " WHERE TAGSNO='" & dttag.Rows(v).Item("SNO").ToString & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()

                StrSql = "UPDATE " & cnAdminDb & "..ITEMTAGMETAL SET TAGNO='" & Ntagno & "'"
                StrSql += " WHERE TAGSNO='" & dttag.Rows(v).Item("SNO").ToString & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()

                StrSql = "UPDATE " & cnAdminDb & "..ITEMTAGMISCCHAR SET TAGNO='" & Ntagno & "'"
                StrSql += " WHERE TAGSNO='" & dttag.Rows(v).Item("SNO").ToString & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()

                StrSql = "UPDATE " & cnAdminDb & "..PURITEMTAG SET TAGNO='" & Ntagno & "'"
                StrSql += " WHERE TAGSNO='" & dttag.Rows(v).Item("SNO").ToString & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()

                StrSql = "UPDATE " & cnAdminDb & "..PURITEMTAGSTONE SET TAGNO='" & Ntagno & "'"
                StrSql += " WHERE TAGSNO='" & dttag.Rows(v).Item("SNO").ToString & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()

            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Tagno Regenerated successfully completed.", MsgBoxStyle.Information)
            'Ntagno = objTag.GetTagNo(dtpRecieptDate.Value.ToString("yyyy-MM-dd"), cmbItem_MAN.Text, SNO)

        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub


End Class