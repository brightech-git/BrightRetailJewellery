Imports System.Data.OleDb
Public Class frmTagTransferStockReport
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dtGridView As DataTable
    Dim defalutDestination As String
    Dim ReplSubItemid As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_NEW_SUBID", "N") = "Y", True, False)
    Dim ReplRecdate As Boolean = False 'IIf(GetAdmindbSoftValue("STKTRAN_REC_DATE", "N") = "Y", True, False)
    Dim PRODTAGSEP As String = GetAdmindbSoftValue("PRODTAGSEP")
    Dim BARCODE2DSEP As String = GetAdmindbSoftValue("BARCODE2DSEP")
    Dim AUTOBOOKTRAN As Boolean = IIf(GetAdmindbSoftValue("AUTOBOOKTRAN", "N") = "Y", True, False)
    Dim IS_IMAGE_TRF As Boolean = IIf(GetAdmindbSoftValue("STKTRANWITHIMAGE", "N") = "Y", True, False)
    Dim Isbulkupdate As Boolean = IIf(GetAdmindbSoftValue("BULKTAGTRANSFER", "N") = "Y", True, False)
    Dim NonTag_trf_Lotno As Boolean = IIf(GetAdmindbSoftValue("NONTAG_TRF_LOTNO ", "N") = "Y", True, False)
    Dim AUTOBOOKVALUE As String = GetAdmindbSoftValue("AUTOBOOKVALUE", "N,0,0,0,0,0")
    Dim AUTOINTERNAL_VOUCHER As String = GetAdmindbSoftValue("AUTOBOOK_VOUCHER", "N")
    Dim IsSearchKeyRestrict As Boolean = IIf(GetAdmindbSoftValue("RESTRICT_SEARCHKEY", "N") = "Y", True, False)
    Dim STKTRAN_MULTIMETAL As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_MULTIMETAL", "Y") = "Y", True, False)
    Dim CENTR_DB_GLB As Boolean = IIf(GetAdmindbSoftValue("CENTR_DB_ALLCOSTID", "N") = "Y", True, False)
    Dim INTRBRANCHTRF_DISABLE As Boolean = IIf(GetAdmindbSoftValue("RESTRICT_INTRBRTRF", "N") = "Y", True, False)
    Dim TagTranCorpOnly As Boolean = IIf(GetAdmindbSoftValue("TAGTRANCORPONLY", "N") = "Y", True, False)
    Dim AUTOBOOKVALUEARRY() As String = Split(AUTOBOOKVALUE, ",")
    Dim AUTOBOOKVALUEENABLE As String
    Dim AUTOBOOKVALUEG_PER As Decimal
    Dim AUTOBOOKVALUES_PER As Decimal
    Dim AUTOBOOKVALUEP_PER As Decimal
    Dim AUTOBOOKVALUED_PER As Decimal
    Dim AUTOBOOKVALUET_PER As Decimal
    Dim TransistNo As Integer
    Dim XCnAdmin As OleDbConnection = Nothing
    Public Xtran As OleDbTransaction = Nothing
    Private XSyncdb As String = Replace(cnAdminDb, "ADMINDB", "UTILDB")
    Dim BillDate As Date
    Dim TranNo As Integer
    Dim Batchno As String    
    Dim GEN_SKUFILE As Boolean = IIf(GetAdmindbSoftValue("GEN_SKUFILE", "N") = "Y", True, False)
    Dim SKUFILEPATH As String = GetAdmindbSoftValue("SKUFILEPATH", "")

    Private Sub frmTagTransferStockReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTagNo.Focused Then Exit Sub
            If txtItemId.Focused Then Exit Sub
            'If txtNetWt_WET.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmTagTransferStockReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dtMetal As New DataTable
        Dim dtCounter As New DataTable
        dtGridView = New DataTable
        dtGridView.Columns.Add("SNO", GetType(String))
        dtGridView.Columns.Add("COUNTER", GetType(String))
        dtGridView.Columns.Add("METALID", GetType(String))
        dtGridView.Columns.Add("ITEM", GetType(String))
        dtGridView.Columns.Add("SUBITEM", GetType(String))
        dtGridView.Columns.Add("TAGNO", GetType(String))
        dtGridView.Columns.Add("PCS", GetType(Integer))
        dtGridView.Columns.Add("GRSWT", GetType(Decimal))
        dtGridView.Columns.Add("LESSWT", GetType(Decimal))
        dtGridView.Columns.Add("NETWT", GetType(Decimal))
        dtGridView.Columns.Add("STNPCS", GetType(Integer))
        dtGridView.Columns.Add("STNWT", GetType(Decimal))
        dtGridView.Columns.Add("PSTNPCS", GetType(Integer))
        dtGridView.Columns.Add("PSTNWT", GetType(Decimal))
        dtGridView.Columns.Add("DIAPCS", GetType(Integer))
        dtGridView.Columns.Add("DIAWT", GetType(Decimal))
        dtGridView.Columns.Add("SALVALUE", GetType(Double))
        dtGridView.Columns.Add("RATE", GetType(Double))
        dtGridView.Columns.Add("COLHEAD", GetType(String))
        dtGridView.Columns.Add("PCTFILE", GetType(String))
        dtGridView.Columns.Add("STOCKMODE", GetType(String))
        dtGridView.Columns.Add("SALEMODE", GetType(String))
        dtGridView.Columns.Add("DESIGNERID", GetType(Integer))
        dtGridView.AcceptChanges()
        gridView.DataSource = dtGridView
        With gridView
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
            .Columns("LESSWT").DefaultCellStyle.Format = "0.000"
            .Columns("NETWT").DefaultCellStyle.Format = "0.000"
            .Columns("STNWT").DefaultCellStyle.Format = "0.000"
            .Columns("PSTNWT").DefaultCellStyle.Format = "0.000"
            .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
            .Columns("COLHEAD").Visible = False
            .Columns("SNO").Visible = False
            .Columns("STOCKMODE").Visible = False
            .Columns("DESIGNERID").Visible = False
            .Columns("METALID").Visible = False
            .Columns("RATE").Visible = False
            .Columns("PCTFILE").Visible = False
            gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
        End With
        cmbCostCentre_MAN.Items.Clear()
        Dim MainCostId As String = GetAdmindbSoftValue("SYNC-TO", "")
        strSql = "SELECT MAIN FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID = '" & cnCostId & "'"
        Dim ThisCO As Boolean = IIf(objGPack.GetSqlValue(strSql).ToString = "Y", True, False)


        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID <> '" & cnCostId & "'"
        If TagTranCorpOnly = True And ThisCO = False Then
            If strBCostid Is Nothing Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE ISNULL(ACTIVE,'')<>'N')"
        Else
            If strBCostid Is Nothing Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE ISNULL(ACTIVE,'')<>'N')"
        End If


        If INTRBRANCHTRF_DISABLE Then
            If Not ThisCO And MainCostId <> "" Then strSql += " AND COSTID ='" & MainCostId & "'"
        End If
        strSql += " ORDER BY COSTNAME"
        objGPack.FillCombo(strSql, cmbCostCentre_MAN)

        '' LOAD METAL
        strSql = vbCrLf + " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT METALNAME,CONVERT(VARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        strSql += vbCrLf + " ORDER BY RESULT,METALNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        cmbMetal.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(cmbMetal, dtMetal, "METALNAME", , "ALL")

        strSql = vbCrLf + " SELECT 'ALL' COUNTERNAME,'ALL' COUNTERID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ITEMCTRNAME as COUNTERNAME,CONVERT(VARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY RESULT,COUNTERNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCounter)
        cmbCounter_OWN.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(cmbCounter_OWN, dtCounter, "COUNTERNAME", , "ALL")
        If ReplSubItemid = False Then
            cmbSubitem.Visible = False
            lblSubitemid.Visible = False
        End If
        btnNew_Click(Me, New EventArgs)
        cmbCostCentre_MAN.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        dtGridView.Rows.Clear()
        cmbCounter_OWN.Text = ""        
        'chkCheckByScan.Checked = False
        chkRecDate.Checked = True
        chkRecDate.Checked = False
        dtpDate.Value = GetEntryDate(GetServerDate)
        dtGridView.Rows.Clear()
        dtGridView.Clear()
        cmbCostCentre_MAN.Enabled = True
        AddGrandTotal()
        cmbCostCentre_MAN.Focus()
        gridView.DataSource = Nothing
        Prop_Gets()
    End Sub


    Private Sub AddGrandTotal()

        Dim Ro As DataRow = Nothing
        Dim pcs As Integer = Val(dtGridView.Compute("SUM(PCS)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim grsWt As Decimal = Val(dtGridView.Compute("SUM(GRSWT)", "COLHEAD <> 'G' or COLHEAD IS NULL or COLHEAD IS NULL").ToString)
        Dim lessWT As Decimal = Val(dtGridView.Compute("SUM(LESSWT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim netWt As Decimal = Val(dtGridView.Compute("SUM(NETWT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)

        Dim stnpcs As Decimal = Val(dtGridView.Compute("SUM(STNPCS)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim stnWt As Decimal = Val(dtGridView.Compute("SUM(STNWT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim Pstnpcs As Decimal = Val(dtGridView.Compute("SUM(PSTNPCS)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim PstnWt As Decimal = Val(dtGridView.Compute("SUM(PSTNWT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)

        Dim diaPcs As Decimal = Val(dtGridView.Compute("SUM(DIAPCS)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim diaWt As Decimal = Val(dtGridView.Compute("SUM(DIAWT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim salValue As Decimal = Val(dtGridView.Compute("SUM(SALVALUE)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        If dtGridView.Rows.Count > 0 Then
            Ro = dtGridView.Select("COLHEAD = 'G'")(0)
        Else
            Ro = dtGridView.NewRow
        End If
        'Ro = dtGridView.NewRow
        Ro.Item("ITEM") = "GRAND TOTAL"
        Ro.Item("PCS") = IIf(pcs <> 0, pcs, DBNull.Value)
        Ro.Item("GRSWT") = IIf(grsWt <> 0, Format(grsWt, "0.000"), DBNull.Value)
        Ro.Item("STNPCS") = IIf(stnpcs <> 0, stnpcs, DBNull.Value)
        Ro.Item("STNWT") = IIf(stnWt <> 0, Format(stnWt, "0.000"), DBNull.Value)
        Ro.Item("PSTNPCS") = IIf(stnpcs <> 0, Pstnpcs, DBNull.Value)
        Ro.Item("PSTNWT") = IIf(stnWt <> 0, Format(PstnWt, "0.000"), DBNull.Value)
        Ro.Item("DIAPCS") = IIf(diaPcs <> 0, diaPcs, DBNull.Value)
        Ro.Item("DIAWT") = IIf(diaWt <> 0, Format(diaWt, "0.000"), DBNull.Value)
        Ro.Item("LESSWT") = IIf(lessWT <> 0, Format(lessWT, "0.000"), DBNull.Value)
        Ro.Item("NETWT") = IIf(netWt <> 0, Format(netWt, "0.000"), DBNull.Value)
        Ro.Item("SALVALUE") = IIf(salValue <> 0, Format(salValue, "0.00"), DBNull.Value)
        Ro.Item("COLHEAD") = "G"
        If dtGridView.Rows.Count > 0 Then

        Else
            dtGridView.Rows.Add(Ro)
        End If
        dtGridView.AcceptChanges()
        gridView.DataSource = dtGridView
        If gridView.Rows.Count > 0 Then gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = reportTotalStyle
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub chkRecDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRecDate.CheckedChanged
        Label10.Visible = Not chkRecDate.Checked
        Label10.Visible = Not chkRecDate.Checked
        If chkRecDate.Checked Then
            chkRecDate.Text = "As On Date"
            dtpTo.Visible = False
        Else
            chkRecDate.Text = "Date From"
            dtpTo.Visible = True
        End If
    End Sub
    
    Private Sub gridView_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridView.UserDeletedRow
        dtGridView.AcceptChanges()
        AddGrandTotal()
    End Sub

   


    Private Sub txtItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemId.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadSalesItemName()
        End If
    End Sub
    Private Sub LoadSalesItemName()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME,"
        strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
        strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
        strSql += " WHEN CALTYPE = 'B' THEN 'BOTH' ELSE 'METAL RATE' END AS CAL_TYPE,"
        strSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' "
        strSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END AS STOCKTYPE"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y'" '' AND STOCKTYPE = 'T'"

        Dim itemId As Integer = Val(BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, , , txtItemId.Text))
        If itemId > 0 Then

            txtItemId.Text = itemId
            If objGPack.GetSqlValue("select STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =" & itemId) = "N" Then Exit Sub
            LoadSalesItemNameDetail()
        Else
            txtItemId.Focus()
            txtItemId.SelectAll()
        End If
    End Sub

    Private Sub LoadSalesItemNameDetail()
        If txtItemId.Text = "" Then
            Exit Sub
        Else
            Me.SelectNextControl(txtItemId, True, True, True, True)
        End If
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "Tag Transfer Stock Report", gridView, BrightPosting.GExport.GExportType.Export)
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "Tag Transfer Stock Report", gridView, BrightPosting.GExport.GExportType.Print)
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim selMetalId As String = Nothing
        Dim selCounterId As String = Nothing
        If objGPack.Validator_Check(Me) Then Exit Sub
        If cmbMetal.Text = "ALL" Then
            strSql = " SELECT METALID FROM " & cnAdminDb & "..METALMAST"
            strSql += " ORDER BY METALNAME"
            Dim dt As New DataTable()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    selMetalId += "'" & dt.Rows(i).Item("METALID").ToString & "',"
                Next
                If selMetalId <> "" Then
                    selMetalId = Mid(selMetalId, 1, selMetalId.Length - 1)
                End If
            End If
        ElseIf cmbMetal.Text <> "" Then
            Dim sql As String = "SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(cmbMetal.Text) & ")"
            Dim dtMetal As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtMetal)
            If dtMetal.Rows.Count > 0 Then
                For j As Integer = 0 To dtMetal.Rows.Count - 1
                    selMetalId += "'"
                    selMetalId += dtMetal.Rows(j).Item("METALID").ToString + ""
                    selMetalId += "'"
                    selMetalId += ","
                Next
                If selMetalId <> "" Then
                    selMetalId = Mid(selMetalId, 1, selMetalId.Length - 1)
                End If
            End If
        End If
        If chkCounter.Checked = True Then
            Dim Selectmetals() As String = selMetalId.Split(",")
            If Not STKTRAN_MULTIMETAL And Selectmetals.Length > 1 Then MsgBox("Multi metal items transfer not possible", MsgBoxStyle.Information) : Exit Sub
        End If
        'Dim dtrow() As DataRow = dtGridView.Select("METALID <> " & selMetalId & "")


        If cmbCounter_OWN.Text = "ALL" Then
            strSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER"
            strSql += " ORDER BY ITEMCTRNAME"
            Dim dt1 As New DataTable()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                For k As Integer = 0 To dt1.Rows.Count - 1
                    selCounterId += "'" & dt1.Rows(k).Item("ITEMCTRID").ToString & "',"
                Next
                If selCounterId <> "" Then
                    selCounterId = Mid(selCounterId, 1, selCounterId.Length - 1)
                End If
            End If
        ElseIf cmbCounter_OWN.Text <> "" Then
            Dim sql As String = "SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & GetQryString(cmbCounter_OWN.Text) & ")"
            Dim dtCounter As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtCounter)
            If dtCounter.Rows.Count > 0 Then
                For m As Integer = 0 To dtCounter.Rows.Count - 1
                    selCounterId += "'" & dtCounter.Rows(m).Item("ITEMCTRID").ToString + "',"
                Next
                If selCounterId <> "" Then
                    selCounterId = Mid(selCounterId, 1, selCounterId.Length - 1)
                End If
            End If
        End If

        Dim STONEDIAMETAL As String = "'D','T'"
        If chkCounter.Checked Then
            If gridView.Rows.Count > 1 Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER1 "
                strSql += vbCrLf + " SELECT SNO,IT.ITEMNAME ITEM"
                If ReplSubItemid Then
                    strSql += vbCrLf + " ,'" & cmbSubitem.Text & "' SUBITEM"
                Else
                    strSql += vbCrLf + " ,SUB.SUBITEMNAME SUBITEM"
                End If
                strSql += vbCrLf + " ,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT"
                strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
                strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PSTNPCS"
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PSTNWT"
                strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
                strSql += vbCrLf + " ,SALVALUE,RATE"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,T.PCTFILE"
                strSql += vbCrLf + " ,CONVERT(INT,1) AS RESULT,IT.ITEMNAME TITEM,CTR.ITEMCTRNAME TCOUNTER,CTR.ITEMCTRNAME COUNTER  "
                strSql += vbCrLf + ",IT.METALID METALID,M.METALNAME  TMETAL,'T' STOCKMODE,SALEMODE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
                If ReplSubItemid = False Then
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SUB ON SUB.SUBITEMID = T.SUBITEMID"
                End If
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID= T.ITEMCTRID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID= IT.METALID "
                strSql += vbCrLf + " WHERE ISSDATE IS NULL"
                strSql += vbCrLf + " AND SNO NOT IN (SELECT SNO FROM TEMPTABLEDB..TEMPTAGTRANSFER1)"
                strSql += vbCrLf + " AND IT.METALID IN (" & selMetalId & ")"
                strSql += vbCrLf + " AND T.ITEMCTRID IN (" & selCounterId & ")"
                strSql += vbCrLf + " AND ISNULL(APPROVAL,'') = ''"
                If Val(txtLotNo_NUM.Text) <> 0 Then
                    strSql += vbCrLf + " AND T.LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
                End If
                If chkCounter.Checked = False Then
                    If cmbCounter_OWN.Text <> "" And cmbCounter_OWN.Text <> "ALL" Then
                        strSql += vbCrLf + " AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_OWN.Text & "')"
                    End If
                End If
                If chkRecDate.Checked = True Then
                    strSql += vbCrLf + " AND RECDATE <= '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
                Else
                    strSql += vbCrLf + " AND RECDATE BETWEEN '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                End If

                If txtTagNo.Text <> "" Then strSql += vbCrLf + " AND T.TAGNO = '" & txtTagNo.Text & "'"
                If Val(txtItemId.Text) > 0 Then strSql += vbCrLf + " AND t.ITEMID = " & Val(txtItemId.Text) & ""
                If Val(txtEstNo.Text) <> 0 Then strSql += vbCrLf + " AND CAST(T.ITEMID AS VARCHAR)+TAGNO IN (SELECT CAST(T.ITEMID AS VARCHAR)+TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"
                strSql += vbCrLf + " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "')"
                strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                strSql += vbCrLf + " AND IT.METALID NOT IN(" & STONEDIAMETAL & ")"                
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT SNO,IT.ITEMNAME ITEM"
                If ReplSubItemid Then
                    strSql += vbCrLf + " ,'" & cmbSubitem.Text & "' SUBITEM"
                Else
                    strSql += vbCrLf + " ,SUB.SUBITEMNAME SUBITEM"
                End If
                strSql += vbCrLf + " ,T.TAGNO,0 PCS,0 GRSWT,0 LESSWT,0 NETWT"
                strSql += vbCrLf + " ,CASE WHEN IT.DIASTONE = 'T' THEN T.PCS ELSE 0 END STNPCS,CASE WHEN IT.DIASTONE = 'T' THEN T.GRSWT ELSE 0 END STNWT"
                strSql += vbCrLf + " ,CASE WHEN IT.DIASTONE = 'P' THEN T.PCS ELSE 0 END PSTNPCS,CASE WHEN IT.DIASTONE = 'P' THEN T.GRSWT ELSE 0 END PSTNWT"
                strSql += vbCrLf + " ,CASE WHEN IT.DIASTONE = 'D' THEN T.PCS ELSE 0 END DIAPCS,CASE WHEN IT.DIASTONE = 'T' THEN T.GRSWT ELSE 0 END DIAWT"
                strSql += vbCrLf + " ,SALVALUE,RATE"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,T.PCTFILE"
                strSql += vbCrLf + " ,CONVERT(INT,1) AS RESULT,IT.ITEMNAME TITEM,CTR.ITEMCTRNAME TCOUNTER,CTR.ITEMCTRNAME COUNTER  "
                strSql += vbCrLf + ",IT.METALID METALID,M.METALNAME  TMETAL,'T' STOCKMODE,SALEMODE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
                If ReplSubItemid = False Then
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SUB ON SUB.SUBITEMID = T.SUBITEMID"
                End If
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID= T.ITEMCTRID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID= IT.METALID "
                strSql += vbCrLf + " WHERE ISSDATE IS NULL"
                strSql += vbCrLf + " AND SNO NOT IN (SELECT SNO FROM TEMPTABLEDB..TEMPTAGTRANSFER1)"
                strSql += vbCrLf + " AND IT.METALID IN (" & selMetalId & ")"
                strSql += vbCrLf + " AND T.ITEMCTRID IN (" & selCounterId & ")"
                strSql += vbCrLf + " AND ISNULL(APPROVAL,'') = ''"
                If Val(txtLotNo_NUM.Text) <> 0 Then
                    strSql += vbCrLf + " AND T.LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
                End If
                If chkCounter.Checked = False Then
                    If cmbCounter_OWN.Text <> "" And cmbCounter_OWN.Text <> "ALL" Then
                        strSql += vbCrLf + " AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_OWN.Text & "')"
                    End If
                End If
                If chkRecDate.Checked = True Then
                    strSql += vbCrLf + " AND RECDATE <= '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
                Else
                    strSql += vbCrLf + " AND RECDATE BETWEEN '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                End If
                If txtTagNo.Text <> "" Then strSql += vbCrLf + " AND T.TAGNO = '" & txtTagNo.Text & "'"
                If Val(txtItemId.Text) > 0 Then strSql += vbCrLf + " AND t.ITEMID = " & Val(txtItemId.Text) & ""
                If Val(txtEstNo.Text) <> 0 Then strSql += vbCrLf + " AND CAST(T.ITEMID AS VARCHAR)+TAGNO IN (SELECT CAST(T.ITEMID AS VARCHAR)+TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"
                strSql += vbCrLf + " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "')"
                strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                strSql += vbCrLf + " AND IT.METALID IN(" & STONEDIAMETAL & ")"                
                cmd = New OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            Else
                strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPTAGTRANSFER1') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPTAGTRANSFER1 "
                strSql += vbCrLf + " SELECT SNO,IT.ITEMNAME ITEM"
                If ReplSubItemid Then
                    strSql += vbCrLf + " ,'" & cmbSubitem.Text & "' SUBITEM"
                Else
                    strSql += vbCrLf + " ,SUB.SUBITEMNAME SUBITEM"
                End If
                strSql += vbCrLf + " ,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT"
                strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
                strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PSTNPCS"
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PSTNWT"
                strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
                strSql += vbCrLf + " ,SALVALUE,RATE"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,T.PCTFILE"
                strSql += vbCrLf + " ,CONVERT(INT,1) AS RESULT,IT.ITEMNAME TITEM,CTR.ITEMCTRNAME TCOUNTER,CTR.ITEMCTRNAME COUNTER  "
                strSql += vbCrLf + ",IT.METALID METALID,M.METALNAME  TMETAL,'T' STOCKMODE,SALEMODE"
                strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTAGTRANSFER1"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
                If ReplSubItemid = False Then
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SUB ON SUB.SUBITEMID = T.SUBITEMID"
                End If
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID= T.ITEMCTRID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID= IT.METALID "
                strSql += vbCrLf + " WHERE ISSDATE IS NULL"
                'strSql += vbCrLf + " AND (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID) IN (" & selMetalId & ")"
                strSql += vbCrLf + " AND IT.METALID IN (" & selMetalId & ")"
                'strSql += vbCrLf + " AND T.ITEMCTRID IN (" & selCounterId & ")"
                strSql += vbCrLf + " AND IT.METALID NOT IN(" & STONEDIAMETAL & ")"
                strSql += vbCrLf + " AND ISNULL(APPROVAL,'') = ''"
                If Val(txtLotNo_NUM.Text) <> 0 Then
                    strSql += vbCrLf + " AND T.LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
                End If
                If chkCounter.Checked = False Then
                    If cmbCounter_OWN.Text <> "" And cmbCounter_OWN.Text <> "ALL" Then strSql += vbCrLf & " AND T.ITEMCTRID IN (" & selCounterId & ")"
                End If
                If chkRecDate.Checked = True Then
                    strSql += vbCrLf + " AND RECDATE <= '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
                Else
                    strSql += vbCrLf + " AND RECDATE BETWEEN '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                End If
                If txtTagNo.Text <> "" Then strSql += vbCrLf + " AND T.TAGNO = '" & txtTagNo.Text & "'"
                If Val(txtItemId.Text) > 0 Then strSql += vbCrLf + " AND t.ITEMID = " & Val(txtItemId.Text) & ""
                If Val(txtEstNo.Text) <> 0 Then strSql += vbCrLf + " AND CAST(T.ITEMID AS VARCHAR)+TAGNO IN (SELECT CAST(T.ITEMID AS VARCHAR)+TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"

                strSql += vbCrLf + " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "')"

                strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"                
                cmd = New OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPTAGTRANSFER') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPTAGTRANSFER"
            strSql += vbCrLf + " SELECT * INTO TEMPTABLEDB..TEMPTAGTRANSFER FROM TEMPTABLEDB..TEMPTAGTRANSFER1 ORDER BY TCOUNTER,TMETAL,TITEM,RESULT,COLHEAD"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            'strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPTAGTRANSFER1 ORDER BY TCOUNTER,TMETAL,TITEM,RESULT,COLHEAD"

            'Dim dtTempx As New DataTable
            'dtTemp.Clear()
            'gridView.DataSource = Nothing
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dtTempx)
            'For Each ro As DataRow In dtGridView.Select("Sno<>''")
            '    dtTempx.ImportRow(ro)
            'Next


            strSql = vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPTAGTRANSFER)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER(TCOUNTER,TMETAL,ITEM,SUBITEM,TAGNO,RESULT,COLHEAD,SNO,STOCKMODE,LESSWT,STNPCS,STNWT,DIAPCS,DIAWT,SALVALUE,RATE,PCTFILE,SALEMODE)"
            strSql += vbCrLf + " SELECT DISTINCT TCOUNTER ,TMETAL,TCOUNTER,'' SUBITEM,'' TAGNO,0 RESULT,'T' COLHEAD,'' SNO,'' STOCKMODE,0 LESSWT,0 STNPCS,0 STNWT,0 DIAPCS,0 DIAWT,0 SALVALUE,0 RATE,'' PCTFILE,'' SALEMODE  FROM TEMPTABLEDB..TEMPTAGTRANSFER "

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER (TCOUNTER,TMETAL,ITEM,SUBITEM,TAGNO,RESULT,COLHEAD,SNO,STOCKMODE,LESSWT,STNPCS,STNWT,DIAPCS,DIAWT,SALVALUE,RATE,PCTFILE,SALEMODE)"
            strSql += vbCrLf + " SELECT DISTINCT TCOUNTER,TMETAL,TMETAL,'' SUBITEM,''TAGNO,6 RESULT,'N' COLHEAD,'' SNO ,'' STOCKMODE,0 LESSWT,0 STNPCS,0 STNWT,0 DIAPCS,0 DIAWT,0 SALVALUE,0 RATE,'' PCTFILE,SALEMODE  FROM TEMPTABLEDB..TEMPTAGTRANSFER"

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER (TCOUNTER,TMETAL,TITEM,ITEM,SUBITEM,TAGNO,RESULT,COLHEAD,PCS,GRSWT,LESSWT,NETWT,STNPCS,STNWT,DIAPCS,DIAWT,SALVALUE,RATE,SNO,STOCKMODE,PCTFILE,SALEMODE)"
            strSql += vbCrLf + " SELECT DISTINCT TCOUNTER,TMETAL,TITEM,'ITEM SUB TOTAL','' SUBITEM,''TAGNO,2 RESULT,'S' COLHEAD,"
            strSql += vbCrLf + " SUM(PCS),SUM(GRSWT),SUM(LESSWT),SUM(NETWT),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(SALVALUE),0 RATE,'' SNO,'' STOCKMODE,''PCTFILE,'' SALEMODE"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGTRANSFER GROUP BY TCOUNTER,TMETAL,TITEM,SALEMODE"

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER(TCOUNTER,TMETAL,ITEM,SUBITEM,TAGNO,RESULT,COLHEAD,PCS,GRSWT,LESSWT,NETWT,STNPCS,STNWT,DIAPCS,DIAWT,SALVALUE,RATE,SNO,TITEM,STOCKMODE,PCTFILE,SALEMODE)  "
            strSql += vbCrLf + " SELECT DISTINCT 'ZZZZZZZ','ZZZZZZZZZZZZ','GRAND TOTAL','' SUBITEM,'' TAGNO,3 RESULT,'G' COLHEAD"
            strSql += vbCrLf + " ,SUM(PCS),SUM(GRSWT),SUM(LESSWT),SUM(NETWT),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(SALVALUE),0 RATE,'' SNO,'GRAND TOTAL','' STOCKMODE,'' PCTFILE,''SALEMODE"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGTRANSFER WHERE RESULT IN (1) "
            strSql += vbCrLf + " End"

            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " DELETE FROM TEMPTABLEDB..TEMPTAGTRANSFER WHERE TITEM IS NULL AND COLHEAD='S'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPTAGTRANSFER ORDER BY TCOUNTER,TMETAL,TITEM,RESULT,COLHEAD"

            If (cmbCounter_OWN.Text = "" Or cmbCounter_OWN.Text <> "ALL") And dtpDate.Enabled = False And Val(txtItemId.Text) = 0 And txtTagNo.Text = "" And Val(txtLotNo_NUM.Text) = 0 Then
                If MessageBox.Show("Do you want to load all tag items?", "Tag Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                End If
            End If
            Dim dtTemp As New DataTable
            'dtTemp.Clear()
            'gridView.DataSource = Nothing
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            'dtTemp = dtTempx.Copy
            If Not dtTemp.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                txtTagNo.Focus()
                'If chkCheckByScan.Checked Then
                '    txtTagNo.Focus()
                'End If
                Exit Sub
                'ElseIf dtTemp.Rows.Count > 0 Then
                '    If grpNonTag.Visible Then txtItemId.Focus() Else txtTagNo.Focus()
            End If

            Dim DTMETALDET As New DataTable
            If cmbMetal.Text = "ALL" Then
                strSql = " SELECT M.METALNAME  ITEM,'' as SUBITEM"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT,SUM(SALVALUE)SALVALUE,'M' COLHEAD,0 RESULT"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGTRANSFER T"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALNAME = T.TMETAL WHERE TMETAL IN (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST)"
                strSql += vbCrLf + " AND COLHEAD='S'"
                strSql += vbCrLf + " GROUP BY M.METALNAME ,TMETAL"

                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(DTMETALDET)
                If DTMETALDET.Rows.Count > 0 Then
                    dtTemp.Merge(DTMETALDET)
                End If
            ElseIf cmbMetal.Text <> "" Then
                strSql = " SELECT M.METALNAME  ITEM,'' as SUBITEM"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT,SUM(SALVALUE)SALVALUE,'M' COLHEAD,0 RESULT"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGTRANSFER T "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALNAME = T.TMETAL WHERE TMETAL IN (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST)"
                strSql += vbCrLf + " AND COLHEAD='S'"
                strSql += vbCrLf + " GROUP BY TMETAL,m.METALNAME"

                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(DTMETALDET)
                If DTMETALDET.Rows.Count > 0 Then
                    dtTemp.Merge(DTMETALDET)
                End If
            End If

            'For Each ro As DataRow In dtTemp.Rows
            '    Dim RowCol() As DataRow = dtGridView.Select("SNO = '" & ro.Item("SNO").ToString & "'")
            '    If RowCol.Length = 0 Then
            '        dtGridView.ImportRow(ro)
            '    End If
            'Next

            dtGridView.AcceptChanges()
            gridView.DataSource = dtTemp
            dtGridView = dtTemp.Copy
            With gridView

                For cnt As Integer = 0 To .ColumnCount - 1
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
                .Columns("LESSWT").DefaultCellStyle.Format = "0.000"
                .Columns("NETWT").DefaultCellStyle.Format = "0.000"
                .Columns("STNWT").DefaultCellStyle.Format = "0.000"
                .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
                .Columns("COLHEAD").Visible = False
                .Columns("RATE").Visible = False
                .Columns("SALVALUE").Visible = False
                .Columns("SNO").Visible = False
                .Columns("PCTFILE").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("TITEM").Visible = False
                .Columns("TCOUNTER").Visible = False
                .Columns("TMETAL").Visible = False
                .Columns("ITEM").Width = 150
                .Columns("SUBITEM").Width = 150
                'gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
            End With
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            'AddGrandTotal()
            GridViewFormat()
            'grpNonTag.Visible = False
            cmbCostCentre_MAN.Enabled = False
            'If chkCheckByScan.Checked Then

            '    txtTagNo.Clear()
            '    txtItemId.Select()
            '    If Val(txtEstNo.Text) <> 0 Then txtEstNo.Clear() : txtEstNo.Select()
            'End If
            Prop_Sets()
        ElseIf chkCounter.Checked = False Then

            strSql = " SELECT SNO,IT.ITEMNAME ITEM"
            If ReplSubItemid Then
                strSql += vbCrLf + " ,'" & cmbSubitem.Text & "' SUBITEM"
            Else
                strSql += vbCrLf + " ,SUB.SUBITEMNAME SUBITEM"
            End If
            strSql += " ,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT"
            strSql += " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
            strSql += " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            strSql += " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PSTNPCS"
            strSql += " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PSTNWT"
            strSql += " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            strSql += " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            strSql += " ,SALVALUE,RATE,IT.METALID METALID "
            strSql += " ,CTR.ITEMCTRNAME AS COUNTER"
            'strSql += " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS COUNTER"
            strSql += " ,CONVERT(VARCHAR(3),NULL)COLHEAD,T.PCTFILE,'T' AS STOCKMODE,SALEMODE"
            strSql += " FROM " & cnAdminDb & "..ITEMTAG T"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
            If ReplSubItemid = False Then
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SUB ON SUB.SUBITEMID = T.SUBITEMID"
            End If
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID= T.ITEMCTRID"
            strSql += " WHERE ISSDATE IS NULL"
            strSql += " AND ISNULL(APPROVAL,'') = ''"
            If Val(txtLotNo_NUM.Text) <> 0 Then
                strSql += " AND T.LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
            End If
            If cmbCounter_OWN.Text <> "" And cmbCounter_OWN.Text <> "ALL" Then
                strSql += " AND T.ITEMCTRID IN (" & selCounterId & ")"
                '(SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_OWN.Text & "')"
            End If
            If chkRecDate.Checked = True Then
                strSql += vbCrLf + " AND RECDATE <= '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + " AND RECDATE BETWEEN '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            End If
            If txtTagNo.Text <> "" Then strSql += " AND TAGNO = '" & txtTagNo.Text & "'"

            If Val(txtItemId.Text) > 0 Then strSql += " AND t.ITEMID = " & Val(txtItemId.Text) & ""

            If Val(txtEstNo.Text) <> 0 Then strSql += vbCrLf + " AND CAST(T.ITEMID AS VARCHAR)+TAGNO IN (SELECT CAST(T.ITEMID AS VARCHAR)+TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"

            strSql += " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "')"
            strSql += " AND COSTID = '" & cnCostId & "'"
            strSql += vbCrLf + " AND METALID NOT IN(" & STONEDIAMETAL & ")"            
            strSql += " UNION ALL"
            strSql += " SELECT SNO,IT.ITEMNAME ITEM"
            If ReplSubItemid Then
                strSql += vbCrLf + " ,'" & cmbSubitem.Text & "' SUBITEM"
            Else
                strSql += vbCrLf + " ,SUB.SUBITEMNAME SUBITEM"
            End If
            strSql += " ,T.TAGNO,0 PCS,0 GRSWT,0 LESSWT, 0 NETWT"
            strSql += vbCrLf + ",CASE WHEN IT.DIASTONE = 'S' THEN T.PCS ELSE 0 END STNPCS,CASE WHEN IT.DIASTONE = 'S' THEN T.GRSWT ELSE 0 END STNWT"
            strSql += vbCrLf + ",CASE WHEN IT.DIASTONE = 'P' THEN T.PCS ELSE 0 END PSTNPCS,CASE WHEN IT.DIASTONE = 'P' THEN T.GRSWT ELSE 0 END PSTNWT"
            strSql += vbCrLf + ",CASE WHEN IT.DIASTONE = 'D' THEN T.PCS ELSE 0 END DIAPCS,CASE WHEN IT.DIASTONE = 'D' THEN T.GRSWT ELSE 0 END DIAWT"
            strSql += " ,SALVALUE,RATE,IT.METALID METALID "
            strSql += " ,CTR.ITEMCTRNAME AS COUNTER"
            'strSql += " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS COUNTER"
            strSql += " ,CONVERT(VARCHAR(3),NULL)COLHEAD,T.PCTFILE,'T' AS STOCKMODE,SALEMODE"
            strSql += " FROM " & cnAdminDb & "..ITEMTAG T"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
            If ReplSubItemid = False Then
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SUB ON SUB.SUBITEMID = T.SUBITEMID"
            End If
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID= T.ITEMCTRID"
            strSql += " WHERE ISSDATE IS NULL"
            strSql += " AND ISNULL(APPROVAL,'') = ''"
            If Val(txtLotNo_NUM.Text) <> 0 Then
                strSql += " AND T.LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
            End If
            If cmbCounter_OWN.Text <> "" And cmbCounter_OWN.Text <> "ALL" Then strSql += " AND T.ITEMCTRID IN (" & selCounterId & ")"
            If chkRecDate.Checked = True Then
                strSql += vbCrLf + " AND RECDATE <= '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + " AND RECDATE BETWEEN '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            End If
            If txtTagNo.Text <> "" Then strSql += " AND TAGNO = '" & txtTagNo.Text & "'"
            If Val(txtItemId.Text) > 0 Then strSql += " AND t.ITEMID = " & Val(txtItemId.Text) & ""
            If Val(txtEstNo.Text) <> 0 Then strSql += vbCrLf + " AND CAST(T.ITEMID AS VARCHAR)+TAGNO IN (SELECT CAST(T.ITEMID AS VARCHAR)+TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"
            strSql += " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "')"
            strSql += " AND COSTID = '" & cnCostId & "'"
            strSql += vbCrLf + " AND METALID IN(" & STONEDIAMETAL & ")"            
            If (cmbCounter_OWN.Text = "" Or cmbCounter_OWN.Text <> "ALL") And dtpDate.Enabled = False And Val(txtItemId.Text) = 0 And txtTagNo.Text = "" And Val(txtLotNo_NUM.Text) = 0 Then
                If MessageBox.Show("Do you want to load all tag items?", "Tag Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                End If
            End If
            Dim dtTemp As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If Not dtTemp.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                'grpNonTag.Visible = False
                txtTagNo.Focus()
                'If chkCheckByScan.Checked Then
                '    txtTagNo.Focus()
                'End If
                If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
                Exit Sub
                'ElseIf dtTemp.Rows.Count > 0 Then
                '    If grpNonTag.Visible = True Then txtItemId.Focus() Else txtTagNo.Focus()
            End If
            Dim dMdt1 As New DataTable
            dMdt1 = dtTemp.DefaultView.ToTable(True, "METALID")
            If Not STKTRAN_MULTIMETAL And dMdt1.Rows.Count > 1 Then MsgBox("Multi metal items transfer not possible", MsgBoxStyle.Information) : Exit Sub
            Dim dMdt2 As New DataTable
            dMdt2 = dtGridView.DefaultView.ToTable(True, "METALID")
            Dim DmDt3 As New DataView
            DmDt3 = dMdt2.DefaultView
            DmDt3.RowFilter = "METALID<>''"
            dMdt2 = DmDt3.ToTable
            If Not STKTRAN_MULTIMETAL And dMdt2.Rows.Count > 1 Then MsgBox("Multi metal items transfer not possible", MsgBoxStyle.Information) : Exit Sub


            For Each ro As DataRow In dtTemp.Rows
                If ro.Item("STOCKMODE").ToString <> "N" Then
                    If Isgridexist(ro.Item("SNO").ToString, ro.Item("STOCKMODE").ToString) = False Then
                        dtGridView.ImportRow(ro)
                    End If
                Else
                    Dim RowCol() As DataRow = dtGridView.Select("SNO = '" & ro.Item("SNO").ToString & "' AND STOCKMODE = '" & ro.Item("STOCKMODE").ToString & "'")
                    If RowCol.Length = 0 Then
                        dtGridView.ImportRow(ro)
                    End If

                End If

            Next
            dtGridView.AcceptChanges()
            gridView.DataSource = dtTemp
            With gridView
                For cnt As Integer = 0 To .ColumnCount - 1
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PSTNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PSTNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
                .Columns("LESSWT").DefaultCellStyle.Format = "0.000"
                .Columns("NETWT").DefaultCellStyle.Format = "0.000"
                .Columns("STNWT").DefaultCellStyle.Format = "0.000"
                .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
                .Columns("COLHEAD").Visible = False
                .Columns("RATE").Visible = False
                .Columns("SNO").Visible = False
                .Columns("PCTFILE").Visible = False
                .Columns("SALVALUE").Visible = False

            End With
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            AddGrandTotal()
            gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
            cmbCostCentre_MAN.Enabled = False
            'grpNonTag.Visible = False
            If txtItemId.Text <> "" Then txtItemId.Clear() : txtItemId.Focus()
            If txtEstNo.Text <> "" Then txtEstNo.Clear() : txtEstNo.Focus()
            If txtTagNo.Text <> "" Then txtTagNo.Clear() : txtTagNo.Focus()

            'If chkCheckByScan.Checked Then
            '    txtTagNo.Clear()
            '    txtItemId.Select()
            '    If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
            'End If
            Prop_Sets()
        End If
    End Sub
    Private Function Isgridexist(ByVal sno As String, Optional ByVal stockmode As String = "") As Boolean
        For ii As Integer = 0 To gridView.Rows.Count - 1
            If gridView.Rows(ii).Cells("SNO").Value.ToString <> "" Then
                If gridView.Rows(ii).Cells("SNO").Value.ToString = sno And gridView.Rows(ii).Cells("STOCKMODE").Value.ToString = stockmode Then
                    Return True
                    Exit Function
                End If
            End If
        Next
        Return False
    End Function

    Private Sub cmbCostCentre_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre_MAN.LostFocus
        cmbTagsCostName_MAN.Items.Clear()
        cmbTagsCostName_MAN.Items.Add(cmbCostCentre_MAN.Text)
        cmbTagsCostName_MAN.Items.Add(cnCostName)
        cmbTagsCostName_MAN.Text = cmbCostCentre_MAN.Text
    End Sub
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "S"
                        .DefaultCellStyle.BackColor = Color.MistyRose
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "T"
                        .DefaultCellStyle.BackColor = reportHeadStyle.BackColor
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "N"
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "M"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub Prop_Sets()
        Dim obj As New frmTagTransferStockReport_Properties
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbCounter = cmbCounter_OWN.Text
        SetSettingsObj(obj, Me.Name, GetType(frmTagTransferStockReport_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTagTransferStockReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTagTransferStockReport_Properties))
        cmbMetal.Text = obj.p_cmbMetal
        cmbCounter_OWN.Text = obj.p_cmbCounter
    End Sub

    Private Sub chkCounter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCounter.CheckedChanged
        If chkCounter.Checked Then
            cmbMetal.Enabled = True
        Else
            cmbMetal.Enabled = False
        End If
    End Sub

    Private Sub txtNetWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then btnSearch.Focus() : Exit Sub

    End Sub
End Class

Public Class frmTagTransferStockReport_Properties
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private cmbCounter As String = "ALL"
    Public Property p_cmbCounter() As String
        Get
            Return cmbCounter
        End Get
        Set(ByVal value As String)
            cmbCounter = value
        End Set
    End Property
End Class
