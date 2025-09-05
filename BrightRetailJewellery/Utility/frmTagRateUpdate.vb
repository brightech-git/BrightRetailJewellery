Imports System.Data.oledb
Public Class frmTagRateUpdate

    Dim strSql As String
    Dim dtCostCentre As New DataTable
    Dim dtItem As DataTable
    Dim dtSubItem As DataTable
    Dim tran As OleDbTransaction
    Dim cmd As New OleDbCommand
    Dim da As New OleDbDataAdapter
    
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ''COSTCENTRE
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += " ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
            chkCmbCostCentre.Enabled = True
        Else
            chkCmbCostCentre.Enabled = False
        End If

        ''ITEM
        strSql = " SELECT 'ALL' ITEMNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
    End Sub
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        ' (WEIGHT + WASTAGE) * RATE  
        ' SALMODE = 'W'
        Try
            If MessageBox.Show("Please, take BackUp,If you want to exit?", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                Dim f As New frmAdminPassword
                If f.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
                'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub

                If Val(Main.tStripGoldRate.Text) = 0 And Val(Main.tStripSilverRate.Text) = 0 Then
                    MsgBox("Update Gold Rate and Silver Rate...")
                    Exit Sub
                ElseIf Val(Main.tStripGoldRate.Text) = 0 Then
                    MsgBox("Update Gold Rate...")
                    Exit Sub
                ElseIf Val(Main.tStripSilverRate.Text) = 0 Then
                    MsgBox("Update Silver Rate...")
                    Exit Sub
                End If

                strSql = " IF OBJECT_ID('MASTER..TEMPRATEMASTUPDATE') IS NOT NULL DROP TABLE TEMPRATEMASTUPDATE"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                strSql = " CREATE TABLE TEMPRATEMASTUPDATE(RATE NUMERIC(15,2),PURITY NUMERIC(15,3),METALID VARCHAR(3))"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                For cnt As Integer = 0 To gridView.Rows.Count - 1
                    With gridView.Rows(cnt)
                        strSql = " INSERT INTO TEMPRATEMASTUPDATE(RATE,PURITY,METALID)"
                        strSql += " VALUES(" & .Cells("RATE").Value.ToString() & "," & .Cells("PURITY").Value.ToString() & ",'" & .Cells("METALID").Value.ToString() & "')"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()
                    End With
                Next

                btnUpdate.Enabled = False
                tran = cn.BeginTransaction

                If chkCmbCostCentre.Enabled = True Then
                    Dim strCost As String
                    strCost = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE "
                    If chkCmbCostCentre.Text <> "ALL" Then
                        strCost += " WHERE ISNULL(COSTNAME,'') IN (" + GetQryString(chkCmbCostCentre.Text, ",") + ")"
                    End If
                    Dim dtCostId As New DataTable
                    cmd = New OleDbCommand(strCost, cn, tran)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtCostId)




                    Dim ToCostId As String = ""
                    For cnt As Integer = 0 To dtCostId.Rows.Count - 1
                        ToCostId = dtCostId.Rows(cnt).Item("COSTID").ToString
                        strSql = vbCrLf + " select sno,CASE WHEN ISNULL(T.ITEMTYPEID,0) <> 0 THEN"
                        strSql += vbCrLf + " 		(SELECT TOP 1 RATE FROM TEMPRATEMASTUPDATE AS R INNER JOIN " & cnAdminDb & "..PURITYMAST AS P"
                        strSql += vbCrLf + " 		ON R.METALID = P.METALID AND R.PURITY = P.PURITY"
                        strSql += vbCrLf + " 		WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID) "
                        strSql += vbCrLf + " 		)"
                        strSql += vbCrLf + "         ELSE"
                        strSql += vbCrLf + " 		(SELECT TOP 1 RATE FROM TEMPRATEMASTUPDATE AS R INNER JOIN " & cnAdminDb & "..PURITYMAST AS P"
                        strSql += vbCrLf + " 		ON R.METALID = P.METALID AND R.PURITY = P.PURITY"
                        strSql += vbCrLf + " 		WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY AS C INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM"
                        strSql += vbCrLf + " 		ON C.CATCODE = IM.CATCODE WHERE IM.ITEMID = T.ITEMID) "
                        strSql += vbCrLf + " 		)"
                        strSql += vbCrLf + " 	END rate"
                        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
                        strSql += vbCrLf + " WHERE ISNULL(SALEMODE,'') = 'W'"
                        strSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & ToCostId & "'"
                        If chkCmbItem.Text <> "ALL" Then
                            strSql += vbCrLf + " AND ISNULL(ITEMID,'') IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
                        End If
                        If chkCmbSubItem.Text <> "ALL" Then
                            strSql += vbCrLf + " AND ISNULL(SUBITEMID,'') IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & GetQryString(chkCmbSubItem.Text) & "))"
                        End If
                        Dim dtupd As New DataTable
                        cmd = New OleDbCommand(strSql, cn, tran)
                        da = New OleDbDataAdapter(cmd)
                        da.Fill(dtupd)
                        For ii As Integer = 0 To dtupd.Rows.Count - 1
                            strSql = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET RATE = " & Val(dtupd.Rows(ii).Item(1).ToString)
                            strSql += vbCrLf + " WHERE SNO ='" & dtupd.Rows(ii).Item(0).ToString & "'"
                            ExecQuery(SyncMode.Stock, strSql, cn, tran, ToCostId)
                            If cnCostId <> ToCostId And ToCostId <> cnHOCostId Then
                                ExecQuery(SyncMode.Stock, strSql, cn, tran, cnHOCostId)
                            End If
                        Next
                        ' (WEIGHT + WASTAGE) * RATE  
                        ' SALMODE = 'W'
                        strSql = "UPDATE " & cnAdminDb & "..ITEMTAG SET SALVALUE = ((ISNULL(CASE WHEN GRSNET = 'G' THEN GRSWT ELSE NETWT END,0) + ISNULL(MAXWAST,0)) * ISNULL(RATE,0)) "
                        strSql += vbCrLf + " + ISNULL((SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE COMPANYID = T.COMPANYID AND COSTID = T.COSTID AND TAGNO = T.TAGNO),0)"
                        strSql += vbCrLf + " + ISNULL((SELECT SUM(AMOUNT) FROM " & cnAdminDb & "..ITEMTAGMISCCHAR WHERE COMPANYID = T.COMPANYID AND COSTID = T.COSTID AND TAGNO = T.TAGNO),0)"
                        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T WHERE ISNULL(SALEMODE,'') = 'W'"
                        strSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & ToCostId & "'"
                        If chkCmbItem.Text <> "ALL" Then
                            strSql += vbCrLf + " AND ISNULL(ITEMID,'') IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
                        End If
                        If chkCmbSubItem.Text <> "ALL" Then
                            strSql += vbCrLf + " AND ISNULL(SUBITEMID,'') IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & GetQryString(chkCmbSubItem.Text) & "))"
                        End If
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, ToCostId)
                        If cnCostId <> ToCostId And ToCostId <> cnHOCostId Then
                            ExecQuery(SyncMode.Stock, strSql, cn, tran, cnHOCostId)
                        End If
                    Next
                Else
                    strSql = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET RATE = "
                    strSql += vbCrLf + " 	CASE WHEN ISNULL(T.ITEMTYPEID,0) <> 0 THEN"
                    strSql += vbCrLf + " 		(SELECT TOP 1 RATE FROM TEMPRATEMASTUPDATE AS R INNER JOIN " & cnAdminDb & "..PURITYMAST AS P"
                    strSql += vbCrLf + " 		ON R.METALID = P.METALID AND R.PURITY = P.PURITY"
                    strSql += vbCrLf + " 		WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID) "
                    'strSql += vbCrLf + " 		AND R.UPDATED = '" + dtpDate.Value.Date.ToString("yyyy-MM-dd") + "'
                    strSql += vbCrLf + " 		)"
                    strSql += vbCrLf + "         ELSE"
                    strSql += vbCrLf + " 		(SELECT TOP 1 RATE FROM TEMPRATEMASTUPDATE AS R INNER JOIN " & cnAdminDb & "..PURITYMAST AS P"
                    strSql += vbCrLf + " 		ON R.METALID = P.METALID AND R.PURITY = P.PURITY"
                    strSql += vbCrLf + " 		WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY AS C INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM"
                    strSql += vbCrLf + " 		ON C.CATCODE = IM.CATCODE WHERE IM.ITEMID = T.ITEMID) "
                    'strSql += vbCrLf + " 		AND R.UPDATED = '" + dtpDate.Value.Date.ToString("yyyy-MM-dd") + "' ORDER BY RATEGROUP DESC)"
                    strSql += vbCrLf + " 		)"
                    strSql += vbCrLf + " 	END"
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
                    strSql += vbCrLf + " WHERE ISNULL(SALEMODE,'') = 'W'"
                    If chkCmbItem.Text <> "ALL" Then
                        strSql += vbCrLf + " AND ISNULL(ITEMID,'') IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
                    End If
                    If chkCmbSubItem.Text <> "ALL" Then
                        strSql += vbCrLf + " AND ISNULL(SUBITEMID,'') IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & GetQryString(chkCmbSubItem.Text) & "))"
                    End If
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()

                    ' (WEIGHT + WASTAGE) * RATE  
                    ' SALMODE = 'W'
                    strSql = "UPDATE " & cnAdminDb & "..ITEMTAG SET SALVALUE = ((ISNULL(CASE WHEN GRSNET = 'G' THEN GRSWT ELSE NETWT END,0) + ISNULL(MAXWAST,0)) * ISNULL(RATE,0)) "
                    strSql += vbCrLf + " + ISNULL((SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE COMPANYID = T.COMPANYID AND COSTID = T.COSTID AND TAGNO = T.TAGNO),0)"
                    strSql += vbCrLf + " + ISNULL((SELECT SUM(AMOUNT) FROM " & cnAdminDb & "..ITEMTAGMISCCHAR WHERE COMPANYID = T.COMPANYID AND COSTID = T.COSTID AND TAGNO = T.TAGNO),0)"
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T WHERE ISNULL(SALEMODE,'') = 'W'"
                    If chkCmbItem.Text <> "ALL" Then
                        strSql += vbCrLf + " AND ISNULL(ITEMID,'') IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
                    End If
                    If chkCmbSubItem.Text <> "ALL" Then
                        strSql += vbCrLf + " AND ISNULL(SUBITEMID,'') IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & GetQryString(chkCmbSubItem.Text) & "))"
                    End If
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If

                tran.Commit()
                tran.Dispose()
                MsgBox("Successfully Updated...")
                btnNew_Click(Me, New EventArgs())
            End If
        Catch ex As Exception
            If tran IsNot Nothing Then
                tran.Rollback()
                tran.Dispose()
            End If
            MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        Finally
            btnUpdate.Enabled = True
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpDate.Value = DateTime.Today.Date
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
        If dtSubItem IsNot Nothing Then
            BrighttechPack.GlobalMethods.FillCombo(chkCmbSubItem, dtSubItem, "SUBITEMNAME", , "ALL")
        End If
        btnUpdate.Enabled = True
        chkCmbCostCentre.Focus()
    End Sub

    Function funcFillGrid() As Integer
        gridView.DataSource = Nothing
        Dim dt As New DataTable
        dt.Clear()

        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPRATE')>0"
        strsql += vbcrlf + "      DROP TABLE TEMPRATE"
        strsql += vbcrlf + "  SELECT METALID"
        strsql += vbcrlf + "  ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = R.METALID)AS METALNAME"
        strsql += vbcrlf + "  ,(SELECT TOP 1 PURITYNAME FROM " & cnAdminDb & "..PURITYMAST WHERE METALID = R.METALID AND PURITY = R.PURITY AND PURITYID NOT IN (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATGROUP = 'L'))AS PURITYNAME"
        strsql += vbcrlf + "  ,PURITY"
        strsql += vbcrlf + "  ,SRATE AS RATE"
        strsql += vbcrlf + "  INTO TEMPRATE"
        strsql += vbcrlf + "  FROM " & cnAdminDb & "..RATEMAST AS R"
        strsql += vbcrlf + "  WHERE RATEGROUP IN (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = R.RDATE)"
        strsql += vbcrlf + "  AND RDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
        strsql += vbcrlf + "  ORDER BY METALID,PURITY DESC"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT METALID"
        strSql += vbCrLf + "  ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = P.METALID)AS METALNAME"
        strSql += vbCrLf + "  ,(SELECT TOP 1 PURITYNAME FROM " & cnAdminDb & "..PURITYMAST WHERE METALID = P.METALID AND PURITY = P.PURITY  AND PURITYID NOT IN (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATGROUP = 'L'))AS PURITYNAME"
        strSql += vbCrLf + "  ,PURITY"
        strSql += vbCrLf + "  ,ISNULL((SELECT TOP 1 ISNULL(RATE,0) FROM TEMPRATE WHERE METALID = P.METALID AND PURITY = P.PURITY),0) AS RATE"
        strSql += vbCrLf + "  FROM"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "  SELECT DISTINCT METALID"
        strSql += vbCrLf + "  ,PURITY FROM "
        strSql += vbCrLf + "  " & cnAdminDb & "..PURITYMAST"
        'strsql += vbcrlf + "  WHERE METALID NOT IN ('T')"
        strSql += vbCrLf + "  )P"
        strSql += vbCrLf + "  ORDER BY METALID,PURITY DESC"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)

        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPRATE')>0"
        strSql += vbCrLf + "      DROP TABLE TEMPRATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        For cnt As Integer = 1 To dt.Rows.Count - 1
            If dt.Rows(cnt - 1).Item("METALID").ToString = dt.Rows(cnt).Item("METALID").ToString Then
                dt.Rows(cnt).Item("METALNAME") = ""
            End If
        Next
        gridView.DataSource = dt

        With gridView.Columns("MetalId")
            .ReadOnly = True
            .Visible = False
            .HeaderText = "METAL"
            .Resizable = DataGridViewTriState.False
        End With
        With gridView.Columns("MetalName")
            .ReadOnly = True
            .HeaderText = "METAL"
            .Resizable = DataGridViewTriState.False
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
        With gridView.Columns("PurityName")
            .ReadOnly = True
            .Width = 200
            .MinimumWidth = 265
            .HeaderText = "PURITY NAME"
            .Resizable = DataGridViewTriState.False
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
        With gridView.Columns("Purity")
            .ReadOnly = True
            .MinimumWidth = 60
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Width = 60
            .HeaderText = "PURITY"
            .Resizable = DataGridViewTriState.False
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
        With gridView.Columns("Rate")
            .ReadOnly = False
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .DefaultCellStyle.Format = "0.00"
            .MinimumWidth = 60
            .Width = 60
            .HeaderText = "RATE"
            .Resizable = DataGridViewTriState.False
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
        'gridView.SelectionMode = DataGridViewSelectionMode.CellSelect
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmTagRateUpdate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gridView.BorderStyle = BorderStyle.None
        gridView.SelectionMode = DataGridViewSelectionMode.CellSelect
        gridView.BackgroundColor = Color.WhiteSmoke
        gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        gridView.ColumnHeadersHeight = 25
        gridView.RowTemplate.Height = 25
        gridView.RowTemplate.Resizable = DataGridViewTriState.False
        gridView.DefaultCellStyle.SelectionBackColor = Color.White
        gridView.DefaultCellStyle.SelectionForeColor = Color.Black
        dtpDate.Value = GetEntryDate(GetServerDate)
        funcFillGrid()
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub frmTagRateUpdate_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If gridView.Focused = True Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub NToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs())
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs())
    End Sub

    Private Sub chkCmbItem_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbItem.Leave
        ''SUB-ITEM
        strSql = " SELECT 'ALL' SUBITEMNAME,'ALL' SUBITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT SUBITEMNAME,CONVERT(VARCHAR,SUBITEMID),2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST"
        If chkCmbItem.Text <> "ALL" Then
            strSql += " WHERE ISNULL(ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') IN (" + GetQryString(chkCmbItem.Text, ",") + "))"
        End If
        strSql += ""
        strSql += " ORDER BY RESULT,SUBITEMNAME"
        dtSubItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSubItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbSubItem, dtSubItem, "SUBITEMNAME", , "ALL")
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        If e.RowIndex >= 0 Then
            If e.RowIndex < gridView.RowCount - 1 Then
                If e.ColumnIndex = 4 _
                    And e.RowIndex > 0 Then
                    If gridView.Item("MetalId", e.RowIndex).Value.ToString = gridView.Item("MetalId", e.RowIndex - 1).Value.ToString _
                    And Val(gridView.Item("Rate", e.RowIndex - 1).Value.ToString) > 0 _
                    And Val(gridView.Item("Rate", e.RowIndex).Value.ToString) > Val(gridView.Item("Rate", e.RowIndex - 1).Value.ToString) Then
                        gridView.Item(e.ColumnIndex, e.RowIndex).Value = ""
                        Exit Sub
                    End If
                End If
            End If
        End If
        funcGenAutoRate(e.RowIndex)
    End Sub

    Function funcGenAutoRate(ByVal curRow As Integer) As Integer
        If Not curRow <> gridView.Rows.Count - 1 Then
            Exit Function
        End If
        If Val(gridView.Item("Purity", curRow).Value.ToString) = 100 Then
            For cnt As Integer = curRow + 1 To gridView.Rows.Count - 1
                If gridView.Item("MetalId", cnt).Value.ToString = gridView.Item("MetalId", curRow).Value.ToString Then
                    gridView.Item("Rate", cnt).Value = (Val(gridView.Item("Rate", curRow).Value.ToString) * Val(gridView.Item("Purity", cnt).Value.ToString)) / 100
                End If
            Next
        End If
    End Function

    Private Sub gridView_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles gridView.DataError
        MsgBox("Incorrect Rate", MsgBoxStyle.Information)
    End Sub

    Private Sub gridView_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView.EditingControlShowing
        If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("RATE").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            '---add an event handler to the TextBox control---
            AddHandler tb.KeyPress, AddressOf TextBox_KeyPress
        End If
    End Sub

    Private Sub TextBox_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = "." And CType(sender, TextBox).Text.Contains(".") Then
            e.Handled = True
            Return
            Exit Sub
        End If
        Select Case e.KeyChar
            Case "+", "-", "0" To "9", ChrW(Keys.Back), ".", _
            ChrW(Keys.Enter), ChrW(Keys.Escape)
            Case Else
                e.Handled = True
                MsgBox("Digits only Allowed 0 to 9", MsgBoxStyle.Information)
                CType(sender, TextBox).Focus()
                Return
        End Select
        If CType(sender, TextBox).Text.Contains(".") Then
            Dim dotPos As Integer = InStr(CType(sender, TextBox).Text, ".", CompareMethod.Text)
            Dim sp() As String = CType(sender, TextBox).Text.Split(".")
            Dim curPos As Integer = CType(sender, TextBox).SelectionStart
            If sp.Length >= 2 Then
                If curPos >= dotPos Then
                    If sp(1).Length > 1 Then
                        e.Handled = True
                        Return
                    End If
                End If
            End If
        End If

        ''---if textbox is empty and user pressed a decimal char---
        'If CType(sender, TextBox).Text = String.Empty And e.KeyChar = Chr(46) Then
        '    e.Handled = True
        '    Return
        'End If
        ''---if textbox already has a decimal point---
        'If CType(sender, TextBox).Text.Contains(Chr(46)) And e.KeyChar = Chr(46) Then
        '    e.Handled = True
        '    Return
        'End If
        ''---if the key pressed is not a valid decimal number---
        'If (Not (Char.IsDigit(e.KeyChar) Or Char.IsControl(e.KeyChar) Or (e.KeyChar = Chr(46)))) Then
        '    e.Handled = True
        'End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        gridView.DefaultCellStyle.SelectionBackColor = Color.LightGoldenrodYellow
        gridView.DefaultCellStyle.SelectionForeColor = Color.Black
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(4)
            End If
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Me.SelectNextControl(gridView, True, True, True, True)
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        gridView.DefaultCellStyle.SelectionBackColor = Color.White
        gridView.DefaultCellStyle.SelectionForeColor = Color.Black
    End Sub

    Private Sub gridView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.SelectionChanged
        Try
            If Not gridView.RowCount > 0 Then Exit Sub
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(4)
        Catch ex As Exception

        End Try
    End Sub
End Class