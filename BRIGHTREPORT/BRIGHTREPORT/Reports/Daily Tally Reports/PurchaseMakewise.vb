Imports System.Data.OleDb
Public Class frmPurchaseMakeWise
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim dtGridView As New DataTable
    Public dtFilteration As New DataTable
    Dim cMenu As New ContextMenuStrip
    Dim lstColumn As New List(Of String)
    Dim sumColumn As New List(Of String)
    Dim sortColumn As String
    Dim dtMetal As DataTable
    Dim Melting As Boolean = IIf(GetAdmindbSoftValue("PURMELTINGGET", "N") = "Y", True, False)

    'calno 01113 sathya
    'CALNO : 1152  ALTERBY MURUGAN.C ( ADD NEW COLUMNS - MELTWT AND MELT PERCENTAGE)
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Me.lblTitle.Text = String.Empty
        gridView.DataSource = Nothing
        '01113
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY where ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        Dim dtcom As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcom)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbcompany, dtcom, "COMPANYNAME", , "ALL")

        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,COSTID,2 RESULT FROM " & cnAdminDb & "..COSTCENTRE "
        strSql += " ORDER BY RESULT,COSTNAME"
        Dim dtcost As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcost)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbcostcenter, dtcost, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkcmbcostcenter.Enabled = False
        '01113
        strSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
        strSql += " ORDER BY RESULT,METALNAME"
        dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")

        strSql = " SELECT 'ALL' NAME,0 ITEMTYPEID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT NAME,ITEMTYPEID,2 RESULT FROM " & cnAdminDb & "..ITEMTYPE "
        strSql += " ORDER BY RESULT,NAME"
        Dim dtItemType As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItemType)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemType, dtItemType, "NAME", , "ALL")

        gridView.ContextMenuStrip = Nothing
        dtpFrom.Select()
    End Sub

    Private Sub dgv_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles gridView.ColumnHeaderMouseClick
        Select Case gridView.Columns(e.ColumnIndex).ValueType.FullName
            Case GetType(Int16).FullName, GetType(Int32).FullName, GetType(Int64).FullName, GetType(Integer).FullName, GetType(Decimal).FullName, GetType(Double).FullName
                If sumColumn.Contains(gridView.Columns(e.ColumnIndex).Name) Then
                    sumColumn.Remove(gridView.Columns(e.ColumnIndex).Name)
                Else
                    sumColumn.Add(gridView.Columns(e.ColumnIndex).Name)
                End If
                dgvTStripMenu(Me, New EventArgs)
        End Select
    End Sub

    Private Sub dgvTStripMenu(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim cnt As Integer = 0
        For Each mm As ToolStripMenuItem In cMenu.Items
            If mm.CheckState = CheckState.Checked Then
                cnt += 1
            End If
        Next
        If cnt > 2 Then
            For Each mm As ToolStripMenuItem In cMenu.Items
                mm.CheckState = CheckState.Unchecked
            Next
            CType(sender, ToolStripMenuItem).CheckState = CheckState.Checked
        End If
        Dim lstColumn As New List(Of String)
        For Each mm As ToolStripMenuItem In cMenu.Items
            If mm.CheckState = CheckState.Checked Then
                lstColumn.Add(mm.Text)
            End If
        Next
        gridView.DataSource = Nothing
        Dim dt As New DataTable
        dt = dtGridView.Copy
        If cnt = 0 Then
            gridView.DataSource = dt
            BrighttechPack.GridViewGrouper.StyleGridColumns(gridView, Nothing)
        Else
            gridView.DataSource = dt
            BrighttechPack.GridViewGrouper.GridView_Grouper(gridView, Nothing, lstColumn, sumColumn, sortColumn)
        End If
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        btnView_Search.Enabled = False
        gridView.DataSource = Nothing
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Try
            lstColumn.Clear()
            sumColumn.Clear()
            sortColumn = ""
            dtGridView.Rows.Clear()
            Dim trantypes As String = ""
            If rbtPurch.Checked Then trantypes = "'PU'"
            If rbtSaleReturn.Checked Then trantypes = "'SR'"
            If rbtBoth.Checked Then trantypes = "'PU','SR'"
            strSql = " SELECT "
            strSql += vbCrLf + " CASE WHEN FLAG = 'W' THEN 'OWN' ELSE 'OTHER' END AS OWN"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE))AS METAL"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(I.ITEMID,0)<>0 THEN (IM.ITEMNAME)  + CASE WHEN ISNULL(I.SUBITEMID,0)<>0 THEN ' ('+(SM.SUBITEMNAME)+')' ELSE '' END ELSE (CA.CATNAME) END AS DESCRIP"
            strSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =I.ITEMTYPEID) AS ITEMTYPE"
            'CALL NO : 1152 
            strSql += vbCrLf + " ,TRANNO,PURITY,PCS,I.GRSWT,LESSWT,DUSTWT, CONVERT(DECIMAL(15,3),(ISNULL(I.GRSWT,0)-(ISNULL(LESSWT,0)+ISNULL(DUSTWT,0))))AS TOTALNETWT "
            strSql += vbCrLf + " ,CONVERT(DECIMAL(10,2), CASE WHEN (I.GRSWT-ISNULL(I.DUSTWT,0)-ISNULL(LESSWT,0)) <> 0 THEN (MELTWT/(I.GRSWT-ISNULL(I.DUSTWT,0)-ISNULL(LESSWT,0)))*100 ELSE 0 END)AS MELTPER"
            strSql += vbCrLf + " ,MELTWT,WASTPER,WASTAGE,NETWT,RATE,AMOUNT,BOARDRATE"
            strSql += vbCrLf + " ,TRANDATE,BATCHNO,TRANDATE TTRANDATE,I.COSTID"
            strSql += vbCrLf + " ,ISNULL(E.EMPNAME,'') EMPNAME"
            strSql += vbCrLf + " ,(SELECT TOP 1 REMARK1 FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO=I.BATCHNO)REMARK"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS I"
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..EMPMASTER E ON I.EMPID=E.EMPID "
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.SUBITEMID = I.SUBITEMID "
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE=I.CATCODE "
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND I.TRANTYPE IN (" & trantypes & ")"
            '01113
            If rbtmother.Checked Then strSql += vbCrLf + " AND FLAG IN('O') "
            If rbtmown.Checked Then strSql += vbCrLf + " AND FLAG IN('W') "
            '   If rbtmboth.Checked Then strSql += vbCrLf + " AND FLAG IN('W','O') "
            strSql += " AND ISNULL(CANCEL,'') = ''"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
                strSql += vbCrLf + " AND I.METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
            End If
            If chkCmbItemType.Text <> "ALL" And chkCmbItemType.Text <> "" Then
                strSql += vbCrLf + " AND ITEMTYPEID IN (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME IN (" & GetQryString(chkCmbItemType.Text) & "))"
            End If
            If chkcmbcompany.Text <> "ALL" And chkcmbcompany.Text <> "" Then
                strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkcmbcompany.Text) & "))"
            End If
            If chkcmbcostcenter.Text <> "ALL" And chkcmbcostcenter.Text <> "" Then
                strSql += vbCrLf + " AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkcmbcostcenter.Text) & "))"
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CASE WHEN R.FLAG = 'W' THEN 'OWN' ELSE 'OTHER' END AS OWN"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE))AS METAL"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(I.STNITEMID,0)<>0 THEN (IM.ITEMNAME)  + CASE WHEN ISNULL(I.STNSUBITEMID,0)<>0 THEN ' ('+(SM.SUBITEMNAME)+')' ELSE '' END ELSE (CA.CATNAME) END AS DESCRIP"
            strSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =R.ITEMTYPEID) AS ITEMTYPE"
            'CALL NO : 1152 (0 MELTET,0 MELTPER)
            strSql += vbCrLf + " ,R.TRANNO,PURITY,PCS,STNWT GRSWT,0 LESSWT,0 DUSTWT,CONVERT(DECIMAL(10,3),STNWT) TOTALNETWT,0 MELTET,0 MELTPER,WASTPER,WASTAGE,STNWT NETWT,STNRATE,I.STNAMT,BOARDRATE"
            strSql += vbCrLf + " ,R.TRANDATE,R.BATCHNO,R.TRANDATE TTRANDATE,I.COSTID"
            strSql += vbCrLf + " ,ISNULL(E.EMPNAME,'') EMPNAME"
            strSql += vbCrLf + " ,(SELECT TOP 1 REMARK1 FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO=I.BATCHNO)REMARK"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE  AS I"
            strSql += vbCrLf + " LEFT JOIN  " & cnStockDb & "..RECEIPT R ON I.ISSSNO =R.SNO "
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..EMPMASTER E ON R.EMPID=E.EMPID "
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.STNITEMID "
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.SUBITEMID = I.STNSUBITEMID "
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE=I.CATCODE "
            strSql += vbCrLf + " WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND I.TRANTYPE IN (" & trantypes & ") AND ISNULL(I.STNAMT,0) <> 0"
            '01113
            If rbtmother.Checked Then strSql += vbCrLf + " AND R.FLAG IN('O') "
            If rbtmown.Checked Then strSql += vbCrLf + " AND R.FLAG IN('W') "
            strSql += " AND ISNULL(R.CANCEL,'') = ''"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
                strSql += vbCrLf + " AND R.METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
            End If
            If chkCmbItemType.Text <> "ALL" And chkCmbItemType.Text <> "" Then
                strSql += vbCrLf + " AND ITEMTYPEID IN (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME IN (" & GetQryString(chkCmbItemType.Text) & "))"
            End If
            If chkcmbcompany.Text <> "ALL" And chkcmbcompany.Text <> "" Then
                strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkcmbcompany.Text) & "))"
            End If
            If chkcmbcostcenter.Text <> "ALL" And chkcmbcostcenter.Text <> "" Then
                strSql += vbCrLf + " AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkcmbcostcenter.Text) & "))"
            End If
            '01113
            dtGridView = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridView)
            If Not dtGridView.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                btnView_Search.Enabled = True
                dtpFrom.Focus()
                Exit Sub
            End If
            gridView.ContextMenuStrip = Nothing
            gridView.DataSource = dtGridView

            lstColumn.Add("OWN")
            lstColumn.Add("METAL")
            sumColumn.Add("PCS")
            sumColumn.Add("GRSWT")
            sumColumn.Add("WASTAGE")
            sumColumn.Add("DUSTWT")
            sumColumn.Add("LESSWT")
            sumColumn.Add("NETWT")
            sumColumn.Add("AMOUNT")
            sumColumn.Add("MELTWT")

            If rbtnRate.Checked Then sortColumn = "RATE ASC"
            If rbtnTranno.Checked Then sortColumn = "TRANNO ASC"
            If rbtnWeight.Checked Then sortColumn = "GRSWT ASC"

            cMenu.Items.Clear()
            For Each col As DataGridViewColumn In gridView.Columns
                If col.Name = "PARCOL" And col.Name = "RATE" Then Continue For
                col.SortMode = DataGridViewColumnSortMode.NotSortable
                Dim menu As New ToolStripMenuItem
                menu.Name = col.Name
                menu.Text = col.HeaderText
                menu.CheckOnClick = True
                If lstColumn.Contains(col.HeaderText) Then menu.Checked = True
                AddHandler menu.Click, AddressOf dgvTStripMenu
                cMenu.Items.Add(menu)
            Next
            dgvTStripMenu(Me, New EventArgs)
            funcTitle()
            gridView.Columns("BATCHNO").Visible = False
            gridView.Columns("TTRANDATE").Visible = False
            gridView.Columns("COSTID").Visible = False
            gridView.Columns("MELTPER").HeaderText = "MELT%"
            gridView.Columns("LESSWT").HeaderText = "STONE/LESS WT"
            gridView.Columns("WASTPER").HeaderText = "WAST%"
            gridView.Columns("TOTALNETWT").HeaderText = "NET.WT"
            If Melting = True Then
                gridView.Columns("MELTWT").Visible = True
                gridView.Columns("MELTPER").Visible = True
            Else
                gridView.Columns("MELTWT").Visible = False
                gridView.Columns("MELTPER").Visible = False
            End If
            gridView.ContextMenuStrip = cMenu
            btnView_Search.Enabled = True

            gridView.Focus()
            gridView.ContextMenuStrip = cmbGridShortCut
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            btnView_Search.Enabled = True
        End Try
    End Sub

    Public Sub SetFilterStr(ByVal filterColumnName As String, ByVal setValue As Object)
        If Not dtFilteration.Columns.Contains(filterColumnName) Then Exit Sub
        If Not dtFilteration.Rows.Count > 0 Then Exit Sub
        dtFilteration.Rows(0).Item(filterColumnName) = setValue
    End Sub

    Public Function GetFilterStr(ByVal filterColumnName As String) As String
        Dim ftrStr As String = Nothing
        If Not dtFilteration.Columns.Contains(filterColumnName) Then Return ftrStr
        If Not dtFilteration.Rows.Count > 0 Then Return ftrStr
        Return dtFilteration.Rows(0).Item(filterColumnName).ToString
        Return ftrStr
    End Function
    

    Private Sub NEWToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NEWToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub EXITToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EXITToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub frmPurchaseMakeWise_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmPurchaseMakeWise_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Function funcTitle() As Integer
        lblTitle.Text = ""
        lblTitle.Text += "PURCHASE DETAILS"
        lblTitle.Text += " FROM " + dtpFrom.Value.ToString("dd/MM/yyyy") + " TO " + dtpTo.Value.ToString("dd/MM/yyyy")
        lblTitle.Refresh()
    End Function

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
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
End Class