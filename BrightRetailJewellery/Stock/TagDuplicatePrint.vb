Imports System.Data.OleDb
Imports System.IO
Public Class TagDuplicatePrint
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dtCostCentre As New DataTable
    Dim TAGSEARCH As Boolean = IIf(GetAdmindbSoftValue("TAGSEARCH", "N").ToUpper.ToString = "Y", True, False)
    Dim CallBarcodeExe As Boolean = IIf(GetAdmindbSoftValue("CALLBARCODEEXE", "N") = "Y", True, False)
    Dim TAG_DUP_SHOWREMARK As Boolean = IIf(GetAdmindbSoftValue("TAG_DUP_SHOWREMARK", "N") = "Y", True, False)
    Dim METALID As String = ""
    Dim objMiscRemark As frmBillRemark

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim flagchk_ As Boolean = False
        If chkDate.Checked = False And txtLotNo_NUM.Text = "" And txtItemId_NUM.Text = "" _
            And txtTagNoFrom.Text = "" And txttray.Text = "" And cmbCounter.Text = "ALL" And txtEstNo.Text.Trim = "" Then
            MsgBox("Please enter Filtration", MsgBoxStyle.Information)
            Exit Sub
        End If

        gridView.DataSource = Nothing
        If TAGSEARCH = False Then
            If txtTagNoFrom.Text = "" Then
                txtTagNoFrom.Text = txtTagNoTo.Text
            End If
            If txtTagNoTo.Text = "" Then
                txtTagNoTo.Text = txtTagNoFrom.Text
            End If
        End If

        strSql = " SELECT ITEMID"
        strSql += vbCrLf + " ,CASE WHEN SUBITEMID = 0 THEN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)"
        strSql += vbCrLf + " ELSE (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID) END AS PARTICULAR"
        strSql += vbCrLf + " ,TAGNO,PCS,GRSWT,NETWT,SALVALUE"
        strSql += vbCrLf + " ,TAGVAL,SNO"
        strSql += vbCrLf + " ,CASE WHEN SALEMODE = 'W' THEN 'WEIGHT' WHEN SALEMODE = 'R' THEN 'RATE' WHEN SALEMODE = 'B' THEN 'BOTH' WHEN SALEMODE =  'M' THEN 'METAL RATE' WHEN SALEMODE = 'F' THEN 'FIXED' ELSE SALEMODE END SALEMODE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " WHERE ISSDATE IS NULL"
        If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        If dtpTagRecDate.Enabled Then
            strSql += " AND RECDATE = '" & dtpTagRecDate.Value.ToString("yyyy-MM-dd") & "'"
        End If
        If Val(txtLotNo_NUM.Text) > 0 Then
            strSql += " AND LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
        End If
        If Val(txtItemId_NUM.Text) > 0 Then strSql += " AND ITEMID = " & Val(txtItemId_NUM.Text) & ""
        If cmbsubitem.Text <> "ALL" Then
            strSql += " AND SUBITEMID IN(SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbsubitem.Text & "')"
        End If
        If txttray.Text <> "" Then strSql += " AND CHKTRAY = '" & Trim(txttray.Text) & "' "
        If txtTagNoFrom.Text <> "" And TAGSEARCH = False Then
            strSql += " AND TAGVAL BETWEEN (SELECT TOP 1 TAGVAL FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=T.ITEMID AND TAGNO = '" & txtTagNoFrom.Text & "' " & IIf(Val(txtItemId_NUM.Text) > 0, " AND ITEMID = " & Val(txtItemId_NUM.Text) & "", "") & ")AND"
            strSql += " (SELECT TOP 1 TAGVAL FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=T.ITEMID AND TAGNO = '" & txtTagNoTo.Text & "' " & IIf(Val(txtItemId_NUM.Text) > 0, " AND ITEMID = " & Val(txtItemId_NUM.Text) & "", "") & ") "
        ElseIf txtTagNoFrom.Text <> "" And txtTagNoTo.Text <> "" And TAGSEARCH Then
            strSql += " AND TAGVAL BETWEEN (SELECT TOP 1 TAGVAL FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=T.ITEMID AND TAGNO = '" & txtTagNoFrom.Text & "' " & IIf(Val(txtItemId_NUM.Text) > 0, " AND ITEMID = " & Val(txtItemId_NUM.Text) & "", "") & ")AND"
            strSql += " (SELECT TOP 1 TAGVAL FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=T.ITEMID AND TAGNO = '" & txtTagNoTo.Text & "' " & IIf(Val(txtItemId_NUM.Text) > 0, " AND ITEMID = " & Val(txtItemId_NUM.Text) & "", "") & ") "
            flagchk_ = True
        ElseIf txtTagNoFrom.Text <> "" And TAGSEARCH And flagchk_ = False Then
            strSql += " AND TAGNO='" & txtTagNoFrom.Text & "' "
        End If
        If cmbCounter.Text <> "ALL" Then
            strSql += " AND ITEMCTRID IN(SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & cmbCounter.Text & "')"
        End If
        If cmbCostCentre.Text <> "ALL" Then
            strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre.Text & "')"
        End If
        If txtEstNo.Text <> "" Then
            'If condition = Nothing Then condition += " WHERE " Else condition += " AND "
            strSql += " AND TAGNO IN (SELECT DISTINCT TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO= '" & txtEstNo.Text & "')"
        End If
        If cmbCalType.Text <> "ALL" Then
            strSql += " AND SALEMODE = '" & Mid(cmbCalType.Text, 1, 1) & "'"
        End If
        strSql += vbCrLf + " ORDER BY TAGVAL"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("CHK", GetType(Boolean))
        dtGrid.Columns("CHK").DefaultValue = chkSelectAll.Checked
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found this selection criteria", MsgBoxStyle.Information)
            chkDate.Select()
            Exit Sub
        End If
        gridView.DataSource = dtGrid
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            gridView.Columns(cnt).ReadOnly = True
        Next
        gridView.Columns("CHK").HeaderText = ""
        gridView.Columns("CHK").ReadOnly = False
        gridView.Columns("TAGVAL").Visible = False
        gridView.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        gridView.Select()
        txtTagNoFrom.Clear()
        txtTagNoTo.Clear()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        ''COSTCENTRE
        strSql = vbCrLf + " SELECT 'ALL' COSTNAME,'ALL'COSTID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COSTNAME,COSTID,2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        cmbCostCentre.Items.Clear()
        For Each Row As DataRow In dtCostCentre.Rows
            cmbCostCentre.Items.Add(Row.Item("COSTNAME").ToString)
        Next
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre.Enabled = True
        Else
            cmbCostCentre.Enabled = False
        End If
        cmbCostCentre.Text = "ALL"
        ''Designer
        cmbCounter.Items.Clear()
        cmbCounter.Items.Add("ALL")
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRNAME"
        objGPack.FillCombo(strSql, cmbCounter, False)
        cmbCounter.Text = "ALL"

        cmbCalType.Items.Add("ALL")
        cmbCalType.Items.Add("WEIGHT")
        cmbCalType.Items.Add("RATE")
        cmbCalType.Items.Add("BOTH")
        cmbCalType.Items.Add("FIXED")
        cmbCalType.Items.Add("METAL RATE")
        cmbCalType.Items.Add("PIECES")
        cmbCalType.Text = "ALL"

        dtpTagRecDate.Value = GetEntryDate(GetServerDate)
        gridView.DataSource = Nothing
        chkDate.Checked = False
        chkSelectAll.Enabled = True
        chkSelectAll.Checked = True
        cmbsubitem.Text = "ALL"
        cmbsubitem.Enabled = False
        chkDate.Select()
    End Sub

    Private Sub TagDuplicatePrint_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name = "tabView" Then
                tabMain.SelectedTab = tabGen
                chkDate.Focus()
            End If
        End If
    End Sub

    Private Sub TagDuplicatePrint_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub chkDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        dtpTagRecDate.Enabled = chkDate.Checked
    End Sub

    Private Sub ShowRemark()
        If objMiscRemark.Visible Then Exit Sub
        objMiscRemark.Text = "Duplicate Remark"
        objMiscRemark.BackColor = Me.BackColor
        objMiscRemark.StartPosition = FormStartPosition.CenterScreen
        objMiscRemark.grpRemark.BackgroundColor = Color.Lavender
        objMiscRemark.ShowDialog()
    End Sub


    Private Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim dtSource As New DataTable
        If gridView.DataSource Is Nothing Then
            MsgBox("There is no record", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtSource = CType(gridView.DataSource, DataTable).Copy
        If Not dtSource.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim roSelected() As DataRow = dtSource.Select("CHK = TRUE")
        If Not roSelected.Length > 0 Then
            MsgBox("There is no selected record", MsgBoxStyle.Information)
            If gridView.RowCount > 0 Then gridView.Select() Else btnSearch.Focus()
            Exit Sub
        End If
        objMiscRemark = New frmBillRemark
        If TAG_DUP_SHOWREMARK Then
            ShowRemark()
        End If
        If CallBarcodeExe = False Then
            For Each ro As DataRow In roSelected
                strSql = "UPDATE " & cnAdminDb & "..ITEMTAG SET RESDATE = '" & GetServerDate() & "' WHERE SNO = '" & ro!SNO & "'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                Dim objBar As New clsBarcodePrint
                strSql = "SELECT (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)METALID FROM"
                strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG AS T WHERE SNO = '" & ro!SNO & "'"
                METALID = objGPack.GetSqlValue(strSql, "", "")
                If METALID = "G" Then
                    objBar.FuncprintBarcode_Single(ro!ITEMID.ToString, ro!TAGNO.ToString)
                Else
                    If Val(GetAdmindbSoftValue("BARCODE_FORMAT", "1")) <= "1" Then
                        objBar.FuncprintBarcode_Single(ro!ITEMID.ToString, ro!TAGNO.ToString)
                    Else
                        FuncprintBarcode_Multi(ro!ITEMID.ToString, ro!TAGNO.ToString, Val(GetAdmindbSoftValue("BARCODE_FORMAT", "1")))
                    End If
                End If
            Next
        Else
            Dim oldItem As Integer = Nothing
            Dim paramStr As String = ""
            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Dim write As StreamWriter
            Dim memfile As String = "\Barcodeprint" & prnmemsuffix & ".mem"
            write = IO.File.CreateText(Application.StartupPath & memfile)
            For Each ro As DataRow In roSelected
                Dim StnWt, DiaWt, PreWt, Total As Decimal
                strSql = "SELECT SUM(IT.STNWT)STNWT,IM.DIASTONE FROM " & cnAdminDb & "..ITEMTAGSTONE IT"
                strSql += " INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID=IT.STNITEMID"
                strSql += " WHERE TAGSNO='" & ro!SNO.ToString & "' "
                strSql += " AND IM.DIASTONE IN('D','S','P')"
                strSql += " GROUP BY IM.DIASTONE"
                Dim dtStone As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtStone)
                If dtStone.Rows.Count > 0 Then
                    DiaWt = IIf(IsDBNull(dtStone.Compute("SUM(STNWT)", "DIASTONE='D'").ToString), 0, Val(dtStone.Compute("SUM(STNWT)", "DIASTONE='D'").ToString))
                    StnWt = IIf(IsDBNull(dtStone.Compute("SUM(STNWT)", "DIASTONE='S'").ToString), 0, Val(dtStone.Compute("SUM(STNWT)", "DIASTONE='S'").ToString))
                    PreWt = IIf(IsDBNull(dtStone.Compute("SUM(STNWT)", "DIASTONE='P'").ToString), 0, Val(dtStone.Compute("SUM(STNWT)", "DIASTONE='P'").ToString))
                    Total = DiaWt + StnWt + PreWt
                End If
                strSql = "INSERT INTO " & cnAdminDb & "..DUPLICATETAG"
                strSql += vbCrLf + "(TAGSNO,TAGNO,ITEMID,GRSWT,NETWT,DIAWT,PREWT,STNWT,TOTSTNWT"
                strSql += vbCrLf + ",USERID,UPDATED,UPTIME,SYSTEMID,APPVER,REMARK1,REMARK2)"
                strSql += vbCrLf + "VALUES"
                strSql += vbCrLf + "("
                strSql += vbCrLf + "'" & ro!SNO.ToString & "'" 'TAGSNO
                strSql += vbCrLf + ",'" & ro!TAGNO.ToString & "'" 'TAGNO
                strSql += vbCrLf + "," & Val(ro!ITEMID.ToString) & "" 'ITEMID
                strSql += vbCrLf + "," & Val(ro!GRSWT.ToString) & "" 'GRSWT
                strSql += vbCrLf + "," & Val(ro!NETWT.ToString) & "" 'NETWT
                strSql += vbCrLf + "," & DiaWt & "" 'DIAWT
                strSql += vbCrLf + "," & PreWt & "" 'PREWT
                strSql += vbCrLf + "," & StnWt & "" 'STNWT
                strSql += vbCrLf + "," & Total & "" 'TOTSTNWT
                strSql += vbCrLf + "," & userId & "" 'USERID
                strSql += vbCrLf + ",'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += vbCrLf + ",'" & systemId & "'" 'SYSTEMID
                strSql += vbCrLf + ",'" & VERSION & "'" 'APPVER
                strSql += vbCrLf + ",'" & objMiscRemark.cmbRemark1_OWN.Text & "'" 'REMARK1
                strSql += vbCrLf + ",'" & objMiscRemark.cmbRemark2_OWN.Text & "'" 'REMARK2
                strSql += vbCrLf + ")"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "UPDATE " & cnAdminDb & "..ITEMTAG SET RESDATE = '" & GetServerDate() & "' WHERE SNO = '" & ro!SNO & "'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
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
        End If
    End Sub

    Private Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        If Not gridView.RowCount > 0 Then Exit Sub
        chkSelectAll.Enabled = False
        For Each dgvRow As DataGridViewRow In gridView.Rows
            dgvRow.Cells("CHK").Value = chkSelectAll.Checked
        Next
        chkSelectAll.Enabled = True
    End Sub

    Function FuncprintBarcode_Single(ByVal ItemId As String, ByVal Tagno As String)
        Try
            Dim systemName As String = My.Computer.Name
            strSql = " IF OBJECT_ID('TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE]') IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE]"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = " SELECT "
            strSql += vbCrLf + " SNO,(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.SUBITEMID)SUBITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.SUBITEMID)SHORTNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID)ITEMTYPE,"
            strSql += vbCrLf + " (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)METALID,"
            strSql += vbCrLf + " ITEMID,SUBITEMID,TABLECODE,DESIGNERID,PCS,GRSWT,NETWT,LESSWT,"
            strSql += vbCrLf + " RATE,MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC,SALVALUE,ITEMTYPEID,STYLENO"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0)STNWT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S') GROUP BY TAGSNO,ITEMID),0)STNPCS,'' STNNAME"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D' AND ISNULL(BEEDS,'')<>'Y') GROUP BY TAGSNO,ITEMID),0)DIAWT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D' AND ISNULL(BEEDS,'')<>'Y') GROUP BY TAGSNO,ITEMID),0)DIAPCS,'' DIANAME"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(BEEDS,'')='Y') GROUP BY TAGSNO,ITEMID),0)BDSWT"
            strSql += vbCrLf + " INTO TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE] FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE ITEMID='" & ItemId & "' AND TAGNO='" & Tagno & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = "SELECT * FROM TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE]"
            Dim dtTag As DataTable
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            dtTag = New DataTable
            da.Fill(dtTag)
            If dtTag.Rows.Count = 0 Then Exit Function
            Dim ItemName As String = "" : Dim SubItemName As String = ""
            Dim GrsWt As String = "" : Dim NetWt As String = ""
            Dim LessWt As String = "" : Dim MAXWASTPER As String = ""
            Dim MAXWAST As String = "" : Dim MAXMCGRM As String = ""
            Dim MAXMC As String = "" : Dim stnAmt As String = ""
            Dim stnWt As String = "" : Dim stnPcs As String = "" : Dim stnName As String = ""
            Dim DiaAmt As String = "" : Dim DiaPcs As String = "" : Dim DiaWt As String = "" : Dim BDSWt As String = ""
            Dim DiaName As String = "" : Dim Rate As String = ""
            Dim SalValue As String = "" : Dim ItemType As String = ""
            Dim MetalId As String = ""

            Dim stnWt1 As String = "" : Dim stnPcs1 As String = "" : Dim stnName1 As String = "" : Dim stoneUnit1 As String = ""
            Dim stnWt2 As String = "" : Dim stnPcs2 As String = "" : Dim stnName2 As String = "" : Dim stoneUnit2 As String = ""
            Dim stnWt3 As String = "" : Dim stnPcs3 As String = "" : Dim stnName3 As String = "" : Dim stoneUnit3 As String = ""
            Dim stnWt4 As String = "" : Dim stnPcs4 As String = "" : Dim stnName4 As String = "" : Dim stoneUnit4 As String = ""

            Dim dtTagStone As New DataTable
            strSql = "SELECT (SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=S.STNITEMID)STNITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=S.STNSUBITEMID)STNSUBITEMNAME,"
            strSql += vbCrLf + " * FROM " & cnAdminDb & "..ITEMTAGSTONE AS S WHERE TAGSNO ='" & dtTag.Rows(0)("SNO").ToString & "'"
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            dtTagStone = New DataTable
            da.Fill(dtTagStone)
            For K As Integer = 0 To dtTagStone.Rows.Count - 1
                If K > 3 Then Exit For
                If K = 0 Then
                    stnWt1 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0, _
                    Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs1 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName1 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    If stnName1 <> "" Then stnName1 = stnName1.ToString.Substring(0, 1)
                    stoneUnit1 = dtTagStone.Rows(K)("STONEUNIT").ToString
                End If
                If K = 1 Then
                    stnWt2 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0, _
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs2 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName2 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    If stnName2 <> "" Then stnName2 = stnName2.ToString.Substring(0, 1)
                    stoneUnit2 = dtTagStone.Rows(K)("STONEUNIT").ToString
                End If
                If K = 2 Then
                    stnWt3 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0, _
                    Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs3 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName3 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    If stnName3 <> "" Then stnName3 = stnName3.ToString.Substring(0, 1)
                    stoneUnit3 = dtTagStone.Rows(K)("STONEUNIT").ToString
                End If
                If K = 3 Then
                    stnWt4 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0, _
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs4 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName4 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    If stnName4 <> "" Then stnName4 = stnName4.ToString.Substring(0, 1)
                    stoneUnit4 = dtTagStone.Rows(K)("STONEUNIT").ToString
                End If
            Next


            ItemName = dtTag.Rows(0)("ITEMNAME").ToString
            If dtTag.Rows(0)("SHORTNAME").ToString <> "" Then
                SubItemName = dtTag.Rows(0)("SHORTNAME").ToString
            Else
                SubItemName = dtTag.Rows(0)("SUBITEMNAME").ToString
            End If
            GrsWt = IIf(Val(dtTag.Rows(0)("GRSWT").ToString) > 0, dtTag.Rows(0)("GRSWT").ToString, "")
            NetWt = IIf(Val(dtTag.Rows(0)("NETWT").ToString) > 0, dtTag.Rows(0)("NETWT").ToString, "") 'dtTag.Rows(0)("NETWT").ToString
            LessWt = IIf(Val(dtTag.Rows(0)("LESSWT").ToString) > 0, dtTag.Rows(0)("LESSWT").ToString, "") ' dtTag.Rows(0)("LESSWT").ToString
            MAXWASTPER = IIf(Val(dtTag.Rows(0)("MAXWASTPER").ToString) > 0, dtTag.Rows(0)("MAXWASTPER").ToString, "") 'dtTag.Rows(0)("MAXWASTPER").ToString
            MAXWAST = IIf(Val(dtTag.Rows(0)("MAXWAST").ToString) > 0, dtTag.Rows(0)("MAXWAST").ToString, "") ' dtTag.Rows(0)("MAXWAST").ToString
            MAXMCGRM = IIf(Val(dtTag.Rows(0)("MAXMCGRM").ToString) > 0, dtTag.Rows(0)("MAXMCGRM").ToString, "") 'dtTag.Rows(0)("MAXMCGRM").ToString
            MAXMC = IIf(Val(dtTag.Rows(0)("MAXMC").ToString) > 0, dtTag.Rows(0)("MAXMC").ToString, "") 'dtTag.Rows(0)("MAXMC").ToString
            stnAmt = ""
            stnWt = IIf(Val(dtTag.Rows(0)("STNWT").ToString) > 0, dtTag.Rows(0)("STNWT").ToString, "") ' dtTag.Rows(0)("STNWT").ToString
            stnPcs = IIf(Val(dtTag.Rows(0)("STNPCS").ToString) > 0, dtTag.Rows(0)("STNPCS").ToString, "") 'dtTag.Rows(0)("STNPCS").ToString
            stnName = dtTag.Rows(0)("STNNAME").ToString
            DiaAmt = ""
            DiaWt = IIf(Val(dtTag.Rows(0)("DIAWT").ToString) > 0, dtTag.Rows(0)("DIAWT").ToString, "") 'dtTag.Rows(0)("DIAWT").ToString
            BDSWt = IIf(Val(dtTag.Rows(0)("BDSWT").ToString) > 0, dtTag.Rows(0)("BDSWT").ToString, "") 'dtTag.Rows(0)("DIAWT").ToString
            DiaPcs = IIf(Val(dtTag.Rows(0)("DIAPCS").ToString) > 0, dtTag.Rows(0)("DIAPCS").ToString, "") 'dtTag.Rows(0)("DIAPCS").ToString
            DiaName = dtTag.Rows(0)("DIANAME").ToString
            Rate = IIf(Val(dtTag.Rows(0)("RATE").ToString) > 0, dtTag.Rows(0)("RATE").ToString, "") 'dtTag.Rows(0)("RATE").ToString
            SalValue = IIf(Val(dtTag.Rows(0)("SALVALUE").ToString) > 0, dtTag.Rows(0)("SALVALUE").ToString, "") 'dtTag.Rows(0)("SALVALUE").ToString
            ItemType = dtTag.Rows(0)("ITEMTYPE").ToString
            MetalId = dtTag.Rows(0)("METALID").ToString

            'systemName
            strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='BARCODE" & systemName & "'"
            Dim barPrinter As String = objGPack.GetSqlValue(strSql, "", "")
            If barPrinter = "" Then
                barPrinter = System.Windows.Forms.SystemInformation.ComputerName
                barPrinter = "\\" & barPrinter & "\BARCODE" & systemName
            End If
            Dim barRead As String = ""

            Dim dtTemplate As DataTable
            strSql = "SELECT * FROM " & cnAdminDb & "..BARCODESETTING WHERE METALID='" & MetalId.ToString & "' AND ISNULL(FILENAME,'')<>''"
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            dtTemplate = New DataTable
            da.Fill(dtTemplate)


            For k As Integer = 0 To dtTemplate.Rows.Count - 1
                Dim strCond As String = ""
                strCond = dtTemplate.Rows(k)("DESCRIPTION").ToString
                strSql = "SELECT 1 FROM TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE] WHERE " & strCond & " AND METALID='" & MetalId.ToString & "' "
                If Val(objGPack.GetSqlValue(strSql, , "").ToString) > 0 Then
                    If dtTemplate.Rows(k)("FILENAME").ToString <> "" Then
                        barRead = Application.StartupPath & "\BARCODE\" & dtTemplate.Rows(k)("FILENAME").ToString & ".PRN"
                        'Exit For
                    End If
                End If
            Next
            If barRead = "" Then
                If MetalId.ToString <> "" Then
                    barRead = Application.StartupPath & "\BARCODE\" & "BARCODE" & MetalId & ".PRN"
                Else
                    barRead = Application.StartupPath & "\BARCODE\" & "BARCODE1.PRN"
                End If
            End If
            If IO.Directory.Exists(Application.StartupPath & "\BARCODE") = False Then
                MsgBox("Directory not Exists..." & vbCrLf & "Directory Name : " & Application.StartupPath & "\BARCODE", MsgBoxStyle.Information)
                Exit Function
            End If
            If IO.File.Exists(barRead) = False Then
                MsgBox("Barcode Template not Found..." & vbCrLf & "File Name : " & barRead.ToString, MsgBoxStyle.Information)
                Exit Function
            End If
            Dim barWrite As String = Application.StartupPath & "\BARCODE\" & "DUPLICATE" & MetalId & systemName & ".MEM"

            Dim fileReader As New System.IO.StreamReader(barRead)
            Dim fileWriter As New System.IO.StreamWriter(barWrite)

            Dim StrRead As String
            While fileReader.EndOfStream = False
                StrRead = fileReader.ReadLine
                'DESCRIPTION
                If StrRead.Contains("TAGNO") Then
                    fileWriter.WriteLine(StrRead.Replace("TAGNO", ItemId & "-" & Tagno))
                ElseIf StrRead.Contains("SUBITEMNAME") Then
                    fileWriter.WriteLine(StrRead.Replace("SUBITEMNAME", SubItemName))
                ElseIf StrRead.Contains("ITEMNAME") Then
                    fileWriter.WriteLine(StrRead.Replace("ITEMNAME", ItemName))
                    'WT
                ElseIf StrRead.Contains("GRSWT") Then
                    If GrsWt <> "" Then fileWriter.WriteLine(StrRead.Replace("GRSWT", GrsWt)) Else Continue While
                ElseIf StrRead.Contains("NETWT") Then
                    If NetWt <> "" Then fileWriter.WriteLine(StrRead.Replace("NETWT", NetWt)) Else Continue While
                ElseIf StrRead.Contains("LESSWT") Then
                    If LessWt <> "" Then fileWriter.WriteLine(StrRead.Replace("LESSWT", LessWt)) Else Continue While
                    'VA
                ElseIf StrRead.Contains("MAXWASTPER") Then
                    If MAXWASTPER <> "" Then fileWriter.WriteLine(StrRead.Replace("MAXWASTPER", MAXWASTPER)) Else Continue While
                ElseIf StrRead.Contains("MAXWAST") Then
                    If MAXWAST <> "" Then fileWriter.WriteLine(StrRead.Replace("MAXWAST", MAXWAST)) Else Continue While
                ElseIf StrRead.Contains("MAXMCGRM") Then
                    If MAXMCGRM <> "" Then fileWriter.WriteLine(StrRead.Replace("MAXMCGRM", MAXMCGRM)) Else Continue While
                ElseIf StrRead.Contains("MAXMC") Then
                    If MAXMC <> "" Then fileWriter.WriteLine(StrRead.Replace("MAXMC", MAXMC)) Else Continue While
                ElseIf StrRead.Contains("ITEMTYPE") Then
                    If ItemType <> "" Then fileWriter.WriteLine(StrRead.Replace("ITEMTYPE", ItemType)) Else Continue While
                ElseIf StrRead.Contains("SALVALUE") Then
                    If SalValue <> "" Then fileWriter.WriteLine(StrRead.Replace("SALVALUE", SalValue)) Else Continue While
                    '    'STONE
                ElseIf StrRead.Contains("STNAMT") Then
                    If stnAmt <> "" Then fileWriter.WriteLine(StrRead.Replace("STNAMT", stnAmt)) Else Continue While
                    'ElseIf StrRead.Contains("STNWT") Then
                    '    If stnWt <> "" Then fileWriter.WriteLine(StrRead.Replace("STNWT", stnWt)) Else Continue While
                    'ElseIf StrRead.Contains("STNPCS") Then
                    '    If stnPcs <> "" Then fileWriter.WriteLine(StrRead.Replace("STNPCS", stnPcs)) Else Continue While
                    'ElseIf StrRead.Contains("STNNAME") Then
                    '    If stnName <> "" Then fileWriter.WriteLine(StrRead.Replace("STNNAME", stnName)) Else Continue While

                    '    'STONE1
                ElseIf StrRead.Contains("STNNAME1") Then
                    If stnName1 <> "" Then
                        StrRead = StrRead.Replace("STNNAME1", stnName1).ToString
                        StrRead = StrRead.Replace("STNPCS1", stnPcs1).ToString
                        StrRead = StrRead.Replace("STNWT1", stnWt1).ToString
                        StrRead = StrRead.Replace("STONEUNIT1", stoneUnit1).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNNAME2") Then
                    If stnName2 <> "" Then
                        StrRead = StrRead.Replace("STNNAME2", stnName2).ToString
                        StrRead = StrRead.Replace("STNPCS2", stnPcs2).ToString
                        StrRead = StrRead.Replace("STNWT2", stnWt2).ToString
                        StrRead = StrRead.Replace("STONEUNIT2", stoneUnit2).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNNAME3") Then
                    If stnName3 <> "" Then
                        StrRead = StrRead.Replace("STNNAME3", stnName3).ToString
                        StrRead = StrRead.Replace("STNPCS3", stnPcs3).ToString
                        StrRead = StrRead.Replace("STNWT3", stnWt3).ToString
                        StrRead = StrRead.Replace("STONEUNIT3", stoneUnit3).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNNAME4") Then
                    If stnName4 <> "" Then
                        StrRead = StrRead.Replace("STNNAME4", stnName4).ToString
                        StrRead = StrRead.Replace("STNPCS4", stnPcs4).ToString
                        StrRead = StrRead.Replace("STNWT4", stnWt4).ToString
                        StrRead = StrRead.Replace("STONEUNIT4", stoneUnit4).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If


                    '    'DIAMOND
                    'ElseIf StrRead.Contains("DIAMT") Then
                    '    If DiaAmt <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAAMT", DiaAmt)) Else Continue While
                    'ElseIf StrRead.Contains("DIAWT") Then
                    '    If DiaWt <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAWT", DiaWt)) Else Continue While
                    'ElseIf StrRead.Contains("DIAPCS") Then
                    '    If DiaPcs <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAPCS", DiaPcs)) Else Continue While
                    'ElseIf StrRead.Contains("DIANAME") Then
                    '    If DiaName <> "" Then fileWriter.WriteLine(StrRead.Replace("DIANAME", DiaName)) Else Continue While
                    'BEEDS                
                ElseIf StrRead.Contains("BDSWT") Then
                    If BDSWt <> "" Then fileWriter.WriteLine(StrRead.Replace("BDSWT", BDSWt)) Else Continue While

                ElseIf StrRead.Contains("RATE") Then
                    If Rate <> "" Then fileWriter.WriteLine(StrRead.Replace("RATE", Rate)) Else Continue While
                Else
                    fileWriter.WriteLine(StrRead)
                End If
            End While
            fileReader.Close()
            fileWriter.Close()
            Dim objBarcodePrint As New RawPrinterHelper
            objBarcodePrint.SendFileToPrinter(barWrite)
            fileWriter.Dispose()
            My.Computer.FileSystem.DeleteFile(barWrite)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Function FuncprintBarcode_Multi(ByVal ItemId As String, ByVal Tagno As String, ByVal PrintFormat As Double)
        Try
            Dim systemName As String = My.Computer.Name
            strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPBARCODE" & systemName & "') IS NOT NULL "
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPBARCODE" & systemName & " ("
            strSql += vbCrLf + " ITEMNAME,SUBITEMNAME,ITEMTYPE,METALID,TAGNO,ITEMID,SUBITEMID,TABLECODE,DESIGNERID,"
            strSql += vbCrLf + " PCS,GRSWT,NETWT,LESSWT,RATE,MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC,SALVALUE,"
            strSql += vbCrLf + " ITEMTYPEID,STYLENO,STNWT,STNPCS,STNNAME,DIAWT,DIAPCS,DIANAME)"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " (SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.SUBITEMID)SUBITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID)ITEMTYPE,"
            strSql += vbCrLf + " (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)METALID,"
            strSql += vbCrLf + " TAGNO,ITEMID,SUBITEMID,TABLECODE,DESIGNERID,PCS,GRSWT,NETWT,LESSWT,"
            strSql += vbCrLf + " RATE,MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC,SALVALUE,ITEMTYPEID,STYLENO"
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='T'))STNWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='T') GROUP BY TAGSNO,ITEMID)STNPCS,'' STNNAME"
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D') GROUP BY TAGSNO,ITEMID)DIAWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D') GROUP BY TAGSNO,ITEMID)DIAPCS,'' DIANAME"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE ITEMID='" & ItemId & "' AND TAGNO='" & Tagno & "'"
            strSql += vbCrLf + " END"
            strSql += vbCrLf + " ELSE"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " (SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.SUBITEMID)SUBITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID)ITEMTYPE,"
            strSql += vbCrLf + " (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)METALID,"
            strSql += vbCrLf + " TAGNO,ITEMID,SUBITEMID,TABLECODE,DESIGNERID,PCS,GRSWT,NETWT,LESSWT,"
            strSql += vbCrLf + " RATE,MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC,SALVALUE,ITEMTYPEID,STYLENO"
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='T'))STNWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='T') GROUP BY TAGSNO,ITEMID)STNPCS,'' STNNAME"
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D') GROUP BY TAGSNO,ITEMID)DIAWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D') GROUP BY TAGSNO,ITEMID)DIAPCS,'' DIANAME"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPBARCODE" & systemName & ""
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE ITEMID='" & ItemId & "' AND TAGNO='" & Tagno & "'"
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = " SELECT COUNT(*) FROM TEMPTABLEDB..TEMPBARCODE" & systemName & " AS T"
            If Val(objGPack.GetSqlValue(strSql, , "").ToString) < PrintFormat Then Exit Function

            strSql = " SELECT * FROM TEMPTABLEDB..TEMPBARCODE" & systemName & " AS T"
            Dim dtTag As DataTable
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            dtTag = New DataTable
            da.Fill(dtTag)
            If dtTag.Rows.Count = 0 Then Exit Function

            'systemName            
            Dim barRead As String = ""
            If MetalId.ToString <> "" Then
                barRead = Application.StartupPath & "\" & "BARCODE" & MetalId & ".PRN"
            Else
                barRead = Application.StartupPath & "\" & "BARCODE1.PRN"
            End If
            If IO.File.Exists(barRead) = False Then
                MsgBox("Barcode Template not Found...", MsgBoxStyle.Information)
                Exit Function
            End If
            Dim barWrite As String = Application.StartupPath & "\DUPLICATE" & METALID & systemName & "_ORG" & ".MEM"

            Dim fileReader As New System.IO.StreamReader(barRead)
            Dim fileWriter As New System.IO.StreamWriter(barWrite)
            Dim StrRead As String
            While fileReader.EndOfStream = False
                Dim Isprinted As Boolean = False
                StrRead = fileReader.ReadLine
                For k As Integer = 1 To dtTag.Rows.Count
                    With dtTag
                        'DESCRIPTION
                        If StrRead.Contains("TAGNO" & k.ToString) Then
                            fileWriter.WriteLine(StrRead.Replace("TAGNO" & k.ToString, .Rows(k - 1)("ITEMID").ToString & .Rows(k - 1)("TAGNO").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("ITEMNAME" & k.ToString) Then
                            fileWriter.WriteLine(StrRead.Replace("ITEMNAME" & k.ToString, .Rows(k - 1)("ITEMNAME").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("SUBITEMNAME" & k.ToString) Then
                            fileWriter.WriteLine(StrRead.Replace("SUBITEMNAME" & k.ToString, .Rows(k - 1)("SUBITEMNAME").ToString))
                            Isprinted = True : Exit For
                            'WT 
                        ElseIf StrRead.Contains("GRSWT" & k.ToString) Then
                            If .Rows(k - 1)("GRSWT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("GRSWT" & k.ToString, .Rows(k - 1)("GRSWT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("NETWT" & k.ToString) Then
                            If .Rows(k - 1)("NETWT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("NETWT" & k.ToString, .Rows(k - 1)("NETWT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("LESSWT" & k.ToString) Then
                            If .Rows(k - 1)("LESSWT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("LESSWT" & k.ToString, .Rows(k - 1)("LESSWT").ToString))
                            Isprinted = True : Exit For
                            'VA
                        ElseIf StrRead.Contains("WASTPER" & k.ToString) Then
                            If .Rows(k - 1)("WASTPER").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("WASTPER" & k.ToString, .Rows(k - 1)("WASTPER").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("WASTWT" & k.ToString) Then
                            If .Rows(k - 1)("WASTWT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("WASTWT" & k.ToString, .Rows(k - 1)("WASTWT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("MCGRM" & k.ToString) Then
                            If .Rows(k - 1)("MCGRM").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("MCGRM" & k.ToString, .Rows(k - 1)("MCGRM").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("MCAMT" & k.ToString) Then
                            If .Rows(k - 1)("MCAMT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("MCAMT" & k.ToString, .Rows(k - 1)("MCAMT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("ITEMTYPE" & k.ToString) Then
                            If .Rows(k - 1)("ITEMTYPE").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("ITEMTYPE" & k.ToString, .Rows(k - 1)("ITEMTYPE").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("SALVALUE" & k.ToString) Then
                            If .Rows(k - 1)("SALVALUE").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("SALVALUE" & k.ToString, .Rows(k - 1)("SALVALUE").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("RATE" & k.ToString) Then
                            If .Rows(k - 1)("RATE").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("RATE" & k.ToString, .Rows(k - 1)("RATE").ToString))
                            Isprinted = True : Exit For
                            'STONE
                        ElseIf StrRead.Contains("STNAMT" & k.ToString) Then
                            If .Rows(k - 1)("STNAMT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("STNAMT" & k.ToString, .Rows(k - 1)("STNAMT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("STNWT" & k.ToString) Then
                            If .Rows(k - 1)("STNWT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("STNWT" & k.ToString, .Rows(k - 1)("STNWT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("STNPCS" & k.ToString) Then
                            If .Rows(k - 1)("STNPCS").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("STNPCS" & k.ToString, .Rows(k - 1)("STNPCS").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("STNNAME" & k.ToString) Then
                            If .Rows(k - 1)("STNNAME").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("STNNAME" & k.ToString, .Rows(k - 1)("STNNAME").ToString))
                            Isprinted = True : Exit For
                            'DIAMOND
                        ElseIf StrRead.Contains("DIAMT" & k.ToString) Then
                            If .Rows(k - 1)("DIAMT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAMT" & k.ToString, .Rows(k - 1)("DIAMT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("DIAWT" & k.ToString) Then
                            If .Rows(k - 1)("DIAWT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAWT" & k.ToString, .Rows(k - 1)("DIAWT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("DIAPCS" & k.ToString) Then
                            If .Rows(k - 1)("DIAPCS").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAPCS" & k.ToString, .Rows(k - 1)("DIAPCS").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("DIANAME" & k.ToString) Then
                            If .Rows(k - 1)("DIANAME").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("DIANAME" & k.ToString, .Rows(k - 1)("DIANAME").ToString))
                            Isprinted = True : Exit For
                        End If
                    End With
                Next
                If Isprinted = False Then fileWriter.WriteLine(StrRead)
            End While
            fileReader.Close()
            fileWriter.Close()
            'SEND FILE TO PRINTER
            Dim objBarcodePrint As New RawPrinterHelper
            objBarcodePrint.SendFileToPrinter(barWrite)
            strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPBARCODE" & systemName & "') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPBARCODE" & systemName & " "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            fileWriter.Dispose()
            My.Computer.FileSystem.DeleteFile(barWrite)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Sub txtItemId_NUM_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtItemId_NUM.Leave
        If Val(txtItemId_NUM.Text) > 0 Then
            cmbsubitem.Items.Clear()
            cmbsubitem.Items.Add("ALL")
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID='" & txtItemId_NUM.Text & "' ORDER BY SUBITEMNAME"
            objGPack.FillCombo(strSql, cmbsubitem, False)
            cmbsubitem.Text = "ALL"
            cmbsubitem.Enabled = True
        Else
            cmbsubitem.Enabled = False
        End If
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        tabMain.SelectedTab = tabView
        dgvView.DataSource = Nothing
        dgvView.Refresh()
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        dtpFrom.Focus()
    End Sub

    Private Sub btnViewSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewSearch.Click
        dgvView.DataSource = Nothing
        dgvView.Refresh()
        strSql = " SELECT ITEMID,TAGNO,GRSWT,NETWT"
        strSql += " ,(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=D.USERID)USERNAME"
        strSql += " ,UPDATED,CONVERT(VARCHAR(20),UPTIME,108) AS UPTIME"
        strSql += " ,REMARK1 + ' ' + REMARK2 AS REASON"
        strSql += " FROM " & cnAdminDb & "..DUPLICATETAG D"
        strSql += " WHERE UPDATED BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "'"
        strSql += " AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        strSql += " ORDER BY UPDATED DESC"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If dtGrid.Rows.Count > 0 Then
            dgvView.DataSource = Nothing
            dgvView.DataSource = dtGrid
            FormatGridColumns(dgvView, False)
            dgvView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            dgvView.Invalidate()
            For Each dgvCol As DataGridViewColumn In dgvView.Columns
                dgvCol.Width = dgvCol.Width
            Next
            dgvView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            dgvView.Select()
        Else
            MsgBox("No Record Found...", MsgBoxStyle.Information)
            dtpFrom.Focus()
            Exit Sub
        End If
    End Sub
End Class