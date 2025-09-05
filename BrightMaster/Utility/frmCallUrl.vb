
Imports System.Data.OleDb
Public Class frmCallUrl
    Dim strsql As String
    Dim cmd As OleDbCommand
    Dim dtBatchNo As New DataTable
    Dim View As Char

    Private Sub frmCallUrl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmCallUrl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        New_Load()
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
    End Sub
    Private Sub New_Load()
        chkASD.Checked = False
        dtpTo.Visible = Not chkASD.Checked
        dtpTo.Enabled = Not chkASD.Checked
        Label1.Visible = Not chkASD.Checked
        btnPost.Enabled = False
        chkASD.Focus()
        grdView.Columns.Clear()
        grdView.DataSource = Nothing
        dtBatchNo = New DataTable
    End Sub

    Private Sub chkASD_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkASD.CheckedChanged
        dtpTo.Visible = Not chkASD.Checked
        dtpTo.Enabled = Not chkASD.Checked
        Label1.Visible = Not chkASD.Checked
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        New_Load()
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        strsql = Nothing
        strsql += vbCrLf & " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP_RSOFT') = 1"
        strsql += vbCrLf & " 	DROP TABLE TEMPTABLEDB..TEMP_RSOFT"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        strsql = vbCrLf
        strsql += vbCrLf & " SELECT BATCHNO,CONVERT(VARCHAR(10),COSTID)+CONVERT(VARCHAR(10),TRANNO) AS BILLNO,ACCODE,TRANDATE AS BILLDATE"
        'strsql += vbCrLf & "  ,(SELECT TOP 1 CASHNAME  FROM " & cnAdminDb & "..CASHCOUNTER  WHERE CASHID = I.CASHID )  AS CASHCOUNTER"
        strsql += vbCrLf & "  , I.CASHID AS CASHCOUNTER"
        strsql += vbCrLf & "  ,(SELECT TOP 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID)  AS USERNAME"
        strsql += vbCrLf & "  ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE = 'AA') AS ADVANCE"
        strsql += vbCrLf & "  ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE = 'DU') AS DUE"
        strsql += vbCrLf & "  ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE = 'GV') AS GIFTVOUCHER"
        strsql += vbCrLf & "  ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE = 'SS') AS SCHEME "
        strsql += vbCrLf & "  ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE = 'SH') AS CHEQUE "
        strsql += vbCrLf & "  ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE = 'CC') AS CREDITCARD "
        strsql += vbCrLf & "  ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE = 'HC') AS HANDCHARG "
        strsql += vbCrLf & "  ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE = 'DI') AS DISCOUNT "
        strsql += vbCrLf & "  ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE = 'CA') AS CASH "
        strsql += vbCrLf & "  ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE = 'PU') AS [PURCHASE]"
        strsql += vbCrLf & "  ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE = 'SR') AS [RETURN]"
        strsql += vbCrLf & "  ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE = 'SV') AS [TAX]"
        strsql += vbCrLf & "  ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE IN('SA','SV')) AS [RECEIVE]"
        strsql += vbCrLf & "  ,'D' COLHEAD,1 RESULT"
        strsql += vbCrLf & "  INTO TEMPTABLEDB..TEMP_RSOFT FROM " & cnStockDb & "..ISSUE AS I "
        strsql += vbCrLf & "  WHERE TRANTYPE IN ('SA','OD') AND ISNULL(CANCEL,'') <> 'Y' "
        If chkASD.Checked Then
            strsql += vbCrLf & "  AND TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        Else
            strsql += vbCrLf & "  AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        End If
        strsql += vbCrLf & "  GROUP BY BATCHNO,TRANNO,ACCODE,TRANDATE ,CASHID,USERID,COSTID "
        strsql += vbCrLf & "  ORDER BY TRANDATE,TRANNO  "
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        strsql = vbCrLf
        strsql += vbCrLf & "   INSERT INTO TEMPTABLEDB..TEMP_RSOFT (ACCODE,RECEIVE,ADVANCE,DUE,GIFTVOUCHER,SCHEME,CHEQUE,CREDITCARD,HANDCHARG,DISCOUNT,CASH,PURCHASE,[RETURN],TAX,COLHEAD, RESULT)"
        strsql += vbCrLf & "   SELECT 'TOTAL',SUM(RECEIVE),SUM(ADVANCE),SUM(DUE),SUM(GIFTVOUCHER),SUM(SCHEME),SUM(CHEQUE),SUM(CREDITCARD),SUM(HANDCHARG),SUM(DISCOUNT),SUM(CASH),SUM(PURCHASE),SUM([RETURN]),SUM(TAX) "
        strsql += vbCrLf & "   ,'G'COLHEAD,99 RESULT FROM TEMPTABLEDB..TEMP_RSOFT "
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        strsql = vbCrLf
        strsql += vbCrLf & "   SELECT * FROM TEMPTABLEDB..TEMP_RSOFT ORDER BY RESULT,BILLDATE,BILLNO"

        da = New OleDbDataAdapter(strsql, cn)
        dtBatchNo = New DataTable
        da.Fill(dtBatchNo)

        grdView.DataSource = Nothing
        grdView.Columns.Clear()
        Dim chk As New DataGridViewCheckBoxColumn()
        grdView.Columns.Add(chk)
        chk.HeaderText = "Chk"
        chk.ReadOnly = False
        chk.Name = "chk"
        grdView.DataSource = dtBatchNo

        btnPost.Enabled = True
        AutiSize_()
    End Sub

    Private Sub btnPost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPost.Click
        Dim dtSales As New DataTable
        If Not dtBatchNo.Rows.Count > 0 Then
            MsgBox("View First")
            Exit Sub
        End If
        Try
            Dim Count As Integer = 0
            For i As Integer = 0 To dtBatchNo.Rows.Count - 1
                If Not grdView.Rows(i).Cells("chk").Value Or grdView.Rows(i).Cells("COLHEAD").Value.ToString = "G" Then Continue For
                strsql = "SELECT TAGNO,GRSWT,WASTAGE,STNAMT AS STONEAMT,AMOUNT AS GROSSAMT"
                strsql += " ,PCS,MCHARGE AS MC,RATE,NETWT,ISNULL(AMOUNT,0)+ISNULL(TAX,0) AS AMOUNT,TAX AS GST,EMPID AS EMPCODE"
                strsql += " FROM " & cnStockDb & "..ISSUE WHERE BATCHNO='" & grdView.Rows(i).Cells("BATCHNO").Value.ToString & "'"
                cmd = New OleDbCommand(strsql, cn)
                da = New OleDbDataAdapter(cmd)
                dtSales = New DataTable
                da.Fill(dtSales)
                If dtSales.Rows.Count > 0 Then
                    Dim url As String = "http://www.rsoft.in/srisaravanajewellers/CreateSaleDetailSaveAPI.php?"
                    url += " billno=" + grdView.Rows(i).Cells("billno").Value.ToString
                    url += "&accode=" + grdView.Rows(i).Cells("accode").Value.ToString
                    url += "&billdate=" + grdView.Rows(i).Cells("billdate").Value.ToString
                    url += "&cashcounter=" + grdView.Rows(i).Cells("cashcounter").Value.ToString
                    url += "&username=" + grdView.Rows(i).Cells("username").Value.ToString
                    url += "&receive=" + grdView.Rows(i).Cells("receive").Value.ToString
                    url += "&advance=" + grdView.Rows(i).Cells("advance").Value.ToString
                    url += "&due=" + grdView.Rows(i).Cells("due").Value.ToString
                    url += "&giftvoucher=" + grdView.Rows(i).Cells("giftvoucher").Value.ToString
                    url += "&scheme=" + grdView.Rows(i).Cells("scheme").Value.ToString
                    url += "&cheque=" + grdView.Rows(i).Cells("cheque").Value.ToString
                    url += "&creditcard=" + grdView.Rows(i).Cells("creditcard").Value.ToString
                    url += "&handcharg=" + grdView.Rows(i).Cells("handcharg").Value.ToString
                    url += "&discount=" + grdView.Rows(i).Cells("discount").Value.ToString
                    url += "&cash=" + grdView.Rows(i).Cells("cash").Value.ToString
                    url += "&item=" + GetJson(dtSales).ToString.ToLower
                    strsql = "EXEC " & cnAdminDb & "..PROC_JSON @URL='" & url & "'"
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                Count += 1
            Next
            MsgBox("Successfully " + Count.ToString + " Items Updated")
            New_Load()
        Catch ex As Exception
            MsgBox(ex.ToString + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Function GetJson(ByVal dt As DataTable) As String
        Return Newtonsoft.Json.JsonConvert.SerializeObject(dt)
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        'Dim rows As New List(Of Dictionary(Of String, Object))()
        'Dim row As Dictionary(Of String, Object) = Nothing
        'For Each dr As DataRow In dt.Rows
        '    row = New Dictionary(Of String, Object)()
        '    For Each dc As DataColumn In dt.Columns
        '        row.Add(dc.ColumnName.Trim(), dr(dc))
        '    Next
        '    rows.Add(row)
        'Next
        'Return serializer.Serialize(rows)
    End Function

    Private Sub grdView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles grdView.KeyDown
        If e.KeyCode = Keys.D Then
            Try
                If View = "D" Then Exit Sub
                strsql = "SELECT TAGNO,GRSWT,WASTAGE,STNAMT AS STONEAMT,AMOUNT AS GROSSAMT"
                strsql += " ,PCS,MCHARGE AS MC,RATE,NETWT,ISNULL(AMOUNT,0)+ISNULL(TAX,0) AS AMOUNT,TAX AS GST,EMPID AS EMPCODE"
                strsql += " FROM " & cnStockDb & "..ISSUE WHERE BATCHNO='" & grdView.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                da = New OleDbDataAdapter(strsql, cn)
                Dim dtSales As DataTable
                dtSales = New DataTable
                da.Fill(dtSales)
                grdView.DataSource = Nothing
                grdView.DataSource = dtSales
                AutiSize_()
                If grdView.Columns.Contains("CHK") Then
                    grdView.Columns("CHK").Visible = False
                End If
                View = "D"
            Catch ex As Exception
                ex = Nothing
            End Try
        ElseIf e.KeyCode = Keys.Escape Then
            If View = "E" Then Exit Sub
            grdView.DataSource = Nothing
            grdView.DataSource = dtBatchNo
            AutiSize_()
            If grdView.Columns.Contains("CHK") Then
                grdView.Columns("CHK").Visible = True
            End If
            View = "E"
        End If
    End Sub
    Private Sub AutiSize_()
        With grdView
            For i As Integer = 1 To .ColumnCount - 1
                .Columns(i).ReadOnly = False
                If .Columns(i).ValueType.Name Is GetType(Decimal).Name Then
                    .Columns(i).DefaultCellStyle.Format = "0.00"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Double).Name Then
                    .Columns(i).DefaultCellStyle.Format = "0.00"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Integer).Name Then
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Date).Name Then
                    .Columns(i).DefaultCellStyle.Format = "dd/MM/yyyy"
                End If
                .Columns(i).ReadOnly = True
            Next
        End With

        If grdView.Columns.Contains("BATCHNO") Then
            grdView.Columns("BATCHNO").Visible = False
        End If
        If grdView.Columns.Contains("COLHEAD") Then
            grdView.Columns("COLHEAD").Visible = False
        End If
        If grdView.Columns.Contains("RESULT") Then
            grdView.Columns("RESULT").Visible = False
        End If
        If grdView.Columns.Contains("GIFTVOUCHER") Then
            grdView.Columns("GIFTVOUCHER").HeaderText = "G_VOUCH"
        End If
        If grdView.Columns.Contains("HANDCHARG") Then
            grdView.Columns("HANDCHARG").HeaderText = "H_CHARG"
        End If
        If grdView.Columns.Contains("RECEIVE") Then
            grdView.Columns("RECEIVE").HeaderText = "TOTAL"
        End If
        If grdView.Columns.Contains("CASHCOUNTER") Then
            grdView.Columns("CASHCOUNTER").HeaderText = "COUNTER"
        End If

        For i As Integer = 0 To grdView.Rows.Count - 1
            If grdView.Columns.Contains("COLHEAD") Then
                If grdView.Rows(i).Cells("COLHEAD").Value.ToString = "G" Then
                    grdView.Rows(i).DefaultCellStyle.BackColor = Color.Green
                    grdView.Rows(i).DefaultCellStyle.ForeColor = Color.SeaShell
                    grdView.Rows(i).DefaultCellStyle.Font = New Font("Verdana", 8, FontStyle.Bold)
                Else
                    grdView.Rows(i).DefaultCellStyle.Font = New Font("Verdana", 8, FontStyle.Regular)
                End If
            End If
        Next

        grdView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        For Each dgvCol As DataGridViewColumn In grdView.Columns
            dgvCol.Width = dgvCol.Width
        Next
        grdView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        
        
    End Sub

    Private Sub frmCallUrl_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        chkASD.Focus()
    End Sub

    Private Sub chkAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAll.CheckedChanged
        For i As Integer = 0 To grdView.Rows.Count - 1
            grdView.Rows(i).Cells("chk").Value = chkAll.Checked
        Next
    End Sub
End Class
