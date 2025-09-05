Imports System.Data.OleDb
Public Class frmPendingLotView
    Dim strSql As String
    Dim objGridShower As frmGridDispDia
    Dim hasDelete As Boolean
    Dim Tag_Tolerance As Decimal = Val(GetAdmindbSoftValue("TAGTOLERANCE", "0"))
    Dim ragshow As Boolean = IIf(GetAdmindbSoftValue("RANGEINLOT", "N") = "Y", True, False)

    Private Sub frmPendingLotView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub frmPendingLotView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        TabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.TabMain.Region = New Region(New RectangleF(Me.TabGen.Left, Me.TabGen.Top, Me.TabGen.Width, Me.TabGen.Height))
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY METALNAME"
        FillCheckedListBox(strSql, chkLstMetal)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            FillCheckedListBox(strSql, chkLstCostCentre)
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY ITEMCTRNAME"
        FillCheckedListBox(strSql, chkLstItemCounter)
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        FillCheckedListBox(strSql, chkLstDesigner)
        hasDelete = objGPack.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
        hasDelete = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpAsOnDate.Value = GetEntryDate(GetServerDate)
        Prop_Gets()
        chkItemCounterSelectAll_CheckedChanged(Me, New EventArgs)
        chkDesignerSelectAll_CheckedChanged(Me, New EventArgs)
        'dtpAsOnDate.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub chkMetalSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMetalSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstMetal, chkMetalSelectAll.Checked)
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub chkItemCounterSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkItemCounterSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItemCounter, chkItemCounterSelectAll.Checked)
    End Sub


    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim Cmd As OleDbCommand
        If chkLstCostCentre.Items.Count > 0 Then
            If Not chkLstCostCentre.CheckedItems.Count > 0 Then
                chkCostCentreSelectAll.Checked = True
            End If
        End If
        If chkLstDesigner.Items.Count > 0 Then
            If Not chkLstDesigner.CheckedItems.Count > 0 Then
                chkDesignerSelectAll.Checked = True
            End If
        End If
        If chkLstItemCounter.Items.Count > 0 Then
            If Not chkLstItemCounter.CheckedItems.Count > 0 Then
                chkItemCounterSelectAll.Checked = True
            End If
        End If
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        Dim chkItemCounter As String = GetChecked_CheckedList(chkLstItemCounter)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        Dim chkDesigner As String = GetChecked_CheckedList(chkLstDesigner)

        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PENDINGLOTVIEW')> 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "PENDINGLOTVIEW"
        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMLOT') > 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ITEMLOT"
        Cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        strSql = " SELECT * INTO TEMPTABLEDB..TEMP" & systemId & "ITEMLOT"
        strSql += " FROM " & cnAdminDb & "..ITEMLOT AS L"
        strSql += vbCrLf + " WHERE "
        'If chkAsonDate.Checked = True Then strSql += "LOTDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND LOTDATE>='" & Format(cnTranFromDate, "yyyy-MM-dd") & "'"
        If chkAsonDate.Checked = True Then strSql += "LOTDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        If chkAsonDate.Checked = False Then strSql += "LOTDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' and '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkItemCounter <> "" Then
            strSql += vbCrLf + " AND ISNULL(ITEMCTRID,0) IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & ")"
            If chkItemCounterSelectAll.Checked = True Then strSql += "  UNION ALL  SELECT 0 ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER "
            strSql += " )"
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkDesigner <> "" Then
            strSql += " AND ISNULL(DESIGNERID,0) IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & ")"
            If chkDesignerSelectAll.Checked = True Then strSql += "  UNION ALL  SELECT 0 DESIGNERID FROM " & cnAdminDb & "..DESIGNER"
            strSql += " )"
        Else
            strSql += " AND ISNULL(DESIGNERID,0) IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'')='Y'"
            If chkDesignerSelectAll.Checked = True Then strSql += "  UNION ALL  SELECT 0 DESIGNERID FROM " & cnAdminDb & "..DESIGNER"
            strSql += " )"
        End If
        Dim minbalwt As Decimal = 0
        If chkwithMinbal.Checked = False Then minbalwt = Tag_Tolerance
        If minbalwt <> 0 Then
            strSql += vbCrLf + " AND ((PCS-CPCS) > 0 OR (GRSWT-CGRSWT) > " & minbalwt & ")"
        Else
            If Not chkWithPendingCompletedLot.Checked Then
                strSql += vbCrLf + " AND ((PCS-CPCS) <> 0 OR (GRSWT-(CGRSWT-ISNULL(DGRSWT,0))) <> 0 OR (NETWT-(CNETWT-ISNULL(DNETWT,0))) <> 0 )"
            Else
                strSql += vbCrLf + " AND ((PCS-CPCS) <> 0 OR (GRSWT-CGRSWT) <> 0 OR (NETWT-CNETWT) <> 0 )"
            End If
        End If
        If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        If Not chkWithPendingCompletedLot.Checked Then
            'strSql += vbCrLf + " AND ISNULL(COMPLETED,'') <> 'Y'"
            If chkAsonDate.Checked = True Then
                strSql += vbCrLf + " AND (CDATE IS NULL OR CDATE>'" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')"
            Else
                strSql += vbCrLf + " AND (CDATE IS NULL OR CDATE>'" & dtpTo.Value.ToString("yyyy-MM-dd") & "')"
            End If
        End If
        strSql += vbCrLf + " AND  ISNULL(CANCEL,'') <> 'Y'"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "ITEMLOT"
        strSql += vbCrLf + " SET PCS = 0,GRSWT = 0,NETWT = 0,DIAPCS = T.PCS"
        strSql += vbCrLf + " ,DIAWT = T.GRSWT"
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "ITEMLOT AS L"
        strSql += " INNER JOIN " & cnAdminDb & "..ITEMLOT AS T ON T.SNO = L.SNO"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'D')"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()


        strSql = " SELECT CONVERT(VARCHAR,LOTDATE,103)LOTDATE,LOTNO"
        If chkOrderByItemId.Checked Then
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(L.ITEMID)) + CONVERT(VARCHAR,L.ITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = L.ITEMID)AS ITEM"
        Else
            strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = L.ITEMID)AS ITEM"
            'strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = L.ITEMID)AS ITEM"
        End If
        strSql += vbCrLf + " ,PCS-CPCS AS BPCS"
        strSql += vbCrLf + " ,GRSWT-CGRSWT AS BGRSWT"
        strSql += vbCrLf + " ,NETWT-CNETWT AS BNETWT"
        strSql += vbCrLf + " ,DIAPCS"
        strSql += vbCrLf + " -ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO = L.SNO) AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') ),0) AS DIAPCS"
        strSql += vbCrLf + " ,DIAWT"
        strSql += vbCrLf + " -ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO = L.SNO) AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') ),0) AS DIAWT"
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = L.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,TABLECODE"
        strSql += vbCrLf + " ,NARRATION"
        If ragshow = True Then strSql += vbCrLf + " ,(SELECT CAPTION FROM " & cnAdminDb & "..RANGEMAST WHERE SNO=L.RANGESNO)RANGE"
        strSql += vbCrLf + " ,LOTDATE LDATE,1 RESULT,' 'COLHEAD,SNO,COMPLETED,COSTID"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "PENDINGLOTVIEW"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ITEMLOT AS L"

        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "PENDINGLOTVIEW) > 0"
        strSql += " BEGIN"
        strSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "PENDINGLOTVIEW"
        strSql += " ("
        strSql += " LOTDATE,BPCS,BGRSWT,BNETWT,DIAPCS,DIAWT,RESULT,COLHEAD"
        strSql += " )"
        strSql += " SELECT 'GRAND TOTAL',SUM(BPCS),SUM(BGRSWT),SUM(BNETWT),SUM(DIAPCS),SUM(DIAWT),3 RESULT,'G' COLHEAD"
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "PENDINGLOTVIEW"
        strSql += " END"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "PENDINGLOTVIEW SET DIAPCS = NULL WHERE DIAPCS = 0"
        strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "PENDINGLOTVIEW SET DIAWT = NULL WHERE DIAWT = 0"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        Prop_Sets()
        strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "PENDINGLOTVIEW"
        If chkOrderByItemId.Checked Then
            strSql += vbCrLf + " ORDER BY RESULT,ITEM"
        Else
            strSql += vbCrLf + " ORDER BY RESULT,LDATE,LOTNO"
        End If

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(String))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If ChkMultiUpdate.Checked Then
            Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
            dtCol.DefaultValue = False
            dtGrid.Columns.Add(dtCol)
        End If
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        With DgvView
            .DataSource = Nothing
            .DataSource = dtGrid
            If ChkMultiUpdate.Checked Then
                btnUpdate.Enabled = True
                .Columns("CHECK").DisplayIndex = 0
                ChkAll.Visible = True
                ChkAll.Checked = False
            Else
                btnUpdate.Enabled = False
                ChkAll.Visible = False
            End If
        End With
        TabMain.SelectedTab = TabView
        Dim title As String
        title = "PENDING LOT VIEW FOR " + IIf(chkMetalName <> "", chkMetalName.Replace("'", ""), " ALL METAL") + vbCrLf
        If chkAsonDate.Checked = True Then title += "AS ON DATE  " + dtpAsOnDate.Text
        If chkAsonDate.Checked = False Then title += " BETWEEN DATE  " + dtpAsOnDate.Text & " AND " & dtpTo.Text
        If chkWithPendingCompletedLot.Checked Then
            title += " WITH PENDING COMPLETED"
        End If
        lblTitle.Text = title
        DataGridView_Detailed_None(DgvView)
        MarkCompleted(dtGrid)
    End Sub

    Private Sub MarkCompleted(ByVal dt As DataTable)
        Dim rowCompleted() As DataRow = Nothing
        rowCompleted = dt.Select("COMPLETED = 'Y'")
        For cnt As Integer = 0 To rowCompleted.Length - 1
            DgvView.Rows(Val(rowCompleted(cnt).Item("KEYNO").ToString)).DefaultCellStyle.BackColor = Color.Red
        Next
    End Sub

    Private Sub DataGridView_Detailed_None(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("LOTDATE").Width = 100
            .Columns("LOTNO").Width = 60
            .Columns("ITEM").Width = 120
            .Columns("BPCS").Width = 60
            .Columns("BGRSWT").Width = 80
            .Columns("BNETWT").Width = 80
            .Columns("DESIGNER").Width = 120
            .Columns("TABLECODE").Width = 80
            .Columns("NARRATION").Width = 120
            .Columns("DIAPCS").Width = 60
            .Columns("DIAWT").Width = 80
            .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            .Columns("BPCS").HeaderText = "PCS"
            .Columns("BGRSWT").HeaderText = "GRSWT"
            .Columns("BNETWT").HeaderText = "NETWT"
            .Columns("LDATE").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("SNO").Visible = False
            .Columns("COMPLETED").Visible = False
            .Columns("COSTID").Visible = False
            .Columns("KEYNO").Visible = False
            BrighttechPack.GlobalMethods.FormatGridColumns(dgv, True)
            FillGridGroupStyle_KeyNoWise(dgv, "LOTDATE")
            If ChkMultiUpdate.Checked Then
                .Columns("CHECK").ReadOnly = False
                .Columns("CHECK").Width = 60
            End If
        End With
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Space) Then
            Dim Cmd As OleDbCommand
            With objGridShower.gridView
                If .Rows(.CurrentRow.Index).Cells("COMPLETED").Value.ToString = "" Then
                    If MessageBox.Show("Mark to complete?", "Mark Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        strSql = " UPDATE " & cnAdminDb & "..ITEMLOT "
                        strSql += " SET COMPLETED = 'Y'"
                        strSql += " ,DPCS=(CPCS-PCS)"
                        strSql += " ,DNETWT=(CNETWT-NETWT)"
                        strSql += " ,DGRSWT=(CGRSWT-GRSWT)"
                        strSql += " ,CDATE='" & GetEntryDate(GetServerDate()) & "'"
                        strSql += " WHERE SNO = '" & .Rows(.CurrentRow.Index).Cells("SNO").Value.ToString & "'"
                        Try
                            tran = Nothing
                            tran = cn.BeginTransaction
                            Cmd = New OleDbCommand(strSql, cn, tran)
                            Cmd.ExecuteNonQuery()
                            .Rows(.CurrentRow.Index).DefaultCellStyle.BackColor = Color.Red
                            tran.Commit()
                            tran = Nothing
                            ' MsgBox("Successfully Marked", MsgBoxStyle.Information)
                        Catch ex As Exception
                            If Not tran Is Nothing Then tran.Commit()
                            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                        End Try
                    End If
                Else
                    If MessageBox.Show("Mark to UnComplete?", "UnMark Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        strSql = " UPDATE " & cnAdminDb & "..ITEMLOT "
                        strSql += " SET COMPLETED = ''"
                        strSql += " ,DPCS=NULL"
                        strSql += " ,DNETWT=NULL"
                        strSql += " ,DGRSWT=NULL"
                        strSql += " ,CDATE=NULL"
                        strSql += " WHERE SNO = '" & .Rows(.CurrentRow.Index).Cells("SNO").Value.ToString & "'"
                        Try
                            tran = Nothing
                            tran = cn.BeginTransaction
                            Cmd = New OleDbCommand(strSql, cn, tran)
                            Cmd.ExecuteNonQuery()
                            tran.Commit()
                            tran = Nothing
                            .Rows(.CurrentRow.Index).DefaultCellStyle.BackColor = Nothing
                            'MsgBox("Successfully UnMarked", MsgBoxStyle.Information)
                        Catch ex As Exception
                            If Not tran Is Nothing Then tran.Commit()
                            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                        End Try
                    End If
                End If
            End With
        End If
    End Sub

    Private Sub GridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        With objGridShower.gridView
            If .CurrentRow Is Nothing Then Exit Sub
            If .Rows(.CurrentRow.Index).Cells("COMPLETED").Value.ToString = "" Then
                objGridShower.lblStatus.Text = "Press spacebar Mark to Complete"
            Else
                objGridShower.lblStatus.Text = "Press spacebar Mark to UnComplete"
            End If
            If hasDelete Then
                objGridShower.lblStatus.Text += "/ Press [Del] to Delete" + vbCrLf
                objGridShower.lblStatus.Text += "/ Press [Ctrl+Del] to Delete All"
            End If
        End With
    End Sub

    Private Sub chkDesignerSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDesignerSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstDesigner, chkDesignerSelectAll.Checked)
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmPendingLotView_Properties
        obj.p_chkWithPendingCompletedLot = chkWithPendingCompletedLot.Checked
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkDesignerSelectAll = chkDesignerSelectAll.Checked
        GetChecked_CheckedList(chkLstDesigner, obj.p_chkLstDesigner)
        obj.p_chkItemCounterSelectAll = chkItemCounterSelectAll.Checked
        GetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter)
        obj.p_chkOrderByItemId = chkOrderByItemId.Checked
        obj.p_chkWithminimumbalance = chkwithMinbal.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmPendingLotView_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmPendingLotView_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmPendingLotView_Properties))
        chkWithPendingCompletedLot.Checked = obj.p_chkWithPendingCompletedLot
        chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, Nothing)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkDesignerSelectAll.Checked = obj.p_chkDesignerSelectAll
        'SetChecked_CheckedList(chkLstDesigner, obj.p_chkLstDesigner, Nothing)
        chkItemCounterSelectAll.Checked = obj.p_chkItemCounterSelectAll
        'SetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter, Nothing)
        chkOrderByItemId.Checked = obj.p_chkOrderByItemId
        chkwithMinbal.Checked = obj.p_chkWithminimumbalance
    End Sub

    Private Sub chkAsonDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAsonDate.CheckedChanged
        If chkAsonDate.Checked Then dtpTo.Visible = False : lblTo.Visible = False : chkAsonDate.Text = "As On Date " Else dtpTo.Visible = True : lblTo.Visible = True : chkAsonDate.Text = "Date From"
    End Sub

    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If TabMain.SelectedTab.Name = "TabView" Then
            TabMain.SelectedTab = TabGen
        End If
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim dt As New DataTable
        dt = DgvView.DataSource
        Dim ros() As DataRow = Nothing
        ros = dt.Select("CHECK = TRUE")
        If Not ros.Length > 0 Then
            MsgBox("There is No Checked Record", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim Cmd As OleDbCommand
        If ChkMultiUpdate.Checked = False Then Exit Sub
        For I As Integer = 0 To DgvView.Rows.Count - 2
            With DgvView
                If .Rows(I).Cells("CHECK").Value = False Then Continue For
                If .Rows(I).Cells("COMPLETED").Value.ToString = "" Then
                    strSql = " UPDATE " & cnAdminDb & "..ITEMLOT "
                    strSql += " SET COMPLETED = 'Y'"
                    strSql += " ,DPCS=(CPCS-PCS)"
                    strSql += " ,DNETWT=(CNETWT-NETWT)"
                    strSql += " ,DGRSWT=(CGRSWT-GRSWT)"
                    strSql += " ,DDIAWT=(-1*" & Val(.Rows(I).Cells("DIAWT").Value.ToString) & ")"
                    strSql += " ,CDATE='" & GetEntryDate(GetServerDate()) & "'"
                    strSql += " WHERE SNO = '" & .Rows(I).Cells("SNO").Value.ToString & "'"
                    Try
                        tran = Nothing
                        tran = cn.BeginTransaction
                        Cmd = New OleDbCommand(strSql, cn, tran)
                        Cmd.ExecuteNonQuery()
                        .Rows(I).DefaultCellStyle.BackColor = Color.Red
                        tran.Commit()
                        tran = Nothing
                    Catch ex As Exception
                        If Not tran Is Nothing Then tran.Commit()
                        MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                    End Try
                Else
                    strSql = " UPDATE " & cnAdminDb & "..ITEMLOT "
                    strSql += " SET COMPLETED = ''"
                    strSql += " ,DPCS=NULL"
                    strSql += " ,DNETWT=NULL"
                    strSql += " ,DGRSWT=NULL"
                    strSql += " ,CDATE=NULL"
                    strSql += " ,DDIAWT=NULL"
                    strSql += " WHERE SNO = '" & .Rows(I).Cells("SNO").Value.ToString & "'"
                    Try
                        tran = Nothing
                        tran = cn.BeginTransaction
                        Cmd = New OleDbCommand(strSql, cn, tran)
                        Cmd.ExecuteNonQuery()
                        tran.Commit()
                        tran = Nothing
                        .Rows(I).DefaultCellStyle.BackColor = Nothing
                    Catch ex As Exception
                        If Not tran Is Nothing Then tran.Commit()
                        MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                    End Try
                End If
            End With
        Next
    End Sub

    Private Sub DgvView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles DgvView.KeyPress
        If ChkMultiUpdate.Checked Then Exit Sub
        If e.KeyChar = Chr(Keys.Space) Then
            Dim Cmd As OleDbCommand
            With DgvView
                If .Rows(.CurrentRow.Index).Cells("COMPLETED").Value.ToString = "" Then
                    If MessageBox.Show("Mark to complete?", "Mark Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        strSql = " UPDATE " & cnAdminDb & "..ITEMLOT "
                        strSql += " SET COMPLETED = 'Y'"
                        strSql += " ,DPCS=(CPCS-PCS)"
                        strSql += " ,DNETWT=(CNETWT-NETWT)"
                        strSql += " ,DGRSWT=(CGRSWT-GRSWT)"
                        strSql += " ,DDIAWT=(-1*" & Val(.Rows(.CurrentRow.Index).Cells("DIAWT").Value.ToString) & ")"
                        strSql += " ,CDATE='" & GetEntryDate(GetServerDate()) & "'"
                        strSql += " WHERE SNO = '" & .Rows(.CurrentRow.Index).Cells("SNO").Value.ToString & "'"
                        Try
                            tran = Nothing
                            tran = cn.BeginTransaction
                            Cmd = New OleDbCommand(strSql, cn, tran)
                            Cmd.ExecuteNonQuery()
                            .Rows(.CurrentRow.Index).DefaultCellStyle.BackColor = Color.Red
                            tran.Commit()
                            tran = Nothing
                        Catch ex As Exception
                            If Not tran Is Nothing Then tran.Commit()
                            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                        End Try
                    End If
                Else
                    If MessageBox.Show("Mark to UnComplete?", "UnMark Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        strSql = " UPDATE " & cnAdminDb & "..ITEMLOT "
                        strSql += " SET COMPLETED = ''"
                        strSql += " ,DPCS=NULL"
                        strSql += " ,DNETWT=NULL"
                        strSql += " ,DGRSWT=NULL"
                        strSql += " ,DDIAWT=NULL"
                        strSql += " ,CDATE=NULL"
                        strSql += " WHERE SNO = '" & .Rows(.CurrentRow.Index).Cells("SNO").Value.ToString & "'"
                        Try
                            tran = Nothing
                            tran = cn.BeginTransaction
                            Cmd = New OleDbCommand(strSql, cn, tran)
                            Cmd.ExecuteNonQuery()
                            tran.Commit()
                            tran = Nothing
                            .Rows(.CurrentRow.Index).DefaultCellStyle.BackColor = Nothing
                        Catch ex As Exception
                            If Not tran Is Nothing Then tran.Commit()
                            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                        End Try
                    End If
                End If
            End With
        ElseIf e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExport_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        ElseIf e.KeyChar = "C" Or e.KeyChar = "c" Then
            Dim Cmd As OleDbCommand
            With DgvView
                If .Rows(.CurrentRow.Index).Cells("COMPLETED").Value.ToString = "" Then
                    strSql = " SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ITEMLOT "
                    strSql += " WHERE SNO = '" & .Rows(.CurrentRow.Index).Cells("SNO").Value.ToString & "' AND ISNULL(CANCEL,'')='Y'"
                    If Val(GetSqlValue(cn, strSql).ToString) > 0 Then
                        MsgBox("Cancelled Lot Cannot Be Deleted", MsgBoxStyle.Information)
                        Exit Sub
                    End If

                    strSql = " SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ITEMTAG "
                    strSql += " WHERE SNO = '" & .Rows(.CurrentRow.Index).Cells("SNO").Value.ToString & "'"
                    If Val(GetSqlValue(cn, strSql).ToString) > 0 Then
                        MsgBox("Taged Lot Cannot Be Deleted", MsgBoxStyle.Information)
                        Exit Sub
                    End If

                    strSql = " SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ITEMNONTAG "
                    strSql += " WHERE SNO = '" & .Rows(.CurrentRow.Index).Cells("SNO").Value.ToString & "'"
                    If Val(GetSqlValue(cn, strSql).ToString) > 0 Then
                        MsgBox("Entered Lot Cannot Be Deleted", MsgBoxStyle.Information)
                        Exit Sub
                    End If

                    strSql = " DELETE FROM " & cnAdminDb & "..ITEMLOT "
                    strSql += " WHERE SNO = '" & .Rows(.CurrentRow.Index).Cells("SNO").Value.ToString & "' "
                    Try
                        tran = Nothing
                        tran = cn.BeginTransaction
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                        tran.Commit()
                        tran = Nothing
                        btnSearch_Click(sender, e)
                    Catch ex As Exception
                        If Not tran Is Nothing Then tran.Commit()
                        MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                    End Try
                Else
                    MsgBox("Completed Lot Cannot Be Deleted", MsgBoxStyle.Information)
                    Exit Sub
                End If

            End With
        End If
    End Sub

    Private Sub ChkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkAll.CheckedChanged
        For i As Integer = 0 To DgvView.Rows.Count - 2
            DgvView.Rows(i).Cells("CHECK").Value = ChkAll.Checked
        Next
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If DgvView.Rows.Count > 0 And TabMain.SelectedTab.Name = TabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, DgvView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If DgvView.Rows.Count > 0 And TabMain.SelectedTab.Name = TabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, DgvView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
End Class

Public Class frmPendingLotView_Properties
    Private chkWithPendingCompletedLot As Boolean = False
    Public Property p_chkWithPendingCompletedLot() As Boolean
        Get
            Return chkWithPendingCompletedLot
        End Get
        Set(ByVal value As Boolean)
            chkWithPendingCompletedLot = value
        End Set
    End Property
    Private chkWithminimumbalance As Boolean = False
    Public Property p_chkWithminimumbalance() As Boolean
        Get
            Return chkWithminimumbalance
        End Get
        Set(ByVal value As Boolean)
            chkWithminimumbalance = value
        End Set
    End Property
    Private chkCostCentreSelectAll As Boolean = False
    Public Property p_chkCostCentreSelectAll() As Boolean
        Get
            Return chkCostCentreSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCostCentreSelectAll = value
        End Set
    End Property
    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
    Private chkMetalSelectAll As Boolean = False
    Public Property p_chkMetalSelectAll() As Boolean
        Get
            Return chkMetalSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkMetalSelectAll = value
        End Set
    End Property
    Private chkLstMetal As New List(Of String)
    Public Property p_chkLstMetal() As List(Of String)
        Get
            Return chkLstMetal
        End Get
        Set(ByVal value As List(Of String))
            chkLstMetal = value
        End Set
    End Property
    Private chkDesignerSelectAll As Boolean = False
    Public Property p_chkDesignerSelectAll() As Boolean
        Get
            Return chkDesignerSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkDesignerSelectAll = value
        End Set
    End Property
    Private chkLstDesigner As New List(Of String)
    Public Property p_chkLstDesigner() As List(Of String)
        Get
            Return chkLstDesigner
        End Get
        Set(ByVal value As List(Of String))
            chkLstDesigner = value
        End Set
    End Property
    Private chkItemCounterSelectAll As Boolean = False
    Public Property p_chkItemCounterSelectAll() As Boolean
        Get
            Return chkItemCounterSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkItemCounterSelectAll = value
        End Set
    End Property
    Private chkLstItemCounter As New List(Of String)
    Public Property p_chkLstItemCounter() As List(Of String)
        Get
            Return chkLstItemCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstItemCounter = value
        End Set
    End Property
    Private chkOrderByItemId As Boolean = False
    Public Property p_chkOrderByItemId() As Boolean
        Get
            Return chkOrderByItemId
        End Get
        Set(ByVal value As Boolean)
            chkOrderByItemId = value
        End Set
    End Property
End Class