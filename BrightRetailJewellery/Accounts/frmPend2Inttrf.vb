Imports System.Data.OleDb
Public Class frmPend2Inttrf
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


    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        tabMain.SelectedTab = tabGeneral
        dtpFrom.Select()
        btnSearch.Enabled = True
        Dim TrsDate As Date = GetServerDate()
        lblTrsDate.Text = "TransferDate:" & Format(TrsDate, "dd-MM-yyyy")
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
            objGPack.FillCombo(strSql, cmbCostCentre_MAN, , True)
            cmbCostCentre_MAN.Text = cnCostName
        Else
            cmbCostCentre_MAN.Text = ""
            cmbCostCentre_MAN.Enabled = False
        End If
        gridView.RowTemplate.Height = 21
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect

        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRNAME"

        Dim dtcounter As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcounter)
        listCounter.Clear()
        For Each dr As DataRow In dtcounter.Rows
            listCounter.Add(dr!ITEMCTRNAME.ToString)
        Next

        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE ='N' ORDER BY ITEMNAME"

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
        'GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
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
        strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PENDTRANS') > 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "PENDTRANS"
        strSql += vbCrLf + " SELECT T.SNO,T.TRANDATE,T.TRANNO,T.TRANTYPE,T.CATCODE,T.METALID,T.ITEMID,T.TAGPCS-T.PCS PCS,T.TAGGRSWT-T.GRSWT GRSWT,T.TAGNETWT-T.NETWT NETWT ,T.TAGPCS,T.TAGGRSWT,T.TAGNETWT,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT,CONVERT(INTEGER,NULL) NEWITEMID ,CONVERT(INTEGER,NULL) NEWCTRID "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "PENDTRANS"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE T "
        'strSql += vbCrLf + " left join " & cnStockDb & "..ISSSTONE STN  ON T.SNO=STN.ISSSNO"
        If chkAsOnDate.Checked = True Then
            strSql += vbCrLf + " WHERE T.TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"            
        End If
        strSql += vbCrLf + " AND T.TRANTYPE IN('SA','OD') AND ISNULL(T.CANCEL,'') = ''"
        strSql += vbCrLf + " AND (T.TAGPCS <> T.PCS OR T.TAGGRSWT <> T.GRSWT OR T.TAGNETWT <> T.NETWT)" ' OR STN.TAGSTNWT <> STN.STNWT)"
        strSql += vbCrLf + " AND (T.TAGPCS <> 0 OR T.TAGGRSWT <> 0 OR T.TAGNETWT <> 0)"
        If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
        If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(T.COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        strSql += vbCrLf + " AND T.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
        strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT T.SNO,T.TRANDATE,T.TRANNO,TRANTYPE,T.CATCODE,T.METALID,T.ITEMID,T.PCS,T.GRSWT,T.NETWT,T.TAGPCS,T.TAGGRSWT,T.TAGNETWT,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT,CONVERT(INTEGER,NULL) NEWITEMID ,CONVERT(INTEGER,NULL) NEWCTRID "
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT T "
        If chkAsOnDate.Checked = True Then
            strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " AND TRANTYPE IN ('SR','PU')"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
        If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
        strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PENDTRANSSTUD_IS') > 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "PENDTRANSSTUD_IS"
        strSql += vbCrLf + " SELECT ISSSNO SNO,TRANDATE,CATCODE,STNITEMID ,TAGSTNPCS-STNPCS STNPCS,TAGSTNWT - STNWT STNWT,CONVERT(DECIMAL,NULL) DIAPCS,CONVERT(DECIMAL,NULL) DIAWT "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "PENDTRANSSTUD_IS"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE I"
        If chkAsOnDate.Checked = True Then
            strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
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
        If chkAsOnDate.Checked = True Then
            strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' AND TRANTYPE = 'SA'  AND ISNULL(BAGNO,'') = '')"
        strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN('D'))"
        strSql += vbCrLf + " AND (TAGSTNPCS <> STNPCS OR TAGSTNWT <> STnWT)"
        strSql += vbCrLf + " AND (TAGSTNPCS <> 0 OR TAGSTNWT <> 0)"
        strSql += vbCrLf + " AND (TAGSTNPCS - STNPCS <> 0 OR TAGSTNWT - STNWT <> 0)"
        If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"


        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()



        strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PENDTRANSSTUD_RE') > 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "PENDTRANSSTUD_RE"
        strSql += vbCrLf + " SELECT TRANDATE,ISSSNO SNO,STNITEMID,CATCODE,sum(STNPCS) STNPCS,sum(STNWT) STNWT,CONVERT(DECIMAL,NULL) DIAPCS,CONVERT(DECIMAL,NULL) DIAWT "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "PENDTRANSSTUD_RE"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE R"
        If chkAsOnDate.Checked = True Then
            strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " AND ISSSNO IN (sELECT SNO FROM " & cnStockDb & "..RECEIPT"
        If chkAsOnDate.Checked = True Then
            strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND TRANTYPE IN ('SR','PU') AND ISNULL(BAGNO,'') = '' )"
        strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN ('S','P'))"
        If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " group by TRANDATE,ISSSNO ,STNITEMID,CATCODE"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT TRANDATE,ISSSNO SNO,STNITEMID,CATCODE,0 STNPCS ,0 STNWT,sum(STNPCS) DIAPCS,sum(STNWT) DIAWT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE R"
        If chkAsOnDate.Checked = True Then
            strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " AND ISSSNO IN (sELECT SNO FROM " & cnStockDb & "..RECEIPT"
        If chkAsOnDate.Checked = True Then
            strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND TRANTYPE IN ('SR','PU') AND ISNULL(BAGNO,'') = '' )"
        strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN ('D'))"
        If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " group by TRANDATE,ISSSNO ,STNITEMID,CATCODE"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = " "

        strSql = " UPDATE A SET A.DIAPCS = B.DIAPCS,A.DIAWT = B.DIAWT,A.STNPCS=B.STNPCS,A.STNWT = B.STNWT FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS A INNER JOIN TEMPTABLEDB..TEMP" & systemId & "PENDTRANSSTUD_IS B ON A.SNO = B.SNO"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " UPDATE A SET A.DIAPCS = B.DIAPCS,A.DIAWT = B.DIAWT,A.STNPCS=B.STNPCS,A.STNWT = B.STNWT FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS A INNER JOIN TEMPTABLEDB..TEMP" & systemId & "PENDTRANSSTUD_RE B ON A.SNO = B.SNO"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        If CTRITEMTRF = "Y" Then
            Dim dtitem As New DataTable
            strSql = "select DISTINCT METALID,CATCODE,TRANTYPE FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtitem)
            'strSql = " UPDATE TEMP" & systemId & "PENDTRANS SET NEWITEMID=(SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID=(CASE WHEN T.TRANTYPE='PU' THEN 'PURCHASE-' WHEN T.TRANTYPE='SR' THEN 'RETURN-' ELSE 'PARTLY-' END + T.METALID))"
            'strSql += vbCrLf + " FROM TEMP" & systemId & "PENDTRANS AS T where METALID = '" & dtitem.Rows(ij).Item("METALID").ToString & "' AND TRANTYPE = '" & dtitem.Rows(ij).Item("TRANTYPE").ToString & "' AND ISNULL(NEWITEMID,0) = 0 "
            'cmd = New OleDbCommand(strSql, cn)
            'cmd.ExecuteNonQuery()
        End If

        strSql = " SELECT T.SNO,CASE WHEN T.TRANTYPE='PU' THEN 'PURCHASE' WHEN T.TRANTYPE='SR' THEN 'SALERETURN' ELSE 'PARTSALE' END AS TRANTYPE"
        strSql += vbCrLf + " ,T.TRANDATE,T.TRANNO,T.ITEMID,T.CATCODE,ISNULL(IM.ITEMNAME,IC.CATNAME) AS DESCRIPTION,SUM(T.PCS)PCS"
        strSql += vbCrLf + " ,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.DIAPCS)DIAPCS,SUM(T.DIAWT) AS DIAWT"
        strSql += vbCrLf + " ,SUM(T.STNPCS)STNPCS,SUM(T.STNWT) AS STNWT "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS AS T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID = IM.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IMM ON T.NEWITEMID = IMM.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY IC ON T.CATCODE = IC.CATCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER NC ON IMM.DEFAULTCOUNTER= NC.ITEMCTRID"
        strSql += vbCrLf + " GROUP BY T.SNO,T.TRANTYPE,T.TRANDATE,T.TRANNO,T.ITEMID,T.CATCODE,IC.CATNAME,IM.ITEMNAME"
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
                .Columns("CHECK").Width = 25
                .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                .Columns("TRANDATE").Width = 80
                .Columns("TRANNO").Width = 60
                .Columns("CATCODE").Visible = False
                .Columns("TRANTYPE").Width = 100
                .Columns("DESCRIPTION").Width = 150
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
                .Columns("SNO").Visible = False
                .Columns("ITEMID").Visible = False
                For cnt As Integer = 0 To .ColumnCount - 1
                    .Columns(cnt).ReadOnly = True
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("CHECK").ReadOnly = False
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
            InternalIssuecreate()
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub InternalIssuecreate()
        Dim dt As New DataTable
        dt = gridView.DataSource
        Dim ros() As DataRow = Nothing
        ros = dt.Select("CHECK = TRUE")
        If Not ros.Length > 0 Then
            MsgBox("There is No Checked Record", MsgBoxStyle.Information)
            Exit Sub
        End If
        If MessageBox.Show("Do you want to transfer the above items?", "Transfer Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then Exit Sub
        Try
            Dim costId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")
            Dim metalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbOpenMetal.Text & "'")
            Dim Accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID ='" & _SyncTo & "'")
            Dim TAccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID ='" & costId & "'")
            Dim TCostname As String = objGPack.GetSqlValue("SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID ='" & _SyncTo & "'")
            Dim mTotPcs As Decimal = 0
            Dim mTotGwt As Decimal = 0
            Dim mTotNwt As Decimal = 0
            Dim mTotStnwt As Decimal = 0
            Dim mTotStnpcs As Decimal = 0
            Dim mTotDiawt As Decimal = 0
            Dim mTotDiapcs As Decimal = 0
            Dim Recsno As String = ""
            Dim Isssno As String = ""
            If Accode = "" Or TAccode = "" Then MsgBox("Internal Transfer Accode is Empty", MsgBoxStyle.Information) : Exit Sub
            'Dim dtitemdet As DataTable = dt.DefaultView.ToTable(True, "TRANDATE", "NEWITEMID", "NEWCTRID")
            Dim dtitemdet As DataTable = dt.DefaultView.ToTable(True, "CATCODE")
            Dim trdate As Date
            trdate = GetServerDate()
            tran = Nothing
            tran = cn.BeginTransaction
            Dim PTrfNo As String = Nothing
GENBAGNO:
            'BagNo = cnCostId & GetTranDbSoftControlValue("BAGNO", True, tran)
            PTrfNo = cnCostId & "P" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & GetTranDbSoftControlValue("PENDING_TRANNO", True, tran)
            ''check
            strSql = " SELECT 'CHECK' FROM " & cnStockDb & "..RECEIPT"
            strSql += vbCrLf + "  WHERE TRFNO = '" & PTrfNo & "'"
            strSql += " UNION ALL SELECT 'CHECK' FROM " & cnStockDb & "..ISSUE"
            strSql += vbCrLf + "  WHERE TRFNO = '" & PTrfNo & "'"
            If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
                GoTo GENBAGNO
            End If
            Dim Batchno As String = GetNewBatchno(cnCostId, trdate, tran)
            Dim billcontrolid As String = "GEN-SM-INTISS"
            strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
            strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
            If UCase(objGPack.GetSqlValue(strSql, , , tran)) <> "Y" Then
                billcontrolid = "GEN-STKREFNO"
            End If
            Dim NEWBILLNO As Integer
            strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
            NEWBILLNO = Val(objGPack.GetSqlValue(strSql, , , tran))
GenerateNewBillNo:
            strSql = " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'IIN' AND TRANNO = " & NEWBILLNO
            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                NEWBILLNO = NEWBILLNO + 1
                GoTo GenerateNewBillNo
            End If
            strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & NEWBILLNO.ToString & "'"
            strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GenerateNewBillNo
            End If
            For Each row As DataRow In dtitemdet.Rows
                Dim mpcs As Integer = Val(dt.Compute("SUM(PCS)", "CATCODE='" & row.Item("CATCODE") & "' AND CHECK = TRUE").ToString)
                Dim mgrswt As Decimal = Val(dt.Compute("SUM(GRSWT)", "CATCODE='" & row.Item("CATCODE") & "' AND CHECK = TRUE").ToString)
                Dim mnetwt As Decimal = Val(dt.Compute("SUM(NETWT)", "CATCODE='" & row.Item("CATCODE") & "' AND CHECK = TRUE").ToString)
                Dim mdiapcs As Decimal = Val(dt.Compute("SUM(DIAPCS)", "CATCODE='" & row.Item("CATCODE") & "' AND CHECK = TRUE").ToString)
                Dim mdiawt As Decimal = Val(dt.Compute("SUM(DIAWT)", "CATCODE='" & row.Item("CATCODE") & "' AND CHECK = TRUE").ToString)
                Dim mstnwt As Decimal = Val(dt.Compute("SUM(STNWT)", "CATCODE='" & row.Item("CATCODE") & "' AND CHECK = TRUE").ToString)
                Dim Sno As String = GetNewSno(TranSnoType.ISSUECODE, tran)
                strSql = " INSERT INTO " & cnStockDb & "..ISSUE"
                strSql += " ("
                strSql += " SNO,COMPANYID,COSTID,CATCODE,TRANDATE,TRANNO,TRANTYPE,"
                strSql += " PCS,GRSWT,LESSWT,NETWT,BATCHNO, CANCEL,TRFNO,REMARK1,ACCODE, "
                strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER)VALUES("
                strSql += " '" & Sno & "','" & GetStockCompId() & "','" & costId & "'"
                strSql += ",'" & row.Item("CATCODE") & "','" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += " ," & NEWBILLNO & " ,'IIN'"
                strSql += " ," & Val(mpcs) & "," & Val(mgrswt) & "," & Val(mgrswt) - Val(mnetwt) & "," & Val(mnetwt) & "" 'NETWT
                strSql += " ,'" & Batchno & "' ,''" 'CANCEL
                strSql += " ,'" & PTrfNo & "'" 'PACKETNO
                strSql += " ,'TRANSFER TO " & TCostname & "','" & Accode & "' "
                strSql += " ," & userId & "" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)

                strSql = " INSERT INTO " & cnStockDb & "..RECEIPT"
                strSql += " ("
                strSql += " SNO,COMPANYID,COSTID,CATCODE,TRANDATE,TRANNO,TRANTYPE,"
                strSql += " PCS,GRSWT,LESSWT,NETWT,BATCHNO, CANCEL,TRFNO,REMARK1,ACCODE, "
                strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER)VALUES("
                strSql += " '" & Sno & "','" & GetStockCompId() & "','" & _SyncTo & "'"
                strSql += ",'" & row.Item("CATCODE") & "','" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += " ," & NEWBILLNO & " ,'RIN'"
                strSql += " ," & Val(mpcs) & "," & Val(mgrswt) & "," & Val(mgrswt) - Val(mnetwt) & "," & Val(mnetwt) & "" 'NETWT
                strSql += " ,'" & Batchno & "' ,''" 'CANCEL
                strSql += " ,'" & PTrfNo & "'" 'PACKETNO
                strSql += " ,'TRANSFER FROM " & cmbCostCentre_MAN.Text & "','" & TAccode & "' "
                strSql += " ," & userId & "" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TRECEIPT", False)
                If Val(mdiapcs) <> 0 Or Val(mdiawt) <> 0 Then

                End If

            Next
            For Each row As DataRow In dt.Select("TRANTYPE='PARTSALE' AND CHECK = TRUE", Nothing)
                strSql = " UPDATE " & cnStockDb & "..ISSUE"
                strSql += vbCrLf + "  SET TRFNO = '" & PTrfNo & "',TRANFLAG ='S'"
                strSql += vbCrLf + "  WHERE SNO = '" & row.Item("SNO").ToString & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            Next

            For Each row As DataRow In dt.Select("TRANTYPE<>'PARTSALE' AND CHECK = TRUE", Nothing)
                strSql = " UPDATE " & cnStockDb & "..RECEIPT"
                strSql += vbCrLf + "  SET TRFNO = '" & PTrfNo & "',TRANFLAG ='S'"
                strSql += vbCrLf + "  WHERE SNO = '" & row.Item("SNO").ToString & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            Next

            tran.Commit()
            tran = Nothing
            MsgBox("Transfered successfully.." & vbCrLf & "Tranfer No.  : " & PTrfNo & " Generated..")

            Dim Pbatchno As String = Batchno
            Dim Pbilldate As Date = trdate
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":IIN")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & pBilldate.ToString("yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                    LSet("TYPE", 15) & ":IIN" & ";" & _
                    LSet("BATCHNO", 15) & ":" & Pbatchno & ";" & _
                    LSet("TRANDATE", 15) & ":" & Pbilldate.ToString("yyyy-MM-dd") & ";" & _
                    LSet("DUPLICATE", 15) & ":N")
                End If
            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If
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
        'Select Case gridView.Columns(e.ColumnIndex).Name
        '    Case "NEWITEM"
        '        txtGrid.Size = New Size(gridView.Columns(e.ColumnIndex).Width, txtGrid.Height)
        '        txtGrid.Location = pt + gridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
        '        Dim NewItemName As String
        '        NewItemName = gridView.CurrentCell.Value.ToString ' objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & gridView.CurrentCell.Value.ToString & "' ", "ITEMNAME", "0")
        '        txtGrid.Text = NewItemName
        '        'txtGrid.Text = gridView.CurrentCell.FormattedValue
        '        txtGrid.Visible = True
        '        txtGrid.Focus()
        '    Case "NEWCOUNTER"
        '        txtGrid.Size = New Size(gridView.Columns(e.ColumnIndex).Width, txtGrid.Height)
        '        txtGrid.Location = pt + gridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
        '        Dim NewItemCtrname As String
        '        NewItemCtrname = gridView.CurrentCell.Value.ToString  'objGPack.GetSqlValue("SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE COUNTERNAME='" & gridView.CurrentCell.Value.ToString & "' ", "ITEMCTRNAME", "0")
        '        txtGrid.Text = NewItemCtrname
        '        'txtGrid.Text = gridView.CurrentCell.FormattedValue
        '        txtGrid.Visible = True
        '        txtGrid.Focus()
        'End Select
    End Sub

    Private Sub gridView_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellLeave
        Select Case gridView.Columns(e.ColumnIndex).Name
         
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

        ElseIf e.KeyCode = Keys.Space Then
            gridView.CurrentRow.Cells("CHECK").Value = Not gridView.CurrentRow.Cells("CHECK").Value
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
        Dim dtsitemdet As DataTable = dts.DefaultView.ToTable(True, "CATCODE")
        Dim mtrdate As String
        Dim mitemid As String
        Dim mitemctrid As String
        For II As Integer = 0 To dtsitemdet.Rows.Count - 1
            mitemid = dtsitemdet.Rows(II).Item("CATCODE").ToString
            'mitemctrid = dtsitemdet.Rows(II).Item("NEWCOUNTER").ToString
            'mtrdate = CType(dtsitemdet.Rows(II).Item("TRANDATE"), Date).ToString("yyyy-MM-dd")
            Dim mpcs As Integer = 0
            Dim mgrswt As Decimal = 0
            Dim mnetwt As Decimal = 0
            Dim mdiapcs As Integer = 0
            Dim mdiawt As Decimal = 0
            Dim mstnwt As Decimal = 0
            For Each row As DataRow In ross
                'If row.Item("TRANDATE") = mtrdate And row.Item("NEWITEMID").ToString = mitemid.ToString And row.Item("NEWCTRID").ToString = mitemctrid.ToString Then
                If row.Item("CATCODE").ToString = mitemid.ToString Then
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
            strSql = "SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & mitemid & "'"
            ' drows!CATCODE = mitemid
            drows!CATNAME = objGPack.GetSqlValue(strSql).ToString
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

            .Columns("CATNAME").Width = 100
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
            .Columns.Add("CATNAME", GetType(String))
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

    Private Sub chkAsOnDate_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckStateChanged
        lblTo.Visible = Not chkAsOnDate.Checked
        dtpTo.Visible = Not chkAsOnDate.Checked
        If chkAsOnDate.Checked Then
            chkAsOnDate.Text = "As On Date"
        Else
            chkAsOnDate.Text = "Date From"
        End If
    End Sub
End Class