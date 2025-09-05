Imports System.Data.OleDb
Public Class frmReprocess
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtGridView As New DataTable
    Dim CTRITEMTRF As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ISCTRITEMTRF'", , , tran)

    Dim WithEvents lstSearch As New ListBox
    Dim searchSender As Control = Nothing
    Dim listItem As New List(Of String)
    Dim listCounter As New List(Of String)
    Dim tempText As New TextBox
    Dim dtSummary As New DataTable
    '.Columns.Add("WASTAGEPER", GetType(Decimal))



    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click

        tabMain.SelectedTab = tabGeneral
        dtpFrom.Select()
        btnSearch.Enabled = True
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        If lstSearch.Visible And (Not searchSender Is Nothing) Then
            searchSender.Select()
            lstSearch.Visible = False
            txtGrid.Visible = False
        End If
        gbSummary.Visible = False
        dtpFrom.Focus()
    End Sub

    Private Sub frmPend_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                If gbSummary.Visible = True Then
                    gbSummary.Visible = False
                    gridView.Focus()
                    Exit Sub
                End If
                If lstSearch.Visible And (Not searchSender Is Nothing) Then
                    searchSender.Select()
                    lstSearch.Visible = False
                    txtGrid.Visible = False
                    Exit Sub
                End If

                'If txtGrid.Focused Then
                '    btnTransfer.Focus()
                'End If
                btnBack_Click(Me, New EventArgs)
            End If
        ElseIf e.KeyCode = Keys.S Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                If gbSummary.Visible = False Then
                    Call Trfsummary()
                    Exit Sub
                End If

            End If
        End If
    End Sub

    Private Sub frmPend_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            If lstSearch.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub frmPend_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre_MAN.Enabled = True
            strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre_MAN, , False)
        Else
            cmbCostCentre_MAN.Text = ""
            cmbCostCentre_MAN.Enabled = False
        End If
        gridView.RowTemplate.Height = 21
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect

        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRNAME"
        'cmbToCounter.Items.Clear()
        'cmbToCounter.Items.Add("")
        'objGPack.FillCombo(strSql, cmbToCounter, False, False)

        Dim dtcounter As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcounter)
        listCounter.Clear()
        For Each dr As DataRow In dtcounter.Rows
            listCounter.Add(dr!ITEMCTRNAME.ToString)
        Next


        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE ='N' ORDER BY ITEMNAME"
        'cmbToitem.Items.Clear()
        'cmbToitem.Items.Add("")
        'objGPack.FillCombo(strSql, cmbToitem, False, False)

        Dim dtitem As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtitem)
        listItem.Clear()
        For Each dr As DataRow In dtitem.Rows
            listItem.Add(dr!ITEMNAME.ToString)
        Next
        txtGrid.Visible = False
        lstSearch.Visible = False

        ''Load Metal
        cmbOpenMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbOpenMetal, False)
        cmbOpenMetal.Text = "ALL"

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        'lblCtr1.Visible = rbtCtrstk.Checked : lblCtr2.Visible = rbtCtrstk.Checked : cmbToitem.Visible = rbtCtrstk.Checked : cmbToCounter.Visible = rbtCtrstk.Checked
        If cmbCostCentre_MAN.Items.Count > 0 And cmbCostCentre_MAN.Text.Trim = "" Then MsgBox("Cost centre should not empty", MsgBoxStyle.Information) : cmbCostCentre_MAN.Focus() : Exit Sub
        gridView.DataSource = Nothing
        lstSearch.Visible = False
        txtGrid.Visible = False
        btnSearch.Enabled = False
        Me.Refresh()
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PENDTRANS') > 0 DROP TABLE TEMP" & systemId & "PENDTRANS"
        strSql += vbCrLf + " SELECT T.SNO,T.TRANDATE,T.TRANNO,T.TRANTYPE,T.CATCODE,T.METALID,T.ITEMID,T.TAGPCS-T.PCS PCS,T.TAGGRSWT-T.GRSWT GRSWT,T.TAGNETWT-T.NETWT NETWT ,T.TAGPCS,T.TAGGRSWT,T.TAGNETWT,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT,CONVERT(INTEGER,NULL) NEWITEMID ,CONVERT(INTEGER,NULL) NEWCTRID "
        strSql += vbCrLf + " INTO TEMP" & systemId & "PENDTRANS"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE T "
        'strSql += vbCrLf + " left join " & cnStockDb & "..ISSSTONE STN  ON T.SNO=STN.ISSSNO"
        strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND T.TRANTYPE IN('SA','OD') AND ISNULL(T.CANCEL,'') = ''"
        strSql += vbCrLf + " AND (T.TAGPCS <> T.PCS OR T.TAGGRSWT <> T.GRSWT OR T.TAGNETWT <> T.NETWT)" ' OR STN.TAGSTNWT <> STN.STNWT)"
        strSql += vbCrLf + " AND (T.TAGPCS <> 0 OR T.TAGGRSWT <> 0 OR T.TAGNETWT <> 0)"
        If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
        If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(T.COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        strSql += vbCrLf + " AND T.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT T.SNO,T.TRANDATE,T.TRANNO,TRANTYPE,T.CATCODE,T.METALID,T.ITEMID,T.PCS,T.GRSWT,T.NETWT,T.TAGPCS,T.TAGGRSWT,T.TAGNETWT,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT,CONVERT(INTEGER,NULL) NEWITEMID ,CONVERT(INTEGER,NULL) NEWCTRID "
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT T "
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND TRANTYPE IN ('SR','PU')"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
        If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PENDTRANSSTUD_IS') > 0 DROP TABLE TEMP" & systemId & "PENDTRANSSTUD_IS"
        strSql += vbCrLf + " SELECT ISSSNO SNO,TRANDATE,CATCODE,STNITEMID ,TAGSTNPCS-STNPCS STNPCS,TAGSTNWT - STNWT STNWT,CONVERT(DECIMAL,NULL) DIAPCS,CONVERT(DECIMAL,NULL) DIAWT "
        strSql += vbCrLf + " INTO TEMP" & systemId & "PENDTRANSSTUD_IS"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE I"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' AND TRANTYPE = 'SA'  AND ISNULL(BAGNO,'') = '')"
        strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN('S','P'))"
        strSql += vbCrLf + " AND (TAGSTNPCS <> STNPCS OR TAGSTNWT <> STnWT)"
        strSql += vbCrLf + " AND (TAGSTNPCS <> 0 OR TAGSTNWT <> 0)"
        strSql += vbCrLf + " AND (TAGSTNPCS - STNPCS <> 0 OR TAGSTNWT - STNWT <> 0)"
        If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT ISSSNO SNO,TRANDATE,CATCODE,STNITEMID ,0 STNPCS,0 STNWT,TAGSTNPCS-STNPCS DIAPCS,TAGSTNWT - STNWT DIAWT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE I"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' AND TRANTYPE = 'SA'  AND ISNULL(BAGNO,'') = '')"
        strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN('D'))"
        strSql += vbCrLf + " AND (TAGSTNPCS <> STNPCS OR TAGSTNWT <> STnWT)"
        strSql += vbCrLf + " AND (TAGSTNPCS <> 0 OR TAGSTNWT <> 0)"
        strSql += vbCrLf + " AND (TAGSTNPCS - STNPCS <> 0 OR TAGSTNWT - STNWT <> 0)"
        If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"


        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()



        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PENDTRANSSTUD_RE') > 0 DROP TABLE TEMP" & systemId & "PENDTRANSSTUD_RE"
        strSql += vbCrLf + " SELECT TRANDATE,ISSSNO SNO,STNITEMID,CATCODE,sum(STNPCS) STNPCS,sum(STNWT) STNWT,CONVERT(DECIMAL,NULL) DIAPCS,CONVERT(DECIMAL,NULL) DIAWT "
        strSql += vbCrLf + " INTO TEMP" & systemId & "PENDTRANSSTUD_RE"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE R"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND ISSSNO IN (sELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND TRANTYPE IN ('SR','PU') AND ISNULL(BAGNO,'') = '' )"
        strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN ('S','P'))"
        If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " group by TRANDATE,ISSSNO ,STNITEMID,CATCODE"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT TRANDATE,ISSSNO SNO,STNITEMID,CATCODE,0 STNPCS ,0 STNWT,sum(STNPCS) DIAPCS,sum(STNWT) DIAWT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE R"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND ISSSNO IN (sELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND TRANTYPE IN ('SR','PU') AND ISNULL(BAGNO,'') = '' )"
        strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN ('D'))"
        If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " group by TRANDATE,ISSSNO ,STNITEMID,CATCODE"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = " "
  

        strSql = " UPDATE A SET A.DIAPCS = B.DIAPCS,A.DIAWT = B.DIAWT,A.STNPCS=B.STNPCS,A.STNWT = B.STNWT FROM TEMP" & systemId & "PENDTRANS A INNER JOIN TEMP" & systemId & "PENDTRANSSTUD_IS B ON A.SNO = B.SNO"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()


        strSql = " UPDATE A SET A.DIAPCS = B.DIAPCS,A.DIAWT = B.DIAWT,A.STNPCS=B.STNPCS,A.STNWT = B.STNWT FROM TEMP" & systemId & "PENDTRANS A INNER JOIN TEMP" & systemId & "PENDTRANSSTUD_RE B ON A.SNO = B.SNO"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        If CTRITEMTRF = "Y" Then
            Dim dtitem As New DataTable
            strSql = "select DISTINCT METALID,TRANTYPE FROM TEMP" & systemId & "PENDTRANS"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtitem)
            For ij As Integer = 0 To dtitem.Rows.Count - 1
                Dim mctrlid As String = ""
                If dtitem.Rows(ij).Item("TRANTYPE").ToString = "PU" Then
                    mctrlid = "PURCHASE-" & dtitem.Rows(ij).Item("METALID").ToString
                ElseIf dtitem.Rows(ij).Item("TRANTYPE").ToString = "PS" Then
                    mctrlid = "PARTLY-" & dtitem.Rows(ij).Item("METALID").ToString
                Else
                    mctrlid = "RETURN-" & dtitem.Rows(ij).Item("METALID").ToString
                End If
                Dim Mctrid As Integer = Val(objGPack.GetSqlValue("SELECT DEFAULTCOUNTER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID IN (SELECT TOP 1 CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='" & mctrlid & "')").ToString)
                If Mctrid <> 0 Then
                    Dim itemIds As String = objGPack.GetSqlValue("SELECT POS_ITEMID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = " & Mctrid).ToString
                    Dim Itemidarry() As String = Split(itemIds, ",")
                    Dim itemIdsr As Integer = Val(Itemidarry(1).ToString())
                    Dim itemIdps As Integer = Val(Itemidarry(0).ToString())
                    Dim itemidpu As Integer = Val(Itemidarry(2).ToString())
                    strSql = " UPDATE TEMP" & systemId & "PENDTRANS SET NEWITEMID=" & itemIdsr
                    strSql += vbCrLf + "  where METALID = '" & dtitem.Rows(ij).Item("METALID").ToString & "' AND TRANTYPE = 'SR' AND ISNULL(NEWITEMID,0) = 0 "
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "PENDTRANS SET NEWITEMID=" & itemIdps
                    strSql += vbCrLf + "  where METALID = '" & dtitem.Rows(ij).Item("METALID").ToString & "' AND TRANTYPE = 'PS' AND ISNULL(NEWITEMID,0) = 0 "
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "PENDTRANS SET NEWITEMID=" & itemidpu
                    strSql += vbCrLf + "  where METALID = '" & dtitem.Rows(ij).Item("METALID").ToString & "' AND TRANTYPE = 'PU' AND ISNULL(NEWITEMID,0) = 0 "
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                Else
                    strSql = " UPDATE TEMP" & systemId & "PENDTRANS SET NEWITEMID=(SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID=(CASE WHEN T.TRANTYPE='PU' THEN 'PURCHASE-' WHEN T.TRANTYPE='SR' THEN 'RETURN-' ELSE 'PARTLY-' END + T.METALID))"
                    strSql += vbCrLf + " FROM TEMP" & systemId & "PENDTRANS AS T where METALID = '" & dtitem.Rows(ij).Item("METALID").ToString & "' AND TRANTYPE = '" & dtitem.Rows(ij).Item("TRANTYPE").ToString & "' AND ISNULL(NEWITEMID,0) = 0 "
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                End If
            Next
        Else
            strSql = " UPDATE TEMP" & systemId & "PENDTRANS SET NEWITEMID=(SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID=(CASE WHEN T.TRANTYPE='PU' THEN 'PURCHASE-' WHEN T.TRANTYPE='SR' THEN 'RETURN-' ELSE 'PARTLY-' END + T.METALID))"
            strSql += vbCrLf + " FROM TEMP" & systemId & "PENDTRANS AS T "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If


        
        strSql = " SELECT T.SNO,CASE WHEN T.TRANTYPE='PU' THEN 'PU' WHEN T.TRANTYPE='SR' THEN 'SR' ELSE 'PS' END AS TRANTYPE"
        strSql += vbCrLf + " ,T.TRANDATE,T.TRANNO,T.ITEMID,ISNULL(IM.ITEMNAME,IC.CATNAME) AS DESCRIPTION,SUM(T.PCS)PCS"
        strSql += vbCrLf + " ,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.DIAPCS)DIAPCS,SUM(T.DIAWT) AS DIAWT"
        strSql += vbCrLf + " ,SUM(T.STNPCS)STNPCS,SUM(T.STNWT) AS STNWT,NEWITEMID,IMM.ITEMNAME NEWITEM,IMM.DEFAULTCOUNTER AS NEWCTRID,NC.ITEMCTRNAME NEWCOUNTER"
        strSql += vbCrLf + " FROM TEMP" & systemId & "PENDTRANS AS T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID = IM.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IMM ON T.NEWITEMID = IMM.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY IC ON T.CATCODE = IC.CATCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER NC ON IMM.DEFAULTCOUNTER= NC.ITEMCTRID"
        strSql += vbCrLf + " GROUP BY T.SNO,T.TRANTYPE,T.TRANDATE,T.TRANNO,T.ITEMID,IC.CATNAME,IM.ITEMNAME,NEWITEMID,IMM.ITEMNAME,IMM.DEFAULTCOUNTER,NC.ITEMCTRNAME"
        strSql += vbCrLf + " ORDER BY TRANDATE,TRANTYPE,TRANNO,IM.ITEMNAME"

        Try
            Dim dtGrid As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
            dtCol.DefaultValue = True
            dtGrid.Columns.Add(dtCol)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Sub
            End If
            tabMain.SelectedTab = tabView
            gridView.DataSource = dtGrid
            With gridView
                .Columns("CHECK").DisplayIndex = 0
                .Columns("CHECK").HeaderText = ""
                .Columns("CHECK").Width = 0
                .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                .Columns("TRANDATE").Width = 80
                .Columns("TRANNO").Width = 60
                .Columns("TRANTYPE").Width = 35
                .Columns("DESCRIPTION").Width = 120
                .Columns("PCS").Width = 40
                .Columns("GRSWT").Width = 90
                .Columns("NETWT").Width = 90
                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNPCS").Visible = False
                .Columns("STNWT").Width = 60
                .Columns("DIAPCS").Width = 55
                .Columns("DIAWT").Width = 60
                .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                .Columns("NEWITEMID").Visible = False
                .Columns("NEWCTRID").Visible = False
                .Columns("NEWITEM").Width = 120
                .Columns("NEWCOUNTER").Width = 120
                .Columns("SNO").Visible = False
                .Columns("ITEMID").Visible = False
                For cnt As Integer = 0 To .ColumnCount - 1
                    .Columns(cnt).ReadOnly = True
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("CHECK").ReadOnly = True
                .Columns("CHECK").Visible = False
                .Columns("NEWITEM").ReadOnly = False
                .Columns("NEWITEM").HeaderText = "TO ITEM"
                .Columns("NEWCOUNTER").ReadOnly = False
                .Columns("NEWCOUNTER").HeaderText = "TO COUNTER"
                .Columns("TRANTYPE").HeaderText = "TYPE"

                '.ReadOnly = False
            End With
            btnTransfer.Focus()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            btnSearch.Enabled = True
        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransfer.Click

        Dim key As String = Nothing
        Try
            For i As Integer = 0 To gridView.Rows.Count - 1
                Dim NewItemid As Integer
                NewItemid = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & gridView.Rows(i).Cells("NEWITEM").Value.ToString & "' ", "ITEMID", "0")
                gridView.Rows(i).Cells("NEWITEMID").Value = NewItemid
                Dim NewItemCtrid As Integer
                NewItemCtrid = objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & gridView.Rows(i).Cells("NEWCOUNTER").Value.ToString & "' ", "ITEMCTRID", "0")
                gridView.Rows(i).Cells("NEWCTRID").Value = NewItemCtrid
            Next
            Ctrstockupdate()

        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try

    End Sub
    Private Sub Ctrstockupdate()
        Dim dt As New DataTable
        Dim itemId As Integer
        Dim itemCtrId As Integer
        Dim itemdesc As String

        dt = gridView.DataSource
        Dim ros() As DataRow = Nothing
        ros = dt.Select("CHECK = TRUE")
        If Not ros.Length > 0 Then
            MsgBox("There is no checked record", MsgBoxStyle.Information)
            Exit Sub
        End If
        If MessageBox.Show("Do you want to transfer the above items into nontag?", "Transfer Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then Exit Sub
        Try
            Dim costId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")
            Dim metalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbOpenMetal.Text & "'")
            Dim mTotPcs As Decimal = 0
            Dim mTotGwt As Decimal = 0
            Dim mTotNwt As Decimal = 0
            Dim mTotStnwt As Decimal = 0
            Dim mTotStnpcs As Decimal = 0
            Dim mTotDiawt As Decimal = 0
            Dim mTotDiapcs As Decimal = 0
            Dim Recsno As String = ""
            Dim Isssno As String = ""
            'For Each row As DataRow In ros
            '    If row.Item("Trantype").ToString = "PS" Then Isssno += "'" & row.Item("SNO").ToString & "'," Else Recsno += "'" & row.Item("SNO").ToString & "',"
            'Next
            Dim dtitemdet As DataTable = dt.DefaultView.ToTable(True, "TRANDATE", "NEWITEMID", "NEWCTRID")
            Dim trdate As String

            Dim itemchk As Boolean = False
            For II As Integer = 0 To dtitemdet.Rows.Count - 1
                If Val(dtitemdet.Rows(II).Item("NEWITEMID").ToString) <> 0 Then
                    itemchk = True
                    Exit For
                End If
            Next
            If itemchk = False Then MsgBox("Item Should Not Be Empty", MsgBoxStyle.Information) : Exit Sub

            tran = Nothing
            tran = cn.BeginTransaction
            Dim BagNo As String = Nothing
GENBAGNO:
            'BagNo = cnCostId & GetTranDbSoftControlValue("BAGNO", True, tran)
            BagNo = cnCostId & "B" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & GetTranDbSoftControlValue("BAGNO", True, tran)
            ''check
            strSql = " SELECT 'CHECK' FROM " & cnStockDb & "..RECEIPT"
            strSql += vbCrLf + "  WHERE BAGNO = '" & BagNo & "'"
            strSql += " UNION ALL SELECT 'CHECK' FROM " & cnStockDb & "..ISSUE"
            strSql += vbCrLf + "  WHERE BAGNO = '" & BagNo & "'"
            If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
                GoTo GENBAGNO
            End If
            For II As Integer = 0 To dtitemdet.Rows.Count - 1


                itemId = Val(dtitemdet.Rows(II).Item("NEWITEMID").ToString)
                itemCtrId = Val(dtitemdet.Rows(II).Item("NEWCTRID").ToString)
                trdate = CType(dtitemdet.Rows(II).Item("TRANDATE"), Date).ToString("yyyy-MM-dd")
                If itemId = 0 Then Continue For

                'trdate = Format(dtitemdet.Rows(II).Item("TRANDATE").ToString, "yyyy-MM-dd")
                Dim mpcs As Integer = 0
                Dim mgrswt As Decimal = 0
                Dim mnetwt As Decimal = 0
                Dim mdiapcs As Integer = 0
                Dim mstnpcs As Integer = 0
                Dim mdiawt As Decimal = 0
                Dim mstnwt As Decimal = 0
                For Each row As DataRow In ros

                    If row.Item("TRANDATE") = trdate And row.Item("NEWITEMID").ToString = itemId.ToString And row.Item("NEWCTRID").ToString = itemCtrId.ToString Then
                        If row.Item("Trantype").ToString = "PS" Then Isssno += "'" & row.Item("SNO").ToString & "'," Else Recsno += "'" & row.Item("SNO").ToString & "',"
                        mpcs += Val(row.Item("PCS").ToString)
                        mgrswt += Val(row.Item("GRSWT").ToString)
                        mnetwt += Val(row.Item("NETWT").ToString)
                        mdiapcs += Val(row.Item("DIAPCS").ToString)
                        mdiawt += Val(row.Item("DIAWT").ToString)
                        'mdiawt += Val(row.Item("STNWT").ToString)
                        mstnpcs += Val(row.Item("STNPCS").ToString)
                        mstnwt += Val(row.Item("STNWT").ToString)
                    End If

                Next


                Dim tagSno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
                strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAG"
                strSql += " ("
                strSql += " SNO,ITEMID,SUBITEMID,COMPANYID,RECDATE,"
                strSql += " PCS,GRSWT,LESSWT,NETWT,"
                strSql += " FINRATE,ISSTYPE,RECISS,POSTED,"
                strSql += " LOTNO,PACKETNO,DREFNO,ITEMCTRID,"
                strSql += " ORDREPNO,ORSNO,NARRATION,PURWASTAGE,"
                strSql += " PURRATE,PURMC,RATE,COSTID,TCOSTID,"
                strSql += " CTGRM,DESIGNERID,ITEMTYPEID,"
                strSql += " CARRYFLAG,REASON,BATCHNO,CANCEL,"
                strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER)VALUES("
                strSql += " '" & tagSno & "'" 'SNO
                strSql += " ," & itemId & "" 'ITEMID
                strSql += " ,0" 'SUBITEMID
                strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                strSql += " ,'" & CType(trdate, Date).ToString("yyyy-MM-dd") & "'" 'RECDATE
                strSql += " ," & Val(mpcs) & "" 'PCS
                strSql += " ," & Val(mgrswt) & "" 'GRSWT
                strSql += " ," & Val(mgrswt) - Val(mnetwt) & "" 'LESSWT
                strSql += " ," & Val(mnetwt) & "" 'NETWT
                strSql += " ,0" 'FINRATE
                strSql += " ,''" 'ISSTYPE
                strSql += " ,'R'" 'RECISS
                strSql += " ,'U'" 'POSTED   ''FROM Purchase Trans
                strSql += " ,0"
                strSql += " ,'" & BagNo & "'" 'PACKETNO
                strSql += " ,0" 'DREFNO
                strSql += " ," & itemCtrId & "" 'ITEMCTRID
                strSql += " ,''" 'ORDREPNO
                strSql += " ,''" 'ORSNO
                strSql += " ,'TRANS FROM PENDING'" 'NARRATION   ''FROM PENDING TRANSFER
                strSql += " ,0" 'PURWASTAGE
                strSql += " ,0" 'PURRATE
                strSql += " ,0" 'PURMC
                strSql += " ,0" 'RATE
                strSql += " ,'" & costId & "'" 'COSTID
                strSql += " ,'" & costId & "'" 'TCOSTID
                strSql += " ,''" 'CTGRM
                strSql += " ,0" 'DESIGNERID
                strSql += " ,0" 'ITEMTYPEID
                strSql += " ,''" 'CARRYFLAG
                strSql += " ,'0'" 'REASON
                strSql += " ,''" 'BATCHNO
                strSql += " ,''" 'CANCEL
                strSql += " ," & userId & "" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "ITEMNONTAG")

                If Val(mdiapcs) <> 0 Or Val(mdiawt) <> 0 Then
                    'Dim DiaItemId As Integer = 9999 'Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & key + DiaMetalId & "'", , , tran))
                    Dim DiaItemId As Integer = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PARTLY-D'", , , tran).ToString)
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAGSTONE("
                    strSql += " SNO,RECISS,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                    strSql += " STNSUBITEMID,STNPCS,STNWT,"
                    strSql += " STNRATE,STNAMT,DESCRIP,"
                    strSql += " RECDATE,PURRATE,PURAMT,CALCMODE,"
                    strSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
                    strSql += " CARRYFLAG,COSTID,SYSTEMID,APPVER)VALUES("
                    'strSql += " '" & GetSnoFromSoftControl("NONTAGSTONESNO", cnStockDb, "ITEMNONTAGSTONE", tran) & "'" ''SNO
                    strSql += " '" & GetNewSno(TranSnoType.ITEMNONTAGSTONECODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
                    strSql += " ,'R'" ' RECISS
                    strSql += " ,'" & tagSno & "'" 'TAGSNO
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ," & DiaItemId & "" 'STNITEMID
                    strSql += " ,0" 'STNSUBITEMID
                    strSql += " ," & Val(mdiapcs) & "" 'STNPCS
                    strSql += " ," & Val(mdiawt) & "" 'STNWT
                    strSql += " ,0" 'STNRATE
                    strSql += " ,0" 'STNAMT
                    strSql += " ,''"
                    strSql += " ,'" & CType(trdate, Date).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,0" 'PURRATE
                    strSql += " ,0" 'PURAMT
                    strSql += " ,'W'" 'CALCMODE
                    strSql += " ,0" 'MINRATE
                    strSql += " ,0" 'SIZECODE
                    strSql += " ,'C'" 'STONEUNIT
                    strSql += " ,NULL" 'ISSDATE
                    strSql += " ,''" 'CARRYFLAG
                    strSql += " ,'" & costId & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "ITEMNONTAGSTONE")
                End If
                If Val(mstnpcs) <> 0 Or Val(mstnwt) <> 0 Then
                    'Dim DiaItemId As Integer = 9999 'Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & key + DiaMetalId & "'", , , tran))
                    Dim StnItemId As Integer = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PARTLY-STN'", , , tran).ToString)
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAGSTONE("
                    strSql += " SNO,RECISS,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                    strSql += " STNSUBITEMID,STNPCS,STNWT,"
                    strSql += " STNRATE,STNAMT,DESCRIP,"
                    strSql += " RECDATE,PURRATE,PURAMT,CALCMODE,"
                    strSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
                    strSql += " CARRYFLAG,COSTID,SYSTEMID,APPVER)VALUES("
                    'strSql += " '" & GetSnoFromSoftControl("NONTAGSTONESNO", cnStockDb, "ITEMNONTAGSTONE", tran) & "'" ''SNO
                    strSql += " '" & GetNewSno(TranSnoType.ITEMNONTAGSTONECODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
                    strSql += " ,'R'" ' RECISS
                    strSql += " ,'" & tagSno & "'" 'TAGSNO
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ," & StnItemId & "" 'STNITEMID
                    strSql += " ,0" 'STNSUBITEMID
                    strSql += " ," & Val(mstnpcs) & "" 'STNPCS
                    strSql += " ," & Val(mstnwt) & "" 'STNWT
                    strSql += " ,0" 'STNRATE
                    strSql += " ,0" 'STNAMT
                    strSql += " ,''"
                    strSql += " ,'" & CType(trdate, Date).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,0" 'PURRATE
                    strSql += " ,0" 'PURAMT
                    strSql += " ,'W'" 'CALCMODE
                    strSql += " ,0" 'MINRATE
                    strSql += " ,0" 'SIZECODE
                    strSql += " ,'C'" 'STONEUNIT
                    strSql += " ,NULL" 'ISSDATE
                    strSql += " ,''" 'CARRYFLAG
                    strSql += " ,'" & costId & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "ITEMNONTAGSTONE")
                End If
            Next
                If Recsno.Length > 0 Then Recsno = Mid(Recsno, 1, Recsno.Length - 1)
                If Isssno.Length > 0 Then Isssno = Mid(Isssno, 1, Isssno.Length - 1)
                strSql = ""
                If Recsno.Length > 0 Then
                    strSql = " UPDATE " & cnStockDb & "..RECEIPT"
                strSql += "  SET BAGNO = '" & BagNo & "',TRANFLAG='S'"
                    strSql += vbCrLf + "  WHERE SNO IN (" & Recsno & ")"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If
                If Isssno.Length > 0 Then
                    strSql = " UPDATE " & cnStockDb & "..ISSUE"
                strSql += vbCrLf + "  SET BAGNO = '" & BagNo & "',TRANFLAG ='S'"
                    strSql += vbCrLf + "  WHERE SNO IN (" & Isssno & ")"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If

                tran.Commit()
                tran = Nothing
                MsgBox("Transfered successfully..")
                MsgBox("BagNo   : " & BagNo & " Generated..")
                btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    'Private Sub cmbToCounter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    If e.KeyCode = Keys.Enter Then
    '        Call Ctrstockupdate()
    '    End If
    'End Sub

    Private Sub gridView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEnter
        Dim pt As Point = gridView.Location
        txtGrid.ReadOnly = False
        txtGrid.Clear()
        txtGrid.SelectAll()
        Select Case gridView.Columns(e.ColumnIndex).Name
            Case "NEWITEM"
                txtGrid.Size = New Size(gridView.Columns(e.ColumnIndex).Width, txtGrid.Height)
                txtGrid.Location = pt + gridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                Dim NewItemName As String
                NewItemName = gridView.CurrentCell.Value.ToString ' objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & gridView.CurrentCell.Value.ToString & "' ", "ITEMNAME", "0")
                txtGrid.Text = NewItemName
                'txtGrid.Text = gridView.CurrentCell.FormattedValue
                txtGrid.Visible = True
                txtGrid.Focus()
            Case "NEWCOUNTER"
                txtGrid.Size = New Size(gridView.Columns(e.ColumnIndex).Width, txtGrid.Height)
                txtGrid.Location = pt + gridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                Dim NewItemCtrname As String
                NewItemCtrname = gridView.CurrentCell.Value.ToString  'objGPack.GetSqlValue("SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE COUNTERNAME='" & gridView.CurrentCell.Value.ToString & "' ", "ITEMCTRNAME", "0")
                txtGrid.Text = NewItemCtrname
                'txtGrid.Text = gridView.CurrentCell.FormattedValue
                txtGrid.Visible = True
                txtGrid.Focus()
        End Select
    End Sub

    Private Sub gridView_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellLeave
        Select Case gridView.Columns(e.ColumnIndex).Name
            Case "NEWITEM"
                Dim NewItemid As Integer
                NewItemid = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtGrid.Text & "' ", "ITEMID", "0")
                gridView.CurrentRow.Cells("NEWITEMID").Value = NewItemid
            Case "NEWCOUNTER"
                Dim NewItemCtrid As Integer
                NewItemCtrid = objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & txtGrid.Text & "' ", "ITEMCTRID", "0")
                gridView.CurrentRow.Cells("NEWCTRID").Value = NewItemCtrid
        End Select
        txtGrid.Clear()
        txtGrid.Visible = False
        lstSearch.Visible = False
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        ElseIf e.KeyCode = Keys.Escape Then
            btnTransfer.Focus()


        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(gridView.CurrentCell.ColumnIndex)
        End If
    End Sub

    Private Sub textGridValidator()
        If lstSearch.Items.Contains(txtGrid.Text) = False Then
            txtGrid.Text = tempText.Text
        End If
    End Sub


    Private Sub Trfsummary()
        dtSummary.Clear()
        DgTrfSummary.DataSource = Nothing
        Dim dts As New DataTable
        dts = gridView.DataSource
        Dim ross() As DataRow = Nothing
        ross = dts.Select("CHECK = TRUE")
        If Not ross.Length > 0 Then
            MsgBox("There is no checked record", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim dtsitemdet As DataTable = dts.DefaultView.ToTable(True, "NEWITEM", "NEWCOUNTER")
        Dim mtrdate As String
        Dim mitemid As String
        Dim mitemctrid As String
        For II As Integer = 0 To dtsitemdet.Rows.Count - 1
            mitemid = dtsitemdet.Rows(II).Item("NEWITEM").ToString
            mitemctrid = dtsitemdet.Rows(II).Item("NEWCOUNTER").ToString
            'mtrdate = CType(dtsitemdet.Rows(II).Item("TRANDATE"), Date).ToString("yyyy-MM-dd")
            Dim mpcs As Integer = 0
            Dim mgrswt As Decimal = 0
            Dim mnetwt As Decimal = 0
            Dim mdiapcs As Integer = 0
            Dim mdiawt As Decimal = 0
            Dim mstnwt As Decimal = 0
            For Each row As DataRow In ross
                'If row.Item("TRANDATE") = mtrdate And row.Item("NEWITEMID").ToString = mitemid.ToString And row.Item("NEWCTRID").ToString = mitemctrid.ToString Then
                If row.Item("NEWITEM").ToString = mitemid.ToString And row.Item("NEWCOUNTER").ToString = mitemctrid.ToString Then
                    mpcs += Val(row.Item("PCS").ToString)
                    mgrswt += Val(row.Item("GRSWT").ToString)
                    mnetwt += Val(row.Item("NETWT").ToString)
                    mdiapcs += Val(row.Item("DIAPCS").ToString)
                    mdiawt += Val(row.Item("DIAWT").ToString)
                    mstnwt += Val(row.Item("STNWT").ToString)
                End If
            Next
            Dim drows As DataRow
            drows = dtSummary.NewRow
            drows!ITEMNAME = mitemid
            drows!CTRNAME = mitemctrid
            drows!PCS = mpcs
            drows!GRSWT = Format(mgrswt, "#0.000")
            drows!NETWT = Format(mnetwt, "#0.000")
            drows!DIAPCS = mdiapcs
            drows!DIAWT = Format(mdiawt, "#0.000")
            drows!STNWT = Format(mstnwt, "#0.000")
            dtSummary.Rows.Add(drows)
            dtSummary.AcceptChanges()
        Next
        With DgTrfSummary
            .DataSource = dtSummary
            .Columns("ITEMNAME").Width = 100
            .Columns("CTRNAME").Width = 100
            .Columns("PCS").Width = 40
            .Columns("GRSWT").Width = 90
            .Columns("NETWT").Width = 90
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNWT").Width = 60
            .Columns("DIAPCS").Width = 55
            .Columns("DIAWT").Width = 60
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).ReadOnly = True
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
        End With
        gbSummary.Visible = True
        gbSummary.BringToFront()
        DgTrfSummary.Focus()
    End Sub
    Private Sub txtGrid_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGrid.KeyDown
        With gridView

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Up Then
                If gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "NEWITEM" Then
                    textGridValidator()
                ElseIf gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "NEWCOUNTER" Then
                    textGridValidator()
                End If
            End If

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
                lstSearch.Visible = False
            ElseIf e.KeyCode = Keys.Up Then
                e.Handled = True
                If .CurrentCell.RowIndex <> 0 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.CurrentCell.ColumnIndex)
                End If
            ElseIf e.KeyCode = Keys.Down Then
                e.Handled = True
                If lstSearch.Visible Then
                    lstSearch.Select()
                ElseIf .CurrentCell.RowIndex <> .RowCount - 1 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(.CurrentCell.ColumnIndex)
                End If
            ElseIf e.KeyCode = Keys.Left Then
                e.Handled = True
                If .CurrentCell.ColumnIndex <> 0 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                End If
            ElseIf e.KeyCode = Keys.Right Then
                e.Handled = True
                If gridView.Columns(gridView.CurrentCell.ColumnIndex).Name <> "NEWCTRID" Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End If
        End With
    End Sub

    Private Sub txtGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrid.KeyPress

        If e.KeyChar = Chr(Keys.Enter) Then
        With gridView
            If Not .RowCount > 0 Then
                btnBack.Focus()
                Exit Sub
            End If
            Select Case .Columns(.CurrentCell.ColumnIndex).Name
                    Case "NEWITEM"
                        Dim NewItemid As Integer
                        NewItemid = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtGrid.Text & "' ", "ITEMID", "0")
                        gridView.CurrentRow.Cells("NEWITEMID").Value = NewItemid
                        gridView.CurrentCell.Value = txtGrid.Text
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells("NEWCOUNTER")
                        .CurrentCell.Selected = True
                    Case "NEWCOUNTER"
                        Dim NewItemCtrid As Integer
                        NewItemCtrid = objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & txtGrid.Text & "' ", "ITEMCTRID", "0")
                        gridView.CurrentRow.Cells("NEWCTRID").Value = NewItemCtrid
                        gridView.CurrentCell.Value = txtGrid.Text
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            'gridView.CurrentCell.Value = txtGrid.Text
                            txtGrid.Visible = False
                            btnTransfer.Focus()
                            SendKeys.Send("{TAB}")
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells("NEWITEM")
                            .CurrentCell.Selected = True
                        End If
                End Select

            End With
        End If
    End Sub

    Private Sub txtGrid_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGrid.KeyUp
        TextScript(txtGrid, e)
    End Sub
    Private Sub txtGrid_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrid.TextChanged
        showSearch(txtGrid)
    End Sub

    Private Sub showSearch(ByVal sender As Control)
            loadLstSearch()
        searchSender = sender
        Dim pt As New Point(GetControlLocation(sender, New Point))
        Me.Controls.Add(lstSearch)
        lstSearch.Location = New Point(pt.X, pt.Y + sender.Height)
        If gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "NEWITEM" Or _
        gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "NEWCOUNTER" Then
            lstSearch.Size = New Size(sender.Width, 80)
        Else
            lstSearch.Size = New Size(sender.Width, 120)
        End If


        lstSearch.BringToFront()

        If lstSearch.Items.Count > 0 Then
            lstSearch.Visible = True
        Else
            lstSearch.Visible = False
        End If
    End Sub
    Private Sub loadLstSearch()
        lstSearch.Items.Clear()
        With gridView
            Select Case .Columns(.CurrentCell.ColumnIndex).Name
                Case "NEWITEM"
                    For i As Integer = 0 To listItem.Count - 1
                        lstSearch.Items.Add(listItem.Item(i))
                    Next
                Case "NEWCOUNTER"
                    For i As Integer = 0 To listCounter.Count - 1
                        lstSearch.Items.Add(listCounter.Item(i))
                    Next
            End Select
        End With
    End Sub
    Private Function GetControlLocation(ByVal ctrl As Control, ByRef pt As Point) As Point
        If TypeOf ctrl Is Form Then
            Return pt
        Else
            pt += ctrl.Location
            Return GetControlLocation(ctrl.Parent, pt)
        End If
        Return pt
    End Function

    Public Sub TextScript(ByRef txt As TextBox, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim sComboText As String = ""
        Dim iLoop As Integer
        Dim sTempString As String
        If e.KeyCode >= 65 And e.KeyCode <= 90 Then
            'only look at letters A-Z
            sTempString = txt.Text
            If Len(sTempString) = 1 Then sComboText = sTempString
            For iLoop = 0 To (lstSearch.Items.Count - 1)
                If UCase((sTempString & Mid$(lstSearch.Items.Item(iLoop), _
                  Len(sTempString) + 1))) = UCase(lstSearch.Items.Item(iLoop)) Then
                    lstSearch.SelectedIndex = iLoop
                    txt.Text = lstSearch.Items.Item(iLoop)
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

    Private Sub btnTransfer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransfer.GotFocus
        txtGrid.Visible = False
    End Sub

    Private Sub lstSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            If lstSearch.SelectedItem Is Nothing Then Exit Sub
            With gridView
                Select Case .Columns(.CurrentCell.ColumnIndex).Name
                    Case "NEWITEM"
                        txtGrid.Text = lstSearch.SelectedItem.ToString
                        Dim NewItemid As Integer
                        NewItemid = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtGrid.Text & "' ", "ITEMID", "0")
                        gridView.CurrentRow.Cells("NEWITEMID").Value = NewItemid

                        .CurrentCell.Value = txtGrid.Text
                        lstSearch.Visible = False
                        txtGrid.SelectAll()

                    Case "NEWCOUNTER"
                        txtGrid.Text = lstSearch.SelectedItem.ToString
                        .CurrentCell.Value = txtGrid.Text
                        Dim NewItemCtrid As Integer
                        NewItemCtrid = objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & txtGrid.Text & "' ", "ITEMCTRID", "0")
                        gridView.CurrentRow.Cells("NEWCTRID").Value = NewItemCtrid
                        lstSearch.Visible = False
                        txtGrid.SelectAll()

                End Select

            End With
        End If

    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        With dtSummary
            .Columns.Add("ITEMNAME", GetType(String))
            .Columns.Add("CTRNAME", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("STNWT", GetType(Decimal))
            .Columns.Add("DIAPCS", GetType(Decimal))
            .Columns.Add("DIAWT", GetType(Decimal))
        End With
        dtSummary.AcceptChanges()
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub DgTrfSummary_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then DgTrfSummary.Visible = False : gridView.Focus()
    End Sub
End Class