Imports System.Data.OleDb
Imports System.IO
Public Class frmPktNoGen
    Dim dtTagDet As New DataTable
    Dim Cmd As New OleDbCommand
    Dim strSql As String
    Dim dtGrid As New DataTable
    Dim cnt As New Integer

    Private Sub fNew()
        Label2.Visible = False
        txtpocketno.Clear()
        txtTagNo.Clear()
        txtfromtag.Clear()
        txttotag.Clear()
        Panel2.Visible = False
        btndelete.Enabled = False
        txtpocketno.Visible = False
        dtTagDet.Rows.Clear()
        dtGrid.Rows.Clear()
        dtTagDet.AcceptChanges()
        txtTagNo.Select()
        gridView.DataSource = Nothing
        gridView.Refresh()
        strSql = vbCrLf + "IF (SELECT TOP 1 1 FROM  TEMPTABLEDB..SYSOBJECTS WHERE NAME ='TEMPPACKNOGEN')>0 DROP TABLE  TEMPTABLEDB..TEMPPACKNOGEN"
        Cmd = New OleDbCommand(strSql, cn)
        Cmd.ExecuteNonQuery()        
    End Sub

    Private Sub frmTagCheck_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dtTagDet = New DataTable
        dtTagDet.Columns.Add("ITEM", GetType(String))
        dtTagDet.Columns.Add("TAGNO", GetType(String))
        dtTagDet.Columns.Add("ITEMID", GetType(String))
        gridView.DataSource = dtTagDet
        gridView.Columns("ITEM").MinimumWidth = 180
        gridView.Columns("ITEMID").Visible = False
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        fNew()
    End Sub

    Private Sub txtTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTagNo.Text = "" Then
                MsgBox("TagNo should not empty", MsgBoxStyle.Information)
                txtTagNo.Focus()
                Exit Sub
            End If
            For Each ro As DataRow In dtGrid.Rows
                If ro("TAGNO").ToString = txtTagNo.Text Then
                    MsgBox("TagNo Already Loaded", MsgBoxStyle.Information)
                    txtTagNo.Clear()
                    txtTagNo.Focus()
                    Exit Sub
                End If
            Next
            dtTagDet.Rows.Clear()
            dtGrid.Rows.Clear()
            strSql = "SELECT TOP 1 1 FROM  TEMPTABLEDB..SYSOBJECTS WHERE NAME ='TEMPPACKNOGEN'"
            cnt = Val(GetSqlValue(cn, strSql))
            If (cnt > 0) Then
                strSql = "SELECT COUNT(*)  FROM TEMPTABLEDB..TEMPPACKNOGEN"
                cnt = Val(GetSqlValue(cn, strSql))
                If (cnt > 0) Then
                    strSql = " INSERT INTO TEMPTABLEDB..TEMPPACKNOGEN(ITEM,TAGNO,PCS,NETWT,GRSWT,ITEMID) "
                    strSql += " SELECT(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
                    strSql += " ,TAGNO,PCS,NETWT,GRSWT,ITEMID FROM " & cnAdminDb & "..ITEMTAG AS T"
                    strSql += " WHERE TAGNO = '" & txtTagNo.Text & "' AND ISNULL(ISSDATE,'')='' "                    
                    strSql += " AND TAGNO NOT IN(SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAGPKTNO)"
                    If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
                    If cnCostId <> "" Then strSql += " AND COSTID = '" & cnCostId & "'"
                    Cmd = New OleDbCommand(strSql, cn)
                    Cmd.ExecuteNonQuery()
                    strSql = "DELETE FROM TEMPTABLEDB..TEMPPACKNOGEN WHERE ITEM='TOTAL'"
                    Cmd = New OleDbCommand(strSql, cn)
                    Cmd.ExecuteNonQuery()
                End If
            Else
                strSql = " SELECT IDENTITY(int, 1,1) AS SNO"
                strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
                strSql += " ,TAGNO,PCS,NETWT,GRSWT,ITEMID INTO TEMPTABLEDB..TEMPPACKNOGEN FROM " & cnAdminDb & "..ITEMTAG AS T"
                strSql += " WHERE TAGNO = '" & txtTagNo.Text & "' AND ISNULL(ISSDATE,'')='' "                
                strSql += " AND TAGNO NOT IN(SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAGPKTNO)"
                If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
                If cnCostId <> "" Then strSql += " AND COSTID = '" & cnCostId & "'"
                Cmd = New OleDbCommand(strSql, cn)
                Cmd.ExecuteNonQuery()
            End If

            strSql = "SELECT TOP 1 1 FROM  TEMPTABLEDB..TEMPPACKNOGEN"
            cnt = Val(GetSqlValue(cn, strSql))
            If (cnt > 0) Then
                strSql = " INSERT INTO TEMPTABLEDB..TEMPPACKNOGEN (ITEM,PCS,NETWT,GRSWT)"
                strSql += "SELECT 'TOTAL'ITEM,SUM(PCS)PCS,SUM(NETWT)NETWT,SUM(GRSWT)GRSWT FROM TEMPTABLEDB..TEMPPACKNOGEN "
                Cmd = New OleDbCommand(strSql, cn)
                Cmd.ExecuteNonQuery()
            End If
            strSql = " SELECT SNO,ITEM,TAGNO,PCS,NETWT,GRSWT,ITEMID FROM TEMPTABLEDB..TEMPPACKNOGEN ORDER BY SNO"
            'strSql = " SELECT "
            'strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            'strSql += " ,TAGNO,ITEMID FROM " & cnAdminDb & "..ITEMTAG AS T"
            'strSql += " WHERE TAGNO = '" & txtTagNo.Text & "' AND ISNULL(ISSDATE,'')='' "
            'strSql += " AND TAGNO NOT IN(SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAGPKTNO)"
            'If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            'If cnCostId <> "" Then strSql += " AND COSTID = '" & cnCostId & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTagDet)
            If dtGrid Is Nothing Then dtGrid = dtTagDet.Clone
            If dtTagDet.Rows.Count > 0 Then
                With gridView
                    If txtTagNo.Text <> "" Then
                        dtGrid.Merge(dtTagDet)
                    Else
                        dtGrid = dtTagDet
                    End If
                    .DataSource = dtGrid

                    .Columns("ITEMID").Visible = False
                    .Columns("SNO").Visible = False
                    .Columns("ITEM").Width = 180
                    .Refresh()
                    txtTagNo.Clear()
                End With
                btnGenerate.Enabled = True
            End If
        End If

    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        If Not gridView.RowCount > 0 Then
            txtTagNo.Focus()
        End If
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        ElseIf e.KeyCode = Keys.Escape Then
            btnGenerate.Focus()
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentCell.RowIndex).Cells("TAGNO")
            Else
                txtTagNo.Focus()
                Exit Sub
            End If
        End If

    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        fNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        If gridView.RowCount <= 0 Then Exit Sub
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        Try
            Dim PacketNo As Integer
            strSql = "SELECT ISNULL(MAX(PACKETNO),0) FROM " & cnAdminDb & "..ITEMTAGPKTNO"
            PacketNo = Val(GetSqlValue(cn, strSql)) + 1
            For I As Integer = 0 To gridView.RowCount - 1
                Dim Tagno As String = gridView.Rows(I).Cells("TAGNO").Value.ToString
                If Tagno <> "" Then
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGPKTNO("
                    strSql += " PACKETNO,TAGNO,USERID,SYSTEMID,APPVER,UPDATED,UPTIME)"
                    strSql += " VALUES "
                    strSql += " ( "
                    strSql += " " & PacketNo & "" 'USERID
                    strSql += " ,'" & Tagno & "'" 'TAGNO
                    strSql += " ," & userId & "" 'USERID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & DateTime.Now.Date.ToString() & "'" 'UPDATED
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += " ) "
                    Cmd = New OleDbCommand(strSql, cn)
                    Cmd.ExecuteNonQuery()
                End If
            Next
            MsgBox("PacketNo " & PacketNo & " Generated", MsgBoxStyle.Information)

            Dim roSelected() As DataRow = CType(gridView.DataSource, DataTable).Select
            Dim oldItem As Integer = Nothing
            Dim paramStr As String = ""
            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Dim write As StreamWriter
            Dim memfile As String = "\Barcodeprint" & prnmemsuffix & ".mem"
            write = IO.File.CreateText(Application.StartupPath & memfile)
            For Each ro As DataRow In roSelected
                If oldItem <> Val(ro!itemid.ToString) Then
                    write.WriteLine(LSet("PROC", 7) & ":" & ro!ITEMID.ToString)
                    paramStr += LSet("PROC", 7) & ":" & ro!ITEMID.ToString & ";"
                    oldItem = Val(ro!itemid.ToString)
                End If
                write.WriteLine(LSet("TAGNO", 7) & ":" & ro!TAGNO.ToString)
                paramStr += LSet("TAGNO", 7) & ":" & ro!TAGNO.ToString & ";"
            Next
            If paramStr.EndsWith(";") Then
                paramStr = Mid(paramStr, 1, paramStr.Length - 1)
            End If
            write.Flush()
            write.Close()
            If EXE_WITH_PARAM = False Then
                If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe")
                Else
                    MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                End If
            Else
                If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe", paramStr)
                Else
                    MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                End If
            End If
            fNew()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        fNew()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        fNew()
        Label2.Visible = True
        txtpocketno.Visible = True
        txtpocketno.Focus()
    End Sub

    Private Sub btndelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btndelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..ITEMTAGPKTNO WHERE 1<>1"
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("PACKETNO").Value.ToString
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..ITEMTAGPKTNO WHERE PACKETNO = '" & delKey & "'")
        btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub txtpocketno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtpocketno.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            btn_search.Focus()
        End If
        '    Dim dt As New DataTable
        '    strSql = " SELECT TAGNO, PACKETNO  "
        '    strSql += " FROM " & cnAdminDb & "..ITEMTAGPKTNO  "
        '    If txtpocketno.Text <> "" Then strSql += " WHERE PACKETNO = '" & txtpocketno.Text.ToString() & "'"
        '    da = New OleDbDataAdapter(strSql, cn)
        '    da.Fill(dt)
        '    If dt.Rows.Count > 0 Then
        '        With gridView
        '            .DataSource = dt
        '            .Refresh()
        '            txtpocketno.Clear()
        '        End With
        '        btnGenerate.Enabled = False
        '    End If
        '    txtpocketno.Visible = False
        '    Label2.Visible = False
        'End If
    End Sub

    Private Sub txtpocketno_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtpocketno.Leave
        'If txtpocketno.Text = "" Then
        '    Dim dt As New DataTable
        '    strSql = " SELECT TAGNO,PACKETNO  "
        '    strSql += " FROM " & cnAdminDb & "..ITEMTAGPKTNO  "
        '    If txtpocketno.Text <> "" Then strSql += " WHERE PACKETNO = '" & txtpocketno.Text.ToString() & "'"
        '    da = New OleDbDataAdapter(strSql, cn)
        '    da.Fill(dt)
        '    If dt.Rows.Count > 0 Then
        '        With gridView
        '            .DataSource = dt
        '            .Refresh()
        '            txtpocketno.Clear()
        '        End With
        '        btnGenerate.Enabled = False
        '    End If
        '    txtpocketno.Visible = False
        '    Label2.Visible = False
        'End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btn_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_search.Click
        Dim dt As New DataTable
        strSql = " SELECT TAGNO, PACKETNO  "
        strSql += " FROM " & cnAdminDb & "..ITEMTAGPKTNO  "
        If txtpocketno.Text <> "" Then strSql += " WHERE PACKETNO = '" & txtpocketno.Text.ToString() & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With gridView
                .DataSource = dt
                .Refresh()
                txtpocketno.Clear()
            End With
            btnGenerate.Enabled = False
        End If
        txtpocketno.Visible = False
        Label2.Visible = False
        btndelete.Enabled = True
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            txtTagNo.Visible = False
            Panel2.Visible = True
            txtfromtag.Focus()
        Else
            txtTagNo.Visible = True
            Panel2.Visible = False
            txtTagNo.Focus()
        End If
    End Sub

    Private Sub txttotag_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txttotag.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtfromtag.Text = "" Or txttotag.Text = "" Then
                MsgBox("TagNo should not empty", MsgBoxStyle.Information)
                txtfromtag.Focus()
                Exit Sub
            End If
            For Each ro As DataRow In dtGrid.Rows
                If ro("TAGNO").ToString = txtfromtag.Text Or ro("TAGNO").ToString = txttotag.Text Then
                    MsgBox("TagNo Already Loaded", MsgBoxStyle.Information)
                    txtfromtag.Clear()
                    txttotag.Clear()
                    txtfromtag.Focus()
                    Exit Sub
                End If
            Next
            dtTagDet.Rows.Clear()
            strSql = vbCrLf + "	  IF (SELECT TOP 1 1 FROM  TEMPTABLEDB..SYSOBJECTS WHERE NAME ='TEMPPACKNOGEN')>0 DROP TABLE  TEMPTABLEDB..TEMPPACKNOGEN"
            Cmd = New OleDbCommand(strSql, cn)
            Cmd.ExecuteNonQuery()
            strSql = " SELECT IDENTITY(int, 1,1) AS SNO, "
            strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += " ,TAGNO,PCS,NETWT,GRSWT,ITEMID INTO TEMPTABLEDB..TEMPPACKNOGEN FROM " & cnAdminDb & "..ITEMTAG AS T WHERE TAGNO"
            strSql += "  BETWEEN CONVERT(integer,'" & txtfromtag.Text & "') AND CONVERT(INTEGER,'" & txttotag.Text & "') "
            strSql += "  AND ISNULL(ISSDATE,'')='' "
            strSql += " AND TAGNO NOT IN(SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAGPKTNO)"
            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            If cnCostId <> "" Then strSql += " AND COSTID = '" & cnCostId & "'"
            Cmd = New OleDbCommand(strSql, cn)
            Cmd.ExecuteNonQuery()
            strSql = "SELECT TOP 1 1 FROM  TEMPTABLEDB..TEMPPACKNOGEN"
            cnt = Val(GetSqlValue(cn, strSql))
            If (cnt > 0) Then
                strSql = " INSERT INTO TEMPTABLEDB..TEMPPACKNOGEN (ITEM,PCS,NETWT,GRSWT)"
                strSql += "SELECT 'TOTAL'ITEM,SUM(PCS)PCS,SUM(NETWT)NETWT,SUM(GRSWT)GRSWT FROM TEMPTABLEDB..TEMPPACKNOGEN "
                Cmd = New OleDbCommand(strSql, cn)
                Cmd.ExecuteNonQuery()
            End If
            strSql = " SELECT SNO,ITEM,TAGNO,PCS,NETWT,GRSWT,ITEMID FROM TEMPTABLEDB..TEMPPACKNOGEN ORDER BY SNO"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTagDet)
            If dtGrid Is Nothing Then dtGrid = dtTagDet.Clone
            If dtTagDet.Rows.Count > 0 Then
                With gridView
                    If txtfromtag.Text <> "" Then
                        dtGrid.Merge(dtTagDet)
                    Else
                        dtGrid = dtTagDet
                    End If
                    .DataSource = dtGrid
                    .Columns("ITEMID").Visible = False
                    .Columns("SNO").Width = 50
                    .Columns("ITEM").Width = 180
                    .Refresh()
                    txtfromtag.Clear()
                End With
                btnGenerate.Enabled = True
            End If
            btnGenerate.Focus()
            Panel2.Visible = False
        End If
    End Sub

    Private Sub txtfromtag_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtfromtag.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txttotag.Focus()
        End If
    End Sub
End Class