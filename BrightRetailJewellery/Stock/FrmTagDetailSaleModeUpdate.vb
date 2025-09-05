Imports System.Data.OleDb
Imports System.Globalization
Imports System.Math

Public Class FrmTagDetailSaleModeUpdate
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim SALVALUEROUND As Decimal = Val(GetAdmindbSoftValue("STKSALVALUEROUND", "0"))

    Private Sub FrmTagDetailSaleModeUpdate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        grpContrainer.Location = New Point((ScreenWid - grpContrainer.Width) / 2, ((ScreenHit - 128) - grpContrainer.Height) / 2)
        pnlContainer_OWN.Location = New Point((ScreenWid - pnlContainer_OWN.Width) / 2, ((ScreenHit - 128) - pnlContainer_OWN.Height) / 2)
       
        cmbItem.Items.Clear()
        cmbItem.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = ''"
        strSql += GetItemQryFilteration("S")
        strSql += " AND ACTIVE = 'Y'"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(DIASTONE,'') <> '' AND ISNULL(STUDDED,'') IN ('L','B')"
        strSql += GetItemQryFilteration("S")
        strSql += " AND ACTIVE = 'Y'"
        objGPack.FillCombo(strSql, cmbItem, False, False)
        cmbItem.Text = "ALL"
        strSql = " SELECT 'ALL' ITEMCTRNAME,0 ITEMCTRID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMCTRNAME,ITEMCTRID,2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER "
        strSql += " ORDER BY RESULT,ITEMCTRNAME"
        Dim dtcounter As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcounter)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbcounter, dtcounter, "ITEMCTRNAME", , "ALL")

        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        For Each ro As DataRow In dt.Rows
            chkLstDesigner.Items.Add(ro(0).ToString)
        Next

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        tabMain.SelectedTab = tabGeneral
        cmbItem.Text = "ALL"
        cmbSubItem_MAN.Text = "ALL"
        CmbOldSaleMode.SelectedIndex = 0
        CmbNewSaleMode.SelectedIndex = 0
        cmbItem.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        GenWeightGrid()
        If Not gridView.RowCount > 0 Then Exit Sub
        tabMain.SelectedTab = tabView
        gridView.Select()
    End Sub
    Private Sub GenStnRateGrid()
       
    End Sub
    Private Sub GenWeightGrid()
        Dim ftrDesigner As String = GetCheckedItemStr(chkLstDesigner)
        Dim ftrcounter As String = GetSelectedCounderid(chkcmbcounter, True)
        strSql = ""
        strSql += " SELECT SNO,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
        strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
        strSql += " ,TAGNO,PCS,GRSWT,LESSWT,NETWT"
        strSql += " ,'" & CmbOldSaleMode.Text & "' AS [OLD SALEMODE]"
        strSql += " ,'" & CmbNewSaleMode.Text & "' AS [NEW SALEMODE]"
        strSql += " ,GRSNET [G/N]"
        strSql += "  FROM " & cnAdminDb & "..ITEMTAG T"
        strSql += " WHERE ISSDATE IS NULL"
        If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
        End If
        If cmbSubItem_MAN.Text <> "ALL" And cmbSubItem_MAN.Text <> "" Then
            strSql += " AND SUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "'"
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'))"
        End If
        If ftrDesigner <> Nothing Then
            strSql += " AND DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & ftrDesigner & "))"
        End If
        strSql += " AND SALEMODE = '" & Mid(CmbOldSaleMode.Text, 1, 1) & "'"
        If chkcmbcounter.Text <> "" And chkcmbcounter.Text <> "ALL" Then
            strSql += " AND T.ITEMCTRID IN (" & ftrcounter & ")"
        End If
        strSql += " ORDER BY ITEM,TAGNO"
        Dim dtGrid As New DataTable
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = True
        dtGrid.Columns.Add(dtCol)

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.AcceptChanges()
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabView.Show()
        gridView.DataSource = dtGrid
        gridView.Columns("CHECK").ReadOnly = False
        FormatGridColumns(gridView, False, False)

        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        With gridView
            .Columns("SNO").Visible = False
            .Columns("OLD SALEMODE").DefaultCellStyle.BackColor = Color.Bisque
            .Columns("NEW SALEMODE").DefaultCellStyle.BackColor = Color.Lavender
        End With
    End Sub
    Private Function GetCheckedItemStr(ByVal chkLst As CheckedListBox) As String
        Dim ret As String = Nothing
        If Not chkLst.Items.Count > 0 Then Return ""
        For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
            ret += "'" & chkLst.CheckedItems.Item(cnt).ToString & "',"
        Next
        If ret <> Nothing Then
            ret = Mid(ret, 1, ret.Length - 1)
        End If
        Return ret
    End Function
    Public Function GetSelectedCounderid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        cmbItem.Select()
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
                strSql = GenUpdateQry(ro)
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
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
        qry += " UPDATE " & cnAdminDb & "..ITEMTAG SET"
        qry += " SALEMODE = '" & Mid(CmbNewSaleMode.Text, 1, 1) & "'"
        qry += " WHERE SNO = '" & ro.Item("SNO").ToString & "'"
        Return qry
    End Function

    Private Sub cmbItem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItem.SelectedIndexChanged
        cmbSubItem_MAN.Items.Clear()
        cmbSubItem_MAN.Items.Add("ALL")
        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            strSql += " ORDER BY SUBITEMNAME"
            objGPack.FillCombo(strSql, cmbSubItem_MAN, False, True)
        End If
        cmbSubItem_MAN.Text = "ALL"
    End Sub

    Private Sub chkDesigner_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDesigner.CheckStateChanged
        CheckList(chkLstDesigner, chkDesigner.Checked)
    End Sub
    Private Sub CheckList(ByVal chkLst As CheckedListBox, ByVal state As Boolean)
        For cnt As Integer = 0 To chkLst.Items.Count - 1
            chkLst.SetItemChecked(cnt, state)
        Next
    End Sub

    Private Sub cmbItem_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubItem_MAN.KeyPress, CmbOldSaleMode.KeyPress, CmbNewSaleMode.KeyPress, cmbItem.KeyPress, chkLstDesigner.KeyPress, chkDesigner.KeyPress, chkcmbcounter.KeyPress, btnSearch.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub FrmTagDetailSaleModeUpdate_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        ElseIf e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F3 Then
            btnExit_Click(Me, New EventArgs)
        End If
    End Sub
End Class