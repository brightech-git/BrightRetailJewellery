Imports System.Data.OleDb
Public Class CounterInwardAuthorize
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtGrid As DataTable
    Private Sub frmCounterChange_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        Me.Refresh()
        strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER')>0 DROP TABLE TEMPCTRANSFER"
        strSql += vbCrLf + "  DECLARE @FRMDATE SMALLDATETIME"
        strSql += vbCrLf + "  DECLARE @TODATE SMALLDATETIME"
        strSql += vbCrLf + "  SELECT @FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "',@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  SELECT C.TAGSNO,C.ITEMID,C.TAGNO,COU.ITEMCTRNAME AS COUNTER,C.RECDATE,C.ISSDATE,C.AU_USERID"
        strSql += vbCrLf + "  ,U.USERNAME"
        strSql += vbCrLf + "  ,C.UPTIME"
        strSql += vbCrLf + "  ,IDENTITY(INT,1,1) AS KEYNO,C.SNO"
        strSql += vbCrLf + "  INTO TEMPCTRANSFER"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..CTRANSFER AS C"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.SNO = C.TAGSNO"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..USERMASTER AS U ON U.USERID = C.USERID"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..ITEMCOUNTER AS COU ON COU.ITEMCTRID = C.ITEMCTRID"
        strSql += vbCrLf + "  WHERE C.RECDATE BETWEEN @FRMDATE AND @TODATE OR C.ISSDATE BETWEEN @FRMDATE AND @TODATE"
        strSql += vbCrLf + "  AND ISNULL(C.USERID,0) <> 0"
        strSql += vbCrLf + "  ORDER BY C.TAGSNO,C.ITEMID,C.TAGNO,C.RECDATE,C.ISSDATE DESC"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "  SELECT I.ITEMNAME AS ITEM,X.TAGNO,T.PCS,T.GRSWT,T.NETWT,X.RECDATE,X.COUNTER AS OLDCOUNTER,X.NEWCOUNTER,X.TRANSUSER,LTRIM(SUBSTRING(CONVERT(vARCHAR,X.UPTIME,100),12,20))TRANSTIME,x.SNO"
        strSql += vbCrLf + "  FROM"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "  SELECT * "
        strSql += vbCrLf + "  ,(SELECT TOP 1 COUNTER FROM TEMPCTRANSFER WHERE TAGSNO = T.TAGSNO AND ITEMID = T.ITEMID AND TAGNO = T.TAGNO"
        strSql += vbCrLf + "   AND KEYNO > T.KEYNO)AS NEWCOUNTER"
        strSql += vbCrLf + "  ,(SELECT TOP 1 username FROM TEMPCTRANSFER WHERE TAGSNO = T.TAGSNO AND ITEMID = T.ITEMID AND TAGNO = T.TAGNO"
        strSql += vbCrLf + "   AND KEYNO > T.KEYNO)AS TRANSUSER"
        strSql += vbCrLf + "  FROM TEMPCTRANSFER AS T"
        strSql += vbCrLf + "  )X"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.SNO = X.TAGSNO"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.ITEMID"
        strSql += vbCrLf + "  WHERE ISNULL(X.NEWCOUNTER,'') <> ''"
        If cmbNewCounter_MAN.Text <> "ALL" And cmbNewCounter_MAN.Text <> "" Then
            strSql += vbCrLf + "  AND X.NEWCOUNTER = '" & cmbNewCounter_MAN.Text & "'"
        End If
        strSql += vbCrLf + "  AND X.AU_USERID IS NULL"

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = chkSelectAll.Checked
        dtGrid.Columns.Add(dtCol)
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        With gridView
            .DataSource = dtGrid
            FormatGridColumns(gridView, False)
            .Columns("CHECK").ReadOnly = False
            .SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            .Columns("CHECK").HeaderText = ""
            .Columns("CHECK").Width = 30
            .Columns("ITEM").Width = 120
            .Columns("TAGNO").Width = 70
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 70
            .Columns("NETWT").Width = 70
            .Columns("RECDATE").Width = 80
            .Columns("RECDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("RECDATE").HeaderText = "TRANSFER DATE"
            .Columns("OLDCOUNTER").Width = 120
            .Columns("NEWCOUNTER").Width = 120
            .Columns("TRANSUSER").Width = 120
            .Columns("TRANSUSER").HeaderText = "TRANSFERED BY"

            .Columns("TRANSTIME").Width = 80
            .Columns("TRANSTIME").HeaderText = "TRANS TIME"
            .Columns("KEYNO").Visible = False
            .Columns("SNO").Visible = False
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
        'dtGridHeader.Rows(0).Item("SUBITEM") = "TAG COUNT :"
        ',PCS,GRSWT,NETWT
        Dim obj As Object
        obj = dtGrid.Compute("SUM(PCS)", "PCS IS NOT NULL")
        dtGridHeader.Rows(0).Item("PCS") = IIf(Val(obj.ToString) <> 0, obj, DBNull.Value)
        If chkSelectAll.Checked Then dtGridHeader.Rows(1).Item("PCS") = IIf(Val(obj.ToString) <> 0, obj, DBNull.Value)
        obj = dtGrid.Compute("SUM(GRSWT)", "GRSWT IS NOT NULL")
        dtGridHeader.Rows(0).Item("GRSWT") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
        If chkSelectAll.Checked Then dtGridHeader.Rows(1).Item("GRSWT") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
        obj = dtGrid.Compute("SUM(NETWT)", "NETWT IS NOT NULL")
        dtGridHeader.Rows(0).Item("NETWT") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
        obj = dtGrid.Compute("COUNT(PCS)", String.Empty)
        dtGridHeader.Rows(0).Item("TAGNO") = IIf(Val(obj.ToString) <> 0, obj, DBNull.Value)
        If chkSelectAll.Checked Then
            dtGridHeader.Rows(1).Item("TAGNO") = IIf(Val(obj.ToString) <> 0, obj, DBNull.Value)
            If Val(obj.ToString) > 0 Then btnAuthorize.Enabled = True
        End If
        gridViewFooter.DataSource = dtGridHeader
        FormatGridColumns(gridViewFooter, False)
        gridViewFooter.DefaultCellStyle.BackColor = Color.Aqua
        gridViewFooter.DefaultCellStyle.SelectionBackColor = Color.Aqua
        gridViewFooter.DefaultCellStyle.SelectionForeColor = Color.Red
        gridViewFooter.DefaultCellStyle.ForeColor = Color.Red
        gridViewFooter.DefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        For cnt As Integer = 0 To gridView.Columns.Count - 1
            gridViewFooter.Columns(cnt).Visible = gridView.Columns(cnt).Visible
            gridViewFooter.Columns(cnt).Width = gridView.Columns(cnt).Width
        Next
        'gridViewFooter.Columns("SUBITEM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        gridView.DataSource = Nothing
        gridViewFooter.DataSource = Nothing
        cmbNewCounter_MAN.Text = "ALL"
        btnAuthorize.Enabled = False
        chkSelectAll.Checked = False
        dtpFrom.Select()
    End Sub

    Private Sub CalcSelectedValues()
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable)
        dt.AcceptChanges()
        Dim pcs As Integer = Val(dt.Compute("COUNT(CHECK)", "CHECK = TRUE").ToString)
        If pcs > 0 Then
            btnAuthorize.Enabled = True
        Else
            btnAuthorize.Enabled = False
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
            Dim cnt As Decimal = Val(gridViewFooter.Rows(1).Cells("TAGNO").Value.ToString)
            If CType(gridView.Rows(e.RowIndex).Cells("CHECK").Value, Boolean) = True Then
                pcs += Val(.Cells("PCS").Value.ToString)
                grsWt += Val(.Cells("GRSWT").Value.ToString)
                netWT += Val(.Cells("NETWT").Value.ToString)
                cnt += 1
            Else
                pcs -= Val(.Cells("PCS").Value.ToString)
                grsWt -= Val(.Cells("GRSWT").Value.ToString)
                netWT -= Val(.Cells("NETWT").Value.ToString)
                cnt -= 1
            End If
            If cnt > 0 Then
                btnAuthorize.Enabled = True
            Else
                btnAuthorize.Enabled = False
            End If
            'gridViewFooter.Rows(1).Cells("SUBITEM").Value = "TAG COUNT :"
            'gridViewFooter.Rows(1).Cells("SUBITEM").Style.Alignment = DataGridViewContentAlignment.MiddleRight
            gridViewFooter.Rows(1).Cells("TAGNO").Value = IIf(cnt <> 0, cnt, DBNull.Value)
            gridViewFooter.Rows(1).Cells("PCS").Value = IIf(pcs <> 0, pcs, DBNull.Value)
            gridViewFooter.Rows(1).Cells("GRSWT").Value = IIf(grsWt <> 0, Format(grsWt, "0.000"), DBNull.Value)
            gridViewFooter.Rows(1).Cells("NETWT").Value = IIf(netWT <> 0, Format(netWT, "0.000"), DBNull.Value)
        End With
    End Sub


    Private Sub gridView_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.CurrentCellDirtyStateChanged
        gridView.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub frmCounterChange_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY DISPLAYORDER,ITEMCTRNAME"
        cmbNewCounter_MAN.Items.Clear()
        cmbNewCounter_MAN.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbNewCounter_MAN, False, False)
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each dgvRow As DataGridViewRow In gridView.Rows
            dgvRow.Cells("CHECK").Value = chkSelectAll.Checked
        Next
    End Sub

    Private Sub btnAuthorize_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAuthorize.Click
        Dim objSecret As New frmAdminPassword()
        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable).Copy
        Dim dv As DataView
        dv = dt.DefaultView
        dv.RowFilter = "CHECK = TRUE"
        Dim costId As String = GetAdmindbSoftValue("SYNC-TO")
        Dim ctrId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbNewCounter_MAN.Text & "'"))
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            For Each ro As DataRow In dv.ToTable.Rows
                strSql = " UPDATE " & cnAdminDb & "..CTRANSFER SET"
                strSql += vbCrLf + " AU_USERID = " & userId & ""
                strSql += vbCrLf + " ,AU_UPDATED = '" & GetEntryDate(GetServerDate(tran), tran) & "'"
                strSql += vbCrLf + " ,AU_UPTIME = '" & GetServerTime(tran) & "'"
                strSql += " WHERE SNO = '" & ro.Item("SNO").ToString & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Authorize Completed..")
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
End Class