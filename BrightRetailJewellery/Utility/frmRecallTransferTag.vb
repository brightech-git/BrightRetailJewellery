Imports System.Data.OleDb
Imports System.IO
Public Class frmRecallTransferTag
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim Syncdb As String = cnAdminDb

    Private Sub frmRecallTransferTag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView_OWN.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmRecallTransferTag_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix) <> 0 Then Syncdb = uprefix + usuffix
        End If
        StrSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE "
        StrSql += " WHERE ISNULL(ACTIVE,'')<>'N'"
        objGPack.FillCombo(StrSql, cmbFromCostcentre_MAN, True, True)
        objGPack.FillCombo(StrSql, cmbToCostcentre_MAN, True, True)
        cmbFromCostcentre_MAN.Focus()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim frmCostid As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbFromCostcentre_MAN.Text & "'", , "", )
        Dim toCostid As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbToCostcentre_MAN.Text & "'", , "", )
        StrSql = " SELECT I.TRANINVNO AS REFNO,I.TRANSFERDATE,I.TAGNO,IM.ITEMNAME,SM.SUBITEMNAME"
        StrSql += vbCrLf + " ,CASE WHEN ISNULL(P.SNO,'')<>'' THEN 'P' ELSE 'T' END AS STATUS,I.COSTID,I.TCOSTID FROM " & cnAdminDb & "..ITEMTAG I"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON I.ITEMID=IM.ITEMID AND I.SUBITEMID=SM.SUBITEMID"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..PITEMTAG P ON P.ITEMID=I.ITEMID AND P.TAGNO=I.TAGNO AND P.TRANINVNO =I.TRANINVNO AND P.SNO=I.SNO "
        StrSql += vbCrLf + " WHERE I.TRANINVNO='" & txtRefno_MAN.Text.Trim & "'"
        StrSql += vbCrLf + " AND I.COSTID='" & frmCostid & "'"
        StrSql += vbCrLf + " AND I.TCOSTID='" & toCostid & "'"
        Dim dtTagDet As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtTagDet)
        If dtTagDet.Rows.Count > 0 Then
            gridView_OWN.DataSource = dtTagDet
        Else
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        With gridView_OWN
            .Columns("REFNO").Width = 70
            .Columns("TAGNO").Width = 70
            .Columns("ITEMNAME").Width = 100
            .Columns("TRANSFERDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            '.Columns("TRANSFERDATE").Visible = False
            .Columns("STATUS").Visible = False
            .Columns("COSTID").Visible = False
            .Columns("TCOSTID").Visible = False
            '.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
        btnUpdate.Enabled = True
        gridView_OWN.Focus()
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If gridView_OWN.Rows.Count > 0 Then
            Dim Refno As String = gridView_OWN.Rows(0).Cells("REFNO").Value.ToString
            Dim FrmCostid As String = gridView_OWN.Rows(0).Cells("COSTID").Value.ToString
            Dim ToCostid As String = gridView_OWN.Rows(0).Cells("TCOSTID").Value.ToString
            If MessageBox.Show("Proceed to recall transfered tags for Refno-" & Refno & "?", "Recall Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If
            Dim objSecret As New frmAdminPassword()
            If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If
            Try
                Dim Qry As String
                tran = Nothing
                tran = cn.BeginTransaction
                If Val(objGPack.GetSqlValue("SELECT 1 FROM " & Syncdb & "..SENDSYNC WHERE SQLTEXT LIKE'%" & Refno & "%' AND STATUS<>'N'", "", "0", tran)) <> 1 Then
                    'Local Execution only
                    StrSql = " SELECT STATUS,UID,UPDFILE FROM " & Syncdb & "..SENDSYNC WHERE SQLTEXT LIKE'%" & Refno & "%' ORDER BY UID"
                    Dim DtSendSync As New DataTable
                    Cmd = New OleDbCommand(StrSql, cn, tran)
                    Da = New OleDbDataAdapter(Cmd)
                    Da.Fill(DtSendSync)
                    For v As Integer = 0 To DtSendSync.Rows.Count - 1
                        StrSql = " UPDATE " & Syncdb & "..SENDSYNC SET STATUS='Y' WHERE UID='" & DtSendSync.Rows(v).Item("UID").ToString & "'"
                        Cmd = New OleDbCommand(StrSql, cn, tran)
                        Cmd.ExecuteNonQuery()
                    Next

                    StrSql = " UPDATE " & cnStockDb & "..ISSUE SET CANCEL='Y' WHERE TRANTYPE='IIN' AND TRANDATE='" & Format(gridView_OWN.Rows(0).Cells("TRANSFERDATE").Value, "yyyy-MM-dd") & "' AND REFNO='" & Refno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ACCTRAN SET CANCEL='Y' WHERE TRANDATE='" & Format(gridView_OWN.Rows(0).Cells("TRANSFERDATE").Value, "yyyy-MM-dd") & "' AND REFNO='" & Refno & "'"
                    Cmd = New OleDbCommand(StrSql, cn, tran)
                    Cmd.ExecuteNonQuery()
                Else
                    If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..PURITEMTAGMETAL WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TRANINVNO='" & Refno & "')", "", "", tran)) = 1 Then
                        Qry = ""
                        StrSql = " DELETE FROM " & cnAdminDb & "..TPURITEMTAGMETAL WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..TITEMTAG WHERE TRANINVNO='" & Refno & "')"
                        Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                        Qry += vbCrLf + " SELECT '" & FrmCostid & "','" & ToCostid & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                        Cmd = New OleDbCommand(Qry, cn, tran) : Cmd.ExecuteNonQuery()
                    End If
                    If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..PURITEMTAGMISCCHAR WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TRANINVNO='" & Refno & "')", "", "", tran)) = 1 Then
                        Qry = ""
                        StrSql = " DELETE FROM " & cnAdminDb & "..TPURITEMTAGMISCCHAR WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..TITEMTAG WHERE TRANINVNO='" & Refno & "')"
                        Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                        Qry += vbCrLf + " SELECT '" & FrmCostid & "','" & ToCostid & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                        Cmd = New OleDbCommand(Qry, cn, tran) : Cmd.ExecuteNonQuery()
                    End If
                    If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..PURITEMTAGSTONE WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TRANINVNO='" & Refno & "')", "", "", tran)) = 1 Then
                        Qry = ""
                        StrSql = " DELETE FROM " & cnAdminDb & "..TPURITEMTAGSTONE WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..TITEMTAG WHERE TRANINVNO='" & Refno & "')"
                        Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                        Qry += vbCrLf + " SELECT '" & FrmCostid & "','" & ToCostid & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                        Cmd = New OleDbCommand(Qry, cn, tran) : Cmd.ExecuteNonQuery()
                    End If
                    If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TRANINVNO='" & Refno & "')", "", "", tran)) = 1 Then
                        Qry = ""
                        StrSql = " DELETE FROM " & cnAdminDb & "..TPURITEMTAG WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..TITEMTAG WHERE TRANINVNO='" & Refno & "')"
                        Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                        Qry += vbCrLf + " SELECT '" & FrmCostid & "','" & ToCostid & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                        Cmd = New OleDbCommand(Qry, cn, tran) : Cmd.ExecuteNonQuery()
                    End If
                    If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TRANINVNO='" & Refno & "')", "", "", tran)) = 1 Then
                        Qry = ""
                        StrSql = " DELETE FROM " & cnAdminDb & "..TITEMTAGMETAL WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..TITEMTAG WHERE TRANINVNO='" & Refno & "')"
                        Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                        Qry += vbCrLf + " SELECT '" & FrmCostid & "','" & ToCostid & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                        Cmd = New OleDbCommand(Qry, cn, tran) : Cmd.ExecuteNonQuery()
                    End If
                    If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TRANINVNO='" & Refno & "')", "", "", tran)) = 1 Then
                        Qry = ""
                        StrSql = " DELETE FROM " & cnAdminDb & "..TITEMTAGHALLMARK WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..TITEMTAG WHERE TRANINVNO='" & Refno & "')"
                        Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                        Qry += vbCrLf + " SELECT '" & FrmCostid & "','" & ToCostid & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                        Cmd = New OleDbCommand(Qry, cn, tran) : Cmd.ExecuteNonQuery()
                    End If
                    If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TRANINVNO='" & Refno & "')", "", "", tran)) = 1 Then
                        Qry = ""
                        StrSql = " DELETE FROM " & cnAdminDb & "..TADDINFOITEMTAG WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..TITEMTAG WHERE TRANINVNO='" & Refno & "')"
                        Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                        Qry += vbCrLf + " SELECT '" & FrmCostid & "','" & ToCostid & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                        Cmd = New OleDbCommand(Qry, cn, tran) : Cmd.ExecuteNonQuery()
                    End If
                    If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TRANINVNO='" & Refno & "')", "", "", tran)) = 1 Then
                        Qry = ""
                        StrSql = " DELETE FROM " & cnAdminDb & "..TITEMTAGSTONE WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..TITEMTAG WHERE TRANINVNO='" & Refno & "')"
                        Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                        Qry += vbCrLf + " SELECT '" & FrmCostid & "','" & ToCostid & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                        Cmd = New OleDbCommand(Qry, cn, tran) : Cmd.ExecuteNonQuery()
                    End If
                    Qry = ""
                    StrSql = " DELETE FROM " & cnAdminDb & "..TITEMTAG WHERE TRANINVNO='" & Refno & "'"
                    Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                    Qry += vbCrLf + " SELECT '" & FrmCostid & "','" & ToCostid & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                    Cmd = New OleDbCommand(Qry, cn, tran) : Cmd.ExecuteNonQuery()
                    Qry = ""

                    StrSql = " UPDATE " & cnStockDb & "..ISSUE SET CANCEL='Y' WHERE TRANTYPE='IIN' AND TRANDATE='" & Format(gridView_OWN.Rows(0).Cells("TRANSFERDATE").Value, "yyyy-MM-dd") & "' AND REFNO='" & Refno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ACCTRAN SET CANCEL='Y' WHERE TRANDATE='" & Format(gridView_OWN.Rows(0).Cells("TRANSFERDATE").Value, "yyyy-MM-dd") & "' AND REFNO='" & Refno & "'"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, cnCostId)
                End If

                StrSql = " UPDATE C SET C.ISSDATE=NULL FROM " & cnAdminDb & "..CTRANSFER AS C," & cnAdminDb & "..ITEMTAG AS I"
                StrSql += vbCrLf + " WHERE C.TAGSNO=I.SNO AND C.ISSDATE=I.ISSDATE AND I.TRANINVNO='" & Refno & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

                StrSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET ISSDATE = NULL,TCOSTID=COSTID,TOFLAG='' WHERE TRANINVNO='" & Refno & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

                If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..PITEMTAGSTONE WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..PITEMTAG WHERE TRANINVNO='" & Refno & "')", "", "", tran)) = 1 Then
                    StrSql = " DELETE FROM " & cnAdminDb & "..PITEMTAGSTONE WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..PITEMTAG WHERE TRANINVNO='" & Refno & "')"
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                End If
                StrSql = " DELETE FROM " & cnAdminDb & "..PITEMTAG WHERE TRANINVNO='" & Refno & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

                tran.Commit()
                MsgBox("Tag recall Successfully completed for Refno-" & Refno & ".", MsgBoxStyle.Information)
                gridView_OWN.DataSource = Nothing
                cmbToCostcentre_MAN.SelectedIndex = 0
                cmbFromCostcentre_MAN.SelectedIndex = 0
                txtRefno_MAN.Text = ""
                btnUpdate.Enabled = False
                cmbFromCostcentre_MAN.Focus()
            Catch ex As Exception
                If tran IsNot Nothing Then tran.Rollback()
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End Try
        End If
    End Sub

    Private Sub BtnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    
    Private Sub cmbFromCostcentre_MAN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFromCostcentre_MAN.SelectedIndexChanged
        'If cmbFromCostcentre_MAN.Text <> "" Then
        '    StrSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE "
        '    StrSql += " WHERE ISNULL(ACTIVE,'')<>'N'"
        '    StrSql += " AND COSTNAME<>'" & cmbFromCostcentre_MAN.Text & "'"
        '    objGPack.FillCombo(StrSql, cmbToCostcentre_MAN, True, True)
        'End If
        
    End Sub
End Class