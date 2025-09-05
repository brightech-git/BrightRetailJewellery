Imports System.Data.OleDb
Imports System.IO
Public Class frmCounterChange
    'CALID-601: CLIENT-UNKNOWN: CORRECTION-APPROVAL NO. FILTERING OPTION IS REQUIRED:  ALTER BY SATHYA
    '290613 : Client- Chandana Jewellers : Alter by vasanth
    '071213 : Client- Algar Jewellers: In search Estimateno filteration is needed : Alter by vasanth
    '160714 : Client- KHAWISH : Approval tag cannot counter transfer: Alter by vasanth
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtGrid As DataTable
    Dim chk As Integer
    Dim defaultcounters As String = GetAdmindbSoftValue("TAG_DEF_COUNTERID", "").ToString
    Dim IsAllowOrderTag As Boolean = IIf(GetAdmindbSoftValue("ORDERTAG_CTRCHANGE", "Y").ToString = "Y", True, False)
    Dim IsDUPTAGCTRCHANGE As Boolean = IIf(GetAdmindbSoftValue("DUPTAG_CTRCHANGE", "N").ToString = "Y", True, False)
    Dim HideCtrTransferTotal As Boolean = IIf(GetAdmindbSoftValue("HIDECTRTRANSFERTOTAL", "N").ToString = "Y", True, False)
    Dim Tag_Auto_CtrChange As Boolean = IIf(GetAdmindbSoftValue("TAG_AUTO_CTRCHANGE", "N").ToString = "Y", True, False)
    Dim TagCtrChangeReason As Boolean = IIf(GetAdmindbSoftValue("TAG_CTR_CHANGE_REASON", "N").ToString = "Y", True, False)
    Dim TagCtrChangeUserBased As Boolean = IIf(GetAdmindbSoftValue("TAG_CTR_CHANGE_USER", "N").ToString = "Y", True, False)
    Dim Ismultiplecounter As Boolean = IIf(GetAdmindbSoftValue("ISMULTIPLECOUNTER", "N").ToString = "Y", True, False)
    Dim dt As New DataTable

    Private Sub frmCounterChange_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtFindTag.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            If TabMain.SelectedTab.Name = TabPage2.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        '290613
        'If chkScan.Checked = False Then gridView.DataSource = Nothing
        'If objGPack.Validator_Check(Me) Then Exit Sub
        If Isgridexist() Then GoTo nnext
        If Ismultiplecounter = True Then

            If chkCmbCounter_Man.Text.Trim = "" Then MsgBox("old  Counter should not empty.", MsgBoxStyle.Information) : chkCmbCounter_Man.Focus() : Exit Sub
        Else
            If cmbOldCounter_MAN.Text.Trim = "" Then MsgBox("old Counter should not empty.", MsgBoxStyle.Information) : cmbOldCounter_MAN.Focus() : Exit Sub
        End If

        If cmbNewCounter_MAN.Text.Trim = "" Then MsgBox("New Counter should not empty.", MsgBoxStyle.Information) : cmbNewCounter_MAN.Focus() : Exit Sub
        lblRowDet1.Text = ""
        Me.Refresh()
        If Ismultiplecounter = True Then
            If chkCmbCounter_Man.Text.Trim = cmbNewCounter_MAN.Text.Trim Then MsgBox("Both counters are same.", MsgBoxStyle.Information) : chkCmbCounter_Man.Focus() : Exit Sub
        Else
            If cmbOldCounter_MAN.Text.Trim = cmbNewCounter_MAN.Text.Trim Then MsgBox("Both counters are same.", MsgBoxStyle.Information) : cmbOldCounter_MAN.Focus() : Exit Sub
        End If




        strSql = " SELECT RECDATE,ITEMID"
        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
        strSql += vbCrLf + " ,TAGNO,t.PCS,GRSWT,NETWT,SALVALUE"
        strSql += vbCrLf + " ,(SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = T.LOTSNO)AS LOTNO"
        strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)aS COUNTER"
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)aS DESIGNER,SNO,COSTID"

        'strSql += vbCrLf + " ,'" & cmbOldCounter_MAN.Text & "'  AS OLD"
        ''strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER where itemctrname in(" & GetQryString(chkCmbCounter_Man.Text) & "))  AS OLD"
        'strSql += vbCrLf + " ,'" & cmbNewCounter_MAN.Text & "'  AS NEW"
        'If Tag_Auto_CtrChange Then
        If Ismultiplecounter = True Then
            strSql += vbCrLf + " , ic.itemctrname as old "
        Else
            strSql += vbCrLf + " ,'" & cmbOldCounter_MAN.Text & "'  AS OLD"
        End If

        strSql += vbCrLf + " ,'" & cmbNewCounter_MAN.Text & "'  AS NEW"
        'End If

        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        '160714
        If Ismultiplecounter = True Then
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON IC.ITEMCTRID=T.ITEMCTRID"
        End If

        strSql += vbCrLf + " WHERE ISNULL(APPROVAL,'')=''"
        If dtpFrom.Enabled Then
            strSql += vbCrLf + " AND RECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & cnCostId & "'"
        Else
            strSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & cnCostId & "'"
        End If
        If LTrim(txtLotNo_NUM.Text.ToString) <> "" Then strSql += vbCrLf + " AND LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
        If LTrim(txtTagNo.Text.ToString) <> "" And LTrim(txtappno.Text.ToString) = "" Then strSql += vbCrLf + " AND TAGNO = '" & txtTagNo.Text & "'"
        If Ismultiplecounter = True Then
            If chkCmbCounter_Man.Text <> "" Then strSql += vbCrLf + " AND ic.ITEMCTRID  IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME in( " & GetQryString(chkCmbCounter_Man.Text) & "))"
        Else
            If cmbOldCounter_MAN.Text <> "" Then strSql += vbCrLf + " AND ITEMCTRID = ISNULL((SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbOldCounter_MAN.Text & "'),0)"
        End If


        If Val(txtItemId.Text) > 0 Then strSql += vbCrLf + " AND ITEMID = " & Val(txtItemId.Text) & ""
        '601
        If LTrim(txtappno.Text.ToString) <> "" Then
            strSql += " AND TAGNO IN(SELECT TAGNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE='AI' AND TRANNO='" & txtappno.Text.ToString() & "'"
            If chkDate.Checked Then strSql += " AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "')"
        End If
        '601

        '071213
        If Val(txtEstNo.Text) <> 0 Then
            ''strSql += vbCrLf + " AND TAGNO IN (SELECT TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"
            strSql += vbCrLf + " AND CONVERT(VARCHAR(10),ITEMID)+'-'+TAGNO IN (SELECT CONVERT(VARCHAR(10),ITEMID)+'-'+TAGNO AS ITAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"
        End If
        '071213
        If Val(txtPktNo.Text) <> 0 Then
            strSql += vbCrLf + " AND TAGNO IN (SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAGPKTNO WHERE PACKETNO=" & Val(txtPktNo.Text) & ")"
        End If

        If IsAllowOrderTag = False Then strSql += vbCrLf + " AND ISNULL(ORDREPNO,'') = ''"
        strSql += vbCrLf + " AND ISSDATE IS NULL"
        Dim dttemp As New DataTable
        dttemp.Columns.Add("KEYNO", GetType(Integer))
        dttemp.Columns("KEYNO").AutoIncrement = True
        dttemp.Columns("KEYNO").AutoIncrementSeed = 0
        dttemp.Columns("KEYNO").AutoIncrementStep = 1
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = chkSelectAll.Checked
        dttemp.Columns.Add(dtCol)
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dttemp)
        If dtGrid.Rows.Count <= 0 Or dtGrid Is Nothing Then
            dtGrid = dttemp.Clone
            dtGrid.Columns.Add("TKEYNO", GetType(Integer))
            dtGrid.Columns("TKEYNO").AutoIncrement = True
            dtGrid.Columns("TKEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("TKEYNO").AutoIncrementStep = 1
        End If
        If Not dttemp.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        Else
            If txtTagNo.Text <> "" Then
                dtGrid.Merge(dttemp)
            Else
                dtGrid = dttemp
            End If
        End If
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)

        Dim dv As DataView
        Dim dvgrid As DataTable
        If dtGrid.Columns.Contains("TKEYNO") Then
            dv = New DataView(dtGrid, "", "TKEYNO Desc", DataViewRowState.CurrentRows)
        Else
            dv = New DataView(dtGrid, "", "KEYNO Desc", DataViewRowState.CurrentRows)
        End If
        dvgrid = New DataTable
        dvgrid = dv.ToTable

        gridView.DataSource = dvgrid
        FormatGridColumns(gridView, False)
        gridView.Columns("CHECK").ReadOnly = False
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        With gridView
            .Columns("CHECK").HeaderText = ""
            .Columns("CHECK").Width = 30
            .Columns("RECDATE").Width = 80
            .Columns("ITEM").Width = 150
            .Columns("SUBITEM").Width = 120
            .Columns("TAGNO").Width = 70
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("SALVALUE").Width = 100
            .Columns("LOTNO").Visible = False
            .Columns("DESIGNER").Visible = False
            .Columns("COUNTER").Visible = False
            .Columns("SNO").Visible = False
            .Columns("KEYNO").Visible = False
            If .Columns.Contains("TKEYNO") Then .Columns("TKEYNO").Visible = False
            .Columns("COSTID").Visible = False
            .Columns("ITEMID").Visible = False
        End With
        Dim dtGridHeader As New DataTable
        dtGridHeader = dtGrid.Clone
        dtGridHeader.Columns("CHECK").DataType = GetType(String)
        dtGridHeader.Rows.Add()
        dtGridHeader.Rows.Add()
        If HideCtrTransferTotal = False Then
            dtGridHeader.Rows(0).Item("CHECK") = ""
            dtGridHeader.Rows(0).Item("ITEM") = "TOTAL"
            dtGridHeader.Rows(0).Item("SUBITEM") = "TAG COUNT :"
        End If
        dtGridHeader.Rows(1).Item("ITEM") = "SELECTED"
        dtGridHeader.Rows(1).Item("CHECK") = ""
        Dim obj As Object
        If HideCtrTransferTotal = False Then
            obj = dtGrid.Compute("SUM(PCS)", "PCS IS NOT NULL")
            dtGridHeader.Rows(0).Item("PCS") = IIf(Val(obj.ToString) <> 0, obj, DBNull.Value)
            If chkSelectAll.Checked Then dtGridHeader.Rows(1).Item("PCS") = IIf(Val(obj.ToString) <> 0, obj, DBNull.Value)
            obj = dtGrid.Compute("SUM(GRSWT)", "GRSWT IS NOT NULL")
            dtGridHeader.Rows(0).Item("GRSWT") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
            If chkSelectAll.Checked Then dtGridHeader.Rows(1).Item("GRSWT") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
            obj = dtGrid.Compute("SUM(NETWT)", "NETWT IS NOT NULL")
            dtGridHeader.Rows(0).Item("NETWT") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
            If chkSelectAll.Checked Then dtGridHeader.Rows(1).Item("NETWT") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
            obj = dtGrid.Compute("SUM(SALVALUE)", "SALVALUE IS NOT NULL")
            dtGridHeader.Rows(0).Item("SALVALUE") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
            If chkSelectAll.Checked Then dtGridHeader.Rows(1).Item("SALVALUE") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
            obj = dtGrid.Compute("COUNT(PCS)", String.Empty)
            dtGridHeader.Rows(0).Item("TAGNO") = IIf(Val(obj.ToString) <> 0, obj, DBNull.Value)
        End If

        If chkSelectAll.Checked Then
            dtGridHeader.Rows(1).Item("TAGNO") = IIf(Val(obj.ToString) <> 0, obj, DBNull.Value)
            If Val(obj.ToString) > 0 Then btnCounterChange.Enabled = True
        End If


        gridViewFooter.DataSource = dtGridHeader
        FormatGridColumns(gridViewFooter, False)
        gridViewFooter.DefaultCellStyle.BackColor = lblRowDet1.BackColor
        gridViewFooter.DefaultCellStyle.SelectionBackColor = lblRowDet1.BackColor
        gridViewFooter.DefaultCellStyle.SelectionForeColor = Color.Red
        gridViewFooter.DefaultCellStyle.ForeColor = Color.Red
        gridViewFooter.DefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        For cnt As Integer = 0 To gridView.Columns.Count - 1
            gridViewFooter.Columns(cnt).Visible = gridView.Columns(cnt).Visible
            gridViewFooter.Columns(cnt).Width = gridView.Columns(cnt).Width
        Next
        gridViewFooter.Columns("SUBITEM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
nnext:
        If chkScan.Checked = True Then txtItemId.Text = "" : txtTagNo.Text = "" : lblItemid.Focus() : Exit Sub
        gridView.Select()
    End Sub
    Private Function Isgridexist() As Boolean
        For ii As Integer = 0 To gridView.Rows.Count - 1
            'rwIndex += 1
            If gridView.Rows(ii).Cells("ITEMID").Value.ToString <> "" Then
                'If txtSARowIndex.Text <> "" And rwIndex = Val(txtSARowIndex.Text) Then
                '    Continue For
                'End If
                If gridView.Rows(ii).Cells("TagNo").Value.ToString = txtTagNo.Text Then
                    MsgBox("Given Tag is Already Loaded", MsgBoxStyle.Information)
                    Return True
                    Exit Function
                End If
            End If
        Next
        Return False
    End Function
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        TabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.TabMain.Region = New Region(New RectangleF(Me.TabGeneral.Left, Me.TabGeneral.Top, Me.TabGeneral.Width, Me.TabGeneral.Height))
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        dtptabfrom.Value = GetEntryDate(GetServerDate)
        dtptabto.Value = GetEntryDate(GetServerDate)
        lblRowDet1.Text = ""
        lblRowDet1.Visible = False
        dtGrid = New DataTable
        gridView.DataSource = Nothing
        gridViewFooter.DataSource = Nothing
        btnCounterChange.Enabled = False
        chkSelectAll.Checked = False
        chkDate.Checked = True
        If defaultcounters <> "" Then
            Dim ctrarry() As String = defaultcounters.Split(",")
            If ctrarry(0).ToString <> ctrarry(1).ToString Then
                If Ismultiplecounter = True Then
                    strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = " & Val(ctrarry(0).ToString) & " AND ACTIVE = 'Y' "
                    chkCmbCounter_Man.Text = objGPack.GetSqlValue(strSql).ToString
                Else
                    strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = " & Val(ctrarry(0).ToString) & " AND ACTIVE = 'Y' "
                    cmbOldCounter_MAN.Text = objGPack.GetSqlValue(strSql).ToString
                End If
                strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = " & Val(ctrarry(1).ToString) & " AND ACTIVE = 'Y' "
                cmbNewCounter_MAN.Text = objGPack.GetSqlValue(strSql).ToString
            End If
        End If
        Me.Refresh()
        chkDate.Focus()
        If Tag_Auto_CtrChange Then
            chkDate.Checked = False
            chkScan.Checked = True
            txtItemId.Select()
        End If
    End Sub

    Private Sub CalcSelectedValues()
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable)
        dt.AcceptChanges()
        Dim pcs As Integer = Val(dt.Compute("COUNT(CHECK)", "CHECK = TRUE").ToString)
        If pcs > 0 Then
            btnCounterChange.Enabled = True
        Else
            btnCounterChange.Enabled = False
        End If
    End Sub

    Private Sub gridView_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellValueChanged
        'CalcSelectedValues()
        If Not e.RowIndex > -1 Then Exit Sub
        gridView.CurrentCell = gridView.Rows(e.RowIndex).Cells("CHECK")
        ''CalcSelected Values
        With gridView.Rows(e.RowIndex)
            Dim pcs As Integer = Val(gridViewFooter.Rows(1).Cells("PCS").Value.ToString)
            Dim grsWt As Decimal = Val(gridViewFooter.Rows(1).Cells("GRSWT").Value.ToString)
            Dim netWT As Decimal = Val(gridViewFooter.Rows(1).Cells("NETWT").Value.ToString)
            Dim salValue As Decimal = Val(gridViewFooter.Rows(1).Cells("SALVALUE").Value.ToString)
            Dim cnt As Decimal = Val(gridViewFooter.Rows(1).Cells("TAGNO").Value.ToString)
            If CType(gridView.Rows(e.RowIndex).Cells("CHECK").Value, Boolean) = True Then
                pcs += Val(.Cells("PCS").Value.ToString)
                grsWt += Val(.Cells("GRSWT").Value.ToString)
                netWT += Val(.Cells("NETWT").Value.ToString)
                salValue += Val(.Cells("SALVALUE").Value.ToString)
                .DefaultCellStyle.SelectionForeColor = Color.Red
                .DefaultCellStyle.ForeColor = Color.Red
                cnt += 1
            Else
                pcs -= Val(.Cells("PCS").Value.ToString)
                grsWt -= Val(.Cells("GRSWT").Value.ToString)
                netWT -= Val(.Cells("NETWT").Value.ToString)
                salValue -= Val(.Cells("SALVALUE").Value.ToString)
                .DefaultCellStyle.SelectionForeColor = Color.Black
                .DefaultCellStyle.ForeColor = Color.Black
                cnt -= 1
            End If
            If cnt > 0 Then
                btnCounterChange.Enabled = True
            Else
                btnCounterChange.Enabled = False
            End If
            gridViewFooter.Rows(1).Cells("SUBITEM").Value = "TAG COUNT :"
            gridViewFooter.Rows(1).Cells("SUBITEM").Style.Alignment = DataGridViewContentAlignment.MiddleRight
            gridViewFooter.Rows(1).Cells("TAGNO").Value = IIf(cnt <> 0, cnt, DBNull.Value)
            gridViewFooter.Rows(1).Cells("PCS").Value = IIf(pcs <> 0, pcs, DBNull.Value)
            gridViewFooter.Rows(1).Cells("GRSWT").Value = IIf(grsWt <> 0, Format(grsWt, "0.000"), DBNull.Value)
            gridViewFooter.Rows(1).Cells("NETWT").Value = IIf(netWT <> 0, Format(netWT, "0.000"), DBNull.Value)
            gridViewFooter.Rows(1).Cells("SALVALUE").Value = IIf(salValue <> 0, Format(salValue, "0.000"), DBNull.Value)
        End With
    End Sub


    Private Sub gridView_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.CurrentCellDirtyStateChanged
        gridView.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblRowDet1.Visible = True
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblRowDet1.Visible = False
    End Sub

    Private Sub gridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        With gridView.Rows(e.RowIndex)
            lblRowDet1.Text = "Lot No : " + .Cells("LOTNO").Value.ToString + "      "
            lblRowDet1.Text += "Designer : " + .Cells("DESIGNER").Value.ToString + "     "
            lblRowDet1.Text += "Counter : " + .Cells("COUNTER").Value.ToString
        End With
    End Sub

    Private Sub frmCounterChange_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dtcounter As New DataTable
        strSql = " SELECT 'ALL' ITEMCTRNAME,'ALL' ITEMCTRID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMCTRNAME,CONVERT(vARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER"
        strSql += " ORDER BY RESULT,ITEMCTRNAME"
        dtcounter = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcounter)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCounter_Man, dtcounter, "ITEMCTRNAME", , "ALL")
        If Ismultiplecounter = True Then
            cmbOldCounter_MAN.Visible = False
            chkCmbCounter_Man.Visible = True
        Else
            chkCmbCounter_Man.Visible = False
            cmbOldCounter_MAN.Visible = True
        End If

        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' "
        If TagCtrChangeUserBased And userId <> 999 Then
            Dim useritemctr As String = ""
            Dim qrystr As String = ""
            qrystr = " SELECT ISNULL(ITEMCTRID,'')ITEMCTRID  FROM " & cnAdminDb & "..USERCASH WHERE USERID ='" & userId.ToString & "' AND ISNULL(ITEMCTRID,'')<>''"
            useritemctr = GetSqlValue(cn, qrystr).ToString
            If useritemctr <> "" Then
                strSql += " AND ITEMCTRID IN (" & useritemctr.ToString & ")"
            End If
        End If
        strSql += " ORDER BY DISPLAYORDER,ITEMCTRNAME "
        objGPack.FillCombo(strSql, cmbOldCounter_MAN)
        objGPack.FillCombo(strSql, cmbNewCounter_MAN)
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        For Each dgvRow As DataGridViewRow In gridView.Rows
            dgvRow.Cells("CHECK").Value = chkSelectAll.Checked
        Next
    End Sub

    Private Sub btnCounterChange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCounterChange.Click
        'If objGPack.Validator_Check(Me) Then Exit Sub
        'If cmbOldCounter_MAN.Text.Trim = "" Then MsgBox("Old Counter should not empty.", MsgBoxStyle.Information) : cmbOldCounter_MAN.Focus() : Exit Sub
        'If cmbNewCounter_MAN.Text.Trim = "" Then MsgBox("New Counter should not empty.", MsgBoxStyle.Information) : cmbNewCounter_MAN.Focus() : Exit Sub
        If Ismultiplecounter = True Then
            If chkCmbCounter_Man.Text.Trim = "" Then MsgBox("old  Counter should not empty.", MsgBoxStyle.Information) : chkCmbCounter_Man.Focus() : Exit Sub
        Else
            If cmbOldCounter_MAN.Text.Trim = "" Then MsgBox("old Counter should not empty.", MsgBoxStyle.Information) : cmbOldCounter_MAN.Focus() : Exit Sub
        End If
        If cmbNewCounter_MAN.Text.Trim = "" Then MsgBox("New Counter should not empty.", MsgBoxStyle.Information) : cmbNewCounter_MAN.Focus() : Exit Sub
        If txtReason.Text.ToString.Trim = "" And TagCtrChangeReason Then
            MsgBox("Reason should not be empty...", MsgBoxStyle.Information)
            txtReason.Focus()
            Exit Sub
        End If
        If MessageBox.Show("Do you want to transfer the counter for selected items at the date of " & Today.Date.ToString("yyyy-MM-dd") & ".?", "Counter change alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Dim dt As New DataTable
            dt = CType(gridView.DataSource, DataTable).Copy
            Dim dv As DataView
            dv = dt.DefaultView
            dv.RowFilter = "CHECK = TRUE"
            Dim ctrId As Integer
            Dim oldCtrId As Integer
            If Tag_Auto_CtrChange = False Then
                If Ismultiplecounter = True Then

                    Dim coldCtrId As String = objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME in(" & GetQryString(chkCmbCounter_Man.Text) & ")")
                Else
                    oldCtrId = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbOldCounter_MAN.Text & "'"))
                End If

                ctrId = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbNewCounter_MAN.Text & "'"))
            End If
            Dim costId As String = GetAdmindbSoftValue("SYNC-TO")
            Dim CounterChange As Boolean = IIf(GetAdmindbSoftValue("COUNTERCHANGE", "Y") = "Y", True, False)
            Dim ExecCount As Integer = 0
            Try
                tran = Nothing
                tran = cn.BeginTransaction

                strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
                strSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                Dim NEWBILLNO As Integer
                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-CTRTRFNO' AND COMPANYID = '" & strCompanyId & "'"
                NEWBILLNO = Val(objGPack.GetSqlValue(strSql, , , tran))

GenerateNewBillNo:
                Dim RefNo As String = cnCostId & NEWBILLNO.ToString
                strSql = "SELECT TOP 1 REFNO FROM " & cnAdminDb & "..CTRANSFER WHERE CONVERT(VARCHAR,REFNO) = '" & RefNo & "'"
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    NEWBILLNO = NEWBILLNO + 1
                    GoTo GenerateNewBillNo
                End If
                strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & (NEWBILLNO + 1).ToString & "'"
                strSql += " WHERE CTLID ='GEN-CTRTRFNO' AND COMPANYID = '" & strCompanyId & "'"
                'strSql += " AND CONVERT(INT,CTLTEXT) = '" & NEWBILLNO & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                If cmd.ExecuteNonQuery() = 0 Then
                    GoTo GenerateNewBillNo
                End If

                For i As Integer = 0 To gridView.RowCount - 1
                    With gridView.Rows(i)
                        If .Cells("CHECK").Value.ToString <> "True" Then Continue For
                        If Tag_Auto_CtrChange Then
                            oldCtrId = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & .Cells("OLD").Value.ToString & "'", , , tran))
                            ctrId = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & .Cells("NEW").Value.ToString & "'", , , tran))
                        End If

                        strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO = '" & .Cells("SNO").Value.ToString & "')>0"
                        strSql += "     BEGIN"
                        strSql += "     UPDATE " & cnAdminDb & "..CTRANSFER SET ISSDATE = '" & GetEntryDate(GetServerDate(tran), tran) & "'"
                        strSql += "     WHERE TAGSNO = '" & .Cells("SNO").Value.ToString & "'"
                        strSql += "     AND ISSDATE IS NULL"
                        strSql += "     END"
                        strSql += " ELSE"
                        strSql += "     BEGIN"
                        strSql += "     INSERT INTO " & cnAdminDb & "..CTRANSFER"
                        strSql += "     (SNO,TAGSNO,ITEMID,TAGNO,ITEMCTRID,RECDATE,ISSDATE,TAGVAL,USERID"
                        strSql += "     ,UPDATED,UPTIME,APPVER,ENTORDER,COSTID,REFNO,REASON)"
                        strSql += "     SELECT '" & GetNewSno(TranSnoType.CTRANSFERCODE, tran, "GET_ADMINSNO_TRAN") & "' AS SNO,SNO TAGSNO"
                        strSql += "     ,ITEMID,TAGNO,ITEMCTRID,RECDATE,'" & GetEntryDate(GetServerDate(tran), tran) & "' AS ISSDATE"
                        strSql += "     ,TAGVAL," & userId & " USERID,'" & GetEntryDate(GetServerDate(tran), tran) & "'UPDATED,'" & GetServerTime(tran) & "' UPTIME,'" & VERSION & "' APPVER,1,COSTID,'" & RefNo & "','" & txtReason.Text & "'"
                        strSql += "     FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & .Cells("SNO").Value.ToString & "'"
                        strSql += "     END"
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, costId)

                        strSql = vbCrLf + " DECLARE @ENTORDER INT"
                        strSql += vbCrLf + " SELECT @ENTORDER = ISNULL(MAX(ENTORDER),0)+1 FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO = '" & .Cells("SNO").Value.ToString & "'"
                        strSql += " INSERT INTO " & cnAdminDb & "..CTRANSFER"
                        strSql += " (SNO,TAGSNO,ITEMID,TAGNO,ITEMCTRID,RECDATE,TAGVAL,USERID"
                        strSql += " ,UPDATED,UPTIME,APPVER,ENTORDER,COSTID,REFNO,REASON)"
                        strSql += " SELECT '" & GetNewSno(TranSnoType.CTRANSFERCODE, tran, "GET_ADMINSNO_TRAN") & "' AS SNO,SNO TAGSNO"
                        strSql += " ,ITEMID,TAGNO," & ctrId & ",'" & GetEntryDate(GetServerDate(tran), tran) & "' AS RECDATE"
                        strSql += " ,TAGVAL," & userId & " USERID,'" & GetEntryDate(GetServerDate(tran), tran) & "' UPDATED,'" & GetServerTime(tran) & "' UPTIME,'" & VERSION & "' APPVER,@ENTORDER,COSTID,'" & RefNo & "','" & txtReason.Text & "'"
                        strSql += " FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & .Cells("SNO").Value.ToString & "'"
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, costId)

                        strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET ITEMCTRID = " & ctrId & ""
                        strSql += " ,TRANSFERDATE = '" & GetEntryDate(GetServerDate(tran), tran) & "' "
                        strSql += " WHERE SNO = '" & .Cells("SNO").Value.ToString & "'"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()

                        ExecQuery(SyncMode.Stock, strSql, cn, tran, costId, , , , False)
                        ExecCount += 1
                    End With
                Next

                'For Each ro As DataRow In dv.ToTable.Rows
                '    If Tag_Auto_CtrChange Then
                '        oldCtrId = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & ro.Item("OLD").ToString & "'", , , tran))
                '        ctrId = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & ro.Item("NEW").ToString & "'", , , tran))
                '    End If
                '    strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO = '" & ro.Item("SNO").ToString & "')>0"
                '    strSql += "     BEGIN"
                '    strSql += "     UPDATE " & cnAdminDb & "..CTRANSFER SET ISSDATE = '" & GetEntryDate(GetServerDate(tran), tran) & "'"
                '    strSql += "     WHERE TAGSNO = '" & ro.Item("SNO").ToString & "'"
                '    strSql += "     AND ISSDATE IS NULL"
                '    strSql += "     END"
                '    strSql += " ELSE"
                '    strSql += "     BEGIN"
                '    strSql += "     INSERT INTO " & cnAdminDb & "..CTRANSFER"
                '    strSql += "     (SNO,TAGSNO,ITEMID,TAGNO,ITEMCTRID,RECDATE,ISSDATE,TAGVAL,USERID"
                '    strSql += "     ,UPDATED,UPTIME,APPVER,ENTORDER,COSTID,REFNO,REASON)"
                '    strSql += "     SELECT '" & GetNewSno(TranSnoType.CTRANSFERCODE, tran, "GET_ADMINSNO_TRAN") & "' AS SNO,SNO TAGSNO"
                '    strSql += "     ,ITEMID,TAGNO,ITEMCTRID,RECDATE,'" & GetEntryDate(GetServerDate(tran), tran) & "' AS ISSDATE"
                '    strSql += "     ,TAGVAL," & userId & " USERID,'" & GetEntryDate(GetServerDate(tran), tran) & "'UPDATED,'" & GetServerTime(tran) & "' UPTIME,'" & VERSION & "' APPVER,1,COSTID,'" & RefNo & "','" & txtReason.Text & "'"
                '    strSql += "     FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & ro.Item("SNO").ToString & "'"
                '    strSql += "     END"
                '    ExecQuery(SyncMode.Stock, strSql, cn, tran, costId)

                '    strSql = vbCrLf + " DECLARE @ENTORDER INT"
                '    strSql += vbCrLf + " SELECT @ENTORDER = ISNULL(MAX(ENTORDER),0)+1 FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO = '" & ro.Item("SNO").ToString & "'"
                '    strSql += " INSERT INTO " & cnAdminDb & "..CTRANSFER"
                '    strSql += " (SNO,TAGSNO,ITEMID,TAGNO,ITEMCTRID,RECDATE,TAGVAL,USERID"
                '    strSql += " ,UPDATED,UPTIME,APPVER,ENTORDER,COSTID,REFNO,REASON)"
                '    strSql += " SELECT '" & GetNewSno(TranSnoType.CTRANSFERCODE, tran, "GET_ADMINSNO_TRAN") & "' AS SNO,SNO TAGSNO"
                '    strSql += " ,ITEMID,TAGNO," & ctrId & ",'" & GetEntryDate(GetServerDate(tran), tran) & "' AS RECDATE"
                '    strSql += " ,TAGVAL," & userId & " USERID,'" & GetEntryDate(GetServerDate(tran), tran) & "' UPDATED,'" & GetServerTime(tran) & "' UPTIME,'" & VERSION & "' APPVER,@ENTORDER,COSTID,'" & RefNo & "','" & txtReason.Text & "'"
                '    strSql += " FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & ro.Item("SNO").ToString & "'"
                '    ExecQuery(SyncMode.Stock, strSql, cn, tran, costId)

                '    strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET ITEMCTRID = " & ctrId & ""
                '    strSql += " ,TRANSFERDATE = '" & GetEntryDate(GetServerDate(tran), tran) & "' "
                '    strSql += " WHERE SNO = '" & ro.Item("SNO").ToString & "'"
                '    '071213
                '    'strSql += " AND ITEMCTRID = " & oldCtrId & ""
                '    cmd = New OleDbCommand(strSql, cn, tran)
                '    cmd.ExecuteNonQuery()
                '    ExecQuery(SyncMode.Stock, strSql, cn, tran, costId, , , , False)
                '    ExecCount += 1
                'Next
                Dim SelCnt As Integer = Val(gridViewFooter.Rows(1).Cells("TAGNO").Value.ToString)
                If ExecCount <> SelCnt Then
                    If tran IsNot Nothing Then tran.Rollback() : tran = Nothing
                    MsgBox("Selected Entry not matched with Transfered Entry.", MsgBoxStyle.Information)
                    Exit Sub
                End If
                tran.Commit()
                tran = Nothing
                If ExecCount <> 0 Then
                    MsgBox("Successfully Transfered.." & vbCrLf & "Trf Ref No." & RefNo)
                End If

                If IsDUPTAGCTRCHANGE Then
                    Dim oldItem As Integer = Nothing
                    Dim paramStr As String = ""
                    Dim prnmemsuffix As String = ""
                    If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId

                    Dim write As StreamWriter
                    Dim memfile As String = "\Barcodeprint" & prnmemsuffix & ".mem"
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    '        write = IO.File.CreateText(Application.StartupPath & "\Barcodeprint.mem")
                    For Each ro As DataRow In dv.ToTable.Rows
                        'For Each ro As DataRow In roSelected
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
                btnNew_Click(Me, New EventArgs)
            Catch ex As Exception
                If tran IsNot Nothing Then tran.Rollback()
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End Try
        End If
    End Sub

    Private Sub chkDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        dtpFrom.Enabled = chkDate.Checked
        dtpTo.Enabled = chkDate.Checked
    End Sub

    Private Sub txtTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTagNo.Text = "" And chkScan.Checked = False Then Exit Sub
            strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = (SELECT TOP 1 ITEMCTRID FROM " & cnAdminDb & "..ITEMTAG "
            strSql += vbCrLf + " WHERE TAGNO = '" & txtTagNo.Text & "' AND ISSDATE IS NULL "
            If Val(txtItemId.Text) > 0 Then strSql += vbCrLf + " AND ITEMID = " & txtItemId.Text & ""
            strSql += " )"
            cmbOldCounter_MAN.Text = objGPack.GetSqlValue(strSql)
            chkCmbCounter_Man.Text = objGPack.GetSqlValue(strSql)
            If Tag_Auto_CtrChange Then
                strSql = " SELECT (SELECT TOP 1 ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID=C.TARGETCTRID)NAME "
                strSql += " FROM " & cnAdminDb & "..ITEMCOUNTER C"
                'strSql += " WHERE ACTIVE = 'Y' AND ITEMCTRNAME = '" & cmbOldCounter_MAN.Text & "' "
                strSql += " WHERE ACTIVE = 'Y' AND ITEMCTRNAME  IN (" & GetQryString(chkCmbCounter_Man.Text) & ") "
                cmbNewCounter_MAN.Text = objGPack.GetSqlValue(strSql, "NAME", "")
                If cmbNewCounter_MAN.Text.ToString = "" Then
                    strSql = vbCrLf + " SELECT TOP 1 (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER  "
                    strSql += vbCrLf + " WHERE ITEMCTRID = ISNULL(C.TARGETCTRID,C.ITEMCTRID)) ITEMCTRNAME "
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMCOUNTER AS C"
                    strSql += vbCrLf + " WHERE ITEMCTRNAME = '" & cmbOldCounter_MAN.Text & "' AND ACTIVE = 'Y'"
                    cmbNewCounter_MAN.Text = objGPack.GetSqlValue(strSql, "ITEMCTRNAME", "")
                End If
            End If
            Dim dtfound As New DataTable
            strSql = "SELECT 1 FROM " & cnAdminDb & "..PITEMTAG T WHERE TAGNO='" & txtTagNo.Text & "' "
            If Val(txtItemId.Text) > 0 Then strSql += vbCrLf + " AND ITEMID = " & txtItemId.Text & ""
            da = New OleDbDataAdapter(strSql, cn)
            dtfound = New DataTable
            da.Fill(dtfound)
            If dtfound.Rows.Count > 0 Then
                MsgBox("Tagno found in Pending Transist " & vbCrLf & "Can not change the counter")
                txtTagNo.SelectAll() : txtTagNo.Focus()
                Exit Sub
            End If

            btnSearch_Click(sender, e)
        End If
    End Sub

    Private Sub txtFindTag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFindTag.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If dtGrid Is Nothing Then Exit Sub
            If Not gridView.Rows.Count > 0 Then Exit Sub
            If txtFindTag.Text = "" Then Exit Sub

            ''''Dim PRODTAGSEP As String = GetAdmindbSoftValue("PRODTAGSEP", "")
            ''''If txtFindTag.Text.Contains(PRODTAGSEP) Then
            ''''    Dim sp() As String = txtItemId.Text.Split(PRODTAGSEP)
            ''''    sp = txtFindTag.Text.Split(PRODTAGSEP)
            ''''    txtFindTag.Text = Trim(sp(1))
            ''''End If

            ''''Dim Row() As DataRow = dtGrid.Select("TAGNO = '" & txtFindTag.Text & "'")
            Dim PRODTAGSEP As String = GetAdmindbSoftValue("PRODTAGSEP", "")
            If txtFindTag.Text.Contains(PRODTAGSEP) Then
                Dim sp() As String = txtItemId.Text.Split(PRODTAGSEP)
                sp = txtFindTag.Text.Split(PRODTAGSEP)
                txtFindTag.Text = Trim(sp(1))
            End If
            Dim Row() As DataRow = Nothing
            If Not txtFindTag.Text.Contains(PRODTAGSEP) Then
                strSql = "SELECT TOP 1 ISNULL(TAGNO,'')TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY ='" & txtFindTag.Text.Trim.Replace(PRODTAGSEP, "") & "'"
                Dim temptagno As String = GetSqlValue(cn, strSql)
                If Not temptagno Is Nothing Then
                    Row = dtGrid.Select("TAGNO = '" & temptagno.ToString.Trim & "'")
                End If
            End If
            If Row Is Nothing Then
                Row = dtGrid.Select("TAGNO = '" & txtFindTag.Text & "'")
            End If

            If Row.Length > 1 Then
                If Val(txtItemId.Text) = 0 Then
                    MsgBox("This tag no exist in multiple items" + vbCrLf + "Please give itemid id in filteration", MsgBoxStyle.Information)
                    txtItemId.Select()
                    Exit Sub
                End If
            End If
            If Row.Length > 0 Then
                gridView.Rows(Val(Row(0).Item("KEYNO".ToString))).Cells("CHECK").Value = True
                txtFindTag.Clear()
            End If
        End If
    End Sub

    Private Sub cmbOldCounter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub cmbNewCounter_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbNewCounter_MAN.GotFocus
        'If cmbOldCounter_MAN.Text <> "" Then
        '    strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' and itemctrname <> '" & cmbOldCounter_MAN.Text & "' ORDER BY DISPLAYORDER,ITEMCTRNAME"
        '    'cmbNewCounter_MAN = Nothing
        '    objGPack.FillCombo(strSql, cmbNewCounter_MAN)
        'End If
        If chkCmbCounter_Man.Text <> "" Then
            strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' and itemctrname <> '" & chkCmbCounter_Man.Text & "' "
            'cmbNewCounter_MAN = Nothing
            If TagCtrChangeUserBased And userId <> 999 Then
                Dim useritemctr As String = ""
                Dim qrystr As String = ""
                qrystr = " SELECT ISNULL(ITEMCTRID,'')ITEMCTRID  FROM " & cnAdminDb & "..USERCASH WHERE USERID ='" & userId.ToString & "' AND ISNULL(ITEMCTRID,'')<>''"
                useritemctr = GetSqlValue(cn, qrystr).ToString
                If useritemctr <> "" Then
                    strSql += " AND ITEMCTRID IN (" & useritemctr.ToString & ")"
                End If
            End If
            strSql += "  ORDER BY DISPLAYORDER,ITEMCTRNAME"
            objGPack.FillCombo(strSql, cmbNewCounter_MAN)
        End If

    End Sub

    Private Sub txtItemId_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim PRODTAGSEP As String = GetAdmindbSoftValue("PRODTAGSEP", "")
            Dim sp() As String = txtItemId.Text.Split(PRODTAGSEP)
            Dim SCANSTR As String = txtItemId.Text
            If PRODTAGSEP <> "" And txtItemId.Text <> "" Then
                sp = txtItemId.Text.Split(PRODTAGSEP)
                txtItemId.Text = Trim(sp(0))
                If sp.Length > 1 Then
                    If Len(SCANSTR) > Len(Trim(sp(0)) & PRODTAGSEP & Trim(sp(1))) Then SCANSTR = Trim(sp(0)) & PRODTAGSEP & Trim(sp(1))
                End If
            End If
CheckItem:
            Dim dtItemDet As New DataTable
            If txtItemId.Text = "" Then
                'LoadSalesItemName()
                Exit Sub
            ElseIf IsNumeric(SCANSTR) = False And SCANSTR.Contains(PRODTAGSEP) = False Then
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(SCANSTR) & "'"
                dtItemDet = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtItemId.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    GoTo LoadItemInfo
                End If
            ElseIf IsNumeric(SCANSTR) = True And SCANSTR.Contains(PRODTAGSEP) = False And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Val(SCANSTR) & "'" & GetItemQryFilteration()) = True Then
                'LoadSalesItemNameDetail() : Exit Sub
            ElseIf PRODTAGSEP <> "" Then
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(SCANSTR).Replace(PRODTAGSEP, "") & "' AND ISSDATE IS NULL"
                dtItemDet = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtItemId.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    GoTo LoadItemInfo
                End If
            ElseIf txtItemId.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemId.Text) & "'" & GetItemQryFilteration()) = False Then
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(txtItemId.Text) & "' AND ISSDATE IS NULL"
                dtItemDet = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtItemId.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    GoTo LoadItemInfo
                Else
                    '   LoadSalesItemName()
                    Exit Sub
                End If
            Else
LoadItemInfo:
                'LoadSalesItemNameDetail()
            End If
            If sp.Length > 1 And PRODTAGSEP <> "" And txtItemId.Text <> "" Then
                txtTagNo.Text = Trim(sp(1))
            End If
            If txtTagNo.Text <> "" Then
                txtTagNo.Focus()
                txtTagNo_KeyPress(Me, New KeyPressEventArgs(e.KeyChar))
            End If
        End If
    End Sub

    Private Sub chkScan_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkScan.KeyPress
        ''290613
        'If chkScan.Checked = True Then lblItemid.Focus() Else lblOldCounter.Focus()
    End Sub

    Private Sub txtEstNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEstNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtEstNo.Text <> "" Then
                btnSearch_Click(sender, e)
            End If
        End If
    End Sub

    Private Sub chkScan_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkScan.LostFocus
        '290613
        'If chkScan.Checked = True Then txtItemId.Focus() Else cmbOldCounter_MAN.Focus() 'lblOldCounter.Focus()
        If Tag_Auto_CtrChange Then
            If chkScan.Checked = True Then txtItemId.Focus() Else chkCmbCounter_Man.Focus() 'cmbOldCounter_MAN.Focus() 'lblOldCounter.Focus()
        Else
            If chkScan.Checked = True Then cmbNewCounter_MAN.Focus() Else chkCmbCounter_Man.Focus() 'cmbOldCounter_MAN.Focus() 'lblOldCounter.Focus()
        End If
    End Sub

    Private Sub txtPktNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPktNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPktNo.Text <> "" Then
                btnSearch_Click(sender, e)
            ElseIf e.KeyChar = Chr(Keys.Escape) Then
                If TabMain.SelectedTab.Name = TabPage2.Name Then
                    btnBack_Click(Me, New EventArgs)
                End If
            End If
        End If
    End Sub

    Private Sub btnview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnview.Click
        dgview.DataSource = Nothing
        TabMain.SelectedTab = TabPage2
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        TabMain.SelectedTab = TabGeneral
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        dgview.DataSource = Nothing

        strSql = vbCrLf + " SELECT  TAGNO,RECDATE AS TRANDATE,ITEMID, "
        strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=A.ITEMID)ITEMNAME, "
        strSql += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & " ..SUBITEMMAST WHERE SUBITEMID =A.SUBITEMID)SUBITEM,GRSWT,NETWT,"
        strSql += vbCrLf + " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID=A.ITEMCTRID)COUNTERNAME "
        strSql += vbCrLf + " FROM ( "
        strSql += vbCrLf + " SELECT CT.TAGNO,CT.ITEMID,CT.RECDATE,CT.ITEMCTRID "
        strSql += vbCrLf + " ,(SELECT SUBITEMID FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID  =CT.ITEMID AND TAGNO  =CT.TAGNO )AS SUBITEMID,"
        strSql += vbCrLf + " (SELECT   GRSWT FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID  =CT.ITEMID AND TAGNO  =CT.TAGNO )AS GRSWT,"
        strSql += vbCrLf + " (SELECT  NETWT FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID  =CT.ITEMID AND TAGNO  =CT.TAGNO )AS NETWT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CTRANSFER CT"
        strSql += vbCrLf + " WHERE  CT.RECDATE  BETWEEN '" & dtptabfrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtptabto.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " )A  ORDER BY RECDATE"

        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dt.Columns.Add(dtCol)
        da.Fill(dt)
        If dt.Rows.Count < 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            dgview.DataSource = Nothing
            dtptabfrom.Focus()
        Else
            dgview.DataSource = Nothing
            dgview.DataSource = dt
            dgview.Columns("CHECK").HeaderText = ""
            dgview.Columns("CHECK").Width = 30
            dgview.Columns("TAGNO").Width = 100
            dgview.Columns("TRANDATE").Width = 100
            dgview.Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            dgview.Columns("ITEMNAME").Width = 100
            dgview.Columns("COUNTERNAME").Width = 100
            dgview.Columns("COUNTERNAME").HeaderText = "COUNTER NAME"
            dgview.Columns("ITEMID").Width = 50
            dgview.Columns("SUBITEM").Width = 125
            dgview.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            'dgview.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgview.Columns("NETWT").Visible = False
        End If
    End Sub
    Private Sub btnDuplicate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDuplicate.Click
        dt = New DataTable
        dt = CType(dgview.DataSource, DataTable).Copy
        Dim dv As DataView
        dv = dt.DefaultView
        dv.RowFilter = "CHECK = TRUE"
        If IsDUPTAGCTRCHANGE Then
            Dim oldItem As Integer = Nothing
            Dim paramStr As String = ""
            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId

            Dim write As StreamWriter
            Dim memfile As String = "\Barcodeprint" & prnmemsuffix & ".mem"
            write = IO.File.CreateText(Application.StartupPath & memfile)

            For Each ro As DataRow In dv.ToTable.Rows
                'For Each ro As DataRow In roSelected
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
    Private Sub ChkAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkAll.CheckedChanged
        For Each dgvRow As DataGridViewRow In dgview.Rows
            dgvRow.Cells("CHECK").Value = ChkAll.Checked
        Next
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnview_Click(Me, New EventArgs)
    End Sub

    Private Sub txtReason_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtReason.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtReason.Text.ToString.Trim = "" And TagCtrChangeReason Then
                MsgBox("Reason should not be empty...", MsgBoxStyle.Information)
                txtReason.Focus()
            End If
        End If
    End Sub

    Private Sub cmbOldCounter_MAN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOldCounter_MAN.SelectedIndexChanged
        If cmbOldCounter_MAN.Text <> "" Then
            strSql = "SELECT ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE I.ITEMCTRID = TRFITEMCTRID),'')TRFITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER I WHERE ITEMCTRNAME = '" & cmbOldCounter_MAN.Text.ToString & "'"
            Dim NewCounter As String = ""
            NewCounter = GetSqlValue(cn, strSql)
            If NewCounter <> "" Then
                cmbNewCounter_MAN.Text = NewCounter
                cmbNewCounter_MAN.Enabled = False
            Else
                cmbNewCounter_MAN.Items.Clear()
                strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ISNULL(ACTIVE,'')<> 'N' "
                If TagCtrChangeUserBased And userId <> 999 Then
                    Dim useritemctr As String = ""
                    Dim qrystr As String = ""
                    qrystr = " SELECT ISNULL(ITEMCTRID,'')ITEMCTRID  FROM " & cnAdminDb & "..USERCASH WHERE USERID ='" & userId.ToString & "' AND ISNULL(ITEMCTRID,'')<>''"
                    useritemctr = GetSqlValue(cn, qrystr).ToString
                    If useritemctr <> "" Then
                        strSql += " AND ITEMCTRID IN (" & useritemctr.ToString & ")"
                    End If
                End If
                strSql += "  ORDER BY DISPLAYORDER,ITEMCTRNAME"
                objGPack.FillCombo(strSql, cmbNewCounter_MAN)
                If cmbNewCounter_MAN.Items.Count > 0 Then
                    cmbNewCounter_MAN.Text = cmbNewCounter_MAN.Items(0).ToString
                End If
                cmbNewCounter_MAN.Enabled = True
            End If
        End If
    End Sub

    Private Sub chkScan_CheckedChanged(sender As Object, e As EventArgs) Handles chkScan.CheckedChanged

    End Sub
End Class