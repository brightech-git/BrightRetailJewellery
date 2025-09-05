Imports System.Data.OleDb
Imports System.Globalization
Imports System.Math

Public Class frmMiscChargeUpdate
    Dim strSql As String
    Dim dt As New DataTable
    Dim cmd As OleDbCommand
    Dim miscId As Integer

    Private Sub frmMiscChargeUpdate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub frmMiscChargeUpdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If ChkCmbCostcentre.Focused Then
                If cmbType.Text = "ITEM" Then ChkCmbItem.Focus() : Exit Sub
                If cmbType.Text = "DESIGNER" Then ChkCmbDesigner.Focus() : Exit Sub
                If cmbType.Text = "TABLE" Then CmbTable.Focus() : Exit Sub
                If cmbType.Text = "TAGTYPE" Then ChkCmbTagType.Focus() : Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmMiscChargeUpdate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        grpContrainer.Location = New Point((ScreenWid - grpContrainer.Width) / 2, ((ScreenHit - 128) - grpContrainer.Height) / 2)
        pnlContainer_OWN.Location = New Point((ScreenWid - pnlContainer_OWN.Width) / 2, ((ScreenHit - 128) - pnlContainer_OWN.Height) / 2)
        tabMode.ItemSize = New System.Drawing.Size(1, 1)
        tabMode.SendToBack()
        Me.tabMode.Region = New Region(New RectangleF(Me.tabitem.Left, Me.tabitem.Top, Me.tabitem.Width, Me.tabitem.Height))
        cmbType.Items.Clear()
        cmbType.Items.Add("ITEM")
        cmbType.Items.Add("DESIGNER")
        cmbType.Items.Add("TABLE")
        cmbType.Items.Add("TAGTYPE")
        strSql = "SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE ACTIVE='Y'"
        objGPack.FillCombo(strSql, CmbChargeType, True, True)
        strSql = " SELECT 'ALL' COSTNAME UNION ALL SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
        Dim dtcost As New DataTable
        da = New OleDbDataAdapter
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcost)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbCostcentre, dtcost, "COSTNAME", , "ALL")
    End Sub

    Private Sub cmbMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub ChkCmbTagLoad()
        strSql = "SELECT DISTINCT IT.NAME ,IT.ITEMTYPEID "
        strSql += " FROM " & cnAdminDb & "..ITEMTAG T"
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST M ON M.ITEMID =T.ITEMID "
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMTYPE IT ON IT.ITEMTYPEID =T.ITEMTYPEID"
        strSql += " AND M.OTHCHARGE ='Y' AND M.ACTIVE='Y'"
        objGPack.FillCombo(strSql, ChkCmbTagType, True, True)
    End Sub
    Private Sub ChkCmbItemLoad()
        strSql = " SELECT ITEMNAME,ITEMID FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ACTIVE = 'Y' AND OTHCHARGE='Y' ORDER BY ITEMID"
        Dim dtItem As New DataTable
        da = New OleDbDataAdapter
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbItem, dtItem, "ITEMNAME", , "ALL")
    End Sub
    Private Sub ChkCmbSubitemLoad()
        Dim SubItemName As String
        SubItemName = GetChecked_CheckedList(ChkCmbItem)
        ChkCmbSubItem.Items.Clear()
        If SubItemName <> "" Then
            strSql = " SELECT SUBITEMNAME,SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ACTIVE = 'Y'"
            strSql += vbCrLf + " AND OTHCHARGE='Y' AND ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN(" & SubItemName & "))"
            Dim dt As New DataTable
            da = New OleDbDataAdapter
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            BrighttechPack.GlobalMethods.FillCombo(ChkCmbSubItem, dt, "SUBITEMNAME", , "ALL")
        End If
    End Sub
    Private Sub chkcmbDesignerLoad()
        strSql = " SELECT DISTINCT D.DESIGNERID,D.DESIGNERNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMTAG T"
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST M ON M.ITEMID =T.ITEMID "
        strSql += " LEFT JOIN " & cnAdminDb & "..DESIGNER D ON D.DESIGNERID=T.DESIGNERID"
        strSql += " AND M.OTHCHARGE ='Y' AND M.ACTIVE='Y'"
        Dim dtDesign As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDesign)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbDesigner, dtDesign, "DESIGNERNAME", , "ALL")
    End Sub
    Private Sub chkcmbTableLoad()
        strSql = " SELECT DISTINCT TABLECODE FROM " & cnAdminDb & "..WMCTABLE  "
        strSql += " WHERE ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ACTIVE='Y' AND OTHCHARGE='Y') ORDER BY TABLECODE"
        objGPack.FillCombo(strSql, CmbTable)
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        strSql = "SELECT MISCID FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME='" & CmbChargeType.Text & "'"
        miscId = Val(objGPack.GetSqlValue(strSql, "MISCID", , tran))

        If UCase(cmbType.Text) = "ITEM" Then
            GetOtherCharge()
        ElseIf UCase(cmbType.Text) = "DESIGNER" Then
            GetOtherCharge()
        ElseIf UCase(cmbType.Text) = "TAGTYPE" Then
            GetOtherCharge()
        Else
            GetOtherCharge()
        End If
        If Not gridView.RowCount > 0 Then Exit Sub
        tabMain.SelectedTab = tabView
        gridView.Select()
    End Sub
    Private Sub GetOtherCharge()
        Dim SubItemName, ItemName, Costcentre As String
        Dim Ids As String
        Ids = GetSelectedCostId(ChkCmbCostcentre, True)
        strSql = "SELECT SNO,COSTID,ITEMID,ITEMNAME,TAGNO,SALVALUE,SUM(CHARGE)OLDCHARGE," & Val(txtAmount.Text) & " AS NEWCHARGE FROM("
        strSql += vbCrLf + " SELECT IT.SNO,IT.COSTID,IT.TAGNO,IT.ITEMID,IM.ITEMNAME,IT.SALVALUE,ISNULL(IC.AMOUNT,0) AS CHARGE "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG IT"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..ITEMTAGMISCCHAR IC ON IC.TAGSNO=IT.SNO AND IC.MISCID=" & miscId
        strSql += vbCrLf + " INNER JOIN  " & cnAdminDb & "..ITEMMAST  IM ON IM.ITEMID=IT.ITEMID AND IM.OTHCHARGE='Y' WHERE IT.ISSDATE IS NULL"
        SubItemName = GetChecked_CheckedList(ChkCmbSubItem)
        If UCase(cmbType.Text) = "ITEM" Then
            ItemName = GetChecked_CheckedList(ChkCmbItem)
            If ItemName <> "" Then strSql += vbCrLf + " AND IT.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & ItemName & "))"
        ElseIf UCase(cmbType.Text) = "DESIGNER" Then
            Dim ftrDesigner As String = GetChecked_CheckedList(ChkCmbDesigner)
            If ftrDesigner <> "" Then
                strSql += vbCrLf + " AND  IT.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN  (" & ftrDesigner & ")) "
                strSql += vbCrLf + " AND IT.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE OTHCHARGE='Y' AND ACTIVE='Y')"
            End If
        ElseIf UCase(cmbType.Text) = "TABLE" Then
            strSql += " AND IT.TABLECODE = '" & CmbTable.Text & "'"
            strSql += vbCrLf + " AND IT.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE OTHCHARGE='Y' AND ACTIVE='Y')"

        ElseIf UCase(cmbType.Text) = "TAGTYPE" Then
            strSql += " AND IT.ITEMTYPEID = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME='" & ChkCmbTagType.Text & "')"
            strSql += vbCrLf + " AND IT.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE OTHCHARGE='Y' AND ACTIVE='Y')"
        End If
        If SubItemName <> "" Then
            strSql += vbCrLf + " AND IT.SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & SubItemName & "))"
        End If
        If Not ChkCmbCostcentre.Text = "ALL" Then
            strSql += vbCrLf + " AND IT.COSTID IN (" & Ids & ")"
        End If
        strSql += vbCrLf + " ) X GROUP BY SNO,TAGNO,ITEMID,ITEMNAME,SALVALUE,COSTID"
        da = New OleDbDataAdapter
        dt = New DataTable
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = True
        dt.Columns.Add(dtCol)
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        dt.AcceptChanges()
        If dt.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dt
            FormatGridColumns(gridView, False, False)
            gridView.Columns("CHECK").ReadOnly = False
            gridView.Columns("SNO").Visible = False
            gridView.Columns("COSTID").Visible = False
            gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Else
            MsgBox("No Record Found", , "BrighttechGold")
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        tabMain.SelectedTab = tabGeneral
        ChkCmbItem.Items.Clear()
        ChkCmbSubItem.Items.Clear()
        ChkCmbCostcentre.Items.Clear()
        ChkCmbDesigner.Items.Clear()
        cmbType.Select()
        txtAmount.Clear()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        cmbType.Select()
    End Sub

    Private Sub cmbType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged
        objGPack.TextClear(tabMode)
        Select Case cmbType.Text.ToUpper
            Case "ITEM"
                tabMode.SelectedTab = tabitem
                ChkCmbItemLoad()
                ChkCmbSubitemLoad()
            Case "TAGTYPE"
                tabMode.SelectedTab = tabTagType
                ChkCmbTagLoad()
            Case "DESIGNER"
                tabMode.SelectedTab = tabDesigner
                chkcmbDesignerLoad()
            Case "TABLE"
                tabMode.SelectedTab = tabTable
                chkcmbTableLoad()
        End Select
        cmbType.Select()
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim dr() As DataRow
        dr = CType(gridView.DataSource, DataTable).Select("CHECK = TRUE")
        If Not dr.Length > 0 Then
            MsgBox("There is no selected record", MsgBoxStyle.Information)
            Exit Sub
        End If
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            For Each ro As DataRow In dr
                GenUpdateQry(ro)
            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Updated Successfully", MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Function GenUpdateQry(ByVal ro As DataRow) As String
        Dim qry As String = Nothing
        Dim Value As Decimal = 0
        If Val(ro.Item("OLDCHARGE").ToString) = 0 Then
            Dim miscSno As String = GetNewSno(TranSnoType.ITEMTAGMISCCHARCODE, tran, "GET_ADMINSNO_TRAN")
            qry = " INSERT INTO " & cnAdminDb & "..ITEMTAGMISCCHAR"
            qry += " ("
            qry += " SNO,ITEMID,TAGNO,MISCID,AMOUNT,"
            qry += " TAGSNO,COSTID,SYSTEMID,APPVER,COMPANYID)VALUES("
            qry += " '" & miscSno & "'" 'SNO
            qry += " ," & ro.Item("ITEMID").ToString & "" 'ITEMID
            qry += " ,'" & ro.Item("TAGNO").ToString & "'" 'TAGNO
            qry += " ," & miscId & "" 'MISCID
            qry += " ," & Val(ro.Item("NEWCHARGE").ToString) & "" 'AMOUNT
            qry += " ,'" & ro.Item("SNO").ToString & "'" 'TAGSNO
            qry += " ,'" & cnCostId & "'" 'COSTID
            qry += " ,'" & systemId & "'" 'SYSTEMID
            qry += " ,'" & VERSION & "'" 'APPVER
            qry += " ,'" & GetStockCompId() & "'" 'COMPANYID
            qry += " )"

            ExecQuery(SyncMode.Transaction, qry, cn, tran, ro.Item("COSTID").ToString)

            Value = Val(ro.Item("SALVALUE").ToString) + Val(ro.Item("NEWCHARGE").ToString)
            qry = "UPDATE " & cnAdminDb & "..ITEMTAG SET "
            qry += vbCrLf + " SALVALUE=" & Value & ""
            qry += vbCrLf + " WHERE SNO='" & ro.Item("SNO").ToString & "'"
            qry += vbCrLf + " AND TAGNO='" & ro.Item("TAGNO").ToString & "'"
            ExecQuery(SyncMode.Transaction, qry, cn, tran, ro.Item("COSTID").ToString)
        Else
            qry = "UPDATE " & cnAdminDb & "..ITEMTAGMISCCHAR SET "
            qry += vbCrLf + " AMOUNT=" & Val(ro.Item("NEWCHARGE").ToString) & ""
            qry += vbCrLf + " WHERE TAGSNO='" & ro.Item("SNO").ToString & "'"
            qry += vbCrLf + " AND TAGNO='" & ro.Item("TAGNO").ToString & "'"
            qry += vbCrLf + " AND MISCID=" & miscId
            ExecQuery(SyncMode.Transaction, qry, cn, tran, ro.Item("COSTID").ToString)

            Value = (Val(ro.Item("SALVALUE").ToString) - Val(ro.Item("OLDCHARGE").ToString)) + Val(ro.Item("NEWCHARGE").ToString)

            qry = "UPDATE " & cnAdminDb & "..ITEMTAG SET "
            qry += vbCrLf + " SALVALUE=" & Value & ""
            qry += vbCrLf + " WHERE SNO='" & ro.Item("SNO").ToString & "'"
            qry += vbCrLf + " AND TAGNO='" & ro.Item("TAGNO").ToString & "'"
            ExecQuery(SyncMode.Transaction, qry, cn, tran, ro.Item("COSTID").ToString)
        End If
    End Function

    Private Sub ChkCmbItem_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkCmbItem.Leave
        ChkCmbSubitemLoad()
    End Sub

    Private Sub ChkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkAll_OWN.CheckedChanged

    End Sub

    Private Sub CmbChargeType_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbChargeType.Leave
        strSql = "SELECT MISCID FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME='" & CmbChargeType.Text & "'"
        miscId = Val(objGPack.GetSqlValue(strSql, "MISCID", , tran))
    End Sub

    Private Sub ChkAll_OWN_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkAll_OWN.CheckStateChanged
        Dim dr() As DataRow
        If gridView.DataSource Is Nothing Then Exit Sub
        dr = CType(gridView.DataSource, DataTable).Select
        For Each ro As DataRow In dr
            ro.Item("CHECK") = ChkAll_OWN.Checked
        Next
    End Sub
End Class