Imports System.Data.OleDb
Imports java.sql
Public Class StockTransfer
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtGrid As DataTable
    Dim IsNonTag As Boolean = False
    Private Sub frmStockTransfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtFindTag.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTagNo.Text = "" Then Exit Sub
            strSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = (SELECT TOP 1 COMPANYID FROM " & cnAdminDb & "..ITEMTAG "
            strSql += vbCrLf + " WHERE TAGNO = '" & txtTagNo.Text & "'"
            If Val(txtItemId_NUM.Text) > 0 Then strSql += vbCrLf + " AND ITEMID = " & txtItemId_NUM.Text & ""
            strSql += " )"
            cmbOldCompany.Text = objGPack.GetSqlValue(strSql)
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView_OWN.DataSource = Nothing
        lblRowDet1.Text = ""
        Me.Refresh()
        Dim selectcostid As String = ""
        If cmbCostCentre.Text <> "" Then
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'"
            selectcostid = objGPack.GetSqlValue(strSql, , "")
        Else
            selectcostid = cnCostId
        End If
        If IsNonTag Then
            strSql = " SELECT RECDATE"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
            strSql += vbCrLf + " ,PCS-sum(I.PCS),GRSWT-SUM(I.GRSWT),NETWT-SUM(I.NETWT),SALVALUE-SUM(I.SALVALUE)"
            strSql += vbCrLf + " ,'MULTI' AS LOTNO"
            strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)aS COUNTER"
            strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)aS DESIGNER,SNO"
            strSql += vbCrLf + " ,'' HM"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS T"
            strSql += vbCrLf + " WHERE "
            If dtpFrom.Enabled Then
                strSql += " RECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += " 1 = 1"
            End If
            If txtLotNo_NUM.Text <> "" Then strSql += vbCrLf + " AND LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
            strSql += " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & cmbOldCompany.Text & "')"
            If cmbNewCounter.Text <> "ALL" And cmbNewCounter.Text <> "" Then
                strSql += " AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbNewCounter.Text & "')"
            End If
            If Val(txtItemId_NUM.Text) > 0 Then strSql += vbCrLf + " AND ITEMID = " & Val(txtItemId_NUM.Text) & ""
            If selectcostid <> "" Then strSql += vbCrLf + " AND COSTID='" & selectcostid & "'"
            If ChkWithApproval.Checked = False And ChkOnlyApproval.Checked = False Then strSql += vbCrLf + " AND APPROVAL<>'A'"
            If ChkOnlyApproval.Checked And ChkWithApproval.Checked = False Then strSql += vbCrLf + " AND APPROVAL='A'"
        Else
            strSql = " SELECT RECDATE"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
            strSql += vbCrLf + " ,TAGNO,PCS,GRSWT,NETWT,SALVALUE"
            strSql += vbCrLf + " ,(SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = T.LOTSNO)AS LOTNO"
            strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)aS COUNTER"
            strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)aS DESIGNER,SNO"
            strSql += vbCrLf + " ,ISNULL((SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE TAGSNO=T.SNO),0) HM"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE "
            If dtpFrom.Enabled Then
                strSql += " RECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += " 1 = 1"
            End If
            If txtLotNo_NUM.Text <> "" Then strSql += vbCrLf + " AND LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
            If txtTagNo.Text <> "" Then strSql += vbCrLf + " AND TAGNO = '" & txtTagNo.Text & "'"
            strSql += " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & cmbOldCompany.Text & "')"
            If cmbNewCounter.Text <> "ALL" And cmbNewCounter.Text <> "" Then
                strSql += " AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbNewCounter.Text & "')"
            End If
            '071213
            'If Val(txtEstNo.Text) <> 0 Then
            '    strSql += vbCrLf + " AND TAGNO IN (SELECT TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"
            'End If
            If Val(txtEstNo.Text) <> 0 Then
                strSql += vbCrLf + " AND TAGNO IN (SELECT TAGNO FROM " & cnStockDb & "..ISSUE WHERE trantype='AI' and TRANNO=" & Val(txtEstNo.Text) & ")"
            End If
            If Val(txtItemId_NUM.Text) > 0 Then strSql += vbCrLf + " AND ITEMID = " & Val(txtItemId_NUM.Text) & ""
            strSql += vbCrLf + " AND ISSDATE IS NULL"
            If selectcostid <> "" Then strSql += vbCrLf + " AND COSTID='" & selectcostid & "'"
            If Val(txtEstNo.Text) = 0 Then
                If ChkWithApproval.Checked = False And ChkOnlyApproval.Checked = False Then strSql += vbCrLf + " AND APPROVAL<>'A'"
            End If
            If ChkOnlyApproval.Checked And ChkWithApproval.Checked = False Then strSql += vbCrLf + " AND APPROVAL='A'"
            If Val(txtEstNo.Text) <> 0 Then
                strSql += vbCrLf + " AND APPROVAL='A'"
            End If

        End If
            dtGrid = New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = chkSelectAll.Checked
        dtGrid.Columns.Add(dtCol)
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        For Each drr As DataRow In dtGrid.Rows
            If Val(drr!HM) <> 0 Then
                drr!CHECK = False
            End If
        Next

        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        gridView_OWN.DataSource = dtGrid
        FormatGridColumns(gridView_OWN, False)
        gridView_OWN.Columns("CHECK").ReadOnly = False
        gridView_OWN.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect

        With gridView_OWN
            .Columns("CHECK").HeaderText = ""
            .Columns("CHECK").Width = 30
            .Columns("RECDATE").Width = 80
            .Columns("ITEM").Width = 150
            .Columns("SUBITEM").Width = 120
            If Not IsNonTag Then .Columns("TAGNO").Width = 70
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("SALVALUE").Width = 100
            .Columns("LOTNO").Visible = False
            .Columns("DESIGNER").Visible = False
            .Columns("COUNTER").Visible = False
            .Columns("SNO").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("HM").Visible = False
        End With
        Dim dtGridHeader As New DataTable
        dtGridHeader = dtGrid.Clone
        dtGridHeader.Columns("CHECK").DataType = GetType(String)
        dtGridHeader.Rows.Add()
        dtGridHeader.Rows.Add()
        dtGridHeader.Rows(0).Item("CHECK") = ""
        dtGridHeader.Rows(1).Item("CHECK") = ""
        dtGridHeader.Rows(0).Item("ITEM") = "TOTAL"
        dtGridHeader.Rows(1).Item("ITEM") = "SELECTED"
        dtGridHeader.Rows(0).Item("SUBITEM") = "TAG COUNT :"
        ',PCS,GRSWT,NETWT,SALVALUE
        Dim obj As Object
        obj = dtGrid.Compute("SUM(PCS)", "PCS IS NOT NULL AND CHECK=TRUE")
        dtGridHeader.Rows(0).Item("PCS") = IIf(Val(obj.ToString) <> 0, obj.ToString, DBNull.Value)
        If chkSelectAll.Checked Then dtGridHeader.Rows(1).Item("PCS") = IIf(Val(obj.ToString) <> 0, obj.ToString, DBNull.Value)
        obj = dtGrid.Compute("SUM(GRSWT)", "GRSWT IS NOT NULL AND CHECK=TRUE")
        dtGridHeader.Rows(0).Item("GRSWT") = IIf(Val(obj.ToString) <> 0, Format(obj.ToString, "0.000"), DBNull.Value)
        If chkSelectAll.Checked Then dtGridHeader.Rows(1).Item("GRSWT") = IIf(Val(obj.ToString) <> 0, Format(obj.ToString, "0.000"), DBNull.Value)
        obj = dtGrid.Compute("SUM(NETWT)", "NETWT IS NOT NULL AND CHECK=TRUE")
        dtGridHeader.Rows(0).Item("NETWT") = IIf(Val(obj.ToString) <> 0, Format(obj.ToString, "0.000"), DBNull.Value)
        If chkSelectAll.Checked Then dtGridHeader.Rows(1).Item("NETWT") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
        obj = dtGrid.Compute("SUM(SALVALUE)", "SALVALUE IS NOT NULL AND CHECK=TRUE")
        dtGridHeader.Rows(0).Item("SALVALUE") = IIf(Val(obj.ToString) <> 0, Format(obj.ToString, "0.000"), DBNull.Value)
        If chkSelectAll.Checked Then dtGridHeader.Rows(1).Item("SALVALUE") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
        obj = dtGrid.Compute("COUNT(PCS)", "PCS IS NOT NULL AND CHECK=TRUE")
        If Not IsNonTag Then
            dtGridHeader.Rows(0).Item("TAGNO") = IIf(Val(obj.ToString) <> 0, obj.ToString, DBNull.Value)
            If chkSelectAll.Checked Then
                dtGridHeader.Rows(1).Item("TAGNO") = IIf(Val(obj.ToString) <> 0, obj.ToString, DBNull.Value)
                If Val(obj.ToString) > 0 Then btnTransfer.Enabled = True
            End If
        End If

        gridViewFooter.DataSource = dtGridHeader
        FormatGridColumns(gridViewFooter, False)
        gridViewFooter.DefaultCellStyle.BackColor = lblRowDet1.BackColor
        gridViewFooter.DefaultCellStyle.SelectionBackColor = lblRowDet1.BackColor
        gridViewFooter.DefaultCellStyle.SelectionForeColor = Color.Red
        gridViewFooter.DefaultCellStyle.ForeColor = Color.Red
        gridViewFooter.DefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        For cnt As Integer = 0 To gridView_OWN.Columns.Count - 1
            gridViewFooter.Columns(cnt).Visible = gridView_OWN.Columns(cnt).Visible
            gridViewFooter.Columns(cnt).Width = gridView_OWN.Columns(cnt).Width
        Next
        gridViewFooter.Columns("SUBITEM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView_OWN.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        funcAddCostCentre()
        lblRowDet1.Text = ""
        lblRowDet1.Visible = False
        gridView_OWN.DataSource = Nothing
        gridViewFooter.DataSource = Nothing
        btnTransfer.Enabled = False
        chkSelectAll.Checked = False
        chkDate.Checked = True
        Me.Refresh()
        chkDate.Focus()
    End Sub

    Function funcAddCostCentre() As Integer
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = "SELECT DISTINCT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            cmbCostCentre.Items.Clear()
            objGPack.FillCombo(strSql, cmbCostCentre, False, False)
            cmbCostCentre.Text = cnCostName
        Else
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Enabled = False
        End If
    End Function
    Private Sub CalcSelectedValues()
        Dim dt As New DataTable
        dt = CType(gridView_OWN.DataSource, DataTable)
        dt.AcceptChanges()
        Dim pcs As Integer = Val(dt.Compute("COUNT(CHECK)", "CHECK = TRUE").ToString)
        If pcs > 0 Then
            btnTransfer.Enabled = True
        Else
            btnTransfer.Enabled = False
        End If
    End Sub

    Private Sub gridView_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.CellValueChanged
        'CalcSelectedValues()
        If Not e.RowIndex > -1 Then Exit Sub

        If Val(gridView_OWN.Rows(e.RowIndex).Cells("HM").Value.ToString) <> 0 And gridView_OWN.Rows(e.RowIndex).Cells("CHECK").Value = True Then
            MsgBox("Cannot Change Tag With HallmarkDetails", MsgBoxStyle.Information)
            gridView_OWN.Rows(e.RowIndex).Cells("CHECK").Value = "False"
            Exit Sub
        ElseIf Val(gridView_OWN.Rows(e.RowIndex).Cells("HM").Value.ToString) <> 0 Then
            Exit Sub
        End If

        gridView_OWN.CurrentCell = gridView_OWN.Rows(e.RowIndex).Cells("CHECK")

        ''CalcSelected Values
        With gridView_OWN.Rows(e.RowIndex)
            Dim pcs As Integer = Val(gridViewFooter.Rows(1).Cells("PCS").Value.ToString)
            Dim grsWt As Decimal = Val(gridViewFooter.Rows(1).Cells("GRSWT").Value.ToString)
            Dim netWT As Decimal = Val(gridViewFooter.Rows(1).Cells("NETWT").Value.ToString)
            Dim salValue As Decimal = Val(gridViewFooter.Rows(1).Cells("SALVALUE").Value.ToString)
            Dim cnt As Decimal = Val(gridViewFooter.Rows(1).Cells("TAGNO").Value.ToString)
            If CType(gridView_OWN.Rows(e.RowIndex).Cells("CHECK").Value, Boolean) = True Then
                pcs += Val(.Cells("PCS").Value.ToString)
                grsWt += Val(.Cells("GRSWT").Value.ToString)
                netWT += Val(.Cells("NETWT").Value.ToString)
                salValue += Val(.Cells("SALVALUE").Value.ToString)
                cnt += 1
            Else
                pcs -= Val(.Cells("PCS").Value.ToString)
                grsWt -= Val(.Cells("GRSWT").Value.ToString)
                netWT -= Val(.Cells("NETWT").Value.ToString)
                salValue -= Val(.Cells("SALVALUE").Value.ToString)
                cnt -= 1
            End If
            If cnt > 0 Then
                btnTransfer.Enabled = True
            Else
                btnTransfer.Enabled = False
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


    Private Sub gridView_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.CurrentCellDirtyStateChanged
        gridView_OWN.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.GotFocus
        lblRowDet1.Visible = True
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.LostFocus
        lblRowDet1.Visible = False
    End Sub

    Private Sub gridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.RowEnter
        With gridView_OWN.Rows(e.RowIndex)
            lblRowDet1.Text = "Lot No : " + .Cells("LOTNO").Value.ToString + "      "
            lblRowDet1.Text += "Designer : " + .Cells("DESIGNER").Value.ToString + "     "
            lblRowDet1.Text += "Counter : " + .Cells("COUNTER").Value.ToString
        End With
    End Sub

    Private Sub frmCounterChange_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY DISPLAYORDER,ITEMCTRNAME"
        objGPack.FillCombo(strSql, cmbNewCounter)
        strSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY ORDER BY COMPANYNAME"
        objGPack.FillCombo(strSql, cmbCompany_MAN)
        strSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY ORDER BY COMPANYNAME"
        objGPack.FillCombo(strSql, cmbOldCompany)
        cmbOldCompany.Text = strCompanyName
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        For Each dgvRow As DataGridViewRow In gridView_OWN.Rows
            If Val(dgvRow.Cells("HM").Value.ToString) <> 0 Then
                dgvRow.Cells("CHECK").Value = "False"
                Exit Sub
            End If
            dgvRow.Cells("CHECK").Value = chkSelectAll.Checked
        Next
    End Sub

    Private Sub btnTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        If objGPack.Validator_Check(Me) Then Exit Sub
        If MessageBox.Show("Do you want to transfer the company for selected items.?", "Company change alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Dim dt As New DataTable
            dt = CType(gridView_OWN.DataSource, DataTable).Copy
            Dim dv As DataView
            dv = dt.DefaultView
            dv.RowFilter = "CHECK = TRUE"
            Dim costId As String = GetAdmindbSoftValue("SYNC-TO")
            Dim toCompany As String = objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & cmbCompany_MAN.Text & "'")
            If toCompany = "" Then
                MsgBox("Company Should not Empty", MsgBoxStyle.Information)
                cmbCompany_MAN.Select()
                Exit Sub
            End If
            Dim StrSno As String = ""
            Try
                tran = Nothing
                tran = cn.BeginTransaction
                For Each ro As DataRow In dv.ToTable.Rows
                    StrSno += "'" & ro.Item("SNO").ToString & "',"
                Next
                If StrSno.EndsWith(",") Then
                    StrSno = Mid(StrSno, 1, StrSno.Length - 1)
                End If
                If StrSno <> "" Then
                    strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET COMPANYID = '" & toCompany & "',recdate ='" & Today.ToString("yyyy-MM-dd") & "' WHERE SNO In (" & StrSno & ")"
                    strSql += " UPDATE " & cnAdminDb & "..ITEMTAGSTONE Set COMPANYID = '" & toCompany & "',recdate ='" & Today.ToString("yyyy-MM-dd") & "' WHERE TAGSNO IN (" & StrSno & ")"
                    strSql += " UPDATE " & cnAdminDb & "..ITEMTAGMISCCHAR SET COMPANYID = '" & toCompany & "' WHERE TAGSNO IN (" & StrSno & ")"
                    strSql += " UPDATE " & cnAdminDb & "..ITEMTAGMETAL SET COMPANYID = '" & toCompany & "',recdate ='" & Today.ToString("yyyy-MM-dd") & "' WHERE TAGSNO IN (" & StrSno & ")"
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, costId)
                    strSql = " UPDATE " & cnAdminDb & "..PURITEMTAG SET COMPANYID = '" & toCompany & "',recdate ='" & Today.ToString("yyyy-MM-dd") & "' WHERE TAGSNO IN (" & StrSno & ")"
                    strSql += " UPDATE " & cnAdminDb & "..PURITEMTAGMISCCHAR SET COMPANYID = '" & toCompany & "' WHERE TAGSNO IN (" & StrSno & ")"
                    strSql += " UPDATE " & cnAdminDb & "..PURITEMTAGMETAL SET COMPANYID = '" & toCompany & "' WHERE TAGSNO IN (" & StrSno & ")"
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, costId)
                End If
                tran.Commit()
                tran = Nothing
                MsgBox("Successfully Transfered..")
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

    Private Sub txtFindTag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFindTag.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If dtGrid Is Nothing Then Exit Sub
            If Not gridView_OWN.Rows.Count > 0 Then Exit Sub
            If txtFindTag.Text = "" Then Exit Sub
            Dim Row() As DataRow = dtGrid.Select("TAGNO = '" & txtFindTag.Text & "'")
            If Row.Length > 1 Then
                If Val(txtItemId_NUM.Text) = 0 Then
                    MsgBox("This tag no exist in multiple items" + vbCrLf + "Please give itemid id in filteration", MsgBoxStyle.Information)
                    txtItemId_NUM.Select()
                    Exit Sub
                End If
            End If
            If Row.Length > 0 Then
                gridView_OWN.Rows(Val(Row(0).Item("KEYNO".ToString))).Cells("CHECK").Value = True
                txtFindTag.Clear()
            End If
        End If
    End Sub

    Private Sub ChkOnlyApproval_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkOnlyApproval.CheckedChanged
        If ChkOnlyApproval.Checked Then ChkWithApproval.Checked = False
    End Sub

    Private Sub ChkWithApproval_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkWithApproval.CheckedChanged
        If ChkWithApproval.Checked Then ChkOnlyApproval.Checked = False
    End Sub
End Class