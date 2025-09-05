Imports System.Data.OleDb
Public Class FRM_STOCKCHECK_REPORT
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtCompany As New DataTable
    Dim dtItemName As New DataTable
    Dim dtDesigner As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtCounter As New DataTable


    Private Sub FRM_STOCKCHECK_REPORT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F3 Then
            btnNew_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub FRM_STOCKCHECK_REPORT_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        'strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        'strSql += vbCrLf + " UNION ALL"
        'strSql += vbCrLf + " SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        'strSql += vbCrLf + " WHERE ACTIVE = 'Y' AND STOCKTYPE = 'T'"
        'strSql += vbCrLf + " ORDER BY RESULT,ITEMNAME"
        'dtItemName = New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtItemName)
        Funcfillitemname("ALL")
        'BrighttechPack.GlobalMethods.FillCombo(chkCmbItemName, dtItemName, "ITEMNAME", , "ALL")
        strSql = " SELECT 'ALL' DESIGNERNAME,'ALL' DESIGNERID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT DESIGNERNAME,CONVERT(vARCHAR,DESIGNERID),2 RESULT FROM " & cnAdminDb & "..DESIGNER"
        strSql += vbCrLf + " ORDER BY RESULT,DESIGNERNAME"
        dtDesigner = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDesigner)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbDesigner, dtDesigner, "DESIGNERNAME", , "ALL")
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , cnCostName)
        If strUserCentrailsed <> "Y" Then chkCmbCostCentre.Enabled = False
        strSql = " SELECT 'ALL' COUNTER,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ITEMCTRNAME AS COUNTER,2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER"
        strSql += vbCrLf + " ORDER BY RESULT,COUNTER"
        dtCounter = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCounter)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCounter, dtCounter, "COUNTER", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCounter.Enabled = False

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Function Funcfillitemname(ByVal defvalue As String) As Integer
        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += vbCrLf + " WHERE ACTIVE = 'Y' AND STOCKTYPE = 'T'"
        strSql += vbCrLf + " ORDER BY RESULT,ITEMNAME"
        dtItemName = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItemName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemName, dtItemName, "ITEMNAME", , defvalue.ToString)
    End Function
    Private Function FillDetailedView(ByVal chkTranNo As String) As DataTable
        strSql = " IF OBJECT_ID('TEMPDB..#CHKTRAY_DET','U') IS NOT NULL DROP TABLE #CHKTRAY_DET"
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " CONVERT(VARCHAR(300),IM.ITEMNAME) AS ITEM"
        strSql += vbCrLf + " ,SM.SUBITEMNAME AS SUBITEM,T.TAGNO,T.PCS,T.GRSWT,T.NETWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
        strSql += vbCrLf + " ,TRANSFERWT CHKWT,(ISNULL(T.GRSWT,0)-ISNULL(TRANSFERWT,0)) DIFFWT"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(20),T.CHKTRAY)AS CHKTRAY,T.CHKDATE"
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS COUNTER"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(20),'DETAILED') TABLENAME,CONVERT(VARCHAR(3),'')COLHEAD"
        strSql += vbCrLf + " ,CONVERT(INT,0)RESULT,CONVERT(BIGINT,TAGVAL)TAGVAL"
        strSql += vbCrLf + " INTO #CHKTRAY_DET"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = T.ITEMID AND SM.SUBITEMID = T.SUBITEMID"
        strSql += vbCrLf + " WHERE T.ISSDATE IS NULL "
        If chkTranNo <> "" Then strSql += vbCrLf + " AND T.CHKTRAY = '" & chkTranNo & "'"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        If chkCmbItemName.Text <> "ALL" And chkCmbItemName.Text <> "" Then
            strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItemName.Text) & "))"
        End If
        If chkCmbDesigner.Text <> "ALL" And chkCmbDesigner.Text <> "" Then
            strSql += vbCrLf + " AND T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & GetQryString(chkCmbDesigner.Text) & "))"
        End If
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            strSql += " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"

        End If
        If chkCmbCounter.Text <> "ALL" And chkCmbCounter.Text <> "" Then
            strSql += " AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & GetQryString(chkCmbCounter.Text) & "))"
        End If
        If RbtChecked.Checked Then
            strSql += vbCrLf + " AND ISNULL(T.CHKTRAY,'') <> '' "
        ElseIf RbtUnchecked.Checked Then
            strSql += vbCrLf + " AND ISNULL(T.CHKTRAY,'') = '' "
        End If

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        If chkTranNo = "" Then
            strSql = " INSERT INTO #CHKTRAY_DET(ITEM,RESULT,COLHEAD,CHKTRAY)"
            strSql += " SELECT CHKTRAY,-1 RESULT,'T'COLHEAD,CHKTRAY"
            strSql += vbCrLf + " FROM #CHKTRAY_DET WHERE RESULT = 0 GROUP BY CHKTRAY"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            strSql = " INSERT INTO #CHKTRAY_DET(ITEM,PCS,GRSWT,NETWT,DIAPCS,DIAWT,CHKWT,DIFFWT,RESULT,COLHEAD,CHKTRAY)"
            strSql += " SELECT CHKTRAY + ' TOT',SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),SUM(CHKWT),SUM(DIFFWT),2 RESULT,'S'COLHEAD,CHKTRAY"
            strSql += vbCrLf + " FROM #CHKTRAY_DET WHERE RESULT = 0 GROUP BY CHKTRAY"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If
        strSql = " INSERT INTO #CHKTRAY_DET(ITEM,PCS,GRSWT,NETWT,DIAPCS,DIAWT,CHKWT,DIFFWT,RESULT,COLHEAD,CHKTRAY)"
        strSql += " SELECT 'GRAND TOTAL',SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),SUM(CHKWT),SUM(DIFFWT),3 RESULT,'G'COLHEAD,'ZZZZZZZZZ' AS CHKTRAYNO"
        strSql += vbCrLf + " FROM #CHKTRAY_DET WHERE RESULT = 0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " UPDATE #CHKTRAY_DET SET CHKWT= NULL WHERE CHKWT=0 "
        strSql += " UPDATE #CHKTRAY_DET SET DIFFWT= NULL WHERE DIFFWT=0 "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()


        strSql = " SELECT * FROM #CHKTRAY_DET ORDER BY CHKTRAY,RESULT,ITEM,TAGVAL"
        Dim dtGrid As New DataTable("DETAILED")
        Dim dtCol As New DataColumn("KEYNO")
        dtCol.AutoIncrement = True
        dtCol.AutoIncrementSeed = 0
        dtCol.AutoIncrementStep = 1
        dtGrid.Columns.Add(dtCol)
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If dtGrid.Rows.Count > 0 Then
            dtGrid.Rows(dtGrid.Rows.Count - 1).Item("CHKTRAY") = DBNull.Value
        End If
        Return dtGrid
    End Function

    Private Sub DataGridView_DetailedFormatting(ByVal dgv As DataGridView)
        With dgv
            .Columns("KEYNO").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("TABLENAME").Visible = False
            .Columns("TAGVAL").Visible = False
            .Columns("CHKDATE").DefaultCellStyle.Format = "dd/MM/yyyy"

            .Columns("ITEM").Width = 150
            .Columns("SUBITEM").Width = 150
            .Columns("TAGNO").Width = 80
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("DIAPCS").Width = 60
            .Columns("DIAWT").Width = 80
            .Columns("CHKWT").Width = 80
            .Columns("DIFFWT").Width = 80

            If chkWithDiffwt.Checked And chkWithDiffwt.Enabled Then
                .Columns("CHKWT").Visible = True
                .Columns("DIFFWT").Visible = True
            Else
                .Columns("CHKWT").Visible = False
                .Columns("DIFFWT").Visible = False
            End If
            .Columns("CHKTRAY").Width = 70
            .Columns("CHKDATE").Width = 80
            .Columns("DESIGNER").Width = 120
            .Columns("COUNTER").Width = 100

            .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            FormatGridColumns(dgv, False, False, , False)
        End With
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Prop_Sets()
        Dim dtGrid As New DataTable("SUMMARY")
        If rbtSummary.Checked Then
            strSql = " IF OBJECT_ID('TEMPDB..#CHKTRAY_INFO','U') IS NOT NULL DROP TABLE #CHKTRAY_INFO"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(100),CASE WHEN ISNULL(CHKTRAY,'') = '' THEN 'Pending' else CHKTRAY END)PARTICULAR"
            strSql += vbCrLf + " ,PCS,GRSWT,NETWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(20),CASE WHEN ISNULL(CHKTRAY,'') = '' THEN 'Pending' else CHKTRAY END)AS CHKTRAY"
            strSql += vbCrLf + " ,CONVERT(INT,2)RESULT"
            strSql += vbCrLf + " ,CONVERT(INT,CASE WHEN ISNULL(CHKTRAY,'') = '' THEN 999999 else 0 END)AS CHKORD"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(3),'')COLHEAD"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(20),'SUMMARY')AS TABLENAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID))AS ITEM"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = T.ITEMID AND SUBITEMID=T.SUBITEMID))AS SUBITEM"
            strSql += vbCrLf + " INTO #CHKTRAY_INFO"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T WHERE ISSDATE IS NULL"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            If chkCmbItemName.Text <> "ALL" And chkCmbItemName.Text <> "" Then
                strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItemName.Text) & "))"
            End If
            If chkCmbDesigner.Text <> "ALL" And chkCmbDesigner.Text <> "" Then
                strSql += vbCrLf + " AND T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & GetQryString(chkCmbDesigner.Text) & "))"
            End If
            If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
                strSql += vbCrLf + " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
            Else
                strSql += vbCrLf + " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"

            End If
            If chkCmbCounter.Text <> "ALL" And chkCmbCounter.Text <> "" Then
                strSql += vbCrLf + " AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & GetQryString(chkCmbCounter.Text) & "))"
            End If
            If RbtChecked.Checked Then
                strSql += vbCrLf + " AND ISNULL(CHKTRAY,'') <> '' "
            ElseIf RbtUnchecked.Checked Then
                strSql += vbCrLf + " AND ISNULL(CHKTRAY,'') = '' "
            End If

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            If chkItem.Checked = False Then
                strSql = " UPDATE #CHKTRAY_INFO SET ITEM = NULL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            End If
            If ChkSubItem.Checked = False Then
                strSql = " UPDATE #CHKTRAY_INFO SET SUBITEM = NULL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            End If

            strSql = " IF OBJECT_ID('TEMPDB..#CHKTRAY_SUM','U') IS NOT NULL DROP TABLE #CHKTRAY_SUM"
            strSql += vbCrLf + " SELECT PARTICULAR"
            strSql += vbCrLf + " ,SUM(X.PCS)AS PCS,SUM(X.GRSWT)AS GRSWT,SUM(X.NETWT) AS NETWT"
            strSql += vbCrLf + " ,SUM(X.DIAPCS)DIAPCS,SUM(X.DIAWT)AS DIAWT,RESULT,COLHEAD,CHKTRAY,TABLENAME,ITEM,SUBITEM,CHKORD"
            strSql += vbCrLf + " INTO #CHKTRAY_SUM"
            strSql += vbCrLf + " FROM #CHKTRAY_INFO AS X"
            strSql += vbCrLf + " GROUP BY RESULT,COLHEAD,CHKTRAY,TABLENAME,ITEM,SUBITEM,PARTICULAR,CHKORD"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            If chkItem.Checked Then
                strSql = " INSERT INTO #CHKTRAY_SUM(PARTICULAR,ITEM,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT DISTINCT ITEM,ITEM,0 RESULT,'T' COLHEAD"
                strSql += vbCrLf + " FROM #CHKTRAY_INFO WHERE RESULT = 2"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                If ChkSubItem.Checked = False Then
                    strSql = " INSERT INTO #CHKTRAY_SUM(PARTICULAR,ITEM,PCS,GRSWT,NETWT,DIAPCS,DIAWT,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT ITEM'-> TOT',ITEM,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),4 RESULT,'S' COLHEAD"
                    strSql += vbCrLf + " FROM #CHKTRAY_INFO WHERE RESULT = 2 GROUP BY ITEM"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                End If
            End If
            If ChkSubItem.Checked Then
                strSql = " INSERT INTO #CHKTRAY_SUM(PARTICULAR,SUBITEM,ITEM,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT DISTINCT SUBITEM,SUBITEM,ITEM,1 RESULT,'T' COLHEAD"
                strSql += vbCrLf + " FROM #CHKTRAY_INFO WHERE RESULT = 2"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                strSql = " INSERT INTO #CHKTRAY_SUM(PARTICULAR,SUBITEM,ITEM,PCS,GRSWT,NETWT,DIAPCS,DIAWT,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT SUBITEM'-> TOT',SUBITEM,ITEM,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),3 RESULT,'S' COLHEAD"
                strSql += vbCrLf + " FROM #CHKTRAY_INFO WHERE RESULT = 2 GROUP BY ITEM,SUBITEM"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            End If
            strSql = " INSERT INTO #CHKTRAY_SUM(PARTICULAR,ITEM,PCS,GRSWT,NETWT,DIAPCS,DIAWT,RESULT,COLHEAD)"
            strSql += vbCrLf + " SELECT 'GRAND TOTAL','ZZZZZZZZ'ITEM,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),5 RESULT,'G' COLHEAD"
            strSql += vbCrLf + " FROM #CHKTRAY_INFO WHERE RESULT = 2"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = " SELECT * FROM #CHKTRAY_SUM"
            strSql += vbCrLf + " ORDER BY ITEM,SUBITEM,RESULT,CHKORD,CHKTRAY"
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        Else
            dtGrid = FillDetailedView("")
        End If
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        AddHandler objGridShower.gridView.KeyPress, AddressOf DataGridView_KeyPress
        objGridShower.Text = "TRAY WISE STOCK CHECK REPORT"
        Dim tit As String = "TRAY WISE STOCK CHECK REPORT" + vbCrLf
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        If rbtSummary.Checked Then
            DataGridView_SummaryFormatting(objGridShower.gridView)
        Else
            DataGridView_DetailedFormatting(objGridShower.gridView)
        End If
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridViewHeader.Visible = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns(objGridShower.gridView.FirstDisplayedCell.ColumnIndex)))
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("CHKTRAY").Visible = False
            .Columns("TABLENAME").Visible = False
            .Columns("ITEM").Visible = False
            .Columns("CHKORD").Visible = False

            .Columns("PARTICULAR").Width = 200
            .Columns("PCS").Width = 70
            .Columns("GRSWT").Width = 100
            .Columns("NETWT").Width = 100
            .Columns("DIAPCS").Width = 70
            .Columns("DIAWT").Width = 100

            .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

            FormatGridColumns(dgv, False, False, , False)
            'FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub


    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If Not dgv.RowCount > 0 Then Exit Sub
            If dgv.CurrentRow Is Nothing Then Exit Sub
            If dgv.CurrentRow.Cells("TABLENAME").Value.ToString = "DETAILED" Then
                Dim f As frmGridDispDia
                f = objGPack.GetParentControl(dgv)
                f.FormReSize = False
                If f.dsGrid.Tables.Contains("SUMMARY") = False Then
                    Exit Sub
                End If
                f.gridView.DataSource = Nothing
                f.gridView.DataSource = f.dsGrid.Tables("SUMMARY")
                f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                DataGridView_SummaryFormatting(f.gridView)
                Dim tit As String = "TRAY WISE STOCK CHECK REPORT" + vbCrLf
                f.lblTitle.Text = tit
                f.lblStatus.Text = "<Press [D] for Detail View>"
                f.FormReSize = True
                f.FormReLocation = True
                f.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(f.gridView.Columns(f.gridView.FirstDisplayedCell.ColumnIndex)))
                f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                f.gridView.Select()
                FillGridGroupStyle_KeyNoWise(f.gridView)
            End If
        End If
    End Sub

    Private Sub DataGridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If UCase(e.KeyChar) = "D" Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.CurrentRow Is Nothing Then Exit Sub
            If dgv.CurrentRow.Cells("TABLENAME").Value.ToString = "SUMMARY" Then
                If dgv.CurrentRow.Cells("COLHEAD").Value.ToString <> "" Then Exit Sub
                Dim f As frmGridDispDia
                f = objGPack.GetParentControl(dgv)
                Dim dt As DataTable = FillDetailedView(dgv.CurrentRow.Cells("CHKTRAY").Value.ToString)
                If Not dt.Rows.Count > 0 Then
                    MsgBox("Record not found", MsgBoxStyle.Information)
                    Exit Sub
                End If
                If f.dsGrid.Tables.Contains(dt.TableName) Then f.dsGrid.Tables.Remove(dt.TableName)
                Dim tit As String = "TRAY WISE STOCK CHECK REPORT" + vbCrLf
                tit += "TRAY : " & dgv.CurrentRow.Cells("CHKTRAY").Value.ToString
                f.lblTitle.Text = tit
                f.dsGrid.Tables.Add(dt)
                f.FormReSize = False
                f.FormReLocation = False
                f.gridView.DataSource = Nothing
                f.gridView.DataSource = f.dsGrid.Tables("DETAILED")
                f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                DataGridView_DetailedFormatting(f.gridView)
                f.lblStatus.Text = "<Press [D] for Detail View>" & "<Press [ESCAPE] for Summary View>"
                f.FormReSize = True
                f.FormReLocation = True
                f.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(f.gridView.Columns(f.gridView.FirstDisplayedCell.ColumnIndex)))
                f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                f.gridView.Select()
                FillGridGroupStyle_KeyNoWise(f.gridView)
            End If
        End If
    End Sub


    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New FRM_STOCKCHECK_REPORT_Properties
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        GetChecked_CheckedList(chkCmbItemName, obj.p_chkCmbItemName)
        GetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDesigner)
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        GetChecked_CheckedList(chkCmbCounter, obj.p_chkCmbCounter)
        SetSettingsObj(obj, Me.Name, GetType(FRM_STOCKCHECK_REPORT_Properties))
    End Sub

    Private Sub Prop_Gets()
        txtItemCode_NUM.Clear()
        Funcfillitemname("ALL")
        Dim obj As New FRM_STOCKCHECK_REPORT_Properties
        GetSettingsObj(obj, Me.Name, GetType(FRM_STOCKCHECK_REPORT_Properties))
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        SetChecked_CheckedList(chkCmbItemName, obj.p_chkCmbItemName, "ALL")
        SetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDesigner, "ALL")
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        SetChecked_CheckedList(chkCmbCounter, obj.p_chkCmbCounter, "ALL")
    End Sub

    Private Sub rbtDetailed_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtDetailed.CheckedChanged
        chkItem.Checked = Not rbtDetailed.Checked
        chkItem.Enabled = Not rbtDetailed.Checked
        ChkSubItem.Checked = Not rbtDetailed.Checked
        ChkSubItem.Enabled = Not rbtDetailed.Checked
        If RbtChecked.Checked And rbtDetailed.Checked Then
            chkWithDiffwt.Enabled = True
        Else
            chkWithDiffwt.Enabled = False
        End If
    End Sub

    Private Sub btngenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btngenerate.Click
        Try
            strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMPSTOCKCHECKTABLE')IS NOT NULL DROP TABLE  TEMPTABLEDB..TEMPSTOCKCHECKTABLE"
            strSql += vbCrLf + "  SELECT ITEMID,SUM(STKPCS)STKPCS,SUM(STKWT)STKWT,SUM(CHKPCS)CHKPCS,SUM(CHKWT)CHKWT, MINDATE,MAXDATE,DATA  INTO TEMPTABLEDB..TEMPSTOCKCHECKTABLE FROM("
            strSql += vbCrLf + "  SELECT ITEMID,SUM(T.PCS)STKPCS,SUM(T.GRSWT)STKWT,0 CHKPCS,0.00 CHKWT"
            strSql += vbCrLf + "  ,'' MAXDATE,'' MINDATE,'S' DATA "
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + "  WHERE T.ISSDATE IS NULL " + Filter()
            strSql += vbCrLf + "  GROUP BY ITEMID"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT ITEMID,0 STKPCS,0.00 STKWT,SUM(T.PCS)CHKPCS,SUM(T.GRSWT)CHKWT"
            strSql += vbCrLf + "  ,MAX(ISNULL(CHKDATE,''))MAXDATE,MIN(ISNULL(CHKDATE,''))MINDATE,'C' DATA"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + "  WHERE T.ISSDATE IS NULL AND ISNULL(CHKTRAY,'') <> '' " + Filter()
            strSql += vbCrLf + "  GROUP BY ITEMID)X GROUP BY ITEMID,MAXDATE,MINDATE,DATA"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()


            strSql = "  SELECT ITEMID,SUM(STKPCS)STKPCS,SUM(STKWT)STKWT,SUM(CHKPCS)CHKPCS,SUM(CHKWT)CHKWT"
            strSql += vbCrLf + "  ,(SELECT TOP 1 MINDATE FROM TEMPTABLEDB..TEMPSTOCKCHECKTABLE WHERE DATA='C') MINDATE"
            strSql += vbCrLf + "  ,(SELECT TOP 1 MAXDATE FROM TEMPTABLEDB..TEMPSTOCKCHECKTABLE WHERE DATA='C') MAXDATE"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPSTOCKCHECKTABLE"
            strSql += vbCrLf + "  GROUP BY ITEMID"
            Dim dtgen As New DataTable
            dtgen = GetSqlTable(strSql, cn)
            Dim maincostid As String
            strSql = "SELECT TOP 1 COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE ISNULL(MAIN,'')='Y'"
            maincostid = GetSqlValue(cn, strSql)
            If dtgen.Rows.Count > 0 Then
                Dim serverdate As String = GetServerDate()
                tran = Nothing
                tran = cn.BeginTransaction

                strSql = " DELETE FROM " & cnAdminDb & "..STOCK_CHKREPORT WHERE REPORTDATE='" & serverdate & "' AND COSTID='" & cnCostId & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, maincostid)

                For i As Integer = 0 To dtgen.Rows.Count - 1
                    Dim sno As String = GetNewSno(TranSnoType.STOCK_CHKREPORT, tran, "GET_ADMINSNO_TRAN")
                    strSql = " INSERT INTO " & cnAdminDb & "..STOCK_CHKREPORT(SNO,COSTID,GENERATEDBY,REPORTDATE,CHKFROMDATE,CHKTODATE,ITEMID,STKPCS,STKWT,CHKPCS,CHKWT)"
                    strSql += vbCrLf + " VALUES("
                    strSql += vbCrLf + " '" & sno & "'" 'SNO
                    strSql += vbCrLf + " ,'" & cnCostId & "'" 'COSTID
                    strSql += vbCrLf + " ,'" & userId & "'" 'GENERATED BY
                    strSql += vbCrLf + " ,'" & serverdate & "'" 'REPORTDATE
                    strSql += vbCrLf + " ,'" & dtgen.Rows(i).Item("MINDATE").ToString & "'" 'CHKFROMDATE
                    strSql += vbCrLf + " ,'" & dtgen.Rows(i).Item("MAXDATE").ToString & "'" 'CHKTODATE
                    strSql += vbCrLf + " ,'" & dtgen.Rows(i).Item("ITEMID").ToString & "'" 'ITEMID
                    strSql += vbCrLf + " ,'" & dtgen.Rows(i).Item("STKPCS").ToString & "'" 'STKPCS
                    strSql += vbCrLf + " ,'" & dtgen.Rows(i).Item("STKWT").ToString & "'"  'STKWT
                    strSql += vbCrLf + " ,'" & dtgen.Rows(i).Item("CHKPCS").ToString & "'"  'CHKPCS
                    strSql += vbCrLf + " ,'" & dtgen.Rows(i).Item("CHKWT").ToString & "'"  'CHKWT
                    strSql += vbCrLf + " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, maincostid)
                Next
                tran.Commit()
                btnNew_Click(Me, New EventArgs())
                MsgBox("Report Generated.")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Function Filter() As String

        Dim retvalue As String = ""
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            retvalue += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        If chkCmbItemName.Text <> "ALL" And chkCmbItemName.Text <> "" Then
            retvalue += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItemName.Text) & "))"
        End If
        If chkCmbDesigner.Text <> "ALL" And chkCmbDesigner.Text <> "" Then
            retvalue += vbCrLf + " AND T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & GetQryString(chkCmbDesigner.Text) & "))"
        End If
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            retvalue += vbCrLf + " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            retvalue += vbCrLf + " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        If chkCmbCounter.Text <> "ALL" And chkCmbCounter.Text <> "" Then
            retvalue += vbCrLf + " AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & GetQryString(chkCmbCounter.Text) & "))"
        End If
        Return retvalue
    End Function

    Private Sub RbtChecked_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtChecked.CheckedChanged
        If RbtChecked.Checked And rbtDetailed.Checked Then
            chkWithDiffwt.Enabled = True
        Else
            chkWithDiffwt.Enabled = False
        End If
    End Sub

    Private Sub txtItemCode_NUM_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItemCode_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtItemCode_NUM.Text) > 0 Then
                Dim tempitem As String = GetSqlValue(cn, "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & Val(txtItemCode_NUM.Text) & "' AND ACTIVE = 'Y' AND STOCKTYPE = 'T'")
                If tempitem Is Nothing Then tempitem = "ALL" : txtItemCode_NUM.Text = ""
                Funcfillitemname(tempitem.ToString)
            End If
        End If
    End Sub
End Class


Public Class FRM_STOCKCHECK_REPORT_Properties
    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCmbCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
        End Set
    End Property
    Private chkCmbItemName As New List(Of String) '= New List(Of String)(New String() {"ALL"})
    Public Property p_chkCmbItemName() As List(Of String)
        Get
            Return chkCmbItemName
        End Get
        Set(ByVal value As List(Of String))
            chkCmbItemName = value
        End Set
    End Property
    Private chkCmbDesigner As New List(Of String) '= New List(Of String)(New String() {"ALL"})
    Public Property p_chkCmbDesigner() As List(Of String)
        Get
            Return chkCmbDesigner
        End Get
        Set(ByVal value As List(Of String))
            chkCmbDesigner = value
        End Set
    End Property
    Private chkCmbCostCentre As New List(Of String) '= New List(Of String)(New String() {"ALL"})
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property
    Private chkCmbCounter As New List(Of String)
    Public Property p_chkCmbCounter() As List(Of String)
        Get
            Return chkCmbCounter
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCounter = value
        End Set
    End Property
End Class