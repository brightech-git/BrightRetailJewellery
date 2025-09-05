Imports System.Data.OleDb
Public Class frmSyncReceivedStatus
    Dim strSql As String = Nothing
    Dim cmd As OleDbCommand
    Dim da As New OleDbDataAdapter
    Dim dt As New DataTable
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub frmSyncReceivedStatus_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSyncReceivedStatus_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strSql = "SELECT COUNT(*)CNT FROM " & cnStockDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME='DAILYREPORT'"
        If objGPack.GetSqlValue(strSql, "CNT", 0) = 0 Then MsgBox("Not Available ", MsgBoxStyle.Information) : Me.BeginInvoke(New MethodInvoker(AddressOf CloseIt)) : Exit Sub
        dtpTo.Value = GetServerDate()
        dtpFrom.Value = dtpTo.Value.AddDays(-7)
        Me.BeginInvoke(New MethodInvoker(AddressOf funcViewSyncRecStatus))
        gridView_OWN.Focus()
    End Sub
    Private Sub btnView_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        gridView_OWN.DataSource = Nothing
        gridView_OWN.Refresh()
        funcViewSyncRecStatus()
    End Sub
    Private Sub funcViewSyncRecStatus()
        Dim SysId As String = Mid(Guid.NewGuid.ToString, 1, 5)
        strSql = vbCrLf + "DECLARE @COSTNAME VARCHAR(50)"
        strSql += vbCrLf + "DECLARE @DISP INT"
        strSql += vbCrLf + "DECLARE @QRY VARCHAR(4000)"
        strSql += vbCrLf + "DECLARE CUR CURSOR FOR"
        strSql += vbCrLf + "SELECT DISTINCT C.COSTID,DISPORDER FROM " & cnAdminDb & "..COSTCENTRE  C"
        strSql += vbCrLf + "JOIN " & cnAdminDb & "..SYNCCOSTCENTRE S ON S.COSTID=C.COSTID "
        strSql += vbCrLf + "WHERE ISNULL(S.ACTIVE,'N')<>'N' "
        strSql += vbCrLf + "AND MAIN<>'Y'ORDER BY DISPORDER"
        strSql += vbCrLf + "OPEN CUR"
        strSql += vbCrLf + "FETCH NEXT FROM CUR INTO @COSTNAME,@DISP"
        strSql += vbCrLf + "SELECT @QRY='IF OBJECT_ID(''TEMPTABLEDB..TEMPDSR" & SysId & "'',''U'') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPDSR" & SysId & "'"
        strSql += vbCrLf + "EXEC(@QRY)"
        strSql += vbCrLf + "SELECT @QRY='CREATE TABLE TEMPTABLEDB..TEMPDSR" & SysId & "(SNO INT IDENTITY(1,1),TRANDATE VARCHAR(20),DAYNAME VARCHAR(20)'"
        strSql += vbCrLf + "WHILE @@FETCH_STATUS<>-1"
        strSql += vbCrLf + "BEGIN"
        strSql += vbCrLf + "	SELECT @QRY=@QRY+CHAR(13)+ ',['+ REPLACE(@COSTNAME,' ','') + '] VARCHAR(20) DEFAULT ''X'''"
        strSql += vbCrLf + "	FETCH NEXT FROM CUR INTO @COSTNAME,@DISP"
        strSql += vbCrLf + "END"
        strSql += vbCrLf + "SELECT @QRY=@QRY+CHAR(13)+')'"
        strSql += vbCrLf + "PRINT(@QRY)"
        strSql += vbCrLf + "EXEC(@QRY)"
        strSql += vbCrLf + "CLOSE CUR"
        strSql += vbCrLf + "DEALLOCATE CUR"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + "SELECT DISTINCT C.COSTID FROM " & cnAdminDb & "..COSTCENTRE  C"
        strSql += vbCrLf + "JOIN " & cnAdminDb & "..SYNCCOSTCENTRE S ON S.COSTID=C.COSTID "
        strSql += vbCrLf + "WHERE ISNULL(S.ACTIVE,'N')<>'N' "
        strSql += vbCrLf + "AND MAIN<>'Y'"
        Dim dtCost As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCost)
        Dim StartDate As Date = dtpTo.Value
        Dim EndDate As Date = dtpFrom.Value
        Dim Diff As Integer = DateDiff(DateInterval.Day, EndDate, StartDate)
        For i As Integer = 0 To Diff
            strSql = "INSERT INTO TEMPTABLEDB..TEMPDSR" & SysId & "(TRANDATE,DAYNAME)VALUES("
            strSql += vbCrLf + "'" & Format(StartDate, "dd-MM-yyyy") & "'"
            strSql += vbCrLf + ",'" & Format(StartDate, "dddd") & "'"
            strSql += vbCrLf + ")"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            StartDate = StartDate.AddDays(-1)
        Next
        strSql = vbCrLf + "SELECT DISTINCT COSTID,CONVERT(VARCHAR(20),TRANDATE,110)TRANDATE FROM " & cnStockDb & "..DAILYREPORT  "
        strSql += vbCrLf + "WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
        strSql += vbCrLf + "ORDER BY TRANDATE DESC ,COSTID "
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            StartDate = dtpTo.Value
            EndDate = dtpFrom.Value
            Diff = DateDiff(DateInterval.Day, EndDate, StartDate)

            For i As Integer = 0 To Diff
                For j As Integer = 0 To dtCost.Rows.Count - 1
                    Dim Ro() As DataRow
                    Dim fltr As String = "COSTID='" & dtCost.Rows(j).Item("COSTID").ToString & "' AND TRANDATE='" & Format(StartDate, "MM-dd-yyyy") & "'"
                    Ro = dt.Select(fltr)
                    If Ro.Length > 0 Then
                        strSql = "UPDATE TEMPTABLEDB..TEMPDSR" & SysId & " SET " & dtCost.Rows(j).Item("COSTID").ToString & "='Received'"
                        strSql += vbCrLf + " WHERE TRANDATE='" & Format(StartDate, "dd-MM-yyyy") & "'"
                        cmd = New OleDbCommand(strSql, cn)
                        cmd.ExecuteNonQuery()
                    End If
                Next
                StartDate = StartDate.AddDays(-1)
            Next
        End If
        strSql = vbCrLf + "SELECT * FROM TEMPTABLEDB..TEMPDSR" & SysId & " ORDER BY SNO"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With gridView_OWN
                .DataSource = Nothing
                .Refresh()
                .DataSource = dt
                .Columns("SNO").Visible = False
                For Each dgvRow As DataGridViewRow In gridView_OWN.Rows
                    For j As Integer = 0 To dtCost.Rows.Count - 1
                        If dgvRow.Cells(dtCost.Rows(j).Item("COSTID")).Value.ToString = "Received" Then
                            dgvRow.Cells(dtCost.Rows(j).Item("COSTID")).Style.ForeColor = Color.Green
                            dgvRow.Cells(dtCost.Rows(j).Item("COSTID")).Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        Else
                            dgvRow.Cells(dtCost.Rows(j).Item("COSTID")).Style.ForeColor = Color.Red
                            dgvRow.Cells(dtCost.Rows(j).Item("COSTID")).Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        End If
                    Next
                Next
                For i As Integer = 0 To .Columns.Count - 1
                    .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
                    .Columns(i).ReadOnly = True
                Next
                .Refresh()
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .RowTemplate.Height = 25
                If gridView_OWN.RowCount > 0 Then
                    gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    gridView_OWN.Invalidate()
                    For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                End If
            End With
        Else
            MsgBox("No Record Found", MsgBoxStyle.Information)
        End If
    End Sub
    Private Sub CloseIt()
        Me.Close()
    End Sub
End Class