Imports System.Data.OleDb
Public Class frmCentRateUpdate
    Dim strSql As String
    Dim da As New OleDbDataAdapter
    Dim cmd As New OleDbCommand
    Dim TAGONLY As Boolean = True
    Dim WEBTAGONLY As Boolean = False
    Private Sub chkItemAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkItemSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItem, chkItemSelectAll.Checked)
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub frmCentRateUpdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
        'If e.KeyChar = Chr(Keys.Space) Then
        '    SendKeys.Send("{TAB}")
        'End If
        If e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub frmCentRateUpdate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        grpControl.Location = New Point((ScreenWid - grpControl.Width) / 2, ((ScreenHit - 128) - grpControl.Height) / 2)
        strSql = "select 1 from " & cnAdminDb & "..PRJMENUS WHERE MENUID = 'CentRateUpdateTStrip'"
        If objGPack.GetSqlValue(strSql) > 0 Then chkTag.Visible = True : chkWebTag.Visible = True Else chkTag.Visible = False : chkWebTag.Visible = False : TAGONLY = True
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        strSql += vbCrLf + "  WHERE ACTIVE = 'Y' AND ISNULL(STUDDED,'') = 'S'"
        strSql += vbCrLf + "  ORDER BY ITEMNAME"
        FillCheckedListBox(strSql, chkLstItem)
        SetChecked_CheckedList(chkLstItem, chkItemSelectAll.Checked)
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub chkLstItem_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstItem.Leave
        If Not chkLstItem.SelectedItems.Count > 0 Then chkItemSelectAll.Checked = True
    End Sub
    Private Sub chkLstItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstItem.LostFocus
        Dim chkItemName As String = GetChecked_CheckedList(chkLstItem)
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
        strSql += vbCrLf + "  WHERE ACTIVE = 'Y'"
        If chkItemName <> "" Then strSql += vbCrLf + "  AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
        strSql += vbCrLf + "  ORDER BY SUBITEMNAME"
        FillCheckedListBox(strSql, chkLstSubItem)
        SetChecked_CheckedList(chkLstSubItem, chkSubItemSelectAll.Checked)
    End Sub
    Private Sub chkSubItemSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSubItemSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstSubItem, chkSubItemSelectAll.Checked)
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        chkItemSelectAll.Select()
        chkItemSelectAll.Checked = False
        chkLstSubItem.Items.Clear()
        chkSubItemSelectAll.Checked = False
        rbtMaxValue.Checked = True
        chkItemSelectAll.Select()
    End Sub
    Private Sub chkLstSubItem_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstSubItem.Leave
        If Not chkLstSubItem.SelectedItems.Count > 0 Then chkSubItemSelectAll.Checked = True
    End Sub
    Private Sub chkTag_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkTag.CheckStateChanged
        TAGONLY = chkTag.Checked
        If chkTag.Checked = True Then
            chkWebTag.Enabled = False
        Else
            chkWebTag.Enabled = True
        End If

    End Sub
    Private Sub chkWebTag_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkWebTag.CheckStateChanged
        WEBTAGONLY = chkWebTag.Checked
        If chkWebTag.Checked = True Then
            chkTag.Enabled = False
        Else
            chkTag.Enabled = True
        End If
    End Sub
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        funcView()
    End Sub
    Function funcView() As Integer
        Dim chkItemName As String = GetChecked_CheckedList(chkLstItem)
        Dim chkSubItemName As String = GetChecked_CheckedList(chkLstSubItem)
        If Not TAGONLY And Not WEBTAGONLY Then MsgBox("Atleast One select as Taged or Webtag") : Exit Function
        If TAGONLY Then
            strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPTAGSTONE')>0 DROP TABLE TEMPTABLEDB..TEMPTAGSTONE"
            strSql += vbCrLf + " SELECT SNO,ITEMNAME,SUBITEMNAME"
            strSql += vbCrLf + " ,STNITEMID,STNSUBITEMID,STNPCS,STNWT,CENTWT,STNAMT,CALCMODE,STONEUNIT"
            strSql += vbCrLf + " ,COLORID,CLARITYID,SHAPEID,TAGNO,COLOR,CLARITY,SHAPE,STNRATE"
            strSql += vbCrLf + " ,(SELECT TOP 1 " & IIf(rbtMaxValue.Checked, "MAXRATE", "MINRATE") & " FROM " & cnAdminDb & "..CENTRATE C WHERE C.ITEMID = X.STNITEMID "
            strSql += vbCrLf + "  AND C.SUBITEMID = X.STNSUBITEMID AND ISNULL(C.COLORID,0) = ISNULL(X.COLORID,0) "
            strSql += vbCrLf + "  AND ISNULL(C.CLARITYID,0) = ISNULL(X.CLARITYID,0) "
            strSql += vbCrLf + "  AND ISNULL(C.SHAPEID,0) = ISNULL(X.SHAPEID,0) "
            strSql += vbCrLf + "  AND ISNULL(C.COSTID,'') = ISNULL(X.COSTID,'') "
            strSql += vbCrLf + "  AND X.CENTWT BETWEEN C.FROMCENT AND C.TOCENT "
            strSql += vbCrLf + "  AND ISNULL(C.ACCODE,'') = '')AS CENTRATE"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)AS STNVALUE"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTAGSTONE"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT SNO"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=STNITEMID) AS ITEMNAME"
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=STNSUBITEMID) AS SUBITEMNAME"
            strSql += vbCrLf + " ,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNRATE,STNAMT,CALCMODE,S.STONEUNIT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),CASE WHEN S.STONEUNIT = 'C' THEN "
            strSql += vbCrLf + " (STNWT / CASE WHEN STNPCS = 0 OR ISNULL(SM.CALTYPE,IM.CALTYPE) = 'D' THEN 1 ELSE STNPCS END)*100"
            strSql += vbCrLf + " ELSE"
            strSql += vbCrLf + "  STNWT / CASE WHEN STNPCS = 0 OR ISNULL(SM.CALTYPE,IM.CALTYPE) = 'D' THEN 1 ELSE STNPCS END"
            strSql += vbCrLf + " END) AS CENTWT"
            strSql += vbCrLf + " ,ISNULL(COLORID,0)COLORID,ISNULL(CLARITYID,0)CLARITYID , ISNULL(SHAPEID,0)SHAPEID"
            strSql += vbCrLf + " ,S.TAGNO"
            strSql += vbCrLf + " ,(SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID=S.COLORID)COLOR"
            strSql += vbCrLf + " ,(SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID=S.CLARITYID)CLARITY"
            strSql += vbCrLf + " ,(SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID=S.SHAPEID)SHAPE"
            strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=S.COSTID)COSTNAME"
            strSql += vbCrLf + " ,S.COSTID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE S"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..SUBITEMMAST SM ON S.STNSUBITEMID = SM.SUBITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..ITEMMAST IM ON S.STNITEMID = IM.ITEMID"
            strSql += vbCrLf + " WHERE EXISTS (SELECT 1 FROM " & cnAdminDb & "..CENTRATE C WHERE C.ITEMID = S.STNITEMID AND C.SUBITEMID = S.STNSUBITEMID AND ISNULL(C.COSTID,'') = ISNULL(S.COSTID,'') AND ISNULL(C.COLORID,0) = ISNULL(S.COLORID,0) AND ISNULL(C.CLARITYID,0) = ISNULL(S.CLARITYID,0) AND ISNULL(C.SHAPEID,0) = ISNULL(S.SHAPEID,0)"
            If chkItemName <> "" Then strSql += vbCrLf + " AND C.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
            If chkSubItemName <> "" Then strSql += vbCrLf + " AND (C.SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & chkSubItemName & ")) OR C.SUBITEMID = 0)"
            strSql += vbCrLf + " )"
            If strBCostid <> Nothing Then strSql += " AND ISNULL(S.COSTID,'') ='" & strBCostid & "'"
            strSql += vbCrLf + " AND TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ISSDATE IS NULL AND DESIGNERID NOT IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNERVA ))"
            strSql += vbCrLf + " )X"
            If Val(txtFromCent_WET.Text) > 0 And Val(txtToCent_WET.Text) > 0 Then
                strSql += vbCrLf + " WHERE CENTWT BETWEEN " & Val(txtFromCent_WET.Text) & " AND " & Val(txtToCent_WET.Text)
            End If
        End If

        If WEBTAGONLY Then
            strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPTAGSTONE')>0 DROP TABLE TEMPTABLEDB..TEMPTAGSTONE"
            strSql += vbCrLf + " SELECT *"
            strSql += vbCrLf + " ,(SELECT TOP 1 " & IIf(rbtMaxValue.Checked, "MAXRATE", "MINRATE") & " FROM " & cnAdminDb & "..CENTRATE C WHERE C.ITEMID = X.STNITEMID AND C.SUBITEMID = X.STNSUBITEMID AND ISNULL(C.COLORID,0) = ISNULL(X.COLORID,0) AND ISNULL(C.CLARITYID,0) = ISNULL(X.CLARITYID,0) AND ISNULL(C.SHAPEID,0) = ISNULL(X.SHAPEID,0) AND X.CENTWT BETWEEN C.FROMCENT AND C.TOCENT AND ISNULL(C.ACCODE,'') = '')AS CENTRATE"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)AS STNVALUE"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTAGSTONE"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT SNO"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=STNITEMID) AS ITEMNAME"
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=STNSUBITEMID) AS SUBITEMNAME"
            strSql += vbCrLf + " ,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNRATE,STNAMT,CALCMODE,S.STONEUNIT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),CASE WHEN S.STONEUNIT = 'C' THEN "
            strSql += vbCrLf + " (STNWT / CASE WHEN STNPCS = 0 OR ISNULL(SM.CALTYPE,IM.CALTYPE) = 'D' THEN 1 ELSE STNPCS END)*100"
            strSql += vbCrLf + " ELSE"
            strSql += vbCrLf + "  STNWT / CASE WHEN STNPCS = 0 OR ISNULL(SM.CALTYPE,IM.CALTYPE) = 'D' THEN 1 ELSE STNPCS END"
            strSql += vbCrLf + " END) AS CENTWT,ISNULL(COLORID,0)COLORID,ISNULL(CLARITYID,0)CLARITYID , ISNULL(SHAPEID,0)SHAPEID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..WITEMTAGSTONE S"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..SUBITEMMAST SM ON S.STNSUBITEMID = SM.SUBITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..ITEMMAST IM ON S.STNITEMID = IM.ITEMID"
            strSql += vbCrLf + " WHERE EXISTS (SELECT 1 FROM " & cnAdminDb & "..CENTRATE C WHERE C.ITEMID = S.STNITEMID AND C.SUBITEMID = S.STNSUBITEMID AND ISNULL(C.COSTID,'') = ISNULL(S.COSTID,'') AND ISNULL(C.COLORID,0) = ISNULL(S.COLORID,0) AND ISNULL(C.CLARITYID,0) = ISNULL(S.CLARITYID,0) AND ISNULL(C.SHAPEID,0) = ISNULL(S.SHAPEID,0)"
            If chkItemName <> "" Then strSql += vbCrLf + " AND C.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
            If chkSubItemName <> "" Then strSql += vbCrLf + " AND (C.SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & chkSubItemName & ")) OR C.SUBITEMID = 0)"
            strSql += vbCrLf + " )"
            If strBCostid <> Nothing Then strSql += " AND ISNULL(S.COSTID,'') ='" & strBCostid & "'"
            strSql += vbCrLf + " AND TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..WITEMTAG WHERE ISSDATE IS NULL)"
            strSql += vbCrLf + " )X"
        End If
        'funcOpenGrid(strSql, gridView)
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = "SELECT * FROM TEMPTABLEDB..TEMPTAGSTONE"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtgrid As New DataTable
        da.Fill(dtgrid)
        If dtgrid.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dtgrid
            With gridView
                .Columns("ITEMNAME").HeaderText = "ITEMNAME"
                .Columns("ITEMNAME").Width = 175
                .Columns("SUBITEMNAME").HeaderText = "SUBITEM NAME"
                .Columns("SUBITEMNAME").Width = 165
                .Columns("STNWT").Width = 100
                .Columns("STNRATE").HeaderText = "OLD RATE"
                .Columns("STNRATE").Width = 100
                .Columns("CENTRATE").HeaderText = "NEW RATE"
                .Columns("CENTRATE").Width = 100
                .Columns("CENTRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CENTWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("TAGNO") Then .Columns("TAGNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SNO").Visible = False
                .Columns("STNITEMID").Visible = False
                .Columns("STNSUBITEMID").Visible = False
                .Columns("STNAMT").Visible = False
                .Columns("CALCMODE").Visible = False
                .Columns("STONEUNIT").Visible = False
                .Columns("COLORID").Visible = False
                .Columns("CLARITYID").Visible = False
                .Columns("SHAPEID").Visible = False
                .Columns("STNVALUE").Visible = False
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                tabMain.SelectedTab = tabView
                .Focus()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End With
        Else
            gridView.DataSource = Nothing
            MsgBox("Record Not Found.", MsgBoxStyle.Information)
        End If
        'tabMain.SelectedTab = tabView
        'gridView.Select()
    End Function

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If TAGONLY Then
            strSql = " DELETE FROM TEMPTABLEDB..TEMPTAGSTONE WHERE ISNULL(CENTRATE,0) = 0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " UPDATE TEMPTABLEDB..TEMPTAGSTONE SET STNVALUE = CASE WHEN CALCMODE = 'P' THEN CENTRATE*STNPCS ELSE CENTRATE*STNWT END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET STNRATE = C.CENTRATE,STNAMT = C.STNVALUE"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAGSTONE AS T INNER JOIN TEMPTABLEDB..TEMPTAGSTONE C ON T.SNO = C.SNO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        If WEBTAGONLY Then
            strSql = " DELETE FROM TEMPTABLEDB..TEMPTAGSTONE WHERE ISNULL(CENTRATE,0) = 0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " UPDATE TEMPTABLEDB..TEMPTAGSTONE SET STNVALUE = CASE WHEN CALCMODE = 'P' THEN CENTRATE*STNPCS ELSE CENTRATE*STNWT END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " UPDATE " & cnAdminDb & "..WITEMTAGSTONE SET STNRATE = C.CENTRATE,STNAMT = C.STNVALUE"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..WITEMTAGSTONE AS T INNER JOIN TEMPTABLEDB..TEMPTAGSTONE C ON T.SNO = C.SNO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        MsgBox("Updated Successfully", MsgBoxStyle.Information)
        btnBack_Click(Me, New EventArgs)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Dim lbltitle1 As String = "Cent Rate Details "
        Dim formattedDate As String = DateTime.Now.ToString("dd/MM/yyyy  hh:mm:ss tt")
        Dim lbltitle = lbltitle1 + formattedDate
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub
End Class